using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests;

[TestClass]
public class InlineBoolTests
{
	[TestMethod]
	public void Bool()
	{
		AssertionUtils.SequenceEqual("True"u8, Inline.Utf8(true));
		AssertionUtils.SequenceEqual("False"u8, Inline.Utf8(false));

		AssertionUtils.SequenceEqual("True", Inline.Utf16(true));
		AssertionUtils.SequenceEqual("False", Inline.Utf16(false));
	}
}
