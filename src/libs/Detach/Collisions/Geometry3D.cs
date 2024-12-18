﻿using Detach.Buffers;
using Detach.Collisions.Primitives3D;
using Detach.Numerics;
using Detach.Utils;
using System.Numerics;

namespace Detach.Collisions;

public static class Geometry3D
{
	#region Point vs primitives

	public static bool PointInSphere(Vector3 point, Sphere sphere)
	{
		return Vector3.DistanceSquared(point, sphere.Position) <= sphere.Radius * sphere.Radius;
	}

	public static Vector3 ClosestPointInSphere(Sphere sphere, Vector3 point)
	{
		return sphere.Position + Vector3.Normalize(point - sphere.Position) * sphere.Radius;
	}

	public static bool PointInAabb(Vector3 point, Aabb aabb)
	{
		Vector3 min = aabb.GetMin();
		Vector3 max = aabb.GetMax();

		return point.X >= min.X && point.X <= max.X
			&& point.Y >= min.Y && point.Y <= max.Y
			&& point.Z >= min.Z && point.Z <= max.Z;
	}

	public static Vector3 ClosestPointInAabb(Aabb aabb, Vector3 point)
	{
		Vector3 min = aabb.GetMin();
		Vector3 max = aabb.GetMax();

		return new Vector3(
			Math.Clamp(point.X, min.X, max.X),
			Math.Clamp(point.Y, min.Y, max.Y),
			Math.Clamp(point.Z, min.Z, max.Z));
	}

	public static bool PointInObb(Vector3 point, Obb obb)
	{
		Vector3 direction = point - obb.Position;
		for (int i = 0; i < 3; i++)
		{
			Vector3 axis = new(
				obb.Orientation[i * 3 + 0],
				obb.Orientation[i * 3 + 1],
				obb.Orientation[i * 3 + 2]);
			float distance = Vector3.Dot(direction, axis);
			if (distance > obb.HalfExtents[i] || distance < -obb.HalfExtents[i])
				return false;
		}

		return true;
	}

	public static Vector3 ClosestPointInObb(Obb obb, Vector3 point)
	{
		Vector3 direction = point - obb.Position;
		Vector3 result = obb.Position;
		for (int i = 0; i < 3; i++)
		{
			Vector3 axis = new(
				obb.Orientation[i * 3 + 0],
				obb.Orientation[i * 3 + 1],
				obb.Orientation[i * 3 + 2]);
			float distance = Vector3.Dot(direction, axis);
			result += axis * Math.Clamp(distance, -obb.HalfExtents[i], obb.HalfExtents[i]);
		}

		return result;
	}

	public static bool PointOnPlane(Vector3 point, Plane plane)
	{
		float dot = Vector3.Dot(point, plane.Normal);
		float distance = dot - plane.D;
		return distance is >= -float.Epsilon and <= float.Epsilon;
	}

	public static Vector3 ClosestPointOnPlane(Plane plane, Vector3 point)
	{
		float dot = Vector3.Dot(point, plane.Normal);
		float distance = dot - plane.D;
		return point - plane.Normal * distance;
	}

	public static bool PointOnLine(Vector3 point, LineSegment3D line)
	{
		Vector3 closest = ClosestPointOnLine(point, line);
		float distanceSquared = Vector3.DistanceSquared(point, closest);
		return distanceSquared <= float.Epsilon;
	}

	public static Vector3 ClosestPointOnLine(Vector3 point, LineSegment3D line)
	{
		Vector3 direction = line.End - line.Start;
		float t = Vector3.Dot(point - line.Start, direction) / Vector3.Dot(direction, direction);
		t = Math.Clamp(t, 0, 1);
		return line.Start + direction * t;
	}

	public static bool PointOnRay(Vector3 point, Ray ray)
	{
		if (point == ray.Origin)
			return true;

		Vector3 normalized = Vector3.Normalize(point - ray.Origin);
		float dot = Vector3.Dot(normalized, ray.Direction);
		return dot is >= 1 - float.Epsilon and <= 1 + float.Epsilon;
	}

	public static Vector3 ClosestPointOnRay(Vector3 point, Ray ray)
	{
		if (point == ray.Origin)
			return point;

		Vector3 normalized = Vector3.Normalize(point - ray.Origin);
		float t = Vector3.Dot(normalized, ray.Direction);
		t /= Vector3.Dot(ray.Direction, ray.Direction);
		t = Math.Max(t, 0);
		return ray.Origin + ray.Direction * t;
	}

	public static bool PointInTriangle(Vector3 point, Triangle3D triangle)
	{
		Vector3 a = triangle.A - point;
		Vector3 b = triangle.B - point;
		Vector3 c = triangle.C - point;

		Vector3 normPbc = Vector3.Cross(b, c);
		Vector3 normPca = Vector3.Cross(c, a);
		Vector3 normPab = Vector3.Cross(a, b);

		if (Vector3.Dot(normPbc, normPca) < 0)
			return false;

		return Vector3.Dot(normPbc, normPab) >= 0;
	}

	public static Vector3 ClosestPointOnTriangle(Vector3 point, Triangle3D triangle)
	{
		Plane plane = CreatePlaneFromTriangle(triangle);
		Vector3 closest = ClosestPointOnPlane(plane, point);
		if (PointInTriangle(closest, triangle))
			return closest;

		Vector3 c1 = ClosestPointOnLine(point, new LineSegment3D(triangle.A, triangle.B));
		Vector3 c2 = ClosestPointOnLine(point, new LineSegment3D(triangle.B, triangle.C));
		Vector3 c3 = ClosestPointOnLine(point, new LineSegment3D(triangle.C, triangle.A));

		float d1 = Vector3.DistanceSquared(point, c1);
		float d2 = Vector3.DistanceSquared(point, c2);
		float d3 = Vector3.DistanceSquared(point, c3);

		if (d1 < d2 && d1 < d3)
			return c1;

		if (d2 < d1 && d2 < d3)
			return c2;

		return c3;
	}

