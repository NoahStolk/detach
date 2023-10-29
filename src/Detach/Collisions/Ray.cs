using System.Numerics;

namespace Detach.Collisions;

public static class Ray
{
	public static (float Distance, int Axis)? IntersectsAxisAlignedBoundingBox(Vector3 rayPosition, Vector3 rayDirection, Vector3 aabbMin, Vector3 aabbMax)
	{
		const float epsilon = 1e-6f;

		float? tMin = null, tMax = null;

		int axis = 0;

		if (Math.Abs(rayDirection.X) < epsilon)
		{
			if (rayPosition.X < aabbMin.X || rayPosition.X > aabbMax.X)
				return null;
		}
		else
		{
			tMin = (aabbMin.X - rayPosition.X) / rayDirection.X;
			tMax = (aabbMax.X - rayPosition.X) / rayDirection.X;

			if (tMin > tMax)
				(tMin, tMax) = (tMax, tMin.Value);
		}

		if (Math.Abs(rayDirection.Y) < epsilon)
		{
			if (rayPosition.Y < aabbMin.Y || rayPosition.Y > aabbMax.Y)
				return null;
		}
		else
		{
			float tMinY = (aabbMin.Y - rayPosition.Y) / rayDirection.Y;
			float tMaxY = (aabbMax.Y - rayPosition.Y) / rayDirection.Y;

			if (tMinY > tMaxY)
				(tMaxY, tMinY) = (tMinY, tMaxY);

			if (tMin > tMaxY || tMinY > tMax)
				return null;

			if (!tMin.HasValue || tMinY > tMin)
			{
				tMin = tMinY;
				axis = 1;
			}

			if (!tMax.HasValue || tMaxY < tMax)
				tMax = tMaxY;
		}

		if (Math.Abs(rayDirection.Z) < epsilon)
		{
			if (rayPosition.Z < aabbMin.Z || rayPosition.Z > aabbMax.Z)
				return null;
		}
		else
		{
			float tMinZ = (aabbMin.Z - rayPosition.Z) / rayDirection.Z;
			float tMaxZ = (aabbMax.Z - rayPosition.Z) / rayDirection.Z;

			if (tMinZ > tMaxZ)
				(tMaxZ, tMinZ) = (tMinZ, tMaxZ);

			if (tMin > tMaxZ || tMinZ > tMax)
				return null;

			if (!tMin.HasValue || tMinZ > tMin)
			{
				tMin = tMinZ;
				axis = 2;
			}

			if (!tMax.HasValue || tMaxZ < tMax)
				tMax = tMaxZ;
		}

		return tMin switch
		{
			// Having a positive tMax and a negative tMin means the ray is inside the box.
			// We expect the intersection distance to be 0 in that case.
			< 0 when tMax > 0 => new(0, axis),

			// A negative tMin means that the intersection point is behind the ray's origin.
			// We discard these as not hitting the box.
			< 0 => null,
			_ => tMin.HasValue ? new(tMin.Value, axis) : null,
		};
	}

	public static Vector3? IntersectsTriangle(Vector3 rayPosition, Vector3 rayDirection, Vector3 triangleP1, Vector3 triangleP2, Vector3 triangleP3)
	{
		const float epsilon = 0.0000001f;

		Vector3 edge1 = triangleP2 - triangleP1;
		Vector3 edge2 = triangleP3 - triangleP1;
		Vector3 h = Vector3.Cross(rayDirection, edge2);
		float a = Vector3.Dot(edge1, h);
		if (a is > -epsilon and < epsilon)
			return null; // Ray is parallel to the triangle.

		float f = 1.0f / a;
		Vector3 s = rayPosition - triangleP1;
		float u = f * Vector3.Dot(s, h);
		if (u is < 0.0f or > 1.0f)
			return null;

		Vector3 q = Vector3.Cross(s, edge1);
		float v = f * Vector3.Dot(rayDirection, q);
		if (v < 0.0f || u + v > 1.0f)
			return null;

		// At this stage we can compute t to find out where the intersection point is on the line.
		float t = f * Vector3.Dot(edge2, q);
		if (t <= epsilon)
		{
			// This means that there is a line intersection but not a ray intersection.
			return null;
		}

		return rayPosition + rayDirection * t;
	}

	public static Vector3? IntersectsSphere(Vector3 rayPosition, Vector3 rayDirection, Vector3 sphereOrigin, float sphereRadius)
	{
		Vector3 l = sphereOrigin - rayPosition;
		float tca = Vector3.Dot(l, rayDirection);
		if (tca < 0)
			return null;

		float d2 = Vector3.Dot(l, l) - tca * tca;
		float r2 = sphereRadius * sphereRadius;
		if (d2 > r2)
			return null;

		float thc = MathF.Sqrt(r2 - d2);
		float t0 = tca - thc;
		float t1 = tca + thc;

		if (t0 > t1)
			(t0, t1) = (t1, t0);

		if (t0 < 0)
		{
			t0 = t1; // If t0 is negative, let's use t1 instead.
			if (t0 < 0)
				return null; // Both t0 and t1 are negative.
		}

		return rayPosition + rayDirection * t0;
	}
}
