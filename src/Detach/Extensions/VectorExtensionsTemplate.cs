using Detach.Numerics;
using System.Numerics;

namespace Detach.Extensions;

public static partial class VectorExtensions
{
	public static IntVector2<sbyte> RoundToIntVector2OfInt8(this Vector2 vector)
	{
		return new IntVector2<sbyte>((sbyte)MathF.Round(vector.X), (sbyte)MathF.Round(vector.Y));
	}

	public static IntVector2<byte> RoundToIntVector2OfUInt8(this Vector2 vector)
	{
		return new IntVector2<byte>((byte)MathF.Round(vector.X), (byte)MathF.Round(vector.Y));
	}

	public static IntVector2<short> RoundToIntVector2OfInt16(this Vector2 vector)
	{
		return new IntVector2<short>((short)MathF.Round(vector.X), (short)MathF.Round(vector.Y));
	}

	public static IntVector2<ushort> RoundToIntVector2OfUInt16(this Vector2 vector)
	{
		return new IntVector2<ushort>((ushort)MathF.Round(vector.X), (ushort)MathF.Round(vector.Y));
	}

	public static IntVector2<int> RoundToIntVector2OfInt32(this Vector2 vector)
	{
		return new IntVector2<int>((int)MathF.Round(vector.X), (int)MathF.Round(vector.Y));
	}

	public static IntVector2<uint> RoundToIntVector2OfUInt32(this Vector2 vector)
	{
		return new IntVector2<uint>((uint)MathF.Round(vector.X), (uint)MathF.Round(vector.Y));
	}

	public static IntVector2<long> RoundToIntVector2OfInt64(this Vector2 vector)
	{
		return new IntVector2<long>((long)MathF.Round(vector.X), (long)MathF.Round(vector.Y));
	}

	public static IntVector2<ulong> RoundToIntVector2OfUInt64(this Vector2 vector)
	{
		return new IntVector2<ulong>((ulong)MathF.Round(vector.X), (ulong)MathF.Round(vector.Y));
	}

	public static IntVector2<sbyte> FloorToIntVector2OfInt8(this Vector2 vector)
	{
		return new IntVector2<sbyte>((sbyte)MathF.Floor(vector.X), (sbyte)MathF.Floor(vector.Y));
	}

	public static IntVector2<byte> FloorToIntVector2OfUInt8(this Vector2 vector)
	{
		return new IntVector2<byte>((byte)MathF.Floor(vector.X), (byte)MathF.Floor(vector.Y));
	}

	public static IntVector2<short> FloorToIntVector2OfInt16(this Vector2 vector)
	{
		return new IntVector2<short>((short)MathF.Floor(vector.X), (short)MathF.Floor(vector.Y));
	}

	public static IntVector2<ushort> FloorToIntVector2OfUInt16(this Vector2 vector)
	{
		return new IntVector2<ushort>((ushort)MathF.Floor(vector.X), (ushort)MathF.Floor(vector.Y));
	}

	public static IntVector2<int> FloorToIntVector2OfInt32(this Vector2 vector)
	{
		return new IntVector2<int>((int)MathF.Floor(vector.X), (int)MathF.Floor(vector.Y));
	}

	public static IntVector2<uint> FloorToIntVector2OfUInt32(this Vector2 vector)
	{
		return new IntVector2<uint>((uint)MathF.Floor(vector.X), (uint)MathF.Floor(vector.Y));
	}

	public static IntVector2<long> FloorToIntVector2OfInt64(this Vector2 vector)
	{
		return new IntVector2<long>((long)MathF.Floor(vector.X), (long)MathF.Floor(vector.Y));
	}

	public static IntVector2<ulong> FloorToIntVector2OfUInt64(this Vector2 vector)
	{
		return new IntVector2<ulong>((ulong)MathF.Floor(vector.X), (ulong)MathF.Floor(vector.Y));
	}

