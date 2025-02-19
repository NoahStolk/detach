using Detach;
using Detach.ImGuiUtilities;
using Detach.Metrics;
using Hexa.NET.ImGui;

namespace Demos.Plot.Services.Ui;

internal sealed class RenderingMetricsWindow(FrameCounter frameCounter)
{
	public void Render()
	{
		if (ImGui.Begin("Rendering"))
		{
			FrameTimesPlot.Render(ref frameCounter.FrameTimesMs.First, frameCounter.FrameTimesMs.Length, frameCounter.FrameTimesMs.Head);

			ImGui.Text(Inline.Utf8($"{frameCounter.FrameCountPreviousSecond} FPS"));
		}

		ImGui.End();
	}
}
