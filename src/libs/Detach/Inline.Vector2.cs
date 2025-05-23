﻿using System.Numerics;

namespace Detach;

// TODO: Remove when this type implements ISpanFormattable and IUtf8SpanFormattable.
public static partial class Inline
{
	public static ReadOnlySpan<byte> Utf8(Vector2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf8(ref charsWritten, value.X, format, provider);
		WriteUtf8(ref charsWritten, NumericSeparatorUtf8);
		WriteUtf8(ref charsWritten, value.Y, format, provider);
		WriteUtf8(ref charsWritten, "\0"u8);

		return _bufferUtf8.AsSpan(0, charsWritten - 1);
	}

	public static ReadOnlySpan<char> Utf16(Vector2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.X, format, provider);
		WriteUtf16(ref charsWritten, NumericSeparatorUtf16);
		WriteUtf16(ref charsWritten, value.Y, format, provider);
		WriteUtf16(ref charsWritten, "\0");

		return _bufferUtf16.AsSpan(0, charsWritten - 1);
	}
}
