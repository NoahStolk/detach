namespace Demos.Collisions3D.Extensions;

internal static class FloatExtensions
{
	public static bool IsZero(this float value)
	{
		return value is < float.Epsilon and > -float.Epsilon;
	}
}
