using Detach.Buffers;
using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions.Primitives2D;

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

	public Buffer4<Vector2> GetVertices()
	{
		Rectangle rectangle = Rectangle.FromTopLeft(Position - HalfExtents, HalfExtents * 2);
		Vector2 min = rectangle.GetMin();
		Vector2 max = rectangle.GetMax();
		Buffer4<Vector2> vertices = default;
		vertices[0] = min;
		vertices[1] = max;
		vertices[2] = new Vector2(min.X, max.Y);
		vertices[3] = new Vector2(max.X, min.Y);
		Matrix2 zRotation = new(
			MathF.Cos(RotationInRadians), MathF.Sin(RotationInRadians),
			-MathF.Sin(RotationInRadians), MathF.Cos(RotationInRadians));

		// Rotate the vertices. This leaves us with the vertices of the oriented rectangle in world space.
		for (int i = 0; i < 4; i++)
			vertices[i] = Matrices.Multiply(vertices[i] - Position, zRotation) + Position;

		return vertices;
	}

	public Interval GetInterval(Vector2 axis)
	{
		Buffer4<Vector2> vertices = GetVertices();

		Interval result = default;
		float projection0 = Vector2.Dot(axis, vertices[0]);
		result.Min = projection0;
		result.Max = projection0;

		for (int i = 1; i < 4; i++)
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
