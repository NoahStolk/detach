using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct LineSegment3D
{
	public Vector3 Start;
	public Vector3 End;

	public LineSegment3D(Vector3 start, Vector3 end)
	{
		Start = start;
		End = end;
	}

	public float Length => Vector3.Distance(Start, End);
	public float LengthSquared => Vector3.DistanceSquared(Start, End);
}
