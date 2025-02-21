using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using Detach.Extensions;

namespace CollisionFormats.Extensions;

public static class BinaryWriterExtensions
{
	public static void Write(this BinaryWriter bw, Circle circle)
	{
		bw.Write(circle.Center);
		bw.Write(circle.Radius);
	}

	public static void Write(this BinaryWriter bw, CircleCast circleCast)
	{
		bw.Write(circleCast.Start);
		bw.Write(circleCast.End);
		bw.Write(circleCast.Radius);
	}

	public static void Write(this BinaryWriter bw, LineSegment2D lineSegment2D)
	{
		bw.Write(lineSegment2D.Start);
		bw.Write(lineSegment2D.End);
	}

	public static void Write(this BinaryWriter bw, OrientedRectangle orientedRectangle)
	{
		bw.Write(orientedRectangle.Center);
		bw.Write(orientedRectangle.HalfExtents);
		bw.Write(orientedRectangle.RotationInRadians);
	}

	public static void Write(this BinaryWriter bw, Rectangle rectangle)
	{
		bw.Write(rectangle.Position);
		bw.Write(rectangle.Size);
	}

	public static void Write(this BinaryWriter bw, Triangle2D triangle2D)
	{
		bw.Write(triangle2D.A);
		bw.Write(triangle2D.B);
		bw.Write(triangle2D.C);
	}

	public static void Write(this BinaryWriter bw, Aabb aabb)
	{
		bw.Write(aabb.Center);
		bw.Write(aabb.Size);
	}

	public static void Write(this BinaryWriter bw, ConeFrustum coneFrustum)
	{
		bw.Write(coneFrustum.BottomCenter);
		bw.Write(coneFrustum.BottomRadius);
		bw.Write(coneFrustum.TopRadius);
		bw.Write(coneFrustum.Height);
	}

	public static void Write(this BinaryWriter bw, Cylinder cylinder)
	{
		bw.Write(cylinder.BottomCenter);
		bw.Write(cylinder.Radius);
		bw.Write(cylinder.Height);
	}

	public static void Write(this BinaryWriter bw, LineSegment3D lineSegment3D)
	{
		bw.Write(lineSegment3D.Start);
		bw.Write(lineSegment3D.End);
	}

	public static void Write(this BinaryWriter bw, Obb obb)
	{
		bw.Write(obb.Center);
		bw.Write(obb.HalfExtents);
		bw.Write(obb.Orientation);
	}

	public static void Write(this BinaryWriter bw, OrientedPyramid orientedPyramid)
	{
		bw.Write(orientedPyramid.Center);
		bw.Write(orientedPyramid.Size);
		bw.Write(orientedPyramid.Orientation);
	}

	public static void Write(this BinaryWriter bw, Pyramid pyramid)
	{
		bw.Write(pyramid.Center);
		bw.Write(pyramid.Size);
	}

	public static void Write(this BinaryWriter bw, Ray ray)
	{
		bw.Write(ray.Origin);
		bw.Write(ray.Direction);
	}

	public static void Write(this BinaryWriter bw, Sphere sphere)
	{
		bw.Write(sphere.Center);
		bw.Write(sphere.Radius);
	}

	public static void Write(this BinaryWriter bw, SphereCast sphereCast)
	{
		bw.Write(sphereCast.Start);
		bw.Write(sphereCast.End);
		bw.Write(sphereCast.Radius);
	}

	public static void Write(this BinaryWriter bw, Triangle3D triangle3D)
	{
		bw.Write(triangle3D.A);
		bw.Write(triangle3D.B);
		bw.Write(triangle3D.C);
	}
}
