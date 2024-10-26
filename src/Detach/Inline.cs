using Detach.Numerics;
using System.Numerics;

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
public static class Inline
{
	private const string _separatorUtf16 = ", ";

	private static readonly byte[] _bufferUtf8 = new byte[2048];
	private static readonly char[] _bufferUtf16 = new char[2048];

	public static Span<byte> BufferUtf8 => _bufferUtf8;
	public static Span<char> BufferUtf16 => _bufferUtf16;

	private static ReadOnlySpan<byte> SeparatorUtf8 => ", "u8;

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

	public static ReadOnlySpan<byte> Utf8(Vector2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf8(ref charsWritten, value.X, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.Y, format, provider);
		return _bufferUtf8.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<byte> Utf8(Vector3 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf8(ref charsWritten, value.X, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.Y, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.Z, format, provider);
		return _bufferUtf8.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<byte> Utf8(Vector4 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf8(ref charsWritten, value.X, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.Y, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.Z, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.W, format, provider);
		return _bufferUtf8.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<byte> Utf8(Quaternion value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf8(ref charsWritten, value.X, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.Y, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.Z, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.W, format, provider);
		return _bufferUtf8.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<byte> Utf8(Rgba value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf8(ref charsWritten, value.R, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.G, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.B, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.A, format, provider);
		return _bufferUtf8.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<byte> Utf8(Matrix3x2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf8(ref charsWritten, value.M11, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M12, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M21, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M22, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M31, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M32, format, provider);
		return _bufferUtf8.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<byte> Utf8(Matrix4x4 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf8(ref charsWritten, value.M11, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M12, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M13, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M14, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M21, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M22, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M23, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M24, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M31, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M32, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M33, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M34, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M41, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M42, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M43, format, provider);
		WriteUtf8(ref charsWritten, SeparatorUtf8);
		WriteUtf8(ref charsWritten, value.M44, format, provider);
		return _bufferUtf8.AsSpan(0, charsWritten);
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

	public static ReadOnlySpan<char> Utf16(Vector2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.X, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.Y, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Vector3 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.X, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.Y, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.Z, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Vector4 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.X, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.Y, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.Z, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.W, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Quaternion value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.X, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.Y, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.Z, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.W, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Rgba value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.R, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.G, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.B, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.A, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Matrix3x2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.M11, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M12, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M21, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M22, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M31, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M32, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Matrix4x4 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.M11, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M12, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M13, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M14, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M21, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M22, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M23, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M24, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M31, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M32, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M33, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M34, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M41, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M42, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M43, format, provider);
		WriteUtf16(ref charsWritten, _separatorUtf16);
		WriteUtf16(ref charsWritten, value.M44, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
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
