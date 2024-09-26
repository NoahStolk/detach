using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.CollisionScenes;

public sealed class PointInTriangle : ICollisionScene
{
	private const float _pointOffset = 96;
	private const float _triangleSize = 128;

	private float _totalTime;
	private Vector2 _point;
	private Triangle2D _triangle;
	private bool _collision;

	public void Update(float dt)
	{
		_totalTime += dt;
		float doubleTime = _totalTime * 2;

		_point = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);

		Vector2 triangleOffset = new(48, 24);
		Vector2 a = CollisionSceneConstants.Origin + triangleOffset + new Vector2(MathF.Cos(_totalTime) * _triangleSize, MathF.Sin(_totalTime) * _triangleSize);
		Vector2 b = CollisionSceneConstants.Origin + triangleOffset + new Vector2(MathF.Cos(_totalTime + MathF.PI * 2 / 3) * _triangleSize, MathF.Sin(_totalTime + MathF.PI * 2 / 3) * _triangleSize);
		Vector2 c = CollisionSceneConstants.Origin + triangleOffset + new Vector2(MathF.Cos(_totalTime + MathF.PI * 4 / 3) * _triangleSize, MathF.Sin(_totalTime + MathF.PI * 4 / 3) * _triangleSize);
		_triangle = new Triangle2D(a, b, c);

		_collision = Geometry2D.PointInTriangle(_point, _triangle);
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
		drawList.AddTriangle(origin + _triangle.A, origin + _triangle.B, origin + _triangle.C, foregroundColor);

		drawList.AddText(origin + _point, textColor, Inline.Span($"Point: {_point.X:0.00} {_point.Y:0.00}"));
		drawList.AddText(origin + _triangle.A, textColor, Inline.Span($"Triangle A: {_triangle.A.X:0.00} {_triangle.A.Y:0.00}"));
		drawList.AddText(origin + _triangle.B, textColor, Inline.Span($"Triangle B: {_triangle.B.X:0.00} {_triangle.B.Y:0.00}"));
		drawList.AddText(origin + _triangle.C, textColor, Inline.Span($"Triangle C: {_triangle.C.X:0.00} {_triangle.C.Y:0.00}"));
	}
}
