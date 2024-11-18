using Detach.ImGuiUtilities.Utils;
using Detach.Metrics;
using Hexa.NET.ImPlot;

namespace Detach.ImGuiUtilities.Services.Ui;

public sealed class AllocatesBytesPlot
{
	private readonly HeapAllocationCounter _heapAllocationCounter;

	public AllocatesBytesPlot(HeapAllocationCounter heapAllocationCounter)
	{
		_heapAllocationCounter = heapAllocationCounter;
	}

	public unsafe bool Render()
	{
		if (!ImPlot.BeginPlot("Allocated Bytes", ImPlotFlags.NoInputs | ImPlotFlags.NoLegend | ImPlotFlags.NoMouseText))
			return false;

		ImPlot.SetupAxes("Frame number", "Allocated bytes", ImPlotAxisFlags.AutoFit, ImPlotAxisFlags.AutoFit);
		ImPlot.SetupAxisFormat(ImAxis.Y1, static (value, buff, size, userData) => ImPlotFormatterDelegates.Format(value, buff, size, userData, "N0"));
		ImPlot.SetupAxisZoomConstraints(ImAxis.Y1, 1000, long.MaxValue);

		ImPlot.PlotLine("Total", ref _heapAllocationCounter.AllocatedBytesBuffer.First, _heapAllocationCounter.AllocatedBytesBuffer.Length, 1, 0, ImPlotLineFlags.None, _heapAllocationCounter.AllocatedBytesBuffer.Head);

		ImPlot.EndPlot();

		return true;
	}
}
