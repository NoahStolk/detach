using Detach.Parsers.Texture.TgaFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Unit.Tests.Parsers.Texture.TgaFormat;

[TestClass]
public sealed class TgaPixelDepthTests
{
	[TestMethod]
	public void TestBytesPerPixel()
	{
		Assert.AreEqual(4, TgaPixelDepth.Bgra.BytesPerPixel());
		Assert.AreEqual(3, TgaPixelDepth.Bgr.BytesPerPixel());
	}
}
