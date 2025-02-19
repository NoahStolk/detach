using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool CylinderCylinder(Cylinder cylinder1, Cylinder cylinder2)
	{
		// Project the cylinder's centers onto the XZ plane.
		Vector2 cylinder1Xz = new(cylinder1.BottomCenter.X, cylinder1.BottomCenter.Z);
		Vector2 cylinder2Xz = new(cylinder2.BottomCenter.X, cylinder2.BottomCenter.Z);

		// Check if the distance in the XZ plane is less than or equal to the sum of the radii.
		float distanceSquaredXz = Vector2.DistanceSquared(cylinder1Xz, cylinder2Xz);
		float radiusSum = cylinder1.Radius + cylinder2.Radius;
		if (distanceSquaredXz > radiusSum * radiusSum)
			return false;

		// Check if the cylinders' heights overlap.
		float cylinder1MinY = cylinder1.BottomCenter.Y;
		float cylinder1MaxY = cylinder1.BottomCenter.Y + cylinder1.Height;
		float cylinder2MinY = cylinder2.BottomCenter.Y;
		float cylinder2MaxY = cylinder2.BottomCenter.Y + cylinder2.Height;

		return cylinder1MaxY >= cylinder2MinY && cylinder1MinY <= cylinder2MaxY;
	}
}
