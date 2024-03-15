using System.Numerics;

namespace Detach.Collisions;

public record struct RaycastResult
{
	// TODO: Use get-only properties.
	public Vector3 Point;
	public Vector3 Normal;
	public float Distance;
}
