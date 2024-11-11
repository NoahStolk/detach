using System.Numerics;

namespace Detach.Metrics;

public sealed class SimpleCounter<T>
	where T : struct, ISubtractionOperators<T, T, T>
{
	public T ValueCurrent { get; private set; }
	public T ValueDifference { get; private set; }
	public T ValuePrevious { get; private set; }

	public void Update(T valueCurrent)
	{
		T valuePrevious = ValueCurrent;
		ValueCurrent = valueCurrent;
		ValuePrevious = valuePrevious;

		ValueDifference = ValueCurrent - ValuePrevious;
	}
}
