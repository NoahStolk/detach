using System.Numerics;

namespace Detach.Collisions;

public static class LineSegment
{
	public static (Vector2 CollisionForce, Vector2 IntersectionPoint)? IntersectsCircle(
		Vector2 lineSegmentP1,
		Vector2 lineSegmentP2,
		Vector2 circleOrigin,
		float circleRadius)
	{
		Vector2 normal = GetNormal(lineSegmentP1, lineSegmentP2);
		Vector2 center = GetCenter(lineSegmentP1, lineSegmentP2);

		if (Vector2.Dot(normal, circleOrigin - center) < 0)
			return null;

		// Compute vectors AC and AB
		Vector2 ac = circleOrigin - lineSegmentP1;
		Vector2 ab = lineSegmentP2 - lineSegmentP1;

		// Get point D by taking the projection of AC onto AB then adding the offset of A
		Vector2 d = Project(ac, ab) + lineSegmentP1;

		Vector2 ad = d - lineSegmentP1;

		// D might not be on AB so calculate k of D down AB (aka solve AD = k * AB)
		// We can use either component, but choose larger value to reduce the chance of dividing by zero
		float k = Math.Abs(ab.X) > Math.Abs(ab.Y) ? ad.X / ab.X : ad.Y / ab.Y;

		// Check if D is off either end of the line segment
		float distanceSegmentToPoint = k switch
		{
			<= 0.0f => MathF.Sqrt(Hypot2(circleOrigin, lineSegmentP1)),
			>= 1.0f => MathF.Sqrt(Hypot2(circleOrigin, lineSegmentP2)),
			_ => MathF.Sqrt(Hypot2(circleOrigin, d)),
		};

		if (distanceSegmentToPoint > circleRadius)
			return null;

		Vector2 collisionForce = Vector2.Normalize(circleOrigin - d) * (circleRadius - distanceSegmentToPoint);
		Vector2 intersectionPoint = d + collisionForce;
		return new ValueTuple<Vector2, Vector2>(collisionForce, intersectionPoint);

		static Vector2 Project(Vector2 a, Vector2 b)
		{
			float k = Vector2.Dot(a, b) / Vector2.Dot(b, b);
			return new Vector2(k * b.X, k * b.Y);
		}

		static float Hypot2(Vector2 a, Vector2 b)
		{
			return Vector2.Dot(a - b, a - b);
		}
	}

	private static Vector2 GetNormal(Vector2 p1, Vector2 p2)
	{
		Vector2 normal = p2 - p1;
		return Vector2.Normalize(new Vector2(normal.Y, -normal.X));
	}

	private static Vector2 GetCenter(Vector2 p1, Vector2 p2)
	{
		return (p1 + p2) / 2;
	}

	private static (Vector2 CircleOrigin, float CircleRadius) GetBoundingSphere(Vector2 p1, Vector2 p2, Vector2 origin)
	{
		float distSq1 = Vector2.DistanceSquared(origin, p1);
		float distSq2 = Vector2.DistanceSquared(origin, p2);
		float radius = MathF.Sqrt(MathF.Max(distSq1, distSq2));
		return new ValueTuple<Vector2, float>(origin, radius);
	}
}
