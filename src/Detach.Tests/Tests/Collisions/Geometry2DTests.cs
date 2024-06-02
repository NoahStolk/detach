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
}
