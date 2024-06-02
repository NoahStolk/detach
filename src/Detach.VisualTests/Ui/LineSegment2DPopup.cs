using Detach.Collisions.Primitives;
using ImGuiNET;
using System.Numerics;

namespace Detach.VisualTests.Ui;

public static class LineSegment2DPopup
{
	public const string PopupName = "New line segment";

	private static Vector2 _lineSegmentStart;
	private static Vector2 _lineSegmentEnd;

	public static void Render(List<LineSegment2D> lineSegments)
	{
		if (ImGui.BeginPopup(PopupName))
		{
			ImGui.InputFloat2("Start", ref _lineSegmentStart);
			ImGui.InputFloat2("End", ref _lineSegmentEnd);

			if (ImGui.Button("Add##line segment"))
			{
				lineSegments.Add(new LineSegment2D(_lineSegmentStart, _lineSegmentEnd));
				ImGui.CloseCurrentPopup();
			}

			ImGui.EndPopup();
		}
	}
}
