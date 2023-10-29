using Detach.Utils;
using System.Numerics;

namespace Detach.Extensions;

public static class QuaternionExtensions
{
	public static void Randomize(this ref Quaternion quaternion, float randomizeAmount, Random? random = null)
	{
		random ??= Random.Shared;
		quaternion.X += random.RandomFloat(-randomizeAmount, randomizeAmount);
		quaternion.Y += random.RandomFloat(-randomizeAmount, randomizeAmount);
		quaternion.Z += random.RandomFloat(-randomizeAmount, randomizeAmount);
		quaternion.W += random.RandomFloat(-randomizeAmount, randomizeAmount);
	}

	public static bool ContainsNaN(this Quaternion quaternion)
		=> !MathUtils.IsFloatReal(quaternion.X) || !MathUtils.IsFloatReal(quaternion.Y) || !MathUtils.IsFloatReal(quaternion.Z) || !MathUtils.IsFloatReal(quaternion.W);
}
