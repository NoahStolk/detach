using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions.Primitives;

public record struct OrientedRectangle
{
	public Vector2 Position;
	public Vector2 HalfExtents;
	public float RotationInRadians;

	public OrientedRectangle(Vector2 position, Vector2 halfExtents, float rotationInRadians)
	{
		Position = position;
		HalfExtents = halfExtents;
		RotationInRadians = rotationInRadians;
	}

	public Interval2D GetInterval(Vector2 axis)
	{
		Rectangle rectangle = new(Position - HalfExtents, HalfExtents * 2);
		Vector2 min = rectangle.GetMin();
		Vector2 max = rectangle.GetMax();
		Span<Vector2> vertices = stackalloc Vector2[]
		{
			min,
			max,
			new Vector2(min.X, max.Y),
			new Vector2(max.X, min.Y),
		};
		Matrix2 zRotation = new(
			MathF.Cos(RotationInRadians), MathF.Sin(RotationInRadians),
			-MathF.Sin(RotationInRadians), MathF.Cos(RotationInRadians));

		// Rotate the vertices. This leaves us with the vertices of the oriented rectangle in world space.
		for (int i = 0; i < vertices.Length; i++)
			vertices[i] = Matrices.Multiply(vertices[i] - Position, zRotation) + Position;

		Interval2D result = default;
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
}
