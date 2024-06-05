# Changelog

This library uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [unreleased]

### Added

- Added `VectorUtils.GetAngleFrom`.
- TODO: Document all added collision types etc.

### Changed

- Renamed `VectorUtils.GetAngle` to `VectorUtils.GetAngleBetween`.

### Removed

- TODO: Document all removed collision types etc.

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
