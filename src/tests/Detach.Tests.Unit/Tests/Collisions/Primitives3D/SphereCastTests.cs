using Detach.Collisions;
using Detach.Collisions.Primitives3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Collisions.Primitives3D;

[TestClass]
public sealed class SphereCastTests
{
	[TestMethod]
	public void SphereCastCylinder()
	{
		Cylinder cylinder = new(new Vector3(0, 5, 0), 2.0f, 10.0f);

		// SphereCast intersecting the bottom circle of the cylinder
		SphereCast sphereCast = new(new Vector3(0, 0, 0), new Vector3(0, 10, 0), 1.0f);
		Assert.IsTrue(Geometry3D.SphereCastCylinder(sphereCast, cylinder));

		// SphereCast intersecting the top circle of the cylinder
		sphereCast = new SphereCast(new Vector3(0, 16, 0), new Vector3(0, 14, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastCylinder(sphereCast, cylinder));

		// SphereCast intersecting the side surface of the cylinder
		sphereCast = new SphereCast(new Vector3(-2, 7.5f, 0), new Vector3(2, 7.5f, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastCylinder(sphereCast, cylinder));

		// SphereCast below the bottom circle of the cylinder
		sphereCast = new SphereCast(new Vector3(-2, 2.5f, 0), new Vector3(2, 2.5f, 0), 0.5f);
		Assert.IsFalse(Geometry3D.SphereCastCylinder(sphereCast, cylinder));
		sphereCast = new SphereCast(new Vector3(0, -1, 0), new Vector3(0, 1, 0), 0.5f);
		Assert.IsFalse(Geometry3D.SphereCastCylinder(sphereCast, cylinder));

		// SphereCast above the top circle of the cylinder
		sphereCast = new SphereCast(new Vector3(-2, 17.5f, 0), new Vector3(2, 17.5f, 0), 0.5f);
		Assert.IsFalse(Geometry3D.SphereCastCylinder(sphereCast, cylinder));
		sphereCast = new SphereCast(new Vector3(0, 20, 0), new Vector3(0, 22, 0), 0.5f);
		Assert.IsFalse(Geometry3D.SphereCastCylinder(sphereCast, cylinder));

		// SphereCast completely outside the cylinder
		sphereCast = new SphereCast(new Vector3(3, 8, 3), new Vector3(4, 9, 4), 0.5f);
		Assert.IsFalse(Geometry3D.SphereCastCylinder(sphereCast, cylinder));

		// SphereCast touching the side surface of the cylinder
		sphereCast = new SphereCast(new Vector3(1.5f, 7.5f, 0), new Vector3(2.5f, 7.5f, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastCylinder(sphereCast, cylinder));

		// SphereCast touching the top circle of the cylinder
		sphereCast = new SphereCast(new Vector3(0, 15.5f, 0), new Vector3(0, 16.5f, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastCylinder(sphereCast, cylinder));

		// SphereCast touching the bottom circle of the cylinder
		sphereCast = new SphereCast(new Vector3(0, 4.5f, 0), new Vector3(0, 3.5f, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastCylinder(sphereCast, cylinder));

		// SphereCast inside the cylinder
		sphereCast = new SphereCast(new Vector3(0, 10f, 0), new Vector3(0, 11f, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastCylinder(sphereCast, cylinder));
	}

	[TestMethod]
	public void SphereCastConeFrustum()
	{
		ConeFrustum coneFrustum = new(Vector3.Zero, 1.0f, 0.5f, 5.0f);

		// SphereCast starting outside and ending inside the cone frustum
		SphereCast sphereCast = new(new Vector3(0, -1, 0), new Vector3(0, 1, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastConeFrustum(sphereCast, coneFrustum));

		// SphereCast completely outside the cone frustum
		sphereCast = new SphereCast(new Vector3(5, 5, 5), new Vector3(6, 6, 6), 0.5f);
		Assert.IsFalse(Geometry3D.SphereCastConeFrustum(sphereCast, coneFrustum));

		// SphereCast starting inside and ending outside the cone frustum
		sphereCast = new SphereCast(new Vector3(0, 1, 0), new Vector3(0, -1, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastConeFrustum(sphereCast, coneFrustum));

		// SphereCast touching the bottom circle of the cone frustum
		sphereCast = new SphereCast(new Vector3(0, -1, 0), Vector3.Zero, 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastConeFrustum(sphereCast, coneFrustum));

		// SphereCast touching the top circle of the cone frustum
		sphereCast = new SphereCast(new Vector3(0, 5, 0), new Vector3(0, 6, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastConeFrustum(sphereCast, coneFrustum));

		// SphereCast touching the side surface of the cone frustum
		sphereCast = new SphereCast(new Vector3(1, 2.5f, 0), new Vector3(1.5f, 2.5f, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastConeFrustum(sphereCast, coneFrustum));

		ConeFrustum wideConeFrustum = new(Vector3.Zero, 4f, 2f, 4f);

		// SphereCast moving up diagonally and ending inside the cone frustum
		sphereCast = new SphereCast(new Vector3(0, 0, -4), new Vector3(0, 2, -2), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastConeFrustum(sphereCast, wideConeFrustum));

		// SphereCast moving up diagonally and ending before the cone frustum
		sphereCast = new SphereCast(new Vector3(0, 0, -8), new Vector3(0, 2, -6), 0.5f);
		Assert.IsFalse(Geometry3D.SphereCastConeFrustum(sphereCast, wideConeFrustum));
	}
}
