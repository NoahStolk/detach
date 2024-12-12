using Detach;
using Detach.ImGuiUtilities.Services.Ui;
using Detach.Metrics;
using Hexa.NET.ImGui;

namespace Demos.Plot.Services.Ui;

internal sealed class RenderingMetricsWindow(FrameCounter frameCounter, FrameTimesPlot frameTimesPlot)
{
	public void Render()
	{
		if (ImGui.Begin("Rendering"))
		{
			frameTimesPlot.Render();

			ImGui.Text(Inline.Utf8($"{frameCounter.FrameCountPreviousSecond} FPS"));
		}

		ImGui.End();
	}
}
