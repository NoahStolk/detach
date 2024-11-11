using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct Cylinder
{
	// TODO: Rename to BaseCenter.
	public Vector3 BasePosition;
	public float Radius;
	public float Height;

	public Cylinder(Vector3 basePosition, float radius, float height)
	{
		BasePosition = basePosition;
		Radius = radius;
		Height = height;
	}
}
