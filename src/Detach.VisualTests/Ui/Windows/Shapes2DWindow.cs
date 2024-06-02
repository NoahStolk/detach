using Detach.Collisions.Primitives;
using ImGuiNET;
using System.Numerics;

namespace Detach.VisualTests.Ui.Windows;

public static class Shapes2DWindow
{
	private static readonly List<LineSegment2D> _lineSegments = [];
	private static readonly List<Circle> _circles = [];
	private static readonly List<Rectangle> _rectangles = [];
	private static readonly List<OrientedRectangle> _orientedRectangles = [];

	public static void Render()
	{
		ImGui.SetNextWindowPos(new Vector2(128, 64));
		ImGui.SetNextWindowSize(new Vector2(1024, 1024));
		if (ImGui.Begin("Shapes 2D", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove))
		{
			RenderSideMenu();

			ImGui.SameLine();
			RenderViewer();
		}

		ImGui.End();
	}

	private static void RenderSideMenu()
	{
		if (ImGui.BeginChild("Shapes 2D Side Menu", new Vector2(256, 0), ImGuiChildFlags.Border))
		{
			// TODO: Create primitives by dragging mouse.
			// TODO: Allow selecting, moving, and deleting primitives.
			// TODO: Show collision results.
			LineSegment2DPopup.Render(_lineSegments);
			if (ImGui.Button("New line segment"))
				ImGui.OpenPopup(LineSegment2DPopup.PopupName);

			CirclePopup.Render(_circles);
			if (ImGui.Button("New circle"))
				ImGui.OpenPopup(CirclePopup.PopupName);

			RectanglePopup.Render(_rectangles);
			if (ImGui.Button("New rectangle"))
				ImGui.OpenPopup(RectanglePopup.PopupName);

			OrientedRectanglePopup.Render(_orientedRectangles);
			if (ImGui.Button("New oriented rectangle"))
				ImGui.OpenPopup(OrientedRectanglePopup.PopupName);
		}

		ImGui.EndChild();
	}

	private static void RenderViewer()
	{
		if (ImGui.BeginChild("Shapes 2D Viewer", new Vector2(0, 0), ImGuiChildFlags.Border))
		{
			ImDrawListPtr drawList = ImGui.GetWindowDrawList();
			Vector2 origin = ImGui.GetCursorScreenPos();
			PositionedDrawList positionedDrawList = new(drawList, origin);

			foreach (LineSegment2D lineSegment in _lineSegments)
				positionedDrawList.AddLine(lineSegment, 0xFFFFFFFF);

			foreach (Circle circle in _circles)
				positionedDrawList.AddCircle(circle, 0xFFFFFFFF);

			foreach (Rectangle rectangle in _rectangles)
				positionedDrawList.AddRectangle(rectangle, 0xFFFFFFFF);

			foreach (OrientedRectangle orientedRectangle in _orientedRectangles)
				positionedDrawList.AddOrientedRectangle(orientedRectangle, 0xFFFFFFFF);
		}

		ImGui.EndChild();
	}
}
