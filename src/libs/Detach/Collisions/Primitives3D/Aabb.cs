using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct Aabb
{
	// TODO: Rename to Center.
	public Vector3 Origin;
	public Vector3 Size;

	public Aabb(Vector3 origin, Vector3 size)
	{
		Origin = origin;
		Size = size;
	}

	public Vector3 GetMin()
	{
		Vector3 p1 = Origin + Size / 2;
		Vector3 p2 = Origin - Size / 2;

		return new Vector3(
			MathF.Min(p1.X, p2.X),
			MathF.Min(p1.Y, p2.Y),
			MathF.Min(p1.Z, p2.Z));
	}

	public Vector3 GetMax()
	{
		Vector3 p1 = Origin + Size / 2;
		Vector3 p2 = Origin - Size / 2;

		return new Vector3(
			MathF.Max(p1.X, p2.X),
			MathF.Max(p1.Y, p2.Y),
			MathF.Max(p1.Z, p2.Z));
	}

	public Interval GetInterval(Vector3 axis)
	{
		Vector3 min = GetMin();
		Vector3 max = GetMax();

		Span<Vector3> vertices =
		[
			new(min.X, min.Y, min.Z),
			new(min.X, min.Y, max.Z),
			new(min.X, max.Y, min.Z),
			new(min.X, max.Y, max.Z),
			new(max.X, min.Y, min.Z),
			new(max.X, min.Y, max.Z),
			new(max.X, max.Y, min.Z),
			new(max.X, max.Y, max.Z),
		];

		Interval result = default;
		float projection0 = Vector3.Dot(axis, vertices[0]);
		result.Min = projection0;
		result.Max = projection0;

		for (int i = 1; i < 8; i++)
		{
			float projection = Vector3.Dot(axis, vertices[i]);
			if (projection < result.Min)
				result.Min = projection;
			else if (projection > result.Max)
				result.Max = projection;
		}

		return result;
	}

	public static Aabb FromMinMax(Vector3 min, Vector3 max)
	{
		return new Aabb((min + max) * 0.5f, max - min);
	}
}
