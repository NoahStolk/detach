using System.Numerics;

namespace Detach.Collisions.Primitives;

public struct Circle
{
	public Vector2 Position;
	public float Radius;

	public Circle(Vector2 position, float radius)
	{
		Position = position;
		Radius = radius;
	}
}
