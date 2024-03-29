# Changelog

This library uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
