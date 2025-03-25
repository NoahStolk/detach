// <auto-generated>
// This code was auto-generated.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>

#nullable enable

using Detach.Buffers;
using Detach.Numerics;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Detach.Extensions;

public static partial class BinaryReaderExtensions
{
	public static Buffer4<sbyte> ReadBuffer4OfInt8(this BinaryReader binaryReader)
	{
		Span<sbyte> buffer = stackalloc sbyte[Buffer4<sbyte>.Size];
		for (int i = 0; i < Buffer4<sbyte>.Size; i++)
			buffer[i] = binaryReader.ReadSByte();
		return new Buffer4<sbyte>(buffer);
	}

	public static Buffer4<byte> ReadBuffer4OfUInt8(this BinaryReader binaryReader)
	{
		Span<byte> buffer = stackalloc byte[Buffer4<byte>.Size];
		for (int i = 0; i < Buffer4<byte>.Size; i++)
			buffer[i] = binaryReader.ReadByte();
		return new Buffer4<byte>(buffer);
	}

	public static Buffer4<short> ReadBuffer4OfInt16(this BinaryReader binaryReader)
	{
		Span<short> buffer = stackalloc short[Buffer4<short>.Size];
		for (int i = 0; i < Buffer4<short>.Size; i++)
			buffer[i] = binaryReader.ReadInt16();
		return new Buffer4<short>(buffer);
	}

	public static Buffer4<ushort> ReadBuffer4OfUInt16(this BinaryReader binaryReader)
	{
		Span<ushort> buffer = stackalloc ushort[Buffer4<ushort>.Size];
		for (int i = 0; i < Buffer4<ushort>.Size; i++)
			buffer[i] = binaryReader.ReadUInt16();
		return new Buffer4<ushort>(buffer);
	}

	public static Buffer4<int> ReadBuffer4OfInt32(this BinaryReader binaryReader)
	{
		Span<int> buffer = stackalloc int[Buffer4<int>.Size];
		for (int i = 0; i < Buffer4<int>.Size; i++)
			buffer[i] = binaryReader.ReadInt32();
		return new Buffer4<int>(buffer);
	}

	public static Buffer4<uint> ReadBuffer4OfUInt32(this BinaryReader binaryReader)
	{
		Span<uint> buffer = stackalloc uint[Buffer4<uint>.Size];
		for (int i = 0; i < Buffer4<uint>.Size; i++)
			buffer[i] = binaryReader.ReadUInt32();
		return new Buffer4<uint>(buffer);
	}

	public static Buffer4<long> ReadBuffer4OfInt64(this BinaryReader binaryReader)
	{
		Span<long> buffer = stackalloc long[Buffer4<long>.Size];
		for (int i = 0; i < Buffer4<long>.Size; i++)
			buffer[i] = binaryReader.ReadInt64();
		return new Buffer4<long>(buffer);
	}

	public static Buffer4<ulong> ReadBuffer4OfUInt64(this BinaryReader binaryReader)
	{
		Span<ulong> buffer = stackalloc ulong[Buffer4<ulong>.Size];
		for (int i = 0; i < Buffer4<ulong>.Size; i++)
			buffer[i] = binaryReader.ReadUInt64();
		return new Buffer4<ulong>(buffer);
	}

	public static Buffer4<Half> ReadBuffer4OfFloat16(this BinaryReader binaryReader)
	{
		Span<Half> buffer = stackalloc Half[Buffer4<Half>.Size];
		for (int i = 0; i < Buffer4<Half>.Size; i++)
			buffer[i] = binaryReader.ReadHalf();
		return new Buffer4<Half>(buffer);
	}

	public static Buffer4<float> ReadBuffer4OfFloat32(this BinaryReader binaryReader)
	{
		Span<float> buffer = stackalloc float[Buffer4<float>.Size];
		for (int i = 0; i < Buffer4<float>.Size; i++)
			buffer[i] = binaryReader.ReadSingle();
		return new Buffer4<float>(buffer);
	}

