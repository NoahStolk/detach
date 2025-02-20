using Detach.Collisions.Primitives2D;
using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry2D
{
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
}
