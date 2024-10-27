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
		AssertionUtils.SequenceEqual("01.1, 02.0"u8, Inline.Utf8(vector2, "00.0", null));

		AssertionUtils.SequenceEqual("1.12, 2", Inline.Utf16(vector2, [], null));
		AssertionUtils.SequenceEqual("1.12, 2", Inline.Utf16(vector2, "G", null));
		AssertionUtils.SequenceEqual("01, 02", Inline.Utf16(vector2, "00", null));
		AssertionUtils.SequenceEqual("01.1, 02.0", Inline.Utf16(vector2, "00.0", null));
	}
}
