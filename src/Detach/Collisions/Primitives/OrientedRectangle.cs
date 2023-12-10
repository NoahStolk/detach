using System.Numerics;

namespace Detach.Collisions.Primitives;

public struct OrientedRectangle
{
	public Vector2 Position;
	public Vector2 HalfExtents;
	public float Rotation;

	public OrientedRectangle(Vector2 position, Vector2 halfExtents, float rotation)
	{
		Position = position;
		HalfExtents = halfExtents;
		Rotation = rotation;
	}
}
