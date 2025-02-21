using Demos.Collisions.Auto.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions.Auto.CollisionScenes.TwoDimensional;

internal sealed class CircleCastLine() : CollisionScene<CircleCast, LineSegment2D>(static (a, b) => Geometry2D.CircleCastLine(a, b))
{
	private const float _pointOffset = 64;
	private const float _linePointOffset = 128;

	public override void Update(float dt)
	{
		base.Update(dt);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		float endOffset = MathF.Sin(TotalTime) + 4;
		A = new CircleCast(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime) * _linePointOffset, MathF.Sin(halfTime) * _linePointOffset),
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime + endOffset) * _linePointOffset, MathF.Sin(halfTime + endOffset) * _linePointOffset),
			(MathF.Sin(halfTime) * 0.5f + 0.75f) * _pointOffset);
		B = new LineSegment2D(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(quarterTime) * _linePointOffset, MathF.Sin(quarterTime) * _linePointOffset),
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _linePointOffset, MathF.Sin(halfTime) * _linePointOffset));
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddCircleCast(A, HasCollision);
		drawList.AddLine(B, HasCollision);
	}
}
