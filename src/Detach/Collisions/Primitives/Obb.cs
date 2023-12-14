using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions.Primitives;

public record struct Obb
{
	public Vector3 Position;
	public Vector3 Size;
	public Matrix3 Orientation;

	public Obb(Vector3 position, Vector3 size, Matrix3 orientation)
	{
		Position = position;
		Size = size;
		Orientation = orientation;
	}

	public Interval GetInterval(Vector3 axis)
	{
		Vector3 c = Position;
		Vector3 e = Size;
		Span<Vector3> axes = stackalloc Vector3[]
		{
			new(Orientation.M11, Orientation.M12, Orientation.M13),
			new(Orientation.M21, Orientation.M22, Orientation.M23),
			new(Orientation.M31, Orientation.M32, Orientation.M33),
		};

		Span<Vector3> vertices = stackalloc Vector3[]
		{
			c + axes[0] * e.X + axes[1] * e.Y + axes[2] * e.Z,
			c - axes[0] * e.X + axes[1] * e.Y + axes[2] * e.Z,
			c + axes[0] * e.X - axes[1] * e.Y + axes[2] * e.Z,
			c + axes[0] * e.X + axes[1] * e.Y - axes[2] * e.Z,
			c - axes[0] * e.X - axes[1] * e.Y - axes[2] * e.Z,
			c + axes[0] * e.X - axes[1] * e.Y - axes[2] * e.Z,
			c - axes[0] * e.X + axes[1] * e.Y - axes[2] * e.Z,
			c - axes[0] * e.X - axes[1] * e.Y + axes[2] * e.Z,
		};

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
}
