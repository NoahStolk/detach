using CollisionFormats.Extensions;
using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using Detach.Extensions;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CollisionFormats.Serialization;

public static class CollisionScenarioSerializer
{
	private static ReadOnlySpan<byte> Identifier => "CSID"u8; // Collision Scenario Input Data

	public static byte[] Serialize<TParam1, TParam2>(CollisionScenario<TParam1, TParam2> collisionScenarios)
		where TParam1 : struct
		where TParam2 : struct
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		bw.Write(Identifier);
		bw.Write(collisionScenarios.AlgorithmName);
		bw.Write((byte)2);
		bw.Write(collisionScenarios.Params1.Count);
		foreach (TParam1 param1 in collisionScenarios.Params1)
			Write(bw, param1);
		foreach (TParam2 param2 in collisionScenarios.Params2)
			Write(bw, param2);

		return ms.ToArray();
	}

	public static CollisionScenario<TParam1, TParam2> Deserialize<TParam1, TParam2>(byte[] data)
		where TParam1 : struct
		where TParam2 : struct
	{
		using MemoryStream ms = new(data);
		using BinaryReader br = new(ms);

		ReadOnlySpan<byte> identifier = br.ReadBytes(Identifier.Length);
		if (!identifier.SequenceEqual(Identifier))
			throw new FormatException("Invalid Collision Scenario Input Data identifier.");

		string algorithmName = br.ReadString();
		byte parameterCount = br.ReadByte();
		if (parameterCount != 2)
			throw new FormatException("Invalid parameter count.");

		int dataCount = br.ReadInt32();
		List<TParam1> params1 = new(dataCount);
		for (int i = 0; i < dataCount; i++)
			params1[i] = Read<TParam1>(br);
		List<TParam2> params2 = new(dataCount);
		for (int i = 0; i < dataCount; i++)
			params2[i] = Read<TParam2>(br);

		return new CollisionScenario<TParam1, TParam2>(algorithmName, params1, params2);
	}

	private static void Write<T>(BinaryWriter bw, T data)
		where T : struct
	{
		switch (typeof(T))
		{
			case { } t when t == typeof(Circle): bw.Write(Unsafe.As<T, Circle>(ref data)); break;
			case { } t when t == typeof(CircleCast): bw.Write(Unsafe.As<T, CircleCast>(ref data)); break;
			case { } t when t == typeof(LineSegment2D): bw.Write(Unsafe.As<T, LineSegment2D>(ref data)); break;
			case { } t when t == typeof(OrientedRectangle): bw.Write(Unsafe.As<T, OrientedRectangle>(ref data)); break;
			case { } t when t == typeof(Vector2): bw.Write(Unsafe.As<T, Vector2>(ref data)); break;
			case { } t when t == typeof(Rectangle): bw.Write(Unsafe.As<T, Rectangle>(ref data)); break;
			case { } t when t == typeof(Triangle2D): bw.Write(Unsafe.As<T, Triangle2D>(ref data)); break;
			case { } t when t == typeof(Aabb): bw.Write(Unsafe.As<T, Aabb>(ref data)); break;
			case { } t when t == typeof(ConeFrustum): bw.Write(Unsafe.As<T, ConeFrustum>(ref data)); break;
			case { } t when t == typeof(Cylinder): bw.Write(Unsafe.As<T, Cylinder>(ref data)); break;
			case { } t when t == typeof(LineSegment3D): bw.Write(Unsafe.As<T, LineSegment3D>(ref data)); break;
			case { } t when t == typeof(Obb): bw.Write(Unsafe.As<T, Obb>(ref data)); break;
			case { } t when t == typeof(OrientedPyramid): bw.Write(Unsafe.As<T, OrientedPyramid>(ref data)); break;
			case { } t when t == typeof(Vector3): bw.Write(Unsafe.As<T, Vector3>(ref data)); break;
			case { } t when t == typeof(Pyramid): bw.Write(Unsafe.As<T, Pyramid>(ref data)); break;
			case { } t when t == typeof(Ray): bw.Write(Unsafe.As<T, Ray>(ref data)); break;
			case { } t when t == typeof(Sphere): bw.Write(Unsafe.As<T, Sphere>(ref data)); break;
			case { } t when t == typeof(SphereCast): bw.Write(Unsafe.As<T, SphereCast>(ref data)); break;
			case { } t when t == typeof(Triangle3D): bw.Write(Unsafe.As<T, Triangle3D>(ref data)); break;
			default: throw new NotSupportedException($"Type {typeof(T)} is not supported.");
		}
	}

	private static T Read<T>(BinaryReader br)
		where T : struct
	{
		return typeof(T) switch
		{
			_ when typeof(T) == typeof(Circle) => Unsafe.BitCast<Circle, T>(br.ReadCircle()),
			_ when typeof(T) == typeof(CircleCast) => Unsafe.BitCast<CircleCast, T>(br.ReadCircleCast()),
			_ when typeof(T) == typeof(LineSegment2D) => Unsafe.BitCast<LineSegment2D, T>(br.ReadLineSegment2D()),
			_ when typeof(T) == typeof(OrientedRectangle) => Unsafe.BitCast<OrientedRectangle, T>(br.ReadOrientedRectangle()),
			_ when typeof(T) == typeof(Vector2) => Unsafe.BitCast<Vector2, T>(br.ReadVector2()),
			_ when typeof(T) == typeof(Rectangle) => Unsafe.BitCast<Rectangle, T>(br.ReadRectangle()),
			_ when typeof(T) == typeof(Triangle2D) => Unsafe.BitCast<Triangle2D, T>(br.ReadTriangle2D()),
			_ when typeof(T) == typeof(Aabb) => Unsafe.BitCast<Aabb, T>(br.ReadAabb()),
			_ when typeof(T) == typeof(ConeFrustum) => Unsafe.BitCast<ConeFrustum, T>(br.ReadConeFrustum()),
			_ when typeof(T) == typeof(Cylinder) => Unsafe.BitCast<Cylinder, T>(br.ReadCylinder()),
			_ when typeof(T) == typeof(LineSegment3D) => Unsafe.BitCast<LineSegment3D, T>(br.ReadLineSegment3D()),
			_ when typeof(T) == typeof(Obb) => Unsafe.BitCast<Obb, T>(br.ReadObb()),
			_ when typeof(T) == typeof(OrientedPyramid) => Unsafe.BitCast<OrientedPyramid, T>(br.ReadOrientedPyramid()),
			_ when typeof(T) == typeof(Vector3) => Unsafe.BitCast<Vector3, T>(br.ReadVector3()),
			_ when typeof(T) == typeof(Pyramid) => Unsafe.BitCast<Pyramid, T>(br.ReadPyramid()),
			_ when typeof(T) == typeof(Ray) => Unsafe.BitCast<Ray, T>(br.ReadRay()),
			_ when typeof(T) == typeof(Sphere) => Unsafe.BitCast<Sphere, T>(br.ReadSphere()),
			_ when typeof(T) == typeof(SphereCast) => Unsafe.BitCast<SphereCast, T>(br.ReadSphereCast()),
			_ when typeof(T) == typeof(Triangle3D) => Unsafe.BitCast<Triangle3D, T>(br.ReadTriangle3D()),
			_ => throw new NotSupportedException($"Type {typeof(T)} is not supported."),
		};
	}
}
