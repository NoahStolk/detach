using Detach.Collisions.Primitives3D;
using Detach.Numerics;
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
		const float epsilon = 0.01f;

		Vector3 normal = Vector3.Cross(triangle.B - triangle.A, triangle.C - triangle.A);
		normal = Vector3.Normalize(normal);
		float distance = Vector3.Dot(point - triangle.A, normal);

		if (Math.Abs(distance) > epsilon)
			return false; // Point is not in the plane of the triangle

		// Project the point onto the triangle's plane
		Vector3 projectedPoint = point - distance * normal;
		return PointInTriangleProjected(projectedPoint, triangle);
	}

	private static bool PointInTriangleProjected(Vector3 point, Triangle3D triangle)
	{
		Vector3 v0 = triangle.B - triangle.A;
		Vector3 v1 = triangle.C - triangle.A;
		Vector3 v2 = point - triangle.A;

		float d00 = Vector3.Dot(v0, v0);
		float d01 = Vector3.Dot(v0, v1);
		float d11 = Vector3.Dot(v1, v1);
		float d20 = Vector3.Dot(v2, v0);
		float d21 = Vector3.Dot(v2, v1);

		float denom = d00 * d11 - d01 * d01;
		if (Math.Abs(denom) < 1e-6)
			return false; // Degenerate triangle

		float v = (d11 * d20 - d01 * d21) / denom;
		float w = (d00 * d21 - d01 * d20) / denom;
		float u = 1.0f - v - w;

		// Point is inside the triangle if u, v, w are all >= 0 and <= 1
		return u >= 0 && v >= 0 && w >= 0;
	}

	public static bool PointInViewFrustum(Vector3 point, ViewFrustum viewFrustum)
	{
		for (int i = 0; i < 6; i++)
		{
			Plane plane = viewFrustum[i];
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

	public static bool PointInPyramid(Vector3 point, Pyramid pyramid)
	{
		Vector3 halfSize = pyramid.Size / 2;
		Vector3 baseCenter = pyramid.Center - new Vector3(0, halfSize.Y, 0);

		// Check if the point is within the bounds of the pyramid's base.
		if (point.X < baseCenter.X - halfSize.X || point.X > baseCenter.X + halfSize.X ||
		    point.Z < baseCenter.Z - halfSize.Z || point.Z > baseCenter.Z + halfSize.Z)
		{
			return false;
		}

		// Check if the point is below the pyramid's apex and above the base.
		float height = pyramid.Size.Y;
		float apexY = baseCenter.Y + height;
		if (point.Y < baseCenter.Y || point.Y > apexY)
		{
			return false;
		}

		// Check if the point is within the pyramid's sloping sides.
		float dx = Math.Abs(point.X - baseCenter.X) / halfSize.X;
		float dz = Math.Abs(point.Z - baseCenter.Z) / halfSize.Z;
		float maxY = apexY - height * Math.Max(dx, dz);

		return point.Y <= maxY;
	}

	public static bool PointInOrientedPyramid(Vector3 point, OrientedPyramid orientedPyramid)
	{
		// Translate point into the local space of the pyramid
		Vector3 translated = point - orientedPyramid.Center;

		// Apply inverse rotation
		Matrix3 inverseTransform = Matrix3.Inverse(orientedPyramid.Orientation);
		Vector3 localPoint = Vector3.Transform(translated, inverseTransform.ToMatrix4x4());

		// Check against the axis-aligned pyramid
		Pyramid pyramid = new(Vector3.Zero, orientedPyramid.Size); // Local pyramid at origin
		return PointInPyramid(localPoint, pyramid);
	}
}
