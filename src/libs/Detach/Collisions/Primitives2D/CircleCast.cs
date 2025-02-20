using System.Numerics;

namespace Detach.Collisions.Primitives2D;

public record struct CircleCast
{
	public Vector2 Start;
	public Vector2 End;
	public float Radius;

	public CircleCast(Vector2 start, Vector2 end, float radius)
	{
		Start = start;
		End = end;
		Radius = radius;
	}
}
