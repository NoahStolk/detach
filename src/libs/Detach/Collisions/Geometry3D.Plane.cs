using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool PlanePlane(Plane plane1, Plane plane2)
	{
		Vector3 cross = Vector3.Cross(plane1.Normal, plane2.Normal);
		return cross.LengthSquared() > float.Epsilon;
	}
}
