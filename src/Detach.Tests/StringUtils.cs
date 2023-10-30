using System.Numerics;

namespace Detach.Tests;

public static class StringUtils
{
	public static Vector2 ParseVector2(string value)
	{
		string[] parts = value.Split(',');
		return new(float.Parse(parts[0]), float.Parse(parts[1]));
	}

	public static Vector3 ParseVector3(string value)
	{
		string[] parts = value.Split(',');
		return new(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
	}
}