	public static bool PointInFrustum(Vector3 point, Frustum frustum)
	{
		for (int i = 0; i < 6; i++)
		{
			Plane plane = frustum[i];
			if (point.X * plane.Normal.X + point.Y * plane.Normal.Y + point.Z * plane.Normal.Z + plane.D > 0)
				return false;
		}

		return true;
	}

	public static bool PointInCylinder(Vector3 point, Cylinder cylinder)
	{
		Vector2 pointXz = new(point.X, point.Z);
		Vector2 positionXz = new(cylinder.BasePosition.X, cylinder.BasePosition.Z);

		return
			Vector2.DistanceSquared(pointXz, positionXz) <= cylinder.Radius * cylinder.Radius &&
			point.Y >= cylinder.BasePosition.Y &&
			point.Y <= cylinder.BasePosition.Y + cylinder.Height;
	}

	#endregion Point vs primitives

	#region Sphere vs primitives

	public static bool SphereSphere(Sphere sphere1, Sphere sphere2)
	{
		float distanceSquared = Vector3.DistanceSquared(sphere1.Position, sphere2.Position);
		float radiusSum = sphere1.Radius + sphere2.Radius;
		return distanceSquared <= radiusSum * radiusSum;
	}

	public static bool SphereAabb(Sphere sphere, Aabb aabb)
	{
		Vector3 closest = ClosestPointInAabb(aabb, sphere.Position);
		float distanceSquared = Vector3.DistanceSquared(sphere.Position, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool SphereObb(Sphere sphere, Obb obb)
	{
		Vector3 closest = ClosestPointInObb(obb, sphere.Position);
		float distanceSquared = Vector3.DistanceSquared(sphere.Position, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool SpherePlane(Sphere sphere, Plane plane)
	{
		Vector3 closest = ClosestPointOnPlane(plane, sphere.Position);
		float distanceSquared = Vector3.DistanceSquared(sphere.Position, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool SphereFrustum(Sphere sphere, Frustum frustum)
	{
		if (PointInFrustum(sphere.Position, frustum))
			return true;

		for (int i = 0; i < 6; ++i)
		{
			if (SpherePlane(sphere, frustum[i]))
				return true;
		}

		return false;
	}

	public static bool SphereCylinder(Sphere sphere, Cylinder cylinder)
	{
		// Project the sphere's center onto the XZ plane.
		Vector2 sphereXz = new(sphere.Position.X, sphere.Position.Z);
		Vector2 cylinderXz = new(cylinder.BasePosition.X, cylinder.BasePosition.Z);

		// Check if the distance in the XZ plane is less than or equal to the sum of the radii.
		float distanceSquaredXz = Vector2.DistanceSquared(sphereXz, cylinderXz);
		float radiusSum = sphere.Radius + cylinder.Radius;
		if (distanceSquaredXz > radiusSum * radiusSum)
			return false;

		// Check if the sphere's center is within the height range of the cylinder.
		float sphereMinY = sphere.Position.Y - sphere.Radius;
		float sphereMaxY = sphere.Position.Y + sphere.Radius;
		float cylinderMinY = cylinder.BasePosition.Y;
		float cylinderMaxY = cylinder.BasePosition.Y + cylinder.Height;

		return sphereMaxY >= cylinderMinY && sphereMinY <= cylinderMaxY;
	}

	#endregion Sphere vs primitives

	#region Aabb vs primitives

	public static bool AabbAabb(Aabb aabb1, Aabb aabb2)
	{
		Vector3 min1 = aabb1.GetMin();
		Vector3 max1 = aabb1.GetMax();
		Vector3 min2 = aabb2.GetMin();
		Vector3 max2 = aabb2.GetMax();

		return min1.X <= max2.X && max1.X >= min2.X
			&& min1.Y <= max2.Y && max1.Y >= min2.Y
			&& min1.Z <= max2.Z && max1.Z >= min2.Z;
	}

	public static bool AabbObbSat(Aabb aabb, Obb obb)
	{
		Span<Vector3> axes = stackalloc Vector3[15];
		axes[0] = new Vector3(1, 0, 0);
		axes[1] = new Vector3(0, 1, 0);
		axes[2] = new Vector3(0, 0, 1);
		axes[3] = new Vector3(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13);
		axes[4] = new Vector3(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23);
		axes[5] = new Vector3(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33);

		for (int i = 0; i < 3; i++)
		{
			axes[6 + i * 3 + 0] = Vector3.Cross(axes[i], axes[0]);
			axes[6 + i * 3 + 1] = Vector3.Cross(axes[i], axes[1]);
			axes[6 + i * 3 + 2] = Vector3.Cross(axes[i], axes[2]);
		}

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(aabb, obb, axes[i]))
				return false;
		}

		return true;
	}

	public static bool AabbPlane(Aabb aabb, Plane plane)
	{
		float pLen =
			aabb.Size.X * Math.Abs(plane.Normal.X) +
			aabb.Size.Y * Math.Abs(plane.Normal.Y) +
			aabb.Size.Z * Math.Abs(plane.Normal.Z);

		float dot = Vector3.Dot(plane.Normal, aabb.Origin);
		float distance = dot - plane.D;
		return Math.Abs(distance) <= pLen;
	}

	#endregion Aabb vs primitives

	#region Obb vs primitives

	public static bool ObbObbSat(Obb obb1, Obb obb2)
	{
		Span<Vector3> axes = stackalloc Vector3[15];
		axes[0] = new Vector3(obb1.Orientation.M11, obb1.Orientation.M12, obb1.Orientation.M13);
		axes[1] = new Vector3(obb1.Orientation.M21, obb1.Orientation.M22, obb1.Orientation.M23);
		axes[2] = new Vector3(obb1.Orientation.M31, obb1.Orientation.M32, obb1.Orientation.M33);
		axes[3] = new Vector3(obb2.Orientation.M11, obb2.Orientation.M12, obb2.Orientation.M13);
		axes[4] = new Vector3(obb2.Orientation.M21, obb2.Orientation.M22, obb2.Orientation.M23);
		axes[5] = new Vector3(obb2.Orientation.M31, obb2.Orientation.M32, obb2.Orientation.M33);

		for (int i = 0; i < 3; i++)
		{
			axes[6 + i * 3 + 0] = Vector3.Cross(axes[i], axes[0]);
			axes[6 + i * 3 + 1] = Vector3.Cross(axes[i], axes[1]);
			axes[6 + i * 3 + 2] = Vector3.Cross(axes[i], axes[2]);
		}

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(obb1, obb2, axes[i]))
				return false;
		}

		return true;
	}

	public static bool ObbPlane(Obb obb, Plane plane)
	{
		Span<Vector3> axes =
		[
			new(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13),
			new(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23),
			new(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33),
		];

		float pLen =
			obb.HalfExtents.X * Math.Abs(Vector3.Dot(plane.Normal, axes[0])) +
			obb.HalfExtents.Y * Math.Abs(Vector3.Dot(plane.Normal, axes[1])) +
			obb.HalfExtents.Z * Math.Abs(Vector3.Dot(plane.Normal, axes[2]));

		float dot = Vector3.Dot(plane.Normal, obb.Position);
		float distance = dot - plane.D;
		return Math.Abs(distance) <= pLen;
	}

	#endregion Obb vs primitives

	#region Plane vs primitives

	public static bool PlanePlane(Plane plane1, Plane plane2)
	{
		Vector3 cross = Vector3.Cross(plane1.Normal, plane2.Normal);
		return cross.LengthSquared() > float.Epsilon;
	}

	#endregion Plane vs primitives

	#region Triangle vs primitives

	public static bool TriangleSphere(Triangle3D triangle, Sphere sphere)
	{
		Vector3 closest = ClosestPointOnTriangle(sphere.Position, triangle);
		float distanceSquared = Vector3.DistanceSquared(sphere.Position, closest);
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

	#endregion Triangle vs primitives

	#region Cylinder vs primitives

	public static bool CylinderCylinder(Cylinder cylinder1, Cylinder cylinder2)
	{
		// Project the cylinder's centers onto the XZ plane.
		Vector2 cylinder1Xz = new(cylinder1.BasePosition.X, cylinder1.BasePosition.Z);
		Vector2 cylinder2Xz = new(cylinder2.BasePosition.X, cylinder2.BasePosition.Z);

		// Check if the distance in the XZ plane is less than or equal to the sum of the radii.
		float distanceSquaredXz = Vector2.DistanceSquared(cylinder1Xz, cylinder2Xz);
		float radiusSum = cylinder1.Radius + cylinder2.Radius;
		if (distanceSquaredXz > radiusSum * radiusSum)
			return false;

		// Check if the cylinders' heights overlap.
		float cylinder1MinY = cylinder1.BasePosition.Y;
		float cylinder1MaxY = cylinder1.BasePosition.Y + cylinder1.Height;
		float cylinder2MinY = cylinder2.BasePosition.Y;
		float cylinder2MaxY = cylinder2.BasePosition.Y + cylinder2.Height;

		return cylinder1MaxY >= cylinder2MinY && cylinder1MinY <= cylinder2MaxY;
	}

	#endregion Cylinder vs primitives

	#region Raycasting

	public static bool Raycast(Sphere sphere, Ray ray, out float distance)
	{
		distance = default;

		Vector3 e = sphere.Position - ray.Origin;
		float radiusSquared = sphere.Radius * sphere.Radius;
		float eSquared = e.LengthSquared();
		float a = Vector3.Dot(e, ray.Direction);
		float bSquared = eSquared - a * a;
		float f = MathF.Sqrt(radiusSquared - bSquared);

		if (radiusSquared - (eSquared - a * a) < 0)
			return false;

		distance = eSquared < radiusSquared ? a + f : a - f;
		return true;
	}

	public static bool Raycast(Sphere sphere, Ray ray, out RaycastResult result)
	{
		result = default;

		Vector3 e = sphere.Position - ray.Origin;
		float radiusSquared = sphere.Radius * sphere.Radius;
		float eSquared = e.LengthSquared();
		float a = Vector3.Dot(e, ray.Direction);
		float bSquared = eSquared - a * a;
		float f = MathF.Sqrt(radiusSquared - bSquared);

		if (radiusSquared - (eSquared - a * a) < 0)
			return false;

		result.Distance = eSquared < radiusSquared ? a + f : a - f;
		result.Point = ray.Origin + ray.Direction * result.Distance;
		result.Normal = Vector3.Normalize(result.Point - sphere.Position);
		return true;
	}

	public static bool Raycast(Aabb aabb, Ray ray, out float distance)
	{
		distance = default;

		Vector3 min = aabb.GetMin();
		Vector3 max = aabb.GetMax();
		Span<float> t =
		[
			(min.X - ray.Origin.X) / ray.Direction.X,
			(max.X - ray.Origin.X) / ray.Direction.X,
			(min.Y - ray.Origin.Y) / ray.Direction.Y,
			(max.Y - ray.Origin.Y) / ray.Direction.Y,
			(min.Z - ray.Origin.Z) / ray.Direction.Z,
			(max.Z - ray.Origin.Z) / ray.Direction.Z,
		];

		float tMin = Math.Max(Math.Max(Math.Min(t[0], t[1]), Math.Min(t[2], t[3])), Math.Min(t[4], t[5]));
		float tMax = Math.Min(Math.Min(Math.Max(t[0], t[1]), Math.Max(t[2], t[3])), Math.Max(t[4], t[5]));

		if (tMax < 0 || tMin > tMax)
			return false;

		distance = tMin < 0 ? tMax : tMin;
		return true;
	}

	public static bool Raycast(Aabb aabb, Ray ray, out RaycastResult result)
	{
		result = default;

		Vector3 min = aabb.GetMin();
		Vector3 max = aabb.GetMax();
		Span<float> t =
		[
			(min.X - ray.Origin.X) / ray.Direction.X,
			(max.X - ray.Origin.X) / ray.Direction.X,
			(min.Y - ray.Origin.Y) / ray.Direction.Y,
			(max.Y - ray.Origin.Y) / ray.Direction.Y,
			(min.Z - ray.Origin.Z) / ray.Direction.Z,
			(max.Z - ray.Origin.Z) / ray.Direction.Z,
		];

		float tMin = Math.Max(Math.Max(Math.Min(t[0], t[1]), Math.Min(t[2], t[3])), Math.Min(t[4], t[5]));
		float tMax = Math.Min(Math.Min(Math.Max(t[0], t[1]), Math.Max(t[2], t[3])), Math.Max(t[4], t[5]));

		if (tMax < 0 || tMin > tMax)
			return false;

		result.Distance = tMin < 0 ? tMax : tMin;
		result.Point = ray.Origin + ray.Direction * result.Distance;

		Span<Vector3> normals =
		[
			new(-1, 0, 0),
			new(1, 0, 0),
			new(0, -1, 0),
			new(0, 1, 0),
			new(0, 0, -1),
			new(0, 0, 1),
		];

		for (int i = 0; i < 6; i++)
		{
			if (Math.Abs(result.Distance - t[i]) < float.Epsilon)
			{
				result.Normal = normals[i];
				break;
			}
		}

		return true;
	}

	public static bool Raycast(Obb obb, Ray ray, out float distance)
	{
		distance = default;

		Vector3 x = new(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13);
		Vector3 y = new(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23);
		Vector3 z = new(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33);
		Vector3 p = obb.Position - ray.Origin;
		Vector3 f = new(
			Vector3.Dot(x, ray.Direction),
			Vector3.Dot(y, ray.Direction),
			Vector3.Dot(z, ray.Direction));
		Vector3 e = new(
			Vector3.Dot(x, p),
			Vector3.Dot(y, p),
			Vector3.Dot(z, p));
		Span<float> t = stackalloc float[6];
		for (int i = 0; i < 3; i++)
		{
			if (Math.Abs(f[i]) < float.Epsilon)
			{
				if (-e[i] - obb.HalfExtents[i] > 0 || -e[i] + obb.HalfExtents[i] < 0)
					return false;

				f[i] = float.Epsilon; // Avoid division by 0.
			}

			t[i * 2 + 0] = (e[i] + obb.HalfExtents[i]) / f[i];
			t[i * 2 + 1] = (e[i] - obb.HalfExtents[i]) / f[i];
		}

		float tMin = Math.Max(Math.Max(Math.Min(t[0], t[1]), Math.Min(t[2], t[3])), Math.Min(t[4], t[5]));
		float tMax = Math.Min(Math.Min(Math.Max(t[0], t[1]), Math.Max(t[2], t[3])), Math.Max(t[4], t[5]));
		if (tMax < 0 || tMin > tMax)
			return false;

		distance = tMin < 0 ? tMax : tMin;
		return true;
	}

	public static bool Raycast(Obb obb, Ray ray, out RaycastResult result)
	{
		result = default;

		Vector3 x = new(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13);
		Vector3 y = new(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23);
		Vector3 z = new(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33);
		Vector3 p = obb.Position - ray.Origin;
		Vector3 f = new(
			Vector3.Dot(x, ray.Direction),
			Vector3.Dot(y, ray.Direction),
			Vector3.Dot(z, ray.Direction));
		Vector3 e = new(
			Vector3.Dot(x, p),
			Vector3.Dot(y, p),
			Vector3.Dot(z, p));
		Span<float> t = stackalloc float[6];
		for (int i = 0; i < 3; i++)
		{
			if (Math.Abs(f[i]) < float.Epsilon)
			{
				if (-e[i] - obb.HalfExtents[i] > 0 || -e[i] + obb.HalfExtents[i] < 0)
					return false;

				f[i] = float.Epsilon; // Avoid division by 0.
			}

			t[i * 2 + 0] = (e[i] + obb.HalfExtents[i]) / f[i];
			t[i * 2 + 1] = (e[i] - obb.HalfExtents[i]) / f[i];
		}

		float tMin = Math.Max(Math.Max(Math.Min(t[0], t[1]), Math.Min(t[2], t[3])), Math.Min(t[4], t[5]));
		float tMax = Math.Min(Math.Min(Math.Max(t[0], t[1]), Math.Max(t[2], t[3])), Math.Max(t[4], t[5]));
		if (tMax < 0 || tMin > tMax)
			return false;

		result.Distance = tMin < 0 ? tMax : tMin;
		result.Point = ray.Origin + ray.Direction * result.Distance;

		Span<Vector3> normals =
		[
			x,
			x * -1,
			y,
			y * -1,
			z,
			z * -1,
		];

		for (int i = 0; i < 6; i++)
		{
			if (Math.Abs(result.Distance - t[i]) < float.Epsilon)
			{
				result.Normal = normals[i];
				break;
			}
		}

		return true;
	}

	public static bool Raycast(Plane plane, Ray ray, out float distance)
	{
		distance = default;

		float nd = Vector3.Dot(ray.Direction, plane.Normal);
		if (nd >= 0)
			return false;

		float pn = Vector3.Dot(ray.Origin, plane.Normal);
		distance = (plane.D - pn) / nd;
		return distance >= 0;
	}

	public static bool Raycast(Plane plane, Ray ray, out RaycastResult result)
	{
		result = default;

		float nd = Vector3.Dot(ray.Direction, plane.Normal);
		if (nd >= 0)
			return false;

		float pn = Vector3.Dot(ray.Origin, plane.Normal);
		float distance = (plane.D - pn) / nd;
		if (distance < 0)
			return false;

		result.Point = ray.Origin + ray.Direction * distance;
		result.Distance = distance;
		result.Normal = Vector3.Normalize(plane.Normal);
		return true;
	}

	public static bool Raycast(Triangle3D triangle, Ray ray, out float distance)
	{
		Plane plane = CreatePlaneFromTriangle(triangle);
		if (!Raycast(plane, ray, out distance))
			return false;

		Vector3 result = ray.Origin + ray.Direction * distance;
		Vector3 barycentric = Barycentric(result, triangle);
		return barycentric is { X: >= 0 and <= 1, Y: >= 0 and <= 1, Z: >= 0 and <= 1 };
	}

	public static bool Raycast(Triangle3D triangle, Ray ray, out RaycastResult raycastResult)
	{
		Plane plane = CreatePlaneFromTriangle(triangle);
		if (!Raycast(plane, ray, out raycastResult))
			return false;

		Vector3 barycentric = Barycentric(raycastResult.Point, triangle);
		return barycentric is { X: >= 0 and <= 1, Y: >= 0 and <= 1, Z: >= 0 and <= 1 };
	}

	// TODO: Test this method.
	public static bool Raycast(Cylinder cylinder, Ray ray, out float distance)
	{
		distance = default;

		Vector2 cylinderXz = new(cylinder.BasePosition.X, cylinder.BasePosition.Z);
		Vector2 rayOriginXz = new(ray.Origin.X, ray.Origin.Z);
		Vector2 rayDirectionXz = new(ray.Direction.X, ray.Direction.Z);

		Vector2 d = cylinderXz - rayOriginXz;
		float a = Vector2.Dot(rayDirectionXz, rayDirectionXz);
		float b = 2 * Vector2.Dot(rayDirectionXz, d);
		float c = Vector2.Dot(d, d) - cylinder.Radius * cylinder.Radius;

		float discriminant = b * b - 4 * a * c;
		if (discriminant < 0)
			return false;

		float t1 = (-b + MathF.Sqrt(discriminant)) / (2 * a);
		float t2 = (-b - MathF.Sqrt(discriminant)) / (2 * a);

		float y1 = ray.Origin.Y + ray.Direction.Y * t1;
		float y2 = ray.Origin.Y + ray.Direction.Y * t2;

		if (y1 < cylinder.BasePosition.Y || y1 > cylinder.BasePosition.Y + cylinder.Height)
			t1 = float.MaxValue;

		if (y2 < cylinder.BasePosition.Y || y2 > cylinder.BasePosition.Y + cylinder.Height)
			t2 = float.MaxValue;

		distance = Math.Min(t1, t2);
		return distance >= 0;
	}

	#endregion Raycasting

	#region Linetesting

	public static bool Linetest(Sphere sphere, LineSegment3D line)
	{
		Vector3 closest = ClosestPointOnLine(sphere.Position, line);
		float distanceSquared = Vector3.DistanceSquared(sphere.Position, closest);
		return distanceSquared <= sphere.Radius * sphere.Radius;
	}

	public static bool Linetest(Aabb aabb, LineSegment3D line)
	{
		Ray ray = new(line.Start, line.End - line.Start);
		if (!Raycast(aabb, ray, out float distance))
			return false;

		return distance >= 0 && distance * distance <= line.LengthSquared;
	}

	public static bool Linetest(Obb obb, LineSegment3D line)
	{
		Ray ray = new(line.Start, line.End - line.Start);
		if (!Raycast(obb, ray, out float distance))
			return false;

		return distance >= 0 && distance * distance <= line.LengthSquared;
	}

	public static bool Linetest(Plane plane, LineSegment3D line)
	{
		Vector3 ab = line.End - line.Start;
		float nA = Vector3.Dot(plane.Normal, line.Start);
		float nAb = Vector3.Dot(plane.Normal, ab);

		float t = (plane.D - nA) / nAb;
		return t is >= 0 and <= 1;
	}

	public static bool Linetest(Triangle3D triangle, LineSegment3D line)
	{
		Ray ray = new(line.Start, line.End - line.Start);
		return Raycast(triangle, ray, out float t) && t >= 0 && t * t <= line.LengthSquared;
	}

	#endregion Linetesting

	#region Sphere casting

	public static bool SphereCastPoint(SphereCast sphereCast, Vector3 point)
	{
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		Vector3 m = sphereCast.Start - point;
		float b = Vector3.Dot(m, direction);
		float c = Vector3.Dot(m, m) - sphereCast.Radius * sphereCast.Radius;

		if (c > 0.0f && b > 0.0f)
			return false;

		float discriminant = b * b - c;
		if (discriminant < 0.0f)
			return false;

		float t = -b - MathF.Sqrt(discriminant);
		if (t < 0.0f)
			t = 0.0f;

		return t <= length;
	}

	public static bool SphereCastLineSegment(SphereCast sphereCast, LineSegment3D line)
	{
		Vector3 edge = line.End - line.Start;
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		Vector3 m = sphereCast.Start - line.Start;
		Vector3 n = Vector3.Cross(edge, direction);
		float d = Vector3.Dot(n, n);
		float e = Vector3.Dot(n, m);

		if (MathF.Abs(d) < float.Epsilon)
		{
			// Sphere is parallel to the edge.
			return false;
		}

		float t = e / d;
		if (t < 0 || t > length)
			return false;

		Vector3 point = sphereCast.Start + t * direction;
		Vector3 closest = ClosestPointOnLine(point, new LineSegment3D(line.Start, line.End));
		return Vector3.DistanceSquared(point, closest) <= sphereCast.Radius * sphereCast.Radius;
	}

	public static bool SphereCastSphere(SphereCast sphereCast, Sphere sphere)
	{
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		Vector3 m = sphereCast.Start - sphere.Position;
		float b = Vector3.Dot(m, direction);
		float c = Vector3.Dot(m, m) - (sphere.Radius + sphereCast.Radius) * (sphere.Radius + sphereCast.Radius);

		// Exit if the start point is outside the sphere, and the sphere from the sphere cast is not moving towards the stationary sphere.
		if (c > 0.0f && b > 0.0f)
			return false;

		float discriminant = b * b - c;

		// A negative discriminant means no real roots, so no intersection.
		if (discriminant < 0.0f)
			return false;

		// Calculate the smallest t value of intersection.
		float t = -b - MathF.Sqrt(discriminant);

		// If t is negative, the intersection point is behind the start point.
		if (t < 0.0f)
			t = 0.0f;

		// Check if the intersection point is within the segment.
		return t <= length;
	}

	public static bool SphereCastSphereCast(SphereCast sphereCast1, SphereCast sphereCast2)
	{
		Vector3 direction1 = sphereCast1.End - sphereCast1.Start;
		Vector3 direction2 = sphereCast2.End - sphereCast2.Start;
		float radiusSum = sphereCast1.Radius + sphereCast2.Radius;

		// Calculate the relative velocity.
		Vector3 relativeVelocity = direction2 - direction1;

		// Calculate the vector between the starting points of the sphere casts.
		Vector3 startDiff = sphereCast2.Start - sphereCast1.Start;

		// Calculate the coefficients of the quadratic equation.
		float a = Vector3.Dot(relativeVelocity, relativeVelocity);
		float b = 2 * Vector3.Dot(relativeVelocity, startDiff);
		float c = Vector3.Dot(startDiff, startDiff) - radiusSum * radiusSum;

		// Solve the quadratic equation for t.
		float discriminant = b * b - 4 * a * c;
		if (discriminant < 0)
		{
			// No real roots, the sphere casts do not intersect.
			return false;
		}

		// Calculate the two possible solutions for t.
		float sqrtDiscriminant = MathF.Sqrt(discriminant);
		float t1 = (-b - sqrtDiscriminant) / (2 * a);
		float t2 = (-b + sqrtDiscriminant) / (2 * a);

		// Check if there is an intersection within the time interval [0, 1].
		return t1 is >= 0 and <= 1 || t2 is >= 0 and <= 1;
	}

	public static bool SphereCastAabb(SphereCast sphereCast, Aabb aabb)
	{
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		// Expand the AABB by the sphere's radius.
		Vector3 aabbMin = aabb.GetMin() - new Vector3(sphereCast.Radius);
		Vector3 aabbMax = aabb.GetMax() + new Vector3(sphereCast.Radius);

		// Perform ray-AABB intersection test.
		float tMin = 0.0f;
		float tMax = length;

		for (int i = 0; i < 3; i++)
		{
			if (MathF.Abs(direction[i]) < float.Epsilon)
			{
				// Ray is parallel to the slab. No hit if origin not within slab.
				if (sphereCast.Start[i] < aabbMin[i] || sphereCast.Start[i] > aabbMax[i])
					return false;
			}
			else
			{
				// Compute intersection t value of ray with near and far plane of slab.
				float ood = 1.0f / direction[i];
				float t1 = (aabbMin[i] - sphereCast.Start[i]) * ood;
				float t2 = (aabbMax[i] - sphereCast.Start[i]) * ood;

				// Make t1 be intersection with near plane, t2 with far plane.
				if (t1 > t2)
					(t1, t2) = (t2, t1);

				// Compute the intersection of slab intersection intervals.
				tMin = MathF.Max(tMin, t1);
				tMax = MathF.Min(tMax, t2);

				// Exit with no collision as soon as slab intersection becomes empty.
				if (tMin > tMax)
					return false;
			}
		}

		return true;
	}

	public static bool SphereCastObb(SphereCast sphereCast, Obb obb)
	{
		// Transform sphere start and end points to OBB's local space.
		Matrix3 invOrientation = Matrix3.Transpose(obb.Orientation);
		Vector3 localSphereStart = VectorUtils.Transform(sphereCast.Start - obb.Position, invOrientation);
		Vector3 localSphereEnd = VectorUtils.Transform(sphereCast.End - obb.Position, invOrientation);

		// Perform sphere cast against the AABB in local space.
		Aabb aabb = new(Vector3.Zero, obb.HalfExtents * 2);
		return SphereCastAabb(new SphereCast(localSphereStart, localSphereEnd, sphereCast.Radius), aabb);
	}

	public static bool SphereCastTriangle(SphereCast sphereCast, Triangle3D triangle)
	{
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		Vector3 m = sphereCast.Start - triangle.A;
		Vector3 n = Vector3.Cross(triangle.B - triangle.A, triangle.C - triangle.A);
		float d = Vector3.Dot(n, direction);
		float e = Vector3.Dot(n, m);

		// Check if the sphere's path is parallel to the triangle's plane.
		if (MathF.Abs(d) < float.Epsilon)
		{
			// Sphere is parallel to the triangle's plane.
			if (MathF.Abs(e) >= sphereCast.Radius)
				return false;
		}
		else
		{
			// Compute the intersection point of the sphere's path with the triangle's plane.
			float t = -e / d;
			if (t < 0 || t > length)
				return false;

			Vector3 point = sphereCast.Start + t * direction;
			if (PointInTriangle(point, triangle))
				return true;
		}

		// Check for intersection with the triangle's vertices and edges.
		return
			SphereCastPoint(sphereCast, triangle.A) ||
			SphereCastPoint(sphereCast, triangle.B) ||
			SphereCastPoint(sphereCast, triangle.C) ||
			SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.A, triangle.B)) ||
			SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.B, triangle.C)) ||
			SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.C, triangle.A));
	}

	public static bool SphereCastTriangle(SphereCast sphereCast, Triangle3D triangle, out Vector3 intersectionPoint)
	{
		intersectionPoint = default;
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();
		direction /= length; // Normalize the direction.

		Vector3 m = sphereCast.Start - triangle.A;
		Vector3 n = Vector3.Cross(triangle.B - triangle.A, triangle.C - triangle.A);
		float d = Vector3.Dot(n, direction);
		float e = Vector3.Dot(n, m);

		// Check if the sphere's path is parallel to the triangle's plane.
		if (MathF.Abs(d) < float.Epsilon)
		{
			// Sphere is parallel to the triangle's plane.
			if (MathF.Abs(e) >= sphereCast.Radius)
				return false;
		}
		else
		{
			// Compute the intersection point of the sphere's path with the triangle's plane.
			float t = -e / d;
			if (t < 0 || t > length)
				return false;

			Vector3 point = sphereCast.Start + t * direction;
			if (PointInTriangle(point, triangle))
			{
				intersectionPoint = point;
				return true;
			}
		}

		// Check for intersection with the triangle's vertices and edges.
		if (SphereCastPoint(sphereCast, triangle.A) ||
		    SphereCastPoint(sphereCast, triangle.B) ||
		    SphereCastPoint(sphereCast, triangle.C) ||
		    SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.A, triangle.B)) ||
		    SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.B, triangle.C)) ||
		    SphereCastLineSegment(sphereCast, new LineSegment3D(triangle.C, triangle.A)))
		{
			Plane plane = CreatePlaneFromTriangle(triangle);
			Vector3 closestStart = ClosestPointOnPlane(plane, sphereCast.Start);
			Vector3 closestEnd = ClosestPointOnPlane(plane, sphereCast.End);
			intersectionPoint = (closestStart + closestEnd) * 0.5f;
			return true;
		}

		return false;
	}

	#endregion Sphere casting

	#region Public helpers

	public static float PlaneEquation(Vector3 point, Plane plane)
	{
		return Vector3.Dot(point, plane.Normal) - plane.D;
	}

	public static Vector3 SatCrossEdge(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		Vector3 ab = a - b;
		Vector3 cd = c - d;
		Vector3 result = Vector3.Cross(ab, cd);
		if (result.LengthSquared() > float.Epsilon)
			return result;

		Vector3 axis = Vector3.Cross(ab, c - a);
		result = Vector3.Cross(ab, axis);
		if (result.LengthSquared() > float.Epsilon)
			return result;

		return Vector3.Zero;
	}

	public static Vector3 Barycentric(Vector3 point, Triangle3D triangle)
	{
		Vector3 ap = point - triangle.A;
		Vector3 bp = point - triangle.B;
		Vector3 cp = point - triangle.C;

		Vector3 ab = triangle.B - triangle.A;
		Vector3 ac = triangle.C - triangle.A;
		Vector3 bc = triangle.C - triangle.B;
		Vector3 cb = triangle.B - triangle.C;
		Vector3 ca = triangle.A - triangle.C;

		Vector3 v = ab - Project(ab, cb);
		float a = 1f - Vector3.Dot(v, ap) / Vector3.Dot(v, ab);

		v = bc - Project(bc, ac);
		float b = 1f - Vector3.Dot(v, bp) / Vector3.Dot(v, bc);

		v = ca - Project(ca, ab);
		float c = 1f - Vector3.Dot(v, cp) / Vector3.Dot(v, ca);

		return new Vector3(a, b, c);
	}

	public static Vector2 Project(Vector2 length, Vector2 direction)
	{
		float dot = Vector2.Dot(length, direction);
		float lengthSquared = direction.LengthSquared();
		return direction * (dot / lengthSquared);
	}

	public static Vector3 Project(Vector3 length, Vector3 direction)
	{
		float dot = Vector3.Dot(length, direction);
		float lengthSquared = direction.LengthSquared();
		return direction * (dot / lengthSquared);
	}

	public static Vector2 Perpendicular(Vector2 length, Vector2 direction)
	{
		return length - Project(length, direction);
	}

	public static Vector3 Perpendicular(Vector3 length, Vector3 direction)
	{
		return length - Project(length, direction);
	}

	public static bool FindCollisionFeatures(Sphere sphere1, Sphere sphere2, out CollisionManifold collisionManifold)
	{
		collisionManifold = CollisionManifold.Empty;

		float r = sphere1.Radius + sphere2.Radius;
		Vector3 d = sphere2.Position - sphere1.Position;
		if (d.LengthSquared() > r * r)
			return false;

		Vector3 direction = Vector3.Normalize(d); // TODO: Prevent NaN direction when spheres have the same position.
		collisionManifold.Normal = direction;
		collisionManifold.Depth = MathF.Abs(d.Length() - r) * 0.5f;
		float dtp = sphere1.Radius - collisionManifold.Depth;
		Vector3 contact = sphere1.Position + direction * dtp;
		collisionManifold.ContactCount = 1;
		collisionManifold.Contacts[0] = contact;
		return true;
	}

	public static bool FindCollisionFeatures(Obb obb, Sphere sphere, out CollisionManifold collisionManifold)
	{
		collisionManifold = CollisionManifold.Empty;

		Vector3 closestPoint = ClosestPointInObb(obb, sphere.Position);
		float distanceSquared = (closestPoint - sphere.Position).LengthSquared();
		if (distanceSquared > sphere.Radius * sphere.Radius)
			return false;

		Vector3 normal;
		if (distanceSquared is > -float.Epsilon and < float.Epsilon)
		{
			float mSq = (closestPoint - obb.Position).LengthSquared();
			if (mSq is > -float.Epsilon and < float.Epsilon)
				return false;

			normal = Vector3.Normalize(closestPoint - obb.Position);
		}
		else
		{
			normal = Vector3.Normalize(sphere.Position - closestPoint);
		}

		Vector3 outsidePoint = sphere.Position - normal * sphere.Radius;
		float distance = (closestPoint - outsidePoint).Length();
		collisionManifold.Normal = normal;
		collisionManifold.Depth = distance * 0.5f;
		collisionManifold.ContactCount = 1;
		collisionManifold.Contacts[0] = closestPoint + (outsidePoint - closestPoint) * 0.5f;
		return true;
	}

	public static bool FindCollisionFeatures(Obb obb1, Obb obb2, out CollisionManifold collisionManifold)
	{
		collisionManifold = CollisionManifold.Empty;

		Span<Vector3> test = stackalloc Vector3[15];
		test[0] = new Vector3(obb1.Orientation.M11, obb1.Orientation.M12, obb1.Orientation.M13);
		test[1] = new Vector3(obb1.Orientation.M21, obb1.Orientation.M22, obb1.Orientation.M23);
		test[2] = new Vector3(obb1.Orientation.M31, obb1.Orientation.M32, obb1.Orientation.M33);
		test[3] = new Vector3(obb2.Orientation.M11, obb2.Orientation.M12, obb2.Orientation.M13);
		test[4] = new Vector3(obb2.Orientation.M21, obb2.Orientation.M22, obb2.Orientation.M23);
		test[5] = new Vector3(obb2.Orientation.M31, obb2.Orientation.M32, obb2.Orientation.M33);
		for (int i = 0; i < 3; i++)
		{
			test[6 + i * 3 + 0] = Vector3.Cross(test[i], test[0]);
			test[6 + i * 3 + 1] = Vector3.Cross(test[i], test[1]);
			test[6 + i * 3 + 2] = Vector3.Cross(test[i], test[2]);
		}

		Vector3? hitNormal = null;
		for (int i = 0; i < test.Length; i++)
		{
			if (test[i].LengthSquared() < float.Epsilon)
				continue;

			float depth = PenetrationDepthObb(obb1, obb2, test[i], out bool shouldFlip);
			if (depth <= 0)
				return false;

			if (depth < collisionManifold.Depth)
			{
				if (shouldFlip)
					test[i] *= -1;

				collisionManifold.Depth = depth;
				hitNormal = test[i];
			}
		}

		if (hitNormal is null)
			return false;

		Vector3 axis = Vector3.Normalize(hitNormal.Value);
		int count1 = ClipEdgesToObb(obb2.GetEdges(), obb1, out Buffer12<Vector3> c1);
		int count2 = ClipEdgesToObb(obb1.GetEdges(), obb2, out Buffer12<Vector3> c2);
		collisionManifold.ContactCount = count1 + count2;
		for (int i = 0; i < count1; i++)
			collisionManifold.Contacts[i] = c1[i];
		for (int i = 0; i < count2; i++)
			collisionManifold.Contacts[count1 + i] = c2[i];

		Interval interval = obb1.GetInterval(axis);
		float distance = (interval.Max - interval.Min) * 0.5f - collisionManifold.Depth * 0.5f;
		Vector3 pointOnPlane = obb1.Position + axis * distance;
		for (int i = collisionManifold.ContactCount - 1; i >= 0; i--)
		{
			Vector3 contact = collisionManifold.Contacts[i];
			collisionManifold.Contacts[i] = contact + axis * Vector3.Dot(axis, pointOnPlane - contact);
		}

		// TODO: Check if this is necessary.
		// TODO: If it is, optimize it.
		// Remove duplicate contact points.
		// Buffer24<Vector3> finalContactPoints = default;
		// int finalContactCount = 0;
		// for (int i = 0; i < collisionManifold.ContactCount; i++)
		// {
		// 	Vector3 contact = collisionManifold.Contacts[i];
		// 	bool duplicate = false;
		// 	for (int j = 0; j < finalContactCount; j++)
		// 	{
		// 		if (Vector3.DistanceSquared(contact, finalContactPoints[j]) < float.Epsilon)
		// 		{
		// 			duplicate = true;
		// 			break;
		// 		}
		// 	}
		//
		// 	if (!duplicate)
		// 		finalContactPoints[finalContactCount++] = contact;
		// }
		//
		// collisionManifold.Contacts = finalContactPoints;
		// collisionManifold.ContactCount = finalContactCount;
		collisionManifold.Normal = axis;
		return true;
	}

	#endregion Public helpers

	#region Private helpers

	private static bool ClipToPlane(Plane plane, LineSegment3D line, out Vector3 result)
	{
		result = default;

		Vector3 ab = line.End - line.Start;
		float nAb = Vector3.Dot(plane.Normal, ab);
		if (nAb == 0)
			return false;

		float nA = Vector3.Dot(plane.Normal, line.Start);
		float t = (plane.D - nA) / nAb;

		if (t is < 0 or > 1)
			return false;

		result = line.Start + ab * t;
		return true;
	}

	private static int ClipEdgesToObb(Buffer12<LineSegment3D> edges, Obb obb, out Buffer12<Vector3> result)
	{
		int count = 0;
		result = default;
		Buffer6<Plane> planes = obb.GetPlanes();
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 12; j++)
			{
				if (count >= 12)
					throw new InvalidOperationException("Too many contacts.");

				if (ClipToPlane(planes[i], edges[j], out Vector3 intersection))
				{
					// The book uses PointInObb here, but that doesn't seem to work, since the points always lie just outside the obb after being clipped by the previous function.
					// I don't understand why this check is needed. If the edge clips successfully, the intersection point should always be on one of the obb planes?
					if (PointInObb(intersection, obb))
						result[count++] = intersection;
				}
			}
		}

		return count;
	}

	private static float PenetrationDepthObb(Obb obb1, Obb obb2, Vector3 axis, out bool shouldFlip)
	{
		shouldFlip = false;

		axis = Vector3.Normalize(axis);
		Interval i1 = obb1.GetInterval(axis);
		Interval i2 = obb2.GetInterval(axis);

		if (!(i2.Min <= i1.Max && i1.Min <= i2.Max))
			return 0;

		float len1 = i1.Max - i1.Min;
		float len2 = i2.Max - i2.Min;
		float min = Math.Min(i1.Min, i2.Min);
		float max = Math.Max(i1.Max, i2.Max);

		float length = max - min;
		shouldFlip = i2.Min < i1.Min;
		return len1 + len2 - length;
	}

	private static bool OverlapOnAxis(Aabb aabb, Obb obb, Vector3 axis)
	{
		Interval interval1 = aabb.GetInterval(axis);
		Interval interval2 = obb.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	private static bool OverlapOnAxis(Obb obb1, Obb obb2, Vector3 axis)
	{
		Interval interval1 = obb1.GetInterval(axis);
		Interval interval2 = obb2.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	private static bool OverlapOnAxis(Aabb aabb, Triangle3D triangle, Vector3 axis)
	{
		Interval interval1 = aabb.GetInterval(axis);
		Interval interval2 = triangle.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	private static bool OverlapOnAxis(Obb obb, Triangle3D triangle, Vector3 axis)
	{
		Interval interval1 = obb.GetInterval(axis);
		Interval interval2 = triangle.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	private static bool OverlapOnAxis(Triangle3D triangle1, Triangle3D triangle2, Vector3 axis)
	{
		Interval interval1 = triangle1.GetInterval(axis);
		Interval interval2 = triangle2.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	internal static Plane CreatePlaneFromTriangle(Triangle3D triangle)
	{
		Vector3 normal = Vector3.Normalize(Vector3.Cross(triangle.B - triangle.A, triangle.C - triangle.A));
		return new Plane(normal, Vector3.Dot(normal, triangle.A));
	}

	#endregion Private helpers
}