	public static IntVector2<sbyte> CeilingToIntVector2OfInt8(this Vector2 vector)
	{
		return new IntVector2<sbyte>((sbyte)MathF.Ceiling(vector.X), (sbyte)MathF.Ceiling(vector.Y));
	}

	public static IntVector2<byte> CeilingToIntVector2OfUInt8(this Vector2 vector)
	{
		return new IntVector2<byte>((byte)MathF.Ceiling(vector.X), (byte)MathF.Ceiling(vector.Y));
	}

	public static IntVector2<short> CeilingToIntVector2OfInt16(this Vector2 vector)
	{
		return new IntVector2<short>((short)MathF.Ceiling(vector.X), (short)MathF.Ceiling(vector.Y));
	}

	public static IntVector2<ushort> CeilingToIntVector2OfUInt16(this Vector2 vector)
	{
		return new IntVector2<ushort>((ushort)MathF.Ceiling(vector.X), (ushort)MathF.Ceiling(vector.Y));
	}

	public static IntVector2<int> CeilingToIntVector2OfInt32(this Vector2 vector)
	{
		return new IntVector2<int>((int)MathF.Ceiling(vector.X), (int)MathF.Ceiling(vector.Y));
	}

	public static IntVector2<uint> CeilingToIntVector2OfUInt32(this Vector2 vector)
	{
		return new IntVector2<uint>((uint)MathF.Ceiling(vector.X), (uint)MathF.Ceiling(vector.Y));
	}

	public static IntVector2<long> CeilingToIntVector2OfInt64(this Vector2 vector)
	{
		return new IntVector2<long>((long)MathF.Ceiling(vector.X), (long)MathF.Ceiling(vector.Y));
	}

	public static IntVector2<ulong> CeilingToIntVector2OfUInt64(this Vector2 vector)
	{
		return new IntVector2<ulong>((ulong)MathF.Ceiling(vector.X), (ulong)MathF.Ceiling(vector.Y));
	}

	public static IntVector3<sbyte> RoundToIntVector3OfInt8(this Vector3 vector)
	{
		return new IntVector3<sbyte>((sbyte)MathF.Round(vector.X), (sbyte)MathF.Round(vector.Y), (sbyte)MathF.Round(vector.Z));
	}

	public static IntVector3<byte> RoundToIntVector3OfUInt8(this Vector3 vector)
	{
		return new IntVector3<byte>((byte)MathF.Round(vector.X), (byte)MathF.Round(vector.Y), (byte)MathF.Round(vector.Z));
	}

	public static IntVector3<short> RoundToIntVector3OfInt16(this Vector3 vector)
	{
		return new IntVector3<short>((short)MathF.Round(vector.X), (short)MathF.Round(vector.Y), (short)MathF.Round(vector.Z));
	}

	public static IntVector3<ushort> RoundToIntVector3OfUInt16(this Vector3 vector)
	{
		return new IntVector3<ushort>((ushort)MathF.Round(vector.X), (ushort)MathF.Round(vector.Y), (ushort)MathF.Round(vector.Z));
	}

	public static IntVector3<int> RoundToIntVector3OfInt32(this Vector3 vector)
	{
		return new IntVector3<int>((int)MathF.Round(vector.X), (int)MathF.Round(vector.Y), (int)MathF.Round(vector.Z));
	}

	public static IntVector3<uint> RoundToIntVector3OfUInt32(this Vector3 vector)
	{
		return new IntVector3<uint>((uint)MathF.Round(vector.X), (uint)MathF.Round(vector.Y), (uint)MathF.Round(vector.Z));
	}

	public static IntVector3<long> RoundToIntVector3OfInt64(this Vector3 vector)
	{
		return new IntVector3<long>((long)MathF.Round(vector.X), (long)MathF.Round(vector.Y), (long)MathF.Round(vector.Z));
	}

