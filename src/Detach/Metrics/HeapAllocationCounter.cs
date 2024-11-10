namespace Detach.Metrics;

public sealed class HeapAllocationCounter
{
	private readonly SimpleCounter<long> _counter = new();

	public long AllocatedBytes => _counter.ValueCurrent;
	public long AllocatedBytesSinceLastUpdate => _counter.ValueDifference;

	public void UpdateAllocatedBytesForCurrentThread()
	{
		_counter.Update(GC.GetAllocatedBytesForCurrentThread());
	}
}
