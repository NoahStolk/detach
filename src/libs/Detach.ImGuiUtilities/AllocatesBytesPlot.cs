using Hexa.NET.ImPlot;

namespace Detach.ImGuiUtilities;

public static class AllocatesBytesPlot
{
	public static unsafe bool Render(ref long first, int size, int head)
	{
		if (!ImPlot.BeginPlot("Allocated Bytes", ImPlotFlags.NoInputs | ImPlotFlags.NoLegend | ImPlotFlags.NoMouseText))
			return false;

		ImPlot.SetupAxes("Frame number", "Allocated bytes", ImPlotAxisFlags.AutoFit, ImPlotAxisFlags.AutoFit);
		ImPlot.SetupAxisFormat(ImAxis.Y1, static (value, buff, size, userData) => ImPlotFormatterDelegates.Format(value, buff, size, userData, "N0"));
		ImPlot.SetupAxisZoomConstraints(ImAxis.Y1, 1000, long.MaxValue);

		ImPlot.PlotLine("Total", ref first, size, 1, 0, ImPlotLineFlags.None, head);

		ImPlot.EndPlot();

		return true;
	}
}
