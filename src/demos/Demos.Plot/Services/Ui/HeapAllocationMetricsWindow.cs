﻿using Detach;
using Detach.ImGuiUtilities;
using Detach.Metrics;
using Hexa.NET.ImGui;

namespace Demos.Plot.Services.Ui;

internal sealed class HeapAllocationMetricsWindow(HeapAllocationCounter heapAllocationCounter)
{
	public void Render()
	{
		if (ImGui.Begin("Allocations"))
		{
			AllocatesBytesPlot.Render(ref heapAllocationCounter.AllocatedBytesBuffer.First, heapAllocationCounter.AllocatedBytesBuffer.Length, heapAllocationCounter.AllocatedBytesBuffer.Head);

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
