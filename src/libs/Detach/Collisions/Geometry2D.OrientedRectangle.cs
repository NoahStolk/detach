using Detach.Collisions.Primitives2D;
using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry2D
{
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
}
