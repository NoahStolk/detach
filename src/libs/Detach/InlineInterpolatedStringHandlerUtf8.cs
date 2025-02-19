using System.Numerics;
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

	// TODO: Remove when this type implements IUtf8SpanFormattable.
	public void AppendFormatted(bool value)
	{
		AppendFormatted(value ? "True"u8 : "False"u8);
		Inline.BufferUtf8[_charsWritten] = 0x00;
	}

	// TODO: Remove when this type implements IUtf8SpanFormattable.
	public void AppendFormatted(Vector2 vector2, ReadOnlySpan<char> format = default)
	{
		AppendFormatted(vector2.X, format);
		AppendFormatted(Inline.NumericSeparatorUtf8);
		AppendFormatted(vector2.Y, format);
		Inline.BufferUtf8[_charsWritten] = 0x00;
	}

	// TODO: Remove when this type implements IUtf8SpanFormattable.
	public void AppendFormatted(Vector3 vector3, ReadOnlySpan<char> format = default)
	{
		AppendFormatted(vector3.X, format);
		AppendFormatted(Inline.NumericSeparatorUtf8);
		AppendFormatted(vector3.Y, format);
		AppendFormatted(Inline.NumericSeparatorUtf8);
		AppendFormatted(vector3.Z, format);
		Inline.BufferUtf8[_charsWritten] = 0x00;
	}

	// TODO: Remove when this type implements IUtf8SpanFormattable.
	public void AppendFormatted(Vector4 vector4, ReadOnlySpan<char> format = default)
	{
		AppendFormatted(vector4.X, format);
		AppendFormatted(Inline.NumericSeparatorUtf8);
		AppendFormatted(vector4.Y, format);
		AppendFormatted(Inline.NumericSeparatorUtf8);
		AppendFormatted(vector4.Z, format);
		AppendFormatted(Inline.NumericSeparatorUtf8);
		AppendFormatted(vector4.W, format);
		Inline.BufferUtf8[_charsWritten] = 0x00;
	}

	// TODO: Remove when this type implements IUtf8SpanFormattable.
	public void AppendFormatted(Quaternion quaternion, ReadOnlySpan<char> format = default)
	{
		AppendFormatted(quaternion.X, format);
		AppendFormatted(Inline.NumericSeparatorUtf8);
		AppendFormatted(quaternion.Y, format);
		AppendFormatted(Inline.NumericSeparatorUtf8);
		AppendFormatted(quaternion.Z, format);
		AppendFormatted(Inline.NumericSeparatorUtf8);
		AppendFormatted(quaternion.W, format);
		Inline.BufferUtf8[_charsWritten] = 0x00;
	}

	// TODO: Add Matrix3x2 and Matrix4x4 support if needed.
}
