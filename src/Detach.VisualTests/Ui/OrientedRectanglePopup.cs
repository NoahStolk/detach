using Detach.Collisions.Primitives;
using ImGuiNET;
using System.Numerics;

namespace Detach.VisualTests.Ui;

public static class OrientedRectanglePopup
{
	public const string PopupName = "New oriented rectangle";

	private static Vector2 _positionCenter;
	private static Vector2 _halfExtents;
	private static float _rotationInRadians;

	public static void Render(List<OrientedRectangle> orientedRectangles)
	{
		if (ImGui.BeginPopup(PopupName))
		{
			ImGui.InputFloat2("Position (center)", ref _positionCenter);
			ImGui.InputFloat2("Half extents", ref _halfExtents);
			ImGui.SliderAngle("Rotation (in radians)", ref _rotationInRadians);

			if (ImGui.Button("Add##oriented rectangle"))
			{
				orientedRectangles.Add(new OrientedRectangle(_positionCenter, _halfExtents, _rotationInRadians));
				ImGui.CloseCurrentPopup();
			}

			ImGui.EndPopup();
		}
	}
}
