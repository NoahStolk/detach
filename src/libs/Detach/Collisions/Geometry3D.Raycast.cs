using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool Raycast(Sphere sphere, Ray ray, out float distance)
	{
		distance = 0;

		Vector3 e = sphere.Center - ray.Origin;
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

		Vector3 e = sphere.Center - ray.Origin;
		float radiusSquared = sphere.Radius * sphere.Radius;
		float eSquared = e.LengthSquared();
		float a = Vector3.Dot(e, ray.Direction);
		float bSquared = eSquared - a * a;
		float f = MathF.Sqrt(radiusSquared - bSquared);

		if (radiusSquared - (eSquared - a * a) < 0)
			return false;

		result.Distance = eSquared < radiusSquared ? a + f : a - f;
		result.Point = ray.Origin + ray.Direction * result.Distance;
		result.Normal = Vector3.Normalize(result.Point - sphere.Center);
		return true;
	}

	public static bool Raycast(Aabb aabb, Ray ray, out float distance)
	{
		distance = 0;

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
		distance = 0;

		Vector3 x = new(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13);
		Vector3 y = new(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23);
		Vector3 z = new(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33);
		Vector3 p = obb.Center - ray.Origin;
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
		Vector3 p = obb.Center - ray.Origin;
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
		distance = 0;

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
		distance = 0;

		Vector2 cylinderXz = new(cylinder.BottomCenter.X, cylinder.BottomCenter.Z);
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

		if (y1 < cylinder.BottomCenter.Y || y1 > cylinder.BottomCenter.Y + cylinder.Height)
			t1 = float.MaxValue;

		if (y2 < cylinder.BottomCenter.Y || y2 > cylinder.BottomCenter.Y + cylinder.Height)
			t2 = float.MaxValue;

		distance = Math.Min(t1, t2);
		return distance >= 0;
	}
}
