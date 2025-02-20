using Detach.Collisions.Primitives2D;
using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry2D
{
	public static bool PointOnLine(Vector2 point, LineSegment2D line, float epsilon = 0.0001f)
	{
		// Find the slope.
		float dy = line.End.Y - line.Start.Y;
		float dx = line.End.X - line.Start.X;
		float m = dy / dx;

		// TODO: Fix infinite slope.

		// Find the y-intercept.
		float b = line.Start.Y - m * line.Start.X;

		// Check if the point is on the line.
		return MathF.Abs(point.Y - (m * point.X + b)) < epsilon;
	}

	public static bool PointInCircle(Vector2 point, Circle circle)
	{
		LineSegment2D line = new(point, circle.Center);
		return line.LengthSquared <= circle.Radius * circle.Radius;
	}

	public static bool PointInRectangle(Vector2 point, Rectangle rectangle)
	{
		Vector2 min = rectangle.GetMin();
		Vector2 max = rectangle.GetMax();

		return point.X >= min.X && point.X <= max.X && point.Y >= min.Y && point.Y <= max.Y;
	}

	public static bool PointInOrientedRectangle(Vector2 point, OrientedRectangle orientedRectangle)
	{
		Vector2 rotVector = point - orientedRectangle.Center;
		float theta = -orientedRectangle.RotationInRadians;
		Matrix2 zRotation = new(
			MathF.Cos(theta), MathF.Sin(theta),
			-MathF.Sin(theta), MathF.Cos(theta));
		rotVector = Matrices.Multiply(rotVector, zRotation);
		Rectangle localRectangle = Rectangle.FromTopLeft(Vector2.Zero, orientedRectangle.HalfExtents * 2);
		Vector2 localPoint = rotVector + orientedRectangle.HalfExtents;
		return PointInRectangle(localPoint, localRectangle);
	}

	public static bool PointInTriangle(Vector2 point, Triangle2D triangle)
	{
		Vector2 a = triangle.A;
		Vector2 b = triangle.B;
		Vector2 c = triangle.C;

		float asX = point.X - a.X;
		float asY = point.Y - a.Y;

		bool sAb = (b.X - a.X) * asY > (b.Y - a.Y) * asX;

		if ((c.X - a.X) * asY > (c.Y - a.Y) * asX == sAb)
			return false;

		return (c.X - b.X) * (point.Y - b.Y) > (c.Y - b.Y) * (point.X - b.X) == sAb;
	}
}
