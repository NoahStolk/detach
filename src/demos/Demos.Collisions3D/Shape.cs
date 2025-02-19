using Detach.Collisions.Primitives3D;
using UnionStruct;

namespace Demos.Collisions3D;

[Union]
internal partial struct Shape
{
	[UnionCase]
	public static partial Shape Aabb(Aabb aabb);

	[UnionCase]
	public static partial Shape ConeFrustum(ConeFrustum coneFrustum);

	[UnionCase]
	public static partial Shape Cylinder(Cylinder cylinder);

	[UnionCase]
	public static partial Shape LineSegment3D(LineSegment3D lineSegment3D);

	[UnionCase]
	public static partial Shape Obb(Obb obb);

	[UnionCase]
	public static partial Shape Ray(Ray ray);

	[UnionCase]
	public static partial Shape Sphere(Sphere sphere);

	[UnionCase]
	public static partial Shape SphereCast(SphereCast sphereCast);

	[UnionCase]
	public static partial Shape Triangle3D(Triangle3D triangle3D);
}
