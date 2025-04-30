using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static bool SphereObb(Sphere sphere, Obb obb, out IntersectionResult result)
	{
		Matrix4x4 obbOrientation = obb.Orientation.ToMatrix4x4();

		Matrix4x4 inverseOrientation = Matrix4x4.Transpose(obbOrientation);
		Vector3 localCenter = Vector3.Transform(sphere.Center - obb.Center, inverseOrientation);

		bool isInside =
			Math.Abs(localCenter.X) <= obb.HalfExtents.X &&
			Math.Abs(localCenter.Y) <= obb.HalfExtents.Y &&
			Math.Abs(localCenter.Z) <= obb.HalfExtents.Z;

		Vector3 clamped = Vector3.Clamp(localCenter, -obb.HalfExtents, obb.HalfExtents);
		Vector3 closestPointWorld = Vector3.Transform(clamped, obbOrientation) + obb.Center;

		if (!isInside)
		{
			Vector3 toCenter = sphere.Center - closestPointWorld;
			float distance = toCenter.Length();

			if (distance > sphere.Radius)
			{
				result = default;
				return false;
			}

			Vector3 delta = localCenter - clamped;
			float absX = Math.Abs(delta.X);
			float absY = Math.Abs(delta.Y);
			float absZ = Math.Abs(delta.Z);

			Vector3 localNormal;
			if (absX > absY && absX > absZ)
				localNormal = new Vector3(delta.X < 0 ? -1 : 1, 0, 0);
			else if (absY > absZ)
				localNormal = new Vector3(0, delta.Y < 0 ? -1 : 1, 0);
			else
				localNormal = new Vector3(0, 0, delta.Z < 0 ? -1 : 1);

			Vector3 worldNormal = Vector3.TransformNormal(localNormal, obbOrientation);
			worldNormal = Vector3.Normalize(worldNormal);

			float penetration = sphere.Radius - distance;

			result = new IntersectionResult(worldNormal, closestPointWorld, penetration);
		}
		else
		{
			// Inside case: find nearest face and project to exit point
			float dx = obb.HalfExtents.X - Math.Abs(localCenter.X);
			float dy = obb.HalfExtents.Y - Math.Abs(localCenter.Y);
			float dz = obb.HalfExtents.Z - Math.Abs(localCenter.Z);

			Vector3 localNormal;
			Vector3 localPoint = localCenter;
			float minDistance;

			if (dx < dy && dx < dz)
			{
				localNormal = new Vector3(localCenter.X < 0 ? -1 : 1, 0, 0);
				localPoint.X = localNormal.X * obb.HalfExtents.X;
				minDistance = dx;
			}
			else if (dy < dz)
			{
				localNormal = new Vector3(0, localCenter.Y < 0 ? -1 : 1, 0);
				localPoint.Y = localNormal.Y * obb.HalfExtents.Y;
				minDistance = dy;
			}
			else
			{
				localNormal = new Vector3(0, 0, localCenter.Z < 0 ? -1 : 1);
				localPoint.Z = localNormal.Z * obb.HalfExtents.Z;
				minDistance = dz;
			}

			Vector3 worldNormal = Vector3.TransformNormal(localNormal, obbOrientation);
			worldNormal = Vector3.Normalize(worldNormal);

			Vector3 worldPoint = Vector3.Transform(localPoint, obbOrientation) + obb.Center;
			float penetration = minDistance + sphere.Radius;

			result = new IntersectionResult(worldNormal, worldPoint, penetration);
		}

		return true;
	}

	public static bool SphereCylinder(Sphere sphere, Cylinder cylinder, out IntersectionResult result)
	{
		Vector3 sphereCenter = sphere.Center;
		Vector3 bottom = cylinder.BottomCenter;
		Vector3 top = bottom + Vector3.UnitY * cylinder.Height;

		// Step 1: Clamp sphere center's Y to cylinder height (for closest point on side wall or caps)
		float clampedY = Math.Clamp(sphereCenter.Y, bottom.Y, top.Y);

		// Step 2: Project sphere center onto cylinder axis to get the height
		Vector3 axisPoint = new Vector3(sphereCenter.X, clampedY, sphereCenter.Z);
		Vector2 toAxisXZ = new Vector2(sphereCenter.X - bottom.X, sphereCenter.Z - bottom.Z);

		// Step 3: Compute distance from axis in XZ plane
		float distXZSq = toAxisXZ.LengthSquared();

		Vector3 closestPoint;
		Vector3 normal;
		float penetration;

		bool isInsideVertical = sphereCenter.Y >= bottom.Y && sphereCenter.Y <= top.Y;
		bool isInsideRadial = distXZSq <= cylinder.Radius * cylinder.Radius;

		if (isInsideVertical && isInsideRadial)
		{
			// Sphere center is *inside* the cylinder
			// Choose nearest "exit point" — top, bottom, or side
			float toTop = top.Y - sphereCenter.Y;
			float toBottom = sphereCenter.Y - bottom.Y;
			float toSide = cylinder.Radius - MathF.Sqrt(distXZSq);

			if (toTop < toBottom && toTop < toSide)
			{
				// Closest to top cap
				normal = Vector3.UnitY;
				closestPoint = new Vector3(sphereCenter.X, top.Y, sphereCenter.Z);
				penetration = toTop + sphere.Radius;
			}
			else if (toBottom < toSide)
			{
				// Closest to bottom cap
				normal = -Vector3.UnitY;
				closestPoint = new Vector3(sphereCenter.X, bottom.Y, sphereCenter.Z);
				penetration = toBottom + sphere.Radius;
			}
			else
			{
				// Closest to side
				Vector2 radialDir = Vector2.Normalize(toAxisXZ);
				Vector2 sidePoint = new Vector2(bottom.X, bottom.Z) + radialDir * cylinder.Radius;
				closestPoint = new Vector3(sidePoint.X, clampedY, sidePoint.Y);
				Vector3 outward = new Vector3(radialDir.X, 0, radialDir.Y);
				normal = outward;
				penetration = (cylinder.Radius - MathF.Sqrt(distXZSq)) + sphere.Radius;
			}

			result = new IntersectionResult(normal, closestPoint, penetration);
			return true;
		}

		// Sphere is outside: compute the closest point on cylinder
		Vector2 dirXZ = toAxisXZ;
		float distXZ = MathF.Sqrt(distXZSq);

		Vector2 closestXZ;
		if (distXZ > cylinder.Radius)
		{
			Vector2 radialDir = dirXZ / distXZ;
			closestXZ = new Vector2(bottom.X, bottom.Z) + radialDir * cylinder.Radius;
		}
		else
		{
			closestXZ = new Vector2(sphereCenter.X, sphereCenter.Z);
		}

		closestPoint = new Vector3(closestXZ.X, clampedY, closestXZ.Y);
		Vector3 toCenter = sphereCenter - closestPoint;
		float distToSurface = toCenter.Length();

		if (distToSurface > sphere.Radius)
		{
			result = default;
			return false; // No intersection
		}

		normal = distToSurface > 0.0001f ? Vector3.Normalize(toCenter) : Vector3.UnitY;
		penetration = sphere.Radius - distToSurface;

		result = new IntersectionResult(normal, closestPoint, penetration);
		return true;
	}
}
