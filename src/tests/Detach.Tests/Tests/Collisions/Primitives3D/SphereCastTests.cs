using Detach.Collisions;
using Detach.Collisions.Primitives3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Tests.Collisions.Primitives3D;

[TestClass]
public sealed class SphereCastTests
{
	[TestMethod]
	public void SphereCastConeFrustum()
	{
		ConeFrustum coneFrustum = new(new Vector3(0, 0, 0), 1.0f, 0.5f, 5.0f);

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
		sphereCast = new SphereCast(new Vector3(0, -1, 0), new Vector3(0, 0, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastConeFrustum(sphereCast, coneFrustum));

		// SphereCast touching the top circle of the cone frustum
		sphereCast = new SphereCast(new Vector3(0, 5, 0), new Vector3(0, 6, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastConeFrustum(sphereCast, coneFrustum));

		// SphereCast touching the side surface of the cone frustum
		sphereCast = new SphereCast(new Vector3(1, 2.5f, 0), new Vector3(1.5f, 2.5f, 0), 0.5f);
		Assert.IsTrue(Geometry3D.SphereCastConeFrustum(sphereCast, coneFrustum));
	}
}
