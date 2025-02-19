using Detach.Collisions.Primitives3D;
using Detach.Numerics;
using Detach.Utils;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool SphereCastPoint(SphereCast sphereCast, Vector3 point)
	{
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		Vector3 m = sphereCast.Start - point;
		float b = Vector3.Dot(m, direction);
		float c = Vector3.Dot(m, m) - sphereCast.Radius * sphereCast.Radius;

		if (c > 0.0f && b > 0.0f)
			return false;

		float discriminant = b * b - c;
		if (discriminant < 0.0f)
			return false;

		float t = -b - MathF.Sqrt(discriminant);
		if (t < 0.0f)
			t = 0.0f;

		return t <= length;
	}

	public static bool SphereCastLineSegment(SphereCast sphereCast, LineSegment3D line)
	{
		Vector3 edge = line.End - line.Start;
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		Vector3 m = sphereCast.Start - line.Start;
		Vector3 n = Vector3.Cross(edge, direction);
		float d = Vector3.Dot(n, n);
		float e = Vector3.Dot(n, m);

		if (MathF.Abs(d) < float.Epsilon)
		{
			// Sphere is parallel to the edge.
			return false;
		}

		float t = e / d;
		if (t < 0 || t > length)
			return false;

		Vector3 point = sphereCast.Start + t * direction;
		Vector3 closest = ClosestPointOnLine(point, new LineSegment3D(line.Start, line.End));
		return Vector3.DistanceSquared(point, closest) <= sphereCast.Radius * sphereCast.Radius;
	}

	public static bool SphereCastSphere(SphereCast sphereCast, Sphere sphere)
	{
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		Vector3 m = sphereCast.Start - sphere.Center;
		float b = Vector3.Dot(m, direction);
		float c = Vector3.Dot(m, m) - (sphere.Radius + sphereCast.Radius) * (sphere.Radius + sphereCast.Radius);

		// Exit if the start point is outside the sphere, and the sphere from the sphere cast is not moving towards the stationary sphere.
		if (c > 0.0f && b > 0.0f)
			return false;

		float discriminant = b * b - c;

		// A negative discriminant means no real roots, so no intersection.
		if (discriminant < 0.0f)
			return false;

		// Calculate the smallest t value of intersection.
		float t = -b - MathF.Sqrt(discriminant);

		// If t is negative, the intersection point is behind the start point.
		if (t < 0.0f)
			t = 0.0f;

		// Check if the intersection point is within the segment.
		return t <= length;
	}

	public static bool SphereCastSphereCast(SphereCast sphereCast1, SphereCast sphereCast2)
	{
		Vector3 direction1 = sphereCast1.End - sphereCast1.Start;
		Vector3 direction2 = sphereCast2.End - sphereCast2.Start;
		float radiusSum = sphereCast1.Radius + sphereCast2.Radius;

		// Calculate the relative velocity.
		Vector3 relativeVelocity = direction2 - direction1;

		// Calculate the vector between the starting points of the sphere casts.
		Vector3 startDiff = sphereCast2.Start - sphereCast1.Start;

		// Calculate the coefficients of the quadratic equation.
		float a = Vector3.Dot(relativeVelocity, relativeVelocity);
		float b = 2 * Vector3.Dot(relativeVelocity, startDiff);
		float c = Vector3.Dot(startDiff, startDiff) - radiusSum * radiusSum;

		// Solve the quadratic equation for t.
		float discriminant = b * b - 4 * a * c;
		if (discriminant < 0)
		{
			// No real roots, the sphere casts do not intersect.
			return false;
		}

		// Calculate the two possible solutions for t.
		float sqrtDiscriminant = MathF.Sqrt(discriminant);
		float t1 = (-b - sqrtDiscriminant) / (2 * a);
		float t2 = (-b + sqrtDiscriminant) / (2 * a);

		// Check if there is an intersection within the time interval [0, 1].
		return t1 is >= 0 and <= 1 || t2 is >= 0 and <= 1;
	}

	public static bool SphereCastAabb(SphereCast sphereCast, Aabb aabb)
	{
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		// Expand the AABB by the sphere's radius.
		Vector3 aabbMin = aabb.GetMin() - new Vector3(sphereCast.Radius);
		Vector3 aabbMax = aabb.GetMax() + new Vector3(sphereCast.Radius);

		// Perform ray-AABB intersection test.
		float tMin = 0.0f;
		float tMax = length;

		for (int i = 0; i < 3; i++)
		{
			if (MathF.Abs(direction[i]) < float.Epsilon)
			{
				// Ray is parallel to the slab. No hit if origin not within slab.
				if (sphereCast.Start[i] < aabbMin[i] || sphereCast.Start[i] > aabbMax[i])
					return false;
			}
			else
			{
				// Compute intersection t value of ray with near and far plane of slab.
				float ood = 1.0f / direction[i];
				float t1 = (aabbMin[i] - sphereCast.Start[i]) * ood;
				float t2 = (aabbMax[i] - sphereCast.Start[i]) * ood;

				// Make t1 be intersection with near plane, t2 with far plane.
				if (t1 > t2)
					(t1, t2) = (t2, t1);

				// Compute the intersection of slab intersection intervals.
				tMin = MathF.Max(tMin, t1);
				tMax = MathF.Min(tMax, t2);

				// Exit with no collision as soon as slab intersection becomes empty.
				if (tMin > tMax)
					return false;
			}
		}

		return true;
	}

	public static bool SphereCastObb(SphereCast sphereCast, Obb obb)
	{
		// Transform sphere start and end points to OBB's local space.
		Matrix3 invOrientation = Matrix3.Transpose(obb.Orientation);
		Vector3 localSphereStart = VectorUtils.Transform(sphereCast.Start - obb.Center, invOrientation);
		Vector3 localSphereEnd = VectorUtils.Transform(sphereCast.End - obb.Center, invOrientation);

		// Perform sphere cast against the AABB in local space.
		Aabb aabb = new(Vector3.Zero, obb.HalfExtents * 2);
		return SphereCastAabb(new SphereCast(localSphereStart, localSphereEnd, sphereCast.Radius), aabb);
	}

	public static bool SphereCastTriangle(SphereCast sphereCast, Triangle3D triangle)
	{
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		Vector3 m = sphereCast.Start - triangle.A;
		Vector3 n = Vector3.Cross(triangle.B - triangle.A, triangle.C - triangle.A);
		float d = Vector3.Dot(n, direction);
		float e = Vector3.Dot(n, m);

		// Check if the sphere's path is parallel to the triangle's plane.
		if (MathF.Abs(d) < float.Epsilon)
		{
			// Sphere is parallel to the triangle's plane.
			if (MathF.Abs(e) >= sphereCast.Radius)
				return false;
		}
		else
		{
			// Compute the intersection point of the sphere's path with the triangle's plane.
			float t = -e / d;
			if (t < 0 || t > length)
				return false;

			Vector3 point = sphereCast.Start + t * direction;
			if (PointInTriangle(point, triangle))
				return true;
		}

		// Check for intersection with the triangle's vertices and edges.
		return
			SphereCastPoint(sphereCast, triangle.A) ||
			SphereCastPoint(sphereCast, triangle.B) ||
			SphereCastPoint(sphereCast, triangle.C) ||
			SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.A, triangle.B)) ||
			SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.B, triangle.C)) ||
			SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.C, triangle.A));
	}

	public static bool SphereCastTriangle(SphereCast sphereCast, Triangle3D triangle, out Vector3 intersectionPoint)
	{
		intersectionPoint = default;
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		Vector3 m = sphereCast.Start - triangle.A;
		Vector3 n = Vector3.Cross(triangle.B - triangle.A, triangle.C - triangle.A);
		float d = Vector3.Dot(n, direction);
		float e = Vector3.Dot(n, m);

		// Check if the sphere's path is parallel to the triangle's plane.
		if (MathF.Abs(d) < float.Epsilon)
		{
			// Sphere is parallel to the triangle's plane.
			if (MathF.Abs(e) >= sphereCast.Radius)
				return false;
		}
		else
		{
			// Compute the intersection point of the sphere's path with the triangle's plane.
			float t = -e / d;
			if (t < 0 || t > length)
				return false;

			Vector3 point = sphereCast.Start + t * direction;
			if (PointInTriangle(point, triangle))
			{
				intersectionPoint = point;
				return true;
			}
		}

		// Check for intersection with the triangle's vertices and edges.
		if (SphereCastPoint(sphereCast, triangle.A) ||
		    SphereCastPoint(sphereCast, triangle.B) ||
		    SphereCastPoint(sphereCast, triangle.C) ||
		    SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.A, triangle.B)) ||
		    SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.B, triangle.C)) ||
		    SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.C, triangle.A)))
		{
			Plane plane = CreatePlaneFromTriangle(triangle);
			Vector3 closestStart = ClosestPointOnPlane(sphereCast.Start, plane);
			Vector3 closestEnd = ClosestPointOnPlane(sphereCast.End, plane);
			intersectionPoint = (closestStart + closestEnd) * 0.5f;
			return true;
		}

		return false;
	}

	public static bool SphereCastCylinder(SphereCast sphereCast, Cylinder cylinder)
	{
		// Check intersection at the start of the SphereCast.
		if (SphereCylinder(new Sphere(sphereCast.Start, sphereCast.Radius), cylinder))
			return true;

		// Check intersection at the end of the SphereCast.
		if (SphereCylinder(new Sphere(sphereCast.End, sphereCast.Radius), cylinder))
			return true;

		// Check intersection along the path of the SphereCast.
		Vector3 direction = Vector3.Normalize(sphereCast.End - sphereCast.Start);
		float length = Vector3.Distance(sphereCast.Start, sphereCast.End);
		for (float t = 0; t <= length; t += sphereCast.Radius)
		{
			Vector3 point = sphereCast.Start + direction * t;
			if (SphereCylinder(new Sphere(point, sphereCast.Radius), cylinder))
				return true;
		}

		return false;
	}

	public static bool SphereCastConeFrustum(SphereCast sphereCast, ConeFrustum coneFrustum)
	{
		// Check intersection at the start of the SphereCast.
		if (SphereConeFrustum(new Sphere(sphereCast.Start, sphereCast.Radius), coneFrustum))
			return true;

		// Check intersection at the end of the SphereCast.
		if (SphereConeFrustum(new Sphere(sphereCast.End, sphereCast.Radius), coneFrustum))
			return true;

		// Check intersection along the path of the SphereCast.
		Vector3 direction = Vector3.Normalize(sphereCast.End - sphereCast.Start);
		float length = Vector3.Distance(sphereCast.Start, sphereCast.End);
		for (float t = 0; t <= length; t += sphereCast.Radius)
		{
			Vector3 point = sphereCast.Start + direction * t;
			if (SphereConeFrustum(new Sphere(point, sphereCast.Radius), coneFrustum))
				return true;
		}

		return false;
	}
}
