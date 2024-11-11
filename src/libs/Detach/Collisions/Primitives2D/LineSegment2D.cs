using System.Numerics;

namespace Detach.Collisions.Primitives2D;

public record struct LineSegment2D
{
	public Vector2 Start;
	public Vector2 End;

	public LineSegment2D(Vector2 start, Vector2 end)
	{
		Start = start;
		End = end;
	}

	public float Length => Vector2.Distance(Start, End);
	public float LengthSquared => Vector2.DistanceSquared(Start, End);
}
