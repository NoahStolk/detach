using Demos.Collisions2D.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions2D.CollisionScenes.TwoDimensional;

internal sealed class RectangleTriangle() : CollisionScene<Rectangle, Triangle2D>(Geometry2D.RectangleTriangle)
{
	private const float _rectangleOffset = 64;
	private const float _triangleSize = 64;
	private static readonly Vector2 _triangleOffset = new(128, 24);

	public override void Update(float dt)
	{
		base.Update(dt);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		A = Rectangle.FromCenter(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _rectangleOffset, MathF.Sin(halfTime) * _rectangleOffset),
			new Vector2(160 + MathF.Sin(quarterTime) * 32));
		B = new Triangle2D(
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime) * _triangleSize, MathF.Sin(TotalTime) * _triangleSize),
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime + MathF.PI * 2 / 3) * _triangleSize, MathF.Sin(TotalTime + MathF.PI * 2 / 3) * _triangleSize),
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime + MathF.PI * 4 / 3) * _triangleSize, MathF.Sin(TotalTime + MathF.PI * 4 / 3) * _triangleSize));
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddRectangle(A, HasCollision);
		drawList.AddTriangle(B, HasCollision);
	}
}
