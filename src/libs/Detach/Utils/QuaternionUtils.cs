using System.Numerics;

namespace Detach.Utils;

public static class QuaternionUtils
{
	public static Quaternion CreateFromRotationBetween(Vector3 directionA, Vector3 directionB)
	{
		float dot = Vector3.Dot(directionA, directionB);
		if (dot > 0.999999f)
			return Quaternion.Identity;

		Vector3 cross;
		if (dot < -0.999999f)
		{
			cross = Vector3.Cross(Vector3.UnitX, directionA);
			if (cross.Length() < 0.000001f)
				cross = Vector3.Cross(Vector3.UnitY, directionA);
			cross = Vector3.Normalize(cross);

			return Quaternion.CreateFromAxisAngle(cross, MathF.PI);
		}

		cross = Vector3.Cross(directionA, directionB);
		Quaternion q = new(cross, 1 + dot);
		return Quaternion.Normalize(q);
	}
}
