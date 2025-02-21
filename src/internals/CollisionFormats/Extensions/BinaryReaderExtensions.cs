using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using Detach.Extensions;
using Detach.Numerics;
using System.Numerics;

namespace CollisionFormats.Extensions;

public static class BinaryReaderExtensions
{
    public static Circle ReadCircle(this BinaryReader br)
    {
        Vector2 center = br.ReadVector2();
        float radius = br.ReadSingle();
        return new Circle(center, radius);
    }

    public static CircleCast ReadCircleCast(this BinaryReader br)
    {
        Vector2 start = br.ReadVector2();
        Vector2 end = br.ReadVector2();
        float radius = br.ReadSingle();
        return new CircleCast(start, end, radius);
    }

    public static LineSegment2D ReadLineSegment2D(this BinaryReader br)
    {
        Vector2 start = br.ReadVector2();
        Vector2 end = br.ReadVector2();
        return new LineSegment2D(start, end);
    }

    public static OrientedRectangle ReadOrientedRectangle(this BinaryReader br)
    {
        Vector2 center = br.ReadVector2();
        Vector2 halfExtents = br.ReadVector2();
        float rotationInRadians = br.ReadSingle();
        return new OrientedRectangle(center, halfExtents, rotationInRadians);
    }

    public static Rectangle ReadRectangle(this BinaryReader br)
    {
        Vector2 position = br.ReadVector2();
        Vector2 size = br.ReadVector2();
        return Rectangle.FromTopLeft(position, size);
    }

    public static Triangle2D ReadTriangle2D(this BinaryReader br)
    {
        Vector2 a = br.ReadVector2();
        Vector2 b = br.ReadVector2();
        Vector2 c = br.ReadVector2();
        return new Triangle2D(a, b, c);
    }

    public static Aabb ReadAabb(this BinaryReader br)
    {
        Vector3 center = br.ReadVector3();
        Vector3 size = br.ReadVector3();
        return new Aabb(center, size);
    }

    public static ConeFrustum ReadConeFrustum(this BinaryReader br)
    {
        Vector3 bottomCenter = br.ReadVector3();
        float bottomRadius = br.ReadSingle();
        float topRadius = br.ReadSingle();
        float height = br.ReadSingle();
        return new ConeFrustum(bottomCenter, bottomRadius, topRadius, height);
    }

    public static Cylinder ReadCylinder(this BinaryReader br)
    {
        Vector3 bottomCenter = br.ReadVector3();
        float radius = br.ReadSingle();
        float height = br.ReadSingle();
        return new Cylinder(bottomCenter, radius, height);
    }

    public static LineSegment3D ReadLineSegment3D(this BinaryReader br)
    {
        Vector3 start = br.ReadVector3();
        Vector3 end = br.ReadVector3();
        return new LineSegment3D(start, end);
    }

    public static Obb ReadObb(this BinaryReader br)
    {
        Vector3 center = br.ReadVector3();
        Vector3 halfExtents = br.ReadVector3();
        Matrix3 orientation = br.ReadMatrix3();
        return new Obb(center, halfExtents, orientation);
    }

    public static OrientedPyramid ReadOrientedPyramid(this BinaryReader br)
    {
        Vector3 center = br.ReadVector3();
        Vector3 size = br.ReadVector3();
        Matrix3 orientation = br.ReadMatrix3();
        return new OrientedPyramid(center, size, orientation);
    }

    public static Pyramid ReadPyramid(this BinaryReader br)
    {
        Vector3 center = br.ReadVector3();
        Vector3 size = br.ReadVector3();
        return new Pyramid(center, size);
    }

    public static Ray ReadRay(this BinaryReader br)
    {
        Vector3 origin = br.ReadVector3();
        Vector3 direction = br.ReadVector3();
        return new Ray(origin, direction);
    }

    public static Sphere ReadSphere(this BinaryReader br)
    {
        Vector3 center = br.ReadVector3();
        float radius = br.ReadSingle();
        return new Sphere(center, radius);
    }

    public static SphereCast ReadSphereCast(this BinaryReader br)
    {
        Vector3 start = br.ReadVector3();
        Vector3 end = br.ReadVector3();
        float radius = br.ReadSingle();
        return new SphereCast(start, end, radius);
    }

    public static Triangle3D ReadTriangle3D(this BinaryReader br)
    {
        Vector3 a = br.ReadVector3();
        Vector3 b = br.ReadVector3();
        Vector3 c = br.ReadVector3();
        return new Triangle3D(a, b, c);
    }
}
