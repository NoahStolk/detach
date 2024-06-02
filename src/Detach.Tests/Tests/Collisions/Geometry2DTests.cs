using Detach.Collisions;
using Detach.Collisions.Primitives;
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
		Rectangle rectangleCenter = new(new Vector2(0, 0), new Vector2(5, 5));
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

		Rectangle rectangleOffset = new(new Vector2(5, 5), new Vector2(5, 5));
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
}
