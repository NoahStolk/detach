using Detach.Buffers;
using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct Pyramid
{
	/// <summary>
	/// The center of the pyramid. This is halfway between the apex and the base.
	/// </summary>
	public Vector3 Center;

	/// <summary>
	/// The size of the pyramid as if it was an AABB.
	/// </summary>
	public Vector3 Size;

	public Pyramid(Vector3 center, Vector3 size)
	{
		Center = center;
		Size = size;
	}

	public Vector3 ApexVertex => Center + new Vector3(0, Size.Y / 2, 0);

	public Buffer4<Vector3> BaseVertices
	{
		get
		{
			Vector3 halfSize = Size / 2;
			return new Buffer4<Vector3>(
				Center + new Vector3(-halfSize.X, -halfSize.Y, -halfSize.Z),
				Center + new Vector3(halfSize.X, -halfSize.Y, -halfSize.Z),
				Center + new Vector3(halfSize.X, -halfSize.Y, halfSize.Z),
				Center + new Vector3(-halfSize.X, -halfSize.Y, halfSize.Z));
		}
	}

	public Buffer6<Triangle3D> Faces
	{
		get
		{
			Buffer4<Vector3> baseVertices = BaseVertices;
			return new Buffer6<Triangle3D>(
				new Triangle3D(baseVertices[0], baseVertices[1], ApexVertex),
				new Triangle3D(baseVertices[1], baseVertices[2], ApexVertex),
				new Triangle3D(baseVertices[2], baseVertices[3], ApexVertex),
				new Triangle3D(baseVertices[3], baseVertices[0], ApexVertex),
				new Triangle3D(baseVertices[0], baseVertices[1], baseVertices[2]),
				new Triangle3D(baseVertices[2], baseVertices[3], baseVertices[0]));
		}
	}
}
