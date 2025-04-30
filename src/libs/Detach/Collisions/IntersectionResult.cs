using System.Numerics;

namespace Detach.Collisions;

public readonly record struct IntersectionResult(Vector3 Normal, Vector3 IntersectionPoint, float PenetrationDepth);
