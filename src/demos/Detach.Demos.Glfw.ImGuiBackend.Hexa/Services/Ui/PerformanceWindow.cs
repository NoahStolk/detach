using Hexa.NET.ImGui;

namespace Detach.Demos.Glfw.ImGuiBackend.Hexa.Services.Ui;

public sealed class PerformanceWindow
{
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

			ImGui.Text(Inline.Utf8($"{_performanceMeasurement.Fps} FPS"));
			ImGui.Text(Inline.Utf8($"Frame time: {_performanceMeasurement.FrameTime:0.0000} s"));

			ImGui.SeparatorText("Allocations");

			ImGui.Text(Inline.Utf8($"Allocated: {_performanceMeasurement.AllocatedBytes:N0} bytes"));
			ImGui.Text(Inline.Utf8($"Since last update: {_performanceMeasurement.AllocatedBytesSinceLastUpdate:N0} bytes"));

			for (int i = 0; i < GC.MaxGeneration + 1; i++)
				ImGui.Text(Inline.Utf8($"Gen{i}: {GC.CollectionCount(i)} times"));

			ImGui.Text(Inline.Utf8($"Total memory: {GC.GetTotalMemory(false):N0} bytes"));
			ImGui.Text(Inline.Utf8($"Total pause duration: {GC.GetTotalPauseDuration().TotalSeconds:0.000} s"));
		}

		ImGui.End();
	}
}
