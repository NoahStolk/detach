using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct Sphere
{
	public Vector3 Center;
	public float Radius;

	public Sphere(Vector3 center, float radius)
	{
		Center = center;
		Radius = radius;
	}
}
