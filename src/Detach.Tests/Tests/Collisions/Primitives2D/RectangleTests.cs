using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Tests.Collisions.Primitives2D;

[TestClass]
public class RectangleTests
{
	[TestMethod]
	public void MinMax()
	{
		Rectangle rect = new(new Vector2(0, 0), new Vector2(1, 1));
		Assert.AreEqual(new Vector2(0, 0), rect.GetMin());
		Assert.AreEqual(new Vector2(1, 1), rect.GetMax());

		rect = new Rectangle(new Vector2(1, 1), new Vector2(1, 1));
		Assert.AreEqual(new Vector2(1, 1), rect.GetMin());
		Assert.AreEqual(new Vector2(2, 2), rect.GetMax());

		rect = new Rectangle(new Vector2(-1, -1), new Vector2(1, 1));
		Assert.AreEqual(new Vector2(-1, -1), rect.GetMin());
		Assert.AreEqual(new Vector2(0, 0), rect.GetMax());

		rect = new Rectangle(new Vector2(-1, 1), new Vector2(1, 1));
		Assert.AreEqual(new Vector2(-1, 1), rect.GetMin());
		Assert.AreEqual(new Vector2(0, 2), rect.GetMax());

		rect = new Rectangle(new Vector2(1, -1), new Vector2(1, 1));
		Assert.AreEqual(new Vector2(1, -1), rect.GetMin());
		Assert.AreEqual(new Vector2(2, 0), rect.GetMax());

		rect = new Rectangle(new Vector2(0, 0), new Vector2(0, 0));
		Assert.AreEqual(new Vector2(0, 0), rect.GetMin());
		Assert.AreEqual(new Vector2(0, 0), rect.GetMax());
	}

	[TestMethod]
	public void CreateFromMinMax()
	{
		Assert.AreEqual(new Rectangle(new Vector2(0, 0), new Vector2(1, 1)), Rectangle.FromMinMax(new Vector2(0, 0), new Vector2(1, 1)));
		Assert.AreEqual(new Rectangle(new Vector2(1, 1), new Vector2(1, 1)), Rectangle.FromMinMax(new Vector2(1, 1), new Vector2(2, 2)));
		Assert.AreEqual(new Rectangle(new Vector2(-1, -1), new Vector2(1, 1)), Rectangle.FromMinMax(new Vector2(-1, -1), new Vector2(0, 0)));
		Assert.AreEqual(new Rectangle(new Vector2(-1, 1), new Vector2(1, 1)), Rectangle.FromMinMax(new Vector2(-1, 1), new Vector2(0, 2)));
		Assert.AreEqual(new Rectangle(new Vector2(1, -1), new Vector2(1, 1)), Rectangle.FromMinMax(new Vector2(1, -1), new Vector2(2, 0)));
		Assert.AreEqual(new Rectangle(new Vector2(0, 0), new Vector2(0, 0)), Rectangle.FromMinMax(new Vector2(0, 0), new Vector2(0, 0)));
	}

	[TestMethod]
	public void GetInterval()
	{
		Rectangle rect = new(new Vector2(0, 0), new Vector2(1, 1));
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
}
