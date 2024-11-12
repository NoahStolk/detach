namespace Detach.Buffers;

public sealed class RingBuffer<T>
{
	private readonly T[] _values;

	public RingBuffer(int length)
	{
		_values = new T[length];
		Length = length;
	}

	public ref T First => ref _values[0];

	public int Length { get; }

	public int Tail { get; private set; }

	public int Head { get; private set; }

	public void Add(T value)
	{
		_values[Tail] = value;
		Tail++;

		if (Tail == Length)
			Tail = 0;
		else if (Tail < 0)
			Tail = Length - 1;

		Head = (Tail - Length) % Length;

		if (Head < 0)
			Head += Length;
	}
}
