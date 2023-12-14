using Detach.Collisions.Primitives;
using System.Numerics;

namespace Detach.Collisions;

public static class Geometry3D
{
	public static bool PointInSphere(Vector3 point, Sphere sphere)
	{
		return Vector3.DistanceSquared(point, sphere.Position) <= sphere.Radius * sphere.Radius;
	}

	public static Vector3 ClosestPointInSphere(Sphere sphere, Vector3 point)
	{
		return sphere.Position + Vector3.Normalize(point - sphere.Position) * sphere.Radius;
	}

	public static bool PointInAabb(Vector3 point, Aabb aabb)
	{
		Vector3 min = aabb.GetMin();
		Vector3 max = aabb.GetMax();

		return point.X >= min.X && point.X <= max.X
			&& point.Y >= min.Y && point.Y <= max.Y
			&& point.Z >= min.Z && point.Z <= max.Z;
	}

	public static Vector3 ClosestPointInAabb(Aabb aabb, Vector3 point)
	{
		Vector3 min = aabb.GetMin();
		Vector3 max = aabb.GetMax();

		return new(
			Math.Clamp(point.X, min.X, max.X),
			Math.Clamp(point.Y, min.Y, max.Y),
			Math.Clamp(point.Z, min.Z, max.Z));
	}

	public static bool PointInObb(Vector3 point, Obb obb)
	{
		Vector3 direction = point - obb.Position;
		for (int i = 0; i < 3; i++)
		{
			Vector3 axis = new(
				obb.Orientation[i * 3 + 0],
				obb.Orientation[i * 3 + 1],
				obb.Orientation[i * 3 + 2]);
			float distance = Vector3.Dot(direction, axis);
			if (distance > obb.Size[i] || distance < -obb.Size[i])
				return false;
		}

		return true;
	}

	public static Vector3 ClosestPointInObb(Obb obb, Vector3 point)
	{
		Vector3 direction = point - obb.Position;
		Vector3 result = obb.Position;
		for (int i = 0; i < 3; i++)
		{
			Vector3 axis = new(
				obb.Orientation[i * 3 + 0],
				obb.Orientation[i * 3 + 1],
				obb.Orientation[i * 3 + 2]);
			float distance = Vector3.Dot(direction, axis);
			result += axis * Math.Clamp(distance, -obb.Size[i], obb.Size[i]);
		}

		return result;
	}

	public static bool PointOnPlane(Vector3 point, Plane plane)
	{
		float dot = Vector3.Dot(point, plane.Normal);
		float distance = dot - plane.D;
		return distance is >= -float.Epsilon and <= float.Epsilon;
	}

	public static Vector3 ClosestPointOnPlane(Plane plane, Vector3 point)
	{
		float dot = Vector3.Dot(point, plane.Normal);
		float distance = dot - plane.D;
		return point - plane.Normal * distance;
	}

	public static bool PointOnLine(Vector3 point, LineSegment3D line)
	{
		Vector3 closest = ClosestPointOnLine(point, line);
		float distanceSquared = Vector3.DistanceSquared(point, closest);
		return distanceSquared <= float.Epsilon;
	}

	public static Vector3 ClosestPointOnLine(Vector3 point, LineSegment3D line)
	{
		Vector3 direction = line.End - line.Start;
		float t = Vector3.Dot(point - line.Start, direction) / Vector3.Dot(direction, direction);
		t = Math.Clamp(t, 0, 1);
		return line.Start + direction * t;
	}

	public static bool PointOnRay(Vector3 point, Ray ray)
	{
		if (point == ray.Origin)
			return true;

		Vector3 normalized = Vector3.Normalize(point - ray.Origin);
		float dot = Vector3.Dot(normalized, ray.Direction);
		return dot is >= 1 - float.Epsilon and <= 1 + float.Epsilon;
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

	public static bool SphereSphere(Sphere sphere1, Sphere sphere2)
	{
		float distanceSquared = Vector3.DistanceSquared(sphere1.Position, sphere2.Position);
		float radiusSum = sphere1.Radius + sphere2.Radius;
		return distanceSquared <= radiusSum * radiusSum;
	}

	public static bool SphereAabb(Sphere sphere, Aabb aabb)
	{
		Vector3 closest = ClosestPointInAabb(aabb, sphere.Position);
		float distanceSquared = Vector3.DistanceSquared(sphere.Position, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool SphereObb(Sphere sphere, Obb obb)
	{
		Vector3 closest = ClosestPointInObb(obb, sphere.Position);
		float distanceSquared = Vector3.DistanceSquared(sphere.Position, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool SpherePlane(Sphere sphere, Plane plane)
	{
		Vector3 closest = ClosestPointOnPlane(plane, sphere.Position);
		float distanceSquared = Vector3.DistanceSquared(sphere.Position, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool AabbAabb(Aabb aabb1, Aabb aabb2)
	{
		Vector3 min1 = aabb1.GetMin();
		Vector3 max1 = aabb1.GetMax();
		Vector3 min2 = aabb2.GetMin();
		Vector3 max2 = aabb2.GetMax();

		return min1.X <= max2.X && max1.X >= min2.X
			&& min1.Y <= max2.Y && max1.Y >= min2.Y
			&& min1.Z <= max2.Z && max1.Z >= min2.Z;
	}
}
