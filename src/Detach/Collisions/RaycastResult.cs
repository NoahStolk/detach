using System.Numerics;

namespace Detach.Collisions;

public record struct RaycastResult
{
	public Vector3 Point;
	public Vector3 Normal;
	public float Distance;
}
