using System.Numerics;

namespace Detach.Collisions.Primitives2D;

public record struct Triangle2D
{
	public Vector2 A;
	public Vector2 B;
	public Vector2 C;

	public Triangle2D(Vector2 a, Vector2 b, Vector2 c)
	{
		A = a;
		B = b;
		C = c;
	}

	public Vector2 this[int index]
	{
		get => index switch
		{
			0 => A,
			1 => B,
			2 => C,
			_ => throw new IndexOutOfRangeException(),
		};
		set
		{
			switch (index)
			{
				case 0: A = value; break;
				case 1: B = value; break;
				case 2: C = value; break;
				default: throw new IndexOutOfRangeException();
			}
		}
	}
}
