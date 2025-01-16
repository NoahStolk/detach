using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct ConeFrustum
{
	public Vector3 BottomCenter;
	public float BottomRadius;
	public float TopRadius;
	public float Height;

	public ConeFrustum(Vector3 bottomCenter, float bottomRadius, float topRadius, float height)
	{
		BottomCenter = bottomCenter;
		BottomRadius = bottomRadius;
		TopRadius = topRadius;
		Height = height;
	}
}
