﻿using Detach.Collisions.Primitives;
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

	private static bool OverlapOnAxis(Aabb aabb, Obb obb, Vector3 axis)
	{
		Interval interval1 = aabb.GetInterval(axis);
		Interval interval2 = obb.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	private static bool OverlapOnAxis(Obb obb1, Obb obb2, Vector3 axis)
	{
		Interval interval1 = obb1.GetInterval(axis);
		Interval interval2 = obb2.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	public static bool AabbObbSat(Aabb aabb, Obb obb)
	{
		Span<Vector3> axes = stackalloc Vector3[15];
		axes[0] = new(1, 0, 0);
		axes[1] = new(0, 1, 0);
		axes[2] = new(0, 0, 1);
		axes[3] = new(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13);
		axes[4] = new(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23);
		axes[5] = new(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33);

		for (int i = 0; i < 3; i++)
		{
			axes[6 + i * 3 + 0] = Vector3.Cross(axes[i], axes[0]);
			axes[6 + i * 3 + 1] = Vector3.Cross(axes[i], axes[1]);
			axes[6 + i * 3 + 2] = Vector3.Cross(axes[i], axes[2]);
		}

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(aabb, obb, axes[i]))
				return false;
		}

		return true;
	}

	public static bool AabbPlane(Aabb aabb, Plane plane)
	{
		float pLen =
			aabb.Size.X * Math.Abs(plane.Normal.X) +
			aabb.Size.Y * Math.Abs(plane.Normal.Y) +
			aabb.Size.Z * Math.Abs(plane.Normal.Z);

		float dot = Vector3.Dot(plane.Normal, aabb.Origin);
		float distance = dot - plane.D;
		return Math.Abs(distance) <= pLen;
	}

	public static bool ObbObbSat(Obb obb1, Obb obb2)
	{
		Span<Vector3> axes = stackalloc Vector3[15];
		axes[0] = new(obb1.Orientation.M11, obb1.Orientation.M12, obb1.Orientation.M13);
		axes[1] = new(obb1.Orientation.M21, obb1.Orientation.M22, obb1.Orientation.M23);
		axes[2] = new(obb1.Orientation.M31, obb1.Orientation.M32, obb1.Orientation.M33);
		axes[3] = new(obb2.Orientation.M11, obb2.Orientation.M12, obb2.Orientation.M13);
		axes[4] = new(obb2.Orientation.M21, obb2.Orientation.M22, obb2.Orientation.M23);
		axes[5] = new(obb2.Orientation.M31, obb2.Orientation.M32, obb2.Orientation.M33);

		for (int i = 0; i < 3; i++)
		{
			axes[6 + i * 3 + 0] = Vector3.Cross(axes[i], axes[0]);
			axes[6 + i * 3 + 1] = Vector3.Cross(axes[i], axes[1]);
			axes[6 + i * 3 + 2] = Vector3.Cross(axes[i], axes[2]);
		}

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(obb1, obb2, axes[i]))
				return false;
		}

		return true;
	}

	public static bool ObbPlane(Obb obb, Plane plane)
	{
		Span<Vector3> axes = stackalloc Vector3[3]
		{
			new(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13),
			new(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23),
			new(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33),
		};

		float pLen =
			obb.Size.X * Math.Abs(Vector3.Dot(plane.Normal, axes[0])) +
			obb.Size.Y * Math.Abs(Vector3.Dot(plane.Normal, axes[1])) +
			obb.Size.Z * Math.Abs(Vector3.Dot(plane.Normal, axes[2]));

		float dot = Vector3.Dot(plane.Normal, obb.Position);
		float distance = dot - plane.D;
		return Math.Abs(distance) <= pLen;
	}

	public static bool PlanePlane(Plane plane1, Plane plane2)
	{
		Vector3 cross = Vector3.Cross(plane1.Normal, plane2.Normal);
		return cross.LengthSquared() > float.Epsilon;
	}

	public static float? Raycast(Sphere sphere, Ray ray)
	{
		Vector3 e = sphere.Position - ray.Origin;
		float radiusSquared = sphere.Radius * sphere.Radius;
		float eSquared = e.LengthSquared();
		float a = Vector3.Dot(e, ray.Direction);
		float bSquared = eSquared - a * a;
		float f = MathF.Sqrt(radiusSquared - bSquared);

		if (radiusSquared - (eSquared - a * a) < 0)
			return null;

		if (eSquared < radiusSquared)
			return a + f;

		return a - f;
	}

	public static float? Raycast(Aabb aabb, Ray ray)
	{
		Vector3 min = aabb.GetMin();
		Vector3 max = aabb.GetMax();
		float t1 = (min.X - ray.Origin.X) / ray.Direction.X;
		float t2 = (max.X - ray.Origin.X) / ray.Direction.X;
		float t3 = (min.Y - ray.Origin.Y) / ray.Direction.Y;
		float t4 = (max.Y - ray.Origin.Y) / ray.Direction.Y;
		float t5 = (min.Z - ray.Origin.Z) / ray.Direction.Z;
		float t6 = (max.Z - ray.Origin.Z) / ray.Direction.Z;

		float tMin = Math.Max(Math.Max(Math.Min(t1, t2), Math.Min(t3, t4)), Math.Min(t5, t6));
		float tMax = Math.Min(Math.Min(Math.Max(t1, t2), Math.Max(t3, t4)), Math.Max(t5, t6));

		if (tMax < 0 || tMin > tMax)
			return null;

		return tMin < 0 ? tMax : tMin;
	}

	public static float? Raycast(Obb obb, Ray ray)
	{
		Vector3 x = new(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13);
		Vector3 y = new(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23);
		Vector3 z = new(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33);
		Vector3 p = obb.Position - ray.Origin;
		Vector3 f = new(
			Vector3.Dot(x, ray.Direction),
			Vector3.Dot(y, ray.Direction),
			Vector3.Dot(z, ray.Direction));
		Vector3 e = new(
			Vector3.Dot(x, p),
			Vector3.Dot(y, p),
			Vector3.Dot(z, p));
		Span<float> t = stackalloc float[6];
		for (int i = 0; i < 3; i++)
		{
			if (Math.Abs(f[i]) < float.Epsilon)
			{
				if (-e[i] - obb.Size[i] > 0 || -e[i] + obb.Size[i] < 0)
					return null;

				f[i] = float.Epsilon; // Avoid division by 0.
			}

			t[i * 2 + 0] = (e[i] + obb.Size[i]) / f[i];
			t[i * 2 + 1] = (e[i] - obb.Size[i]) / f[i];
		}

		float tMin = Math.Max(Math.Max(Math.Min(t[0], t[1]), Math.Min(t[2], t[3])), Math.Min(t[4], t[5]));
		float tMax = Math.Min(Math.Min(Math.Max(t[0], t[1]), Math.Max(t[2], t[3])), Math.Max(t[4], t[5]));
		if (tMax < 0 || tMin > tMax)
			return null;

		return tMin < 0 ? tMax : tMin;
	}

	public static float? Raycast(Plane plane, Ray ray)
	{
		float nd = Vector3.Dot(ray.Direction, plane.Normal);
		if (nd >= 0)
			return null;

		float pn = Vector3.Dot(ray.Origin, plane.Normal);
		float t = (plane.D - pn) / nd;
		return t < 0 ? null : t;
	}
}
