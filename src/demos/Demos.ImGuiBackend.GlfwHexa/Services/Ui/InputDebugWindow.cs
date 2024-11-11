using Detach.Numerics;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.ImGuiBackend.GlfwHexa.Services.Ui;

public sealed unsafe class InputDebugWindow
{
	private static readonly byte[] _debugText0 = new byte[1024];
	private static readonly byte[] _debugText1 = new byte[1024];
	private static readonly byte[] _debugText2 = new byte[1024];
	private static readonly byte[] _debugText3 = new byte[1024];
	private static readonly byte[] _debugText4 = new byte[1024];
	private static readonly byte[] _debugText5 = new byte[1024];

	private bool _checkbox;

	public InputDebugWindow()
	{
		"Type letters and numbers: "u8.CopyTo(_debugText0);
		"Type letters and numbers while holding (SHIFT): "u8.CopyTo(_debugText1);
		"Enter some enters, and use the arrow keys to navigate.\nUse backspace and delete to remove text."u8.CopyTo(_debugText2);
		"Insert some tabs (only works for this input field).\nHold keys to see the repeat rate."u8.CopyTo(_debugText3);
		"Select all text, copy, paste, and use CTRL + arrows to navigate between words.\nUse CTRL + backspace to delete words."u8.CopyTo(_debugText4);
		"Use SHIFT + arrows / home / end / page up / page down to select text."u8.CopyTo(_debugText5);
	}

	private static bool InputText(ReadOnlySpan<byte> label, byte[] input)
	{
		fixed (byte* ptr = input)
			return ImGui.InputText(label, ptr, (ulong)input.Length);
	}

	private static bool InputTextMultiline(ReadOnlySpan<byte> label, byte[] input, Vector2 size, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None)
	{
		fixed (byte* ptr = input)
			return ImGui.InputTextMultiline(label, ptr, (ulong)input.Length, size, flags);
	}

	public void Render()
	{
		if (ImGui.Begin("Input testing"))
		{
			ImGui.SeparatorText("Test keyboard input");

			InputText("Letters, numbers"u8, _debugText0);
			InputText("Letters, numbers (SHIFT)"u8, _debugText1);

			InputTextMultiline("Enter, arrow keys, backspace, delete"u8, _debugText2, new Vector2(0, 64));
			InputTextMultiline("Tab"u8, _debugText3, new Vector2(0, 64), ImGuiInputTextFlags.AllowTabInput);
			InputTextMultiline("CTRL shortcut\n- CTRL+A\n- CTRL+C\n- CTRL+V\n- CTRL+arrows"u8, _debugText4, new Vector2(0, 64));
			InputTextMultiline("SHIFT shortcuts\n- SHIFT+arrows\n- SHIFT+home"u8, _debugText5, new Vector2(0, 64));

			ImGui.SeparatorText("Test mouse input");

			ImGui.Checkbox("Checkbox", ref _checkbox);

			if (ImGui.BeginChild("Scroll area", new Vector2(256, 128)))
			{
				for (int i = 0; i < 50; i++)
				{
					Rgba color = (i % 3) switch
					{
						0 => Rgba.Yellow,
						1 => Rgba.Aqua,
						_ => Rgba.Red,
					};
					ReadOnlySpan<byte> text = (i % 3) switch
					{
						0 => "Scrolling should not go to top or bottom instantly"u8,
						1 => "Scrolling should go evenly per frame (not missing inputs or jumping)"u8,
						_ => "This should work with and without VSync"u8,
					};

					ImGui.PushStyleColor(ImGuiCol.Text, color);
					ImGui.TextWrapped(text);
					ImGui.PopStyleColor();

					ImGui.Separator();
				}
			}

			ImGui.EndChild();
		}

		ImGui.End();
	}
}
