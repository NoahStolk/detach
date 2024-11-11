using Detach;
using Detach.Metrics;
using ImGuiNET;

namespace Demos.ImGuiBackend.GlfwImGuiNET.Services.Ui;

public sealed class PerformanceWindow
{
	private readonly FrameCounter _frameCounter;
	private readonly HeapAllocationCounter _heapAllocationCounter;

	public PerformanceWindow(FrameCounter frameCounter, HeapAllocationCounter heapAllocationCounter)
	{
		_frameCounter = frameCounter;
		_heapAllocationCounter = heapAllocationCounter;
	}

	public void Render()
	{
		if (ImGui.Begin("Performance"))
		{
			ImGui.SeparatorText("Rendering");

			ImGui.Text(Inline.Utf16($"{_frameCounter.FrameCountPreviousSecond} FPS"));
			ImGui.Text(Inline.Utf16($"Frame time: {_frameCounter.CurrentFrameTime:0.0000} s"));

			ImGui.SeparatorText("Allocations");

			ImGui.Text(Inline.Utf16($"Allocated: {_heapAllocationCounter.AllocatedBytes:N0} bytes"));
			ImGui.Text(Inline.Utf16($"Since last update: {_heapAllocationCounter.AllocatedBytesSinceLastUpdate:N0} bytes"));

			for (int i = 0; i < GC.MaxGeneration + 1; i++)
				ImGui.Text(Inline.Utf16($"Gen{i}: {GC.CollectionCount(i)} times"));

			ImGui.Text(Inline.Utf16($"Total memory: {GC.GetTotalMemory(false):N0} bytes"));
			ImGui.Text(Inline.Utf16($"Total pause duration: {GC.GetTotalPauseDuration().TotalSeconds:0.000} s"));
		}

		ImGui.End();
	}
}
