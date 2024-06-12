using Detach.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Detach.Numerics;

public readonly record struct Rgba(byte R, byte G, byte B, byte A = byte.MaxValue)
{
	public static Rgba Invisible => default;

	public static Rgba White => new(byte.MaxValue, byte.MaxValue, byte.MaxValue);
	public static Rgba Black => new(byte.MinValue, byte.MinValue, byte.MinValue);

	public static Rgba HalfTransparentWhite => new(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue / 2);
	public static Rgba HalfTransparentBlack => new(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue / 2);

	public static Rgba Red => new(byte.MaxValue, byte.MinValue, byte.MinValue);
	public static Rgba Green => new(byte.MinValue, byte.MaxValue, byte.MinValue);
	public static Rgba Blue => new(byte.MinValue, byte.MinValue, byte.MaxValue);

	public static Rgba Yellow => new(byte.MaxValue, byte.MaxValue, byte.MinValue);
	public static Rgba Aqua => new(byte.MinValue, byte.MaxValue, byte.MaxValue);
	public static Rgba Purple => new(byte.MaxValue, byte.MinValue, byte.MaxValue);

	public static Rgba Orange => new(byte.MaxValue, byte.MaxValue / 2, byte.MinValue);

	public static implicit operator Vector3(Rgba rgba)
	{
		return new Vector3(rgba.R / (float)byte.MaxValue, rgba.G / (float)byte.MaxValue, rgba.B / (float)byte.MaxValue);
	}

	public static implicit operator Vector4(Rgba rgba)
	{
		return new Vector4(rgba.R / (float)byte.MaxValue, rgba.G / (float)byte.MaxValue, rgba.B / (float)byte.MaxValue, rgba.A / (float)byte.MaxValue);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rgba operator +(Rgba left, Rgba right)
	{
		return new Rgba((byte)(left.R + right.R), (byte)(left.G + right.G), (byte)(left.B + right.B), (byte)(left.A + right.A));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rgba operator -(Rgba left, Rgba right)
	{
		return new Rgba((byte)(left.R - right.R), (byte)(left.G - right.G), (byte)(left.B - right.B), (byte)(left.A - right.A));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rgba operator *(Rgba left, Rgba right)
	{
		return new Rgba((byte)(left.R * right.R), (byte)(left.G * right.G), (byte)(left.B * right.B), (byte)(left.A * right.A));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rgba operator /(Rgba left, Rgba right)
	{
		return new Rgba((byte)(left.R / right.R), (byte)(left.G / right.G), (byte)(left.B / right.B), (byte)(left.A / right.A));
	}

	public int GetPerceivedBrightness()
	{
		return (int)Math.Sqrt(R * R * 0.299 + G * G * 0.587 + B * B * 0.114);
	}

	public Rgba Intensify(byte component)
	{
		byte r = (byte)Math.Min(byte.MaxValue, R + component);
		byte g = (byte)Math.Min(byte.MaxValue, G + component);
		byte b = (byte)Math.Min(byte.MaxValue, B + component);
		byte a = (byte)Math.Min(byte.MaxValue, A + component);
		return new Rgba(r, g, b, a);
	}

	public Rgba Desaturate(float f)
	{
		float r = R / (float)byte.MaxValue;
		float g = G / (float)byte.MaxValue;
		float b = B / (float)byte.MaxValue;

		float l = 0.3f * r + 0.6f * g + 0.1f * b;
		float newR = r + f * (l - r);
		float newG = g + f * (l - g);
		float newB = b + f * (l - b);

		return new Rgba((byte)(newR * byte.MaxValue), (byte)(newG * byte.MaxValue), (byte)(newB * byte.MaxValue), A);
	}

	public Rgba Darken(float amount)
	{
		return new Rgba((byte)(R * (1 - amount)), (byte)(G * (1 - amount)), (byte)(B * (1 - amount)), A);
	}

	public int GetHue()
	{
		byte min = Math.Min(Math.Min(R, G), B);
		byte max = Math.Max(Math.Max(R, G), B);

		if (min == max)
			return 0;

		float hue;
		if (max == R)
			hue = (G - B) / (float)(max - min);
		else if (max == G)
			hue = 2f + (B - R) / (float)(max - min);
		else
			hue = 4f + (R - G) / (float)(max - min);

		hue *= 60;
		if (hue < 0)
			hue += 360;

		return (int)Math.Round(hue);
	}

	public static Rgba Lerp(Rgba value1, Rgba value2, float amount)
	{
		float r = MathUtils.Lerp(value1.R, value2.R, amount);
		float g = MathUtils.Lerp(value1.G, value2.G, amount);
		float b = MathUtils.Lerp(value1.B, value2.B, amount);
		float a = MathUtils.Lerp(value1.A, value2.A, amount);
		return new Rgba((byte)r, (byte)g, (byte)b, (byte)a);
	}

	public static Rgba Invert(Rgba rgba)
	{
		return new Rgba((byte)(byte.MaxValue - rgba.R), (byte)(byte.MaxValue - rgba.G), (byte)(byte.MaxValue - rgba.B), rgba.A);
	}

	public static Rgba Gray(float value)
	{
		if (value is < 0 or > 1)
			throw new ArgumentOutOfRangeException(nameof(value));

		byte component = (byte)(value * byte.MaxValue);
		return new Rgba(component, component, component);
	}

	public static Rgba FromHsv(float hue, float saturation, float value)
	{
		saturation = Math.Clamp(saturation, 0, 1);
		value = Math.Clamp(value, 0, 1);

		int hi = (int)MathF.Floor(hue / 60) % 6;
		float f = hue / 60 - MathF.Floor(hue / 60);

		value *= byte.MaxValue;
		byte v = (byte)value;
		byte p = (byte)(value * (1 - saturation));
		byte q = (byte)(value * (1 - f * saturation));
		byte t = (byte)(value * (1 - (1 - f) * saturation));

		return hi switch
		{
			0 => new Rgba(v, t, p),
			1 => new Rgba(q, v, p),
			2 => new Rgba(p, v, t),
			3 => new Rgba(p, q, v),
			4 => new Rgba(t, p, v),
			_ => new Rgba(v, p, q),
		};
	}

	public static Rgba FromVector3(Vector3 vector)
	{
		return new Rgba((byte)(vector.X * byte.MaxValue), (byte)(vector.Y * byte.MaxValue), (byte)(vector.Z * byte.MaxValue));
	}

	public static Rgba FromVector4(Vector4 vector)
	{
		return new Rgba((byte)(vector.X * byte.MaxValue), (byte)(vector.Y * byte.MaxValue), (byte)(vector.Z * byte.MaxValue), (byte)(vector.W * byte.MaxValue));
	}

	public int ToRgbaInt()
	{
		return (R << 24) + (G << 16) + (B << 8) + A;
	}

	public int ToArgbInt()
	{
		return (A << 24) + (R << 16) + (G << 8) + B;
	}

	public static Rgba FromRgbaInt(int rgba)
	{
		return new Rgba((byte)(rgba >> 24), (byte)(rgba >> 16), (byte)(rgba >> 8), (byte)rgba);
	}

	public static Rgba FromArgbInt(int argb)
	{
		return new Rgba((byte)(argb >> 16), (byte)(argb >> 8), (byte)argb, (byte)(argb >> 24));
	}
}
