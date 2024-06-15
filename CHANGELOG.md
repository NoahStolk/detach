# Changelog

This library uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
