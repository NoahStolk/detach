using System.Numerics;

namespace Detach.Extensions;

public static partial class VectorExtensions
{
	public static Vector2 Round(this Vector2 vector, int digits)
	{
		return new Vector2(MathF.Round(vector.X, digits), MathF.Round(vector.Y, digits));
	}

	public static Vector3 Round(this Vector3 vector, int digits)
	{
		return new Vector3(MathF.Round(vector.X, digits), MathF.Round(vector.Y, digits), MathF.Round(vector.Z, digits));
	}

	public static Vector4 Round(this Vector4 vector, int digits)
	{
		return new Vector4(MathF.Round(vector.X, digits), MathF.Round(vector.Y, digits), MathF.Round(vector.Z, digits), MathF.Round(vector.W, digits));
	}

	public static bool IsFinite(this Vector2 vector)
	{
		return float.IsFinite(vector.X) && float.IsFinite(vector.Y);
	}

	public static bool IsFinite(this Vector3 vector)
	{
		return float.IsFinite(vector.X) && float.IsFinite(vector.Y) && float.IsFinite(vector.Z);
	}

	public static bool IsFinite(this Vector4 vector)
	{
		return float.IsFinite(vector.X) && float.IsFinite(vector.Y) && float.IsFinite(vector.Z) && float.IsFinite(vector.W);
	}
}
