﻿namespace Detach;

public static partial class Inline
{
	public static ReadOnlySpan<byte> Utf8(System.Numerics.Matrix3x2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf8(ref charsWritten, "<"u8);
		WriteUtf8(ref charsWritten, value.M11, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M12, format, provider);
		WriteUtf8(ref charsWritten, "> <"u8);
		WriteUtf8(ref charsWritten, value.M21, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M22, format, provider);
		WriteUtf8(ref charsWritten, "> <"u8);
		WriteUtf8(ref charsWritten, value.M31, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M32, format, provider);
		WriteUtf8(ref charsWritten, ">"u8);

		return _bufferUtf8.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(System.Numerics.Matrix3x2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, "<");
		WriteUtf16(ref charsWritten, value.M11, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M12, format, provider);
		WriteUtf16(ref charsWritten, "> <");
		WriteUtf16(ref charsWritten, value.M21, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M22, format, provider);
		WriteUtf16(ref charsWritten, "> <");
		WriteUtf16(ref charsWritten, value.M31, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M32, format, provider);
		WriteUtf16(ref charsWritten, ">");

		return _bufferUtf16.AsSpan(0, charsWritten);
	}
}