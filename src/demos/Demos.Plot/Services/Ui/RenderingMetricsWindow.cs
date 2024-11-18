using Detach;
using Detach.ImGuiUtilities.Services.Ui;
using Detach.Metrics;
using Hexa.NET.ImGui;

namespace Demos.Plot.Services.Ui;

public sealed class RenderingMetricsWindow
{
	private readonly FrameCounter _frameCounter;
	private readonly FrameTimesPlot _frameTimesPlot;

	public RenderingMetricsWindow(FrameCounter frameCounter, FrameTimesPlot frameTimesPlot)
	{
		_frameCounter = frameCounter;
		_frameTimesPlot = frameTimesPlot;
	}

	public void Render()
	{
		if (ImGui.Begin("Rendering"))
		{
			_frameTimesPlot.Render();

			ImGui.Text(Inline.Utf8($"{_frameCounter.FrameCountPreviousSecond} FPS"));
		}

		ImGui.End();
	}
}
