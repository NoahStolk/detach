using Detach.Buffers;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.CollisionScenes;

public sealed class PointInOrientedRectangle : ICollisionScene
{
	private const float _pointOffset = 64;
	private const float _rectangleOffset = 128;

	private float _totalTime;
	private Vector2 _point;
	private OrientedRectangle _orientedRectangle;
	private bool _collision;

	public void Update(float dt)
	{
		_totalTime += dt;
		float doubleTime = _totalTime * 2;

		_point = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);

		Vector2 center = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(_totalTime) * _rectangleOffset, MathF.Sin(_totalTime) * _rectangleOffset);
		Vector2 halfExtents = new(64 + MathF.Sin(_totalTime) * 32, 32 + MathF.Cos(_totalTime) * 16);
		float rotationInRadians = _totalTime * 1.5f;
		_orientedRectangle = new OrientedRectangle(center, halfExtents, rotationInRadians);

		_collision = Geometry2D.PointInOrientedRectangle(_point, _orientedRectangle);
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
		Buffer4<Vector2> vertices = _orientedRectangle.GetVertices();
		drawList.AddQuad(origin + vertices[0], origin + vertices[1], origin + vertices[2], origin + vertices[3], foregroundColor);

		drawList.AddText(origin + _point, textColor, Inline.Span($"Point: {_point.X:0.00} {_point.Y:0.00}"));
		drawList.AddText(origin + _orientedRectangle.Center, textColor, Inline.Span($"Rectangle center: {_orientedRectangle.Center.X:0.00} {_orientedRectangle.Center.Y:0.00}"));
		drawList.AddText(origin + _orientedRectangle.Center + _orientedRectangle.HalfExtents, textColor, Inline.Span($"Rectangle half extents: {_orientedRectangle.HalfExtents.X:0.00} {_orientedRectangle.HalfExtents.Y:0.00}"));
		drawList.AddText(origin + _orientedRectangle.Center + _orientedRectangle.HalfExtents with { X = 0 }, textColor, Inline.Span($"Rectangle rotation: {_orientedRectangle.RotationInRadians:0.00}"));
	}
}
