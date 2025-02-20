using Detach.Collisions.Primitives2D;
using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry2D
{
	public static bool RectangleRectangle(Rectangle rectangle1, Rectangle rectangle2)
	{
		Vector2 min1 = rectangle1.GetMin();
		Vector2 max1 = rectangle1.GetMax();
		Vector2 min2 = rectangle2.GetMin();
		Vector2 max2 = rectangle2.GetMax();

		return min1.X <= max2.X && max1.X >= min2.X && min1.Y <= max2.Y && max1.Y >= min2.Y;
	}

	public static bool RectangleRectangleSat(Rectangle rectangle1, Rectangle rectangle2)
	{
		Span<Vector2> axes = [new(1, 0), new(0, 1)];

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(rectangle1, rectangle2, axes[i]))
				return false;
		}

		return true;
	}

	public static bool RectangleOrientedRectangleSat(Rectangle rectangle, OrientedRectangle orientedRectangle)
	{
		Span<Vector2> axes = [new(1, 0), new(0, 1), default, default];

		Matrix2 zRotation = new(
			MathF.Cos(orientedRectangle.RotationInRadians), MathF.Sin(orientedRectangle.RotationInRadians),
			-MathF.Sin(orientedRectangle.RotationInRadians), MathF.Cos(orientedRectangle.RotationInRadians));

		Vector2 axis = Vector2.Normalize(orientedRectangle.HalfExtents with { Y = 0 });
		axes[2] = Matrices.Multiply(axis, zRotation);

		axis = Vector2.Normalize(orientedRectangle.HalfExtents with { X = 0 });
		axes[3] = Matrices.Multiply(axis, zRotation);

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(rectangle, orientedRectangle, axes[i]))
				return false;
		}

		return true;
	}

	public static bool RectangleTriangle(Rectangle rectangle, Triangle2D triangle)
	{
		if (PointInTriangle(rectangle.GetMin(), triangle) || PointInTriangle(rectangle.GetMax(), triangle))
			return true;

		LineSegment2D ab = new(triangle.A, triangle.B);
		LineSegment2D bc = new(triangle.B, triangle.C);
		LineSegment2D ca = new(triangle.C, triangle.A);
		return LineRectangle(ab, rectangle) || LineRectangle(bc, rectangle) || LineRectangle(ca, rectangle);
	}
}