	public static Buffer4<double> ReadBuffer4OfFloat64(this BinaryReader binaryReader)
	{
		Span<double> buffer = stackalloc double[Buffer4<double>.Size];
		for (int i = 0; i < Buffer4<double>.Size; i++)
			buffer[i] = binaryReader.ReadDouble();
		return new Buffer4<double>(buffer);
	}

	public static Buffer6<sbyte> ReadBuffer6OfInt8(this BinaryReader binaryReader)
	{
		Span<sbyte> buffer = stackalloc sbyte[Buffer6<sbyte>.Size];
		for (int i = 0; i < Buffer6<sbyte>.Size; i++)
			buffer[i] = binaryReader.ReadSByte();
		return new Buffer6<sbyte>(buffer);
	}

	public static Buffer6<byte> ReadBuffer6OfUInt8(this BinaryReader binaryReader)
	{
		Span<byte> buffer = stackalloc byte[Buffer6<byte>.Size];
		for (int i = 0; i < Buffer6<byte>.Size; i++)
			buffer[i] = binaryReader.ReadByte();
		return new Buffer6<byte>(buffer);
	}

	public static Buffer6<short> ReadBuffer6OfInt16(this BinaryReader binaryReader)
	{
		Span<short> buffer = stackalloc short[Buffer6<short>.Size];
		for (int i = 0; i < Buffer6<short>.Size; i++)
			buffer[i] = binaryReader.ReadInt16();
		return new Buffer6<short>(buffer);
	}

	public static Buffer6<ushort> ReadBuffer6OfUInt16(this BinaryReader binaryReader)
	{
		Span<ushort> buffer = stackalloc ushort[Buffer6<ushort>.Size];
		for (int i = 0; i < Buffer6<ushort>.Size; i++)
			buffer[i] = binaryReader.ReadUInt16();
		return new Buffer6<ushort>(buffer);
	}

	public static Buffer6<int> ReadBuffer6OfInt32(this BinaryReader binaryReader)
	{
		Span<int> buffer = stackalloc int[Buffer6<int>.Size];
		for (int i = 0; i < Buffer6<int>.Size; i++)
			buffer[i] = binaryReader.ReadInt32();
		return new Buffer6<int>(buffer);
	}

	public static Buffer6<uint> ReadBuffer6OfUInt32(this BinaryReader binaryReader)
	{
		Span<uint> buffer = stackalloc uint[Buffer6<uint>.Size];
		for (int i = 0; i < Buffer6<uint>.Size; i++)
			buffer[i] = binaryReader.ReadUInt32();
		return new Buffer6<uint>(buffer);
	}

	public static Buffer6<long> ReadBuffer6OfInt64(this BinaryReader binaryReader)
	{
		Span<long> buffer = stackalloc long[Buffer6<long>.Size];
		for (int i = 0; i < Buffer6<long>.Size; i++)
			buffer[i] = binaryReader.ReadInt64();
		return new Buffer6<long>(buffer);
	}

	public static Buffer6<ulong> ReadBuffer6OfUInt64(this BinaryReader binaryReader)
	{
		Span<ulong> buffer = stackalloc ulong[Buffer6<ulong>.Size];
		for (int i = 0; i < Buffer6<ulong>.Size; i++)
			buffer[i] = binaryReader.ReadUInt64();
		return new Buffer6<ulong>(buffer);
	}

	public static Buffer6<Half> ReadBuffer6OfFloat16(this BinaryReader binaryReader)
	{
		Span<Half> buffer = stackalloc Half[Buffer6<Half>.Size];
		for (int i = 0; i < Buffer6<Half>.Size; i++)
			buffer[i] = binaryReader.ReadHalf();
		return new Buffer6<Half>(buffer);
	}

	public static Buffer6<float> ReadBuffer6OfFloat32(this BinaryReader binaryReader)
	{
		Span<float> buffer = stackalloc float[Buffer6<float>.Size];
		for (int i = 0; i < Buffer6<float>.Size; i++)
			buffer[i] = binaryReader.ReadSingle();
		return new Buffer6<float>(buffer);
	}

