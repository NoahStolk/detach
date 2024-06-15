using System.Numerics;

namespace Detach.Collisions.Primitives2D;

public record struct Circle
{
	public Vector2 Position;
	public float Radius;

	public Circle(Vector2 position, float radius)
	{
		Position = position;
		Radius = radius;
	}
}
