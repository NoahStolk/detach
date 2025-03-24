using System.Globalization;
using System.Numerics;

namespace Detach.Tests.Unit;

public static class StringUtils
{
	public static Vector2 ParseVector2(string value)
	{
		string[] parts = value.Split(',');
		return new Vector2(float.Parse(parts[0], CultureInfo.InvariantCulture), float.Parse(parts[1], CultureInfo.InvariantCulture));
	}

	public static Vector3 ParseVector3(string value)
	{
		string[] parts = value.Split(',');
		return new Vector3(float.Parse(parts[0], CultureInfo.InvariantCulture), float.Parse(parts[1], CultureInfo.InvariantCulture), float.Parse(parts[2], CultureInfo.InvariantCulture));
	}
}
