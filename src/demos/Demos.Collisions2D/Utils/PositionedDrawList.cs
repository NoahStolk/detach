using Detach;
using Detach.Buffers;
using Detach.Collisions.Primitives2D;
using Detach.Utils;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions2D.Utils;

internal readonly record struct PositionedDrawList(ImDrawListPtr DrawList, Vector2 Origin)
{
	private const uint _textColor = 0xFFFFFFFF;
	private const uint _foregroundColor = 0xFFFFFFFF;
	private const uint _foregroundColorCollision = 0xFF00FF00;
	private const uint _backgroundColor = 0xFF000000;

	private static uint GetForegroundColor(bool hasCollision)
	{
		return hasCollision ? _foregroundColorCollision : _foregroundColor;
	}

	public void AddBackground(Vector2 size)
	{
		DrawList.AddRectFilled(Origin, Origin + size, _backgroundColor);
	}

	public void AddLine(LineSegment2D lineSegment, bool hasCollision)
	{
		DrawList.AddLine(Origin + lineSegment.Start, Origin + lineSegment.End, GetForegroundColor(hasCollision));

		DrawList.AddText(Origin + lineSegment.Start, _textColor, Inline.Utf8($"Line start: {lineSegment.Start.X:0.00} {lineSegment.Start.Y:0.00}"));
		DrawList.AddText(Origin + lineSegment.End, _textColor, Inline.Utf8($"Line end: {lineSegment.End.X:0.00} {lineSegment.End.Y:0.00}"));
	}

	public void AddCircle(Circle circle, bool hasCollision)
	{
		DrawList.AddCircle(Origin + circle.Center, circle.Radius, GetForegroundColor(hasCollision));

		DrawList.AddText(Origin + circle.Center, _textColor, Inline.Utf8($"Circle position: {circle.Center.X:0.00} {circle.Center.Y:0.00}"));
		DrawList.AddText(Origin + circle.Center + new Vector2(0, circle.Radius), _textColor, Inline.Utf8($"Circle radius: {circle.Radius:0.00}"));
	}

	public void AddCircleCast(CircleCast circleCast, bool hasCollision)
	{
		DrawList.AddCircle(Origin + circleCast.Start, circleCast.Radius, GetForegroundColor(hasCollision));
		DrawList.AddCircle(Origin + circleCast.End, circleCast.Radius, GetForegroundColor(hasCollision));
		DrawList.AddLine(Origin + circleCast.Start, Origin + circleCast.End, GetForegroundColor(hasCollision));

		Vector2 direction = Vector2.Normalize(circleCast.End - circleCast.Start);
		Vector2 rotated = VectorUtils.RotateVector(direction, MathF.PI * 0.5f) * circleCast.Radius;
		DrawList.AddLine(Origin + circleCast.Start + rotated, Origin + circleCast.End + rotated, GetForegroundColor(hasCollision));
		DrawList.AddLine(Origin + circleCast.Start - rotated, Origin + circleCast.End - rotated, GetForegroundColor(hasCollision));

		DrawList.AddText(Origin + circleCast.Start, _textColor, Inline.Utf8($"Circle cast start: {circleCast.Start.X:0.00} {circleCast.Start.Y:0.00}"));
		DrawList.AddText(Origin + circleCast.End, _textColor, Inline.Utf8($"Circle cast end: {circleCast.End.X:0.00} {circleCast.End.Y:0.00}"));
		DrawList.AddText(Origin + circleCast.Start + circleCast.Radius * new Vector2(0, 1), _textColor, Inline.Utf8($"Circle cast radius: {circleCast.Radius:0.00}"));
	}

	public void AddPoint(Vector2 point, bool hasCollision)
	{
		DrawList.AddCircleFilled(Origin + point, 3, GetForegroundColor(hasCollision));

		DrawList.AddText(Origin + point, _textColor, Inline.Utf8($"Point: {point.X:0.00} {point.Y:0.00}"));
	}

	public void AddRectangle(Rectangle rectangle, bool hasCollision)
	{
		DrawList.AddRect(Origin + rectangle.Position, Origin + rectangle.Position + rectangle.Size, GetForegroundColor(hasCollision));

		DrawList.AddText(Origin + rectangle.Position, _textColor, Inline.Utf8($"Rectangle position: {rectangle.Position.X:0.00} {rectangle.Position.Y:0.00}"));
		DrawList.AddText(Origin + rectangle.Position + rectangle.Size, _textColor, Inline.Utf8($"Rectangle size: {rectangle.Size.X:0.00} {rectangle.Size.Y:0.00}"));
	}

	public void AddOrientedRectangle(OrientedRectangle orientedRectangle, bool hasCollision)
	{
		Buffer4<Vector2> vertices = orientedRectangle.GetVertices();
		DrawList.AddQuad(Origin + vertices[0], Origin + vertices[1], Origin + vertices[2], Origin + vertices[3], GetForegroundColor(hasCollision));

		DrawList.AddText(Origin + orientedRectangle.Center, _textColor, Inline.Utf8($"Rectangle center: {orientedRectangle.Center.X:0.00} {orientedRectangle.Center.Y:0.00}"));
		DrawList.AddText(Origin + orientedRectangle.Center + orientedRectangle.HalfExtents, _textColor, Inline.Utf8($"Rectangle half extents: {orientedRectangle.HalfExtents.X:0.00} {orientedRectangle.HalfExtents.Y:0.00}"));
		DrawList.AddText(Origin + orientedRectangle.Center - orientedRectangle.HalfExtents with { X = 0 }, _textColor, Inline.Utf8($"Rectangle rotation: {orientedRectangle.RotationInRadians:0.00}"));
	}

	public void AddTriangle(Triangle2D triangle2D, bool hasCollision)
	{
		DrawList.AddTriangle(Origin + triangle2D.A, Origin + triangle2D.B, Origin + triangle2D.C, GetForegroundColor(hasCollision));

		DrawList.AddText(Origin + triangle2D.A, _textColor, Inline.Utf8($"Triangle A: {triangle2D.A.X:0.00} {triangle2D.A.Y:0.00}"));
		DrawList.AddText(Origin + triangle2D.B, _textColor, Inline.Utf8($"Triangle B: {triangle2D.B.X:0.00} {triangle2D.B.Y:0.00}"));
		DrawList.AddText(Origin + triangle2D.C, _textColor, Inline.Utf8($"Triangle C: {triangle2D.C.X:0.00} {triangle2D.C.Y:0.00}"));
	}
}
