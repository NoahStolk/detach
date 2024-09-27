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
| Triangle          | 1     | 1             | 1      | 1         | 1                 | -        |

### 3D Intersections (implementation count)

|                  | Point | LineSegment3D | Ray | Sphere | Aabb | Obb | Plane | Triangle | Frustum | Cylinder | OrientedCylinder | Capsule | OrientedCapsule |
|------------------|-------|---------------|-----|--------|------|-----|-------|----------|---------|----------|------------------|---------|-----------------|
| Point            | N/A   | 1             | 1   | 1      | 1    | 1   | 1     | 1        | 1       | -        | -                | -       | -               |
| LineSegment3D    | 1     | N/A           | N/A | 1      | 1    | 1   | 1     | 1        | -       | -        | -                | -       | -               |
| Ray              | 1     | N/A           | N/A | 2      | 2    | 2   | 2     | 2        | -       | -        | -                | -       | -               |
| Sphere           | 1     | 1             | 2   | 1      | 1    | 1   | 1     | 1        | 1       | -        | -                | -       | -               |
| Aabb             | 1     | 1             | 2   | 1      | 1    | 1   | 1     | 1        | -       | -        | -                | -       | -               |
| Obb              | 1     | 1             | 2   | 1      | 1    | 1   | 1     | 1        | -       | -        | -                | -       | -               |
| Plane            | 1     | 1             | 2   | 1      | 1    | 1   | 1     | 1        | -       | -        | -                | -       | -               |
| Triangle         | 1     | 1             | 2   | 1      | 1    | 1   | 1     | 2        | -       | -        | -                | -       | -               |
| Frustum          | 1     | -             | -   | 1      | -    | -   | -     | -        | -       | -        | -                | -       | -               |
| Cylinder         | -     | -             | -   | -      | -    | -   | -     | -        | -       | -        | -                | -       | -               |
| OrientedCylinder | -     | -             | -   | -      | -    | -   | -     | -        | -       | -        | -                | -       | -               |
| Capsule          | -     | -             | -   | -      | -    | -   | -     | -        | -       | -        | -                | -       | -               |
| OrientedCapsule  | -     | -             | -   | -      | -    | -   | -     | -        | -       | -        | -                | -       | -               |
