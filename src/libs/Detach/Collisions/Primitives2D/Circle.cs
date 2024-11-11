using System.Numerics;

namespace Detach.Collisions.Primitives2D;

public record struct Circle
{
	// TODO: Rename to Center.
	public Vector2 Position;
	public float Radius;

	public Circle(Vector2 position, float radius)
	{
		Position = position;
		Radius = radius;
	}
}
