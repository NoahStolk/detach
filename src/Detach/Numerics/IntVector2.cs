#pragma warning disable SA1201 // Elements should appear in the correct order
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable S1104 // Fields should not have public accessibility
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Detach.Numerics;

public struct IntVector2<T> : IEquatable<IntVector2<T>>, IFormattable
	where T : IBinaryInteger<T>, IMinMaxValue<T>
{
	public T X;
	public T Y;

	public IntVector2(T value)
		: this(value, value)
	{
	}

	public IntVector2(T x, T y)
	{
		X = x;
		Y = y;
	}

	public static IntVector2<T> Zero => default;

	public static IntVector2<T> One => new(T.One);

	public static IntVector2<T> UnitX => new(T.One, T.Zero);

	public static IntVector2<T> UnitY => new(T.Zero, T.One);

	public T this[T index]
	{
		get
		{
			return index switch
			{
				0 => X,
				1 => Y,
				_ => throw new IndexOutOfRangeException($"Invalid index for {nameof(IntVector2<T>)}: {index}"),
			};
		}
		set
		{
			switch (index)
			{
				case 0: X = value; break;
				case 1: Y = value; break;
				default: throw new IndexOutOfRangeException($"Invalid index for {nameof(IntVector2<T>)}: {index}");
			}
		}
	}

	#region Equality

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(IntVector2<T> left, IntVector2<T> right)
	{
		return left.X == right.X && left.Y == right.Y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(IntVector2<T> left, IntVector2<T> right)
	{
		return !(left == right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override readonly bool Equals([NotNullWhen(true)] object? obj)
	{
		return obj is IntVector2<T> other && Equals(other);
	}

	public readonly bool Equals(IntVector2<T> other)
	{
		return this == other;
	}

	public override readonly int GetHashCode()
	{
		return HashCode.Combine(X, Y);
	}

	#endregion Equality

	#region Addition

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> operator +(IntVector2<T> left, IntVector2<T> right)
	{
		return new IntVector2<T>(left.X + right.X, left.Y + right.Y);
	}

	#endregion Addition

	#region Subtraction

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> operator -(IntVector2<T> left, IntVector2<T> right)
	{
		return new IntVector2<T>(left.X - right.X, left.Y - right.Y);
	}

	#endregion Subtraction

	#region Multiplication

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> operator *(IntVector2<T> left, IntVector2<T> right)
	{
		return new IntVector2<T>(left.X * right.X, left.Y * right.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> operator *(IntVector2<T> left, T right)
	{
		return left * new IntVector2<T>(right);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> operator *(T left, IntVector2<T> right)
	{
		return right * left;
	}

	#endregion Multiplication

	#region Division

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> operator /(IntVector2<T> left, IntVector2<T> right)
	{
		return new IntVector2<T>(left.X / right.X, left.Y / right.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> operator /(IntVector2<T> value1, T value2)
	{
		return value1 / new IntVector2<T>(value2);
	}

	#endregion Division

	#region Negation

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> operator -(IntVector2<T> value)
	{
		return Zero - value;
	}

	#endregion Negation

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> Max(IntVector2<T> value1, IntVector2<T> value2)
	{
		return new IntVector2<T>(
			value1.X > value2.X ? value1.X : value2.X,
			value1.Y > value2.Y ? value1.Y : value2.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> Min(IntVector2<T> value1, IntVector2<T> value2)
	{
		return new IntVector2<T>(
			value1.X < value2.X ? value1.X : value2.X,
			value1.Y < value2.Y ? value1.Y : value2.Y);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> Abs(IntVector2<T> value)
	{
		return new IntVector2<T>(T.Abs(value.X), T.Abs(value.Y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntVector2<T> Clamp(IntVector2<T> value1, IntVector2<T> min, IntVector2<T> max)
	{
		return Min(Max(value1, min), max);
	}

	public override readonly string ToString()
	{
		return ToString("G", CultureInfo.CurrentCulture);
	}

	public readonly string ToString(string? format)
	{
		return ToString(format, CultureInfo.CurrentCulture);
	}

	public readonly string ToString(string? format, IFormatProvider? formatProvider)
	{
		StringBuilder sb = new();
		string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
		sb.Append('<');
		sb.Append(X.ToString(format, formatProvider));
		sb.Append(separator);
		sb.Append(' ');
		sb.Append(Y.ToString(format, formatProvider));
		sb.Append('>');
		return sb.ToString();
	}
}
