# Detach

[![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Detach.svg)](https://www.nuget.org/packages/NoahStolk.Detach/)

Zero-dependency library for game engines, games, and other real-time applications (.NET 8 only)

## Collisions

### 2D Intersections (implementation count)

|                   | Point | LineSegment2D | Circle | Rectangle | OrientedRectangle | Triangle |
|-------------------|-------|---------------|--------|-----------|-------------------|----------|
| Point             | N/A   | 1             | 1      | 1         | 1                 | 1        |
| LineSegment2D     | 1     | 1             | 1      | 1         | 1                 | 1        |
| Circle            | 1     | 1             | 1      | 1         | 1                 | 1        |
| Rectangle         | 1     | 1             | 1      | 2         | 1                 | 1        |
| OrientedRectangle | 1     | 1             | 1      | 1         | 1                 | 1        |
| Triangle          | 1     | 1             | 1      | 1         | 1                 | 0        |

### 3D Intersections (implementation count)

|                  | Point | Sphere | Aabb | Obb | Plane | LineSegment3D | Ray | Triangle | Frustum | Cylinder | OrientedCylinder | Capsule | OrientedCapsule |
|------------------|-------|--------|------|-----|-------|---------------|-----|----------|---------|----------|------------------|---------|-----------------|
| Point            | N/A   | 1      | 1    | 1   | 1     | 1             | 1   | 1        | 1       | 0        | 0                | 0       | 0               |
| Sphere           | 1     | 1      | 1    | 1   | 1     | 1             | 2   | 1        | 1       | 0        | 0                | 0       | 0               |
| Aabb             | 1     | 1      | 1    | 1   | 1     | 1             | 2   | 1        | 0       | 0        | 0                | 0       | 0               |
| Obb              | 1     | 1      | 1    | 1   | 1     | 1             | 2   | 1        | 0       | 0        | 0                | 0       | 0               |
| Plane            | 1     | 1      | 1    | 1   | 1     | 1             | 2   | 1        | 0       | 0        | 0                | 0       | 0               |
| LineSegment3D    | 1     | 1      | 1    | 1   | 1     | 0             | 0   | 1        | 0       | 0        | 0                | 0       | 0               |
| Ray              | 1     | 2      | 2    | 2   | 2     | 0             | 2   | 2        | 0       | 0        | 0                | 0       | 0               |
| Triangle         | 1     | 1      | 1    | 1   | 1     | 1             | 2   | 2        | 0       | 0        | 0                | 0       | 0               |
| Frustum          | 1     | 1      | 0    | 0   | 0     | 0             | 0   | 0        | 0       | 0        | 0                | 0       | 0               |
| Cylinder         | 0     | 0      | 0    | 0   | 0     | 0             | 0   | 0        | 0       | 0        | 0                | 0       | 0               |
| OrientedCylinder | 0     | 0      | 0    | 0   | 0     | 0             | 0   | 0        | 0       | 0        | 0                | 0       | 0               |
| Capsule          | 0     | 0      | 0    | 0   | 0     | 0             | 0   | 0        | 0       | 0        | 0                | 0       | 0               |
| OrientedCapsule  | 0     | 0      | 0    | 0   | 0     | 0             | 0   | 0        | 0       | 0        | 0                | 0       | 0               |
