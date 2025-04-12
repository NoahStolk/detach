using Detach.Parsers.Texture;
using Detach.Parsers.Texture.TgaFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Detach.Tests.Unit.Tests.Parsers.Texture.TgaFormat;

[TestClass]
public sealed class TgaParserTests
{
	[DataTestMethod]
	[DataRow("Font.tga", 1152, 12)]
	[DataRow("Placeholder.tga", 16, 16)]
	public void TestTgaParse(string fileName, int expectedWidth, int expectedHeight)
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath(fileName));
		TextureData texture = TgaParser.Parse(bytes);

		Assert.AreEqual(expectedWidth, texture.Width);
		Assert.AreEqual(expectedHeight, texture.Height);
	}

	[DataTestMethod]
	[DataRow("Sample_NoCompression.tga")]
	[DataRow("Sample_RleCompression.tga")]
	public void TestCompressionParse(string fileName)
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath(fileName));
		TextureData texture = TgaParser.Parse(bytes);

		Assert.AreEqual(72, texture.Width);
		Assert.AreEqual(16, texture.Height);
		Assert.AreEqual(72 * 16 * 4, texture.ColorData.Length);

		// Top-left pixel.
		Assert.AreEqual(0x00, texture.ColorData[0]);
		Assert.AreEqual(0x94, texture.ColorData[1]);
		Assert.AreEqual(0xFF, texture.ColorData[2]);
		Assert.AreEqual(0xFF, texture.ColorData[3]);

		// Pixel at 3x3.
		Assert.AreEqual(0x00, texture.ColorData[72 * 2 * 4 + 3 * 4 + 0]);
		Assert.AreEqual(0x00, texture.ColorData[72 * 2 * 4 + 3 * 4 + 1]);
		Assert.AreEqual(0x00, texture.ColorData[72 * 2 * 4 + 3 * 4 + 2]);
		Assert.AreEqual(0x00, texture.ColorData[72 * 2 * 4 + 3 * 4 + 3]);

		// Pixel at 4x7.
		Assert.AreEqual(0xFF, texture.ColorData[72 * 6 * 4 + 4 * 4 + 0]);
		Assert.AreEqual(0x6A, texture.ColorData[72 * 6 * 4 + 4 * 4 + 1]);
		Assert.AreEqual(0x00, texture.ColorData[72 * 6 * 4 + 4 * 4 + 2]);
		Assert.AreEqual(0xFF, texture.ColorData[72 * 6 * 4 + 4 * 4 + 3]);
	}

	[DataTestMethod]
	[DataRow("Large_NoCompression.tga")]
	[DataRow("Large_RleCompression.tga")]
	public void TestLarge(string fileName)
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath(fileName));

		long timestamp = Stopwatch.GetTimestamp();
		TextureData texture = TgaParser.Parse(bytes);
		Console.WriteLine(Stopwatch.GetElapsedTime(timestamp));

		Assert.AreEqual(800, texture.Width);
		Assert.AreEqual(600, texture.Height);
		Assert.AreEqual(800 * 600 * 4, texture.ColorData.Length);
	}

	[DataTestMethod]
	[DataRow("Checkerboard_24Bit.tga")]
	[DataRow("Checkerboard_24Bit_Rle.tga")]
	[DataRow("Checkerboard_32Bit.tga")]
	[DataRow("Checkerboard_32Bit_Rle.tga")]
	public void TestPixelDepthParse(string fileName)
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath(fileName));
		TextureData texture = TgaParser.Parse(bytes);

		Assert.AreEqual(3, texture.Width);
		Assert.AreEqual(2, texture.Height);
		Assert.AreEqual(3 * 2 * 4, texture.ColorData.Length);

		for (int i = 0; i < 6; i++)
		{
			bool isBlack = i % 2 == 0;
			Assert.AreEqual(isBlack ? 0x00 : 0xFF, texture.ColorData[i * 4 + 0]);
			Assert.AreEqual(isBlack ? 0x00 : 0xFF, texture.ColorData[i * 4 + 1]);
			Assert.AreEqual(isBlack ? 0x00 : 0xFF, texture.ColorData[i * 4 + 2]);
			Assert.AreEqual(0xFF, texture.ColorData[i * 4 + 3]);
		}
	}

	[TestMethod]
	public void TestSimpleRleParse()
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath("White4x4.tga"));
		TextureData texture = TgaParser.Parse(bytes);

		Assert.AreEqual(4, texture.Width);
		Assert.AreEqual(4, texture.Height);
		Assert.AreEqual(4 * 4 * 4, texture.ColorData.Length);

		foreach (byte component in texture.ColorData)
			Assert.AreEqual(0xFF, component);
	}

	[TestMethod]
	public void TestRleParse()
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath("RleTest.tga"));
		TextureData texture = TgaParser.Parse(bytes);

		Assert.AreEqual(4, texture.Width);
		Assert.AreEqual(4, texture.Height);
		Assert.AreEqual(4 * 4 * 4, texture.ColorData.Length);

		const bool r = true;
		const bool _ = false;
		bool[,] redBuffer =
		{
			{ _, _, _, _ },
			{ _, r, r, r },
			{ _, _, _, _ },
			{ r, r, r, _ },
		};
		for (int i = 0; i < texture.Width; i++)
		{
			for (int j = 0; j < texture.Height; j++)
			{
				int index = (i + j * texture.Width) * 4;
				bool isRed = redBuffer[j, i];
				Assert.AreEqual(0xFF, texture.ColorData[index + 0]);
				Assert.AreEqual(isRed ? 0x00 : 0xFF, texture.ColorData[index + 1]);
				Assert.AreEqual(isRed ? 0x00 : 0xFF, texture.ColorData[index + 2]);
				Assert.AreEqual(0xFF, texture.ColorData[index + 3]);
			}
		}
	}
}
