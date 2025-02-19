using Detach.Collisions;
using Detach.Collisions.Primitives3D;
using Detach.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Tests.Collisions;

[TestClass]
public class Geometry3DTests
{
	[TestMethod]
	public void TriangleSphere()
	{
		Triangle3D triangleAtOriginFacingUp = new(new Vector3(-1.5f, 0, -0.5f), new Vector3(0.5f, 0, 1.5f), new Vector3(0.5f, 0, -0.5f));

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

	[TestMethod]
	public void CreatePlaneFromTriangle()
	{
		// Test with 1000 random triangles.
		for (int i = 0; i < 1000; i++)
		{
			Triangle3D triangle = new()
			{
				A = Random.Shared.RandomVector3(-10, 10),
				B = Random.Shared.RandomVector3(-10, 10),
				C = Random.Shared.RandomVector3(-10, 10),
			};
			Plane plane = Geometry3D.CreatePlaneFromTriangle(triangle);
			Plane planeBcl = Plane.CreateFromVertices(triangle.A, triangle.B, triangle.C);
			AssertionUtils.AreEqual(plane.Normal, planeBcl.Normal);
			Assert.AreEqual(plane.D, -planeBcl.D);
		}
	}

	[TestMethod]
	public void RaycastWithTriangle()
	{
		Triangle3D triangleAtOriginFacingUp = new()
		{
			A = new Vector3(-1.5f, 0, -0.5f),
			B = new Vector3(+0.5f, 0, +1.5f),
			C = new Vector3(+0.5f, 0, -0.5f),
		};

		Vector3 rayOrigin = new(0, 1, 0);
		Ray rayShootingDown = new(rayOrigin, -Vector3.UnitY);
		Ray rayShootingUp = new(rayOrigin, Vector3.UnitY);
		Ray rayShootingRight = new(rayOrigin, Vector3.UnitX);
		Ray rayShootingLeft = new(rayOrigin, -Vector3.UnitX);
		Ray rayShootingForward = new(rayOrigin, Vector3.UnitZ);
		Ray rayShootingBackward = new(rayOrigin, -Vector3.UnitZ);

		// No collision.
		Assert.IsFalse(Geometry3D.Raycast(triangleAtOriginFacingUp, rayShootingUp, out float _));
		Assert.IsFalse(Geometry3D.Raycast(triangleAtOriginFacingUp, rayShootingRight, out float _));
		Assert.IsFalse(Geometry3D.Raycast(triangleAtOriginFacingUp, rayShootingLeft, out float _));
		Assert.IsFalse(Geometry3D.Raycast(triangleAtOriginFacingUp, rayShootingForward, out float _));
		Assert.IsFalse(Geometry3D.Raycast(triangleAtOriginFacingUp, rayShootingBackward, out float _));

		// Ray shooting down collides with triangle facing up, at point 0,0,0.
		Assert.IsTrue(Geometry3D.Raycast(triangleAtOriginFacingUp, rayShootingDown, out RaycastResult result));
		Assert.AreEqual(1, result.Distance);
		Assert.AreEqual(Vector3.Zero, result.Point);
		Assert.AreEqual(Vector3.UnitY, result.Normal);

		// Transform the ray.
		Ray diagonalRay = new(new Vector3(1, 1, 0), Vector3.Normalize(new Vector3(-1, -1, 0)));
		Assert.IsTrue(Geometry3D.Raycast(triangleAtOriginFacingUp, diagonalRay, out RaycastResult resultDiagonal));
		Assert.AreEqual(MathF.Sqrt(2), resultDiagonal.Distance, 0.0001f);
		AssertionUtils.AreEqual(Vector3.Zero, resultDiagonal.Point);
		AssertionUtils.AreEqual(Vector3.UnitY, resultDiagonal.Normal);

		// Move the ray along the Z axis.
		Ray diagonalRayMoved = new(new Vector3(1, 1, 3), Vector3.Normalize(new Vector3(-1, -1, 0)));
		Assert.IsFalse(Geometry3D.Raycast(triangleAtOriginFacingUp, diagonalRayMoved, out RaycastResult _));

		// Move the triangle along the Z axis.
		Triangle3D movedTriangleFacingUp = new(new Vector3(-1.5f, 0, 2.5f), new Vector3(0.5f, 0, 4.5f), new Vector3(0.5f, 0, 2.5f));
		Assert.IsTrue(Geometry3D.Raycast(movedTriangleFacingUp, diagonalRayMoved, out RaycastResult resultMoved));
		Assert.AreEqual(MathF.Sqrt(2), resultMoved.Distance, 0.0001f);
		AssertionUtils.AreEqual(new Vector3(0, 0, 3), resultMoved.Point);
		AssertionUtils.AreEqual(Vector3.UnitY, resultMoved.Normal);

		// Rotate the triangle.
		Triangle3D movedAndRotatedTriangle = new()
		{
			A = new Vector3(0, -1, 2),
			B = new Vector3(0, +1, 3),
			C = new Vector3(0, -1, 4),
		};
		Assert.IsTrue(Geometry3D.Raycast(movedAndRotatedTriangle, diagonalRayMoved, out RaycastResult resultRotated));
		Assert.AreEqual(MathF.Sqrt(2), resultRotated.Distance, 0.0001f);
		AssertionUtils.AreEqual(new Vector3(0, 0, 3), resultRotated.Point);
		AssertionUtils.AreEqual(Vector3.UnitX, resultRotated.Normal);

		Triangle3D triangleFacingBackward = new()
		{
			A = new Vector3(-1, -1, -1),
			B = new Vector3(+0, +1, -1),
			C = new Vector3(+1, -1, -1),
		};

		ShootZRay(true, new Vector3(0, 0, -10));
		ShootZRay(true, new Vector3(0, 0, -1.01f));
		ShootZRay(true, new Vector3(0, 0, -1));
		ShootZRay(false, Vector3.Zero);
		ShootZRay(false, new Vector3(0, 0, 0.9f));
		ShootZRay(false, new Vector3(0, 0, 1));
		ShootZRay(false, new Vector3(0, 0, 10f));
		ShootZRay(false, new Vector3(0, 0, 1.1f));

		void ShootZRay(bool expected, Vector3 rayPosition)
		{
			Assert.AreEqual(expected, Geometry3D.Raycast(triangleFacingBackward, new Ray(rayPosition, Vector3.UnitZ), out RaycastResult _));
		}
	}

	[TestMethod]
	public void RaycastWithPlane()
	{
		Plane planeAtOriginFacingUp = new() { Normal = Vector3.UnitY, D = 0 };

		Assert.IsFalse(Geometry3D.Raycast(planeAtOriginFacingUp, new Ray(Vector3.UnitY, Vector3.UnitY), out RaycastResult _));
		Assert.IsFalse(Geometry3D.Raycast(planeAtOriginFacingUp, new Ray(Vector3.UnitY, Vector3.UnitX), out RaycastResult _));
		Assert.IsFalse(Geometry3D.Raycast(planeAtOriginFacingUp, new Ray(Vector3.UnitY, -Vector3.UnitX), out RaycastResult _));
		Assert.IsFalse(Geometry3D.Raycast(planeAtOriginFacingUp, new Ray(Vector3.UnitY, Vector3.UnitZ), out RaycastResult _));
		Assert.IsFalse(Geometry3D.Raycast(planeAtOriginFacingUp, new Ray(Vector3.UnitY, -Vector3.UnitZ), out RaycastResult _));
		ShootRayWithExpectedHit(1, Vector3.Zero, planeAtOriginFacingUp, new Ray(Vector3.UnitY, -Vector3.UnitY));

		ShootRayWithExpectedHit(MathF.Sqrt(2), Vector3.Zero, planeAtOriginFacingUp, new Ray(new Vector3(1, 1, 0), Vector3.Normalize(new Vector3(-1, -1, 0))));

		ShootRayWithExpectedHit(1, new Vector3(0, 0, 1), new Plane(Vector3.UnitZ, 1), new Ray(new Vector3(0, 0, 2), -Vector3.UnitZ));
		ShootRayWithExpectedHit(2, new Vector3(0, 0, 1), new Plane(Vector3.UnitZ, 1), new Ray(new Vector3(0, 0, 3), -Vector3.UnitZ));
		ShootRayWithExpectedHit(5, new Vector3(0, 0, 1), new Plane(Vector3.UnitZ, 1), new Ray(new Vector3(0, 0, 6), -Vector3.UnitZ));

		ShootRayWithExpectedHit(0.5f, new Vector3(0, 0, 1.5f), new Plane(Vector3.UnitZ, 1.5f), new Ray(new Vector3(0, 0, 2), -Vector3.UnitZ));
		ShootRayWithExpectedHit(7, new Vector3(0, 0, -5), new Plane(Vector3.UnitZ, -5), new Ray(new Vector3(0, 0, 2), -Vector3.UnitZ));
		ShootRayWithExpectedHit(3, new Vector3(0, 0, -5), new Plane(Vector3.UnitZ, -5), new Ray(new Vector3(0, 0, -2), -Vector3.UnitZ));

		ShootRayWithExpectedHit(1, new Vector3(5, -5, 1), new Plane(Vector3.UnitZ, 1), new Ray(new Vector3(5, -5, 2), -Vector3.UnitZ));

		Assert.IsTrue(Geometry3D.Raycast(new Plane(-Vector3.UnitZ, 1), new Ray(new Vector3(0, 0, -10f), Vector3.UnitZ), out RaycastResult _));
		Assert.IsTrue(Geometry3D.Raycast(new Plane(-Vector3.UnitZ, 1), new Ray(new Vector3(0, 0, -1.01f), Vector3.UnitZ), out RaycastResult _));
		Assert.IsTrue(Geometry3D.Raycast(new Plane(-Vector3.UnitZ, 1), new Ray(new Vector3(0, 0, -1f), Vector3.UnitZ), out RaycastResult _));
		Assert.IsFalse(Geometry3D.Raycast(new Plane(-Vector3.UnitZ, 1), new Ray(new Vector3(0, 0, -0.9f), Vector3.UnitZ), out RaycastResult _));
		Assert.IsFalse(Geometry3D.Raycast(new Plane(-Vector3.UnitZ, 1), new Ray(new Vector3(0, 0, 1f), Vector3.UnitZ), out RaycastResult _));
		Assert.IsFalse(Geometry3D.Raycast(new Plane(-Vector3.UnitZ, 1), new Ray(new Vector3(0, 0, 10f), Vector3.UnitZ), out RaycastResult _));
		Assert.IsFalse(Geometry3D.Raycast(new Plane(-Vector3.UnitZ, 1), new Ray(new Vector3(0, 0, 1.1f), Vector3.UnitZ), out RaycastResult _));

		static void ShootRayWithExpectedHit(float expectedDistance, Vector3 expectedIntersectionPoint, Plane plane, Ray ray)
		{
			Assert.IsTrue(Geometry3D.Raycast(plane, ray, out RaycastResult result));
			Assert.AreEqual(expectedDistance, result.Distance, 0.0001f);
			AssertionUtils.AreEqual(expectedIntersectionPoint, result.Point);
			AssertionUtils.AreEqual(plane.Normal, result.Normal);
		}
	}

	[TestMethod]
	public void SphereCastTriangle()
	{
		SphereCast sphereCast = new(Vector3.Zero, Vector3.UnitZ, 0.5f);

		Triangle3D triangleOnZ0 = new()
		{
			A = new Vector3(-1, -1, 0),
			B = new Vector3(+1, -1, 0),
			C = new Vector3(+0, +1, 0),
		};
		Assert.IsTrue(Geometry3D.SphereCastTriangle(sphereCast, triangleOnZ0));

		Triangle3D triangleOnZ1 = new()
		{
			A = new Vector3(-1, -1, 1),
			B = new Vector3(+1, -1, 1),
			C = new Vector3(+0, +1, 1),
		};
		Assert.IsTrue(Geometry3D.SphereCastTriangle(sphereCast, triangleOnZ1));

		Triangle3D triangleOnZ2 = new()
		{
			A = new Vector3(-1, -1, 2),
			B = new Vector3(+1, -1, 2),
			C = new Vector3(+0, +1, 2),
		};
		Assert.IsFalse(Geometry3D.SphereCastTriangle(sphereCast, triangleOnZ2));

		Triangle3D largeTriangle = new()
		{
			A = new Vector3(-16, -16, 0.5f),
			B = new Vector3(+16, -16, 0.5f),
			C = new Vector3(+00, +16, 0.5f),
		};
		Assert.IsTrue(Geometry3D.SphereCastTriangle(sphereCast, largeTriangle));
	}

	[TestMethod]
	public void SphereCastTriangleWithIntersectionPoint()
	{
		SphereCast sphereCast = new(Vector3.Zero, new Vector3(1, 0, 1), 0.5f);

		Triangle3D triangle = new()
		{
			A = new Vector3(-4, -4, 0.5f),
			B = new Vector3(+4, -4, 0.5f),
			C = new Vector3(+0, +4, 0.5f),
		};
		Assert.IsTrue(Geometry3D.SphereCastTriangle(sphereCast, triangle, out Vector3 intersectionPoint));
		AssertionUtils.AreEqual(new Vector3(0.5f, 0, 0.5f), intersectionPoint);

		sphereCast = new SphereCast(Vector3.Zero, new Vector3(1, 0, 1), 1);
		triangle = new Triangle3D
		{
			A = new Vector3(-4, -4, 0.25f),
			B = new Vector3(+4, -4, 0.25f),
			C = new Vector3(+0, +4, 0.25f),
		};
		Assert.IsTrue(Geometry3D.SphereCastTriangle(sphereCast, triangle, out intersectionPoint));
		AssertionUtils.AreEqual(new Vector3(0.25f, 0, 0.25f), intersectionPoint);
	}
}
