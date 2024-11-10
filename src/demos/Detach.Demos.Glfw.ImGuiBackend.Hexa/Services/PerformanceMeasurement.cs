namespace Detach.Demos.Glfw.ImGuiBackend.Hexa.Services;

public sealed class PerformanceMeasurement
{
	private int _currentSecond;
	private int _renders;

	public int Fps { get; private set; }
	public double FrameTime { get; private set; }

	public long AllocatedBytes { get; private set; }
	public long AllocatedBytesSinceLastUpdate { get; private set; }
	public long PreviousAllocatedBytes { get; private set; }

	public void UpdateAllocatedBytes(long allocatedBytes)
	{
		AllocatedBytes = allocatedBytes;
		AllocatedBytesSinceLastUpdate = AllocatedBytes - PreviousAllocatedBytes;
		PreviousAllocatedBytes = allocatedBytes;
	}

	public void UpdateFrameTime(double currentTime, double frameTime)
	{
		_renders++;
		FrameTime = (float)frameTime;
		if (_currentSecond == (int)currentTime)
			return;

		Fps = _renders;
		_renders = 0;
		_currentSecond = (int)currentTime;
	}
}
