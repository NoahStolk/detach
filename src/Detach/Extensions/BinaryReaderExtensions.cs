using Detach.Numerics;
using System.Numerics;

namespace Detach.Extensions;

public static class BinaryReaderExtensions
{
	public static Vector2 ReadVector2AsHalfPrecision(this BinaryReader br)
	{
		return new((float)br.ReadHalf(), (float)br.ReadHalf());
	}

	public static Vector3 ReadVector3AsHalfPrecision(this BinaryReader br)
	{
		return new((float)br.ReadHalf(), (float)br.ReadHalf(), (float)br.ReadHalf());
	}

	public static Vector4 ReadVector4AsHalfPrecision(this BinaryReader br)
	{
		return new((float)br.ReadHalf(), (float)br.ReadHalf(), (float)br.ReadHalf(), (float)br.ReadHalf());
	}

	public static Vector2 ReadVector2(this BinaryReader br)
	{
		return new(br.ReadSingle(), br.ReadSingle());
	}

	public static Vector3 ReadVector3(this BinaryReader br)
	{
		return new(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
	}

	public static Vector4 ReadVector4(this BinaryReader br)
	{
		return new(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
	}

	public static Plane ReadPlane(this BinaryReader br)
	{
		return new(br.ReadVector3(), br.ReadSingle());
	}

	public static Quaternion ReadQuaternion(this BinaryReader br)
	{
		return new(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
	}

	public static Matrix4x4 ReadMatrix4x4(this BinaryReader br)
	{
		return new(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
	}

	public static Color ReadColor(this BinaryReader br)
	{
		return new(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
	}

	public static List<T> ReadLengthPrefixedList<T>(this BinaryReader br, Func<BinaryReader, T> reader)
	{
		int length = br.ReadInt32();

		List<T> list = new();
		for (int i = 0; i < length; i++)
			list.Add(reader(br));

		return list;
	}
}
