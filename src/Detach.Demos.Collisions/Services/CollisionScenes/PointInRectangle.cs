using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.CollisionScenes;

public sealed class PointInRectangle : ICollisionScene
{
	private const float _pointOffset = 64;
	private const float _rectangleOffset = 128;

	private float _totalTime;
	private Vector2 _point;
	private Rectangle _rectangle;
	private bool _collision;

	public void Update(float dt)
	{
		_totalTime += dt;
		float doubleTime = _totalTime * 2;

		_point = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);

		Vector2 center = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(_totalTime) * _rectangleOffset, MathF.Sin(_totalTime) * _rectangleOffset);
		Vector2 size = new(160 + MathF.Sin(_totalTime) * 32);
		_rectangle = Rectangle.FromCenter(center, size);

		_collision = Geometry2D.PointInRectangle(_point, _rectangle);
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
		drawList.AddRect(origin + _rectangle.Position, origin + _rectangle.Position + _rectangle.Size, foregroundColor);

		drawList.AddText(origin + _point, textColor, Inline.Span($"Point: {_point.X:0.00} {_point.Y:0.00}"));
		drawList.AddText(origin + _rectangle.Position, textColor, Inline.Span($"Rectangle position: {_rectangle.Position.X:0.00} {_rectangle.Position.Y:0.00}"));
		drawList.AddText(origin + _rectangle.Position + _rectangle.Size, textColor, Inline.Span($"Rectangle size: {_rectangle.Size.X:0.00} {_rectangle.Size.Y:0.00}"));
	}
}
