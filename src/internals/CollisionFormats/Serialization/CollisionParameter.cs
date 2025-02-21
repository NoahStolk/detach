using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using System.Numerics;
using UnionStruct;

namespace CollisionFormats.Serialization;

[Union]
public partial struct CollisionParameter
{
	[UnionCase]
	public static partial CollisionParameter Circle(Circle circle);

	[UnionCase]
	public static partial CollisionParameter CircleCast(CircleCast circleCast);

	[UnionCase]
	public static partial CollisionParameter LineSegment2D(LineSegment2D lineSegment2D);

	[UnionCase]
	public static partial CollisionParameter OrientedRectangle(OrientedRectangle orientedRectangle);

	[UnionCase]
	public static partial CollisionParameter Point2D(Vector2 point2D);

	[UnionCase]
	public static partial CollisionParameter Rectangle(Rectangle rectangle);

	[UnionCase]
	public static partial CollisionParameter Triangle2D(Triangle2D triangle2D);

	[UnionCase]
	public static partial CollisionParameter Aabb(Aabb aabb);

	[UnionCase]
	public static partial CollisionParameter ConeFrustum(ConeFrustum coneFrustum);

	[UnionCase]
	public static partial CollisionParameter Cylinder(Cylinder cylinder);

	[UnionCase]
	public static partial CollisionParameter LineSegment3D(LineSegment3D lineSegment3D);

	[UnionCase]
	public static partial CollisionParameter Obb(Obb obb);

	[UnionCase]
	public static partial CollisionParameter OrientedPyramid(OrientedPyramid orientedPyramid);

	[UnionCase]
	public static partial CollisionParameter Point3D(Vector3 point3D);

	[UnionCase]
	public static partial CollisionParameter Pyramid(Pyramid pyramid);

	[UnionCase]
	public static partial CollisionParameter Ray(Ray ray);

	[UnionCase]
	public static partial CollisionParameter Sphere(Sphere sphere);

	[UnionCase]
	public static partial CollisionParameter SphereCast(SphereCast sphereCast);

	[UnionCase]
	public static partial CollisionParameter Triangle3D(Triangle3D triangle3D);

	[UnionCase]
	public static partial CollisionParameter ViewFrustum(ViewFrustum viewFrustum);
}
