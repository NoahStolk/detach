using Demos.Collisions.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Demos.Collisions.CollisionScenes.TwoDimensional;

public sealed class RectangleOrientedRectangleSat : CollisionScene<Rectangle, OrientedRectangle>
{
	private const float _rectangleOffsetA = 64;
	private const float _rectangleOffsetB = 128;

	public RectangleOrientedRectangleSat()
		: base(Geometry2D.RectangleOrientedRectangleSat)
	{
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		A = Rectangle.FromCenter(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime) * _rectangleOffsetA, MathF.Sin(quarterTime) * _rectangleOffsetA),
			new Vector2(160 + MathF.Sin(quarterTime) * 32));
		B = new OrientedRectangle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _rectangleOffsetB, MathF.Sin(TotalTime) * _rectangleOffsetB),
			new Vector2(64 + MathF.Sin(TotalTime) * 32, 32 + MathF.Cos(TotalTime) * 16),
			TotalTime * 1.5f);
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddRectangle(A, HasCollision);
		drawList.AddOrientedRectangle(B, HasCollision);
	}
}
