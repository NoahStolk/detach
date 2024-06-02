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

		// TODO: Fix infinite slope.

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
		if (t is < 0 or > 1)
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

	public static bool RectangleRectangle(Rectangle rectangle1, Rectangle rectangle2)
	{
		Vector2 min1 = rectangle1.GetMin();
		Vector2 max1 = rectangle1.GetMax();
		Vector2 min2 = rectangle2.GetMin();
		Vector2 max2 = rectangle2.GetMax();

		return min1.X <= max2.X && max1.X >= min2.X && min1.Y <= max2.Y && max1.Y >= min2.Y;
	}

	private static bool OverlapOnAxis(Rectangle rectangle1, Rectangle rectangle2, Vector2 axis)
	{
		Interval interval1 = rectangle1.GetInterval(axis);
		Interval interval2 = rectangle2.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	private static bool OverlapOnAxis(Rectangle rectangle, OrientedRectangle orientedRectangle, Vector2 axis)
	{
		Interval interval1 = rectangle.GetInterval(axis);
		Interval interval2 = orientedRectangle.GetInterval(axis);
		return interval2.Min <= interval1.Max && interval1.Min <= interval2.Max;
	}

	public static bool RectangleRectangleSat(Rectangle rectangle1, Rectangle rectangle2)
	{
		Span<Vector2> axes = stackalloc Vector2[] { new(1, 0), new(0, 1) };

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(rectangle1, rectangle2, axes[i]))
				return false;
		}

		return true;
	}

	public static bool RectangleOrientedRectangleSat(Rectangle rectangle, OrientedRectangle orientedRectangle)
	{
		Span<Vector2> axes = stackalloc Vector2[] { new(1, 0), new(0, 1), default, default };

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

	public static bool OrientedRectangleOrientedRectangleSat(OrientedRectangle orientedRectangle1, OrientedRectangle orientedRectangle2)
	{
		Rectangle local1 = new(Vector2.Zero, orientedRectangle1.HalfExtents * 2);
		Vector2 rotVector = orientedRectangle2.Position - orientedRectangle1.Position;
		OrientedRectangle local2 = new(orientedRectangle2.Position, orientedRectangle2.HalfExtents, orientedRectangle2.RotationInRadians);
		local2.RotationInRadians -= orientedRectangle1.RotationInRadians;

		float theta = -orientedRectangle1.RotationInRadians;
		Matrix2 zRotation = new(
			MathF.Cos(theta), MathF.Sin(theta),
			-MathF.Sin(theta), MathF.Cos(theta));

		rotVector = Matrices.Multiply(rotVector, zRotation);
		local2.Position = rotVector + orientedRectangle1.HalfExtents;

		return RectangleOrientedRectangleSat(local1, local2);
	}

	public static Circle ContainingCircle(Vector2[] points)
	{
		if (points.Length == 0)
			throw new ArgumentException("The array must contain at least one point.", nameof(points));

		Vector2 center = Vector2.Zero;
		for (int i = 0; i < points.Length; i++)
			center += points[i];
		center /= points.Length;

		float radiusSquared = 0;
		for (int i = 0; i < points.Length; i++)
		{
			float distanceSquared = (points[i] - center).LengthSquared();
			if (distanceSquared > radiusSquared)
				radiusSquared = distanceSquared;
		}

		return new Circle(center, MathF.Sqrt(radiusSquared));
	}

	public static Rectangle ContainingRectangle(Vector2[] points)
	{
		if (points.Length == 0)
			throw new ArgumentException("The array must contain at least one point.", nameof(points));

		Vector2 min = points[0];
		Vector2 max = points[0];

		for (int i = 1; i < points.Length; i++)
		{
			if (points[i].X < min.X)
				min.X = points[i].X;
			else if (points[i].X > max.X)
				max.X = points[i].X;

			if (points[i].Y < min.Y)
				min.Y = points[i].Y;
			else if (points[i].Y > max.Y)
				max.Y = points[i].Y;
		}

		return Rectangle.FromMinMax(min, max);
	}
}
