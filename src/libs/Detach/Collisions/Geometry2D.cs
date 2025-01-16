using Detach.Collisions.Primitives2D;
using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions;

public static class Geometry2D
{
	#region Point vs primitives

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

	#endregion Point vs primitives

	#region Line vs primitives

	public static bool LineLine(LineSegment2D line1, LineSegment2D line2)
	{
		Vector2 p = line1.Start;
		Vector2 r = line1.End - line1.Start;
		Vector2 q = line2.Start;
		Vector2 s = line2.End - line2.Start;

		float rCrossS = Cross(r, s);
		if (rCrossS == 0)
			return Cross(q - p, r) == 0;

		float t = Cross(q - p, s) / rCrossS;
		float u = Cross(q - p, r) / rCrossS;

		return t is >= 0 and <= 1 && u is >= 0 and <= 1;

		float Cross(Vector2 v1, Vector2 v2)
		{
			return v1.X * v2.Y - v1.Y * v2.X;
		}
	}

	public static bool LineCircle(LineSegment2D line, Circle circle)
	{
		Vector2 ab = line.End - line.Start;
		float t = Vector2.Dot(circle.Center - line.Start, ab) / Vector2.Dot(ab, ab);
		t = Math.Clamp(t, 0, 1); // Clamp t to the range [0, 1] to ensure the closest point is on the segment.

		Vector2 closestPoint = line.Start + t * ab;
		float distanceSquared = Vector2.DistanceSquared(closestPoint, circle.Center);

		return distanceSquared <= circle.Radius * circle.Radius;
	}

	public static bool LineRectangle(LineSegment2D line, Rectangle rectangle)
	{
		if (PointInRectangle(line.Start, rectangle) || PointInRectangle(line.End, rectangle))
			return true;

		Vector2 norm = Vector2.Normalize(line.End - line.Start);
		norm.X = norm.X != 0 ? 1 / norm.X : 0;
		norm.Y = norm.Y != 0 ? 1 / norm.Y : 0;
		Vector2 min = (rectangle.GetMin() - line.Start) * norm;
		Vector2 max = (rectangle.GetMax() - line.Start) * norm;

		float tMin = MathF.Max(MathF.Min(min.X, max.X), MathF.Min(min.Y, max.Y));
		float tMax = MathF.Min(MathF.Max(min.X, max.X), MathF.Max(min.Y, max.Y));
		if (tMax < 0 || tMin > tMax)
			return false;

		float t = tMin < 0 ? tMax : tMin;
		return t > 0 && t * t < line.LengthSquared;
	}

	public static bool LineOrientedRectangle(LineSegment2D line, OrientedRectangle orientedRectangle)
	{
		float theta = -orientedRectangle.RotationInRadians;
		Matrix2 zRotation = new(
			MathF.Cos(theta), MathF.Sin(theta),
			-MathF.Sin(theta), MathF.Cos(theta));

		LineSegment2D localLine = new(
			Matrices.Multiply(line.Start - orientedRectangle.Center, zRotation) + orientedRectangle.HalfExtents,
			Matrices.Multiply(line.End - orientedRectangle.Center, zRotation) + orientedRectangle.HalfExtents);
		Rectangle localRectangle = Rectangle.FromTopLeft(Vector2.Zero, orientedRectangle.HalfExtents * 2);
		return LineRectangle(localLine, localRectangle);
	}

	public static bool LineTriangle(LineSegment2D line, Triangle2D triangle)
	{
		if (PointInTriangle(line.Start, triangle) || PointInTriangle(line.End, triangle))
			return true;

		LineSegment2D ab = new(triangle.A, triangle.B);
		LineSegment2D bc = new(triangle.B, triangle.C);
		LineSegment2D ca = new(triangle.C, triangle.A);
		return LineLine(line, ab) || LineLine(line, bc) || LineLine(line, ca);
	}

	#endregion Line vs primitives

	#region Circle vs primitives

