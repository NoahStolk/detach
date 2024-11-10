using ImGuiNET;

namespace Demos.ImGuiBackend.GlfwImGuiNET.Services.Ui;

public sealed class SettingsWindow
{
	private readonly Silk.NET.GLFW.Glfw _glfw;

	private bool _vsync;

	public SettingsWindow(Silk.NET.GLFW.Glfw glfw)
	{
		_glfw = glfw;
	}

	public void Render()
	{
		if (ImGui.Begin("Settings"))
		{
			if (ImGui.Checkbox("VSync", ref _vsync))
				_glfw.SwapInterval(_vsync ? 1 : 0);
		}

		ImGui.End();
	}
}
