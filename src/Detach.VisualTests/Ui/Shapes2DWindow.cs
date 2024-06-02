﻿using Detach.Collisions.Primitives;
using Detach.VisualTests.Collisions;
using Detach.VisualTests.State;
using ImGuiNET;
using System.Numerics;
using System.Text;

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

			if (ImGui.CollapsingHeader("Shapes"))
			{
				// TODO: Allow deleting shapes.
				ShapeTables.RenderLineSegment2Ds();
				ShapeTables.RenderCircles();
				ShapeTables.RenderRectangles();
				ShapeTables.RenderOrientedRectangles();
			}

			CollisionHandler.PerformCollisions();
			ImGui.InputTextMultiline("Test code", ref _unitTestCode, 1024, new Vector2(0, 384));
			if (ImGui.Button("Generate test code"))
				_unitTestCode = BuildTestCode();

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

	private static string BuildTestCode()
	{
		StringBuilder sb = new();

		for (int i = 0; i < Shapes2DState.LineSegments.Count; i++)
		{
			LineSegment2D lineSegment = Shapes2DState.LineSegments[i];
			sb.AppendLine($"LineSegment2D {GetLocalName(lineSegment, i)} = new(new Vector2({lineSegment.Start.X}f, {lineSegment.Start.Y}f), new Vector2({lineSegment.End.X}f, {lineSegment.End.Y}f));");
		}

		for (int i = 0; i < Shapes2DState.Circles.Count; i++)
		{
			Circle circle = Shapes2DState.Circles[i];
			sb.AppendLine($"Circle {GetLocalName(circle, i)} = new(new Vector2({circle.Position.X}f, {circle.Position.Y}f), {circle.Radius}f);");
		}

		for (int i = 0; i < Shapes2DState.Rectangles.Count; i++)
		{
			Rectangle rectangle = Shapes2DState.Rectangles[i];
			sb.AppendLine($"Rectangle {GetLocalName(rectangle, i)} = new(new Vector2({rectangle.Position.X}f, {rectangle.Position.Y}f), new Vector2({rectangle.Size.X}f, {rectangle.Size.Y}f));");
		}

		for (int i = 0; i < Shapes2DState.OrientedRectangles.Count; i++)
		{
			OrientedRectangle orientedRectangle = Shapes2DState.OrientedRectangles[i];
			sb.AppendLine($"OrientedRectangle {GetLocalName(orientedRectangle, i)} = new(new Vector2({orientedRectangle.Position.X}f, {orientedRectangle.Position.Y}f), new Vector2({orientedRectangle.HalfExtents.X}f, {orientedRectangle.HalfExtents.Y}f), {orientedRectangle.RotationInRadians}f);");
		}

		foreach (CollisionResult cr in CollisionHandler.Collisions)
		{
			string assertion = cr.IsColliding ? "Assert.IsTrue" : "Assert.IsFalse";
			string functionName = $"{GetTypeName(cr.A)}{GetTypeName(cr.B)}";
			if (functionName is "RectangleOrientedRectangle" or "OrientedRectangleOrientedRectangle")
				functionName += "Sat";

			string aName = GetLocalName(cr.A, cr.IndexA);
			string bName = GetLocalName(cr.B, cr.IndexB);
			sb.AppendLine($"{assertion}(Geometry2D.{functionName}({aName}, {bName}));");
		}

		return sb.ToString();

		static string GetLocalName(object obj, int index)
		{
			return FirstCharToLower(GetTypeName(obj)) + index;
		}

		static string GetTypeName(object obj)
		{
			return obj switch
			{
				LineSegment2D => "Line",
				Circle => "Circle",
				Rectangle => "Rectangle",
				OrientedRectangle => "OrientedRectangle",
				_ => "Unknown",
			};
		}

		static string FirstCharToLower(string str)
		{
			return str.Length switch
			{
				0 => string.Empty,
				1 => char.ToLower(str[0]).ToString(),
				_ => char.ToLower(str[0]) + str[1..],
			};
		}
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
