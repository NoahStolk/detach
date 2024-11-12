using Demos.Plot.Utils;
using Detach;
using Detach.Metrics;
using Hexa.NET.ImGui;
using Hexa.NET.ImPlot;

namespace Demos.Plot.Services.Ui;

public sealed class PerformanceWindow
{
	private readonly FrameCounter _frameCounter;
	private readonly HeapAllocationCounter _heapAllocationCounter;

	public PerformanceWindow(FrameCounter frameCounter, HeapAllocationCounter heapAllocationCounter)
	{
		_frameCounter = frameCounter;
		_heapAllocationCounter = heapAllocationCounter;
	}

	public unsafe void Render()
	{
		if (ImGui.Begin("Performance"))
		{
			ImGui.SeparatorText("Rendering");

			ImGui.Text(Inline.Utf8($"{_frameCounter.FrameCountPreviousSecond} FPS"));

			if (ImPlot.BeginPlot("Frame Times", ImPlotFlags.NoLegend | ImPlotFlags.NoMouseText))
			{
				ImPlot.SetupAxes("Frame number", "Frame time", ImPlotAxisFlags.AutoFit, ImPlotAxisFlags.None);
				ImPlot.SetupAxesLimits(0, _frameCounter.FrameTimesMs.Length, 0, 33);
				ImPlot.SetupAxisLimitsConstraints(ImAxis.Y1, 0, 1000);

				ImPlot.PushStyleVar(ImPlotStyleVar.FillAlpha, 0.125f);
				ImPlot.PlotShaded("Total", ref _frameCounter.FrameTimesMs.First, _frameCounter.FrameTimesMs.Length, 0, 1, 0, ImPlotShadedFlags.None, _frameCounter.FrameTimesMs.Head);
				ImPlot.PopStyleVar();

				ImPlot.PlotLine("Total", ref _frameCounter.FrameTimesMs.First, _frameCounter.FrameTimesMs.Length, 1, 0, ImPlotLineFlags.None, _frameCounter.FrameTimesMs.Head);

				ImPlot.EndPlot();
			}

			ImGui.SeparatorText("Allocations");

			ImGui.Text(Inline.Utf8($"Allocated: {_heapAllocationCounter.AllocatedBytes:N0} bytes"));
			ImGui.Text(Inline.Utf8($"Since last update: {_heapAllocationCounter.AllocatedBytesSinceLastUpdate:N0} bytes"));

			if (ImPlot.BeginPlot("Allocated Bytes", ImPlotFlags.NoInputs | ImPlotFlags.NoLegend | ImPlotFlags.NoMouseText))
			{
				ImPlot.SetupAxes("Frame number", "Allocated bytes", ImPlotAxisFlags.AutoFit, ImPlotAxisFlags.AutoFit);
				ImPlot.SetupAxisFormat(ImAxis.Y1, static (value, buff, size, userData) => ImPlotFormatterDelegates.Format(value, buff, size, userData, "N0"));
				ImPlot.SetupAxisZoomConstraints(ImAxis.Y1, 1000, long.MaxValue);

				ImPlot.PlotLine("Total", ref _heapAllocationCounter.AllocatedBytesBuffer.First, _heapAllocationCounter.AllocatedBytesBuffer.Length, 1, 0, ImPlotLineFlags.None, _heapAllocationCounter.AllocatedBytesBuffer.Head);

				ImPlot.EndPlot();
			}

			for (int i = 0; i < GC.MaxGeneration + 1; i++)
				ImGui.Text(Inline.Utf8($"Gen{i}: {GC.CollectionCount(i)} times"));

			ImGui.Text(Inline.Utf8($"Total memory: {GC.GetTotalMemory(false):N0} bytes"));
			ImGui.Text(Inline.Utf8($"Total pause duration: {GC.GetTotalPauseDuration().TotalSeconds:0.000} s"));
		}

		ImGui.End();
	}
}
