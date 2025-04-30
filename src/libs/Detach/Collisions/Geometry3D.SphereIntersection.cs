using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool SphereObb(Sphere sphere, Obb obb, out IntersectionResult result)
	{
		Matrix4x4 obbOrientation = obb.Orientation.ToMatrix4x4();

		// Transform the sphere center into OBB local space
		Matrix4x4 inverseOrientation = Matrix4x4.Transpose(obbOrientation); // Because it's orthonormal
		Vector3 localCenter = Vector3.Transform(sphere.Center - obb.Center, inverseOrientation);

		// Clamp the local point to the box's extents to find the closest point on the box
		Vector3 clamped = Vector3.Clamp(localCenter, -obb.HalfExtents, obb.HalfExtents);

		// Back-transform to world space to get the closest point on the OBB
		Vector3 closestPointWorld = Vector3.Transform(clamped, obbOrientation) + obb.Center;

		// Compute vector from the closest point to sphere center
		Vector3 toCenter = sphere.Center - closestPointWorld;
		float distanceSq = toCenter.LengthSquared();

		if (distanceSq > sphere.Radius * sphere.Radius)
		{
			result = default;
			return false; // No intersection
		}

		// Determine which face was hit most based on which axis had the greatest clamping
		Vector3 delta = localCenter - clamped;
		float absX = Math.Abs(delta.X);
		float absY = Math.Abs(delta.Y);
		float absZ = Math.Abs(delta.Z);

		Vector3 localNormal;

		if (absX > absY && absX > absZ)
			localNormal = new Vector3(delta.X < 0 ? -1 : 1, 0, 0);
		else if (absY > absZ)
			localNormal = new Vector3(0, delta.Y < 0 ? -1 : 1, 0);
		else
			localNormal = new Vector3(0, 0, delta.Z < 0 ? -1 : 1);

		// Transform the normal back to world space
		Vector3 worldNormal = Vector3.TransformNormal(localNormal, obbOrientation);
		worldNormal = Vector3.Normalize(worldNormal);

		result = new IntersectionResult(worldNormal, closestPointWorld);
		return true;
	}

	public static bool SphereCylinder(Sphere sphere, Cylinder cylinder, out IntersectionResult result)
	{
		result = default;

		// Check intersection with bottom circle
		Vector2 sphereXz = new(sphere.Center.X, sphere.Center.Z);
		Vector2 cylinderXz = new(cylinder.BottomCenter.X, cylinder.BottomCenter.Z);
		float distanceSquaredXz = Vector2.DistanceSquared(sphereXz, cylinderXz);
		float radiusSum = sphere.Radius + cylinder.Radius;

		// Check if sphere is within the cylinder's height range
		float sphereMinY = sphere.Center.Y - sphere.Radius;
		float sphereMaxY = sphere.Center.Y + sphere.Radius;
		float cylinderMinY = cylinder.BottomCenter.Y;
		float cylinderMaxY = cylinder.BottomCenter.Y + cylinder.Height;

		// Check bottom circle
		if (sphereMinY <= cylinderMinY && sphereMaxY >= cylinderMinY && distanceSquaredXz <= radiusSum * radiusSum)
		{
			result = new IntersectionResult(-Vector3.UnitY, sphere.Center + Vector3.UnitY * (sphere.Radius - (cylinderMinY - sphereMinY)));
			return true;
		}

		// Check top circle
		if (sphereMaxY >= cylinderMaxY && sphereMinY <= cylinderMaxY && distanceSquaredXz <= radiusSum * radiusSum)
		{
			result = new IntersectionResult(Vector3.UnitY, sphere.Center - Vector3.UnitY * (sphere.Radius - (sphereMaxY - cylinderMaxY)));
			return true;
		}

		// Check side surface
		if (sphereMaxY >= cylinderMinY && sphereMinY <= cylinderMaxY)
		{
			float distanceXz = MathF.Sqrt(distanceSquaredXz);
			if (distanceXz <= cylinder.Radius + sphere.Radius)
			{
				Vector3 normal, intersectionPoint;

				// Special case: sphere is centered on cylinder's axis
				const float epsilon = 0.001f;
				if (distanceXz is > -epsilon and < epsilon)
				{
					// Return normal of the closest circle
					float distanceToBottom = Math.Abs(sphere.Center.Y - cylinderMinY);
					float distanceToTop = Math.Abs(sphere.Center.Y - cylinderMaxY);
					normal = distanceToBottom < distanceToTop ? -Vector3.UnitY : Vector3.UnitY;
					intersectionPoint = sphere.Center - normal * (sphere.Radius - Math.Min(distanceToBottom, distanceToTop));
					result = new IntersectionResult(normal, intersectionPoint);
					return true;
				}

				// Calculate normal pointing from cylinder center to sphere
				normal = new Vector3(sphere.Center.X - cylinder.BottomCenter.X, 0, sphere.Center.Z - cylinder.BottomCenter.Z);
				normal = Vector3.Normalize(normal);
				intersectionPoint = sphere.Center - normal * (sphere.Radius - (cylinder.Radius - distanceXz));
				result = new IntersectionResult(normal, intersectionPoint);
				return true;
			}
		}

		return false;
	}
}
