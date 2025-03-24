using Detach.Parsers.Texture;
using Detach.Parsers.Texture.TgaFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Unit.Tests.Parsers.Texture.TgaFormat;

[TestClass]
public class TgaWriterTests
{
	[DataTestMethod]
	[DataRow("Checkerboard_24Bit.tga")]
	[DataRow("Checkerboard_24Bit_Rle.tga")]
	[DataRow("Checkerboard_32Bit.tga")]
	[DataRow("Checkerboard_32Bit_Rle.tga")]
	[DataRow("Font.tga")]
	[DataRow("Placeholder.tga")]
	[DataRow("RleTest.tga")]
	[DataRow("Sample_NoCompression.tga")]
	[DataRow("Sample_RleCompression.tga")]
	[DataRow("White4x4.tga")]
	public void TestWriteComparison(string fileName)
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath(fileName));
		TextureData originalTexture = TgaParser.Parse(bytes);

		byte[] writtenBytes = TgaWriter.Write(originalTexture);
		TextureData writtenTexture = TgaParser.Parse(writtenBytes);

		Assert.AreEqual(originalTexture.Width, writtenTexture.Width);
		Assert.AreEqual(originalTexture.Height, writtenTexture.Height);
		Assert.AreEqual(originalTexture.ColorData.Length, writtenTexture.ColorData.Length);
		Assert.IsTrue(originalTexture.ColorData.SequenceEqual(writtenTexture.ColorData));
	}
}
