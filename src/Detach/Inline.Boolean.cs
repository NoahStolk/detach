namespace Detach;

// TODO: Remove when this type implements ISpanFormattable and IUtf8SpanFormattable.
public static partial class Inline
{
	public static ReadOnlySpan<byte> Utf8(bool value)
	{
		int charsWritten = 0;
		WriteUtf8(ref charsWritten, value ? "True"u8 : "False"u8);

		return _bufferUtf8.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(bool value)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value ? "True" : "False");

		return _bufferUtf16.AsSpan(0, charsWritten);
	}
}
