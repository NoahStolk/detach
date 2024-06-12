using Detach.Numerics;

namespace Detach.Extensions;

public static partial class BinaryReaderExtensions
{
	public static IntVector2<sbyte> ReadIntVector2OfInt8(this BinaryReader binaryReader)
	{
		return new IntVector2<sbyte>(binaryReader.ReadSByte(), binaryReader.ReadSByte());
	}

	public static IntVector2<byte> ReadIntVector2OfUInt8(this BinaryReader binaryReader)
	{
		return new IntVector2<byte>(binaryReader.ReadByte(), binaryReader.ReadByte());
	}

	public static IntVector2<short> ReadIntVector2OfInt16(this BinaryReader binaryReader)
	{
		return new IntVector2<short>(binaryReader.ReadInt16(), binaryReader.ReadInt16());
	}

	public static IntVector2<ushort> ReadIntVector2OfUInt16(this BinaryReader binaryReader)
	{
		return new IntVector2<ushort>(binaryReader.ReadUInt16(), binaryReader.ReadUInt16());
	}

	public static IntVector2<int> ReadIntVector2OfInt32(this BinaryReader binaryReader)
	{
		return new IntVector2<int>(binaryReader.ReadInt32(), binaryReader.ReadInt32());
	}

	public static IntVector2<uint> ReadIntVector2OfUInt32(this BinaryReader binaryReader)
	{
		return new IntVector2<uint>(binaryReader.ReadUInt32(), binaryReader.ReadUInt32());
	}

	public static IntVector2<long> ReadIntVector2OfInt64(this BinaryReader binaryReader)
	{
		return new IntVector2<long>(binaryReader.ReadInt64(), binaryReader.ReadInt64());
	}

	public static IntVector2<ulong> ReadIntVector2OfUInt64(this BinaryReader binaryReader)
	{
		return new IntVector2<ulong>(binaryReader.ReadUInt64(), binaryReader.ReadUInt64());
	}

	public static IntVector3<sbyte> ReadIntVector3OfInt8(this BinaryReader binaryReader)
	{
		return new IntVector3<sbyte>(binaryReader.ReadSByte(), binaryReader.ReadSByte(), binaryReader.ReadSByte());
	}

	public static IntVector3<byte> ReadIntVector3OfUInt8(this BinaryReader binaryReader)
	{
		return new IntVector3<byte>(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte());
	}

	public static IntVector3<short> ReadIntVector3OfInt16(this BinaryReader binaryReader)
	{
		return new IntVector3<short>(binaryReader.ReadInt16(), binaryReader.ReadInt16(), binaryReader.ReadInt16());
	}

	public static IntVector3<ushort> ReadIntVector3OfUInt16(this BinaryReader binaryReader)
	{
		return new IntVector3<ushort>(binaryReader.ReadUInt16(), binaryReader.ReadUInt16(), binaryReader.ReadUInt16());
	}

	public static IntVector3<int> ReadIntVector3OfInt32(this BinaryReader binaryReader)
	{
		return new IntVector3<int>(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());
	}

	public static IntVector3<uint> ReadIntVector3OfUInt32(this BinaryReader binaryReader)
	{
		return new IntVector3<uint>(binaryReader.ReadUInt32(), binaryReader.ReadUInt32(), binaryReader.ReadUInt32());
	}

	public static IntVector3<long> ReadIntVector3OfInt64(this BinaryReader binaryReader)
	{
		return new IntVector3<long>(binaryReader.ReadInt64(), binaryReader.ReadInt64(), binaryReader.ReadInt64());
	}

	public static IntVector3<ulong> ReadIntVector3OfUInt64(this BinaryReader binaryReader)
	{
		return new IntVector3<ulong>(binaryReader.ReadUInt64(), binaryReader.ReadUInt64(), binaryReader.ReadUInt64());
	}

	public static IntVector4<sbyte> ReadIntVector4OfInt8(this BinaryReader binaryReader)
	{
		return new IntVector4<sbyte>(binaryReader.ReadSByte(), binaryReader.ReadSByte(), binaryReader.ReadSByte(), binaryReader.ReadSByte());
	}

	public static IntVector4<byte> ReadIntVector4OfUInt8(this BinaryReader binaryReader)
	{
		return new IntVector4<byte>(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte());
	}

	public static IntVector4<short> ReadIntVector4OfInt16(this BinaryReader binaryReader)
	{
		return new IntVector4<short>(binaryReader.ReadInt16(), binaryReader.ReadInt16(), binaryReader.ReadInt16(), binaryReader.ReadInt16());
	}

	public static IntVector4<ushort> ReadIntVector4OfUInt16(this BinaryReader binaryReader)
	{
		return new IntVector4<ushort>(binaryReader.ReadUInt16(), binaryReader.ReadUInt16(), binaryReader.ReadUInt16(), binaryReader.ReadUInt16());
	}

	public static IntVector4<int> ReadIntVector4OfInt32(this BinaryReader binaryReader)
	{
		return new IntVector4<int>(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());
	}

	public static IntVector4<uint> ReadIntVector4OfUInt32(this BinaryReader binaryReader)
	{
		return new IntVector4<uint>(binaryReader.ReadUInt32(), binaryReader.ReadUInt32(), binaryReader.ReadUInt32(), binaryReader.ReadUInt32());
	}

	public static IntVector4<long> ReadIntVector4OfInt64(this BinaryReader binaryReader)
	{
		return new IntVector4<long>(binaryReader.ReadInt64(), binaryReader.ReadInt64(), binaryReader.ReadInt64(), binaryReader.ReadInt64());
	}

	public static IntVector4<ulong> ReadIntVector4OfUInt64(this BinaryReader binaryReader)
	{
		return new IntVector4<ulong>(binaryReader.ReadUInt64(), binaryReader.ReadUInt64(), binaryReader.ReadUInt64(), binaryReader.ReadUInt64());
	}

}
