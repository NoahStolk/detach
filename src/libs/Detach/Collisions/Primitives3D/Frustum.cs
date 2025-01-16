using System.Numerics;

namespace Detach.Collisions.Primitives3D;

// TODO: This might need to be renamed to SquareFrustum or ViewFrustum.
public readonly record struct Frustum
{
	private const int _planeCount = 6;

	private readonly Plane _p0;
	private readonly Plane _p1;
	private readonly Plane _p2;
	private readonly Plane _p3;
	private readonly Plane _p4;
	private readonly Plane _p5;

	public Frustum(Matrix4x4 viewProjection)
	{
		for (int i = 0; i < _planeCount; i++)
			this[i] = CreatePlane(viewProjection, i);

		static Plane CreatePlane(Matrix4x4 viewFrustumProjection, int index)
		{
			Plane plane = index switch
			{
				0 => new Plane(-viewFrustumProjection.M13, -viewFrustumProjection.M23, -viewFrustumProjection.M33, -viewFrustumProjection.M43),
				1 => new Plane(viewFrustumProjection.M13 - viewFrustumProjection.M14, viewFrustumProjection.M23 - viewFrustumProjection.M24, viewFrustumProjection.M33 - viewFrustumProjection.M34, viewFrustumProjection.M43 - viewFrustumProjection.M44),
				2 => new Plane(-viewFrustumProjection.M14 - viewFrustumProjection.M11, -viewFrustumProjection.M24 - viewFrustumProjection.M21, -viewFrustumProjection.M34 - viewFrustumProjection.M31, -viewFrustumProjection.M44 - viewFrustumProjection.M41),
				3 => new Plane(viewFrustumProjection.M11 - viewFrustumProjection.M14, viewFrustumProjection.M21 - viewFrustumProjection.M24, viewFrustumProjection.M31 - viewFrustumProjection.M34, viewFrustumProjection.M41 - viewFrustumProjection.M44),
				4 => new Plane(viewFrustumProjection.M12 - viewFrustumProjection.M14, viewFrustumProjection.M22 - viewFrustumProjection.M24, viewFrustumProjection.M32 - viewFrustumProjection.M34, viewFrustumProjection.M42 - viewFrustumProjection.M44),
				5 => new Plane(-viewFrustumProjection.M14 - viewFrustumProjection.M12, -viewFrustumProjection.M24 - viewFrustumProjection.M22, -viewFrustumProjection.M34 - viewFrustumProjection.M32, -viewFrustumProjection.M44 - viewFrustumProjection.M42),
				_ => throw new ArgumentOutOfRangeException(nameof(index)),
			};

			float factor = 1f / plane.Normal.Length();
			plane.Normal.X *= factor;
			plane.Normal.Y *= factor;
			plane.Normal.Z *= factor;
			plane.D *= factor;

			return plane;
		}
	}

	public Plane this[int index]
	{
		get => index switch
		{
			0 => _p0,
			1 => _p1,
			2 => _p2,
			3 => _p3,
			4 => _p4,
			5 => _p5,
			_ => throw new IndexOutOfRangeException(),
		};
		private init
		{
			switch (index)
			{
				case 0: _p0 = value; break;
				case 1: _p1 = value; break;
				case 2: _p2 = value; break;
				case 3: _p3 = value; break;
				case 4: _p4 = value; break;
				case 5: _p5 = value; break;
				default: throw new IndexOutOfRangeException();
			}
		}
	}
}
