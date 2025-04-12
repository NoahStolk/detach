using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Unit.Tests;

[TestClass]
public sealed class InlineSpanFormattableTests
{
	[TestMethod]
	public void FloatingPoint32()
	{
		const float value = 1.12f;

		AssertionUtils.SequenceEqual("1.12"u8, Inline.Utf8(value, [], null));
		AssertionUtils.SequenceEqual("1.12"u8, Inline.Utf8(value, "G", null));
		AssertionUtils.SequenceEqual("01"u8, Inline.Utf8(value, "00", null));
		AssertionUtils.SequenceEqual("01.1"u8, Inline.Utf8(value, "00.#", null));
		AssertionUtils.SequenceEqual("01.1"u8, Inline.Utf8(value, "00.0", null));
		AssertionUtils.SequenceEqual("1"u8, Inline.Utf8(value, "0", null));
		Assert.AreEqual(0x00, Inline.BufferUtf8["1"u8.Length]);

		AssertionUtils.SequenceEqual("1.12", Inline.Utf16(value, [], null));
		AssertionUtils.SequenceEqual("1.12", Inline.Utf16(value, "G", null));
		AssertionUtils.SequenceEqual("01", Inline.Utf16(value, "00", null));
		AssertionUtils.SequenceEqual("01.1", Inline.Utf16(value, "00.#", null));
		AssertionUtils.SequenceEqual("01.1", Inline.Utf16(value, "00.0", null));
		AssertionUtils.SequenceEqual("1", Inline.Utf16(value, "0", null));
		Assert.AreEqual('\0', Inline.BufferUtf16["1".Length]);
	}

	[TestMethod]
	public void Integer32()
	{
		const int value = 123;

		AssertionUtils.SequenceEqual("123"u8, Inline.Utf8(value, [], null));
		AssertionUtils.SequenceEqual("123"u8, Inline.Utf8(value, "G", null));
		AssertionUtils.SequenceEqual("123"u8, Inline.Utf8(value, "00", null));
		AssertionUtils.SequenceEqual("123"u8, Inline.Utf8(value, "00.#", null));
		AssertionUtils.SequenceEqual("123.0"u8, Inline.Utf8(value, "00.0", null));
		AssertionUtils.SequenceEqual("0123"u8, Inline.Utf8(value, "0000", null));
		AssertionUtils.SequenceEqual("123"u8, Inline.Utf8(value, "0", null));
		Assert.AreEqual(0x00, Inline.BufferUtf8["123"u8.Length]);

		AssertionUtils.SequenceEqual("123", Inline.Utf16(value, [], null));
		AssertionUtils.SequenceEqual("123", Inline.Utf16(value, "G", null));
		AssertionUtils.SequenceEqual("123", Inline.Utf16(value, "00", null));
		AssertionUtils.SequenceEqual("123", Inline.Utf16(value, "00.#", null));
		AssertionUtils.SequenceEqual("123.0", Inline.Utf16(value, "00.0", null));
		AssertionUtils.SequenceEqual("0123", Inline.Utf16(value, "0000", null));
		AssertionUtils.SequenceEqual("123", Inline.Utf16(value, "0", null));
		Assert.AreEqual('\0', Inline.BufferUtf16["123".Length]);
	}
}
