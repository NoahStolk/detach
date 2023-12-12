using Detach.Collisions.Primitives;
using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions;

public static class Geometry2D
{
	public static bool PointOnLine(Vector2 point, LineSegment2D line)
	{
		// Find the slope.
		float dy = line.End.Y - line.Start.Y;
		float dx = line.End.X - line.Start.X;
		float m = dy / dx;

		// Find the y-intercept.
		float b = line.Start.Y - m * line.Start.X;

		// Check if the point is on the line.
		return MathF.Abs(point.Y - (m * point.X + b)) < 0.0001f;
	}

	public static bool PointInCircle(Vector2 point, Circle circle)
	{
		LineSegment2D line = new(point, circle.Position);
		return line.LengthSquared <= circle.Radius * circle.Radius;
	}

	public static bool PointInRectangle(Vector2 point, Rectangle rectangle)
	{
		Vector2 min = rectangle.GetMin();
		Vector2 max = rectangle.GetMax();

		return point.X >= min.X && point.X <= max.X && point.Y >= min.Y && point.Y <= max.Y;
	}

	public static bool PointInOrientedRectangle(Vector2 point, OrientedRectangle rectangle)
	{
		Vector2 rotVector = point - rectangle.Position;
		float theta = -rectangle.RotationInRadians;
		Matrix2 zRotation = new(
			MathF.Cos(theta), MathF.Sin(theta),
			-MathF.Sin(theta), MathF.Cos(theta));
		rotVector = Matrices.Multiply(rotVector, zRotation);
		Rectangle localRectangle = new(Vector2.Zero, rectangle.HalfExtents * 2);
		Vector2 localPoint = rotVector + rectangle.HalfExtents;
		return PointInRectangle(localPoint, localRectangle);
	}

	public static bool LineCircle(LineSegment2D line, Circle circle)
	{
		Vector2 ab = line.End - line.Start;
		float t = Vector2.Dot(circle.Position - line.Start, ab) / Vector2.Dot(ab, ab);
		if (t < 0 || t > 1)
			return false;

		Vector2 closestPoint = line.Start + ab * t;

		LineSegment2D circleToClosest = new(circle.Position, closestPoint);
		return circleToClosest.LengthSquared <= circle.Radius * circle.Radius;
	}
}
