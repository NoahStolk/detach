# Changelog

This library uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 0.13.0

### Added

- Added `Cylinder` struct to `Detach.Collisions.Primitives3D` namespace.
- Added cylinder collision functions to `Geometry3D`:
  - `PointInCylinder`
  - `SphereCylinder`
  - `CylinderCylinder`
  - `Raycast(Cylinder cylinder, Ray ray, out float distance)`

## 0.12.0

### Added

- Added optional parameter `epsilon` to `Geometry2D.PointOnLine` method. 

### Changed

- Renamed `OrientedRectangle.Position` to `OrientedRectangle.Center`.

### Fixed

- `OrientedRectangle.GetVertices` now returns vertices in the correct order. The 2nd and 3rd vertices were swapped.
- Fixed `Geometry2D.LineCircle` implementation. It did not detect all intersections correctly.

## 0.11.2

### Changed

- Improved performance for `Geometry3D.Raycast(Triangle3D triangle, Ray ray, out RaycastResult raycastResult)` function.

## 0.11.1

### Fixed

- Fixed incorrect math in `Geometry3D` class caused by `Plane` distances being negated.

## 0.11.0

### Added

- Added `Rectangle.FromCenter` and `Rectangle.FromTopLeft` static methods.
- Added `Rectangle.Center` property.

### Removed

- Removed public `Rectangle` constructor. Use `Rectangle.FromTopLeft` instead.

## 0.10.3

### Removed

- Removed overloads added in 0.10.2. Use `Vector2.Clamp`, `Vector3.Clamp`, and `Vector4.Clamp` instead.

## 0.10.2

### Added

- Added more overloads for `VectorUtils.Clamp`.

## 0.10.1

### Added

- Added `ReadRgb` extension method to `BinaryReaderExtensions`.
- Added `Write` extension method accepting `Rgb` to `BinaryWriterExtensions`.

## 0.10.0

### Added

- Added `Frustum` struct to `Detach.Collisions.Primitives3D` namespace.
- Added `PointInFrustum` and `SphereFrustum` methods to `Geometry3D` class.
- Added `Triangle2D` struct to `Detach.Collisions.Primitives2D` namespace.
- Added `PointInTriangle`, `LineTriangle`, `CircleTriangle`, `RectangleTriangle`, and `OrientedRectangleTriangle` methods to `Geometry2D` class.

### Changed

- Moved 2D and 3D primitives to `Detach.Collisions.Primitives2D` and `Detach.Collisions.Primitives3D` namespaces respectively.
- Renamed `Triangle` to `Triangle3D`.

## 0.9.1

### Added

- Added `RandomRgb` extension methods to `RandomExtensions`.

## 0.9.0

### Added

- Added `Rgb` struct.
- Added constructor to `Rgba` struct accepting `Rgb` and `byte` parameters.

## 0.8.0

### Added

- Added utility methods to pack `Rgba` data into an `int` and vice versa:
  - `ToRgbaInt`
  - `ToArgbInt`
  - `FromRgbaInt`
  - `FromArgbInt`

### Changed

- Renamed `Color` to `Rgba` to avoid conflicts with System.Drawing.
- Renamed `ReadColor` in `BinaryReaderExtensions` to `ReadRgba`.
- Renamed `RandomColor` in `RandomExtensions` to `RandomRgba`.
- The `A` parameter in the `Rgba` constructor is now optional. 

### Removed

- Removed `ReadableColorForBrightness` from `Rgba`.

## 0.7.0

### Added

- Added integer vector types.

## 0.6.1

### Fixed

- Fixed `ObjParser` crashing when encountering additional spaces in `.obj` files.

## 0.6.0

Rewrote collisions entirely.

### Added

- Added `VectorUtils.GetAngleFrom`.
- Added `CollisionManifold`, `Interval`, and `RaycastResult` structs.
- Added `Geometry2D` and `Geometry3D` static classes for 2D and 3D collision detection.
- Added collision primitives:
  - `Aabb`
  - `Circle`
  - `LineSegment2D`
  - `LineSegment3D`
  - `Obb`
  - `OrientedRectangle`
  - `Ray`
  - `Rectangle`
  - `Sphere`
  - `Triangle`
- Added `Matrix2`, `Matrix3`, and `Matrix4` structs.
- Added `Matrices` static class for matrix operations.
- Added `IMatrixOperations` interface containing static abstract methods, properties, and operators for matrix operations.

### Changed

- Renamed `VectorUtils.GetAngle` to `VectorUtils.GetAngleBetween`.

### Removed

- Removed old collision types:
  - `AxisAlignedRectangle`
  - `Circle`
  - `LineSegment`
  - `Ray`
  - `Sphere`
  - `Triangle`
  - `ViewFrustum`

## 0.5.1

### Added

- Added `mtllib` support to `ObjParser`.
- Added `MaterialLibraries` property to `ModelData` record.

## 0.5.0

### Added

- Added `MtlParser` class for parsing material files.

### Changed

- `Orientation` is now a `record struct`.

### Fixed

- Fixed not always using invariant culture when parsing .obj and .wav data.

## 0.4.0

### Added

- Added `ObjectName` and `GroupName` to `MeshData` record.

### Changed

- Meshes in .obj files are now grouped by object name rather than material name.

## 0.3.1

### Added

- Added `Inline.Span` overloads for `Matrix3x2` and `Matrix4x4`.

## 0.3.0

### Changed

- Updated to .NET 8.0.
- Renamed `Sphere.IntersectsSphere` to `Sphere.IntersectsOrContainsSphere`.
- Renamed `Circle.IntersectsCircle` to `Circle.IntersectsOrContainsCircle`.

## 0.2.0

### Added

- Added `MathUtils.ToRadians` and `MathUtils.ToDegrees` overloads accepting `Vector3`.
- Added `MathUtils.CreateRotationMatrixFromEulerAngles` method.

## 0.1.0

- Initial release
