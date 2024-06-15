using Detach.Collisions;
using Detach.Collisions.Primitives3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Tests.Collisions;

[TestClass]
public class Geometry3DTests
{
	[TestMethod]
	public void TriangleSphere()
	{
		Triangle triangleAtOriginFacingUp = new(new Vector3(-1.5f, 0, -0.5f), new Vector3(0.5f, 0, 1.5f), new Vector3(0.5f, 0, -0.5f));

		// No collision.
		Assert.IsFalse(Geometry3D.TriangleSphere(triangleAtOriginFacingUp, new Sphere(new Vector3(2.00f, 0.00f, 0.00f), 0.75f)));
		Assert.IsFalse(Geometry3D.TriangleSphere(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 1.75f, 0.00f), 0.75f)));

		// Surface collision at 0,0,0.
		Assert.IsTrue(Geometry3D.TriangleSphere(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 0.75f, 0.00f), 0.75f)));

		// Spheres along the Y axis. Intersection point is at 0,0,0.
		Assert.IsTrue(Geometry3D.TriangleSphere(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 0.50f, 0.00f), 1.00f)));
		Assert.IsTrue(Geometry3D.TriangleSphere(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 0.50f, 0.00f), 0.75f)));
		Assert.IsTrue(Geometry3D.TriangleSphere(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 0.75f, 0.00f), 1.00f)));
		Assert.IsTrue(Geometry3D.TriangleSphere(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 0.99f, 0.00f), 1.00f)));

		// Offset the sphere at the X axis. Should still be inside the triangle.
		Assert.IsTrue(Geometry3D.TriangleSphere(triangleAtOriginFacingUp, new Sphere(new Vector3(0.45f, 0.50f, 0.00f), 1.00f)));
		Assert.IsTrue(Geometry3D.TriangleSphere(triangleAtOriginFacingUp, new Sphere(new Vector3(0.50f, 0.50f, 0.00f), 1.00f)));

		// Should not be inside the triangle anymore, so perform edge tests.
		Assert.IsTrue(Geometry3D.TriangleSphere(triangleAtOriginFacingUp, new Sphere(new Vector3(0.55f, 0.50f, 0.00f), 1.00f)));
	}
}
