using Detach.Buffers;
using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry3D
{
	public static float PlaneEquation(Vector3 point, Plane plane)
	{
		return Vector3.Dot(point, plane.Normal) - plane.D;
	}

	public static Vector3 SatCrossEdge(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		Vector3 ab = a - b;
		Vector3 cd = c - d;
		Vector3 result = Vector3.Cross(ab, cd);
		if (result.LengthSquared() > float.Epsilon)
			return result;

		Vector3 axis = Vector3.Cross(ab, c - a);
		result = Vector3.Cross(ab, axis);
		if (result.LengthSquared() > float.Epsilon)
			return result;

		return Vector3.Zero;
	}

	public static Vector3 Barycentric(Vector3 point, Triangle3D triangle)
	{
		Vector3 ap = point - triangle.A;
		Vector3 bp = point - triangle.B;
		Vector3 cp = point - triangle.C;

		Vector3 ab = triangle.B - triangle.A;
		Vector3 ac = triangle.C - triangle.A;
		Vector3 bc = triangle.C - triangle.B;
		Vector3 cb = triangle.B - triangle.C;
		Vector3 ca = triangle.A - triangle.C;

		Vector3 v = ab - Project(ab, cb);
		float a = 1f - Vector3.Dot(v, ap) / Vector3.Dot(v, ab);

		v = bc - Project(bc, ac);
		float b = 1f - Vector3.Dot(v, bp) / Vector3.Dot(v, bc);

		v = ca - Project(ca, ab);
		float c = 1f - Vector3.Dot(v, cp) / Vector3.Dot(v, ca);

		return new Vector3(a, b, c);
	}

	public static Vector2 Project(Vector2 length, Vector2 direction)
	{
		float dot = Vector2.Dot(length, direction);
		float lengthSquared = direction.LengthSquared();
		return direction * (dot / lengthSquared);
	}

	public static Vector3 Project(Vector3 length, Vector3 direction)
	{
		float dot = Vector3.Dot(length, direction);
		float lengthSquared = direction.LengthSquared();
		return direction * (dot / lengthSquared);
	}

	public static Vector2 Perpendicular(Vector2 length, Vector2 direction)
	{
		return length - Project(length, direction);
	}

	public static Vector3 Perpendicular(Vector3 length, Vector3 direction)
	{
		return length - Project(length, direction);
	}

	public static bool FindCollisionFeatures(Sphere sphere1, Sphere sphere2, out CollisionManifold collisionManifold)
	{
		collisionManifold = CollisionManifold.Empty;

		float r = sphere1.Radius + sphere2.Radius;
		Vector3 d = sphere2.Center - sphere1.Center;
		if (d.LengthSquared() > r * r)
			return false;

		Vector3 direction = Vector3.Normalize(d); // TODO: Prevent NaN direction when spheres have the same position.
		collisionManifold.Normal = direction;
		collisionManifold.Depth = MathF.Abs(d.Length() - r) * 0.5f;
		float dtp = sphere1.Radius - collisionManifold.Depth;
		Vector3 contact = sphere1.Center + direction * dtp;
		collisionManifold.ContactCount = 1;
		collisionManifold.Contacts[0] = contact;
		return true;
	}

	public static bool FindCollisionFeatures(Obb obb, Sphere sphere, out CollisionManifold collisionManifold)
	{
		collisionManifold = CollisionManifold.Empty;

		Vector3 closestPoint = ClosestPointInObb(sphere.Center, obb);
		float distanceSquared = (closestPoint - sphere.Center).LengthSquared();
		if (distanceSquared > sphere.Radius * sphere.Radius)
			return false;

		Vector3 normal;
		if (distanceSquared is > -float.Epsilon and < float.Epsilon)
		{
			float mSq = (closestPoint - obb.Center).LengthSquared();
			if (mSq is > -float.Epsilon and < float.Epsilon)
				return false;

			normal = Vector3.Normalize(closestPoint - obb.Center);
		}
		else
		{
			normal = Vector3.Normalize(sphere.Center - closestPoint);
		}

		Vector3 outsidePoint = sphere.Center - normal * sphere.Radius;
		float distance = (closestPoint - outsidePoint).Length();
		collisionManifold.Normal = normal;
		collisionManifold.Depth = distance * 0.5f;
		collisionManifold.ContactCount = 1;
		collisionManifold.Contacts[0] = closestPoint + (outsidePoint - closestPoint) * 0.5f;
		return true;
	}

	public static bool FindCollisionFeatures(Obb obb1, Obb obb2, out CollisionManifold collisionManifold)
	{
		collisionManifold = CollisionManifold.Empty;

		Span<Vector3> test = stackalloc Vector3[15];
		test[0] = new Vector3(obb1.Orientation.M11, obb1.Orientation.M12, obb1.Orientation.M13);
		test[1] = new Vector3(obb1.Orientation.M21, obb1.Orientation.M22, obb1.Orientation.M23);
		test[2] = new Vector3(obb1.Orientation.M31, obb1.Orientation.M32, obb1.Orientation.M33);
		test[3] = new Vector3(obb2.Orientation.M11, obb2.Orientation.M12, obb2.Orientation.M13);
		test[4] = new Vector3(obb2.Orientation.M21, obb2.Orientation.M22, obb2.Orientation.M23);
		test[5] = new Vector3(obb2.Orientation.M31, obb2.Orientation.M32, obb2.Orientation.M33);
		for (int i = 0; i < 3; i++)
		{
			test[6 + i * 3 + 0] = Vector3.Cross(test[i], test[0]);
			test[6 + i * 3 + 1] = Vector3.Cross(test[i], test[1]);
			test[6 + i * 3 + 2] = Vector3.Cross(test[i], test[2]);
		}

		Vector3? hitNormal = null;
		for (int i = 0; i < test.Length; i++)
		{
			if (test[i].LengthSquared() < float.Epsilon)
				continue;

			float depth = PenetrationDepthObb(obb1, obb2, test[i], out bool shouldFlip);
			if (depth <= 0)
				return false;

			if (depth < collisionManifold.Depth)
			{
				if (shouldFlip)
					test[i] *= -1;

				collisionManifold.Depth = depth;
				hitNormal = test[i];
			}
		}

		if (hitNormal is null)
			return false;

		Vector3 axis = Vector3.Normalize(hitNormal.Value);
		int count1 = ClipEdgesToObb(obb2.GetEdges(), obb1, out Buffer12<Vector3> c1);
		int count2 = ClipEdgesToObb(obb1.GetEdges(), obb2, out Buffer12<Vector3> c2);
		collisionManifold.ContactCount = count1 + count2;
		for (int i = 0; i < count1; i++)
			collisionManifold.Contacts[i] = c1[i];
		for (int i = 0; i < count2; i++)
			collisionManifold.Contacts[count1 + i] = c2[i];

		Interval interval = obb1.GetInterval(axis);
		float distance = (interval.Max - interval.Min) * 0.5f - collisionManifold.Depth * 0.5f;
		Vector3 pointOnPlane = obb1.Center + axis * distance;
		for (int i = collisionManifold.ContactCount - 1; i >= 0; i--)
		{
			Vector3 contact = collisionManifold.Contacts[i];
			collisionManifold.Contacts[i] = contact + axis * Vector3.Dot(axis, pointOnPlane - contact);
		}

		// TODO: Check if this is necessary.
		// TODO: If it is, optimize it.
		// Remove duplicate contact points.
		// Buffer24<Vector3> finalContactPoints = default;
		// int finalContactCount = 0;
		// for (int i = 0; i < collisionManifold.ContactCount; i++)
		// {
		// 	Vector3 contact = collisionManifold.Contacts[i];
		// 	bool duplicate = false;
		// 	for (int j = 0; j < finalContactCount; j++)
		// 	{
		// 		if (Vector3.DistanceSquared(contact, finalContactPoints[j]) < float.Epsilon)
		// 		{
		// 			duplicate = true;
		// 			break;
		// 		}
		// 	}
		//
		// 	if (!duplicate)
		// 		finalContactPoints[finalContactCount++] = contact;
		// }
		//
		// collisionManifold.Contacts = finalContactPoints;
		// collisionManifold.ContactCount = finalContactCount;
		collisionManifold.Normal = axis;
		return true;
	}
}
