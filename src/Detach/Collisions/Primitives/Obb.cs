﻿using Detach.Buffers;
using Detach.Numerics;
using System.Numerics;

namespace Detach.Collisions.Primitives;

public record struct Obb
{
	public Vector3 Position;
	public Vector3 Size;
	public Matrix3 Orientation;

	public Obb(Vector3 position, Vector3 size, Matrix3 orientation)
	{
		Position = position;
		Size = size;
		Orientation = orientation;
	}

	public Interval GetInterval(Vector3 axis)
	{
		Vector3 c = Position;
		Vector3 e = Size;
		Span<Vector3> axes = stackalloc Vector3[]
		{
			new(Orientation.M11, Orientation.M12, Orientation.M13),
			new(Orientation.M21, Orientation.M22, Orientation.M23),
			new(Orientation.M31, Orientation.M32, Orientation.M33),
		};

		Span<Vector3> vertices = stackalloc Vector3[]
		{
			c + axes[0] * e.X + axes[1] * e.Y + axes[2] * e.Z,
			c - axes[0] * e.X + axes[1] * e.Y + axes[2] * e.Z,
			c + axes[0] * e.X - axes[1] * e.Y + axes[2] * e.Z,
			c + axes[0] * e.X + axes[1] * e.Y - axes[2] * e.Z,
			c - axes[0] * e.X - axes[1] * e.Y - axes[2] * e.Z,
			c + axes[0] * e.X - axes[1] * e.Y - axes[2] * e.Z,
			c - axes[0] * e.X + axes[1] * e.Y - axes[2] * e.Z,
			c - axes[0] * e.X - axes[1] * e.Y + axes[2] * e.Z,
		};

		Interval result = default;
		float projection0 = Vector3.Dot(axis, vertices[0]);
		result.Min = projection0;
		result.Max = projection0;

		for (int i = 1; i < 8; i++)
		{
			float projection = Vector3.Dot(axis, vertices[i]);
			if (projection < result.Min)
				result.Min = projection;
			else if (projection > result.Max)
				result.Max = projection;
		}

		return result;
	}

	public Buffer8<Vector3> GetVertices()
	{
		Vector3 c = Position;
		Vector3 e = Size;
		Span<Vector3> axes = stackalloc Vector3[]
		{
			new(Orientation.M11, Orientation.M12, Orientation.M13),
			new(Orientation.M21, Orientation.M22, Orientation.M23),
			new(Orientation.M31, Orientation.M32, Orientation.M33),
		};

		Buffer8<Vector3> result = default;
		result[0] = c + axes[0] * e.X + axes[1] * e.Y + axes[2] * e.Z;
		result[1] = c - axes[0] * e.X + axes[1] * e.Y + axes[2] * e.Z;
		result[2] = c + axes[0] * e.X - axes[1] * e.Y + axes[2] * e.Z;
		result[3] = c + axes[0] * e.X + axes[1] * e.Y - axes[2] * e.Z;
		result[4] = c - axes[0] * e.X - axes[1] * e.Y - axes[2] * e.Z;
		result[5] = c + axes[0] * e.X - axes[1] * e.Y - axes[2] * e.Z;
		result[6] = c - axes[0] * e.X + axes[1] * e.Y - axes[2] * e.Z;
		result[7] = c - axes[0] * e.X - axes[1] * e.Y + axes[2] * e.Z;
		return result;
	}

	public Buffer12<LineSegment3D> GetEdges()
	{
		Buffer8<Vector3> vertices = GetVertices();
		Span<ValueTuple<int, int>> indices = stackalloc ValueTuple<int, int>[]
		{
			(6, 1),
			(6, 3),
			(6, 4),
			(2, 7),
			(2, 5),
			(2, 0),
			(0, 1),
			(0, 3),
			(7, 1),
			(7, 4),
			(4, 5),
			(5, 3),
		};

		Buffer12<LineSegment3D> result = default;
		for (int i = 0; i < indices.Length; i++)
		{
			ValueTuple<int, int> index = indices[i];
			result[i] = new(vertices[index.Item1], vertices[index.Item2]);
		}

		return result;
	}

	public Buffer6<Plane> GetPlanes()
	{
		Vector3 c = Position;
		Vector3 e = Size;
		Span<Vector3> axes = stackalloc Vector3[]
		{
			new(Orientation.M11, Orientation.M12, Orientation.M13),
			new(Orientation.M21, Orientation.M22, Orientation.M23),
			new(Orientation.M31, Orientation.M32, Orientation.M33),
		};

		Buffer6<Plane> result = default;
		result[0] = new(axes[0], Vector3.Dot(axes[0], c + axes[0] * e.X));
		result[1] = new(-axes[0], -Vector3.Dot(axes[0], c - axes[0] * e.X));
		result[2] = new(axes[1], Vector3.Dot(axes[1], c + axes[1] * e.Y));
		result[3] = new(-axes[1], -Vector3.Dot(axes[1], c - axes[1] * e.Y));
		result[4] = new(axes[2], Vector3.Dot(axes[2], c + axes[2] * e.Z));
		result[5] = new(-axes[2], -Vector3.Dot(axes[2], c - axes[2] * e.Z));
		return result;
	}
}
