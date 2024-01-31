using ImGuiNET;

namespace Detach.VisualTests.Ui;

public static class MainWindow
{
	public static void Render()
	{
		if (ImGui.Begin("Main Window"))
		{
			ImGui.Text("Hello, world!");
		}

		ImGui.End();
	}
}
