using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool AabbAabb(Aabb aabb1, Aabb aabb2)
	{
		Vector3 min1 = aabb1.GetMin();
		Vector3 max1 = aabb1.GetMax();
		Vector3 min2 = aabb2.GetMin();
		Vector3 max2 = aabb2.GetMax();

		return min1.X <= max2.X && max1.X >= min2.X
			&& min1.Y <= max2.Y && max1.Y >= min2.Y
			&& min1.Z <= max2.Z && max1.Z >= min2.Z;
	}

	public static bool AabbObbSat(Aabb aabb, Obb obb)
	{
		Span<Vector3> axes = stackalloc Vector3[15];
		axes[0] = new Vector3(1, 0, 0);
		axes[1] = new Vector3(0, 1, 0);
		axes[2] = new Vector3(0, 0, 1);
		axes[3] = new Vector3(obb.Orientation.M11, obb.Orientation.M12, obb.Orientation.M13);
		axes[4] = new Vector3(obb.Orientation.M21, obb.Orientation.M22, obb.Orientation.M23);
		axes[5] = new Vector3(obb.Orientation.M31, obb.Orientation.M32, obb.Orientation.M33);

		for (int i = 0; i < 3; i++)
		{
			axes[6 + i * 3 + 0] = Vector3.Cross(axes[i], axes[0]);
			axes[6 + i * 3 + 1] = Vector3.Cross(axes[i], axes[1]);
			axes[6 + i * 3 + 2] = Vector3.Cross(axes[i], axes[2]);
		}

		for (int i = 0; i < axes.Length; i++)
		{
			if (!OverlapOnAxis(aabb, obb, axes[i]))
				return false;
		}

		return true;
	}

	public static bool AabbPlane(Aabb aabb, Plane plane)
	{
		float pLen =
			aabb.Size.X * Math.Abs(plane.Normal.X) +
			aabb.Size.Y * Math.Abs(plane.Normal.Y) +
			aabb.Size.Z * Math.Abs(plane.Normal.Z);

		float dot = Vector3.Dot(plane.Normal, aabb.Center);
		float distance = dot - plane.D;
		return Math.Abs(distance) <= pLen;
	}

	public static bool AabbCylinder(Aabb aabb, Cylinder cylinder)
	{
		// Project the cylinder's center onto the XZ plane.
		Vector2 cylinderXz = new(cylinder.BottomCenter.X, cylinder.BottomCenter.Z);
		Vector2 aabbXz = new(aabb.Center.X, aabb.Center.Z);

		// Check if the distance in the XZ plane is less than or equal to the sum of the radii.
		float distanceSquaredXz = Vector2.DistanceSquared(cylinderXz, aabbXz);
		float radiusSum = cylinder.Radius + Math.Max(aabb.Size.X, aabb.Size.Z) / 2;
		if (distanceSquaredXz > radiusSum * radiusSum)
			return false;

		// Check if the cylinder's height overlaps with the AABB.
		float cylinderMinY = cylinder.BottomCenter.Y;
		float cylinderMaxY = cylinder.BottomCenter.Y + cylinder.Height;
		float aabbMinY = aabb.Center.Y - aabb.Size.Y / 2;
		float aabbMaxY = aabb.Center.Y + aabb.Size.Y / 2;

		return cylinderMaxY >= aabbMinY && cylinderMinY <= aabbMaxY;
	}
}
