using Detach.Collisions;
using Detach.Collisions.Primitives3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Collisions.Primitives3D;

[TestClass]
public sealed class SphereTests
{
	[TestMethod]
	public void SphereConeFrustum()
	{
		// Sphere completely inside the cone frustum
		ConeFrustum coneFrustum = new(new Vector3(0, 0, 0), 2, 1, 5);
		Sphere sphere = new(new Vector3(0, 2, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereConeFrustum(sphere, coneFrustum));

		// Sphere completely containing the cone frustum
		sphere = new Sphere(new Vector3(0, 2.5f, 0), 10f);
		Assert.IsTrue(Geometry3D.SphereConeFrustum(sphere, coneFrustum));

		// Sphere intersecting the bottom circle of the cone frustum
		sphere = new Sphere(new Vector3(0, 0, 0), 2.5f);
		Assert.IsTrue(Geometry3D.SphereConeFrustum(sphere, coneFrustum));

		// Sphere intersecting the top circle of the cone frustum
		sphere = new Sphere(new Vector3(0, 5, 0), 1.5f);
		Assert.IsTrue(Geometry3D.SphereConeFrustum(sphere, coneFrustum));

		// Sphere intersecting the side surface of the cone frustum
		sphere = new Sphere(new Vector3(1, 2.5f, 0), 1.0f);
		Assert.IsTrue(Geometry3D.SphereConeFrustum(sphere, coneFrustum));

		// Sphere completely outside the cone frustum
		sphere = new Sphere(new Vector3(5, 5, 5), 0.5f);
		Assert.IsFalse(Geometry3D.SphereConeFrustum(sphere, coneFrustum));

		// Sphere touching the bottom circle of the cone frustum
		sphere = new Sphere(new Vector3(0, 0, 0), 2.0f);
		Assert.IsTrue(Geometry3D.SphereConeFrustum(sphere, coneFrustum));

		// Sphere touching the top circle of the cone frustum
		sphere = new Sphere(new Vector3(0, 5, 0), 1.0f);
		Assert.IsTrue(Geometry3D.SphereConeFrustum(sphere, coneFrustum));

		// Sphere touching the side surface of the cone frustum
		sphere = new Sphere(new Vector3(1, 2.5f, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereConeFrustum(sphere, coneFrustum));
	}
}
