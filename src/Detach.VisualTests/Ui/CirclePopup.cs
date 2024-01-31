using Detach.Collisions.Primitives;
using ImGuiNET;
using System.Numerics;

namespace Detach.VisualTests.Ui;

public static class CirclePopup
{
	public const string PopupName = "New circle";

	private static Vector2 _position;
	private static float _radius;

	public static void Render(List<Circle> circles)
	{
		if (ImGui.BeginPopup(PopupName))
		{
			ImGui.InputFloat2("Position", ref _position);
			ImGui.InputFloat("Radius", ref _radius);

			if (ImGui.Button("Add##circle"))
			{
				circles.Add(new(_position, _radius));
				ImGui.CloseCurrentPopup();
			}

			ImGui.EndPopup();
		}
	}
}
