#pragma warning disable SA1201 // Elements should appear in the correct order
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable S1104 // Fields should not have public accessibility
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields

using Detach.Utils;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Detach.Numerics;

public struct IntVector3<T> : IEquatable<IntVector3<T>>, ISpanFormattable, IUtf8SpanFormattable
	where T : IBinaryInteger<T>, IMinMaxValue<T>
{
	public T X;
	public T Y;
	public T Z;

	public IntVector3(T value)
		: this(value, value, value)
	{
	}

	public IntVector3(T x, T y, T z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public static IntVector3<T> Zero => default;

	public static IntVector3<T> One => new(T.One);

	public static IntVector3<T> UnitX => new(T.One, T.Zero, T.Zero);

	public static IntVector3<T> UnitY => new(T.Zero, T.One, T.Zero);

	public static IntVector3<T> UnitZ => new(T.Zero, T.Zero, T.One);

	public T this[T index]
	{
		get
		{
			return index switch
			{
				0 => X,
				1 => Y,
				2 => Z,
				_ => throw new IndexOutOfRangeException($"Invalid index for {nameof(IntVector3<T>)}: {index}"),
			};
		}
		set
		{
			switch (index)
			{
				case 0: X = value; break;
				case 1: Y = value; break;
				case 2: Z = value; break;
				default: throw new IndexOutOfRangeException($"Invalid index for {nameof(IntVector3<T>)}: {index}");
			}
		}
	}

	#region Equality

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(IntVector3<T> left, IntVector3<T> right)
	{
		return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(IntVector3<T> left, IntVector3<T> right)
	{
		return !(left == right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override readonly bool Equals([NotNullWhen(true)] object? obj)
	{
		return obj is IntVector3<T> other && Equals(other);
	}

	public readonly bool Equals(IntVector3<T> other)
	{
		return this == other;
	}

	public override readonly int GetHashCode()
	{
		return HashCode.Combine(X, Y, Z);
	}

	#endregion Equality

	#region Addition

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> operator +(IntVector3<T> left, IntVector3<T> right)
	{
		return new IntVector3<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
	}

	#endregion Addition

	#region Subtraction

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> operator -(IntVector3<T> left, IntVector3<T> right)
	{
		return new IntVector3<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
	}

	#endregion Subtraction

	#region Multiplication

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> operator *(IntVector3<T> left, IntVector3<T> right)
	{
		return new IntVector3<T>(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> operator *(IntVector3<T> left, T right)
	{
		return left * new IntVector3<T>(right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> operator *(T left, IntVector3<T> right)
	{
		return right * left;
	}

	#endregion Multiplication

	#region Division

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> operator /(IntVector3<T> left, IntVector3<T> right)
	{
		return new IntVector3<T>(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> operator /(IntVector3<T> value1, T value2)
	{
		return value1 / new IntVector3<T>(value2);
	}

	#endregion Division

	#region Negation

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> operator -(IntVector3<T> value)
	{
		return Zero - value;
	}

	#endregion Negation

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> Max(IntVector3<T> value1, IntVector3<T> value2)
	{
		return new IntVector3<T>(
			value1.X > value2.X ? value1.X : value2.X,
			value1.Y > value2.Y ? value1.Y : value2.Y,
			value1.Z > value2.Z ? value1.Z : value2.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> Min(IntVector3<T> value1, IntVector3<T> value2)
	{
		return new IntVector3<T>(
			value1.X < value2.X ? value1.X : value2.X,
			value1.Y < value2.Y ? value1.Y : value2.Y,
			value1.Z < value2.Z ? value1.Z : value2.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> Abs(IntVector3<T> value)
	{
		return new IntVector3<T>(T.Abs(value.X), T.Abs(value.Y), T.Abs(value.Z));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector3<T> Clamp(IntVector3<T> value1, IntVector3<T> min, IntVector3<T> max)
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
			SpanFormattableUtils.TryFormat(Z, utf8Destination, ref bytesWritten, format, provider);
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		charsWritten = 0;
		return
			SpanFormattableUtils.TryFormat(X, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(Y, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(Z, destination, ref charsWritten, format, provider);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		FormattableString formattable = FormattableStringFactory.Create(
			"{0}, {1}, {2}",
			X.ToString(format, formatProvider),
			Y.ToString(format, formatProvider),
			Z.ToString(format, formatProvider));
		return formattable.ToString(formatProvider);
	}

	public override string ToString()
	{
		return $"{X}, {Y}, {Z}";
	}
}
