using Demos.Collisions.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using ImGuiNET;
using System.Numerics;

namespace Demos.Collisions.CollisionScenes.TwoDimensional;

public sealed class PointInTriangle : CollisionScene<Vector2, Triangle2D>
{
	private const float _pointOffset = 96;
	private const float _triangleSize = 128;
	private static readonly Vector2 _triangleOffset = new(48, 24);

	public PointInTriangle()
		: base(Geometry2D.PointInTriangle)
	{
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float doubleTime = TotalTime * 2;
		A = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);
		B = new Triangle2D(
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime) * _triangleSize, MathF.Sin(TotalTime) * _triangleSize),
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime + MathF.PI * 2 / 3) * _triangleSize, MathF.Sin(TotalTime + MathF.PI * 2 / 3) * _triangleSize),
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime + MathF.PI * 4 / 3) * _triangleSize, MathF.Sin(TotalTime + MathF.PI * 4 / 3) * _triangleSize));
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddPoint(A, HasCollision);
		drawList.AddTriangle(B, HasCollision);
	}
}
