using Detach.Buffers;

namespace Detach.Metrics;

public sealed class FrameCounter
{
	private int _currentSecond;

	/// <summary>
	/// The amount of frames in the current second.
	/// </summary>
	public int FrameCountCurrentSecond { get; private set; }

	/// <summary>
	/// The amount of frames in the previous second.
	/// </summary>
	public int FrameCountPreviousSecond { get; private set; }

	/// <summary>
	/// The length of the current frame.
	/// </summary>
	public double CurrentFrameTime { get; private set; }

	/// <summary>
	/// Buffer containing the most recent frame times in milliseconds.
	/// </summary>
	public RingBuffer<double> FrameTimesMs { get; } = new(1024);

	/// <summary>
	/// Updates the counter.
	/// </summary>
	/// <param name="currentTime">The current time according to the window.</param>
	/// <param name="currentFrameTime">The length of the current frame.</param>
	public void Update(double currentTime, double currentFrameTime)
	{
		int currentSecond = (int)currentTime;

		FrameCountCurrentSecond++;
		CurrentFrameTime = currentFrameTime;
		FrameTimesMs.Add(currentFrameTime * 1000);

		if (_currentSecond == currentSecond)
			return;

		FrameCountPreviousSecond = FrameCountCurrentSecond;
		FrameCountCurrentSecond = 0;
		_currentSecond = currentSecond;
	}
}
