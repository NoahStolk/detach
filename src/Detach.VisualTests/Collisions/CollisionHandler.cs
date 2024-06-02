using Detach.Collisions;
using Detach.Collisions.Primitives;
using Detach.VisualTests.State;
using Rectangle = Detach.Collisions.Primitives.Rectangle;

namespace Detach.VisualTests.Collisions;

public static class CollisionHandler
{
	public static List<CollisionResult> Collisions { get; } = [];

	public static void PerformCollisions()
	{
		Collisions.Clear();

		for (int i = 0; i < Shapes2DState.LineSegments.Count; i++)
		{
			LineSegment2D lineSegment = Shapes2DState.LineSegments[i];

			for (int j = i + 1; j < Shapes2DState.LineSegments.Count; j++)
			{
				LineSegment2D lineSegment2 = Shapes2DState.LineSegments[j];
				Collisions.Add(new CollisionResult(lineSegment, lineSegment2, Geometry2D.LineLine(lineSegment, lineSegment2)));
			}

			foreach (Circle circle in Shapes2DState.Circles)
				Collisions.Add(new CollisionResult(lineSegment, circle, Geometry2D.LineCircle(lineSegment, circle)));

			foreach (Rectangle rectangle in Shapes2DState.Rectangles)
				Collisions.Add(new CollisionResult(lineSegment, rectangle, Geometry2D.LineRectangle(lineSegment, rectangle)));

			foreach (OrientedRectangle orientedRectangle in Shapes2DState.OrientedRectangles)
				Collisions.Add(new CollisionResult(lineSegment, orientedRectangle, Geometry2D.LineOrientedRectangle(lineSegment, orientedRectangle)));
		}

		for (int i = 0; i < Shapes2DState.Circles.Count; i++)
		{
			Circle circle = Shapes2DState.Circles[i];

			for (int j = i + 1; j < Shapes2DState.Circles.Count; j++)
			{
				Circle circle2 = Shapes2DState.Circles[j];
				Collisions.Add(new CollisionResult(circle, circle2, Geometry2D.CircleCircle(circle, circle2)));
			}

			foreach (Rectangle rectangle in Shapes2DState.Rectangles)
				Collisions.Add(new CollisionResult(circle, rectangle, Geometry2D.CircleRectangle(circle, rectangle)));

			foreach (OrientedRectangle orientedRectangle in Shapes2DState.OrientedRectangles)
				Collisions.Add(new CollisionResult(circle, orientedRectangle, Geometry2D.CircleOrientedRectangle(circle, orientedRectangle)));
		}

		for (int i = 0; i < Shapes2DState.Rectangles.Count; i++)
		{
			Rectangle rectangle = Shapes2DState.Rectangles[i];

			for (int j = i + 1; j < Shapes2DState.Rectangles.Count; j++)
			{
				Rectangle rectangle2 = Shapes2DState.Rectangles[j];
				Collisions.Add(new CollisionResult(rectangle, rectangle2, Geometry2D.RectangleRectangle(rectangle, rectangle2)));
			}

			foreach (OrientedRectangle orientedRectangle in Shapes2DState.OrientedRectangles)
				Collisions.Add(new CollisionResult(rectangle, orientedRectangle, Geometry2D.RectangleOrientedRectangleSat(rectangle, orientedRectangle)));
		}

		for (int i = 0; i < Shapes2DState.OrientedRectangles.Count; i++)
		{
			OrientedRectangle orientedRectangle = Shapes2DState.OrientedRectangles[i];

			for (int j = i + 1; j < Shapes2DState.OrientedRectangles.Count; j++)
			{
				OrientedRectangle orientedRectangle2 = Shapes2DState.OrientedRectangles[j];
				Collisions.Add(new CollisionResult(orientedRectangle, orientedRectangle2, Geometry2D.OrientedRectangleOrientedRectangleSat(orientedRectangle, orientedRectangle2)));
			}
		}
	}
}