	public static IntVector3<ulong> RoundToIntVector3OfUInt64(this Vector3 vector)
	{
		return new IntVector3<ulong>((ulong)MathF.Round(vector.X), (ulong)MathF.Round(vector.Y), (ulong)MathF.Round(vector.Z));
	}

	public static IntVector3<sbyte> FloorToIntVector3OfInt8(this Vector3 vector)
	{
		return new IntVector3<sbyte>((sbyte)MathF.Floor(vector.X), (sbyte)MathF.Floor(vector.Y), (sbyte)MathF.Floor(vector.Z));
	}

	public static IntVector3<byte> FloorToIntVector3OfUInt8(this Vector3 vector)
	{
		return new IntVector3<byte>((byte)MathF.Floor(vector.X), (byte)MathF.Floor(vector.Y), (byte)MathF.Floor(vector.Z));
	}

	public static IntVector3<short> FloorToIntVector3OfInt16(this Vector3 vector)
	{
		return new IntVector3<short>((short)MathF.Floor(vector.X), (short)MathF.Floor(vector.Y), (short)MathF.Floor(vector.Z));
	}

	public static IntVector3<ushort> FloorToIntVector3OfUInt16(this Vector3 vector)
	{
		return new IntVector3<ushort>((ushort)MathF.Floor(vector.X), (ushort)MathF.Floor(vector.Y), (ushort)MathF.Floor(vector.Z));
	}

	public static IntVector3<int> FloorToIntVector3OfInt32(this Vector3 vector)
	{
		return new IntVector3<int>((int)MathF.Floor(vector.X), (int)MathF.Floor(vector.Y), (int)MathF.Floor(vector.Z));
	}

	public static IntVector3<uint> FloorToIntVector3OfUInt32(this Vector3 vector)
	{
		return new IntVector3<uint>((uint)MathF.Floor(vector.X), (uint)MathF.Floor(vector.Y), (uint)MathF.Floor(vector.Z));
	}

	public static IntVector3<long> FloorToIntVector3OfInt64(this Vector3 vector)
	{
		return new IntVector3<long>((long)MathF.Floor(vector.X), (long)MathF.Floor(vector.Y), (long)MathF.Floor(vector.Z));
	}

	public static IntVector3<ulong> FloorToIntVector3OfUInt64(this Vector3 vector)
	{
		return new IntVector3<ulong>((ulong)MathF.Floor(vector.X), (ulong)MathF.Floor(vector.Y), (ulong)MathF.Floor(vector.Z));
	}

	public static IntVector3<sbyte> CeilingToIntVector3OfInt8(this Vector3 vector)
	{
		return new IntVector3<sbyte>((sbyte)MathF.Ceiling(vector.X), (sbyte)MathF.Ceiling(vector.Y), (sbyte)MathF.Ceiling(vector.Z));
	}

	public static IntVector3<byte> CeilingToIntVector3OfUInt8(this Vector3 vector)
	{
		return new IntVector3<byte>((byte)MathF.Ceiling(vector.X), (byte)MathF.Ceiling(vector.Y), (byte)MathF.Ceiling(vector.Z));
	}

	public static IntVector3<short> CeilingToIntVector3OfInt16(this Vector3 vector)
	{
		return new IntVector3<short>((short)MathF.Ceiling(vector.X), (short)MathF.Ceiling(vector.Y), (short)MathF.Ceiling(vector.Z));
	}

	public static IntVector3<ushort> CeilingToIntVector3OfUInt16(this Vector3 vector)
	{
		return new IntVector3<ushort>((ushort)MathF.Ceiling(vector.X), (ushort)MathF.Ceiling(vector.Y), (ushort)MathF.Ceiling(vector.Z));
	}

	public static IntVector3<int> CeilingToIntVector3OfInt32(this Vector3 vector)
	{
		return new IntVector3<int>((int)MathF.Ceiling(vector.X), (int)MathF.Ceiling(vector.Y), (int)MathF.Ceiling(vector.Z));
	}

