namespace Detach.Numerics;

internal static class Colors
{
	public static int GetPerceivedBrightness(byte r, byte g, byte b)
	{
		return (int)Math.Sqrt(r * r * 0.299 + g * g * 0.587 + b * b * 0.114);
	}

	public static int GetHue(byte r, byte g, byte b)
	{
		byte min = Math.Min(Math.Min(r, g), b);
		byte max = Math.Max(Math.Max(r, g), b);

		if (min == max)
			return 0;

		float hue;
		if (max == r)
			hue = (g - b) / (float)(max - min);
		else if (max == g)
			hue = 2f + (b - r) / (float)(max - min);
		else
			hue = 4f + (r - g) / (float)(max - min);

		hue *= 60;
		if (hue < 0)
			hue += 360;

		return (int)Math.Round(hue);
	}
}
