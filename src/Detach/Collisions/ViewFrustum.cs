using System.Numerics;

namespace Detach.Collisions;

public static class ViewFrustum
{
	private const int _planeCount = 6;

	public static bool ContainsPoint(Matrix4x4 viewFrustumProjection, Vector3 point)
	{
		for (int i = 0; i < _planeCount; i++)
		{
			Plane plane = CreatePlane(viewFrustumProjection, i);
			if (point.X * plane.Normal.X + point.Y * plane.Normal.Y + point.Z * plane.Normal.Z + plane.D > 0)
				return false;
		}

		return true;
	}

	private static Plane CreatePlane(Matrix4x4 viewFrustumProjection, int index)
	{
		Plane plane = index switch
		{
			0 => new(-viewFrustumProjection.M13, -viewFrustumProjection.M23, -viewFrustumProjection.M33, -viewFrustumProjection.M43),
			1 => new(viewFrustumProjection.M13 - viewFrustumProjection.M14, viewFrustumProjection.M23 - viewFrustumProjection.M24, viewFrustumProjection.M33 - viewFrustumProjection.M34, viewFrustumProjection.M43 - viewFrustumProjection.M44),
			2 => new(-viewFrustumProjection.M14 - viewFrustumProjection.M11, -viewFrustumProjection.M24 - viewFrustumProjection.M21, -viewFrustumProjection.M34 - viewFrustumProjection.M31, -viewFrustumProjection.M44 - viewFrustumProjection.M41),
			3 => new(viewFrustumProjection.M11 - viewFrustumProjection.M14, viewFrustumProjection.M21 - viewFrustumProjection.M24, viewFrustumProjection.M31 - viewFrustumProjection.M34, viewFrustumProjection.M41 - viewFrustumProjection.M44),
			4 => new(viewFrustumProjection.M12 - viewFrustumProjection.M14, viewFrustumProjection.M22 - viewFrustumProjection.M24, viewFrustumProjection.M32 - viewFrustumProjection.M34, viewFrustumProjection.M42 - viewFrustumProjection.M44),
			5 => new(-viewFrustumProjection.M14 - viewFrustumProjection.M12, -viewFrustumProjection.M24 - viewFrustumProjection.M22, -viewFrustumProjection.M34 - viewFrustumProjection.M32, -viewFrustumProjection.M44 - viewFrustumProjection.M42),
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
