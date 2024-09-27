using Detach.Buffers;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Detach.VisualTests.Ui;

public readonly record struct PositionedDrawList(ImDrawListPtr DrawList, Vector2 Origin)
{
	public void AddLine(LineSegment2D lineSegment, uint color)
	{
		DrawList.AddLine(Origin + lineSegment.Start, Origin + lineSegment.End, color);
	}

	public void AddCircle(Circle circle, uint color)
	{
		DrawList.AddCircle(Origin + circle.Position, circle.Radius, color);
	}

	public void AddRectangle(Rectangle rectangle, uint color)
	{
		Vector2 min = rectangle.GetMin();
		Vector2 max = rectangle.GetMax();
		DrawList.AddRect(Origin + min, Origin + max, color);
	}

	public void AddOrientedRectangle(OrientedRectangle orientedRectangle, uint color)
	{
		Buffer4<Vector2> verticesBuffer = orientedRectangle.GetVertices();
		for (int i = 0; i < 4; i++)
			verticesBuffer[i] += Origin;

		Span<Vector2> vertices = MemoryMarshal.CreateSpan(ref Unsafe.As<Buffer4<Vector2>, Vector2>(ref verticesBuffer), 4);
		DrawList.AddPolyline(ref vertices[0], vertices.Length, color, ImDrawFlags.Closed, 1);
	}
}
