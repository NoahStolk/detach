using Detach.Collisions.Primitives3D;
using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool SphereObb(Sphere sphere, Obb obb, out IntersectionResult result)
	{
		result = default;

		// Step 1: Transform sphere center into OBB local space
		Matrix3 invOrientation = Matrix3.Transpose(obb.Orientation); // Assuming Orthogonal matrix
		Vector3 localCenter = invOrientation * (sphere.Center - obb.Center);

		// Step 2: Clamp local center to OBB bounds
		Vector3 clamped = Vector3.Clamp(localCenter, -obb.HalfExtents, obb.HalfExtents);

		// Step 3: Get vector from clamped point to local sphere center
		Vector3 localDelta = localCenter - clamped;
		float distanceSq = localDelta.LengthSquared();

		// Step 4: Check intersection
		if (distanceSq > sphere.Radius * sphere.Radius)
			return false;

		// Step 5: Determine best face normal (in local space)
		Vector3 bestNormal = Vector3.Zero;
		float maxDot = float.NegativeInfinity;

		// Avoid zero vector if sphere center is inside OBB
		if (localDelta.LengthSquared() > 1e-6f)
		{
			Vector3 localNormal = Vector3.Normalize(localDelta);

			// Compare against each axis
			for (int i = 0; i < 3; i++)
			{
				Vector3 axis = i switch
				{
					0 => Vector3.UnitX,
					1 => Vector3.UnitY,
					_ => Vector3.UnitZ,
				};
				float dot = MathF.Abs(Vector3.Dot(localNormal, axis));
				if (dot > maxDot)
				{
					maxDot = dot;
					bestNormal = axis * MathF.Sign(Vector3.Dot(localNormal, axis));
				}
			}
		}
		else
		{
			// Sphere center is inside OBB: pick the face with minimal penetration
			Vector3 distances = obb.HalfExtents - Vector3.Abs(localCenter);
			if (distances.X <= distances.Y && distances.X <= distances.Z)
				bestNormal = new Vector3(MathF.Sign(localCenter.X), 0, 0);
			else if (distances.Y <= distances.X && distances.Y <= distances.Z)
				bestNormal = new Vector3(0, MathF.Sign(localCenter.Y), 0);
			else
				bestNormal = new Vector3(0, 0, MathF.Sign(localCenter.Z));
		}

		// Step 6: Compute intersection point on the best face (in local space)
		Vector3 localIntersection = clamped;
		if (MathF.Abs(bestNormal.X) > 0)
			localIntersection.X = bestNormal.X * obb.HalfExtents.X;
		if (MathF.Abs(bestNormal.Y) > 0)
			localIntersection.Y = bestNormal.Y * obb.HalfExtents.Y;
		if (MathF.Abs(bestNormal.Z) > 0)
			localIntersection.Z = bestNormal.Z * obb.HalfExtents.Z;

		// Step 7: Transform results to world space
		Vector3 worldIntersection = obb.Center + obb.Orientation * localIntersection;
		Vector3 worldNormal = Vector3.Normalize(obb.Orientation * bestNormal);

		result = new IntersectionResult(worldNormal, worldIntersection);
		return true;

		static Vector3 TransformWithZFlip(Matrix3 m, Vector3 v)
		{
			Vector3 result = m * v;
			result.Z = -result.Z;
			return result;
		}
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
