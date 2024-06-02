using Detach.Buffers;
using System.Numerics;

namespace Detach.Collisions;

public struct CollisionManifold
{
	public Vector3 Normal;
	public float Depth;
	public int ContactCount;
	public Buffer24<Vector3> Contacts;

	public static CollisionManifold Empty => new()
	{
		Normal = Vector3.UnitZ,
		Depth = float.MaxValue,
		ContactCount = 0,
	};
}
