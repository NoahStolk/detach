﻿using Demos.Collisions.Auto.Utils;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions.Auto.CollisionScenes.TwoDimensional;

internal sealed class OrientedRectangleOrientedRectangleSat() : CollisionScene<OrientedRectangle, OrientedRectangle>(Geometry2D.OrientedRectangleOrientedRectangleSat)
{
	private const float _rectangleOffsetA = 64;
	private const float _rectangleOffsetB = 128;

	public override void Update(float dt)
	{
		base.Update(dt);

		float halfTime = TotalTime / 2;
		float quarterTime = TotalTime / 4;
		A = new OrientedRectangle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _rectangleOffsetA, MathF.Sin(_rectangleOffsetA) * _rectangleOffsetB),
			new Vector2(96 + MathF.Sin(TotalTime) * 64, 48 + MathF.Cos(halfTime) * 12),
			TotalTime * 0.5f);
		B = new OrientedRectangle(
			CollisionSceneConstants.Origin + new Vector2(MathF.Cos(TotalTime) * _rectangleOffsetB, MathF.Sin(TotalTime) * _rectangleOffsetB),
			new Vector2(64 + MathF.Sin(quarterTime) * 32, 32 + MathF.Cos(TotalTime) * 16),
			TotalTime * 1.5f);
	}

	public override void Render()
	{
		PositionedDrawList drawList = new(ImGui.GetWindowDrawList(), ImGui.GetCursorScreenPos());
		drawList.AddBackground(CollisionSceneConstants.Size);
		drawList.AddOrientedRectangle(A, HasCollision);
		drawList.AddOrientedRectangle(B, HasCollision);
	}
}
