namespace Detach.Demos.Glfw.ImGuiBackend.ImGuiNET.Services;

public sealed class PerformanceMeasurement
{
	private int _currentSecond;
	private int _renders;
	public int Fps { get; private set; }
	public double FrameTime { get; private set; }

	public void Update(double currentTime, double frameTime)
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