	public static Buffer6<double> ReadBuffer6OfFloat64(this BinaryReader binaryReader)
	{
		Span<double> buffer = stackalloc double[Buffer6<double>.Size];
		for (int i = 0; i < Buffer6<double>.Size; i++)
			buffer[i] = binaryReader.ReadDouble();
		return new Buffer6<double>(buffer);
	}

	public static Buffer8<sbyte> ReadBuffer8OfInt8(this BinaryReader binaryReader)
	{
		Span<sbyte> buffer = stackalloc sbyte[Buffer8<sbyte>.Size];
		for (int i = 0; i < Buffer8<sbyte>.Size; i++)
			buffer[i] = binaryReader.ReadSByte();
		return new Buffer8<sbyte>(buffer);
	}

	public static Buffer8<byte> ReadBuffer8OfUInt8(this BinaryReader binaryReader)
	{
		Span<byte> buffer = stackalloc byte[Buffer8<byte>.Size];
		for (int i = 0; i < Buffer8<byte>.Size; i++)
			buffer[i] = binaryReader.ReadByte();
		return new Buffer8<byte>(buffer);
	}

	public static Buffer8<short> ReadBuffer8OfInt16(this BinaryReader binaryReader)
	{
		Span<short> buffer = stackalloc short[Buffer8<short>.Size];
		for (int i = 0; i < Buffer8<short>.Size; i++)
			buffer[i] = binaryReader.ReadInt16();
		return new Buffer8<short>(buffer);
	}

	public static Buffer8<ushort> ReadBuffer8OfUInt16(this BinaryReader binaryReader)
	{
		Span<ushort> buffer = stackalloc ushort[Buffer8<ushort>.Size];
		for (int i = 0; i < Buffer8<ushort>.Size; i++)
			buffer[i] = binaryReader.ReadUInt16();
		return new Buffer8<ushort>(buffer);
	}

	public static Buffer8<int> ReadBuffer8OfInt32(this BinaryReader binaryReader)
	{
		Span<int> buffer = stackalloc int[Buffer8<int>.Size];
		for (int i = 0; i < Buffer8<int>.Size; i++)
			buffer[i] = binaryReader.ReadInt32();
		return new Buffer8<int>(buffer);
	}

	public static Buffer8<uint> ReadBuffer8OfUInt32(this BinaryReader binaryReader)
	{
		Span<uint> buffer = stackalloc uint[Buffer8<uint>.Size];
		for (int i = 0; i < Buffer8<uint>.Size; i++)
			buffer[i] = binaryReader.ReadUInt32();
		return new Buffer8<uint>(buffer);
	}

	public static Buffer8<long> ReadBuffer8OfInt64(this BinaryReader binaryReader)
	{
		Span<long> buffer = stackalloc long[Buffer8<long>.Size];
		for (int i = 0; i < Buffer8<long>.Size; i++)
			buffer[i] = binaryReader.ReadInt64();
		return new Buffer8<long>(buffer);
	}

	public static Buffer8<ulong> ReadBuffer8OfUInt64(this BinaryReader binaryReader)
	{
		Span<ulong> buffer = stackalloc ulong[Buffer8<ulong>.Size];
		for (int i = 0; i < Buffer8<ulong>.Size; i++)
			buffer[i] = binaryReader.ReadUInt64();
		return new Buffer8<ulong>(buffer);
	}

	public static Buffer8<Half> ReadBuffer8OfFloat16(this BinaryReader binaryReader)
	{
		Span<Half> buffer = stackalloc Half[Buffer8<Half>.Size];
		for (int i = 0; i < Buffer8<Half>.Size; i++)
			buffer[i] = binaryReader.ReadHalf();
		return new Buffer8<Half>(buffer);
	}

	public static Buffer8<float> ReadBuffer8OfFloat32(this BinaryReader binaryReader)
	{
		Span<float> buffer = stackalloc float[Buffer8<float>.Size];
		for (int i = 0; i < Buffer8<float>.Size; i++)
			buffer[i] = binaryReader.ReadSingle();
		return new Buffer8<float>(buffer);
	}

	public static Buffer8<double> ReadBuffer8OfFloat64(this BinaryReader binaryReader)
	{
		Span<double> buffer = stackalloc double[Buffer8<double>.Size];
		for (int i = 0; i < Buffer8<double>.Size; i++)
			buffer[i] = binaryReader.ReadDouble();
		return new Buffer8<double>(buffer);
	}

