using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool PointInSphere(Vector3 point, Sphere sphere)
	{
		return Vector3.DistanceSquared(point, sphere.Center) <= sphere.Radius * sphere.Radius;
	}

	public static bool PointInAabb(Vector3 point, Aabb aabb)
	{
		Vector3 min = aabb.GetMin();
		Vector3 max = aabb.GetMax();

		return
			point.X >= min.X && point.X <= max.X &&
			point.Y >= min.Y && point.Y <= max.Y &&
			point.Z >= min.Z && point.Z <= max.Z;
	}

	public static bool PointInObb(Vector3 point, Obb obb)
	{
		Vector3 direction = point - obb.Center;
		for (int i = 0; i < 3; i++)
		{
			Vector3 axis = new(
				obb.Orientation[i * 3 + 0],
				obb.Orientation[i * 3 + 1],
				obb.Orientation[i * 3 + 2]);
			float distance = Vector3.Dot(direction, axis);
			if (distance > obb.HalfExtents[i] || distance < -obb.HalfExtents[i])
				return false;
		}

		return true;
	}

	public static bool PointOnPlane(Vector3 point, Plane plane)
	{
		float dot = Vector3.Dot(point, plane.Normal);
		float distance = dot - plane.D;
		return distance is >= -float.Epsilon and <= float.Epsilon;
	}

	public static bool PointOnLine(Vector3 point, LineSegment3D line)
	{
		Vector3 closest = ClosestPointOnLine(point, line);
		float distanceSquared = Vector3.DistanceSquared(point, closest);
		return distanceSquared <= float.Epsilon;
	}

	public static bool PointOnRay(Vector3 point, Ray ray)
	{
		if (point == ray.Origin)
			return true;

		Vector3 normalized = Vector3.Normalize(point - ray.Origin);
		float dot = Vector3.Dot(normalized, ray.Direction);
		return dot is >= 1 - float.Epsilon and <= 1 + float.Epsilon;
	}

	public static bool PointInTriangle(Vector3 point, Triangle3D triangle)
	{
		Vector3 a = triangle.A - point;
		Vector3 b = triangle.B - point;
		Vector3 c = triangle.C - point;

		Vector3 normPbc = Vector3.Cross(b, c);
		Vector3 normPca = Vector3.Cross(c, a);
		Vector3 normPab = Vector3.Cross(a, b);

		if (Vector3.Dot(normPbc, normPca) < 0)
			return false;

		return Vector3.Dot(normPbc, normPab) >= 0;
	}

	public static bool PointInPyramidFrustum(Vector3 point, PyramidFrustum pyramidFrustum)
	{
		for (int i = 0; i < 6; i++)
		{
			Plane plane = pyramidFrustum[i];
			if (point.X * plane.Normal.X + point.Y * plane.Normal.Y + point.Z * plane.Normal.Z + plane.D > 0)
				return false;
		}

		return true;
	}

	public static bool PointInCylinder(Vector3 point, Cylinder cylinder)
	{
		Vector2 pointXz = new(point.X, point.Z);
		Vector2 positionXz = new(cylinder.BottomCenter.X, cylinder.BottomCenter.Z);

		return
			Vector2.DistanceSquared(pointXz, positionXz) <= cylinder.Radius * cylinder.Radius &&
			point.Y >= cylinder.BottomCenter.Y &&
			point.Y <= cylinder.BottomCenter.Y + cylinder.Height;
	}
}
