using Detach.Collisions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests.Collisions.Primitives;

[TestClass]
public class RectangleTests
{
	[TestMethod]
	public void MinMax()
	{
		Rectangle rect = new(new(0, 0), new(1, 1));
		Assert.AreEqual(new(0, 0), rect.GetMin());
		Assert.AreEqual(new(1, 1), rect.GetMax());

		rect = new(new(1, 1), new(1, 1));
		Assert.AreEqual(new(1, 1), rect.GetMin());
		Assert.AreEqual(new(2, 2), rect.GetMax());

		rect = new(new(-1, -1), new(1, 1));
		Assert.AreEqual(new(-1, -1), rect.GetMin());
		Assert.AreEqual(new(0, 0), rect.GetMax());

		rect = new(new(-1, 1), new(1, 1));
		Assert.AreEqual(new(-1, 1), rect.GetMin());
		Assert.AreEqual(new(0, 2), rect.GetMax());

		rect = new(new(1, -1), new(1, 1));
		Assert.AreEqual(new(1, -1), rect.GetMin());
		Assert.AreEqual(new(2, 0), rect.GetMax());

		rect = new(new(0, 0), new(0, 0));
		Assert.AreEqual(new(0, 0), rect.GetMin());
		Assert.AreEqual(new(0, 0), rect.GetMax());
	}

	[TestMethod]
	public void CreateFromMinMax()
	{
		Assert.AreEqual(new(new(0, 0), new(1, 1)), Rectangle.FromMinMax(new(0, 0), new(1, 1)));
		Assert.AreEqual(new(new(1, 1), new(1, 1)), Rectangle.FromMinMax(new(1, 1), new(2, 2)));
		Assert.AreEqual(new(new(-1, -1), new(1, 1)), Rectangle.FromMinMax(new(-1, -1), new(0, 0)));
		Assert.AreEqual(new(new(-1, 1), new(1, 1)), Rectangle.FromMinMax(new(-1, 1), new(0, 2)));
		Assert.AreEqual(new(new(1, -1), new(1, 1)), Rectangle.FromMinMax(new(1, -1), new(2, 0)));
		Assert.AreEqual(new(new(0, 0), new(0, 0)), Rectangle.FromMinMax(new(0, 0), new(0, 0)));
	}

	[TestMethod]
	public void GetInterval()
	{
		Rectangle rect = new(new(0, 0), new(1, 1));
		Assert.AreEqual(new(0, 1), rect.GetInterval(new(1, 0)));
		Assert.AreEqual(new(0, 1), rect.GetInterval(new(0, 1)));
		Assert.AreEqual(new(0, 2), rect.GetInterval(new(1, 1)));
		Assert.AreEqual(new(-1, 0), rect.GetInterval(new(-1, 0)));
		Assert.AreEqual(new(-1, 0), rect.GetInterval(new(0, -1)));
		Assert.AreEqual(new(-2, 0), rect.GetInterval(new(-1, -1)));
		Assert.AreEqual(new(-1, 1), rect.GetInterval(new(1, -1)));
		Assert.AreEqual(new(-1, 1), rect.GetInterval(new(-1, 1)));
		Assert.AreEqual(new(-1, 1), rect.GetInterval(new(-1, 1)));
	}
}