	public static Buffer12<sbyte> ReadBuffer12OfInt8(this BinaryReader binaryReader)
	{
		Span<sbyte> buffer = stackalloc sbyte[Buffer12<sbyte>.Size];
		for (int i = 0; i < Buffer12<sbyte>.Size; i++)
			buffer[i] = binaryReader.ReadSByte();
		return new Buffer12<sbyte>(buffer);
	}

	public static Buffer12<byte> ReadBuffer12OfUInt8(this BinaryReader binaryReader)
	{
		Span<byte> buffer = stackalloc byte[Buffer12<byte>.Size];
		for (int i = 0; i < Buffer12<byte>.Size; i++)
			buffer[i] = binaryReader.ReadByte();
		return new Buffer12<byte>(buffer);
	}

	public static Buffer12<short> ReadBuffer12OfInt16(this BinaryReader binaryReader)
	{
		Span<short> buffer = stackalloc short[Buffer12<short>.Size];
		for (int i = 0; i < Buffer12<short>.Size; i++)
			buffer[i] = binaryReader.ReadInt16();
		return new Buffer12<short>(buffer);
	}

	public static Buffer12<ushort> ReadBuffer12OfUInt16(this BinaryReader binaryReader)
	{
		Span<ushort> buffer = stackalloc ushort[Buffer12<ushort>.Size];
		for (int i = 0; i < Buffer12<ushort>.Size; i++)
			buffer[i] = binaryReader.ReadUInt16();
		return new Buffer12<ushort>(buffer);
	}

	public static Buffer12<int> ReadBuffer12OfInt32(this BinaryReader binaryReader)
	{
		Span<int> buffer = stackalloc int[Buffer12<int>.Size];
		for (int i = 0; i < Buffer12<int>.Size; i++)
			buffer[i] = binaryReader.ReadInt32();
		return new Buffer12<int>(buffer);
	}

	public static Buffer12<uint> ReadBuffer12OfUInt32(this BinaryReader binaryReader)
	{
		Span<uint> buffer = stackalloc uint[Buffer12<uint>.Size];
		for (int i = 0; i < Buffer12<uint>.Size; i++)
			buffer[i] = binaryReader.ReadUInt32();
		return new Buffer12<uint>(buffer);
	}

	public static Buffer12<long> ReadBuffer12OfInt64(this BinaryReader binaryReader)
	{
		Span<long> buffer = stackalloc long[Buffer12<long>.Size];
		for (int i = 0; i < Buffer12<long>.Size; i++)
			buffer[i] = binaryReader.ReadInt64();
		return new Buffer12<long>(buffer);
	}

	public static Buffer12<ulong> ReadBuffer12OfUInt64(this BinaryReader binaryReader)
	{
		Span<ulong> buffer = stackalloc ulong[Buffer12<ulong>.Size];
		for (int i = 0; i < Buffer12<ulong>.Size; i++)
			buffer[i] = binaryReader.ReadUInt64();
		return new Buffer12<ulong>(buffer);
	}

	public static Buffer12<Half> ReadBuffer12OfFloat16(this BinaryReader binaryReader)
	{
		Span<Half> buffer = stackalloc Half[Buffer12<Half>.Size];
		for (int i = 0; i < Buffer12<Half>.Size; i++)
			buffer[i] = binaryReader.ReadHalf();
		return new Buffer12<Half>(buffer);
	}

	public static Buffer12<float> ReadBuffer12OfFloat32(this BinaryReader binaryReader)
	{
		Span<float> buffer = stackalloc float[Buffer12<float>.Size];
		for (int i = 0; i < Buffer12<float>.Size; i++)
			buffer[i] = binaryReader.ReadSingle();
		return new Buffer12<float>(buffer);
	}

	public static Buffer12<double> ReadBuffer12OfFloat64(this BinaryReader binaryReader)
	{
		Span<double> buffer = stackalloc double[Buffer12<double>.Size];
		for (int i = 0; i < Buffer12<double>.Size; i++)
			buffer[i] = binaryReader.ReadDouble();
		return new Buffer12<double>(buffer);
	}

