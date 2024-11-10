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

	public static void SequenceEqual(ReadOnlySpan<byte> expected, ReadOnlySpan<byte> actual)
	{
		SequenceEqual(expected, actual, actual.Length);
	}

	public static void SequenceEqual(ReadOnlySpan<char> expected, ReadOnlySpan<char> actual)
	{
		SequenceEqual(expected, actual, actual.Length);
	}

	private static void SequenceEqual(ReadOnlySpan<byte> expected, ReadOnlySpan<byte> actual, int bytesWritten)
	{
		Assert.IsTrue(expected.SequenceEqual(actual[..bytesWritten]), Encoding.UTF8.GetString(actual[..bytesWritten]));
	}

	private static void SequenceEqual(ReadOnlySpan<char> expected, ReadOnlySpan<char> actual, int charsWritten)
	{
		Assert.IsTrue(expected.SequenceEqual(actual[..charsWritten]), new string(expected[..charsWritten]));
	}

	public static void AssertUtf8SpanFormattable<T>(ReadOnlySpan<byte> expected, Span<byte> utf8, T value, ReadOnlySpan<char> format)
		where T : IUtf8SpanFormattable
	{
		value.TryFormat(utf8, out int bytesWritten, format, null);
		SequenceEqual(expected, utf8, bytesWritten);
		utf8.Clear();
	}

	public static void AssertUtf16SpanFormattable<T>(ReadOnlySpan<char> expected, Span<char> utf16, T value, ReadOnlySpan<char> format)
		where T : ISpanFormattable
	{
		value.TryFormat(utf16, out int charsWritten, format, null);
		SequenceEqual(expected, utf16, charsWritten);
		utf16.Clear();
	}
}
