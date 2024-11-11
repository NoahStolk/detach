using Demos.Collisions.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Demos.Collisions.CollisionScenes.TwoDimensional;

public sealed class PointInCircle : CollisionScene<Vector2, Circle>
{
	private const float _pointOffset = 64;
	private const float _circleOffset = 128;

	public PointInCircle()
		: base(Geometry2D.PointInCircle)
	{
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float doubleTime = TotalTime * 2;
		A = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);
		B = new Circle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _circleOffset, MathF.Sin(TotalTime) * _circleOffset),
			64 + MathF.Sin(TotalTime) * 32);
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddPoint(A, HasCollision);
		drawList.AddCircle(B, HasCollision);
	}
}