	public static Buffer16<sbyte> ReadBuffer16OfInt8(this BinaryReader binaryReader)
	{
		Span<sbyte> buffer = stackalloc sbyte[Buffer16<sbyte>.Size];
		for (int i = 0; i < Buffer16<sbyte>.Size; i++)
			buffer[i] = binaryReader.ReadSByte();
		return new Buffer16<sbyte>(buffer);
	}

	public static Buffer16<byte> ReadBuffer16OfUInt8(this BinaryReader binaryReader)
	{
		Span<byte> buffer = stackalloc byte[Buffer16<byte>.Size];
		for (int i = 0; i < Buffer16<byte>.Size; i++)
			buffer[i] = binaryReader.ReadByte();
		return new Buffer16<byte>(buffer);
	}

	public static Buffer16<short> ReadBuffer16OfInt16(this BinaryReader binaryReader)
	{
		Span<short> buffer = stackalloc short[Buffer16<short>.Size];
		for (int i = 0; i < Buffer16<short>.Size; i++)
			buffer[i] = binaryReader.ReadInt16();
		return new Buffer16<short>(buffer);
	}

	public static Buffer16<ushort> ReadBuffer16OfUInt16(this BinaryReader binaryReader)
	{
		Span<ushort> buffer = stackalloc ushort[Buffer16<ushort>.Size];
		for (int i = 0; i < Buffer16<ushort>.Size; i++)
			buffer[i] = binaryReader.ReadUInt16();
		return new Buffer16<ushort>(buffer);
	}

	public static Buffer16<int> ReadBuffer16OfInt32(this BinaryReader binaryReader)
	{
		Span<int> buffer = stackalloc int[Buffer16<int>.Size];
		for (int i = 0; i < Buffer16<int>.Size; i++)
			buffer[i] = binaryReader.ReadInt32();
		return new Buffer16<int>(buffer);
	}

	public static Buffer16<uint> ReadBuffer16OfUInt32(this BinaryReader binaryReader)
	{
		Span<uint> buffer = stackalloc uint[Buffer16<uint>.Size];
		for (int i = 0; i < Buffer16<uint>.Size; i++)
			buffer[i] = binaryReader.ReadUInt32();
		return new Buffer16<uint>(buffer);
	}

	public static Buffer16<long> ReadBuffer16OfInt64(this BinaryReader binaryReader)
	{
		Span<long> buffer = stackalloc long[Buffer16<long>.Size];
		for (int i = 0; i < Buffer16<long>.Size; i++)
			buffer[i] = binaryReader.ReadInt64();
		return new Buffer16<long>(buffer);
	}

	public static Buffer16<ulong> ReadBuffer16OfUInt64(this BinaryReader binaryReader)
	{
		Span<ulong> buffer = stackalloc ulong[Buffer16<ulong>.Size];
		for (int i = 0; i < Buffer16<ulong>.Size; i++)
			buffer[i] = binaryReader.ReadUInt64();
		return new Buffer16<ulong>(buffer);
	}

	public static Buffer16<Half> ReadBuffer16OfFloat16(this BinaryReader binaryReader)
	{
		Span<Half> buffer = stackalloc Half[Buffer16<Half>.Size];
		for (int i = 0; i < Buffer16<Half>.Size; i++)
			buffer[i] = binaryReader.ReadHalf();
		return new Buffer16<Half>(buffer);
	}

	public static Buffer16<float> ReadBuffer16OfFloat32(this BinaryReader binaryReader)
	{
		Span<float> buffer = stackalloc float[Buffer16<float>.Size];
		for (int i = 0; i < Buffer16<float>.Size; i++)
			buffer[i] = binaryReader.ReadSingle();
		return new Buffer16<float>(buffer);
	}

	public static Buffer16<double> ReadBuffer16OfFloat64(this BinaryReader binaryReader)
	{
		Span<double> buffer = stackalloc double[Buffer16<double>.Size];
		for (int i = 0; i < Buffer16<double>.Size; i++)
			buffer[i] = binaryReader.ReadDouble();
		return new Buffer16<double>(buffer);
	}

