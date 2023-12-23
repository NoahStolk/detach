using Detach.Buffers;
using System.Numerics;

namespace Detach.Collisions;

public struct CollisionManifold
{
	public bool Colliding;
	public Vector3 Normal;
	public float Depth;
	public int ContactCount;
	public Buffer144<Vector3> Contacts;
}
