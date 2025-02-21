using Demos.Collisions.Auto.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions.Auto.CollisionScenes.TwoDimensional;

internal sealed class OrientedRectangleTriangle() : CollisionScene<OrientedRectangle, Triangle2D>(Geometry2D.OrientedRectangleTriangle)
{
	private const float _rectangleOffset = 64;
	private const float _triangleSize = 64;
	private static readonly Vector2 _triangleOffset = new(128, 24);

	public override void Update(float dt)
	{
		base.Update(dt);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		A = new OrientedRectangle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime) * _rectangleOffset, MathF.Sin(TotalTime) * _rectangleOffset),
			new Vector2(64 + MathF.Sin(TotalTime) * 32, 32 + MathF.Cos(quarterTime) * 16),
			TotalTime * 1.5f);
		B = new Triangle2D(
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime) * _triangleSize, MathF.Sin(TotalTime) * _triangleSize),
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime + MathF.PI * 2 / 3) * _triangleSize, MathF.Sin(TotalTime + MathF.PI * 2 / 3) * _triangleSize),
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime + MathF.PI * 4 / 3) * _triangleSize, MathF.Sin(TotalTime + MathF.PI * 4 / 3) * _triangleSize));
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddOrientedRectangle(A, HasCollision);
		drawList.AddTriangle(B, HasCollision);
	}
}
