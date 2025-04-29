using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool Linetest(Sphere sphere, LineSegment3D line)
	{
		Vector3 closest = ClosestPointOnLine(sphere.Center, line);
		float distanceSquared = Vector3.DistanceSquared(sphere.Center, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool Linetest(Aabb aabb, LineSegment3D line)
	{
		if (PointInAabb(line.Start, aabb) || PointInAabb(line.End, aabb))
			return true;

		Ray ray = new(line.Start, line.End - line.Start);
		if (!Raycast(aabb, ray, out float distance))
			return false;

		return distance >= 0 && distance * distance <= line.LengthSquared;
	}

	public static bool Linetest(Obb obb, LineSegment3D line)
	{
		if (PointInObb(line.Start, obb) || PointInObb(line.End, obb))
			return true;

		Ray ray = new(line.Start, line.End - line.Start);
		if (!Raycast(obb, ray, out float distance))
			return false;

		return distance >= 0 && distance * distance <= line.LengthSquared;
	}

	public static bool Linetest(Plane plane, LineSegment3D line)
	{
		Vector3 ab = line.End - line.Start;
		float nA = Vector3.Dot(plane.Normal, line.Start);
		float nAb = Vector3.Dot(plane.Normal, ab);

		float t = (plane.D - nA) / nAb;
		return t is >= 0 and <= 1;
	}

	public static bool Linetest(Triangle3D triangle, LineSegment3D line)
	{
		Ray ray = new(line.Start, line.End - line.Start);
		return Raycast(triangle, ray, out float t) && t >= 0 && t * t <= line.LengthSquared;
	}
}
