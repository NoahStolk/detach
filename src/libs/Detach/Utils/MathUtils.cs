using System.Numerics;
using System.Runtime.CompilerServices;

namespace Detach.Utils;

public static class MathUtils
{
	private const float _toRad = MathF.PI / 180;
	private const float _toDeg = 180 / MathF.PI;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float ToDegrees(float radians)
	{
		return radians * _toDeg;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 ToDegrees(Vector3 radians)
	{
		return radians * 180 / MathF.PI;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float ToRadians(float degrees)
	{
		return degrees * _toRad;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 ToRadians(Vector3 degrees)
	{
		return degrees * MathF.PI / 180;
	}

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
