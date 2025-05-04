using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct StandingCapsuleCast
{
	public Vector3 StartBottomCenter;
	public float Radius;
	public float Height;
	public Vector3 EndBottomCenter;

	public StandingCapsuleCast(Vector3 startBottom, Vector3 endBottom, float radius, float height)
	{
		StartBottomCenter = startBottom;
		EndBottomCenter = endBottom;
		Radius = radius;
		Height = height;
	}

	public Vector3 StartTopCenter => StartBottomCenter + new Vector3(0, Height - 2 * Radius, 0);
	public Vector3 EndTopCenter => EndBottomCenter + new Vector3(0, Height - 2 * Radius, 0);
}
