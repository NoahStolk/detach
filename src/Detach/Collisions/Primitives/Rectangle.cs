using System.Numerics;

namespace Detach.Collisions.Primitives;

public struct Rectangle
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
		return new(MathF.Min(p1.X, p2.X), MathF.Min(p1.Y, p2.Y));
	}

	public Vector2 GetMax()
	{
		Vector2 p1 = Position;
		Vector2 p2 = Position + Size;
		return new(MathF.Max(p1.X, p2.X), MathF.Max(p1.Y, p2.Y));
	}

	public static Rectangle FromMinMax(Vector2 min, Vector2 max)
	{
		return new Rectangle(min, max - min);
	}
}
