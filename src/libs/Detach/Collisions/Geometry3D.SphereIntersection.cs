using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool SphereObb(Sphere sphere, Obb obb, out IntersectionResult result)
	{
		Matrix4x4 obbOrientation = obb.Orientation.ToMatrix4x4();

		Matrix4x4 inverseOrientation = Matrix4x4.Transpose(obbOrientation);
		Vector3 localCenter = Vector3.Transform(sphere.Center - obb.Center, inverseOrientation);

		bool isInside =
			Math.Abs(localCenter.X) <= obb.HalfExtents.X &&
			Math.Abs(localCenter.Y) <= obb.HalfExtents.Y &&
			Math.Abs(localCenter.Z) <= obb.HalfExtents.Z;

		Vector3 clamped = Vector3.Clamp(localCenter, -obb.HalfExtents, obb.HalfExtents);
		Vector3 closestPointWorld = Vector3.Transform(clamped, obbOrientation) + obb.Center;

		if (!isInside)
		{
			Vector3 toCenter = sphere.Center - closestPointWorld;
			float distance = toCenter.Length();

			if (distance > sphere.Radius)
			{
				result = default;
				return false;
			}

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

			Vector3 worldNormal = Vector3.TransformNormal(localNormal, obbOrientation);
			worldNormal = Vector3.Normalize(worldNormal);

			float penetration = sphere.Radius - distance;

			result = new IntersectionResult(worldNormal, closestPointWorld, penetration);
		}
		else
		{
			// Inside case: find nearest face and project to exit point
			float dx = obb.HalfExtents.X - Math.Abs(localCenter.X);
			float dy = obb.HalfExtents.Y - Math.Abs(localCenter.Y);
			float dz = obb.HalfExtents.Z - Math.Abs(localCenter.Z);

			Vector3 localNormal;
			Vector3 localPoint = localCenter;
			float minDistance;

			if (dx < dy && dx < dz)
			{
				localNormal = new Vector3(localCenter.X < 0 ? -1 : 1, 0, 0);
				localPoint.X = localNormal.X * obb.HalfExtents.X;
				minDistance = dx;
			}
			else if (dy < dz)
			{
				localNormal = new Vector3(0, localCenter.Y < 0 ? -1 : 1, 0);
				localPoint.Y = localNormal.Y * obb.HalfExtents.Y;
				minDistance = dy;
			}
			else
			{
				localNormal = new Vector3(0, 0, localCenter.Z < 0 ? -1 : 1);
				localPoint.Z = localNormal.Z * obb.HalfExtents.Z;
				minDistance = dz;
			}

			Vector3 worldNormal = Vector3.TransformNormal(localNormal, obbOrientation);
			worldNormal = Vector3.Normalize(worldNormal);

			Vector3 worldPoint = Vector3.Transform(localPoint, obbOrientation) + obb.Center;
			float penetration = minDistance + sphere.Radius;

			result = new IntersectionResult(worldNormal, worldPoint, penetration);
		}

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
			result = new IntersectionResult(-Vector3.UnitY, sphere.Center + Vector3.UnitY * (sphere.Radius - (cylinderMinY - sphereMinY)), 0);
			return true;
		}

		// Check top circle
		if (sphereMaxY >= cylinderMaxY && sphereMinY <= cylinderMaxY && distanceSquaredXz <= radiusSum * radiusSum)
		{
			result = new IntersectionResult(Vector3.UnitY, sphere.Center - Vector3.UnitY * (sphere.Radius - (sphereMaxY - cylinderMaxY)), 0);
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
					result = new IntersectionResult(normal, intersectionPoint, 0);
					return true;
				}

				// Calculate normal pointing from cylinder center to sphere
				normal = new Vector3(sphere.Center.X - cylinder.BottomCenter.X, 0, sphere.Center.Z - cylinder.BottomCenter.Z);
				normal = Vector3.Normalize(normal);
				intersectionPoint = sphere.Center - normal * (sphere.Radius - (cylinder.Radius - distanceXz));
				result = new IntersectionResult(normal, intersectionPoint, 0);
				return true;
			}
		}

		return false;
	}
}
