﻿using Demos.Collisions.Auto.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions.Auto.CollisionScenes.TwoDimensional;

internal sealed class CircleOrientedRectangle() : CollisionScene<Circle, OrientedRectangle>(Geometry2D.CircleOrientedRectangle)
{
	private const float _circleOffset = 64;
	private const float _rectangleOffset = 128;

	public override void Update(float dt)
	{
		base.Update(dt);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		A = new Circle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(halfTime) * _circleOffset, MathF.Sin(quarterTime) * _circleOffset),
			64 + MathF.Sin(TotalTime) * 32);
		B = new OrientedRectangle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _rectangleOffset, MathF.Sin(TotalTime) * _rectangleOffset),
			new Vector2(64 + MathF.Sin(TotalTime) * 32, 32 + MathF.Cos(TotalTime) * 16),
			TotalTime * 1.5f);
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddCircle(A, HasCollision);
		drawList.AddOrientedRectangle(B, HasCollision);
	}
}
