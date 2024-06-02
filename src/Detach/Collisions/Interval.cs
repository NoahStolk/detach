namespace Detach.Collisions;

public record struct Interval
{
	public float Min;
	public float Max;

	public Interval(float min, float max)
	{
		Min = min;
		Max = max;
	}
}
