using Detach.Collisions.Primitives2D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry2D
{
	public static bool CircleCastPoint(CircleCast circleCast, Vector2 point)
	{
		if (PointInCircle(point, new Circle(circleCast.Start, circleCast.Radius)))
			return true;

		if (PointInCircle(point, new Circle(circleCast.End, circleCast.Radius)))
			return true;

		Vector2 closestPoint = ClosestPointOnLine(point, new LineSegment2D(circleCast.Start, circleCast.End));
		return PointInCircle(point, new Circle(closestPoint, circleCast.Radius));
	}
}
