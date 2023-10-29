using System.Numerics;
using System.Runtime.CompilerServices;

namespace Detach.Utils;

public static class MatrixUtils
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix4x4 CreateScale2d(float scaleX, float scaleY, float scaleZ = 1) => Matrix4x4.CreateScale(scaleX, scaleY, scaleZ);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix4x4 CreateScale2d(Vector2 scale, float scaleZ = 1) => Matrix4x4.CreateScale(scale.X, scale.Y, scaleZ);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix4x4 CreateTranslation2d(float translationX, float translationY, float translationZ = 0) => Matrix4x4.CreateTranslation(translationX, translationY, translationZ);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix4x4 CreateTranslation2d(Vector2 translation, float translationZ = 0) => Matrix4x4.CreateTranslation(translation.X, translation.Y, translationZ);
}
