using Detach.Buffers;
using Detach.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests.Buffers;

[TestClass]
public sealed class BinaryBufferTests
{
	private static ReadOnlySpan<byte> Buffer =>
	[
		0x01, 0x02, 0x03, 0x04,
		0x05, 0x06, 0x07, 0x08,
		0x09, 0x0A, 0x0B, 0x0C,
		0x0D, 0x0E, 0x0F, 0x10,
	];

	[TestMethod]
	public void ReadBytes16()
	{
		using MemoryStream readStream = new(Buffer.ToArray());
		using BinaryReader binaryReader = new(readStream);
		Buffer16<byte> buffer = binaryReader.ReadBuffer16OfUInt8();

		using MemoryStream writeStream = new();
		using BinaryWriter binaryWriter = new(writeStream);
		binaryWriter.Write(buffer);

		Assert.IsTrue(Buffer.SequenceEqual(writeStream.ToArray()));
	}

	[TestMethod]
	public void ReadFloats4()
	{
		using MemoryStream readStream = new(Buffer.ToArray());
		using BinaryReader binaryReader = new(readStream);
		Buffer4<float> buffer = binaryReader.ReadBuffer4OfFloat32();

		using MemoryStream writeStream = new();
		using BinaryWriter binaryWriter = new(writeStream);
		binaryWriter.Write(buffer);

		Assert.IsTrue(Buffer.SequenceEqual(writeStream.ToArray()));
	}
}
