using Detach.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Numerics;

[TestClass]
public class RgbTests
{
	[DataTestMethod]
	[DataRow(0, 0, 0)]
	[DataRow(1, 2, 3)]
	[DataRow(5, 6, 7)]
	[DataRow(255, 6, 255)]
	[DataRow(255, 255, 255)]
	public void RgbConversions(int r, int g, int b)
	{
		Rgb expectedRgb = new((byte)r, (byte)g, (byte)b);

		Assert.AreEqual(expectedRgb, Rgb.FromVector4(new Vector4(r / 255f, g / 255f, b / 255f, 1)));
		Assert.AreEqual(expectedRgb, Rgb.FromVector3(new Vector3(r / 255f, g / 255f, b / 255f)));
		Assert.AreEqual(expectedRgb, Rgb.FromRgbInt(expectedRgb.ToRgbInt()));
	}
}
