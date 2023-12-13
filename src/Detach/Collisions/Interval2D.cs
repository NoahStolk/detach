namespace Detach.Collisions;

public record struct Interval2D
{
	public float Min;
	public float Max;

	public Interval2D(float min, float max)
	{
		Min = min;
		Max = max;
	}
}
