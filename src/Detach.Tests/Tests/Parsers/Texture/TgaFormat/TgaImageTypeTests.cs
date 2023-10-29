using Detach.Parsers.Texture.TgaFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests.Parsers.Texture.TgaFormat;

[TestClass]
public class TgaImageTypeTests
{
	[TestMethod]
	public void TestRunLengthEncoded()
	{
		Assert.IsFalse(TgaImageType.NoImageData.IsRunLengthEncoded());
		Assert.IsFalse(TgaImageType.ColorMapped.IsRunLengthEncoded());
		Assert.IsFalse(TgaImageType.TrueColor.IsRunLengthEncoded());
		Assert.IsFalse(TgaImageType.Grayscale.IsRunLengthEncoded());
		Assert.IsTrue(TgaImageType.RunLengthEncodedColorMapped.IsRunLengthEncoded());
		Assert.IsTrue(TgaImageType.RunLengthEncodedTrueColor.IsRunLengthEncoded());
		Assert.IsTrue(TgaImageType.RunLengthEncodedGrayscale.IsRunLengthEncoded());
	}
}
