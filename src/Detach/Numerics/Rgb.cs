using Detach.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Detach.Numerics;

public readonly record struct Rgb(byte R, byte G, byte B)
{
	public Rgb(Rgba rgba)
		: this(rgba.R, rgba.G, rgba.B)
	{
	}

	public static Rgb White => new(byte.MaxValue, byte.MaxValue, byte.MaxValue);
	public static Rgb Black => new(byte.MinValue, byte.MinValue, byte.MinValue);

	public static Rgb Red => new(byte.MaxValue, byte.MinValue, byte.MinValue);
	public static Rgb Green => new(byte.MinValue, byte.MaxValue, byte.MinValue);
	public static Rgb Blue => new(byte.MinValue, byte.MinValue, byte.MaxValue);

	public static Rgb Yellow => new(byte.MaxValue, byte.MaxValue, byte.MinValue);
	public static Rgb Aqua => new(byte.MinValue, byte.MaxValue, byte.MaxValue);
	public static Rgb Purple => new(byte.MaxValue, byte.MinValue, byte.MaxValue);

	public static Rgb Orange => new(byte.MaxValue, byte.MaxValue / 2, byte.MinValue);

	public static implicit operator Vector3(Rgb rgb)
	{
		return new Vector3(rgb.R / (float)byte.MaxValue, rgb.G / (float)byte.MaxValue, rgb.B / (float)byte.MaxValue);
	}

	public static implicit operator Vector4(Rgb rgb)
	{
		return new Vector4(rgb.R / (float)byte.MaxValue, rgb.G / (float)byte.MaxValue, rgb.B / (float)byte.MaxValue, 1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rgb operator +(Rgb left, Rgb right)
	{
		return new Rgb((byte)(left.R + right.R), (byte)(left.G + right.G), (byte)(left.B + right.B));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rgb operator -(Rgb left, Rgb right)
	{
		return new Rgb((byte)(left.R - right.R), (byte)(left.G - right.G), (byte)(left.B - right.B));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rgb operator *(Rgb left, Rgb right)
	{
		return new Rgb((byte)(left.R * right.R), (byte)(left.G * right.G), (byte)(left.B * right.B));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Rgb operator /(Rgb left, Rgb right)
	{
		return new Rgb((byte)(left.R / right.R), (byte)(left.G / right.G), (byte)(left.B / right.B));
	}

	public int GetPerceivedBrightness()
	{
		return Colors.GetPerceivedBrightness(R, G, B);
	}

	public Rgb Intensify(byte component)
	{
		byte r = (byte)Math.Min(byte.MaxValue, R + component);
		byte g = (byte)Math.Min(byte.MaxValue, G + component);
		byte b = (byte)Math.Min(byte.MaxValue, B + component);
		return new Rgb(r, g, b);
	}

	public Rgb Desaturate(float f)
	{
		float r = R / (float)byte.MaxValue;
		float g = G / (float)byte.MaxValue;
		float b = B / (float)byte.MaxValue;

		float l = 0.3f * r + 0.6f * g + 0.1f * b;
		float newR = r + f * (l - r);
		float newG = g + f * (l - g);
		float newB = b + f * (l - b);

		return new Rgb((byte)(newR * byte.MaxValue), (byte)(newG * byte.MaxValue), (byte)(newB * byte.MaxValue));
	}

	public Rgb Darken(float amount)
	{
		return new Rgb((byte)(R * (1 - amount)), (byte)(G * (1 - amount)), (byte)(B * (1 - amount)));
	}

	public int GetHue()
	{
		return Colors.GetHue(R, G, B);
	}

	public static Rgb Lerp(Rgb value1, Rgb value2, float amount)
	{
		float r = MathUtils.Lerp(value1.R, value2.R, amount);
		float g = MathUtils.Lerp(value1.G, value2.G, amount);
		float b = MathUtils.Lerp(value1.B, value2.B, amount);
		return new Rgb((byte)r, (byte)g, (byte)b);
	}

	public static Rgb Invert(Rgb rgb)
	{
		return new Rgb((byte)(byte.MaxValue - rgb.R), (byte)(byte.MaxValue - rgb.G), (byte)(byte.MaxValue - rgb.B));
	}

	public static Rgb Gray(float value)
	{
		if (value is < 0 or > 1)
			throw new ArgumentOutOfRangeException(nameof(value));

		byte component = (byte)(value * byte.MaxValue);
		return new Rgb(component, component, component);
	}

	public static Rgb FromHsv(float hue, float saturation, float value)
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
			0 => new Rgb(v, t, p),
			1 => new Rgb(q, v, p),
			2 => new Rgb(p, v, t),
			3 => new Rgb(p, q, v),
			4 => new Rgb(t, p, v),
			_ => new Rgb(v, p, q),
		};
	}

	public static Rgb FromVector3(Vector3 vector)
	{
		return new Rgb((byte)(vector.X * byte.MaxValue), (byte)(vector.Y * byte.MaxValue), (byte)(vector.Z * byte.MaxValue));
	}

	public static Rgb FromVector4(Vector4 vector)
	{
		return new Rgb((byte)(vector.X * byte.MaxValue), (byte)(vector.Y * byte.MaxValue), (byte)(vector.Z * byte.MaxValue));
	}

	public int ToRgbInt()
	{
		return (R << 24) + (G << 16) + (B << 8);
	}

	public static Rgb FromRgbInt(int rgb)
	{
		return new Rgb((byte)(rgb >> 24), (byte)(rgb >> 16), (byte)(rgb >> 8));
	}
}
