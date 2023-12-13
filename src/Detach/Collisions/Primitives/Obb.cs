using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions.Primitives;

public record struct Obb
{
	public Vector3 Position;
	public Vector3 Size;
	public Matrix3 Orientation;

	public Obb(Vector3 position, Vector3 size, Matrix3 orientation)
	{
		Position = position;
		Size = size;
		Orientation = orientation;
	}
}
