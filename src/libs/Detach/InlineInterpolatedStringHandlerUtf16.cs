using System.Runtime.CompilerServices;

namespace Detach;

#pragma warning disable RCS1163
[InterpolatedStringHandler]
public ref struct InlineInterpolatedStringHandlerUtf16
{
	private int _charsWritten;

	// ReSharper disable UnusedParameter.Local
	public InlineInterpolatedStringHandlerUtf16(int literalLength, int formattedCount)
	{
	}

	// ReSharper restore UnusedParameter.Local
	public static implicit operator ReadOnlySpan<char>(InlineInterpolatedStringHandlerUtf16 handler)
	{
		Inline.BufferUtf16[handler._charsWritten] = '\0'; // Null-terminate the string in case the underlying memory is used directly.
		return Inline.BufferUtf16[..handler._charsWritten];
	}

	public void AppendLiteral(string s)
	{
		if (!s.TryCopyTo(Inline.BufferUtf16[_charsWritten..]))
			throw new InvalidOperationException("The formatted string is too long.");

		_charsWritten += s.Length;
	}

	public void AppendFormatted(ReadOnlySpan<char> s)
	{
		if (!s.TryCopyTo(Inline.BufferUtf16[_charsWritten..]))
			throw new InvalidOperationException("The formatted string is too long.");

		_charsWritten += s.Length;
	}

	public void AppendFormatted<T>(T t, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : ISpanFormattable
	{
		if (!t.TryFormat(Inline.BufferUtf16[_charsWritten..], out int charsWritten, format, provider))
			throw new InvalidOperationException("The formatted string is too long.");

		_charsWritten += charsWritten;
	}

	// TODO: Add bool, Vector, Matrix3x2 and Matrix4x4 support if needed.
}
