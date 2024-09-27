using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.CollisionScenes;

public sealed class PointInTriangle : CollisionScene<Vector2, Triangle2D>
{
	private const float _pointOffset = 96;
	private const float _triangleSize = 128;

	public PointInTriangle()
		: base(Geometry2D.PointInTriangle)
	{
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float doubleTime = TotalTime * 2;

		A = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);

		Vector2 triangleOffset = new(48, 24);
		Vector2 a = CollisionSceneConstants.Origin + triangleOffset + new Vector2(MathF.Cos(TotalTime) * _triangleSize, MathF.Sin(TotalTime) * _triangleSize);
		Vector2 b = CollisionSceneConstants.Origin + triangleOffset + new Vector2(MathF.Cos(TotalTime + MathF.PI * 2 / 3) * _triangleSize, MathF.Sin(TotalTime + MathF.PI * 2 / 3) * _triangleSize);
		Vector2 c = CollisionSceneConstants.Origin + triangleOffset + new Vector2(MathF.Cos(TotalTime + MathF.PI * 4 / 3) * _triangleSize, MathF.Sin(TotalTime + MathF.PI * 4 / 3) * _triangleSize);
		B = new Triangle2D(a, b, c);
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
		drawList.AddTriangle(origin + B.A, origin + B.B, origin + B.C, foregroundColor);

		drawList.AddText(origin + A, textColor, Inline.Span($"Point: {A.X:0.00} {A.Y:0.00}"));
		drawList.AddText(origin + B.A, textColor, Inline.Span($"Triangle A: {B.A.X:0.00} {B.A.Y:0.00}"));
		drawList.AddText(origin + B.B, textColor, Inline.Span($"Triangle B: {B.B.X:0.00} {B.B.Y:0.00}"));
		drawList.AddText(origin + B.C, textColor, Inline.Span($"Triangle C: {B.C.X:0.00} {B.C.Y:0.00}"));
	}
}
