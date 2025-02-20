using Detach.Collisions.Primitives2D;
using System.Numerics;

namespace Detach.Collisions;

public static partial class Geometry2D
{
	public static Vector2 ClosestPointOnLine(Vector2 point, LineSegment2D line)
	{
		Vector2 direction = line.End - line.Start;
		float t = Vector2.Dot(point - line.Start, direction) / Vector2.Dot(direction, direction);
		t = Math.Clamp(t, 0, 1);
		return line.Start + direction * t;
	}
}
