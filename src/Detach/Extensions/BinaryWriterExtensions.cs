using Detach.Numerics;
using System.Numerics;

namespace Detach.Extensions;

public static class BinaryWriterExtensions
{
	public static void WriteAsHalfPrecision(this BinaryWriter bw, Vector2 vector)
	{
		bw.Write((Half)vector.X);
		bw.Write((Half)vector.Y);
	}

	public static void WriteAsHalfPrecision(this BinaryWriter bw, Vector3 vector)
	{
		bw.Write((Half)vector.X);
		bw.Write((Half)vector.Y);
		bw.Write((Half)vector.Z);
	}

	public static void Write(this BinaryWriter bw, Vector2 vector)
	{
		bw.Write(vector.X);
		bw.Write(vector.Y);
	}

	public static void Write(this BinaryWriter bw, Vector3 vector)
	{
		bw.Write(vector.X);
		bw.Write(vector.Y);
		bw.Write(vector.Z);
	}

	public static void Write(this BinaryWriter bw, Vector4 vector)
	{
		bw.Write(vector.X);
		bw.Write(vector.Y);
		bw.Write(vector.Z);
		bw.Write(vector.W);
	}

	public static void Write(this BinaryWriter bw, Plane plane)
	{
		bw.Write(plane.Normal);
		bw.Write(plane.D);
	}

	public static void Write(this BinaryWriter bw, Quaternion quaternion)
	{
		bw.Write(quaternion.X);
		bw.Write(quaternion.Y);
		bw.Write(quaternion.Z);
		bw.Write(quaternion.W);
	}

	public static void Write(this BinaryWriter bw, Matrix4x4 matrix)
	{
		bw.Write(matrix.M11);
		bw.Write(matrix.M12);
		bw.Write(matrix.M13);
		bw.Write(matrix.M14);
		bw.Write(matrix.M21);
		bw.Write(matrix.M22);
		bw.Write(matrix.M23);
		bw.Write(matrix.M24);
		bw.Write(matrix.M31);
		bw.Write(matrix.M32);
		bw.Write(matrix.M33);
		bw.Write(matrix.M34);
		bw.Write(matrix.M41);
		bw.Write(matrix.M42);
		bw.Write(matrix.M43);
		bw.Write(matrix.M44);
	}

	public static void Write(this BinaryWriter bw, Color color)
	{
		bw.Write(color.R);
		bw.Write(color.G);
		bw.Write(color.B);
		bw.Write(color.A);
	}

	public static void WriteLengthPrefixedList<T>(this BinaryWriter bw, List<T> list, Action<BinaryWriter, T> writer)
	{
		bw.Write(list.Count);

		for (int i = 0; i < list.Count; i++)
			writer(bw, list[i]);
	}
}
