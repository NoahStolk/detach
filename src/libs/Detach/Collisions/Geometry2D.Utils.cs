using Detach.Collisions.Primitives2D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry2D
{
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
}
