using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.CollisionScenes;

public sealed class PointInCircle : CollisionScene<Vector2, Circle>
{
	private const float _pointOffset = 64;
	private const float _circleOffset = 128;

	public PointInCircle()
		: base(Geometry2D.PointInCircle)
	{
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float doubleTime = TotalTime * 2;

		A = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);
		B = new Circle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _circleOffset, MathF.Sin(TotalTime) * _circleOffset),
			64 + MathF.Sin(TotalTime) * 32);
	}

	public override void Render()
	{
		const uint textColor = 0xFFFFFFFF;
		const uint backgroundColor = 0xFF000000;
		uint foregroundColor = HasCollision ? 0xFF00FF00 : 0xFFFFFFFF;

		Vector2 origin = ImGui.GetCursorScreenPos();

		ImDrawListPtr drawList = ImGui.GetWindowDrawList();
		drawList.AddRectFilled(origin, origin + CollisionSceneConstants.Size, backgroundColor);
		drawList.AddCircleFilled(origin + A, 3, foregroundColor);
		drawList.AddCircle(origin + B.Position, B.Radius, foregroundColor);

		drawList.AddText(origin + A, textColor, Inline.Span($"Point: {A.X:0.00} {A.Y:0.00}"));
		drawList.AddText(origin + B.Position, textColor, Inline.Span($"Circle position: {B.Position.X:0.00} {B.Position.Y:0.00}"));
		drawList.AddText(origin + B.Position + new Vector2(0, B.Radius), textColor, Inline.Span($"Circle radius: {B.Radius:0.00}"));
	}
}
