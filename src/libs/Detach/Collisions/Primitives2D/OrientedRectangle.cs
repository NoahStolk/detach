using Detach.Buffers;
using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions.Primitives2D;

public record struct OrientedRectangle
{
	/// <summary>
	/// The center of the oriented rectangle.
	/// </summary>
	public Vector2 Center;

	/// <summary>
	/// The half-extents of the oriented rectangle.
	/// </summary>
	public Vector2 HalfExtents;

	/// <summary>
	/// The rotation of the oriented rectangle in radians.
	/// </summary>
	public float RotationInRadians;

	public OrientedRectangle(Vector2 center, Vector2 halfExtents, float rotationInRadians)
	{
		Center = center;
		HalfExtents = halfExtents;
		RotationInRadians = rotationInRadians;
	}

	/// <summary>
	/// Returns the vertices of the oriented rectangle in world space.
	/// </summary>
	public Buffer4<Vector2> GetVertices()
	{
		Rectangle rectangle = Rectangle.FromTopLeft(Center - HalfExtents, HalfExtents * 2);
		Vector2 min = rectangle.GetMin();
		Vector2 max = rectangle.GetMax();
		Buffer4<Vector2> vertices = default;
		vertices[0] = min;
		vertices[1] = new Vector2(min.X, max.Y);
		vertices[2] = max;
		vertices[3] = new Vector2(max.X, min.Y);
		Matrix2 zRotation = new(
			MathF.Cos(RotationInRadians), MathF.Sin(RotationInRadians),
			-MathF.Sin(RotationInRadians), MathF.Cos(RotationInRadians));

		// Rotate the vertices. This leaves us with the vertices of the oriented rectangle in world space.
		for (int i = 0; i < 4; i++)
			vertices[i] = Matrices.Multiply(vertices[i] - Center, zRotation) + Center;

		return vertices;
	}

	/// <summary>
	/// Returns the interval of the oriented rectangle projected onto the given axis.
	/// </summary>
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
