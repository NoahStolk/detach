using Detach.Collisions.Primitives2D;
using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry2D
{
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
}
