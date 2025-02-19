using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool ObbObbSat(Obb obb1, Obb obb2)
	{
		Span<Vector3> axes = stackalloc Vector3[15];
		axes[0] = new Vector3(obb1.Orientation.M11, obb1.Orientation.M12, obb1.Orientation.M13);
		axes[1] = new Vector3(obb1.Orientation.M21, obb1.Orientation.M22, obb1.Orientation.M23);
		axes[2] = new Vector3(obb1.Orientation.M31, obb1.Orientation.M32, obb1.Orientation.M33);
		axes[3] = new Vector3(obb2.Orientation.M11, obb2.Orientation.M12, obb2.Orientation.M13);
		axes[4] = new Vector3(obb2.Orientation.M21, obb2.Orientation.M22, obb2.Orientation.M23);
		axes[5] = new Vector3(obb2.Orientation.M31, obb2.Orientation.M32, obb2.Orientation.M33);

		for (int i = 0; i < 3; i++)
		{
			axes[6 + i * 3 + 0] = Vector3.Cross(axes[i], axes[0]);
			axes[6 + i * 3 + 1] = Vector3.Cross(axes[i], axes[1]);
			axes[6 + i * 3 + 2] = Vector3.Cross(axes[i], axes[2]);
		}

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(obb1, obb2, axes[i]))
				return false;
		}

		return true;
	}

	public static bool ObbPlane(Obb obb, Plane plane)
	{
		Span<Vector3> axes =
		[
			new(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13),
			new(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23),
			new(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33),
		];

		float pLen =
			obb.HalfExtents.X * Math.Abs(Vector3.Dot(plane.Normal, axes[0])) +
			obb.HalfExtents.Y * Math.Abs(Vector3.Dot(plane.Normal, axes[1])) +
			obb.HalfExtents.Z * Math.Abs(Vector3.Dot(plane.Normal, axes[2]));

		float dot = Vector3.Dot(plane.Normal, obb.Center);
		float distance = dot - plane.D;
		return Math.Abs(distance) <= pLen;
	}
}
