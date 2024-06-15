using Detach.Collisions.Primitives2D;
using Detach.VisualTests.State;
using ImGuiNET;

namespace Detach.VisualTests.Ui;

public static class ShapeTables
{
	public static void RenderLineSegment2Ds()
	{
		if (ImGui.BeginTable("LineSegment2Ds", 3))
		{
			ImGui.TableSetupColumn("Id", ImGuiTableColumnFlags.WidthFixed);
			ImGui.TableSetupColumn("Start", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableSetupColumn("End", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableHeadersRow();

			for (int i = 0; i < Shapes2DState.LineSegments.Count; i++)
			{
				LineSegment2D lineSegment = Shapes2DState.LineSegments[i];
				ImGui.TableNextRow();

				ImGui.TableNextColumn();
				ImGui.Text(Inline.Span(i));

				ImGui.TableNextColumn();
				if (ImGui.SliderFloat2(Inline.Span($"Start##{i}"), ref lineSegment.Start, 0, 1024, "%.0f"))
					Shapes2DState.LineSegments[i] = lineSegment;

				ImGui.TableNextColumn();
				if (ImGui.SliderFloat2(Inline.Span($"End##{i}"), ref lineSegment.End, 0, 1024, "%.0f"))
					Shapes2DState.LineSegments[i] = lineSegment;
			}

			ImGui.EndTable();
		}
	}

	public static void RenderCircles()
	{
		if (ImGui.BeginTable("Circles", 3))
		{
			ImGui.TableSetupColumn("Id", ImGuiTableColumnFlags.WidthFixed);
			ImGui.TableSetupColumn("Position", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableSetupColumn("Radius", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableHeadersRow();

			for (int i = 0; i < Shapes2DState.Circles.Count; i++)
			{
				Circle circle = Shapes2DState.Circles[i];
				ImGui.TableNextRow();

				ImGui.TableNextColumn();
				ImGui.Text(Inline.Span(i));

				ImGui.TableNextColumn();
				if (ImGui.SliderFloat2(Inline.Span($"Position##{i}"), ref circle.Position, 0, 1024, "%.0f"))
					Shapes2DState.Circles[i] = circle;

				ImGui.TableNextColumn();
				if (ImGui.SliderFloat(Inline.Span($"Radius##{i}"), ref circle.Radius, 0, 256, "%.0f"))
					Shapes2DState.Circles[i] = circle;
			}

			ImGui.EndTable();
		}
	}

	public static void RenderRectangles()
	{
		if (ImGui.BeginTable("Rectangles", 3))
		{
			ImGui.TableSetupColumn("Id", ImGuiTableColumnFlags.WidthFixed);
			ImGui.TableSetupColumn("Position", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableSetupColumn("Size", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableHeadersRow();

			for (int i = 0; i < Shapes2DState.Rectangles.Count; i++)
			{
				Rectangle rectangle = Shapes2DState.Rectangles[i];
				ImGui.TableNextRow();

				ImGui.TableNextColumn();
				ImGui.Text(Inline.Span(i));

				ImGui.TableNextColumn();
				if (ImGui.SliderFloat2(Inline.Span($"Position##{i}"), ref rectangle.Position, 0, 1024, "%.0f"))
					Shapes2DState.Rectangles[i] = rectangle;

				ImGui.TableNextColumn();
				if (ImGui.SliderFloat2(Inline.Span($"Size##{i}"), ref rectangle.Size, 0, 256, "%.0f"))
					Shapes2DState.Rectangles[i] = rectangle;
			}

			ImGui.EndTable();
		}
	}

	public static void RenderOrientedRectangles()
	{
		if (ImGui.BeginTable("OrientedRectangles", 4))
		{
			ImGui.TableSetupColumn("Id", ImGuiTableColumnFlags.WidthFixed);
			ImGui.TableSetupColumn("Position", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableSetupColumn("HalfExtents", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableSetupColumn("Rotation", ImGuiTableColumnFlags.WidthStretch);
			ImGui.TableHeadersRow();

			for (int i = 0; i < Shapes2DState.OrientedRectangles.Count; i++)
			{
				OrientedRectangle orientedRectangle = Shapes2DState.OrientedRectangles[i];
				ImGui.TableNextRow();

				ImGui.TableNextColumn();
				ImGui.Text(Inline.Span(i));

				ImGui.TableNextColumn();
				if (ImGui.SliderFloat2(Inline.Span($"Position##{i}"), ref orientedRectangle.Position, 0, 256, "%.0f"))
					Shapes2DState.OrientedRectangles[i] = orientedRectangle;

				ImGui.TableNextColumn();
				if (ImGui.SliderFloat2(Inline.Span($"HalfExtents##{i}"), ref orientedRectangle.HalfExtents, 0, 256, "%.0f"))
					Shapes2DState.OrientedRectangles[i] = orientedRectangle;

				ImGui.TableNextColumn();
				if (ImGui.SliderFloat(Inline.Span($"Rotation##{i}"), ref orientedRectangle.RotationInRadians, -MathF.PI, MathF.PI, "%.2f"))
					Shapes2DState.OrientedRectangles[i] = orientedRectangle;
			}

			ImGui.EndTable();
		}
	}
}
