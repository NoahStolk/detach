using Detach.Numerics;
using System.Numerics;

namespace Detach.Utils;

public static class VectorUtils
{
	/// <summary>
	/// Returns the angle in radians of the directional vector.
	/// </summary>
	public static float GetAngleFrom(Vector2 directionalVector)
	{
		return MathF.Atan2(directionalVector.Y, directionalVector.X);
	}

	/// <summary>
	/// Returns the angle in radians between two directional vectors.
	/// </summary>
	public static float GetAngleBetween(Vector2 directionA, Vector2 directionB)
	{
		float dotProduct = directionA.X * directionB.X + directionA.Y * directionB.Y;
		float determinant = directionA.X * directionB.Y - directionA.Y * directionB.X;
		return MathF.Atan2(determinant, dotProduct);
	}

	/// <summary>
	/// Returns the angle in radians between two directional vectors.
	/// </summary>
	public static float GetAngleBetween(Vector3 directionA, Vector3 directionB)
	{
		float dotProduct = Vector3.Dot(directionA, directionB);
		float determinant = Vector3.Cross(directionA, directionB).Length();
		return MathF.Atan2(determinant, dotProduct);
	}

	public static Vector2 RotateVector(Vector2 vector2, float radians)
	{
		return new Vector2(
			vector2.X * MathF.Cos(radians) - vector2.Y * MathF.Sin(radians),
			vector2.X * MathF.Sin(radians) + vector2.Y * MathF.Cos(radians));
	}

	public static Vector2 Clamp(Vector2 vector, float min, float max)
	{
		return new Vector2(Math.Clamp(vector.X, min, max), Math.Clamp(vector.Y, min, max));
	}

	public static Vector3 Clamp(Vector3 vector, float min, float max)
	{
		return new Vector3(Math.Clamp(vector.X, min, max), Math.Clamp(vector.Y, min, max), Math.Clamp(vector.Z, min, max));
	}

	public static Vector4 Clamp(Vector4 vector, float min, float max)
	{
		return new Vector4(Math.Clamp(vector.X, min, max), Math.Clamp(vector.Y, min, max), Math.Clamp(vector.Z, min, max), Math.Clamp(vector.W, min, max));
	}

	public static Vector3 Reflect(Vector3 direction, Vector3 normal)
	{
		float dn = 2 * Vector3.Dot(direction, normal);
		return direction - normal * dn;
	}

	public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
	{
		float sqrMagnitude = vector.X * vector.X + vector.Y * vector.Y;
		if (sqrMagnitude <= maxLength * maxLength)
			return vector;

		float magnitude = MathF.Sqrt(sqrMagnitude);
		float normalizedX = vector.X / magnitude;
		float normalizedY = vector.Y / magnitude;
		return new Vector2(normalizedX * maxLength, normalizedY * maxLength);
	}

	public static Vector3 ClampMagnitude(Vector3 vector, float maxLength)
	{
		float sqrMagnitude = vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z;
		if (sqrMagnitude <= maxLength * maxLength)
			return vector;

		float magnitude = MathF.Sqrt(sqrMagnitude);
		float normalizedX = vector.X / magnitude;
		float normalizedY = vector.Y / magnitude;
		float normalizedZ = vector.Z / magnitude;
		return new Vector3(normalizedX * maxLength, normalizedY * maxLength, normalizedZ * maxLength);
	}

	public static Vector2 ClampMagnitudeMin(Vector2 vector, float minLength)
	{
		float sqrMagnitude = vector.X * vector.X + vector.Y * vector.Y;
		if (sqrMagnitude >= minLength * minLength)
			return vector;

		float magnitude = MathF.Sqrt(sqrMagnitude);
		float normalizedX = vector.X / magnitude;
		float normalizedY = vector.Y / magnitude;
		return new Vector2(normalizedX * minLength, normalizedY * minLength);
	}

	public static Vector3 ClampMagnitudeMin(Vector3 vector, float minLength)
	{
		float sqrMagnitude = vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z;
		if (sqrMagnitude >= minLength * minLength)
			return vector;

		float magnitude = MathF.Sqrt(sqrMagnitude);
		float normalizedX = vector.X / magnitude;
		float normalizedY = vector.Y / magnitude;
		float normalizedZ = vector.Z / magnitude;
		return new Vector3(normalizedX * minLength, normalizedY * minLength, normalizedZ * minLength);
	}

	/// <summary>
	/// Transforms the vector by the specified 3x3 matrix.
	/// </summary>
	public static Vector3 Transform(Vector3 vector, Matrix3 matrix)
	{
		return new Vector3(
			vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31,
			vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32,
			vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33);
	}
}
