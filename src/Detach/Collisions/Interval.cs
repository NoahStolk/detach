namespace Detach.Collisions;

public record struct Interval
{
	// TODO: Use get-only properties.
	public float Min;
	public float Max;

	public Interval(float min, float max)
	{
		Min = min;
		Max = max;
	}
}
