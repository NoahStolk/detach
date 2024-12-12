using Detach;
using Detach.Metrics;
using Hexa.NET.ImGui;

namespace Demos.ImGuiBackend.GlfwHexa.Services.Ui;

internal sealed class PerformanceWindow(FrameCounter frameCounter, HeapAllocationCounter heapAllocationCounter)
{
	public void Render()
	{
		if (ImGui.Begin("Performance"))
		{
			ImGui.SeparatorText("Rendering");

			ImGui.Text(Inline.Utf8($"{frameCounter.FrameCountPreviousSecond} FPS"));
			ImGui.Text(Inline.Utf8($"Frame time: {frameCounter.CurrentFrameTime:0.0000} s"));

			ImGui.SeparatorText("Allocations");

			ImGui.Text(Inline.Utf8($"Allocated: {heapAllocationCounter.AllocatedBytes:N0} bytes"));
			ImGui.Text(Inline.Utf8($"Since last update: {heapAllocationCounter.AllocatedBytesSinceLastUpdate:N0} bytes"));

			for (int i = 0; i < GC.MaxGeneration + 1; i++)
				ImGui.Text(Inline.Utf8($"Gen{i}: {GC.CollectionCount(i)} times"));

			ImGui.Text(Inline.Utf8($"Total memory: {GC.GetTotalMemory(false):N0} bytes"));
			ImGui.Text(Inline.Utf8($"Total pause duration: {GC.GetTotalPauseDuration().TotalSeconds:0.000} s"));
		}

		ImGui.End();
	}
}
