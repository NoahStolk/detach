using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.CollisionScenes;

public sealed class PointOnLine : CollisionScene<Vector2, LineSegment2D>
{
	private const float _pointOffset = 64;
	private const float _linePointOffset = 128;

	public override void Update(float dt)
	{
		base.Update(dt);

		A = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _pointOffset, MathF.Sin(TotalTime) * _pointOffset);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		B = new LineSegment2D(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime) * _linePointOffset, MathF.Sin(halfTime) * _linePointOffset),
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(quarterTime) * _linePointOffset, MathF.Sin(quarterTime) * _linePointOffset));

		HasCollision = Geometry2D.PointOnLine(A, B, 1f);
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
		drawList.AddLine(origin + B.Start, origin + B.End, foregroundColor);

		drawList.AddText(origin + A, textColor, Inline.Span($"Point: {A.X:0.00} {A.Y:0.00}"));
		drawList.AddText(origin + B.Start, textColor, Inline.Span($"Line start: {B.Start.X:0.00} {B.Start.Y:0.00}"));
		drawList.AddText(origin + B.End, textColor, Inline.Span($"Line end: {B.End.X:0.00} {B.End.Y:0.00}"));
	}
}
