using Detach.Parsers.Sound;
using Detach.Parsers.Sound.WavFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Unit.Tests.Parsers.Sound.WavFormat;

// TODO: Test JUNK header.
[TestClass]
public class WaveParserTests
{
	[DataTestMethod]
	[DataRow("Sample1.wav", (short)1, 11000, 11000, (short)1, (short)8, 2208, 2208, 0.2)]
	[DataRow("Sample2.wav", (short)1, 11025, 22050, (short)2, (short)16, 61544, 30772, 2.791)]
	public void TestWaveParse(string fileName, short expectedChannels, int expectedSampleRate, int expectedByteRate, short expectedBlockAlign, short expectedBitsPerSample, int expectedDataSize, int expectedSampleCount, double expectedLengthInSeconds)
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath(fileName));
		SoundData sound = WaveParser.Parse(bytes);

		Assert.AreEqual(expectedChannels, sound.Channels);
		Assert.AreEqual(expectedSampleRate, sound.SampleRate);
		Assert.AreEqual(expectedByteRate, sound.ByteRate);
		Assert.AreEqual(expectedBlockAlign, sound.BlockAlign);
		Assert.AreEqual(expectedBitsPerSample, sound.BitsPerSample);
		Assert.AreEqual(expectedDataSize, sound.Data.Length);

		Assert.AreEqual(expectedSampleCount, sound.SampleCount);
		Assert.AreEqual(expectedLengthInSeconds, sound.LengthInSeconds, 0.01);
	}

	[DataTestMethod]
	[DataRow("Font.tga")]
	public void TestInvalidWaveParse(string fileName)
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath(fileName));
		Assert.ThrowsExactly<WaveParseException>(() => _ = WaveParser.Parse(bytes));
	}
}
