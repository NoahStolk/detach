using Detach.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests.Numerics;

[TestClass]
public class SpanFormattableTests
{
	[TestMethod]
	public void IntVector2()
	{
		IntVector2<int> intVector2 = new(1, 2);

		Assert.AreEqual("<1, 2>", intVector2.ToString());
		Assert.AreEqual("<1, 2>", intVector2.ToString("G", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable(utf8, intVector2, [], "<1, 2>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, intVector2, "G", "<1, 2>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, intVector2, "00", "<01, 02>"u8);

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable(utf16, intVector2, [], "<1, 2>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, intVector2, "G", "<1, 2>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, intVector2, "00", "<01, 02>");
	}

	[TestMethod]
	public void IntVector3()
	{
		IntVector3<int> intVector3 = new(1, 2, 3);

		Assert.AreEqual("<1, 2, 3>", intVector3.ToString());
		Assert.AreEqual("<1, 2, 3>", intVector3.ToString("G", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable(utf8, intVector3, [], "<1, 2, 3>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, intVector3, "G", "<1, 2, 3>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, intVector3, "00", "<01, 02, 03>"u8);

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable(utf16, intVector3, [], "<1, 2, 3>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, intVector3, "G", "<1, 2, 3>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, intVector3, "00", "<01, 02, 03>");
	}

	[TestMethod]
	public void IntVector4()
	{
		IntVector4<int> intVector4 = new(1, 2, 3, 4);

		Assert.AreEqual("<1, 2, 3, 4>", intVector4.ToString());
		Assert.AreEqual("<1, 2, 3, 4>", intVector4.ToString("G", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable(utf8, intVector4, [], "<1, 2, 3, 4>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, intVector4, "G", "<1, 2, 3, 4>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, intVector4, "00", "<01, 02, 03, 04>"u8);

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable(utf16, intVector4, [], "<1, 2, 3, 4>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, intVector4, "G", "<1, 2, 3, 4>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, intVector4, "00", "<01, 02, 03, 04>");
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
		AssertionUtils.AssertUtf8SpanFormattable(utf8, matrix2, [], "<1, 2.25> <3, 4>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, matrix2, "G", "<1, 2.25> <3, 4>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, matrix2, "0.0", "<1.0, 2.3> <3.0, 4.0>"u8);

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable(utf16, matrix2, [], "<1, 2.25> <3, 4>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, matrix2, "G", "<1, 2.25> <3, 4>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, matrix2, "0.0", "<1.0, 2.3> <3.0, 4.0>");
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
		AssertionUtils.AssertUtf8SpanFormattable(utf8, matrix3, [], "<1, 2.25, 3> <4, 5, 6> <7, 8, 9>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, matrix3, "G", "<1, 2.25, 3> <4, 5, 6> <7, 8, 9>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, matrix3, "0.0", "<1.0, 2.3, 3.0> <4.0, 5.0, 6.0> <7.0, 8.0, 9.0>"u8);

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable(utf16, matrix3, [], "<1, 2.25, 3> <4, 5, 6> <7, 8, 9>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, matrix3, "G", "<1, 2.25, 3> <4, 5, 6> <7, 8, 9>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, matrix3, "0.0", "<1.0, 2.3, 3.0> <4.0, 5.0, 6.0> <7.0, 8.0, 9.0>");
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
		AssertionUtils.AssertUtf8SpanFormattable(utf8, matrix4, [], "<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, matrix4, "G", "<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, matrix4, "0.0", "<1.0, 2.3, 3.0, 4.0> <5.0, 6.0, 7.0, 8.0> <9.0, 10.0, -11.0, 12.0> <13.0, 14.0, 15.0, 16.0>"u8);

		Span<char> utf16 = stackalloc char[96];
		AssertionUtils.AssertUtf16SpanFormattable(utf16, matrix4, [], "<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, matrix4, "G", "<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, matrix4, "0.0", "<1.0, 2.3, 3.0, 4.0> <5.0, 6.0, 7.0, 8.0> <9.0, 10.0, -11.0, 12.0> <13.0, 14.0, 15.0, 16.0>");
	}

	[TestMethod]
	public void Rgb()
	{
		Rgb rgb = new(64, 128, 192);

		Assert.AreEqual("64, 128, 192", rgb.ToString());
		Assert.AreEqual("64, 128, 192", rgb.ToString("G", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable(utf8, rgb, [], "64, 128, 192"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, rgb, "G", "64, 128, 192"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, rgb, "000", "064, 128, 192"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, rgb, "x", "40, 80, c0"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, rgb, "X", "40, 80, C0"u8);

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable(utf16, rgb, [], "64, 128, 192");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, rgb, "G", "64, 128, 192");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, rgb, "x", "40, 80, c0");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, rgb, "X", "40, 80, C0");
	}

	[TestMethod]
	public void Rgba()
	{
		Rgba rgba = new(64, 128, 192, 255);

		Assert.AreEqual("64, 128, 192, 255", rgba.ToString());
		Assert.AreEqual("64, 128, 192, 255", rgba.ToString("G", null));

		Span<byte> utf8 = stackalloc byte[64];
		AssertionUtils.AssertUtf8SpanFormattable(utf8, rgba, [], "64, 128, 192, 255"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, rgba, "G", "64, 128, 192, 255"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, rgba, "000", "064, 128, 192, 255"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, rgba, "x", "40, 80, c0, ff"u8);
		AssertionUtils.AssertUtf8SpanFormattable(utf8, rgba, "X", "40, 80, C0, FF"u8);

		Span<char> utf16 = stackalloc char[64];
		AssertionUtils.AssertUtf16SpanFormattable(utf16, rgba, [], "64, 128, 192, 255");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, rgba, "G", "64, 128, 192, 255");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, rgba, "000", "064, 128, 192, 255");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, rgba, "x", "40, 80, c0, ff");
		AssertionUtils.AssertUtf16SpanFormattable(utf16, rgba, "X", "40, 80, C0, FF");
	}
}
