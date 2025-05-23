using Detach.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Detach.Numerics;

public readonly struct Spinor : IEquatable<Spinor>, ISpanFormattable, IUtf8SpanFormattable
{
	private readonly float _real;
	private readonly float _complex;

	public Spinor(float real, float complex)
	{
		_real = real;
		_complex = complex;
	}

	public Spinor(float angle)
	{
		_real = MathF.Cos(angle / 2);
		_complex = MathF.Sin(angle / 2);
	}

	public Spinor(Vector2 directionalVector)
		: this(MathF.Atan2(directionalVector.Y, directionalVector.X))
	{
	}

	public static Spinor Identity => new(0);

	public static Spinor operator +(Spinor left, Spinor right)
	{
		return new Spinor(left._real + right._real, left._complex + right._complex);
	}

	public static Spinor operator *(Spinor left, Spinor right)
	{
		return new Spinor(left._real * right._real - left._complex * right._complex, left._real * right._complex + left._complex * right._real);
	}

	public static bool operator ==(Spinor left, Spinor right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Spinor left, Spinor right)
	{
		return !(left == right);
	}

	public Spinor GetScale(float scale)
	{
		return new Spinor(_real * scale, _complex * scale);
	}

	public Spinor GetInvert()
	{
		Spinor s = new(_real, -_complex);
		return s.GetScale(s.GetLengthSquared());
	}

	public float GetLength()
	{
		return MathF.Sqrt(GetLengthSquared());
	}

	public float GetLengthSquared()
	{
		return _real * _real + _complex * _complex;
	}

	public Spinor GetNormalized()
	{
		float length = GetLength();
		return new Spinor(_real / length, _complex / length);
	}

	public float GetAngle()
	{
		return MathF.Atan2(_complex, _real) * 2;
	}

	public Vector2 GetDirectionalVector()
	{
		float angle = GetAngle();
		return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
	}

	public static Spinor Lerp(Spinor from, Spinor to, float amount)
	{
		return (from.GetScale(1 - amount) + to.GetScale(amount)).GetNormalized();
	}

	public static Spinor Slerp(Spinor from, Spinor to, float amount)
	{
		float toReal, toComplex, scale0, scale1;

		float cosOmega = from._real * to._real + from._complex * to._complex;
		if (cosOmega < 0)
		{
			cosOmega = -cosOmega;
			toComplex = -to._complex;
			toReal = -to._real;
		}
		else
		{
			toComplex = to._complex;
			toReal = to._real;
		}

		if (1 - cosOmega > 0.001)
		{
			float omega = MathF.Acos(cosOmega);
			float sinOmega = MathF.Sin(omega);
			scale0 = MathF.Sin((1 - amount) * omega) / sinOmega;
			scale1 = MathF.Sin(amount * omega) / sinOmega;
		}
		else
		{
			scale0 = 1 - amount;
			scale1 = amount;
		}

		return new Spinor(scale0 * from._real + scale1 * toReal, scale0 * from._complex + scale1 * toComplex);
	}

	public static float Slerp(float fromAngle, float toAngle, float amount)
	{
		Spinor from = new(MathF.Cos(fromAngle / 2), MathF.Sin(fromAngle / 2));
		Spinor to = new(MathF.Cos(toAngle / 2), MathF.Sin(toAngle / 2));
		return Slerp(from, to, amount).GetAngle();
	}

	public static Spinor Add(Spinor left, Spinor right)
	{
		return left + right;
	}

	public static Spinor Multiply(Spinor left, Spinor right)
	{
		return left * right;
	}

	public override bool Equals(object? obj)
	{
		return obj is Spinor other && Equals(other);
	}

	public bool Equals(Spinor other)
	{
		// ReSharper disable CompareOfFloatsByEqualityOperator
		return _real == other._real && _complex == other._complex;

		// ReSharper restore CompareOfFloatsByEqualityOperator
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_real, _complex);
	}

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		bytesWritten = 0;
		return
			SpanFormattableUtils.TryFormat(_real, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(_complex, utf8Destination, ref bytesWritten, format, provider);
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		charsWritten = 0;
		return
			SpanFormattableUtils.TryFormat(_real, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(_complex, destination, ref charsWritten, format, provider);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		FormattableString formattable = FormattableStringFactory.Create(
			"{0}, {1}",
			_real.ToString(format, formatProvider),
			_complex.ToString(format, formatProvider));
		return formattable.ToString(formatProvider);
	}

	public override string ToString()
	{
		return $"{_real}, {_complex}";
	}
}
