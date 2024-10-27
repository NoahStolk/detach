namespace Detach.Utils;

internal static class SpanFormattableUtils
{
	public static bool TryFormatLiteral(ReadOnlySpan<byte> literal, Span<byte> utf8Destination, ref int bytesWritten)
	{
		bool success = literal.TryCopyTo(utf8Destination[bytesWritten..]);
		bytesWritten += literal.Length;
		return success;
	}

	public static bool TryFormatLiteral(string literal, Span<char> destination, ref int charsWritten)
	{
		bool success = literal.AsSpan().TryCopyTo(destination[charsWritten..]);
		charsWritten += literal.Length;
		return success;
	}

	public static bool TryFormat<T>(T value, Span<byte> utf8Destination, ref int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		where T : IUtf8SpanFormattable
	{
		bool success = value.TryFormat(utf8Destination[bytesWritten..], out int bytesWrittenOut, format, provider);
		bytesWritten += bytesWrittenOut;
		return success;
	}

	public static bool TryFormat<T>(T value, Span<char> destination, ref int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
		where T : ISpanFormattable
	{
		bool success = value.TryFormat(destination[charsWritten..], out int charsWrittenOut, format, provider);
		charsWritten += charsWrittenOut;
		return success;
	}
}
