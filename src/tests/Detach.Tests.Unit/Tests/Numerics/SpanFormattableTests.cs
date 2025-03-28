using Detach.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Unit.Tests.Numerics;

[TestClass]
public class SpanFormattableTests
{
	[TestMethod]
	public void IntVector2()
	{
		IntVector2<int> intVector2 = new(1, 2);

		Assert.AreEqual("1, 2", intVector2.ToString());
		Assert.AreEqual("1, 2", intVector2.ToString("G", null));
		Assert.AreEqual("01, 02", intVector2.ToString("00", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable("1, 2"u8, utf8, intVector2, []);
		AssertionUtils.AssertUtf8SpanFormattable("1, 2"u8, utf8, intVector2, "G");
		AssertionUtils.AssertUtf8SpanFormattable("01, 02"u8, utf8, intVector2, "00");

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable("1, 2", utf16, intVector2, []);
		AssertionUtils.AssertUtf16SpanFormattable("1, 2", utf16, intVector2, "G");
		AssertionUtils.AssertUtf16SpanFormattable("01, 02", utf16, intVector2, "00");
	}

	[TestMethod]
	public void IntVector3()
	{
		IntVector3<int> intVector3 = new(1, 2, 3);

		Assert.AreEqual("1, 2, 3", intVector3.ToString());
		Assert.AreEqual("1, 2, 3", intVector3.ToString("G", null));
		Assert.AreEqual("01, 02, 03", intVector3.ToString("00", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable("1, 2, 3"u8, utf8, intVector3, []);
		AssertionUtils.AssertUtf8SpanFormattable("1, 2, 3"u8, utf8, intVector3, "G");
		AssertionUtils.AssertUtf8SpanFormattable("01, 02, 03"u8, utf8, intVector3, "00");

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable("1, 2, 3", utf16, intVector3, []);
		AssertionUtils.AssertUtf16SpanFormattable("1, 2, 3", utf16, intVector3, "G");
		AssertionUtils.AssertUtf16SpanFormattable("01, 02, 03", utf16, intVector3, "00");
	}

	[TestMethod]
	public void IntVector4()
	{
		IntVector4<int> intVector4 = new(1, 2, 3, 4);

		Assert.AreEqual("1, 2, 3, 4", intVector4.ToString());
		Assert.AreEqual("1, 2, 3, 4", intVector4.ToString("G", null));
		Assert.AreEqual("01, 02, 03, 04", intVector4.ToString("00", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable("1, 2, 3, 4"u8, utf8, intVector4, []);
		AssertionUtils.AssertUtf8SpanFormattable("1, 2, 3, 4"u8, utf8, intVector4, "G");
		AssertionUtils.AssertUtf8SpanFormattable("01, 02, 03, 04"u8, utf8, intVector4, "00");

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable("1, 2, 3, 4", utf16, intVector4, []);
		AssertionUtils.AssertUtf16SpanFormattable("1, 2, 3, 4", utf16, intVector4, "G");
		AssertionUtils.AssertUtf16SpanFormattable("01, 02, 03, 04", utf16, intVector4, "00");
	}

	[TestMethod]
	public void Matrix2()
	{
		Matrix2 matrix2 = new(
			1, 2.25f,
			3, 4);

		Assert.AreEqual("<1, 2.25> <3, 4>", matrix2.ToString());
		Assert.AreEqual("<1, 2.25> <3, 4>", matrix2.ToString("G", null));
		Assert.AreEqual("<1.0, 2.3> <3.0, 4.0>", matrix2.ToString("0.0", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable("<1, 2.25> <3, 4>"u8, utf8, matrix2, []);
		AssertionUtils.AssertUtf8SpanFormattable("<1, 2.25> <3, 4>"u8, utf8, matrix2, "G");
		AssertionUtils.AssertUtf8SpanFormattable("<1.0, 2.3> <3.0, 4.0>"u8, utf8, matrix2, "0.0");

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable("<1, 2.25> <3, 4>", utf16, matrix2, []);
		AssertionUtils.AssertUtf16SpanFormattable("<1, 2.25> <3, 4>", utf16, matrix2, "G");
		AssertionUtils.AssertUtf16SpanFormattable("<1.0, 2.3> <3.0, 4.0>", utf16, matrix2, "0.0");
	}

	[TestMethod]
	public void Matrix3()
	{
		Matrix3 matrix3 = new(
			1, 2.25f, 3,
			4, 5, 6,
			7, 8, 9);

		Assert.AreEqual("<1, 2.25, 3> <4, 5, 6> <7, 8, 9>", matrix3.ToString());
		Assert.AreEqual("<1, 2.25, 3> <4, 5, 6> <7, 8, 9>", matrix3.ToString("G", null));
		Assert.AreEqual("<1.0, 2.3, 3.0> <4.0, 5.0, 6.0> <7.0, 8.0, 9.0>", matrix3.ToString("0.0", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable("<1, 2.25, 3> <4, 5, 6> <7, 8, 9>"u8, utf8, matrix3, []);
		AssertionUtils.AssertUtf8SpanFormattable("<1, 2.25, 3> <4, 5, 6> <7, 8, 9>"u8, utf8, matrix3, "G");
		AssertionUtils.AssertUtf8SpanFormattable("<1.0, 2.3, 3.0> <4.0, 5.0, 6.0> <7.0, 8.0, 9.0>"u8, utf8, matrix3, "0.0");

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable("<1, 2.25, 3> <4, 5, 6> <7, 8, 9>", utf16, matrix3, []);
		AssertionUtils.AssertUtf16SpanFormattable("<1, 2.25, 3> <4, 5, 6> <7, 8, 9>", utf16, matrix3, "G");
		AssertionUtils.AssertUtf16SpanFormattable("<1.0, 2.3, 3.0> <4.0, 5.0, 6.0> <7.0, 8.0, 9.0>", utf16, matrix3, "0.0");
	}

	[TestMethod]
	public void Matrix4()
	{
		Matrix4 matrix4 = new(
			1, 2.25f, 3, 4,
			5, 6, 7, 8,
			9, 10, -11, 12,
			13, 14, 15, 16);

		Assert.AreEqual("<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>", matrix4.ToString());
		Assert.AreEqual("<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>", matrix4.ToString("G", null));
		Assert.AreEqual("<1.0, 2.3, 3.0, 4.0> <5.0, 6.0, 7.0, 8.0> <9.0, 10.0, -11.0, 12.0> <13.0, 14.0, 15.0, 16.0>", matrix4.ToString("0.0", null));

		Span<byte> utf8 = stackalloc byte[96];
		AssertionUtils.AssertUtf8SpanFormattable("<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>"u8, utf8, matrix4, []);
		AssertionUtils.AssertUtf8SpanFormattable("<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>"u8, utf8, matrix4, "G");
		AssertionUtils.AssertUtf8SpanFormattable("<1.0, 2.3, 3.0, 4.0> <5.0, 6.0, 7.0, 8.0> <9.0, 10.0, -11.0, 12.0> <13.0, 14.0, 15.0, 16.0>"u8, utf8, matrix4, "0.0");

		Span<char> utf16 = stackalloc char[96];
		AssertionUtils.AssertUtf16SpanFormattable("<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>", utf16, matrix4, []);
		AssertionUtils.AssertUtf16SpanFormattable("<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>", utf16, matrix4, "G");
		AssertionUtils.AssertUtf16SpanFormattable("<1.0, 2.3, 3.0, 4.0> <5.0, 6.0, 7.0, 8.0> <9.0, 10.0, -11.0, 12.0> <13.0, 14.0, 15.0, 16.0>", utf16, matrix4, "0.0");
	}

	[TestMethod]
	public void Rgb()
	{
		Rgb rgb = new(64, 128, 192);

		Assert.AreEqual("64, 128, 192", rgb.ToString());
		Assert.AreEqual("64, 128, 192", rgb.ToString("G", null));
		Assert.AreEqual("064, 128, 192", rgb.ToString("000", null));
		Assert.AreEqual("40, 80, c0", rgb.ToString("x", null));
		Assert.AreEqual("40, 80, C0", rgb.ToString("X", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable("64, 128, 192"u8, utf8, rgb, []);
		AssertionUtils.AssertUtf8SpanFormattable("64, 128, 192"u8, utf8, rgb, "G");
		AssertionUtils.AssertUtf8SpanFormattable("064, 128, 192"u8, utf8, rgb, "000");
		AssertionUtils.AssertUtf8SpanFormattable("40, 80, c0"u8, utf8, rgb, "x");
		AssertionUtils.AssertUtf8SpanFormattable("40, 80, C0"u8, utf8, rgb, "X");

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable("64, 128, 192", utf16, rgb, []);
		AssertionUtils.AssertUtf16SpanFormattable("64, 128, 192", utf16, rgb, "G");
		AssertionUtils.AssertUtf16SpanFormattable("40, 80, c0", utf16, rgb, "x");
		AssertionUtils.AssertUtf16SpanFormattable("40, 80, C0", utf16, rgb, "X");
	}

	[TestMethod]
	public void Rgba()
	{
		Rgba rgba = new(64, 128, 192, 255);

		Assert.AreEqual("64, 128, 192, 255", rgba.ToString());
		Assert.AreEqual("64, 128, 192, 255", rgba.ToString("G", null));
		Assert.AreEqual("064, 128, 192, 255", rgba.ToString("000", null));
		Assert.AreEqual("40, 80, c0, ff", rgba.ToString("x", null));
		Assert.AreEqual("40, 80, C0, FF", rgba.ToString("X", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable("64, 128, 192, 255"u8, utf8, rgba, []);
		AssertionUtils.AssertUtf8SpanFormattable("64, 128, 192, 255"u8, utf8, rgba, "G");
		AssertionUtils.AssertUtf8SpanFormattable("064, 128, 192, 255"u8, utf8, rgba, "000");
		AssertionUtils.AssertUtf8SpanFormattable("40, 80, c0, ff"u8, utf8, rgba, "x");
		AssertionUtils.AssertUtf8SpanFormattable("40, 80, C0, FF"u8, utf8, rgba, "X");

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable("64, 128, 192, 255", utf16, rgba, []);
		AssertionUtils.AssertUtf16SpanFormattable("64, 128, 192, 255", utf16, rgba, "G");
		AssertionUtils.AssertUtf16SpanFormattable("064, 128, 192, 255", utf16, rgba, "000");
		AssertionUtils.AssertUtf16SpanFormattable("40, 80, c0, ff", utf16, rgba, "x");
		AssertionUtils.AssertUtf16SpanFormattable("40, 80, C0, FF", utf16, rgba, "X");
	}

	[TestMethod]
	public void Spinor()
	{
		Spinor spinor = new(0.5f, 0.5f);

		Assert.AreEqual("0.5, 0.5", spinor.ToString());
		Assert.AreEqual("0.5, 0.5", spinor.ToString("G", null));
		Assert.AreEqual("0.50, 0.50", spinor.ToString("0.00", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable("0.5, 0.5"u8, utf8, spinor, []);
		AssertionUtils.AssertUtf8SpanFormattable("0.5, 0.5"u8, utf8, spinor, "G");
		AssertionUtils.AssertUtf8SpanFormattable("0.50, 0.50"u8, utf8, spinor, "0.00");

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable("0.5, 0.5", utf16, spinor, []);
		AssertionUtils.AssertUtf16SpanFormattable("0.5, 0.5", utf16, spinor, "G");
		AssertionUtils.AssertUtf16SpanFormattable("0.50, 0.50", utf16, spinor, "0.00");
	}
}
