using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Unit.Tests;

[TestClass]
public class InlineBoolTests
{
	[TestMethod]
	public void Bool()
	{
		AssertionUtils.SequenceEqual("True"u8, Inline.Utf8(true));
		AssertionUtils.SequenceEqual("False"u8, Inline.Utf8(false));
		AssertionUtils.SequenceEqual("True"u8, Inline.Utf8(true)); // Test again to ensure the buffer is cleared.
		Assert.AreEqual(0x00, Inline.BufferUtf8["True"u8.Length]);

		AssertionUtils.SequenceEqual("True", Inline.Utf16(true));
		AssertionUtils.SequenceEqual("False", Inline.Utf16(false));
		AssertionUtils.SequenceEqual("True", Inline.Utf16(true));
		Assert.AreEqual('\0', Inline.BufferUtf16["True".Length]);
	}
}
