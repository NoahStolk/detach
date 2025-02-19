using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool TriangleSphere(Triangle3D triangle, Sphere sphere)
	{
		Vector3 closest = ClosestPointOnTriangle(sphere.Center, triangle);
		float distanceSquared = Vector3.DistanceSquared(sphere.Center, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool TriangleAabb(Triangle3D triangle, Aabb aabb)
	{
		Vector3 f0 = triangle.B - triangle.A;
		Vector3 f1 = triangle.C - triangle.B;
		Vector3 f2 = triangle.A - triangle.C;

		Vector3 u0 = new(1, 0, 0);
		Vector3 u1 = new(0, 1, 0);
		Vector3 u2 = new(0, 0, 1);

		Span<Vector3> axes =
		[
			u0,
			u1,
			u2,

			Vector3.Cross(f0, f1),

			Vector3.Cross(u0, f0),
			Vector3.Cross(u0, f1),
			Vector3.Cross(u0, f2),
			Vector3.Cross(u1, f0),
			Vector3.Cross(u1, f1),
			Vector3.Cross(u1, f2),
			Vector3.Cross(u2, f0),
			Vector3.Cross(u2, f1),
			Vector3.Cross(u2, f2),
		];

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(aabb, triangle, axes[i]))
				return false;
		}

		return true;
	}

	public static bool TriangleObb(Triangle3D triangle, Obb obb)
	{
		Vector3 f0 = triangle.B - triangle.A;
		Vector3 f1 = triangle.C - triangle.B;
		Vector3 f2 = triangle.A - triangle.C;
		Vector3 u0 = new(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13);
		Vector3 u1 = new(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23);
		Vector3 u2 = new(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33);

		Span<Vector3> axes =
		[
			u0,
			u1,
			u2,

			Vector3.Cross(f0, f1),

			Vector3.Cross(u0, f0),
			Vector3.Cross(u0, f1),
			Vector3.Cross(u0, f2),
			Vector3.Cross(u1, f0),
			Vector3.Cross(u1, f1),
			Vector3.Cross(u1, f2),
			Vector3.Cross(u2, f0),
			Vector3.Cross(u2, f1),
			Vector3.Cross(u2, f2),
		];

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(obb, triangle, axes[i]))
				return false;
		}

		return true;
	}

	public static bool TrianglePlane(Triangle3D triangle, Plane plane)
	{
		float side1 = PlaneEquation(triangle.A, plane);
		float side2 = PlaneEquation(triangle.B, plane);
		float side3 = PlaneEquation(triangle.C, plane);

		if (side1 == 0 && side2 == 0 && side3 == 0)
			return true;

		if (side1 > 0 && side2 > 0 && side3 > 0)
			return false;

		if (side1 < 0 && side2 < 0 && side3 < 0)
			return false;

		return true;
	}

	public static bool TriangleTriangle(Triangle3D triangle1, Triangle3D triangle2)
	{
		Vector3 f0 = triangle1.B - triangle1.A;
		Vector3 f1 = triangle1.C - triangle1.B;
		Vector3 f2 = triangle1.A - triangle1.C;
		Vector3 u0 = triangle2.B - triangle2.A;
		Vector3 u1 = triangle2.C - triangle2.B;
		Vector3 u2 = triangle2.A - triangle2.C;

		Span<Vector3> axes =
		[
			Vector3.Cross(f0, f1),

			Vector3.Cross(u0, u1),

			Vector3.Cross(u0, f0),
			Vector3.Cross(u0, f1),
			Vector3.Cross(u0, f2),
			Vector3.Cross(u1, f0),
			Vector3.Cross(u1, f1),
			Vector3.Cross(u1, f2),
			Vector3.Cross(u2, f0),
			Vector3.Cross(u2, f1),
			Vector3.Cross(u2, f2),
		];

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(triangle1, triangle2, axes[i]))
				return false;
		}

		return true;
	}

	public static bool TriangleTriangleRobust(Triangle3D triangle1, Triangle3D triangle2)
	{
		Span<Vector3> axes =
		[
			SatCrossEdge(triangle1.A, triangle1.B, triangle1.B, triangle2.C),
			SatCrossEdge(triangle2.A, triangle2.B, triangle2.B, triangle1.C),

			SatCrossEdge(triangle2.A, triangle2.B, triangle1.A, triangle1.B),
			SatCrossEdge(triangle2.A, triangle2.B, triangle1.B, triangle1.C),
			SatCrossEdge(triangle2.A, triangle2.B, triangle1.C, triangle1.A),

			SatCrossEdge(triangle2.B, triangle2.C, triangle1.A, triangle1.B),
			SatCrossEdge(triangle2.B, triangle2.C, triangle1.B, triangle1.C),
			SatCrossEdge(triangle2.B, triangle2.C, triangle1.C, triangle1.A),

			SatCrossEdge(triangle2.C, triangle2.A, triangle1.A, triangle1.B),
			SatCrossEdge(triangle2.C, triangle2.A, triangle1.B, triangle1.C),
			SatCrossEdge(triangle2.C, triangle2.A, triangle1.C, triangle1.A),
		];

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(triangle1, triangle2, axes[i]) && axes[i].LengthSquared() > float.Epsilon)
				return false;
		}

		return true;
	}
}
