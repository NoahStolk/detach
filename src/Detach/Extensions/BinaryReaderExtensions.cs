using Detach.Numerics;
using System.Numerics;

namespace Detach.Extensions;

public static class BinaryReaderExtensions
{
	public static Vector2 ReadVector2AsHalfPrecision(this BinaryReader br)
	{
		return new Vector2((float)br.ReadHalf(), (float)br.ReadHalf());
	}

	public static Vector3 ReadVector3AsHalfPrecision(this BinaryReader br)
	{
		return new Vector3((float)br.ReadHalf(), (float)br.ReadHalf(), (float)br.ReadHalf());
	}

	public static Vector4 ReadVector4AsHalfPrecision(this BinaryReader br)
	{
		return new Vector4((float)br.ReadHalf(), (float)br.ReadHalf(), (float)br.ReadHalf(), (float)br.ReadHalf());
	}

	public static Vector2 ReadVector2(this BinaryReader br)
	{
		return new Vector2(br.ReadSingle(), br.ReadSingle());
	}

	public static Vector3 ReadVector3(this BinaryReader br)
	{
		return new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
	}

	public static Vector4 ReadVector4(this BinaryReader br)
	{
		return new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
	}

	public static Plane ReadPlane(this BinaryReader br)
	{
		return new Plane(br.ReadVector3(), br.ReadSingle());
	}

	public static Quaternion ReadQuaternion(this BinaryReader br)
	{
		return new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
	}

	// ReSharper disable once InconsistentNaming
	public static Matrix4x4 ReadMatrix4x4(this BinaryReader br)
	{
		return new Matrix4x4(
			br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
			br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
			br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
			br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
	}

	public static Color ReadColor(this BinaryReader br)
	{
		return new Color(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
	}

	public static List<T> ReadLengthPrefixedList<T>(this BinaryReader br, Func<BinaryReader, T> reader)
	{
		int length = br.ReadInt32();

		List<T> list = [];
		for (int i = 0; i < length; i++)
			list.Add(reader(br));

		return list;
	}
}
