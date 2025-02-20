using Detach.Collisions.Primitives2D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry2D
{
	private static bool OverlapOnAxis(Rectangle rectangle1, Rectangle rectangle2, Vector2 axis)
	{
		Interval interval1 = rectangle1.GetInterval(axis);
		Interval interval2 = rectangle2.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	private static bool OverlapOnAxis(Rectangle rectangle, OrientedRectangle orientedRectangle, Vector2 axis)
	{
		Interval interval1 = rectangle.GetInterval(axis);
		Interval interval2 = orientedRectangle.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}
}
