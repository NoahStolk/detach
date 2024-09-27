using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Detach.Demos.Collisions.Utils;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.Services.CollisionScenes;

public sealed class LineCircle : CollisionScene<LineSegment2D, Circle>
{
	private const float _linePointOffsetA = 64;
	private const float _circleOffset = 128;

	public LineCircle()
		: base(Geometry2D.LineCircle)
	{
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		A = new LineSegment2D(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(quarterTime) * _linePointOffsetA, MathF.Sin(quarterTime) * _linePointOffsetA),
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _linePointOffsetA, MathF.Sin(halfTime) * _linePointOffsetA));
		B = new Circle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _circleOffset, MathF.Sin(TotalTime) * _circleOffset),
			64 + MathF.Sin(TotalTime) * 32);
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddLine(A, HasCollision);
		drawList.AddCircle(B, HasCollision);

		Vector2 ab = A.End - A.Start;
		float t = Vector2.Dot(B.Position - A.Start, ab) / Vector2.Dot(ab, ab);

		drawList.DrawList.AddText(drawList.Origin, 0xFFFFFFFF, $"t: {t}");
		Vector2 closestPoint = A.Start + ab * t;
		drawList.DrawList.AddCircleFilled(drawList.Origin + closestPoint, 3, 0xFF0000FF);

		LineSegment2D circleToClosest = new(B.Position, closestPoint);
		drawList.DrawList.AddLine(drawList.Origin + circleToClosest.Start, drawList.Origin + circleToClosest.End, 0xFF0000FF);
	}
}
