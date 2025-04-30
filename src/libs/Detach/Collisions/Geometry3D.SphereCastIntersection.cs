using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool SphereCastObb(SphereCast sphereCast, Obb obb, out IntersectionResult result)
	{
		Matrix4x4 obbOrientation = obb.Orientation.ToMatrix4x4();
		Matrix4x4 inverseOrientation = Matrix4x4.Transpose(obbOrientation);

		// Transform sphere cast into OBB's local space
		Vector3 localStart = Vector3.Transform(sphereCast.Start - obb.Center, inverseOrientation);
		Vector3 localEnd = Vector3.Transform(sphereCast.End - obb.Center, inverseOrientation);
		Vector3 localDirection = localEnd - localStart;

		Vector3 expandedHalfExtents = obb.HalfExtents + new Vector3(sphereCast.Radius);
		Vector3 invDirection = new(
			1f / (localDirection.X == 0 ? float.Epsilon : localDirection.X),
			1f / (localDirection.Y == 0 ? float.Epsilon : localDirection.Y),
			1f / (localDirection.Z == 0 ? float.Epsilon : localDirection.Z));

		Vector3 min = -expandedHalfExtents;
		Vector3 max = expandedHalfExtents;

		float tmin = 0f;
		float tmax = 1f;

		for (int i = 0; i < 3; i++)
		{
			float start = localStart[i];
			float direction = localDirection[i];
			float minBound = min[i];
			float maxBound = max[i];

			if (Math.Abs(direction) < 1e-8f)
			{
				if (start < minBound || start > maxBound)
				{
					result = default;
					return false;
				}
			}
			else
			{
				float t1 = (minBound - start) * invDirection[i];
				float t2 = (maxBound - start) * invDirection[i];

				if (t1 > t2)
				{
					(t1, t2) = (t2, t1);
				}

				tmin = Math.Max(tmin, t1);
				tmax = Math.Min(tmax, t2);

				if (tmin > tmax)
				{
					result = default;
					return false;
				}
			}
		}

		Vector3 localHitPoint = localStart + localDirection * tmin;

		// Compute the normal from the closest face at the hit point
		Vector3 normal;
		Vector3 localPoint = Vector3.Clamp(localHitPoint, -obb.HalfExtents, obb.HalfExtents);
		Vector3 delta = localHitPoint - localPoint;

		float absX = Math.Abs(delta.X);
		float absY = Math.Abs(delta.Y);
		float absZ = Math.Abs(delta.Z);

		if (absX > absY && absX > absZ)
			normal = new Vector3(delta.X < 0 ? -1 : 1, 0, 0);
		else if (absY > absZ)
			normal = new Vector3(0, delta.Y < 0 ? -1 : 1, 0);
		else
			normal = new Vector3(0, 0, delta.Z < 0 ? -1 : 1);

		Vector3 worldNormal = Vector3.Normalize(Vector3.TransformNormal(normal, obbOrientation));
		Vector3 worldHit = Vector3.Transform(localHitPoint, obbOrientation) + obb.Center;
		float totalDistance = Vector3.Distance(sphereCast.Start, sphereCast.End);
		float penetration = (1.0f - tmin) * totalDistance;

		result = new IntersectionResult(worldNormal, worldHit, penetration);
		return true;
	}

	public static bool SphereCastSphere(SphereCast sphereCast, Sphere target, out IntersectionResult result)
	{
		Vector3 direction = sphereCast.End - sphereCast.Start;
		float length = direction.Length();

		float penetration;

		// If the cast has no length, fall back to static sphere check
		if (length < 1e-6f)
		{
			float distance = Vector3.Distance(sphereCast.Start, target.Center);
			float combinedRadius = sphereCast.Radius + target.Radius;

			if (distance <= combinedRadius)
			{
				Vector3 normal = Vector3.Normalize(target.Center - sphereCast.Start);
				Vector3 point = sphereCast.Start + normal * sphereCast.Radius;
				penetration = combinedRadius - distance;

				result = new IntersectionResult(normal, point, penetration);
				return true;
			}

			result = default;
			return false;
		}

		Vector3 dirNormalized = direction / length;
		Vector3 m = sphereCast.Start - target.Center;
		float r = sphereCast.Radius + target.Radius;

		float b = Vector3.Dot(m, dirNormalized);
		float c = Vector3.Dot(m, m) - r * r;

		// If the starting point is already intersecting
		if (c <= 0f)
		{
			Vector3 normal = Vector3.Normalize(m);
			Vector3 point = sphereCast.Start - normal * sphereCast.Radius;
			penetration = r;

			result = new IntersectionResult(normal, point, penetration);
			return true;
		}

		// If moving away from the target, no intersection
		if (b > 0f)
		{
			result = default;
			return false;
		}

		float discriminant = b * b - c;
		if (discriminant < 0f)
		{
			result = default;
			return false;
		}

		// Intersection occurs
		float t = -b - MathF.Sqrt(discriminant);
		if (t > length)
		{
			result = default;
			return false;
		}

		// Compute contact info
		Vector3 contactPoint = sphereCast.Start + dirNormalized * t;
		Vector3 contactNormal = Vector3.Normalize(contactPoint - target.Center);
		Vector3 intersectionPoint = contactPoint - contactNormal * sphereCast.Radius;

		penetration = (1.0f - t / length) * length;

		result = new IntersectionResult(contactNormal, intersectionPoint, penetration);
		return true;
	}
}
