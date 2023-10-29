using System.Numerics;

namespace Detach.Collisions;

public static class Sphere
{
	public static bool ContainsPoint(Vector3 sphereOrigin, float sphereRadius, Vector3 point)
	{
		return Vector3.DistanceSquared(sphereOrigin, point) <= sphereRadius * sphereRadius;
	}

	public static bool IntersectsSphere(Vector3 sphereOriginA, float sphereRadiusA, Vector3 sphereOriginB, float sphereRadiusB)
	{
		return Vector3.DistanceSquared(sphereOriginA, sphereOriginB) <= (sphereRadiusA + sphereRadiusB) * (sphereRadiusA + sphereRadiusB);
	}
}
