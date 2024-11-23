using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct Triangle3D
{
	public Vector3 A;
	public Vector3 B;
	public Vector3 C;

	public Triangle3D(Vector3 a, Vector3 b, Vector3 c)
	{
		A = a;
		B = b;
		C = c;
	}

	public Vector3 this[int index]
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

	public Interval GetInterval(Vector3 axis)
	{
		Interval result = default;
		float projection0 = Vector3.Dot(axis, this[0]);
		result.Min = projection0;
		result.Max = projection0;

		for (int i = 1; i < 3; i++)
		{
			float projection = Vector3.Dot(axis, this[i]);
			if (projection < result.Min)
				result.Min = projection;
			else if (projection > result.Max)
				result.Max = projection;
		}

		return result;
	}

	public Vector3 GetNormal()
	{
		Vector3 normal = Vector3.Cross(B - A, C - A);
		return Vector3.Normalize(normal);
	}
}