	public static Buffer24<sbyte> ReadBuffer24OfInt8(this BinaryReader binaryReader)
	{
		Span<sbyte> buffer = stackalloc sbyte[Buffer24<sbyte>.Size];
		for (int i = 0; i < Buffer24<sbyte>.Size; i++)
			buffer[i] = binaryReader.ReadSByte();
		return new Buffer24<sbyte>(buffer);
	}

	public static Buffer24<byte> ReadBuffer24OfUInt8(this BinaryReader binaryReader)
	{
		Span<byte> buffer = stackalloc byte[Buffer24<byte>.Size];
		for (int i = 0; i < Buffer24<byte>.Size; i++)
			buffer[i] = binaryReader.ReadByte();
		return new Buffer24<byte>(buffer);
	}

	public static Buffer24<short> ReadBuffer24OfInt16(this BinaryReader binaryReader)
	{
		Span<short> buffer = stackalloc short[Buffer24<short>.Size];
		for (int i = 0; i < Buffer24<short>.Size; i++)
			buffer[i] = binaryReader.ReadInt16();
		return new Buffer24<short>(buffer);
	}

	public static Buffer24<ushort> ReadBuffer24OfUInt16(this BinaryReader binaryReader)
	{
		Span<ushort> buffer = stackalloc ushort[Buffer24<ushort>.Size];
		for (int i = 0; i < Buffer24<ushort>.Size; i++)
			buffer[i] = binaryReader.ReadUInt16();
		return new Buffer24<ushort>(buffer);
	}

	public static Buffer24<int> ReadBuffer24OfInt32(this BinaryReader binaryReader)
	{
		Span<int> buffer = stackalloc int[Buffer24<int>.Size];
		for (int i = 0; i < Buffer24<int>.Size; i++)
			buffer[i] = binaryReader.ReadInt32();
		return new Buffer24<int>(buffer);
	}

	public static Buffer24<uint> ReadBuffer24OfUInt32(this BinaryReader binaryReader)
	{
		Span<uint> buffer = stackalloc uint[Buffer24<uint>.Size];
		for (int i = 0; i < Buffer24<uint>.Size; i++)
			buffer[i] = binaryReader.ReadUInt32();
		return new Buffer24<uint>(buffer);
	}

	public static Buffer24<long> ReadBuffer24OfInt64(this BinaryReader binaryReader)
	{
		Span<long> buffer = stackalloc long[Buffer24<long>.Size];
		for (int i = 0; i < Buffer24<long>.Size; i++)
			buffer[i] = binaryReader.ReadInt64();
		return new Buffer24<long>(buffer);
	}

	public static Buffer24<ulong> ReadBuffer24OfUInt64(this BinaryReader binaryReader)
	{
		Span<ulong> buffer = stackalloc ulong[Buffer24<ulong>.Size];
		for (int i = 0; i < Buffer24<ulong>.Size; i++)
			buffer[i] = binaryReader.ReadUInt64();
		return new Buffer24<ulong>(buffer);
	}

	public static Buffer24<Half> ReadBuffer24OfFloat16(this BinaryReader binaryReader)
	{
		Span<Half> buffer = stackalloc Half[Buffer24<Half>.Size];
		for (int i = 0; i < Buffer24<Half>.Size; i++)
			buffer[i] = binaryReader.ReadHalf();
		return new Buffer24<Half>(buffer);
	}

	public static Buffer24<float> ReadBuffer24OfFloat32(this BinaryReader binaryReader)
	{
		Span<float> buffer = stackalloc float[Buffer24<float>.Size];
		for (int i = 0; i < Buffer24<float>.Size; i++)
			buffer[i] = binaryReader.ReadSingle();
		return new Buffer24<float>(buffer);
	}

	public static Buffer24<double> ReadBuffer24OfFloat64(this BinaryReader binaryReader)
	{
		Span<double> buffer = stackalloc double[Buffer24<double>.Size];
		for (int i = 0; i < Buffer24<double>.Size; i++)
			buffer[i] = binaryReader.ReadDouble();
		return new Buffer24<double>(buffer);
	}

