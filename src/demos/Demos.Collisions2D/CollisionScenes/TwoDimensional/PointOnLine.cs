using Demos.Collisions2D.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Demos.Collisions2D.CollisionScenes.TwoDimensional;

internal sealed class PointOnLine() : CollisionScene<Vector2, LineSegment2D>(static (a, b) => Geometry2D.PointOnLine(a, b, 1f))
{
	private const float _pointOffset = 64;
	private const float _linePointOffset = 128;

	public override void Update(float dt)
	{
		base.Update(dt);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		A = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _pointOffset, MathF.Sin(TotalTime) * _pointOffset);
		B = new LineSegment2D(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime) * _linePointOffset, MathF.Sin(halfTime) * _linePointOffset),
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(quarterTime) * _linePointOffset, MathF.Sin(quarterTime) * _linePointOffset));
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddPoint(A, HasCollision);
		drawList.AddLine(B, HasCollision);
	}
}
