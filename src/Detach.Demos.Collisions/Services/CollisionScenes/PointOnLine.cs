using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.CollisionScenes;

public sealed class PointOnLine : ICollisionScene
{
	private const float _pointOffset = 64;
	private const float _linePointOffset = 128;

	private float _totalTime;
	private Vector2 _point;
	private LineSegment2D _line;
	private bool _collision;

	public void Update(float dt)
	{
		_totalTime += dt;
		_point = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(_totalTime) * _pointOffset, MathF.Sin(_totalTime) * _pointOffset);

		float halfTime = _totalTime / 2;
		float quarterTime = _totalTime / 4;
		_line = new LineSegment2D(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime) * _linePointOffset, MathF.Sin(halfTime) * _linePointOffset),
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(quarterTime) * _linePointOffset, MathF.Sin(quarterTime) * _linePointOffset));

		_collision = Geometry2D.PointOnLine(_point, _line, 1f);
	}

	public void Render()
	{
		const uint textColor = 0xFFFFFFFF;
		const uint backgroundColor = 0xFF000000;
		uint foregroundColor = _collision ? 0xFF00FF00 : 0xFFFFFFFF;

		Vector2 origin = ImGui.GetCursorScreenPos();

		ImDrawListPtr drawList = ImGui.GetWindowDrawList();
		drawList.AddRectFilled(origin, origin + CollisionSceneConstants.Size, backgroundColor);
		drawList.AddCircleFilled(origin + _point, 3, foregroundColor);
		drawList.AddLine(origin + _line.Start, origin + _line.End, foregroundColor);

		drawList.AddText(origin + _point, textColor, Inline.Span($"Point: {_point.X:0.00} {_point.Y:0.00}"));
		drawList.AddText(origin + _line.Start, textColor, Inline.Span($"Line start: {_line.Start.X:0.00} {_line.Start.Y:0.00}"));
		drawList.AddText(origin + _line.End, textColor, Inline.Span($"Line end: {_line.End.X:0.00} {_line.End.Y:0.00}"));
	}
}
