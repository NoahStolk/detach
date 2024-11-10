# Collisions

## 2D Intersections (implementation count)

|                   | Point | LineSegment2D | Circle | Rectangle | OrientedRectangle | Triangle |
|-------------------|-------|---------------|--------|-----------|-------------------|----------|
| Point             | N/A   | 1             | 1      | 1         | 1                 | 1        |
| LineSegment2D     | 1     | 1             | 1      | 1         | 1                 | 1        |
| Circle            | 1     | 1             | 1      | 1         | 1                 | 1        |
| Rectangle         | 1     | 1             | 1      | 2         | 1                 | 1        |
| OrientedRectangle | 1     | 1             | 1      | 1         | 1                 | 1        |
| Triangle          | 1     | 1             | 1      | 1         | 1                 | -        |

## 3D Intersections (implementation count)

|                  | Point | LineSegment3D | Ray | SphereCast | Sphere | Aabb | Obb | Plane | Triangle | Frustum | Cylinder | OrientedCylinder | Capsule | OrientedCapsule |
|------------------|-------|---------------|-----|------------|--------|------|-----|-------|----------|---------|----------|------------------|---------|-----------------|
| Point            | N/A   | 1             | 1   | -          | 1      | 1    | 1   | 1     | 1        | 1       | 1        | -                | -       | -               |
| LineSegment3D    | 1     | N/A           | N/A | -          | 1      | 1    | 1   | 1     | 1        | -       | -        | -                | -       | -               |
| Ray              | 1     | N/A           | N/A | -          | 2      | 2    | 2   | 2     | 2        | -       | 1        | -                | -       | -               |
| SphereCast       | -     | -             | -   | 1          | 1      | 1    | 1   | -     | -        | -       | -        | -                | -       | -               |
| Sphere           | 1     | 1             | 2   | 1          | 1      | 1    | 1   | 1     | 1        | 1       | 1        | -                | -       | -               |
| Aabb             | 1     | 1             | 2   | 1          | 1      | 1    | 1   | 1     | 1        | -       | -        | -                | -       | -               |
| Obb              | 1     | 1             | 2   | 1          | 1      | 1    | 1   | 1     | 1        | -       | -        | -                | -       | -               |
| Plane            | 1     | 1             | 2   | -          | 1      | 1    | 1   | 1     | 1        | -       | -        | -                | -       | -               |
| Triangle         | 1     | 1             | 2   | -          | 1      | 1    | 1   | 1     | 2        | -       | -        | -                | -       | -               |
| Frustum          | 1     | -             | -   | -          | 1      | -    | -   | -     | -        | -       | -        | -                | -       | -               |
| Cylinder         | 1     | -             | 1   | -          | 1      | -    | -   | -     | -        | -       | 1        | -                | -       | -               |
| OrientedCylinder | -     | -             | -   | -          | -      | -    | -   | -     | -        | -       | -        | -                | -       | -               |
| Capsule          | -     | -             | -   | -          | -      | -    | -   | -     | -        | -       | -        | -                | -       | -               |
| OrientedCapsule  | -     | -             | -   | -          | -      | -    | -   | -     | -        | -       | -        | -                | -       | -               |
