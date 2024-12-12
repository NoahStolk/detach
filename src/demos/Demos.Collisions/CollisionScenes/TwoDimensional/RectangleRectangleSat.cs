using Demos.Collisions.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Demos.Collisions.CollisionScenes.TwoDimensional;

internal sealed class RectangleRectangleSat() : CollisionScene<Rectangle, Rectangle>(Geometry2D.RectangleRectangleSat)
{
	private const float _rectangleOffsetA = 64;
	private const float _rectangleOffsetB = 128;

	public override void Update(float dt)
	{
		base.Update(dt);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		A = Rectangle.FromCenter(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime) * _rectangleOffsetA, MathF.Sin(quarterTime) * _rectangleOffsetA),
			new Vector2(96 + MathF.Sin(quarterTime) * 32));
		B = Rectangle.FromCenter(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _rectangleOffsetB, MathF.Sin(TotalTime) * _rectangleOffsetB),
			new Vector2(160 + MathF.Sin(halfTime) * 32));
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddRectangle(A, HasCollision);
		drawList.AddRectangle(B, HasCollision);
	}
}
