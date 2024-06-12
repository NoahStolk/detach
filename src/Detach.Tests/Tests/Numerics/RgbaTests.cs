using Detach.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Tests.Numerics;

[TestClass]
public class RgbaTests
{
	[DataTestMethod]
	[DataRow(0, 0, 0, 0)]
	[DataRow(1, 2, 3, 4)]
	[DataRow(5, 6, 7, 8)]
	[DataRow(255, 6, 255, 8)]
	[DataRow(255, 255, 255, 255)]
	public void RgbaConversions(int r, int g, int b, int a)
	{
		Rgba expectedRgba = new((byte)r, (byte)g, (byte)b, (byte)a);
		Rgba expectedRgb = expectedRgba with { A = byte.MaxValue };

		Assert.AreEqual(expectedRgba, Rgba.FromVector4(new Vector4(r / 255f, g / 255f, b / 255f, a / 255f)));
		Assert.AreEqual(expectedRgb, Rgba.FromVector3(new Vector3(r / 255f, g / 255f, b / 255f)));

		Assert.AreEqual(expectedRgba, Rgba.FromRgbaInt(expectedRgba.ToRgbaInt()));
		Assert.AreEqual(expectedRgb, Rgba.FromRgbaInt(expectedRgb.ToRgbaInt()));

		Assert.AreEqual(expectedRgba, Rgba.FromArgbInt(expectedRgba.ToArgbInt()));
		Assert.AreEqual(expectedRgb, Rgba.FromArgbInt(expectedRgb.ToArgbInt()));
	}
}
