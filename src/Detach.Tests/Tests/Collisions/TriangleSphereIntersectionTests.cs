using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Numerics;

namespace Detach.Tests.Tests.Collisions;

[TestClass]
public class TriangleSphereIntersectionTests
{
	[TestMethod]
	public void TestCollisionWithOriginTriangleFacingUp()
	{
		Triangle triangleAtOriginFacingUp = new(new Vector3(-1.5f, 0, -0.5f), new Vector3(0.5f, 0, 1.5f), new Vector3(0.5f, 0, -0.5f));

		// No collision.
		TestCollision(triangleAtOriginFacingUp, new Sphere(new Vector3(2.00f, 0.00f, 0.00f), 0.75f), null);
		TestCollision(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 1.75f, 0.00f), 0.75f), null);

		// Surface collision at 0,0,0.
		TestCollision(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 0.75f, 0.00f), 0.75f), (default, default));

		// Spheres along the Y axis. Intersection point is at 0,0,0.
		TestCollision(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 0.50f, 0.00f), 1.00f), (new Vector3(0.00f, 0.50f, 0.00f), default));
		TestCollision(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 0.50f, 0.00f), 0.75f), (new Vector3(0.00f, 0.25f, 0.00f), default));
		TestCollision(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 0.75f, 0.00f), 1.00f), (new Vector3(0.00f, 0.25f, 0.00f), default));
		TestCollision(triangleAtOriginFacingUp, new Sphere(new Vector3(0.00f, 0.99f, 0.00f), 1.00f), (new Vector3(0.00f, 0.01f, 0.00f), default));

		// Offset the sphere at the X axis. Should still be inside the triangle.
		TestCollision(triangleAtOriginFacingUp, new Sphere(new Vector3(0.45f, 0.50f, 0.00f), 1.00f), (new Vector3(0.00f, 0.50f, 0.00f), new Vector3(0.45f, 0.00f, 0.00f)));
		TestCollision(triangleAtOriginFacingUp, new Sphere(new Vector3(0.50f, 0.50f, 0.00f), 1.00f), (new Vector3(0.00f, 0.50f, 0.00f), new Vector3(0.50f, 0.00f, 0.00f)));

		// Should not be inside the triangle anymore, so perform edge tests.
		TestCollision(triangleAtOriginFacingUp, new Sphere(new Vector3(0.55f, 0.50f, 0.00f), 1.00f), (new Vector3(0.00f, 0.497506201f, 0.00f), new Vector3(0.50f, 0.00f, 0.00f)));
	}

	private static void TestCollision(Triangle triangle, Sphere sphere, (Vector3 CollisionForce, Vector3 IntersectionPoint)? expectedIntersection)
	{
		(Vector3 CollisionForce, Vector3 IntersectionPoint)? intersection = Detach.Collisions.Triangle.IntersectsSphere(triangle.P1, triangle.P2, triangle.P3, sphere.Origin, sphere.Radius);

		if (!expectedIntersection.HasValue)
		{
			Assert.IsNull(intersection);
			return;
		}

		Assert.IsNotNull(intersection);

		AssertVector3Equality(expectedIntersection.Value.CollisionForce, intersection.Value.CollisionForce);
		AssertVector3Equality(expectedIntersection.Value.IntersectionPoint, intersection.Value.IntersectionPoint);

		static void AssertVector3Equality(Vector3 expected, Vector3 actual)
		{
			const float delta = 0.00001f;
			string message = $"Expected {expected.ToString("N4", CultureInfo.InvariantCulture)}, actual {actual.ToString("N4", CultureInfo.InvariantCulture)}";

			Assert.AreEqual(expected.X, actual.X, delta, message);
			Assert.AreEqual(expected.Y, actual.Y, delta, message);
			Assert.AreEqual(expected.Z, actual.Z, delta, message);
		}
	}

	private readonly record struct Triangle(Vector3 P1, Vector3 P2, Vector3 P3);

	private readonly record struct Sphere(Vector3 Origin, float Radius);
}
