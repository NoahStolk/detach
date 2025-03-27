using System.Numerics;
using System.Runtime.CompilerServices;

namespace Detach.Utils;

public static class MathUtils
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix4x4 CreateRotationMatrixFromEulerAngles(Vector3 eulerAngles)
	{
		return Matrix4x4.CreateFromYawPitchRoll(eulerAngles.Y, eulerAngles.X, eulerAngles.Z);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Lerp(float value1, float value2, float amount)
	{
		return value1 + (value2 - value1) * amount;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float LerpClamped(float value1, float value2, float amount)
	{
		return Lerp(value1, value2, Math.Clamp(amount, 0, 1));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsFloatReal(float value)
	{
		return !float.IsNaN(value) && !float.IsInfinity(value);
	}

	/// <summary>
	/// Returns the fractional part of the value. This is calculated as <c>value - MathF.Truncate(value)</c>.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Fraction(float value)
	{
		return value - MathF.Truncate(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAlmostZero(float value, float tolerance = 0.0001f)
	{
		return value > -tolerance && value < tolerance;
	}

	public static float Bezier(float t)
	{
		return t * t * (3.0f - 2.0f * t);
	}
}
