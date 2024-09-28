using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Detach.Demos.Collisions.Utils;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.CollisionScenes.TwoDimensional;

public sealed class CircleCircle : CollisionScene<Circle, Circle>
{
	private const float _circleOffsetA = 64;
	private const float _circleOffsetB = 128;

	public CircleCircle()
		: base(Geometry2D.CircleCircle)
	{
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		A = new Circle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime) * _circleOffsetA, MathF.Sin(quarterTime) * _circleOffsetA),
			64 + MathF.Sin(TotalTime) * 32);
		B = new Circle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _circleOffsetB, MathF.Sin(TotalTime) * _circleOffsetB),
			64 + MathF.Sin(TotalTime) * 32);
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddCircle(A, HasCollision);
		drawList.AddCircle(B, HasCollision);
	}
}
