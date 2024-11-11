using Demos.Collisions.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Demos.Collisions.CollisionScenes.TwoDimensional;

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
	}
}
