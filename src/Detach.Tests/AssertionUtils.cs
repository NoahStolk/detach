using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using System.Text;

namespace Detach.Tests;

public static class AssertionUtils
{
	public static void AreEqual(Vector3 expected, Vector3 actual, float delta = 0.0001f)
	{
		Assert.AreEqual(expected.X, actual.X, delta);
		Assert.AreEqual(expected.Y, actual.Y, delta);
		Assert.AreEqual(expected.Z, actual.Z, delta);
	}

	public static void AssertUtf8SpanFormattable<T>(Span<byte> utf8, T value, ReadOnlySpan<char> format, ReadOnlySpan<byte> expected)
		where T : IUtf8SpanFormattable
	{
		value.TryFormat(utf8, out int bytesWritten, format, null);
		Assert.IsTrue(expected.SequenceEqual(utf8[..bytesWritten]), Encoding.UTF8.GetString(utf8[..bytesWritten]));
		utf8.Clear();
	}

	public static void AssertUtf16SpanFormattable<T>(Span<char> utf16, T value, ReadOnlySpan<char> format, ReadOnlySpan<char> expected)
		where T : ISpanFormattable
	{
		value.TryFormat(utf16, out int charsWritten, format, null);
		Assert.IsTrue(expected.SequenceEqual(utf16[..charsWritten]), new string(utf16[..charsWritten]));
		utf16.Clear();
	}
}
