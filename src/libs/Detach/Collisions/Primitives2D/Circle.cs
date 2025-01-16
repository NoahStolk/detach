using System.Numerics;

namespace Detach.Collisions.Primitives2D;

public record struct Circle
{
	public Vector2 Center;
	public float Radius;

	public Circle(Vector2 center, float radius)
	{
		Center = center;
		Radius = radius;
	}
}
