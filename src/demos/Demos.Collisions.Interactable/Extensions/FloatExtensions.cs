namespace Demos.Collisions.Interactable.Extensions;

internal static class FloatExtensions
{
	public static bool IsZero(this float value)
	{
		return value is < float.Epsilon and > -float.Epsilon;
	}
}
