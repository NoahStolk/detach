using Hexa.NET.ImGui;
using Hexa.NET.ImPlot;

namespace Demos.Plot.Services.Ui;

public sealed class PlotWindow
{
	private readonly int[] _barData = Enumerable.Range(0, 10).ToArray();
	private readonly float[] _xData = Enumerable.Range(0, 10).Select(i => (float)i).ToArray();
	private readonly float[] _yData = Enumerable.Range(0, 10).Select(i => MathF.Sin(i)).ToArray();

	public unsafe void Render()
	{
		if (ImGui.Begin("Plot Window"))
		{
			if (ImPlot.BeginPlot("Plot"))
			{
				fixed (int* barDataPtr = _barData)
					ImPlot.PlotBars("Bars", barDataPtr, _barData.Length);

				fixed (float* xDataPtr = _xData)
				{
					fixed (float* yDataPtr = _yData)
						ImPlot.PlotLine("Lines", xDataPtr, yDataPtr, _xData.Length);
				}

				ImPlot.EndPlot();
			}
		}

		ImGui.End();
	}
}
