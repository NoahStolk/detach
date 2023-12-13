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

	public static bool PointInOrientedRectangle(Vector2 point, OrientedRectangle orientedRectangle)
	{
		Vector2 rotVector = point - orientedRectangle.Position;
		float theta = -orientedRectangle.RotationInRadians;
		Matrix2 zRotation = new(
			MathF.Cos(theta), MathF.Sin(theta),
			-MathF.Sin(theta), MathF.Cos(theta));
		rotVector = Matrices.Multiply(rotVector, zRotation);
		Rectangle localRectangle = new(Vector2.Zero, orientedRectangle.HalfExtents * 2);
		Vector2 localPoint = rotVector + orientedRectangle.HalfExtents;
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

	public static bool LineRectangle(LineSegment2D line, Rectangle rectangle)
	{
		if (PointInRectangle(line.Start, rectangle) || PointInRectangle(line.End, rectangle))
			return true;

		Vector2 norm = Vector2.Normalize(line.End - line.Start);
		norm.X = (norm.X != 0) ? 1 / norm.X : 0;
		norm.Y = (norm.Y != 0) ? 1 / norm.Y : 0;
		Vector2 min = (rectangle.GetMin() - line.Start) * norm;
		Vector2 max = (rectangle.GetMax() - line.Start) * norm;

		float tMin = MathF.Max(MathF.Min(min.X, max.X), MathF.Min(min.Y, max.Y));
		float tMax = MathF.Min(MathF.Max(min.X, max.X), MathF.Max(min.Y, max.Y));
		if (tMax < 0 || tMin > tMax)
			return false;

		float t = (tMin < 0) ? tMax : tMin;
		return t > 0 && t * t < line.LengthSquared;
	}

	public static bool LineOrientedRectangle(LineSegment2D line, OrientedRectangle orientedRectangle)
	{
		float theta = -orientedRectangle.RotationInRadians;
		Matrix2 zRotation = new(
			MathF.Cos(theta), MathF.Sin(theta),
			-MathF.Sin(theta), MathF.Cos(theta));

		LineSegment2D localLine = new(
			Matrices.Multiply(line.Start - orientedRectangle.Position, zRotation) + orientedRectangle.HalfExtents,
			Matrices.Multiply(line.End - orientedRectangle.Position, zRotation) + orientedRectangle.HalfExtents);
		Rectangle localRectangle = new(Vector2.Zero, orientedRectangle.HalfExtents * 2);
		return LineRectangle(localLine, localRectangle);
	}

	public static bool CircleCircle(Circle circle1, Circle circle2)
	{
		LineSegment2D line = new(circle1.Position, circle2.Position);
		float radii = circle1.Radius + circle2.Radius;
		return line.LengthSquared <= radii * radii;
	}

	public static bool CircleRectangle(Circle circle, Rectangle rectangle)
	{
		if (PointInRectangle(circle.Position, rectangle))
			return true;

		Vector2 min = rectangle.GetMin();
		Vector2 max = rectangle.GetMax();

		Vector2 closestPoint = new(
			Math.Clamp(circle.Position.X, min.X, max.X),
			Math.Clamp(circle.Position.Y, min.Y, max.Y));

		LineSegment2D circleToClosest = new(circle.Position, closestPoint);
		return circleToClosest.LengthSquared <= circle.Radius * circle.Radius;
	}

	public static bool CircleOrientedRectangle(Circle circle, OrientedRectangle orientedRectangle)
	{
		float theta = -orientedRectangle.RotationInRadians;
		Matrix2 zRotation = new(
			MathF.Cos(theta), MathF.Sin(theta),
			-MathF.Sin(theta), MathF.Cos(theta));

		Circle localCircle = new(
			Matrices.Multiply(circle.Position - orientedRectangle.Position, zRotation) + orientedRectangle.HalfExtents,
			circle.Radius);
		Rectangle localRectangle = new(Vector2.Zero, orientedRectangle.HalfExtents * 2);
		return CircleRectangle(localCircle, localRectangle);
	}
}
