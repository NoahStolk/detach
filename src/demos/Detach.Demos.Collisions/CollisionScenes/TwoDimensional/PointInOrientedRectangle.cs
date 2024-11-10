using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Detach.Demos.Collisions.Utils;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.CollisionScenes.TwoDimensional;

public sealed class PointInOrientedRectangle : CollisionScene<Vector2, OrientedRectangle>
{
	private const float _pointOffset = 64;
	private const float _rectangleOffset = 128;

	public PointInOrientedRectangle()
		: base(Geometry2D.PointInOrientedRectangle)
	{
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float doubleTime = TotalTime * 2;
		A = CollisionSceneConstants.Origin + new Vector2(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);
		B = new OrientedRectangle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _rectangleOffset, MathF.Sin(TotalTime) * _rectangleOffset),
			new Vector2(64 + MathF.Sin(TotalTime) * 32, 32 + MathF.Cos(TotalTime) * 16),
			TotalTime * 1.5f);
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddPoint(A, HasCollision);
		drawList.AddOrientedRectangle(B, HasCollision);
	}
}
