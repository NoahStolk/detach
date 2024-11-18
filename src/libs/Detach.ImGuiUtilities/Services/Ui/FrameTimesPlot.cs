using Detach.Metrics;
using Hexa.NET.ImPlot;

namespace Detach.ImGuiUtilities.Services.Ui;

public sealed class FrameTimesPlot
{
	private readonly FrameCounter _frameCounter;

	public FrameTimesPlot(FrameCounter frameCounter)
	{
		_frameCounter = frameCounter;
	}

	public bool Render()
	{
		if (!ImPlot.BeginPlot("Frame Times", ImPlotFlags.NoLegend | ImPlotFlags.NoMouseText))
			return false;

		ImPlot.SetupAxes("Frame number", "Frame time", ImPlotAxisFlags.AutoFit, ImPlotAxisFlags.None);
		ImPlot.SetupAxesLimits(0, _frameCounter.FrameTimesMs.Length, 0, 33);
		ImPlot.SetupAxisLimitsConstraints(ImAxis.Y1, 0, 1000);

		ImPlot.PushStyleVar(ImPlotStyleVar.FillAlpha, 0.125f);
		ImPlot.PlotShaded("Total", ref _frameCounter.FrameTimesMs.First, _frameCounter.FrameTimesMs.Length, 0, 1, 0, ImPlotShadedFlags.None, _frameCounter.FrameTimesMs.Head);
		ImPlot.PopStyleVar();

		ImPlot.PlotLine("Total", ref _frameCounter.FrameTimesMs.First, _frameCounter.FrameTimesMs.Length, 1, 0, ImPlotLineFlags.None, _frameCounter.FrameTimesMs.Head);

		ImPlot.EndPlot();

		return true;
	}
}
