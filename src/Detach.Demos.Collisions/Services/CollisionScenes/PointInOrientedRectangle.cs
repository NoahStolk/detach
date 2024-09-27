using Detach.Buffers;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.CollisionScenes;

public sealed class PointInOrientedRectangle : CollisionScene<Vector2, OrientedRectangle>
{
	private const float _pointOffset = 64;
	private const float _rectangleOffset = 128;

	public PointInOrientedRectangle()
		: base(Geometry2D.PointInOrientedRectangle)
	{
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float doubleTime = TotalTime * 2;

		A = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);

		Vector2 center = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _rectangleOffset, MathF.Sin(TotalTime) * _rectangleOffset);
		Vector2 halfExtents = new(64 + MathF.Sin(TotalTime) * 32, 32 + MathF.Cos(TotalTime) * 16);
		float rotationInRadians = TotalTime * 1.5f;
		B = new OrientedRectangle(center, halfExtents, rotationInRadians);
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
		Buffer4<Vector2> vertices = B.GetVertices();
		drawList.AddQuad(origin + vertices[0], origin + vertices[1], origin + vertices[2], origin + vertices[3], foregroundColor);

		drawList.AddText(origin + A, textColor, Inline.Span($"Point: {A.X:0.00} {A.Y:0.00}"));
		drawList.AddText(origin + B.Center, textColor, Inline.Span($"Rectangle center: {B.Center.X:0.00} {B.Center.Y:0.00}"));
		drawList.AddText(origin + B.Center + B.HalfExtents, textColor, Inline.Span($"Rectangle half extents: {B.HalfExtents.X:0.00} {B.HalfExtents.Y:0.00}"));
		drawList.AddText(origin + B.Center + B.HalfExtents with { X = 0 }, textColor, Inline.Span($"Rectangle rotation: {B.RotationInRadians:0.00}"));
	}
}
