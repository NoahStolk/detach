﻿using Detach;
using Detach.ImGuiUtilities.Services.Ui;
using Detach.Metrics;
using Hexa.NET.ImGui;

namespace Demos.Plot.Services.Ui;

public sealed class HeapAllocationMetricsWindow
{
	private readonly HeapAllocationCounter _heapAllocationCounter;
	private readonly AllocatesBytesPlot _allocatesBytesPlot;

	public HeapAllocationMetricsWindow(HeapAllocationCounter heapAllocationCounter, AllocatesBytesPlot allocatesBytesPlot)
	{
		_heapAllocationCounter = heapAllocationCounter;
		_allocatesBytesPlot = allocatesBytesPlot;
	}

	public void Render()
	{
		if (ImGui.Begin("Allocations"))
		{
			_allocatesBytesPlot.Render();

			ImGui.Text(Inline.Utf8($"Allocated: {_heapAllocationCounter.AllocatedBytes:N0} bytes"));
			ImGui.Text(Inline.Utf8($"Since last update: {_heapAllocationCounter.AllocatedBytesSinceLastUpdate:N0} bytes"));

			for (int i = 0; i < GC.MaxGeneration + 1; i++)
				ImGui.Text(Inline.Utf8($"Gen{i}: {GC.CollectionCount(i)} times"));

			ImGui.Text(Inline.Utf8($"Total memory: {GC.GetTotalMemory(false):N0} bytes"));
			ImGui.Text(Inline.Utf8($"Total pause duration: {GC.GetTotalPauseDuration().TotalSeconds:0.000} s"));
		}

		ImGui.End();
	}
}
