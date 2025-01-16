using Detach.Collisions.Primitives3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Tests.Collisions.Primitives3D;

[TestClass]
public sealed class AabbTests
{
	[TestMethod]
	public void GetMinMax()
	{
		Vector3 origin = new(0, 0, 0);
		Vector3 size = new(1, 1, 1);
		Aabb aabb = new(origin, size);

		Vector3 min = aabb.GetMin();
		Vector3 max = aabb.GetMax();

		Assert.AreEqual(new Vector3(-0.5f, -0.5f, -0.5f), min);
		Assert.AreEqual(new Vector3(0.5f, 0.5f, 0.5f), max);
	}

	[TestMethod]
	public void FromMinMax()
	{
		Vector3 min = new(-0.5f, -0.5f, -0.5f);
		Vector3 max = new(0.5f, 0.5f, 0.5f);
		Aabb aabb = Aabb.FromMinMax(min, max);

		Assert.AreEqual(new Vector3(0, 0, 0), aabb.Center);
		Assert.AreEqual(new Vector3(1, 1, 1), aabb.Size);
		Assert.AreEqual(min, aabb.GetMin());
		Assert.AreEqual(max, aabb.GetMax());
	}
}
