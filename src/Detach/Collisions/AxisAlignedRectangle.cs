using System.Numerics;

namespace Detach.Collisions;

public static class AxisAlignedRectangle
{
	public static bool ContainsPoint(Vector2 axisAlignedRectangleMin, Vector2 axisAlignedRectangleMax, Vector2 point)
	{
		return point.X >= axisAlignedRectangleMin.X && point.X <= axisAlignedRectangleMax.X && point.Y >= axisAlignedRectangleMin.Y && point.Y <= axisAlignedRectangleMax.Y;
	}

	public static bool IntersectsOrContainsRectangle(Vector2 axisAlignedRectangleMinA, Vector2 axisAlignedRectangleMaxA, Vector2 axisAlignedRectangleMinB, Vector2 axisAlignedRectangleMaxB)
	{
		return axisAlignedRectangleMinA.X <= axisAlignedRectangleMaxB.X && axisAlignedRectangleMaxA.X >= axisAlignedRectangleMinB.X && axisAlignedRectangleMinA.Y <= axisAlignedRectangleMaxB.Y && axisAlignedRectangleMaxA.Y >= axisAlignedRectangleMinB.Y;
	}

	public static bool IntersectsOrContainsLineSegment(Vector2 axisAlignedRectangleMin, Vector2 axisAlignedRectangleMax, Vector2 lineSegmentP1, Vector2 lineSegmentP2)
	{
		// Find min and max X for the segment
		float minX = lineSegmentP1.X;
		float maxX = lineSegmentP2.X;

		if (lineSegmentP1.X > lineSegmentP2.X)
		{
			minX = lineSegmentP2.X;
			maxX = lineSegmentP1.X;
		}

		// Find the intersection of the segment's and rectangle's x-projections
		if (maxX > axisAlignedRectangleMax.X)
			maxX = axisAlignedRectangleMax.X;

		if (minX < axisAlignedRectangleMin.X)
			minX = axisAlignedRectangleMin.X;

		// If their projections do not intersect, return false
		if (minX > maxX)
			return false;

		// Find corresponding min and max Y for min and max X we found before
		float minY = lineSegmentP1.Y;
		float maxY = lineSegmentP2.Y;

		float dx = lineSegmentP2.X - lineSegmentP1.X;

		if (MathF.Abs(dx) > 0.0000001f)
		{
			float a = (lineSegmentP2.Y - lineSegmentP1.Y) / dx;
			float b = lineSegmentP1.Y - a * lineSegmentP1.X;
			minY = a * minX + b;
			maxY = a * maxX + b;
		}

		if (minY > maxY)
			(maxY, minY) = (minY, maxY);

		// Find the intersection of the segment's and rectangle's y-projections
		if (maxY > axisAlignedRectangleMax.Y)
			maxY = axisAlignedRectangleMax.Y;

		if (minY < axisAlignedRectangleMin.Y)
			minY = axisAlignedRectangleMin.Y;

		// If Y-projections do not intersect, return false
		return minY <= maxY;
	}
}
