using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct Ray
{
	public Vector3 Origin;
	public Vector3 Direction;

	public Ray(Vector3 origin, Vector3 direction)
	{
		Origin = origin;
		Direction = Vector3.Normalize(direction);
	}

	public static Ray FromPoints(Vector3 start, Vector3 end)
	{
		return new Ray(start, end - start);
	}
}
