using Detach.Buffers;
using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static Vector3 ClosestPointInSphere(Vector3 point, Sphere sphere)
	{
		return sphere.Center + Vector3.Normalize(point - sphere.Center) * sphere.Radius;
	}

	public static Vector3 ClosestPointInAabb(Vector3 point, Aabb aabb)
	{
		Vector3 min = aabb.GetMin();
		Vector3 max = aabb.GetMax();

		return new Vector3(
			Math.Clamp(point.X, min.X, max.X),
			Math.Clamp(point.Y, min.Y, max.Y),
			Math.Clamp(point.Z, min.Z, max.Z));
	}

	public static Vector3 ClosestPointInObb(Vector3 point, Obb obb)
	{
		Vector3 direction = point - obb.Center;
		Vector3 result = obb.Center;
		for (int i = 0; i < 3; i++)
		{
			Vector3 axis = new(
				obb.Orientation[i * 3 + 0],
				obb.Orientation[i * 3 + 1],
				obb.Orientation[i * 3 + 2]);
			float distance = Vector3.Dot(direction, axis);
			result += axis * Math.Clamp(distance, -obb.HalfExtents[i], obb.HalfExtents[i]);
		}

		return result;
	}

	public static Vector3 ClosestPointOnPlane(Vector3 point, Plane plane)
	{
		float dot = Vector3.Dot(point, plane.Normal);
		float distance = dot - plane.D;
		return point - plane.Normal * distance;
	}

	public static Vector3 ClosestPointOnLine(Vector3 point, LineSegment3D line)
	{
		Vector3 direction = line.End - line.Start;
		float t = Vector3.Dot(point - line.Start, direction) / Vector3.Dot(direction, direction);
		t = Math.Clamp(t, 0, 1);
		return line.Start + direction * t;
	}

	public static Vector3 ClosestPointOnRay(Vector3 point, Ray ray)
	{
		if (point == ray.Origin)
			return point;

		Vector3 normalized = Vector3.Normalize(point - ray.Origin);
		float t = Vector3.Dot(normalized, ray.Direction);
		t /= Vector3.Dot(ray.Direction, ray.Direction);
		t = Math.Max(t, 0);
		return ray.Origin + ray.Direction * t;
	}

	public static Vector3 ClosestPointOnTriangle(Vector3 point, Triangle3D triangle)
	{
		Plane plane = CreatePlaneFromTriangle(triangle);
		Vector3 closest = ClosestPointOnPlane(point, plane);
		if (PointInTriangle(closest, triangle))
			return closest;

		Vector3 c1 = ClosestPointOnLine(point, new LineSegment3D(triangle.A, triangle.B));
		Vector3 c2 = ClosestPointOnLine(point, new LineSegment3D(triangle.B, triangle.C));
		Vector3 c3 = ClosestPointOnLine(point, new LineSegment3D(triangle.C, triangle.A));

		float d1 = Vector3.DistanceSquared(point, c1);
		float d2 = Vector3.DistanceSquared(point, c2);
		float d3 = Vector3.DistanceSquared(point, c3);

		if (d1 < d2 && d1 < d3)
			return c1;

		if (d2 < d1 && d2 < d3)
			return c2;

		return c3;
	}

	public static Vector3 ClosestPointInPyramid(Vector3 point, Pyramid pyramid)
	{
		return ClosestPointInPyramidFaces(point, pyramid.Faces);
	}

	public static Vector3 ClosestPointInOrientedPyramid(Vector3 point, OrientedPyramid pyramid)
	{
		return ClosestPointInPyramidFaces(point, pyramid.Faces);
	}

	private static Vector3 ClosestPointInPyramidFaces(Vector3 point, Buffer6<Triangle3D> pyramidFaces)
	{
		Buffer6<Vector3> closestPoints = default;
		for (int i = 0; i < 6; i++)
		{
			Triangle3D face = pyramidFaces[i];
			closestPoints[i] = ClosestPointOnTriangle(point, face);
		}

		Vector3 closest = closestPoints[0];
		float closestDistanceSquared = Vector3.DistanceSquared(point, closest);
		for (int i = 1; i < 6; i++)
		{
			float distanceSquared = Vector3.DistanceSquared(point, closestPoints[i]);
			if (distanceSquared < closestDistanceSquared)
			{
				closest = closestPoints[i];
				closestDistanceSquared = distanceSquared;
			}
		}

		return closest;
	}
}
