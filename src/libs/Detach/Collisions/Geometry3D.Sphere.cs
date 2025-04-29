using Detach.Buffers;
using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using Detach.Utils;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool SphereSphere(Sphere sphere1, Sphere sphere2)
	{
		float distanceSquared = Vector3.DistanceSquared(sphere1.Center, sphere2.Center);
		float radiusSum = sphere1.Radius + sphere2.Radius;
		return distanceSquared <= radiusSum * radiusSum;
	}

	public static bool SphereAabb(Sphere sphere, Aabb aabb)
	{
		Vector3 closest = ClosestPointInAabb(sphere.Center, aabb);
		float distanceSquared = Vector3.DistanceSquared(sphere.Center, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool SphereObb(Sphere sphere, Obb obb)
	{
		Vector3 closest = ClosestPointInObb(sphere.Center, obb);
		float distanceSquared = Vector3.DistanceSquared(sphere.Center, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool SpherePlane(Sphere sphere, Plane plane)
	{
		Vector3 closest = ClosestPointOnPlane(sphere.Center, plane);
		float distanceSquared = Vector3.DistanceSquared(sphere.Center, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool SphereViewFrustum(Sphere sphere, ViewFrustum viewFrustum)
	{
		if (PointInViewFrustum(sphere.Center, viewFrustum))
			return true;

		for (int i = 0; i < 6; ++i)
		{
			if (SpherePlane(sphere, viewFrustum[i]))
				return true;
		}

		return false;
	}

	public static bool SphereCylinder(Sphere sphere, Cylinder cylinder)
	{
		// Project the sphere's center onto the XZ plane.
		Vector2 sphereXz = new(sphere.Center.X, sphere.Center.Z);
		Vector2 cylinderXz = new(cylinder.BottomCenter.X, cylinder.BottomCenter.Z);

		// Check if the distance in the XZ plane is less than or equal to the sum of the radii.
		float distanceSquaredXz = Vector2.DistanceSquared(sphereXz, cylinderXz);
		float radiusSum = sphere.Radius + cylinder.Radius;
		if (distanceSquaredXz > radiusSum * radiusSum)
			return false;

		// Check if the sphere's center is within the height range of the cylinder.
		float sphereMinY = sphere.Center.Y - sphere.Radius;
		float sphereMaxY = sphere.Center.Y + sphere.Radius;
		float cylinderMinY = cylinder.BottomCenter.Y;
		float cylinderMaxY = cylinder.BottomCenter.Y + cylinder.Height;

		return sphereMaxY >= cylinderMinY && sphereMinY <= cylinderMaxY;
	}

	public static bool SphereConeFrustum(Sphere sphere, ConeFrustum coneFrustum)
	{
		// Check intersection with bottom circle.
		Vector3 bottomCenter = coneFrustum.BottomCenter;
		float bottomRadius = coneFrustum.BottomRadius;
		if (Vector3.DistanceSquared(sphere.Center, bottomCenter) <= (sphere.Radius + bottomRadius) * (sphere.Radius + bottomRadius))
			return true;

		// Check intersection with top circle.
		Vector3 topCenter = bottomCenter + new Vector3(0, coneFrustum.Height, 0);
		float topRadius = coneFrustum.TopRadius;
		if (Vector3.DistanceSquared(sphere.Center, topCenter) <= (sphere.Radius + topRadius) * (sphere.Radius + topRadius))
			return true;

		// Check intersection with side surface.
		Vector3 axis = Vector3.Normalize(topCenter - bottomCenter);
		Vector3 sphereToBottom = sphere.Center - bottomCenter;
		float projectionLength = Vector3.Dot(sphereToBottom, axis);
		if (projectionLength < 0 || projectionLength > coneFrustum.Height)
			return false;

		float radiusAtProjection = MathUtils.Lerp(bottomRadius, topRadius, projectionLength / coneFrustum.Height);
		Vector3 closestPointOnAxis = bottomCenter + axis * projectionLength;
		float distanceSquared = Vector3.DistanceSquared(sphere.Center, closestPointOnAxis);
		return distanceSquared <= (sphere.Radius + radiusAtProjection) * (sphere.Radius + radiusAtProjection);
	}

	public static bool SpherePyramid(Sphere sphere, Pyramid pyramid)
	{
		Buffer4<Vector3> bottomRectangleVertices = pyramid.BaseVertices;

		Circle sphereOnXz = new(new Vector2(sphere.Center.X, sphere.Center.Z), sphere.Radius);
		Rectangle pyramidBaseOnXz = Rectangle.FromCenter(new Vector2(pyramid.Center.X, pyramid.Center.Z), new Vector2(pyramid.Size.X, pyramid.Size.Z));
		if (!Geometry2D.CircleRectangle(sphereOnXz, pyramidBaseOnXz))
			return false;

		Circle sphereOnXy = new(new Vector2(sphere.Center.X, sphere.Center.Y), sphere.Radius);
		Triangle2D pyramidOnXy = new(
			new Vector2(pyramid.ApexVertex.X, pyramid.ApexVertex.Y),
			new Vector2(bottomRectangleVertices[0].X, bottomRectangleVertices[0].Y),
			new Vector2(bottomRectangleVertices[1].X, bottomRectangleVertices[1].Y));
		if (!Geometry2D.CircleTriangle(sphereOnXy, pyramidOnXy))
			return false;

		Circle sphereOnYz = new(new Vector2(sphere.Center.Y, sphere.Center.Z), sphere.Radius);
		Triangle2D pyramidOnYz = new(
			new Vector2(pyramid.ApexVertex.Y, pyramid.ApexVertex.Z),
			new Vector2(bottomRectangleVertices[0].Y, bottomRectangleVertices[0].Z),
			new Vector2(bottomRectangleVertices[2].Y, bottomRectangleVertices[2].Z));
		return Geometry2D.CircleTriangle(sphereOnYz, pyramidOnYz);
	}

	public static bool SphereOrientedPyramid(Sphere sphere, OrientedPyramid orientedPyramid)
	{
		Vector3 closestPoint = ClosestPointInOrientedPyramid(sphere.Center, orientedPyramid);
		float distanceSquared = Vector3.DistanceSquared(sphere.Center, closestPoint);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static Vector3? SphereObbNormal(Sphere sphere, Obb obb)
	{
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

			return normal;
		}

		return null;
	}

	public static Vector3? SphereCylinderNormal(Sphere sphere, Cylinder cylinder)
	{
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
			return -Vector3.UnitY; // Normal points away from cylinder

		// Check top circle
		if (sphereMaxY >= cylinderMaxY && sphereMinY <= cylinderMaxY && distanceSquaredXz <= radiusSum * radiusSum)
			return Vector3.UnitY; // Normal points away from cylinder

		// Check side surface
		if (sphereMaxY >= cylinderMinY && sphereMinY <= cylinderMaxY)
		{
			float distanceXz = MathF.Sqrt(distanceSquaredXz);
			if (distanceXz <= cylinder.Radius + sphere.Radius)
			{
				// Special case: sphere is centered on cylinder's axis
				const float epsilon = 0.001f;
				if (distanceXz is > -epsilon and < epsilon)
				{
					// Return normal of the closest circle
					float distanceToBottom = Math.Abs(sphere.Center.Y - cylinderMinY);
					float distanceToTop = Math.Abs(sphere.Center.Y - cylinderMaxY);
					return distanceToBottom < distanceToTop ? -Vector3.UnitY : Vector3.UnitY;
				}

				// Calculate normal pointing from cylinder center to sphere
				Vector3 normal = new(sphere.Center.X - cylinder.BottomCenter.X, 0, sphere.Center.Z - cylinder.BottomCenter.Z);
				return Vector3.Normalize(normal);
			}
		}

		return null;
	}
}
