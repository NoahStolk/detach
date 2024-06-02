﻿using Detach.Collisions;
using Detach.Collisions.Primitives;
using Detach.VisualTests.State;
using ImGuiNET;
using System.Numerics;

namespace Detach.VisualTests.Ui;

// ReSharper disable ForCanBeConvertedToForeach
public static class Shapes2DWindow
{
	public static void Render()
	{
		const float padding = 64;
		ImGui.SetNextWindowPos(new Vector2(padding, padding));
		ImGui.SetNextWindowSize(new Vector2(Constants.WindowWidth - padding * 2, Constants.WindowHeight - padding * 2));
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
		if (ImGui.BeginChild("Shapes 2D Side Menu", new Vector2(512, 0), ImGuiChildFlags.Border))
		{
			if (ImGui.BeginCombo("SelectedShape", Shapes2DState.SelectedShapeType.ToString()))
			{
				foreach (SelectedShapeType shapeType in Enum.GetValues<SelectedShapeType>())
				{
					bool isSelected = Shapes2DState.SelectedShapeType == shapeType;
					if (ImGui.Selectable(shapeType.ToString(), isSelected))
						Shapes2DState.SelectedShapeType = shapeType;
				}

				ImGui.EndCombo();
			}

			if (ImGui.Button("Clear all"))
			{
				Shapes2DState.LineSegments.Clear();
				Shapes2DState.Circles.Clear();
				Shapes2DState.Rectangles.Clear();
				Shapes2DState.OrientedRectangles.Clear();
			}

			if (ImGui.CollapsingHeader("Shapes"))
			{
				// TODO: Allow deleting shapes.
				ShapeTables.RenderLineSegment2Ds();
				ShapeTables.RenderCircles();
				ShapeTables.RenderRectangles();
				ShapeTables.RenderOrientedRectangles();
			}

			if (ImGui.CollapsingHeader("Collisions"))
			{
				// TODO: Generate test code from the current scenario:
				// - Generate all shapes (Circle circle0 = new(new Vector2(123, 234), 80);)
				// - Generate all collisions (Assert.IsTrue(Geometry2D.CircleRectangle(circle0, rectangle0));)
				// - etc.
				for (int i = 0; i < Shapes2DState.LineSegments.Count; i++)
				{
					LineSegment2D lineSegment = Shapes2DState.LineSegments[i];

					for (int j = i + 1; j < Shapes2DState.LineSegments.Count; j++)
					{
						LineSegment2D lineSegment2 = Shapes2DState.LineSegments[j];
						if (Geometry2D.LineLine(lineSegment, lineSegment2))
							ImGui.Text("LineSegment2D - LineSegment2D");
					}

					for (int j = 0; j < Shapes2DState.Circles.Count; j++)
					{
						Circle circle = Shapes2DState.Circles[j];
						if (Geometry2D.LineCircle(lineSegment, circle))
							ImGui.Text("LineSegment2D - Circle");
					}

					for (int j = 0; j < Shapes2DState.Rectangles.Count; j++)
					{
						Rectangle rectangle = Shapes2DState.Rectangles[j];
						if (Geometry2D.LineRectangle(lineSegment, rectangle))
							ImGui.Text("LineSegment2D - Rectangle");
					}

					for (int j = 0; j < Shapes2DState.OrientedRectangles.Count; j++)
					{
						OrientedRectangle orientedRectangle = Shapes2DState.OrientedRectangles[j];
						if (Geometry2D.LineOrientedRectangle(lineSegment, orientedRectangle))
							ImGui.Text("LineSegment2D - OrientedRectangle");
					}
				}
			}
		}

		ImGui.EndChild();
	}

	private static void RenderViewer()
	{
		if (ImGui.BeginChild("Shapes 2D Viewer", new Vector2(0, 0), ImGuiChildFlags.Border))
		{
			Vector2 origin = ImGui.GetCursorScreenPos();
			Vector2 mousePosition = ImGui.GetMousePos();
			Vector2 relativeMousePosition = mousePosition - origin;

			if (!Shapes2DState.IsCreatingShape && ImGui.IsWindowHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
			{
				Shapes2DState.IsCreatingShape = true;
				Shapes2DState.ShapeStart = relativeMousePosition;
			}

			if (Shapes2DState.IsCreatingShape && ImGui.IsMouseReleased(ImGuiMouseButton.Left))
			{
				Shapes2DState.IsCreatingShape = false;
				CreateShape(Shapes2DState.ShapeStart, relativeMousePosition);
			}

			ImDrawListPtr drawList = ImGui.GetWindowDrawList();
			PositionedDrawList positionedDrawList = new(drawList, origin);

			foreach (LineSegment2D lineSegment in Shapes2DState.LineSegments)
				positionedDrawList.AddLine(lineSegment, 0xFFFFFFFF);

			foreach (Circle circle in Shapes2DState.Circles)
				positionedDrawList.AddCircle(circle, 0xFFFFFFFF);

			foreach (Rectangle rectangle in Shapes2DState.Rectangles)
				positionedDrawList.AddRectangle(rectangle, 0xFFFFFFFF);

			foreach (OrientedRectangle orientedRectangle in Shapes2DState.OrientedRectangles)
				positionedDrawList.AddOrientedRectangle(orientedRectangle, 0xFFFFFFFF);
		}

		ImGui.EndChild();
	}

	private static void CreateShape(Vector2 start, Vector2 end)
	{
		switch (Shapes2DState.SelectedShapeType)
		{
			case SelectedShapeType.LineSegment2D: Shapes2DState.LineSegments.Add(new LineSegment2D(start, end)); break;
			case SelectedShapeType.Circle: Shapes2DState.Circles.Add(CreateCircle(start, end)); break;
			case SelectedShapeType.Rectangle: Shapes2DState.Rectangles.Add(Rectangle.FromMinMax(start, end)); break;
			case SelectedShapeType.OrientedRectangle: Shapes2DState.OrientedRectangles.Add(CreateOrientedRectangle(start, end)); break;
		}
	}

	private static Circle CreateCircle(Vector2 start, Vector2 end)
	{
		Vector2 average = (start + end) / 2;
		float radius = Vector2.Distance(start, average);
		return new Circle(average, radius);
	}

	private static OrientedRectangle CreateOrientedRectangle(Vector2 start, Vector2 end)
	{
		Vector2 average = (start + end) / 2;
		Vector2 halfExtents = (end - start) / 2;
		float rotation = MathF.Atan2(end.Y - start.Y, end.X - start.X);
		return new OrientedRectangle(average, halfExtents, rotation);
	}
}
