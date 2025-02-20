using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Tests.Collisions;

[TestClass]
public class Geometry2DTests
{
	[TestMethod]
	public void PointOnLine()
	{
		// Horizontal line
		LineSegment2D lineHorizontal = new(new Vector2(0, 0), new Vector2(5, 0));
		Assert.IsTrue(Geometry2D.PointOnLine(new Vector2(0, 0), lineHorizontal));
		Assert.IsTrue(Geometry2D.PointOnLine(new Vector2(1, 0), lineHorizontal));
		Assert.IsTrue(Geometry2D.PointOnLine(new Vector2(5, 0), lineHorizontal));
		Assert.IsTrue(Geometry2D.PointOnLine(new Vector2(6, 0), lineHorizontal));
		Assert.IsFalse(Geometry2D.PointOnLine(new Vector2(5, 1), lineHorizontal));
		Assert.IsFalse(Geometry2D.PointOnLine(new Vector2(0, 1), lineHorizontal));
		Assert.IsFalse(Geometry2D.PointOnLine(new Vector2(0, -1), lineHorizontal));
		Assert.IsFalse(Geometry2D.PointOnLine(new Vector2(5, -1), lineHorizontal));

		// Vertical line
		// TODO: Fix infinite slope.
		// LineSegment2D lineVertical = new(new(0, 0), new(0, 5));
		// Assert.IsTrue(Geometry2D.PointOnLine(new(0, 0), lineVertical));
		// Assert.IsTrue(Geometry2D.PointOnLine(new(0, 1), lineVertical));
		// Assert.IsTrue(Geometry2D.PointOnLine(new(0, 5), lineVertical));
		// Assert.IsTrue(Geometry2D.PointOnLine(new(0, 6), lineVertical));
		// Assert.IsFalse(Geometry2D.PointOnLine(new(1, 5), lineVertical));
		// Assert.IsFalse(Geometry2D.PointOnLine(new(1, 0), lineVertical));
		// Assert.IsFalse(Geometry2D.PointOnLine(new(-1, 0), lineVertical));
		// Assert.IsFalse(Geometry2D.PointOnLine(new(-1, 5), lineVertical));

		// Diagonal line
		LineSegment2D lineDiagonal = new(new Vector2(-1, -1), new Vector2(6, 6));
		Assert.IsTrue(Geometry2D.PointOnLine(new Vector2(-1, -1), lineDiagonal));
		Assert.IsTrue(Geometry2D.PointOnLine(new Vector2(0, 0), lineDiagonal));
		Assert.IsTrue(Geometry2D.PointOnLine(new Vector2(6, 6), lineDiagonal));
		Assert.IsTrue(Geometry2D.PointOnLine(new Vector2(7, 7), lineDiagonal));
		Assert.IsFalse(Geometry2D.PointOnLine(new Vector2(6, 5), lineDiagonal));
		Assert.IsFalse(Geometry2D.PointOnLine(new Vector2(0, 5), lineDiagonal));
		Assert.IsFalse(Geometry2D.PointOnLine(new Vector2(5, 6), lineDiagonal));
		Assert.IsFalse(Geometry2D.PointOnLine(new Vector2(0, -0.5f), lineDiagonal));
	}

