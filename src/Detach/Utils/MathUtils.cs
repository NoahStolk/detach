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
	public static Vector3 ToDegrees(Vector3 radians)
	{
		return radians * 180 / MathF.PI;
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
	public static bool IsFloatReal(float value)
	{
		return !float.IsNaN(value) && !float.IsInfinity(value);
	}
}
