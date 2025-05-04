using Detach.Collisions.Primitives3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Collisions.Primitives3D;

[TestClass]
public sealed class StandingCapsuleTests
{
	[TestMethod]
	public void GetBoundsOrigin()
	{
		StandingCapsule capsule = new(new Vector3(0, 0, 0), 1, 4);
		Vector3 min = new(-1, -1, -1);
		Vector3 max = new(1, 3, 1);

		Aabb expectedBounds = Aabb.FromMinMax(min, max);
		Aabb actualBounds = capsule.GetBounds();
		Assert.AreEqual(expectedBounds, actualBounds);
	}

	[TestMethod]
	public void GetBoundsOffset()
	{
		StandingCapsule capsule = new(new Vector3(1, 2, 3), 1, 4);
		Vector3 min = new(0, 1, 2);
		Vector3 max = new(2, 5, 4);

		Aabb expectedBounds = Aabb.FromMinMax(min, max);
		Aabb actualBounds = capsule.GetBounds();
		Assert.AreEqual(expectedBounds, actualBounds);
	}
}
