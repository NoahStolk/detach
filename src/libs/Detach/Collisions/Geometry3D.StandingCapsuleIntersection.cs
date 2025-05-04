using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool StandingCapsuleTriangle(StandingCapsule capsule, Triangle3D triangle, out IntersectionResult intersectionResult)
	{
		intersectionResult = default;

		// Check bottom sphere
		Sphere bottomSphere = new(capsule.BottomCenter, capsule.Radius);
		if (SphereTriangle(bottomSphere, triangle, out IntersectionResult bottomResult))
		{
			intersectionResult = bottomResult;
			return true;
		}

		// Check top sphere
		Sphere topSphere = new(capsule.TopCenter, capsule.Radius);
		if (SphereTriangle(topSphere, triangle, out IntersectionResult topResult))
		{
			intersectionResult = topResult;
			return true;
		}

		// Check cylindrical segment
		if (CapsuleSegmentTriangle(capsule.BottomCenter, capsule.TopCenter, capsule.Radius, triangle, out var cylResult))
		{
			intersectionResult = cylResult;
			return true;
		}

		return false;
	}

	private static bool CapsuleSegmentTriangle(Vector3 segA, Vector3 segB, float radius, Triangle3D triangle, out IntersectionResult result)
	{
		result = default;

		// Find closest point on triangle to the segment
		Vector3 closestOnTriangle = ClosestPointTriangleSegment(triangle, segA, segB, out Vector3 closestOnSegment);

		Vector3 delta = closestOnSegment - closestOnTriangle;
		float distSq = delta.LengthSquared();

		if (distSq <= radius * radius)
		{
			float dist = MathF.Sqrt(distSq);
			Vector3 normal = dist > 1e-6f ? delta / dist : triangle.GetNormal();

			result = new IntersectionResult(
				normal,
				closestOnSegment - normal * radius,
				radius - dist
			);

			return true;
		}

		return false;
	}

	private static Vector3 ClosestPointTriangleSegment(Triangle3D triangle, Vector3 segA, Vector3 segB, out Vector3 closestOnSegment)
	{
		float bestDistSq = float.MaxValue;
		Vector3 bestPointTri = default;
		Vector3 bestPointSeg = default;

		// Check triangle edges vs segment
		for (int i = 0; i < 3; i++)
		{
			Vector3 triEdgeA = triangle[i];
			Vector3 triEdgeB = triangle[(i + 1) % 3];

			ClosestPointsBetweenSegments(triEdgeA, triEdgeB, segA, segB, out Vector3 ptOnTri, out Vector3 ptOnSeg);
			float distSq = Vector3.DistanceSquared(ptOnTri, ptOnSeg);
			if (distSq < bestDistSq)
			{
				bestDistSq = distSq;
				bestPointTri = ptOnTri;
				bestPointSeg = ptOnSeg;
			}
		}

		// Check triangle face vs segment
		Vector3 ptOnTriPlane = ClosestPointOnTriangle(segA, triangle);
		float distSqA = Vector3.DistanceSquared(segA, ptOnTriPlane);
		if (distSqA < bestDistSq)
		{
			bestDistSq = distSqA;
			bestPointTri = ptOnTriPlane;
			bestPointSeg = segA;
		}

		Vector3 ptOnTriPlaneB = ClosestPointOnTriangle(segB, triangle);
		float distSqB = Vector3.DistanceSquared(segB, ptOnTriPlaneB);
		if (distSqB < bestDistSq)
		{
			bestDistSq = distSqB;
			bestPointTri = ptOnTriPlaneB;
			bestPointSeg = segB;
		}

		closestOnSegment = bestPointSeg;
		return bestPointTri;
	}

	private static void ClosestPointsBetweenSegments(
		Vector3 p1, Vector3 q1,
		Vector3 p2, Vector3 q2,
		out Vector3 c1, out Vector3 c2)
	{
		Vector3 d1 = q1 - p1; // segment 1 direction
		Vector3 d2 = q2 - p2; // segment 2 direction
		Vector3 r = p1 - p2;

		float a = Vector3.Dot(d1, d1);
		float e = Vector3.Dot(d2, d2);
		float f = Vector3.Dot(d2, r);

		float s, t;

		if (a <= 1e-6f && e <= 1e-6f)
		{
			// both segments degenerate to points
			s = t = 0f;
			c1 = p1;
			c2 = p2;
			return;
		}
		if (a <= 1e-6f)
		{
			// first segment degenerates to a point
			s = 0f;
			t = Math.Clamp(f / e, 0f, 1f);
		}
		else
		{
			float c = Vector3.Dot(d1, r);
			if (e <= 1e-6f)
			{
				// second segment degenerates to a point
				t = 0f;
				s = Math.Clamp(-c / a, 0f, 1f);
			}
			else
			{
				// general case
				float b = Vector3.Dot(d1, d2);
				float denom = a * e - b * b;

				if (denom != 0f)
					s = Math.Clamp((b * f - c * e) / denom, 0f, 1f);
				else
					s = 0f;

				t = (b * s + f) / e;

				if (t < 0f)
				{
					t = 0f;
					s = Math.Clamp(-c / a, 0f, 1f);
				}
				else if (t > 1f)
				{
					t = 1f;
					s = Math.Clamp((b - c) / a, 0f, 1f);
				}
			}
		}

		c1 = p1 + d1 * s;
		c2 = p2 + d2 * t;
	}
}
