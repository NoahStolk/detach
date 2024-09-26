using System.Numerics;

namespace Detach.Collisions.Primitives2D;

public record struct Rectangle
{
	/// <summary>
	/// The top-left corner of the rectangle.
	/// </summary>
	public Vector2 Position;

	/// <summary>
	/// The size of the rectangle.
	/// </summary>
	public Vector2 Size;

	private Rectangle(Vector2 topLeftPosition, Vector2 size)
	{
		Position = topLeftPosition;
		Size = size;
	}

	/// <summary>
	/// The center of the rectangle.
	/// </summary>
	public Vector2 Center => Position + Size / 2;

	/// <summary>
	/// Returns the top-left corner of the rectangle.
	/// </summary>
	public Vector2 GetMin()
	{
		Vector2 p1 = Position;
		Vector2 p2 = Position + Size;
		return new Vector2(MathF.Min(p1.X, p2.X), MathF.Min(p1.Y, p2.Y));
	}

	/// <summary>
	/// Returns the bottom-right corner of the rectangle.
	/// </summary>
	public Vector2 GetMax()
	{
		Vector2 p1 = Position;
		Vector2 p2 = Position + Size;
		return new Vector2(MathF.Max(p1.X, p2.X), MathF.Max(p1.Y, p2.Y));
	}

	/// <summary>
	/// Returns the interval of the rectangle projected onto the given axis.
	/// </summary>
	public Interval GetInterval(Vector2 axis)
	{
		Vector2 min = GetMin();
		Vector2 max = GetMax();

		Span<Vector2> vertices =
		[
			new(min.X, min.Y),
			new(min.X, max.Y),
			new(max.X, max.Y),
			new(max.X, min.Y),
		];

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

	/// <summary>
	/// Creates a rectangle from the given min and max points.
	/// </summary>
	public static Rectangle FromMinMax(Vector2 min, Vector2 max)
	{
		return new Rectangle(min, max - min);
	}

	/// <summary>
	/// Creates a rectangle from the given center and size.
	/// </summary>
	public static Rectangle FromCenter(Vector2 center, Vector2 size)
	{
		return new Rectangle(center - size / 2, size);
	}

	/// <summary>
	/// Creates a rectangle from the given top-left corner and size.
	/// </summary>
	public static Rectangle FromTopLeft(Vector2 topLeft, Vector2 size)
	{
		return new Rectangle(topLeft, size);
	}
}