	public static IntVector3<uint> CeilingToIntVector3OfUInt32(this Vector3 vector)
	{
		return new IntVector3<uint>((uint)MathF.Ceiling(vector.X), (uint)MathF.Ceiling(vector.Y), (uint)MathF.Ceiling(vector.Z));
	}

	public static IntVector3<long> CeilingToIntVector3OfInt64(this Vector3 vector)
	{
		return new IntVector3<long>((long)MathF.Ceiling(vector.X), (long)MathF.Ceiling(vector.Y), (long)MathF.Ceiling(vector.Z));
	}

	public static IntVector3<ulong> CeilingToIntVector3OfUInt64(this Vector3 vector)
	{
		return new IntVector3<ulong>((ulong)MathF.Ceiling(vector.X), (ulong)MathF.Ceiling(vector.Y), (ulong)MathF.Ceiling(vector.Z));
	}

	public static IntVector4<sbyte> RoundToIntVector4OfInt8(this Vector4 vector)
	{
		return new IntVector4<sbyte>((sbyte)MathF.Round(vector.X), (sbyte)MathF.Round(vector.Y), (sbyte)MathF.Round(vector.Z), (sbyte)MathF.Round(vector.W));
	}

	public static IntVector4<byte> RoundToIntVector4OfUInt8(this Vector4 vector)
	{
		return new IntVector4<byte>((byte)MathF.Round(vector.X), (byte)MathF.Round(vector.Y), (byte)MathF.Round(vector.Z), (byte)MathF.Round(vector.W));
	}

	public static IntVector4<short> RoundToIntVector4OfInt16(this Vector4 vector)
	{
		return new IntVector4<short>((short)MathF.Round(vector.X), (short)MathF.Round(vector.Y), (short)MathF.Round(vector.Z), (short)MathF.Round(vector.W));
	}

	public static IntVector4<ushort> RoundToIntVector4OfUInt16(this Vector4 vector)
	{
		return new IntVector4<ushort>((ushort)MathF.Round(vector.X), (ushort)MathF.Round(vector.Y), (ushort)MathF.Round(vector.Z), (ushort)MathF.Round(vector.W));
	}

	public static IntVector4<int> RoundToIntVector4OfInt32(this Vector4 vector)
	{
		return new IntVector4<int>((int)MathF.Round(vector.X), (int)MathF.Round(vector.Y), (int)MathF.Round(vector.Z), (int)MathF.Round(vector.W));
	}

	public static IntVector4<uint> RoundToIntVector4OfUInt32(this Vector4 vector)
	{
		return new IntVector4<uint>((uint)MathF.Round(vector.X), (uint)MathF.Round(vector.Y), (uint)MathF.Round(vector.Z), (uint)MathF.Round(vector.W));
	}

	public static IntVector4<long> RoundToIntVector4OfInt64(this Vector4 vector)
	{
		return new IntVector4<long>((long)MathF.Round(vector.X), (long)MathF.Round(vector.Y), (long)MathF.Round(vector.Z), (long)MathF.Round(vector.W));
	}

	public static IntVector4<ulong> RoundToIntVector4OfUInt64(this Vector4 vector)
	{
		return new IntVector4<ulong>((ulong)MathF.Round(vector.X), (ulong)MathF.Round(vector.Y), (ulong)MathF.Round(vector.Z), (ulong)MathF.Round(vector.W));
	}

	public static IntVector4<sbyte> FloorToIntVector4OfInt8(this Vector4 vector)
	{
		return new IntVector4<sbyte>((sbyte)MathF.Floor(vector.X), (sbyte)MathF.Floor(vector.Y), (sbyte)MathF.Floor(vector.Z), (sbyte)MathF.Floor(vector.W));
	}

	public static IntVector4<byte> FloorToIntVector4OfUInt8(this Vector4 vector)
	{
		return new IntVector4<byte>((byte)MathF.Floor(vector.X), (byte)MathF.Floor(vector.Y), (byte)MathF.Floor(vector.Z), (byte)MathF.Floor(vector.W));
	}

