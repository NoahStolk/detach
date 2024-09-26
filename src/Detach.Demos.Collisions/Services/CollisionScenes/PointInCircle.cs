using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.CollisionScenes;

public sealed class PointInCircle : ICollisionScene
{
	private const float _pointOffset = 64;
	private const float _circleOffset = 128;

	private float _totalTime;
	private Vector2 _point;
	private Circle _circle;
	private bool _collision;

	public void Update(float dt)
	{
		_totalTime += dt;
		float doubleTime = _totalTime * 2;

		_point = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);
		_circle = new Circle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(_totalTime) * _circleOffset, MathF.Sin(_totalTime) * _circleOffset),
			64 + MathF.Sin(_totalTime) * 32);

		_collision = Geometry2D.PointInCircle(_point, _circle);
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
		drawList.AddCircle(origin + _circle.Position, _circle.Radius, foregroundColor);

		drawList.AddText(origin + _point, textColor, Inline.Span($"Point: {_point.X:0.00} {_point.Y:0.00}"));
		drawList.AddText(origin + _circle.Position, textColor, Inline.Span($"Circle position: {_circle.Position.X:0.00} {_circle.Position.Y:0.00}"));
		drawList.AddText(origin + _circle.Position + new Vector2(0, _circle.Radius), textColor, Inline.Span($"Circle radius: {_circle.Radius:0.00}"));
	}
}
