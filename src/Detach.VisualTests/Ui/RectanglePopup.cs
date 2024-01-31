using Detach.Collisions.Primitives;
using ImGuiNET;
using System.Numerics;

namespace Detach.VisualTests.Ui;

public static class RectanglePopup
{
	public const string PopupName = "New rectangle";

	private static Vector2 _positionTopLeft;
	private static Vector2 _size;

	public static void Render(List<Rectangle> rectangles)
	{
		if (ImGui.BeginPopup(PopupName))
		{
			ImGui.InputFloat2("Position (top left)", ref _positionTopLeft);
			ImGui.InputFloat2("Size", ref _size);

			if (ImGui.Button("Add##rectangle"))
			{
				rectangles.Add(new(_positionTopLeft, _size));
				ImGui.CloseCurrentPopup();
			}

			ImGui.EndPopup();
		}
	}
}
