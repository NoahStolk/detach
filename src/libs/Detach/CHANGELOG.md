# Changelog

This library uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [unreleased]

### Fixed

- Fixed bugs in:
  - `Geometry3D.Linetest(Aabb, LineSegment3D)`
  - `Geometry3D.Linetest(Obb, LineSegment3D)`
  - `Geometry3D.ClosestPointOnTriangle`
  - `Geometry3D.TriangleSphere`

## 0.26.6

### Fixed

- Fixed `Geometry3D.PointInTriangle` returning false positives when the point is inside the triangle but outside the plane.
- Fixed bugs in `Geometry3D.PointInOrientedPyramid`.

## 0.26.5

### Changed

- Renamed `VectorExtensions.IsReal` to `VectorExtensions.IsFinite`.
- Replaced `QuaternionExtensions.ContainsNaN` with `QuaternionExtensions.IsFinite`. Note that the new method returns the opposite value.

### Removed

- Removed `MathUtils.IsFloatReal`. Use `float.IsFinite` instead.

## 0.26.4

### Fixed

- Fixed not being able to read WAV files where the `data` chunk was not aligned to 4 bytes.

## 0.26.3

### Added

- Added constructors accepting `Matrix4` and `Matrix4x4` to `Matrix3` struct.

## 0.26.2

### Changed

- Improved performance of `TgaImageData.Read` method.

## 0.26.1

### Removed

- Removed `MathUtils.ToDegrees(float)` and `MathUtils.ToRadians(float)`. Use `float.DegreesToRadians` and `float.RadiansToDegrees` instead.
- Removed `MathUtils.ToDegrees(Vector3)` and `MathUtils.ToRadians(Vector3)`. Use `Vector3.DegreesToRadians` and `Vector3.RadiansToDegrees` instead.

## 0.26.0

### Added

- Added `Detach.CodeWriter` type.

## 0.25.2

### Added

- Added `MathUtils.Bezier` method.
- Added `Rgba.ToAbgrInt` and `Rgba.FromAbgrInt` methods.

## 0.25.1

### Added

- Added `VectorUtils.ClampMagnitudeMin` methods for `Vector2` and `Vector3`.
- Added `MathUtils.LerpClamped` method.

## 0.25.0

### Added

- Added new collision algorithms:
  - Added `Pyramid` 3D primitive along with the following methods:
    - `Geometry3D.PointInPyramid`
    - `Geometry3D.ClosestPointInPyramid`
    - `Geometry3D.SpherePyramid`
    - `Geometry3D.SphereCastPyramid`
  - Added `OrientedPyramid` 3D primitive along with the following methods:
    - `Geometry3D.PointInOrientedPyramid`
    - `Geometry3D.ClosestPointInOrientedPyramid`
    - `Geoemtry3D.SphereOrientedPyramid`
    - `Geometry3D.SphereCastOrientedPyramid`
  - Added `CircleCast` 2D primitive along with the following methods:
    - `Geometry2D.CircleCastPoint`
    - `Geometry2D.CircleCastLine`
  - Added `Geometry2D.ClosestPointOnLine` method.
- Added `ViewFrustum.GetCorners` method (previously `Frustum`).
- Added support for `bool`, `Vector2`, `Vector3`, `Vector4`, and `Quaternion` to `InlineInterpolatedStringHandlerUtf8`.
- Added `MathUtils.IsAlmostZero` method.
- Added `VectorUtils.RotateVector` method to rotate a directional `Vector2` by an angle.
- Added `Matrix3.GetYawPitchRoll` method.
- Added new extension methods for `BinaryReader`:
  - `ReadMatrix3x2`
  - `ReadMatrix2`
  - `ReadMatrix3`
  - `ReadMatrix4`
- Added new extension methods for `BinaryWriter`:
  - `Write(Matrix3x2)`
  - `Write(Matrix2)`
  - `Write(Matrix3)`
  - `Write(Matrix4)`

### Changed

- Changed the order of parameters in some `Geometry3D.ClosestPointIn...` methods. The `point` parameter is now always the first parameter.
- Renamed `Frustum` to `ViewFrustum`.
- Renamed `Geometry3D.SphereCastLineSegment` to `Geometry3D.SphereCastLine`.

### Fixed

- Fixed `Geometry3D.SphereCastLine` (previously `Geometry3D.SphereCastLineSegment`) not working correctly.
- Fixed minor precision issues in `Geometry2D.LineLine` and `Geometry2D.LineRectangle` methods.

## 0.24.3

### Added

- Added `MathUtils.Fraction` method.

## 0.24.2

### Added

- Added `Geometry3D.AabbCylinder` method.

## 0.24.1

### Fixed

- Fixed implementation of `Geometry3D.SphereCastCylinder` method.

## 0.24.0

### Added

