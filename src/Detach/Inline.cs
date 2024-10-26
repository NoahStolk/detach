namespace Detach;

/// <summary>
/// <para>
/// Unsafe methods to quickly format values into either a fixed UTF-8 or a fixed UTF-16 buffer, without allocating heap memory.
/// These must only be used inline, as the buffers are only valid until the next method call.
/// </para>
/// <para>
/// The buffers have a fixed size of 2048 characters, so the total length of the formatted string cannot exceed this limit.
/// </para>
/// </summary>
public static partial class Inline
{
	private static readonly byte[] _bufferUtf8 = new byte[2048];
	private static readonly char[] _bufferUtf16 = new char[2048];

	public static Span<byte> BufferUtf8 => _bufferUtf8;
	public static Span<char> BufferUtf16 => _bufferUtf16;

	public static ReadOnlySpan<byte> Utf8(InlineInterpolatedStringHandlerUtf8 interpolatedStringHandler)
	{
		return interpolatedStringHandler;
	}

	public static ReadOnlySpan<byte> Utf8<T>(T t, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : IUtf8SpanFormattable
	{
		if (!t.TryFormat(BufferUtf8, out int charsWritten, format, provider))
			throw new InvalidOperationException("The formatted string is too long.");

		return BufferUtf8[..charsWritten];
	}

	private static void WriteUtf8(ref int charsWritten, ReadOnlySpan<byte> value)
	{
		if (!value.TryCopyTo(BufferUtf8[charsWritten..]))
			throw new InvalidOperationException("The formatted string is too long.");

		charsWritten += value.Length;
	}

	private static void WriteUtf8<T>(ref int charsWritten, T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : IUtf8SpanFormattable
	{
		if (!value.TryFormat(BufferUtf8[charsWritten..], out int charsWrittenValue, format, provider))
			throw new InvalidOperationException("The formatted string is too long.");

		charsWritten += charsWrittenValue;
	}

	public static ReadOnlySpan<char> Utf16(InlineInterpolatedStringHandlerUtf16 handler)
	{
		return handler;
	}

	public static ReadOnlySpan<char> Utf16<T>(T t, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : ISpanFormattable
	{
		if (!t.TryFormat(_bufferUtf16, out int charsWritten, format, provider))
			throw new InvalidOperationException("The formatted string is too long.");

		return BufferUtf16[..charsWritten];
	}

	private static void WriteUtf16(ref int charsWritten, string value)
	{
		if (!value.AsSpan().TryCopyTo(BufferUtf16[charsWritten..]))
			throw new InvalidOperationException("The formatted string is too long.");

		charsWritten += value.Length;
	}

	private static void WriteUtf16<T>(ref int charsWritten, T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : ISpanFormattable
	{
		if (!value.TryFormat(BufferUtf16[charsWritten..], out int charsWrittenValue, format, provider))
			throw new InvalidOperationException("The formatted string is too long.");

		charsWritten += charsWrittenValue;
	}
}
