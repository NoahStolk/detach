using Detach.Utils;
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

	public static bool IsReal(this Vector2 vector)
	{
		return MathUtils.IsFloatReal(vector.X) && MathUtils.IsFloatReal(vector.Y);
	}

	public static bool IsReal(this Vector3 vector)
	{
		return MathUtils.IsFloatReal(vector.X) && MathUtils.IsFloatReal(vector.Y) && MathUtils.IsFloatReal(vector.Z);
	}

	public static bool IsReal(this Vector4 vector)
	{
		return MathUtils.IsFloatReal(vector.X) && MathUtils.IsFloatReal(vector.Y) && MathUtils.IsFloatReal(vector.Z) && MathUtils.IsFloatReal(vector.W);
	}
}
