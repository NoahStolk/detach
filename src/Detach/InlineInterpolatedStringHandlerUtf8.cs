using System.Runtime.CompilerServices;
using System.Text;

namespace Detach;

#pragma warning disable RCS1163
[InterpolatedStringHandler]
public ref struct InlineInterpolatedStringHandlerUtf8
{
	private int _charsWritten;

	// ReSharper disable UnusedParameter.Local
	public InlineInterpolatedStringHandlerUtf8(int literalLength, int formattedCount)
	{
	}

	// ReSharper restore UnusedParameter.Local
	public static implicit operator ReadOnlySpan<byte>(InlineInterpolatedStringHandlerUtf8 handler)
	{
		Inline.BufferUtf8[handler._charsWritten] = 0x00; // Null-terminate the UTF-8 string in case the underlying memory is used directly.
		return Inline.BufferUtf8[..handler._charsWritten];
	}

	public unsafe void AppendLiteral(string s)
	{
		fixed (char* utf16Ptr = s)
		{
			int utf8ByteCount = Encoding.UTF8.GetByteCount(utf16Ptr, s.Length);
			if (utf8ByteCount == 0)
				return;

			if (utf8ByteCount > Inline.BufferUtf8.Length - _charsWritten)
				throw new InvalidOperationException("The formatted string is too long.");

			fixed (byte* bufferPtr = Inline.BufferUtf8)
				_charsWritten += Encoding.UTF8.GetBytes(utf16Ptr, s.Length, bufferPtr + _charsWritten, utf8ByteCount);
		}
	}

	public void AppendFormatted(ReadOnlySpan<byte> s)
	{
		if (!s.TryCopyTo(Inline.BufferUtf8[_charsWritten..]))
			throw new InvalidOperationException("The formatted string is too long.");

		_charsWritten += s.Length;
	}

	public void AppendFormatted<T>(T t, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : IUtf8SpanFormattable
	{
		if (!t.TryFormat(Inline.BufferUtf8[_charsWritten..], out int charsWritten, format, provider))
			throw new InvalidOperationException("The formatted string is too long.");

		_charsWritten += charsWritten;
	}
}
