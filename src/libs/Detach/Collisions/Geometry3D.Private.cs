using Detach.Buffers;
using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	private static bool ClipToPlane(Plane plane, LineSegment3D line, out Vector3 result)
	{
		result = default;

		Vector3 ab = line.End - line.Start;
		float nAb = Vector3.Dot(plane.Normal, ab);
		if (nAb == 0)
			return false;

		float nA = Vector3.Dot(plane.Normal, line.Start);
		float t = (plane.D - nA) / nAb;

		if (t is < 0 or > 1)
			return false;

		result = line.Start + ab * t;
		return true;
	}

	private static int ClipEdgesToObb(Buffer12<LineSegment3D> edges, Obb obb, out Buffer12<Vector3> result)
	{
		int count = 0;
		result = default;
		Buffer6<Plane> planes = obb.GetPlanes();
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 12; j++)
			{
				if (count >= 12)
					throw new InvalidOperationException("Too many contacts.");

				if (ClipToPlane(planes[i], edges[j], out Vector3 intersection))
				{
					// The book uses PointInObb here, but that doesn't seem to work, since the points always lie just outside the obb after being clipped by the previous function.
					// I don't understand why this check is needed. If the edge clips successfully, the intersection point should always be on one of the obb planes?
					if (PointInObb(intersection, obb))
						result[count++] = intersection;
				}
			}
		}

		return count;
	}

	private static float PenetrationDepthObb(Obb obb1, Obb obb2, Vector3 axis, out bool shouldFlip)
	{
		shouldFlip = false;

		axis = Vector3.Normalize(axis);
		Interval i1 = obb1.GetInterval(axis);
		Interval i2 = obb2.GetInterval(axis);

		if (!(i2.Min <= i1.Max && i1.Min <= i2.Max))
			return 0;

		float len1 = i1.Max - i1.Min;
		float len2 = i2.Max - i2.Min;
		float min = Math.Min(i1.Min, i2.Min);
		float max = Math.Max(i1.Max, i2.Max);

		float length = max - min;
		shouldFlip = i2.Min < i1.Min;
		return len1 + len2 - length;
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

	private static bool OverlapOnAxis(Aabb aabb, Triangle3D triangle, Vector3 axis)
	{
		Interval interval1 = aabb.GetInterval(axis);
		Interval interval2 = triangle.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	private static bool OverlapOnAxis(Obb obb, Triangle3D triangle, Vector3 axis)
	{
		Interval interval1 = obb.GetInterval(axis);
		Interval interval2 = triangle.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	private static bool OverlapOnAxis(Triangle3D triangle1, Triangle3D triangle2, Vector3 axis)
	{
		Interval interval1 = triangle1.GetInterval(axis);
		Interval interval2 = triangle2.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	// Method is internal for testing purposes.
	internal static Plane CreatePlaneFromTriangle(Triangle3D triangle)
	{
		// Use a fixed vertex order (A->B->C) to ensure consistent normal orientation.
		Vector3 ab = triangle.B - triangle.A;
		Vector3 ac = triangle.C - triangle.A;
		Vector3 normal = Vector3.Normalize(Vector3.Cross(ab, ac));
		return new Plane(normal, Vector3.Dot(normal, triangle.A));
	}
}