	public static bool CircleCircle(Circle circle1, Circle circle2)
	{
		LineSegment2D line = new(circle1.Center, circle2.Center);
		float radii = circle1.Radius + circle2.Radius;
		return line.LengthSquared <= radii * radii;
	}

	public static bool CircleRectangle(Circle circle, Rectangle rectangle)
	{
		if (PointInRectangle(circle.Center, rectangle))
			return true;

		Vector2 min = rectangle.GetMin();
		Vector2 max = rectangle.GetMax();

		Vector2 closestPoint = new(
			Math.Clamp(circle.Center.X, min.X, max.X),
			Math.Clamp(circle.Center.Y, min.Y, max.Y));

		LineSegment2D circleToClosest = new(circle.Center, closestPoint);
		return circleToClosest.LengthSquared <= circle.Radius * circle.Radius;
	}

	public static bool CircleOrientedRectangle(Circle circle, OrientedRectangle orientedRectangle)
	{
		float theta = -orientedRectangle.RotationInRadians;
		Matrix2 zRotation = new(
			MathF.Cos(theta), MathF.Sin(theta),
			-MathF.Sin(theta), MathF.Cos(theta));

		Circle localCircle = new(
			Matrices.Multiply(circle.Center - orientedRectangle.Center, zRotation) + orientedRectangle.HalfExtents,
			circle.Radius);
		Rectangle localRectangle = Rectangle.FromTopLeft(Vector2.Zero, orientedRectangle.HalfExtents * 2);
		return CircleRectangle(localCircle, localRectangle);
	}

	public static bool CircleTriangle(Circle circle, Triangle2D triangle)
	{
		if (PointInTriangle(circle.Center, triangle))
			return true;

		LineSegment2D ab = new(triangle.A, triangle.B);
		LineSegment2D bc = new(triangle.B, triangle.C);
		LineSegment2D ca = new(triangle.C, triangle.A);
		return LineCircle(ab, circle) || LineCircle(bc, circle) || LineCircle(ca, circle);
	}

	#endregion Circle vs primitives

	#region Rectangle vs primitives

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

	#endregion Rectangle vs primitives

	#region Oriented rectangle vs primitives

	public static bool OrientedRectangleOrientedRectangleSat(OrientedRectangle orientedRectangle1, OrientedRectangle orientedRectangle2)
	{
		Rectangle local1 = Rectangle.FromTopLeft(Vector2.Zero, orientedRectangle1.HalfExtents * 2);
		Vector2 rotVector = orientedRectangle2.Center - orientedRectangle1.Center;
		OrientedRectangle local2 = new(orientedRectangle2.Center, orientedRectangle2.HalfExtents, orientedRectangle2.RotationInRadians);
		local2.RotationInRadians -= orientedRectangle1.RotationInRadians;

		float theta = -orientedRectangle1.RotationInRadians;
		Matrix2 zRotation = new(
			MathF.Cos(theta), MathF.Sin(theta),
			-MathF.Sin(theta), MathF.Cos(theta));

		rotVector = Matrices.Multiply(rotVector, zRotation);
		local2.Center = rotVector + orientedRectangle1.HalfExtents;

		return RectangleOrientedRectangleSat(local1, local2);
	}

	public static bool OrientedRectangleTriangle(OrientedRectangle orientedRectangle, Triangle2D triangle)
	{
		if (PointInTriangle(orientedRectangle.Center, triangle))
			return true;

		LineSegment2D ab = new(triangle.A, triangle.B);
		LineSegment2D bc = new(triangle.B, triangle.C);
		LineSegment2D ca = new(triangle.C, triangle.A);
		return LineOrientedRectangle(ab, orientedRectangle) || LineOrientedRectangle(bc, orientedRectangle) || LineOrientedRectangle(ca, orientedRectangle);
	}

	#endregion Oriented rectangle vs primitives

	#region Public helpers

	public static Circle ContainingCircle(ReadOnlySpan<Vector2> points)
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

	public static Rectangle ContainingRectangle(ReadOnlySpan<Vector2> points)
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

	#endregion Public helpers

	#region Private helpers

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

	#endregion Private helpers
}
