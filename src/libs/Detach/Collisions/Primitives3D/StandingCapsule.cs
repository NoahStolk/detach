using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct StandingCapsule
{
	public Vector3 BottomCenter; // center of bottom sphere
	public float Radius;
	public float Height; // full height including cylindrical middle + caps

	public StandingCapsule(Vector3 bottomCenter, float radius, float height)
	{
		BottomCenter = bottomCenter;
		Radius = radius;
		Height = height;
	}

	public Vector3 TopCenter => BottomCenter + new Vector3(0, Height - 2 * Radius, 0);
}
