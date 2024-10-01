﻿using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Detach.Demos.Collisions.Utils;
using ImGuiNET;
using System.Numerics;

namespace Detach.Demos.Collisions.CollisionScenes.TwoDimensional;

public sealed class CircleTriangle : CollisionScene<Circle, Triangle2D>
{
	private const float _circleOffset = 64;
	private const float _triangleSize = 128;
	private static readonly Vector2 _triangleOffset = new(48, 24);

	public CircleTriangle()
		: base(Geometry2D.CircleTriangle)
	{
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		A = new Circle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime) * _circleOffset, MathF.Sin(quarterTime) * _circleOffset),
			64 + MathF.Sin(TotalTime) * 32);
		B = new Triangle2D(
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime) * _triangleSize, MathF.Sin(TotalTime) * _triangleSize),
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime + MathF.PI * 2 / 3) * _triangleSize, MathF.Sin(TotalTime + MathF.PI * 2 / 3) * _triangleSize),
			CollisionSceneConstants.Origin + _triangleOffset + new Vector2(MathF.Cos(TotalTime + MathF.PI * 4 / 3) * _triangleSize, MathF.Sin(TotalTime + MathF.PI * 4 / 3) * _triangleSize));
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddCircle(A, HasCollision);
		drawList.AddTriangle(B, HasCollision);
	}
}