	[TestMethod]
	public void PointInCircle()
	{
		Circle circleCenter = new(new Vector2(0, 0), 5);
		Assert.IsTrue(Geometry2D.PointInCircle(new Vector2(0, 0), circleCenter));
		Assert.IsTrue(Geometry2D.PointInCircle(new Vector2(3, 4), circleCenter));
		Assert.IsTrue(Geometry2D.PointInCircle(new Vector2(5, 0), circleCenter));
		Assert.IsTrue(Geometry2D.PointInCircle(new Vector2(0, 5), circleCenter));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(6, 0), circleCenter));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(0, 6), circleCenter));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(6, 6), circleCenter));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(0, -6), circleCenter));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(-6, 0), circleCenter));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(-6, -6), circleCenter));

		Circle circleOffset = new(new Vector2(5, 5), 5);
		Assert.IsTrue(Geometry2D.PointInCircle(new Vector2(5, 5), circleOffset));
		Assert.IsTrue(Geometry2D.PointInCircle(new Vector2(8, 9), circleOffset));
		Assert.IsTrue(Geometry2D.PointInCircle(new Vector2(10, 5), circleOffset));
		Assert.IsTrue(Geometry2D.PointInCircle(new Vector2(5, 10), circleOffset));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(11, 5), circleOffset));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(5, 11), circleOffset));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(11, 11), circleOffset));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(5, -1), circleOffset));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(-1, 5), circleOffset));
		Assert.IsFalse(Geometry2D.PointInCircle(new Vector2(-1, -1), circleOffset));
	}

	[TestMethod]
	public void PointInRectangle()
	{
		Rectangle rectangleCenter = Rectangle.FromTopLeft(new Vector2(0, 0), new Vector2(5, 5));
		Assert.IsTrue(Geometry2D.PointInRectangle(new Vector2(0, 0), rectangleCenter));
		Assert.IsTrue(Geometry2D.PointInRectangle(new Vector2(3, 3), rectangleCenter));
		Assert.IsTrue(Geometry2D.PointInRectangle(new Vector2(5, 5), rectangleCenter));
		Assert.IsTrue(Geometry2D.PointInRectangle(new Vector2(0, 5), rectangleCenter));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(6, 0), rectangleCenter));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(0, 6), rectangleCenter));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(6, 6), rectangleCenter));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(0, -1), rectangleCenter));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(-1, 0), rectangleCenter));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(-1, -1), rectangleCenter));

		Rectangle rectangleOffset = Rectangle.FromTopLeft(new Vector2(5, 5), new Vector2(5, 5));
		Assert.IsTrue(Geometry2D.PointInRectangle(new Vector2(5, 5), rectangleOffset));
		Assert.IsTrue(Geometry2D.PointInRectangle(new Vector2(8, 9), rectangleOffset));
		Assert.IsTrue(Geometry2D.PointInRectangle(new Vector2(10, 5), rectangleOffset));
		Assert.IsTrue(Geometry2D.PointInRectangle(new Vector2(5, 10), rectangleOffset));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(11, 5), rectangleOffset));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(5, 11), rectangleOffset));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(11, 11), rectangleOffset));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(5, -1), rectangleOffset));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(-1, 5), rectangleOffset));
		Assert.IsFalse(Geometry2D.PointInRectangle(new Vector2(-1, -1), rectangleOffset));
	}

	[TestMethod]
	public void PointInOrientedRectangle()
	{
		OrientedRectangle orientedRectangle = new(new Vector2(0, 0), new Vector2(5, 5), MathF.PI / 4);
		Assert.IsTrue(Geometry2D.PointInOrientedRectangle(new Vector2(0, 0), orientedRectangle));
		Assert.IsTrue(Geometry2D.PointInOrientedRectangle(new Vector2(3, 3), orientedRectangle));
		//Assert.IsTrue(Geometry2D.PointInOrientedRectangle(new Vector2(5, 5), orientedRectangle));
		Assert.IsTrue(Geometry2D.PointInOrientedRectangle(new Vector2(0, 5), orientedRectangle));
		//Assert.IsFalse(Geometry2D.PointInOrientedRectangle(new Vector2(6, 0), orientedRectangle));
		//Assert.IsFalse(Geometry2D.PointInOrientedRectangle(new Vector2(0, 6), orientedRectangle));
		Assert.IsFalse(Geometry2D.PointInOrientedRectangle(new Vector2(6, 6), orientedRectangle));
		//Assert.IsFalse(Geometry2D.PointInOrientedRectangle(new Vector2(0, -1), orientedRectangle));
		//Assert.IsFalse(Geometry2D.PointInOrientedRectangle(new Vector2(-1, 0), orientedRectangle));
		//Assert.IsFalse(Geometry2D.PointInOrientedRectangle(new Vector2(-1, -1), orientedRectangle));
	}

	[TestMethod]
	public void PointInTriangle()
	{
		Triangle2D triangle = new(new Vector2(0, 0), new Vector2(5, 0), new Vector2(0, 5));
		Assert.IsTrue(Geometry2D.PointInTriangle(new Vector2(0.1f, 0.1f), triangle));
		Assert.IsTrue(Geometry2D.PointInTriangle(new Vector2(2, 2), triangle));
		Assert.IsTrue(Geometry2D.PointInTriangle(new Vector2(0.1f, 4.8f), triangle));
		Assert.IsFalse(Geometry2D.PointInTriangle(new Vector2(6, 0), triangle));
		Assert.IsFalse(Geometry2D.PointInTriangle(new Vector2(0, 6), triangle));
		Assert.IsFalse(Geometry2D.PointInTriangle(new Vector2(6, 6), triangle));
		Assert.IsFalse(Geometry2D.PointInTriangle(new Vector2(0, -1), triangle));
		Assert.IsFalse(Geometry2D.PointInTriangle(new Vector2(-1, 0), triangle));
		Assert.IsFalse(Geometry2D.PointInTriangle(new Vector2(-1, -1), triangle));
	}

	[TestMethod]
	public void TwoRectanglesColliding()
	{
		Rectangle rectangle0 = Rectangle.FromTopLeft(new Vector2(155f, 80f), new Vector2(88f, 121f));
		Rectangle rectangle1 = Rectangle.FromTopLeft(new Vector2(211f, 156f), new Vector2(67f, 92f));
		Assert.IsTrue(Geometry2D.RectangleRectangle(rectangle0, rectangle1));
	}

	[TestMethod]
	public void TwoRectanglesNotColliding()
	{
		Rectangle rectangle0 = Rectangle.FromTopLeft(new Vector2(155f, 80f), new Vector2(88f, 75f));
		Rectangle rectangle1 = Rectangle.FromTopLeft(new Vector2(211f, 156f), new Vector2(67f, 102f));
		Assert.IsFalse(Geometry2D.RectangleRectangle(rectangle0, rectangle1));
	}

	[TestMethod]
	public void TwoOrientedRectanglesColliding()
	{
		OrientedRectangle orientedRectangle0 = new(new Vector2(182.5f, 180f), new Vector2(59.5f, 57f), 0.76394224f);
		OrientedRectangle orientedRectangle1 = new(new Vector2(152.5f, 78.5f), new Vector2(71.5f, 13f), 0.31f);
		Assert.IsTrue(Geometry2D.OrientedRectangleOrientedRectangleSat(orientedRectangle0, orientedRectangle1));
	}

	[TestMethod]
	public void TwoOrientedRectanglesNotColliding()
	{
		OrientedRectangle orientedRectangle0 = new(new Vector2(182.5f, 180f), new Vector2(59.5f, 57f), 0.76394224f);
		OrientedRectangle orientedRectangle1 = new(new Vector2(152.5f, 78.5f), new Vector2(71.5f, 13f), 0.13f);
		Assert.IsFalse(Geometry2D.OrientedRectangleOrientedRectangleSat(orientedRectangle0, orientedRectangle1));
	}

	#region Various

	[TestMethod]
	public void Various()
	{
		LineSegment2D line0 = new(new Vector2(129f, 182f), new Vector2(241f, 288f));
		LineSegment2D line1 = new(new Vector2(229f, 205f), new Vector2(119f, 280f));
		Circle circle0 = new(new Vector2(188f, 359.5f), 66.83001f);
		Rectangle rectangle0 = Rectangle.FromTopLeft(new Vector2(76f, 213f), new Vector2(146f, 125f));
		Rectangle rectangle1 = Rectangle.FromTopLeft(new Vector2(216f, 390f), new Vector2(69f, 54f));
		Assert.IsTrue(Geometry2D.LineLine(line0, line1));
		Assert.IsFalse(Geometry2D.LineCircle(line0, circle0));
		Assert.IsTrue(Geometry2D.LineRectangle(line0, rectangle0));
		Assert.IsFalse(Geometry2D.LineRectangle(line0, rectangle1));
		Assert.IsFalse(Geometry2D.LineCircle(line1, circle0));
		Assert.IsTrue(Geometry2D.LineRectangle(line1, rectangle0));
		Assert.IsFalse(Geometry2D.LineRectangle(line1, rectangle1));
		Assert.IsTrue(Geometry2D.CircleRectangle(circle0, rectangle0));
		Assert.IsTrue(Geometry2D.CircleRectangle(circle0, rectangle1));
		Assert.IsFalse(Geometry2D.RectangleRectangle(rectangle0, rectangle1));
	}

	[TestMethod]
	public void ThreeCircles()
	{
		Circle circle0 = new(new Vector2(196.5f, 225.5f), 66.772f);
		Circle circle1 = new(new Vector2(236f, 147.5f), 67.2328f);
		Circle circle2 = new(new Vector2(287f, 230.5f), 18.309834f);
		Assert.IsTrue(Geometry2D.CircleCircle(circle0, circle1));
		Assert.IsFalse(Geometry2D.CircleCircle(circle0, circle2));
		Assert.IsFalse(Geometry2D.CircleCircle(circle1, circle2));
	}

	[TestMethod]
	public void FourCircles()
	{
		Circle circle0 = new(new Vector2(238f, 177.5f), 62.516f);
		Circle circle1 = new(new Vector2(132f, 100.5f), 68f);
		Circle circle2 = new(new Vector2(381.5f, 82.5f), 60.91387f);
		Circle circle3 = new(new Vector2(465f, 171.5f), 62f);
		Assert.IsFalse(Geometry2D.CircleCircle(circle0, circle1));
		Assert.IsFalse(Geometry2D.CircleCircle(circle0, circle2));
		Assert.IsFalse(Geometry2D.CircleCircle(circle0, circle3));
		Assert.IsFalse(Geometry2D.CircleCircle(circle1, circle2));
		Assert.IsFalse(Geometry2D.CircleCircle(circle1, circle3));
		Assert.IsTrue(Geometry2D.CircleCircle(circle2, circle3));
	}

	#endregion Various

	[TestMethod]
	public void CircleCastLineSegmentWithZeroRadius()
	{
		CircleCast circleCast = new(Vector2.Zero, new Vector2(0, 3), 0);

		LineSegment2D lineSegment = new(new Vector2(-10, -10), new Vector2(-9, -9));
		Assert.IsFalse(Geometry2D.CircleCastLine(circleCast, lineSegment));
	}
}
