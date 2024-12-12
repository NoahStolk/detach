using Detach;
using Detach.GlfwExtensions;
using Detach.Numerics;
using ImGuiNET;
using Silk.NET.GLFW;

namespace Demos.ImGuiBackend.GlfwImGuiNET.Services.Ui;

internal sealed class MouseInputWindow(GlfwInput glfwInput)
{
	private readonly Dictionary<MouseButton, string> _mouseButtonDisplayStringCache = [];

	public void Render()
	{
		if (ImGui.Begin("Mouse input"))
		{
			ImGui.SeparatorText("GLFW mouse buttons");

			if (ImGui.BeginTable("GLFW mouse buttons", 8))
			{
				for (int i = 0; i < 8; i++)
				{
					if (i == 0)
						ImGui.TableNextRow();

					MouseButton button = (MouseButton)i;
					if (!Enum.IsDefined(button))
						continue;

					bool isDown = glfwInput.IsMouseButtonDown(button);

					ImGui.TableNextColumn();

					if (!_mouseButtonDisplayStringCache.TryGetValue(button, out string? displayString))
					{
						displayString = button.ToString();
						_mouseButtonDisplayStringCache[button] = displayString;
					}

					ImGui.TextColored(isDown ? Rgba.White : Rgba.Gray(0.4f), displayString);
				}

				ImGui.EndTable();
			}

			ImGui.SeparatorText("GLFW mouse wheel");

			ImGui.Text(Inline.Utf16($"Y: {glfwInput.MouseWheelY:0.00}"));

			ImGui.SeparatorText("GLFW mouse position");

			ImGui.Text(Inline.Utf16(glfwInput.CursorPosition, "0.00", null));

			ImGui.SeparatorText("GLFW [LMB] state");

			ImGui.Text(Inline.Utf16($"Down: {(glfwInput.IsMouseButtonDown(MouseButton.Left) ? "true" : "false")}"));
			ImGui.Text(Inline.Utf16($"Pressed: {(glfwInput.IsMouseButtonPressed(MouseButton.Left) ? "true" : "false")}"));
			ImGui.Text(Inline.Utf16($"Released: {(glfwInput.IsMouseButtonReleased(MouseButton.Left) ? "true" : "false")}"));
		}

		ImGui.End();
	}
}
