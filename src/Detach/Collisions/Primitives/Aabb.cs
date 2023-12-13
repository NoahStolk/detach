using System.Numerics;

namespace Detach.Collisions.Primitives;

public record struct Aabb
{
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

	public static Aabb FromMinMax(Vector3 min, Vector3 max)
	{
		return new Aabb((min + max) * 0.5f, (max - min) * 0.5f);
	}
}
