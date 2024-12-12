using Detach;
using Detach.GlfwExtensions;
using Detach.Numerics;
using ImGuiNET;
using Silk.NET.GLFW;
using System.Numerics;

namespace Demos.ImGuiBackend.GlfwImGuiNET.Services.Ui;

internal sealed class KeyboardInputWindow(GlfwInput glfwInput)
{
	private readonly Dictionary<Keys, string> _keyDisplayStringCache = [];

	public void Render()
	{
		if (ImGui.Begin("Keyboard input"))
		{
			ImGuiIOPtr io = ImGui.GetIO();

			ImGui.SeparatorText("ImGui key modifiers");
			ImGui.TextColored(io.KeyCtrl ? Rgba.White : Rgba.Gray(0.4f), "CTRL");
			ImGui.SameLine();
			ImGui.TextColored(io.KeyShift ? Rgba.White : Rgba.Gray(0.4f), "SHIFT");
			ImGui.SameLine();
			ImGui.TextColored(io.KeyAlt ? Rgba.White : Rgba.Gray(0.4f), "ALT");
			ImGui.SameLine();
			ImGui.TextColored(io.KeySuper ? Rgba.White : Rgba.Gray(0.4f), "SUPER");

			ImGui.SeparatorText("GLFW keys");
			if (ImGui.BeginTable("GLFW keys", 8))
			{
				for (int i = 0; i < 1024; i++)
				{
					if (i == 0)
						ImGui.TableNextRow();

					Keys key = (Keys)i;
					if (!Enum.IsDefined(key))
						continue;

					bool isDown = glfwInput.IsKeyDown(key);

					ImGui.TableNextColumn();

					if (!_keyDisplayStringCache.TryGetValue(key, out string? displayString))
					{
						displayString = key.ToString();
						_keyDisplayStringCache[key] = displayString;
					}

					ImGui.TextColored(isDown ? Rgba.White : Rgba.Gray(0.4f), displayString);
				}

				ImGui.EndTable();
			}

			ImGui.SeparatorText("GLFW pressed chars");

			ImGui.Text(Inline.Utf16($"{glfwInput.CharsPressed.Count} key(s):"));
			if (ImGui.BeginChild("CharsPressedChildWindow", new Vector2(0, 48), ImGuiChildFlags.Border, ImGuiWindowFlags.AlwaysVerticalScrollbar))
			{
				for (int i = 0; i < glfwInput.CharsPressed.Count; i++)
				{
					ImGui.Text(Inline.Utf16((char)glfwInput.CharsPressed[i]));
				}
			}

			ImGui.EndChild();

			ImGui.SeparatorText("GLFW [SPACE] key state");

			ImGui.Text(Inline.Utf16($"Down: {(glfwInput.IsKeyDown(Keys.Space) ? "true" : "false")}"));
			ImGui.Text(Inline.Utf16($"Repeating: {(glfwInput.IsKeyRepeating(Keys.Space) ? "true" : "false")}"));
			ImGui.Text(Inline.Utf16($"Pressed: {(glfwInput.IsKeyPressed(Keys.Space) ? "true" : "false")}"));
			ImGui.Text(Inline.Utf16($"Released: {(glfwInput.IsKeyReleased(Keys.Space) ? "true" : "false")}"));
		}

		ImGui.End();
	}
}
