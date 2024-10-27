using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Tests;

[TestClass]
public class InlineSystemNumericsTests
{
	[TestMethod]
	public void Vector2()
	{
		Vector2 vector2 = new(1.12f, 2);

		AssertionUtils.SequenceEqual("1.12, 2"u8, Inline.Utf8(vector2, [], null));
		AssertionUtils.SequenceEqual("1.12, 2"u8, Inline.Utf8(vector2, "G", null));
		AssertionUtils.SequenceEqual("01, 02"u8, Inline.Utf8(vector2, "00", null));
		AssertionUtils.SequenceEqual("01.1, 02"u8, Inline.Utf8(vector2, "00.#", null));
		AssertionUtils.SequenceEqual("01.1, 02.0"u8, Inline.Utf8(vector2, "00.0", null));

		AssertionUtils.SequenceEqual("1.12, 2", Inline.Utf16(vector2, [], null));
		AssertionUtils.SequenceEqual("1.12, 2", Inline.Utf16(vector2, "G", null));
		AssertionUtils.SequenceEqual("01, 02", Inline.Utf16(vector2, "00", null));
		AssertionUtils.SequenceEqual("01.1, 02", Inline.Utf16(vector2, "00.#", null));
		AssertionUtils.SequenceEqual("01.1, 02.0", Inline.Utf16(vector2, "00.0", null));
	}

	[TestMethod]
	public void Vector3()
	{
		Vector3 vector3 = new(1.12f, 2, 3.456f);

		AssertionUtils.SequenceEqual("1.12, 2, 3.456"u8, Inline.Utf8(vector3, [], null));
		AssertionUtils.SequenceEqual("1.12, 2, 3.456"u8, Inline.Utf8(vector3, "G", null));
		AssertionUtils.SequenceEqual("01, 02, 03"u8, Inline.Utf8(vector3, "00", null));
		AssertionUtils.SequenceEqual("01.1, 02, 03.5"u8, Inline.Utf8(vector3, "00.#", null));
		AssertionUtils.SequenceEqual("01.1, 02.0, 03.5"u8, Inline.Utf8(vector3, "00.0", null));

		AssertionUtils.SequenceEqual("1.12, 2, 3.456", Inline.Utf16(vector3, [], null));
		AssertionUtils.SequenceEqual("1.12, 2, 3.456", Inline.Utf16(vector3, "G", null));
		AssertionUtils.SequenceEqual("01, 02, 03", Inline.Utf16(vector3, "00", null));
		AssertionUtils.SequenceEqual("01.1, 02, 03.5", Inline.Utf16(vector3, "00.#", null));
		AssertionUtils.SequenceEqual("01.1, 02.0, 03.5", Inline.Utf16(vector3, "00.0", null));
	}

	[TestMethod]
	public void Vector4()
	{
		Vector4 vector4 = new(1.12f, 2, 3.456f, 4.789f);

		AssertionUtils.SequenceEqual("1.12, 2, 3.456, 4.789"u8, Inline.Utf8(vector4, [], null));
		AssertionUtils.SequenceEqual("1.12, 2, 3.456, 4.789"u8, Inline.Utf8(vector4, "G", null));
		AssertionUtils.SequenceEqual("01, 02, 03, 05"u8, Inline.Utf8(vector4, "00", null));
		AssertionUtils.SequenceEqual("01.1, 02, 03.5, 04.8"u8, Inline.Utf8(vector4, "00.#", null));
		AssertionUtils.SequenceEqual("01.1, 02.0, 03.5, 04.8"u8, Inline.Utf8(vector4, "00.0", null));

		AssertionUtils.SequenceEqual("1.12, 2, 3.456, 4.789", Inline.Utf16(vector4, [], null));
		AssertionUtils.SequenceEqual("1.12, 2, 3.456, 4.789", Inline.Utf16(vector4, "G", null));
		AssertionUtils.SequenceEqual("01, 02, 03, 05", Inline.Utf16(vector4, "00", null));
		AssertionUtils.SequenceEqual("01.1, 02, 03.5, 04.8", Inline.Utf16(vector4, "00.#", null));
		AssertionUtils.SequenceEqual("01.1, 02.0, 03.5, 04.8", Inline.Utf16(vector4, "00.0", null));
	}

	[TestMethod]
	public void Quaternion()
	{
		Quaternion quaternion = new(1.12f, 2, 3.456f, 4.789f);

		AssertionUtils.SequenceEqual("1.12, 2, 3.456, 4.789"u8, Inline.Utf8(quaternion, [], null));
		AssertionUtils.SequenceEqual("1.12, 2, 3.456, 4.789"u8, Inline.Utf8(quaternion, "G", null));
		AssertionUtils.SequenceEqual("01, 02, 03, 05"u8, Inline.Utf8(quaternion, "00", null));
		AssertionUtils.SequenceEqual("01.1, 02, 03.5, 04.8"u8, Inline.Utf8(quaternion, "00.#", null));
		AssertionUtils.SequenceEqual("01.1, 02.0, 03.5, 04.8"u8, Inline.Utf8(quaternion, "00.0", null));

		AssertionUtils.SequenceEqual("1.12, 2, 3.456, 4.789", Inline.Utf16(quaternion, [], null));
		AssertionUtils.SequenceEqual("1.12, 2, 3.456, 4.789", Inline.Utf16(quaternion, "G", null));
		AssertionUtils.SequenceEqual("01, 02, 03, 05", Inline.Utf16(quaternion, "00", null));
		AssertionUtils.SequenceEqual("01.1, 02, 03.5, 04.8", Inline.Utf16(quaternion, "00.#", null));
		AssertionUtils.SequenceEqual("01.1, 02.0, 03.5, 04.8", Inline.Utf16(quaternion, "00.0", null));
	}

