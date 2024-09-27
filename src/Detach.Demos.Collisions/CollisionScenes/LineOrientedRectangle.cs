using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Detach.Demos.Collisions.Utils;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.CollisionScenes;

public sealed class LineOrientedRectangle : CollisionScene<LineSegment2D, OrientedRectangle>
{
	private const float _linePointOffsetA = 64;
	private const float _rectangleOffset = 128;

	public LineOrientedRectangle()
		: base(Geometry2D.LineOrientedRectangle)
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
		B = new OrientedRectangle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _rectangleOffset, MathF.Sin(TotalTime) * _rectangleOffset),
			new Vector2(64 + MathF.Sin(TotalTime) * 32, 32 + MathF.Cos(TotalTime) * 16),
			TotalTime * 1.5f);
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddLine(A, HasCollision);
		drawList.AddOrientedRectangle(B, HasCollision);
	}
}
