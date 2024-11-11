#pragma warning disable SA1201 // Elements should appear in the correct order
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable S1104 // Fields should not have public accessibility
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields

using Detach.Utils;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Detach.Numerics;

public struct IntVector4<T> : IEquatable<IntVector4<T>>, ISpanFormattable, IUtf8SpanFormattable
	where T : IBinaryInteger<T>, IMinMaxValue<T>
{
	public T X;
	public T Y;
	public T Z;
	public T W;

	public IntVector4(T value)
		: this(value, value, value, value)
	{
	}

	public IntVector4(T x, T y, T z, T w)
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public static IntVector4<T> Zero => default;

	public static IntVector4<T> One => new(T.One);

	public static IntVector4<T> UnitX => new(T.One, T.Zero, T.Zero, T.Zero);

	public static IntVector4<T> UnitY => new(T.Zero, T.One, T.Zero, T.Zero);

	public static IntVector4<T> UnitZ => new(T.Zero, T.Zero, T.One, T.Zero);

	public static IntVector4<T> UnitW => new(T.Zero, T.Zero, T.Zero, T.One);

	public T this[T index]
	{
		get
		{
			return index switch
			{
				0 => X,
				1 => Y,
				2 => Z,
				3 => W,
				_ => throw new IndexOutOfRangeException($"Invalid index for {nameof(IntVector4<T>)}: {index}"),
			};
		}
		set
		{
			switch (index)
			{
				case 0: X = value; break;
				case 1: Y = value; break;
				case 2: Z = value; break;
				case 3: W = value; break;
				default: throw new IndexOutOfRangeException($"Invalid index for {nameof(IntVector4<T>)}: {index}");
			}
		}
	}

	#region Equality

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(IntVector4<T> left, IntVector4<T> right)
	{
		return left.X == right.X && left.Y == right.Y && left.Z == right.Z && left.W == right.W;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(IntVector4<T> left, IntVector4<T> right)
	{
		return !(left == right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override readonly bool Equals([NotNullWhen(true)] object? obj)
	{
		return obj is IntVector4<T> other && Equals(other);
	}

	public readonly bool Equals(IntVector4<T> other)
	{
		return this == other;
	}

	public override readonly int GetHashCode()
	{
		return HashCode.Combine(X, Y, Z, W);
	}

	#endregion Equality

	#region Addition

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> operator +(IntVector4<T> left, IntVector4<T> right)
	{
		return new IntVector4<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
	}

	#endregion Addition

	#region Subtraction

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> operator -(IntVector4<T> left, IntVector4<T> right)
	{
		return new IntVector4<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
	}

	#endregion Subtraction

	#region Multiplication

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> operator *(IntVector4<T> left, IntVector4<T> right)
	{
		return new IntVector4<T>(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> operator *(IntVector4<T> left, T right)
	{
		return left * new IntVector4<T>(right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> operator *(T left, IntVector4<T> right)
	{
		return right * left;
	}

	#endregion Multiplication

	#region Division

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> operator /(IntVector4<T> left, IntVector4<T> right)
	{
		return new IntVector4<T>(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> operator /(IntVector4<T> value1, T value2)
	{
		return value1 / new IntVector4<T>(value2);
	}

	#endregion Division

	#region Negation

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> operator -(IntVector4<T> value)
	{
		return Zero - value;
	}

	#endregion Negation

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> Max(IntVector4<T> value1, IntVector4<T> value2)
	{
		return new IntVector4<T>(
			value1.X > value2.X ? value1.X : value2.X,
			value1.Y > value2.Y ? value1.Y : value2.Y,
			value1.Z > value2.Z ? value1.Z : value2.Z,
			value1.W > value2.W ? value1.W : value2.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> Min(IntVector4<T> value1, IntVector4<T> value2)
	{
		return new IntVector4<T>(
			value1.X < value2.X ? value1.X : value2.X,
			value1.Y < value2.Y ? value1.Y : value2.Y,
			value1.Z < value2.Z ? value1.Z : value2.Z,
			value1.W < value2.W ? value1.W : value2.W);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> Abs(IntVector4<T> value)
	{
		return new IntVector4<T>(T.Abs(value.X), T.Abs(value.Y), T.Abs(value.Z), T.Abs(value.W));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector4<T> Clamp(IntVector4<T> value1, IntVector4<T> min, IntVector4<T> max)
	{
		return Min(Max(value1, min), max);
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		bytesWritten = 0;
		return
			SpanFormattableUtils.TryFormat(X, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(Y, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(Z, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(W, utf8Destination, ref bytesWritten, format, provider);
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		charsWritten = 0;
		return
			SpanFormattableUtils.TryFormat(X, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(Y, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(Z, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(W, destination, ref charsWritten, format, provider);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		FormattableString formattable = FormattableStringFactory.Create(
			"{0}, {1}, {2}, {3}",
			X.ToString(format, formatProvider),
			Y.ToString(format, formatProvider),
			Z.ToString(format, formatProvider),
			W.ToString(format, formatProvider));
		return formattable.ToString(formatProvider);
	}

	public override string ToString()
	{
		return $"{X}, {Y}, {Z}, {W}";
	}
}
