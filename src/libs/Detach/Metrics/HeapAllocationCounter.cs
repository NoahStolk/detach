using Detach.Buffers;

namespace Detach.Metrics;

public sealed class HeapAllocationCounter
{
	private readonly SimpleCounter<long> _counter = new();

	public long AllocatedBytes => _counter.ValueCurrent;

	public long AllocatedBytesSinceLastUpdate => _counter.ValueDifference;

	public RingBuffer<long> AllocatedBytesBuffer { get; } = new(1024);

	public void UpdateAllocatedBytesForCurrentThread()
	{
		_counter.Update(GC.GetAllocatedBytesForCurrentThread());
		AllocatedBytesBuffer.Add(AllocatedBytes);
	}
}
