﻿using System.Numerics;

namespace Detach.Collisions.Primitives3D;

public record struct Sphere
{
	// TODO: Rename to Center.
	public Vector3 Position;
	public float Radius;

	public Sphere(Vector3 position, float radius)
	{
		Position = position;
		Radius = radius;
	}
}
