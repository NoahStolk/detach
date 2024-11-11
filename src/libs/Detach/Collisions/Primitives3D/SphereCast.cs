using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct SphereCast
{
	public Vector3 Start;
	public Vector3 End;
	public float Radius;

	public SphereCast(Vector3 start, Vector3 end, float radius)
	{
		Start = start;
		End = end;
		Radius = radius;
	}
}
