using Detach;
using ImGuiNET;

namespace Demos.ImGuiBackend.GlfwImGuiNET.Services.Ui;

public sealed class PerformanceWindow
{
	private long _previousAllocatedBytes;

	private readonly PerformanceMeasurement _performanceMeasurement;

	public PerformanceWindow(PerformanceMeasurement performanceMeasurement)
	{
		_performanceMeasurement = performanceMeasurement;
	}

	public void Render()
	{
		if (ImGui.Begin("Performance"))
		{
			ImGui.SeparatorText("Rendering");

			ImGui.Text(Inline.Utf16($"{_performanceMeasurement.Fps} FPS"));
			ImGui.Text(Inline.Utf16($"Frame time: {_performanceMeasurement.FrameTime:0.0000} s"));

			ImGui.SeparatorText("Allocations");

			long allocatedBytes = GC.GetAllocatedBytesForCurrentThread();
			ImGui.Text(Inline.Utf16($"Allocated: {allocatedBytes:N0} bytes"));
			ImGui.Text(Inline.Utf16($"Since last update: {allocatedBytes - _previousAllocatedBytes:N0} bytes"));
			_previousAllocatedBytes = allocatedBytes;

			for (int i = 0; i < GC.MaxGeneration + 1; i++)
				ImGui.Text(Inline.Utf16($"Gen{i}: {GC.CollectionCount(i)} times"));

			ImGui.Text(Inline.Utf16($"Total memory: {GC.GetTotalMemory(false):N0} bytes"));
			ImGui.Text(Inline.Utf16($"Total pause duration: {GC.GetTotalPauseDuration().TotalSeconds:0.000} s"));
		}

		ImGui.End();
	}
}
