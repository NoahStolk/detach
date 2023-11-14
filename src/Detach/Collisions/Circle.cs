using System.Numerics;

namespace Detach.Collisions;

public static class Circle
{
	public static bool ContainsPoint(Vector2 circleOrigin, float circleRadius, Vector2 point)
	{
		return Vector2.DistanceSquared(circleOrigin, point) <= circleRadius * circleRadius;
	}

	public static bool IntersectsOrContainsCircle(Vector2 circleOriginA, float circleRadiusA, Vector2 circleOriginB, float circleRadiusB)
	{
		return Vector2.DistanceSquared(circleOriginA, circleOriginB) <= (circleRadiusA + circleRadiusB) * (circleRadiusA + circleRadiusB);
	}
}
