using Detach.Buffers;
using System.Numerics;

namespace Detach.Collisions;

public struct CollisionManifold
{
	// TODO: Refactor and use out parameters instead of bool field.
	public bool Colliding;
	public Vector3 Normal;
	public float Depth;
	public int ContactCount;
	public Buffer24<Vector3> Contacts;

	public static CollisionManifold Empty => new()
	{
		Colliding = false,
		Normal = Vector3.UnitZ,
		Depth = float.MaxValue,
		ContactCount = 0,
	};
}