	// ReSharper disable once InconsistentNaming
	[TestMethod]
	public void Matrix3x2()
	{
		// ReSharper disable once InconsistentNaming
		Matrix3x2 matrix3x2 = new(
			1, 2.25f,
			3, 4,
			5, 6);

		AssertionUtils.SequenceEqual("<1, 2.25> <3, 4> <5, 6>"u8, Inline.Utf8(matrix3x2, [], null));
		AssertionUtils.SequenceEqual("<1, 2.25> <3, 4> <5, 6>"u8, Inline.Utf8(matrix3x2, "G", null));
		AssertionUtils.SequenceEqual("<01, 02> <03, 04> <05, 06>"u8, Inline.Utf8(matrix3x2, "00", null));
		AssertionUtils.SequenceEqual("<01, 02.3> <03, 04> <05, 06>"u8, Inline.Utf8(matrix3x2, "00.#", null));
		AssertionUtils.SequenceEqual("<01.0, 02.3> <03.0, 04.0> <05.0, 06.0>"u8, Inline.Utf8(matrix3x2, "00.0", null));

		AssertionUtils.SequenceEqual("<1, 2.25> <3, 4> <5, 6>", Inline.Utf16(matrix3x2, [], null));
		AssertionUtils.SequenceEqual("<1, 2.25> <3, 4> <5, 6>", Inline.Utf16(matrix3x2, "G", null));
		AssertionUtils.SequenceEqual("<01, 02> <03, 04> <05, 06>", Inline.Utf16(matrix3x2, "00", null));
		AssertionUtils.SequenceEqual("<01, 02.3> <03, 04> <05, 06>", Inline.Utf16(matrix3x2, "00.#", null));
		AssertionUtils.SequenceEqual("<01.0, 02.3> <03.0, 04.0> <05.0, 06.0>", Inline.Utf16(matrix3x2, "00.0", null));
	}

	// ReSharper disable once InconsistentNaming
	[TestMethod]
	public void Matrix4x4()
	{
		// ReSharper disable once InconsistentNaming
		Matrix4x4 matrix4x4 = new(
			1, 2.25f, 3, 4,
			5, 6, 7, 8,
			9, 10, -11, 12,
			13, 14, 15, 16);

		AssertionUtils.SequenceEqual("<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>"u8, Inline.Utf8(matrix4x4, [], null));
		AssertionUtils.SequenceEqual("<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>"u8, Inline.Utf8(matrix4x4, "G", null));
		AssertionUtils.SequenceEqual("<01, 02, 03, 04> <05, 06, 07, 08> <09, 10, -11, 12> <13, 14, 15, 16>"u8, Inline.Utf8(matrix4x4, "00", null));
		AssertionUtils.SequenceEqual("<01, 02.3, 03, 04> <05, 06, 07, 08> <09, 10, -11, 12> <13, 14, 15, 16>"u8, Inline.Utf8(matrix4x4, "00.#", null));
		AssertionUtils.SequenceEqual("<01.0, 02.3, 03.0, 04.0> <05.0, 06.0, 07.0, 08.0> <09.0, 10.0, -11.0, 12.0> <13.0, 14.0, 15.0, 16.0>"u8, Inline.Utf8(matrix4x4, "00.0", null));

		AssertionUtils.SequenceEqual("<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>", Inline.Utf16(matrix4x4, [], null));
		AssertionUtils.SequenceEqual("<1, 2.25, 3, 4> <5, 6, 7, 8> <9, 10, -11, 12> <13, 14, 15, 16>", Inline.Utf16(matrix4x4, "G", null));
		AssertionUtils.SequenceEqual("<01, 02, 03, 04> <05, 06, 07, 08> <09, 10, -11, 12> <13, 14, 15, 16>", Inline.Utf16(matrix4x4, "00", null));
		AssertionUtils.SequenceEqual("<01, 02.3, 03, 04> <05, 06, 07, 08> <09, 10, -11, 12> <13, 14, 15, 16>", Inline.Utf16(matrix4x4, "00.#", null));
		AssertionUtils.SequenceEqual("<01.0, 02.3, 03.0, 04.0> <05.0, 06.0, 07.0, 08.0> <09.0, 10.0, -11.0, 12.0> <13.0, 14.0, 15.0, 16.0>", Inline.Utf16(matrix4x4, "00.0", null));
	}
}
