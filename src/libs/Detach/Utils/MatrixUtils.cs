using System.Numerics;
using System.Runtime.CompilerServices;

namespace Detach.Utils;

public static class MatrixUtils
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix4x4 CreateScale2d(float scaleX, float scaleY, float scaleZ = 1)
	{
		return Matrix4x4.CreateScale(scaleX, scaleY, scaleZ);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix4x4 CreateScale2d(Vector2 scale, float scaleZ = 1)
	{
		return Matrix4x4.CreateScale(scale.X, scale.Y, scaleZ);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix4x4 CreateTranslation2d(float translationX, float translationY, float translationZ = 0)
	{
		return Matrix4x4.CreateTranslation(translationX, translationY, translationZ);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix4x4 CreateTranslation2d(Vector2 translation, float translationZ = 0)
	{
		return Matrix4x4.CreateTranslation(translation.X, translation.Y, translationZ);
	}

	public static Matrix4x4 CreateRotationMatrixAroundPoint(Vector3 axis, float angle, Vector3 point)
	{
		axis = Vector3.Normalize(axis);

		Matrix4x4 translationToOrigin = Matrix4x4.CreateTranslation(-point);
		Matrix4x4 translationBack = Matrix4x4.CreateTranslation(point);

		Matrix4x4 rotation = Matrix4x4.CreateFromAxisAngle(axis, angle);

		// Combine the matrices: translate to origin, rotate, then translate back.
		return translationToOrigin * rotation * translationBack;
	}

	public static Matrix4x4 CreateRotationMatrixFromDirection(Vector3 direction)
	{
		direction = Vector3.Normalize(direction);

		// Use the z-axis as a reference direction.
		Vector3 referenceDirection = Vector3.UnitZ;

		Vector3 rotationAxis = Vector3.Cross(referenceDirection, direction);
		float axisLength = rotationAxis.Length();

		// If the direction is parallel to the reference direction.
		if (axisLength < 1e-6)
		{
			// If the direction is the same as the reference direction.
			if (Vector3.Dot(referenceDirection, direction) > 0)
				return Matrix4x4.Identity;

			// If the direction is opposite to the reference direction.
			return Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, MathF.PI);
		}

		rotationAxis /= axisLength;

		// Compute the angle between the reference direction and the given direction.
		float angle = MathF.Acos(Vector3.Dot(referenceDirection, direction));

		return Matrix4x4.CreateFromAxisAngle(rotationAxis, angle);
	}
}
