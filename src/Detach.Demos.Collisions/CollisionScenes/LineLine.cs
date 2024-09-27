using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Detach.Demos.Collisions.Utils;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.CollisionScenes;

public sealed class LineLine : CollisionScene<LineSegment2D, LineSegment2D>
{
	private const float _linePointOffsetA = 64;
	private const float _linePointOffsetB = 128;

	public LineLine()
		: base(Geometry2D.LineLine)
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
		B = new LineSegment2D(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime) * _linePointOffsetB, MathF.Sin(halfTime) * _linePointOffsetB),
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(quarterTime) * _linePointOffsetB, MathF.Sin(quarterTime) * _linePointOffsetB));
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddLine(A, HasCollision);
		drawList.AddLine(B, HasCollision);
	}
}
