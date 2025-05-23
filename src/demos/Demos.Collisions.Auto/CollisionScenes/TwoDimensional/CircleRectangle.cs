﻿using Demos.Collisions.Auto.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions.Auto.CollisionScenes.TwoDimensional;

internal sealed class CircleRectangle() : CollisionScene<Circle, Rectangle>(Geometry2D.CircleRectangle)
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
		B = Rectangle.FromCenter(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _rectangleOffset, MathF.Sin(TotalTime) * _rectangleOffset),
			new Vector2(160 + MathF.Sin(TotalTime) * 32));
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddCircle(A, HasCollision);
		drawList.AddRectangle(B, HasCollision);
	}
}
