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
				Collisions.Add(new CollisionResult(lineSegment, lineSegment2, i, j, Geometry2D.LineLine(lineSegment, lineSegment2)));
			}

			for (int j = 0; j < Shapes2DState.Circles.Count; j++)
			{
				Circle circle = Shapes2DState.Circles[j];
				Collisions.Add(new CollisionResult(lineSegment, circle, i, j, Geometry2D.LineCircle(lineSegment, circle)));
			}

			for (int j = 0; j < Shapes2DState.Rectangles.Count; j++)
			{
				Rectangle rectangle = Shapes2DState.Rectangles[j];
				Collisions.Add(new CollisionResult(lineSegment, rectangle, i, j, Geometry2D.LineRectangle(lineSegment, rectangle)));
			}

			for (int j = 0; j < Shapes2DState.OrientedRectangles.Count; j++)
			{
				OrientedRectangle orientedRectangle = Shapes2DState.OrientedRectangles[j];
				Collisions.Add(new CollisionResult(lineSegment, orientedRectangle, i, j, Geometry2D.LineOrientedRectangle(lineSegment, orientedRectangle)));
			}
		}

		for (int i = 0; i < Shapes2DState.Circles.Count; i++)
		{
			Circle circle = Shapes2DState.Circles[i];

			for (int j = i + 1; j < Shapes2DState.Circles.Count; j++)
			{
				Circle circle2 = Shapes2DState.Circles[j];
				Collisions.Add(new CollisionResult(circle, circle2, i, j, Geometry2D.CircleCircle(circle, circle2)));
			}

			for (int j = 0; j < Shapes2DState.Rectangles.Count; j++)
			{
				Rectangle rectangle = Shapes2DState.Rectangles[j];
				Collisions.Add(new CollisionResult(circle, rectangle, i, j, Geometry2D.CircleRectangle(circle, rectangle)));
			}

			for (int j = 0; j < Shapes2DState.OrientedRectangles.Count; j++)
			{
				OrientedRectangle orientedRectangle = Shapes2DState.OrientedRectangles[j];
				Collisions.Add(new CollisionResult(circle, orientedRectangle, i, j, Geometry2D.CircleOrientedRectangle(circle, orientedRectangle)));
			}
		}

		for (int i = 0; i < Shapes2DState.Rectangles.Count; i++)
		{
			Rectangle rectangle = Shapes2DState.Rectangles[i];

			for (int j = i + 1; j < Shapes2DState.Rectangles.Count; j++)
			{
				Rectangle rectangle2 = Shapes2DState.Rectangles[j];
				Collisions.Add(new CollisionResult(rectangle, rectangle2, i, j, Geometry2D.RectangleRectangle(rectangle, rectangle2)));
			}

			for (int j = 0; j < Shapes2DState.OrientedRectangles.Count; j++)
			{
				OrientedRectangle orientedRectangle = Shapes2DState.OrientedRectangles[j];
				Collisions.Add(new CollisionResult(rectangle, orientedRectangle, i, j, Geometry2D.RectangleOrientedRectangleSat(rectangle, orientedRectangle)));
			}
		}

		for (int i = 0; i < Shapes2DState.OrientedRectangles.Count; i++)
		{
			OrientedRectangle orientedRectangle = Shapes2DState.OrientedRectangles[i];

			for (int j = i + 1; j < Shapes2DState.OrientedRectangles.Count; j++)
			{
				OrientedRectangle orientedRectangle2 = Shapes2DState.OrientedRectangles[j];
				Collisions.Add(new CollisionResult(orientedRectangle, orientedRectangle2, i, j, Geometry2D.OrientedRectangleOrientedRectangleSat(orientedRectangle, orientedRectangle2)));
			}
		}
	}
}
