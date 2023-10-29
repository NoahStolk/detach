using System.Numerics;

namespace Detach.Collisions;

public static class Triangle
{
	public static (Vector3 CollisionForce, Vector3 IntersectionPoint)? IntersectsSphere(Vector3 triangleP1, Vector3 triangleP2, Vector3 triangleP3, Vector3 sphereOrigin, float sphereRadius)
	{
		Vector3 normal = GetNormal(triangleP1, triangleP2, triangleP3);
		float signedDistance = Vector3.Dot(sphereOrigin - triangleP1, normal); // Signed distance between sphere and plane.
		if (signedDistance < -sphereRadius || signedDistance > sphereRadius)
			return null;

		Vector3 point0 = sphereOrigin - normal * signedDistance; // Projected sphere center on triangle plane.

		// Now determine whether point0 is inside all triangle edges:
		Vector3 c0 = Vector3.Cross(point0 - triangleP1, triangleP2 - triangleP1);
		Vector3 c1 = Vector3.Cross(point0 - triangleP2, triangleP3 - triangleP2);
		Vector3 c2 = Vector3.Cross(point0 - triangleP3, triangleP1 - triangleP3);
		if (Vector3.Dot(c0, normal) <= 0 && Vector3.Dot(c1, normal) <= 0 && Vector3.Dot(c2, normal) <= 0)
		{
			float distance = (point0 - sphereOrigin).Length();
			return new(normal * (sphereRadius - distance), point0);
		}

		float sphereRadiusSq = sphereRadius * sphereRadius;

		// Calculate closest intersection point for every edge.
		Vector3 point1 = ClosestPointOnLineSegment(triangleP1, triangleP2, sphereOrigin);
		Vector3 point2 = ClosestPointOnLineSegment(triangleP2, triangleP3, sphereOrigin);
		Vector3 point3 = ClosestPointOnLineSegment(triangleP3, triangleP1, sphereOrigin);

		// If no edges intersect, there is no collision.
		bool intersects1 = IntersectsEdge(sphereRadiusSq, point1, sphereOrigin);
		bool intersects2 = IntersectsEdge(sphereRadiusSq, point2, sphereOrigin);
		bool intersects3 = IntersectsEdge(sphereRadiusSq, point3, sphereOrigin);
		if (!intersects1 && !intersects2 && !intersects3)
			return null;

		// Find closest edge.
		Vector3 closestPoint = GetClosestPoint(sphereOrigin, point1, point2, point3);
		float distanceToBestPoint = (closestPoint - sphereOrigin).Length();
		return new(normal * (sphereRadius - distanceToBestPoint), closestPoint);

		static Vector3 GetNormal(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			return Vector3.Normalize(Vector3.Cross(p2 - p1, p3 - p1));
		}

		static Vector3 ClosestPointOnLineSegment(Vector3 lineA, Vector3 lineB, Vector3 spherePosition)
		{
			Vector3 ab = lineB - lineA;
			float t = Vector3.Dot(spherePosition - lineA, ab) / Vector3.Dot(ab, ab);
			return lineA + Math.Clamp(t, 0, 1) * ab;
		}

		static bool IntersectsEdge(float sphereRadiusSq, Vector3 closestPointOnLine, Vector3 spherePosition)
		{
			Vector3 diff = spherePosition - closestPointOnLine;
			float distSq = Vector3.Dot(diff, diff);
			return distSq < sphereRadiusSq;
		}

		static Vector3 GetClosestPoint(Vector3 spherePosition, Vector3 point1, Vector3 point2, Vector3 point3)
		{
			Vector3 d = spherePosition - point1;
			float bestDistSq = Vector3.Dot(d, d);
			Vector3 bestPoint = point1;

			d = spherePosition - point2;
			float distSq = Vector3.Dot(d, d);
			if (distSq < bestDistSq)
			{
				bestDistSq = distSq;
				bestPoint = point2;
			}

			d = spherePosition - point3;
			distSq = Vector3.Dot(d, d);
			if (distSq < bestDistSq)
				bestPoint = point3;

			return bestPoint;
		}
	}
}
