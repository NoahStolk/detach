using System.Numerics;

namespace Detach.Collisions.Primitives;

public record struct Rectangle
{
	public Vector2 Position;
	public Vector2 Size;

	public Rectangle(Vector2 position, Vector2 size)
	{
		Position = position;
		Size = size;
	}

	public Vector2 GetMin()
	{
		Vector2 p1 = Position;
		Vector2 p2 = Position + Size;
		return new Vector2(MathF.Min(p1.X, p2.X), MathF.Min(p1.Y, p2.Y));
	}

	public Vector2 GetMax()
	{
		Vector2 p1 = Position;
		Vector2 p2 = Position + Size;
		return new Vector2(MathF.Max(p1.X, p2.X), MathF.Max(p1.Y, p2.Y));
	}

	public Interval GetInterval(Vector2 axis)
	{
		Vector2 min = GetMin();
		Vector2 max = GetMax();

		Span<Vector2> vertices = stackalloc Vector2[]
		{
			new(min.X, min.Y),
			new(min.X, max.Y),
			new(max.X, max.Y),
			new(max.X, min.Y),
		};

		Interval result = default;
		float projection0 = Vector2.Dot(axis, vertices[0]);
		result.Min = projection0;
		result.Max = projection0;

		for (int i = 1; i < vertices.Length; i++)
		{
			float projection = Vector2.Dot(axis, vertices[i]);
			if (projection < result.Min)
				result.Min = projection;
			else if (projection > result.Max)
				result.Max = projection;
		}

		return result;
	}

	public static Rectangle FromMinMax(Vector2 min, Vector2 max)
	{
		return new Rectangle(min, max - min);
	}
}