- Added `Geometry3D.SphereCastCylinder` method.
- Added `ConeFrustum` struct to `Detach.Collisions.Primitives3D` namespace.
- Added `Geometry3D.SphereConeFrustum` and `Geometry3D.SphereCastConeFrustum` methods.

### Changed

- Renamed `Circle.Position` to `Circle.Center`.
- Renamed `Aabb.Origin` to `Aabb.Center`.
- Renamed `Cylinder.BasePosition` to `Cylinder.BottomCenter`.
- Renamed `Obb.Position` to `Obb.Center`.
- Renamed `Sphere.Position` to `Sphere.Center`.

## 0.23.1

### Fixed

- Fixed `Aabb.FromMinMax` not calculating the correct size.

## 0.23.0

### Added

- Added extension methods for `BinaryReader` and `BinaryWriter` accepting inline `Buffer` structs.

## 0.22.1

### Added

- Added `Size` const fields to inline `Buffer` struct types.

## 0.22.0

### Added

- Added constructors to inline `Buffer` struct types.

### Removed

- Removed `FromSpan` static methods from inline `Buffer` struct types. Use the constructors instead.

## 0.21.0

### Added

- Added `Triangle3D.GetNormal` method.
- Added `MatrixUtils.CreateRotationMatrixAroundPoint` method.
- Added `MatrixUtils.CreateRotationMatrixFromDirection` method.
- Added `Spinor.Identity` property.

## 0.20.0

### Added

- Added `params` keyword to `options` parameter in `RandomExtensions.Choose<T>` method.

### Changed

- Updated to .NET 9.0.

## 0.19.0

### Added

- Added generic `RingBuffer<T>` class to `Detach.Buffers` namespace.
- Added ring buffer properties to some metrics types. These can be used for displaying performance graphs.
  - `FrameCounter.FrameTimesMs`
  - `HeapAllocationCounter.AllocatedBytesBuffer`

## 0.18.2

### Added

- Added `SphereCastTriangle` overload to `Geometry3D` with an `out Vector3 intersectionPoint` parameter.

## 0.18.1

### Fixed

- Fixed various overloads for `Inline.Utf8` and `Inline.Utf16` not terminating the data with a `\0` character. This is to prevent reading past the end of the string when the underlying memory is used directly. Previously, this only worked for interpolated string handlers. The fix is applied to the overloads accepting the following types: 
  - `Boolean`
  - `ISpanFormattable (T)`
  - `IUtf8SpanFormattable (T)`
  - `Matrix3x2`
  - `Matrix4x4`
  - `Quaternion`
  - `Vector2`
  - `Vector3`
  - `Vector4`

## 0.18.0

### Added

- Added new collision methods to `Geometry3D`:
  - `Geometry3D.SphereCastPoint`
  - `Geometry3D.SphereCastLineSegment`
  - `Geometry3D.SphereCastTriangle`

## 0.17.0

### Added

- Added some metrics types:
  - `Detach.Metrics.FrameCounter`
  - `Detach.Metrics.HeapAllocationCounter`
  - `Detach.Metrics.SimpleCounter`

## 0.16.1

### Added

- Added `Inline.Utf8` and `Inline.Utf16` methods accepting a `bool` parameter.

## 0.16.0

### Added

- Added `Inline.Utf8` methods.
- Added `Inline.BufferUtf8` property.
- Added `InlineInterpolatedStringHandlerUtf8` struct.
- Some types now implement `ISpanFormattable` and `IUtf8SpanFormattable`:
  - `IntVector2`
  - `IntVector3`
  - `IntVector4`
  - `Matrix2`
  - `Matrix3`
  - `Matrix4`
  - `Rgb`
  - `Rgba`
  - `Spinor`

### Changed

- Renamed `Inline.Span` methods to `Inline.Utf16`.
- Renamed `Inline.Buffer` property to `Inline.BufferUtf16`.
- The `ToString` implementation has been changed for the following types:
  - `IntVector2`
  - `IntVector3`
  - `IntVector4`
- Converting `InlineInterpolatedStringHandlerUtf16` to `ReadOnlySpan<char>` now sets the next character to `'\0'` to prevent reading past the end of the string when the underlying memory is used directly.

### Removed

- Removed redundant `Inline.Span` overload accepting a `string` parameter.

## 0.15.0

### Added

- Added new collision methods to `Geometry3D`:
  - `Geometry3D.SphereCastSphere`
  - `Geometry3D.SphereCastSphereCast`

## 0.14.0

### Added

- Added `SphereCast` 3D primitive to `Detach.Collisions.Primitives3D` namespace.
- Added `SphereCastAabb` and `SphereCastObb` methods to `Geometry3D` class.
- Added `VectorUtils.Transform(Vector3 vector, Matrix3 matrix)` method.
- Added `Matrix3` constructor accepting 3 `Vector3` parameters.
- Added `ToMatrix4` and `ToMatrix4x4` methods to `Matrix3` struct.
- Added `FromQuaternion` method to `Matrix3` struct.

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
