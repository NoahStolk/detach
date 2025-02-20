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

	public static bool CircleCastLine(CircleCast circleCast, LineSegment2D line)
	{
		// Check if the start or end points of the circle cast are within the radius of the line segment.
		if (LineCircle(line, new Circle(circleCast.Start, circleCast.Radius)) ||
		    LineCircle(line, new Circle(circleCast.End, circleCast.Radius)))
		{
			return true;
		}

		// Prevent endless for loop.
		if (circleCast.Radius < 0.0001f)
			return false;

		// TODO: This seems to be a bit inefficient.
		// Check if the line segment intersects with the swept area of the circle cast.
		Vector2 direction = Vector2.Normalize(circleCast.End - circleCast.Start);
		float length = Vector2.Distance(circleCast.Start, circleCast.End);
		for (float t = 0; t <= length; t += circleCast.Radius / 2)
		{
			Vector2 point = circleCast.Start + direction * t;
			if (LineCircle(line, new Circle(point, circleCast.Radius)))
			{
				return true;
			}
		}

		return false;
	}
}
