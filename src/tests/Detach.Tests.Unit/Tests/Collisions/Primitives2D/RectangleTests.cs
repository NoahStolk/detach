using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Collisions.Primitives2D;

[TestClass]
public sealed class RectangleTests
{
	[TestMethod]
	public void MinMax()
	{
		Rectangle rect = Rectangle.FromTopLeft(new Vector2(0, 0), new Vector2(1, 1));
		Assert.AreEqual(new Vector2(0, 0), rect.GetMin());
		Assert.AreEqual(new Vector2(1, 1), rect.GetMax());

		rect = Rectangle.FromTopLeft(new Vector2(1, 1), new Vector2(1, 1));
		Assert.AreEqual(new Vector2(1, 1), rect.GetMin());
		Assert.AreEqual(new Vector2(2, 2), rect.GetMax());

		rect = Rectangle.FromTopLeft(new Vector2(-1, -1), new Vector2(1, 1));
		Assert.AreEqual(new Vector2(-1, -1), rect.GetMin());
		Assert.AreEqual(new Vector2(0, 0), rect.GetMax());

		rect = Rectangle.FromTopLeft(new Vector2(-1, 1), new Vector2(1, 1));
		Assert.AreEqual(new Vector2(-1, 1), rect.GetMin());
		Assert.AreEqual(new Vector2(0, 2), rect.GetMax());

		rect = Rectangle.FromTopLeft(new Vector2(1, -1), new Vector2(1, 1));
		Assert.AreEqual(new Vector2(1, -1), rect.GetMin());
		Assert.AreEqual(new Vector2(2, 0), rect.GetMax());

		rect = Rectangle.FromTopLeft(new Vector2(0, 0), new Vector2(0, 0));
		Assert.AreEqual(new Vector2(0, 0), rect.GetMin());
		Assert.AreEqual(new Vector2(0, 0), rect.GetMax());
	}

	[TestMethod]
	public void CreateFromMinMax()
	{
		Assert.AreEqual(Rectangle.FromTopLeft(new Vector2(0, 0), new Vector2(1, 1)), Rectangle.FromMinMax(new Vector2(0, 0), new Vector2(1, 1)));
		Assert.AreEqual(Rectangle.FromTopLeft(new Vector2(1, 1), new Vector2(1, 1)), Rectangle.FromMinMax(new Vector2(1, 1), new Vector2(2, 2)));
		Assert.AreEqual(Rectangle.FromTopLeft(new Vector2(-1, -1), new Vector2(1, 1)), Rectangle.FromMinMax(new Vector2(-1, -1), new Vector2(0, 0)));
		Assert.AreEqual(Rectangle.FromTopLeft(new Vector2(-1, 1), new Vector2(1, 1)), Rectangle.FromMinMax(new Vector2(-1, 1), new Vector2(0, 2)));
		Assert.AreEqual(Rectangle.FromTopLeft(new Vector2(1, -1), new Vector2(1, 1)), Rectangle.FromMinMax(new Vector2(1, -1), new Vector2(2, 0)));
		Assert.AreEqual(Rectangle.FromTopLeft(new Vector2(0, 0), new Vector2(0, 0)), Rectangle.FromMinMax(new Vector2(0, 0), new Vector2(0, 0)));
	}

	[TestMethod]
	public void GetInterval()
	{
		Rectangle rect = Rectangle.FromTopLeft(new Vector2(0, 0), new Vector2(1, 1));
		Assert.AreEqual(new Interval(0, 1), rect.GetInterval(new Vector2(1, 0)));
		Assert.AreEqual(new Interval(0, 1), rect.GetInterval(new Vector2(0, 1)));
		Assert.AreEqual(new Interval(0, 2), rect.GetInterval(new Vector2(1, 1)));
		Assert.AreEqual(new Interval(-1, 0), rect.GetInterval(new Vector2(-1, 0)));
		Assert.AreEqual(new Interval(-1, 0), rect.GetInterval(new Vector2(0, -1)));
		Assert.AreEqual(new Interval(-2, 0), rect.GetInterval(new Vector2(-1, -1)));
		Assert.AreEqual(new Interval(-1, 1), rect.GetInterval(new Vector2(1, -1)));
		Assert.AreEqual(new Interval(-1, 1), rect.GetInterval(new Vector2(-1, 1)));
		Assert.AreEqual(new Interval(-1, 1), rect.GetInterval(new Vector2(-1, 1)));
	}

	[TestMethod]
	public void IntersectsSat()
	{
		long allocatedHeapBytes = GC.GetAllocatedBytesForCurrentThread();

		Rectangle rect1 = Rectangle.FromTopLeft(new Vector2(0, 0), new Vector2(1, 1));
		Rectangle rect2 = Rectangle.FromTopLeft(new Vector2(0.5f, 0.25f), new Vector2(0.25f, 1));
		Assert.IsTrue(Geometry2D.RectangleRectangleSat(rect1, rect2));

		Assert.AreEqual(allocatedHeapBytes, GC.GetAllocatedBytesForCurrentThread());
	}
}