	public static IntVector4<short> FloorToIntVector4OfInt16(this Vector4 vector)
	{
		return new IntVector4<short>((short)MathF.Floor(vector.X), (short)MathF.Floor(vector.Y), (short)MathF.Floor(vector.Z), (short)MathF.Floor(vector.W));
	}

	public static IntVector4<ushort> FloorToIntVector4OfUInt16(this Vector4 vector)
	{
		return new IntVector4<ushort>((ushort)MathF.Floor(vector.X), (ushort)MathF.Floor(vector.Y), (ushort)MathF.Floor(vector.Z), (ushort)MathF.Floor(vector.W));
	}

	public static IntVector4<int> FloorToIntVector4OfInt32(this Vector4 vector)
	{
		return new IntVector4<int>((int)MathF.Floor(vector.X), (int)MathF.Floor(vector.Y), (int)MathF.Floor(vector.Z), (int)MathF.Floor(vector.W));
	}

	public static IntVector4<uint> FloorToIntVector4OfUInt32(this Vector4 vector)
	{
		return new IntVector4<uint>((uint)MathF.Floor(vector.X), (uint)MathF.Floor(vector.Y), (uint)MathF.Floor(vector.Z), (uint)MathF.Floor(vector.W));
	}

	public static IntVector4<long> FloorToIntVector4OfInt64(this Vector4 vector)
	{
		return new IntVector4<long>((long)MathF.Floor(vector.X), (long)MathF.Floor(vector.Y), (long)MathF.Floor(vector.Z), (long)MathF.Floor(vector.W));
	}

	public static IntVector4<ulong> FloorToIntVector4OfUInt64(this Vector4 vector)
	{
		return new IntVector4<ulong>((ulong)MathF.Floor(vector.X), (ulong)MathF.Floor(vector.Y), (ulong)MathF.Floor(vector.Z), (ulong)MathF.Floor(vector.W));
	}

	public static IntVector4<sbyte> CeilingToIntVector4OfInt8(this Vector4 vector)
	{
		return new IntVector4<sbyte>((sbyte)MathF.Ceiling(vector.X), (sbyte)MathF.Ceiling(vector.Y), (sbyte)MathF.Ceiling(vector.Z), (sbyte)MathF.Ceiling(vector.W));
	}

	public static IntVector4<byte> CeilingToIntVector4OfUInt8(this Vector4 vector)
	{
		return new IntVector4<byte>((byte)MathF.Ceiling(vector.X), (byte)MathF.Ceiling(vector.Y), (byte)MathF.Ceiling(vector.Z), (byte)MathF.Ceiling(vector.W));
	}

	public static IntVector4<short> CeilingToIntVector4OfInt16(this Vector4 vector)
	{
		return new IntVector4<short>((short)MathF.Ceiling(vector.X), (short)MathF.Ceiling(vector.Y), (short)MathF.Ceiling(vector.Z), (short)MathF.Ceiling(vector.W));
	}

	public static IntVector4<ushort> CeilingToIntVector4OfUInt16(this Vector4 vector)
	{
		return new IntVector4<ushort>((ushort)MathF.Ceiling(vector.X), (ushort)MathF.Ceiling(vector.Y), (ushort)MathF.Ceiling(vector.Z), (ushort)MathF.Ceiling(vector.W));
	}

	public static IntVector4<int> CeilingToIntVector4OfInt32(this Vector4 vector)
	{
		return new IntVector4<int>((int)MathF.Ceiling(vector.X), (int)MathF.Ceiling(vector.Y), (int)MathF.Ceiling(vector.Z), (int)MathF.Ceiling(vector.W));
	}

