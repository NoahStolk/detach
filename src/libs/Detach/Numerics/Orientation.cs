using System.Numerics;

namespace Detach.Numerics;

public record struct Orientation
{
	public Vector3 At { get; init; }
	public Vector3 Up { get; init; }
}
