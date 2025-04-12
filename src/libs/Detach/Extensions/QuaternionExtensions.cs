using System.Numerics;

namespace Detach.Extensions;

public static class QuaternionExtensions
{
	public static void Randomize(this ref Quaternion quaternion, float randomizeAmount)
	{
		Randomize(ref quaternion, randomizeAmount, Random.Shared);
	}

	public static void Randomize(this ref Quaternion quaternion, float randomizeAmount, Random random)
	{
		quaternion.X += random.RandomFloat(-randomizeAmount, randomizeAmount);
		quaternion.Y += random.RandomFloat(-randomizeAmount, randomizeAmount);
		quaternion.Z += random.RandomFloat(-randomizeAmount, randomizeAmount);
		quaternion.W += random.RandomFloat(-randomizeAmount, randomizeAmount);
	}

	public static bool IsFinite(this Quaternion quaternion)
	{
		return float.IsFinite(quaternion.X) && float.IsFinite(quaternion.Y) && float.IsFinite(quaternion.Z) && float.IsFinite(quaternion.W);
	}
}
