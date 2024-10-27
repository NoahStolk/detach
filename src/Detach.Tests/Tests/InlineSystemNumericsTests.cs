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
}
