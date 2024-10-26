using Detach.Numerics;
using System.Numerics;

namespace Detach;

/// <summary>
/// <para>
/// Unsafe methods to quickly format values into a <see cref="Span{T}"/> without allocating heap memory.
/// These must only be used inline, as the <see cref="Span{T}"/> is only valid until the next method call.
/// </para>
/// <para>
/// The <see cref="Span{T}"/> is backed by a static buffer of 2048 characters, so the total length of the formatted string cannot exceed this limit.
/// </para>
/// </summary>
public static class Inline
{
	private static readonly byte[] _bufferUtf8 = new byte[2048];
	private static readonly char[] _bufferUtf16 = new char[2048];

	public static Span<byte> BufferUtf8 => _bufferUtf8;
	public static Span<char> BufferUtf16 => _bufferUtf16;

	public static ReadOnlySpan<char> Utf16(InlineInterpolatedStringHandlerUtf16 handler)
	{
		return handler;
	}

	public static ReadOnlySpan<char> Utf16<T>(T t, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : ISpanFormattable
	{
		return t.TryFormat(_bufferUtf16, out int charsWritten, format, provider) ? _bufferUtf16.AsSpan()[..charsWritten] : ReadOnlySpan<char>.Empty;
	}

	public static ReadOnlySpan<char> Utf16(Vector2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.X, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.Y, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Vector3 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.X, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.Y, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.Z, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Vector4 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.X, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.Y, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.Z, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.W, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Quaternion value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.X, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.Y, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.Z, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.W, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Rgba value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.R, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.G, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.B, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.A, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Matrix3x2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.M11, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M12, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M21, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M22, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M31, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M32, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(Matrix4x4 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		WriteUtf16(ref charsWritten, value.M11, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M12, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M13, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M14, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M21, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M22, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M23, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M24, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M31, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M32, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M33, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M34, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M41, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M42, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M43, format, provider);
		WriteUtf16(ref charsWritten, ", ");
		WriteUtf16(ref charsWritten, value.M44, format, provider);
		return _bufferUtf16.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Utf16(string str)
	{
		return str.AsSpan();
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