	public static IntVector4<uint> CeilingToIntVector4OfUInt32(this Vector4 vector)
	{
		return new IntVector4<uint>((uint)MathF.Ceiling(vector.X), (uint)MathF.Ceiling(vector.Y), (uint)MathF.Ceiling(vector.Z), (uint)MathF.Ceiling(vector.W));
	}

	public static IntVector4<long> CeilingToIntVector4OfInt64(this Vector4 vector)
	{
		return new IntVector4<long>((long)MathF.Ceiling(vector.X), (long)MathF.Ceiling(vector.Y), (long)MathF.Ceiling(vector.Z), (long)MathF.Ceiling(vector.W));
	}

	public static IntVector4<ulong> CeilingToIntVector4OfUInt64(this Vector4 vector)
	{
		return new IntVector4<ulong>((ulong)MathF.Ceiling(vector.X), (ulong)MathF.Ceiling(vector.Y), (ulong)MathF.Ceiling(vector.Z), (ulong)MathF.Ceiling(vector.W));
	}

	public static Vector2 ToVector2(this IntVector2<sbyte> vector)
	{
		return new Vector2(vector.X, vector.Y);
	}

	public static Vector2 ToVector2(this IntVector2<byte> vector)
	{
		return new Vector2(vector.X, vector.Y);
	}

	public static Vector2 ToVector2(this IntVector2<short> vector)
	{
		return new Vector2(vector.X, vector.Y);
	}

	public static Vector2 ToVector2(this IntVector2<ushort> vector)
	{
		return new Vector2(vector.X, vector.Y);
	}

	public static Vector2 ToVector2(this IntVector2<int> vector)
	{
		return new Vector2(vector.X, vector.Y);
	}

	public static Vector2 ToVector2(this IntVector2<uint> vector)
	{
		return new Vector2(vector.X, vector.Y);
	}

	public static Vector2 ToVector2(this IntVector2<long> vector)
	{
		return new Vector2(vector.X, vector.Y);
	}

	public static Vector2 ToVector2(this IntVector2<ulong> vector)
	{
		return new Vector2(vector.X, vector.Y);
	}

	public static Vector3 ToVector3(this IntVector3<sbyte> vector)
	{
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public static Vector3 ToVector3(this IntVector3<byte> vector)
	{
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public static Vector3 ToVector3(this IntVector3<short> vector)
	{
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public static Vector3 ToVector3(this IntVector3<ushort> vector)
	{
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public static Vector3 ToVector3(this IntVector3<int> vector)
	{
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public static Vector3 ToVector3(this IntVector3<uint> vector)
	{
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public static Vector3 ToVector3(this IntVector3<long> vector)
	{
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public static Vector3 ToVector3(this IntVector3<ulong> vector)
	{
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public static Vector4 ToVector4(this IntVector4<sbyte> vector)
	{
		return new Vector4(vector.X, vector.Y, vector.Z, vector.W);
	}

	public static Vector4 ToVector4(this IntVector4<byte> vector)
	{
		return new Vector4(vector.X, vector.Y, vector.Z, vector.W);
	}

	public static Vector4 ToVector4(this IntVector4<short> vector)
	{
		return new Vector4(vector.X, vector.Y, vector.Z, vector.W);
	}

	public static Vector4 ToVector4(this IntVector4<ushort> vector)
	{
		return new Vector4(vector.X, vector.Y, vector.Z, vector.W);
	}

	public static Vector4 ToVector4(this IntVector4<int> vector)
	{
		return new Vector4(vector.X, vector.Y, vector.Z, vector.W);
	}

	public static Vector4 ToVector4(this IntVector4<uint> vector)
	{
		return new Vector4(vector.X, vector.Y, vector.Z, vector.W);
	}

	public static Vector4 ToVector4(this IntVector4<long> vector)
	{
		return new Vector4(vector.X, vector.Y, vector.Z, vector.W);
	}

	public static Vector4 ToVector4(this IntVector4<ulong> vector)
	{
		return new Vector4(vector.X, vector.Y, vector.Z, vector.W);
	}

}
