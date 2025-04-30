using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool SphereObb(Sphere sphere, Obb obb, out IntersectionResult result)
	{
		result = default;

		// Transform sphere center into OBB's local space
		Vector3 localCenter = Vector3.Transform(sphere.Center - obb.Center, obb.Orientation.ToMatrix4x4());

		// Find the closest intersecting plane
		float closestDistance = float.MaxValue;
		int closestAxis = -1;

		// Check each axis-aligned plane in local space
		for (int i = 0; i < 3; i++)
		{
			// Calculate distance from sphere center to plane
			float distance = Math.Abs(localCenter[i]) - obb.HalfExtents[i];

			// Only consider planes that the sphere is intersecting
			if (distance <= sphere.Radius)
			{
				// Check if the sphere's projection onto the other axes is within the OBB's bounds
				bool isWithinBounds = true;
				for (int j = 0; j < 3; j++)
				{
					if (i == j)
						continue; // Skip the current axis

					if (Math.Abs(localCenter[j]) > obb.HalfExtents[j] + sphere.Radius)
					{
						isWithinBounds = false;
						break;
					}
				}

				if (isWithinBounds)
				{
					// Use the absolute value of the distance to the plane
					float absDistance = Math.Abs(distance);
					if (absDistance < closestDistance)
					{
						closestDistance = absDistance;
						closestAxis = i;
					}
				}
			}
		}

		if (closestAxis != -1)
		{
			// Get the normal in world space
			Vector3 normal = new(
				obb.Orientation[closestAxis * 3 + 0],
				obb.Orientation[closestAxis * 3 + 1],
				obb.Orientation[closestAxis * 3 + 2]);

			// Ensure normal points away from the sphere
			if (Vector3.Dot(sphere.Center - obb.Center, normal) < 0)
				normal = -normal;

			// Calculate the point on the OBB's plane
			Vector3 planePoint = obb.Center;
			if (localCenter[closestAxis] > 0)
				planePoint += normal * obb.HalfExtents[closestAxis];
			else
				planePoint -= normal * obb.HalfExtents[closestAxis];

			// Project sphere center onto the plane
			Vector3 projectedPoint = sphere.Center - normal * Vector3.Dot(sphere.Center - planePoint, normal);

			// Calculate intersection point by moving from projected point along normal by penetration depth
			Vector3 intersectionPoint = projectedPoint + normal * (sphere.Radius - closestDistance);

			result = new IntersectionResult(normal, intersectionPoint);
			return true;
		}

		return false;
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
