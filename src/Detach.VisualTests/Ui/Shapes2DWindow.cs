using Detach.Collisions.Primitives;
using Detach.Utils;
using Detach.VisualTests.Collisions;
using Detach.VisualTests.State;
using Detach.VisualTests.TestCodeGenerator;
using ImGuiNET;
using System.Numerics;

namespace Detach.VisualTests.Ui;

// ReSharper disable ForCanBeConvertedToForeach
public static class Shapes2DWindow
{
	private static string _unitTestCode = string.Empty;

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
		if (ImGui.BeginChild("Shapes 2D Side Menu", new Vector2(768, 0), ImGuiChildFlags.Border))
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

			// TODO: More controls:
			// - Allow deleting shapes.
			// - Allow rotating oriented rectangles.
			if (ImGui.CollapsingHeader("Shapes"))
			{
				ShapeTables.RenderLineSegment2Ds();
				ShapeTables.RenderCircles();
				ShapeTables.RenderRectangles();
				ShapeTables.RenderOrientedRectangles();
			}

			CollisionHandler.PerformCollisions();
			ImGui.InputTextMultiline("Test code", ref _unitTestCode, 1024, new Vector2(0, 384));
			if (ImGui.Button("Generate test code"))
				_unitTestCode = Generator.BuildTestCode();

			if (ImGui.CollapsingHeader("Collisions"))
			{
				foreach (CollisionResult collision in CollisionHandler.Collisions)
				{
					ImGui.TextColored(collision.IsColliding ? new Vector4(1, 0, 0.5f, 1) : Vector4.One, $"{collision.A} vs {collision.B}");
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

			if (Shapes2DState.IsCreatingShape)
			{
				const uint color = 0xBBFFFFFF;
				switch (Shapes2DState.SelectedShapeType)
				{
					case SelectedShapeType.LineSegment2D: positionedDrawList.AddLine(CreateLineSegment(Shapes2DState.ShapeStart, relativeMousePosition), color); break;
					case SelectedShapeType.Circle: positionedDrawList.AddCircle(CreateCircle(Shapes2DState.ShapeStart, relativeMousePosition), color); break;
					case SelectedShapeType.Rectangle: positionedDrawList.AddRectangle(Rectangle.FromMinMax(Shapes2DState.ShapeStart, relativeMousePosition), color); break;
					case SelectedShapeType.OrientedRectangle: positionedDrawList.AddOrientedRectangle(CreateOrientedRectangle(Shapes2DState.ShapeStart, relativeMousePosition), color); break;
				}

				positionedDrawList.AddCircle(new Circle(Shapes2DState.ShapeStart, 5), 0xFF0000FF);

				if (ImGui.BeginTooltip())
				{
					Vector2 direction = relativeMousePosition - Shapes2DState.ShapeStart;
					ImGui.Text(direction.ToString());
					ImGui.Text(VectorUtils.GetAngleFrom(direction).ToString("0.00"));

					ImGui.EndTooltip();
				}
			}

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
			case SelectedShapeType.LineSegment2D: Shapes2DState.LineSegments.Add(CreateLineSegment(start, end)); break;
			case SelectedShapeType.Circle: Shapes2DState.Circles.Add(CreateCircle(start, end)); break;
			case SelectedShapeType.Rectangle: Shapes2DState.Rectangles.Add(Rectangle.FromMinMax(start, end)); break;
			case SelectedShapeType.OrientedRectangle: Shapes2DState.OrientedRectangles.Add(CreateOrientedRectangle(start, end)); break;
		}
	}

	private static LineSegment2D CreateLineSegment(Vector2 start, Vector2 end)
	{
		return new LineSegment2D(start, end);
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
		return new OrientedRectangle(average, halfExtents, 0);
	}
}