	public static Buffer32<sbyte> ReadBuffer32OfInt8(this BinaryReader binaryReader)
	{
		Span<sbyte> buffer = stackalloc sbyte[Buffer32<sbyte>.Size];
		for (int i = 0; i < Buffer32<sbyte>.Size; i++)
			buffer[i] = binaryReader.ReadSByte();
		return new Buffer32<sbyte>(buffer);
	}

	public static Buffer32<byte> ReadBuffer32OfUInt8(this BinaryReader binaryReader)
	{
		Span<byte> buffer = stackalloc byte[Buffer32<byte>.Size];
		for (int i = 0; i < Buffer32<byte>.Size; i++)
			buffer[i] = binaryReader.ReadByte();
		return new Buffer32<byte>(buffer);
	}

	public static Buffer32<short> ReadBuffer32OfInt16(this BinaryReader binaryReader)
	{
		Span<short> buffer = stackalloc short[Buffer32<short>.Size];
		for (int i = 0; i < Buffer32<short>.Size; i++)
			buffer[i] = binaryReader.ReadInt16();
		return new Buffer32<short>(buffer);
	}

	public static Buffer32<ushort> ReadBuffer32OfUInt16(this BinaryReader binaryReader)
	{
		Span<ushort> buffer = stackalloc ushort[Buffer32<ushort>.Size];
		for (int i = 0; i < Buffer32<ushort>.Size; i++)
			buffer[i] = binaryReader.ReadUInt16();
		return new Buffer32<ushort>(buffer);
	}

	public static Buffer32<int> ReadBuffer32OfInt32(this BinaryReader binaryReader)
	{
		Span<int> buffer = stackalloc int[Buffer32<int>.Size];
		for (int i = 0; i < Buffer32<int>.Size; i++)
			buffer[i] = binaryReader.ReadInt32();
		return new Buffer32<int>(buffer);
	}

	public static Buffer32<uint> ReadBuffer32OfUInt32(this BinaryReader binaryReader)
	{
		Span<uint> buffer = stackalloc uint[Buffer32<uint>.Size];
		for (int i = 0; i < Buffer32<uint>.Size; i++)
			buffer[i] = binaryReader.ReadUInt32();
		return new Buffer32<uint>(buffer);
	}

	public static Buffer32<long> ReadBuffer32OfInt64(this BinaryReader binaryReader)
	{
		Span<long> buffer = stackalloc long[Buffer32<long>.Size];
		for (int i = 0; i < Buffer32<long>.Size; i++)
			buffer[i] = binaryReader.ReadInt64();
		return new Buffer32<long>(buffer);
	}

	public static Buffer32<ulong> ReadBuffer32OfUInt64(this BinaryReader binaryReader)
	{
		Span<ulong> buffer = stackalloc ulong[Buffer32<ulong>.Size];
		for (int i = 0; i < Buffer32<ulong>.Size; i++)
			buffer[i] = binaryReader.ReadUInt64();
		return new Buffer32<ulong>(buffer);
	}

	public static Buffer32<Half> ReadBuffer32OfFloat16(this BinaryReader binaryReader)
	{
		Span<Half> buffer = stackalloc Half[Buffer32<Half>.Size];
		for (int i = 0; i < Buffer32<Half>.Size; i++)
			buffer[i] = binaryReader.ReadHalf();
		return new Buffer32<Half>(buffer);
	}

	public static Buffer32<float> ReadBuffer32OfFloat32(this BinaryReader binaryReader)
	{
		Span<float> buffer = stackalloc float[Buffer32<float>.Size];
		for (int i = 0; i < Buffer32<float>.Size; i++)
			buffer[i] = binaryReader.ReadSingle();
		return new Buffer32<float>(buffer);
	}

	public static Buffer32<double> ReadBuffer32OfFloat64(this BinaryReader binaryReader)
	{
		Span<double> buffer = stackalloc double[Buffer32<double>.Size];
		for (int i = 0; i < Buffer32<double>.Size; i++)
			buffer[i] = binaryReader.ReadDouble();
		return new Buffer32<double>(buffer);
	}

}
