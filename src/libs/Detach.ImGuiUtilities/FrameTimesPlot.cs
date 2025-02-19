using Hexa.NET.ImPlot;

namespace Detach.ImGuiUtilities;

public static class FrameTimesPlot
{
	public static bool Render(ref double first, int size, int head)
	{
		if (!ImPlot.BeginPlot("Frame Times", ImPlotFlags.NoLegend | ImPlotFlags.NoMouseText))
			return false;

		ImPlot.SetupAxes("Frame number", "Frame time", ImPlotAxisFlags.AutoFit, ImPlotAxisFlags.None);
		ImPlot.SetupAxesLimits(0, size, 0, 33);
		ImPlot.SetupAxisLimitsConstraints(ImAxis.Y1, 0, 1000);

		ImPlot.PushStyleVar(ImPlotStyleVar.FillAlpha, 0.125f);
		ImPlot.PlotShaded("Total", ref first, size, 0, 1, 0, ImPlotShadedFlags.None, head);
		ImPlot.PopStyleVar();

		ImPlot.PlotLine("Total", ref first, size, 1, 0, ImPlotLineFlags.None, head);

		ImPlot.EndPlot();

		return true;
	}
}
