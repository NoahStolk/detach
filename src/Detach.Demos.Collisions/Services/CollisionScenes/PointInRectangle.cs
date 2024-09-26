using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.CollisionScenes;

public sealed class PointInRectangle : CollisionScene<Vector2, Rectangle>
{
	private const float _pointOffset = 64;
	private const float _rectangleOffset = 128;

	public override void Update(float dt)
	{
		base.Update(dt);

		float doubleTime = TotalTime * 2;

		A = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);

		Vector2 center = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _rectangleOffset, MathF.Sin(TotalTime) * _rectangleOffset);
		Vector2 size = new(160 + MathF.Sin(TotalTime) * 32);
		B = Rectangle.FromCenter(center, size);

		HasCollision = Geometry2D.PointInRectangle(A, B);
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
		drawList.AddRect(origin + B.Position, origin + B.Position + B.Size, foregroundColor);

		drawList.AddText(origin + A, textColor, Inline.Span($"Point: {A.X:0.00} {A.Y:0.00}"));
		drawList.AddText(origin + B.Position, textColor, Inline.Span($"Rectangle position: {B.Position.X:0.00} {B.Position.Y:0.00}"));
		drawList.AddText(origin + B.Position + B.Size, textColor, Inline.Span($"Rectangle size: {B.Size.X:0.00} {B.Size.Y:0.00}"));
	}
}
