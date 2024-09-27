using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct Cylinder
{
	public Vector3 Position;
	public float Radius;
	public float Height;

	public Cylinder(Vector3 position, float radius, float height)
	{
		Position = position;
		Radius = radius;
		Height = height;
	}
}
