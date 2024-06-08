using Detach.Buffers;
using System.Numerics;

namespace Detach.Collisions;

public struct CollisionManifold : IEquatable<CollisionManifold>
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

	public static bool operator ==(CollisionManifold left, CollisionManifold right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(CollisionManifold left, CollisionManifold right)
	{
		return !left.Equals(right);
	}

	public bool Equals(CollisionManifold other)
	{
		return Normal.Equals(other.Normal) && Depth.Equals(other.Depth) && ContactCount == other.ContactCount && Contacts.Equals(other.Contacts);
	}

	public override bool Equals(object? obj)
	{
		return obj is CollisionManifold other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Normal, Depth, ContactCount, Contacts);
	}
}
