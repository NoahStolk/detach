using Detach.Collisions.Primitives3D;
using Detach.Utils;
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
		Vector2 toAxisXz = new(sphereCenter.X - bottom.X, sphereCenter.Z - bottom.Z);

		// Step 3: Compute distance from axis in XZ plane
		float distXzSq = toAxisXz.LengthSquared();

		Vector3 closestPoint;
		Vector3 normal;
		float penetration;

		bool isInsideVertical = sphereCenter.Y >= bottom.Y && sphereCenter.Y <= top.Y;
		bool isInsideRadial = distXzSq <= cylinder.Radius * cylinder.Radius;

		if (isInsideVertical && isInsideRadial)
		{
			// Sphere center is *inside* the cylinder
			// Choose nearest "exit point" — top, bottom, or side
			float toTop = top.Y - sphereCenter.Y;
			float toBottom = sphereCenter.Y - bottom.Y;
			float toSide = cylinder.Radius - MathF.Sqrt(distXzSq);

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
				Vector2 radialDir = Vector2.Normalize(toAxisXz);
				Vector2 sidePoint = new Vector2(bottom.X, bottom.Z) + radialDir * cylinder.Radius;
				closestPoint = new Vector3(sidePoint.X, clampedY, sidePoint.Y);
				Vector3 outward = new(radialDir.X, 0, radialDir.Y);
				normal = outward;
				penetration = cylinder.Radius - MathF.Sqrt(distXzSq) + sphere.Radius;
			}

			result = new IntersectionResult(normal, closestPoint, penetration);
			return true;
		}

		// Sphere is outside: compute the closest point on cylinder
		float distXz = MathF.Sqrt(distXzSq);

		Vector2 closestXz;
		if (distXz > cylinder.Radius)
		{
			Vector2 radialDir = toAxisXz / distXz;
			closestXz = new Vector2(bottom.X, bottom.Z) + radialDir * cylinder.Radius;
		}
		else
		{
			closestXz = new Vector2(sphereCenter.X, sphereCenter.Z);
		}

		closestPoint = new Vector3(closestXz.X, clampedY, closestXz.Y);
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

	public static bool SphereConeFrustum(Sphere sphere, ConeFrustum coneFrustum, out IntersectionResult result)
	{
		Vector3 sphereCenter = sphere.Center;
		Vector3 bottom = coneFrustum.BottomCenter;
		Vector3 top = bottom + Vector3.UnitY * coneFrustum.Height;

		float clampedY = Math.Clamp(sphereCenter.Y, bottom.Y, top.Y);
		float t = (clampedY - bottom.Y) / coneFrustum.Height;
		float radiusAtY = MathUtils.Lerp(coneFrustum.BottomRadius, coneFrustum.TopRadius, t);

		Vector2 toAxisXz = new(sphereCenter.X - bottom.X, sphereCenter.Z - bottom.Z);
		float distXzSq = toAxisXz.LengthSquared();
		float distXz = MathF.Sqrt(distXzSq);

		Vector3 closestPoint;
		Vector3 normal;
		float penetration;

		bool isInsideVertical = sphereCenter.Y >= bottom.Y && sphereCenter.Y <= top.Y;
		bool isInsideRadial = distXz <= radiusAtY;

		if (isInsideVertical && isInsideRadial)
		{
			// Inside the frustum
			float toBottom = sphereCenter.Y - bottom.Y;
			float toTop = top.Y - sphereCenter.Y;
			float toSide = radiusAtY - distXz;

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
				Vector2 radialDir = distXz > 0.0001f ? toAxisXz / distXz : Vector2.UnitX;
				Vector2 sidePointXz = new Vector2(bottom.X, bottom.Z) + radialDir * radiusAtY;
				closestPoint = new Vector3(sidePointXz.X, clampedY, sidePointXz.Y);
				normal = new Vector3(radialDir.X, 0, radialDir.Y);
				penetration = radiusAtY - distXz + sphere.Radius;
			}

			result = new IntersectionResult(normal, closestPoint, penetration);
			return true;
		}

		// Sphere is outside: compute the closest point on frustum
		Vector2 closestXz;

		if (distXz > radiusAtY)
		{
			Vector2 radialDir = toAxisXz / distXz;
			closestXz = new Vector2(bottom.X, bottom.Z) + radialDir * radiusAtY;
		}
		else
		{
			closestXz = new Vector2(sphereCenter.X, sphereCenter.Z);
		}

		closestPoint = new Vector3(closestXz.X, clampedY, closestXz.Y);
		Vector3 toCenter = sphereCenter - closestPoint;
		float distToSurface = toCenter.Length();

		if (distToSurface > sphere.Radius)
		{
			result = default;
			return false;
		}

		normal = distToSurface > 0.0001f ? Vector3.Normalize(toCenter) : Vector3.UnitY;
		penetration = sphere.Radius - distToSurface;

		result = new IntersectionResult(normal, closestPoint, penetration);
		return true;
	}
}
