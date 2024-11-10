using Detach.Parsers.Texture.TgaFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests.Parsers.Texture.TgaFormat;

[TestClass]
public class TgaPixelDepthTests
{
	[TestMethod]
	public void TestBytesPerPixel()
	{
		Assert.AreEqual(4, TgaPixelDepth.Bgra.BytesPerPixel());
		Assert.AreEqual(3, TgaPixelDepth.Bgr.BytesPerPixel());
	}
}
