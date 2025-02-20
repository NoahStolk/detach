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
}
