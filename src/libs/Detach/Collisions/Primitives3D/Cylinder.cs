using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct Cylinder
{
	public Vector3 BottomCenter;
	public float Radius;
	public float Height;

	public Cylinder(Vector3 bottomCenter, float radius, float height)
	{
		BottomCenter = bottomCenter;
		Radius = radius;
		Height = height;
	}
}
