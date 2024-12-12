﻿using Hexa.NET.ImGui;
using Silk.NET.GLFW;

namespace Demos.ImGuiBackend.GlfwHexa.Services.Ui;

internal sealed class SettingsWindow(Glfw glfw)
{
	private bool _vsync;

	public void Render()
	{
		if (ImGui.Begin("Settings"))
		{
			if (ImGui.Checkbox("VSync", ref _vsync))
				glfw.SwapInterval(_vsync ? 1 : 0);
		}

		ImGui.End();
	}
}
