using Detach;
using Detach.Collisions.Primitives3D;
using Detach.Numerics;
using Hexa.NET.ImGui;
using System.Numerics;

namespace Demos.Collisions3D.Services.Ui;

internal sealed class ShapeSelectWindow(ShapesState shapesState)
{
	public void Render()
	{
		if (ImGui.Begin("Shapes"))
		{
			RenderShapeSelector(0, ref shapesState.SelectedShapeA);
			RenderShapeSelector(1, ref shapesState.SelectedShapeB);
		}

		ImGui.End();
	}

	private static void RenderShapeSelector(int index, ref Shape shape)
	{
		int shapeIndex = shape.CaseIndex;
		if (ImGui.Combo(Inline.Utf8($"Shape{index}"), ref shapeIndex, "Aabb\0ConeFrustum\0Cylinder\0Frustum\0LineSegment3D\0Obb\0Ray\0Sphere\0SphereCast\0Triangle3D\0"u8))
		{
			shape = shapeIndex switch
			{
				0 => Shape.Aabb(new Aabb(Vector3.Zero, Vector3.One)),
				1 => Shape.ConeFrustum(new ConeFrustum(Vector3.Zero, 4, 2, 4)),
				2 => Shape.Cylinder(new Cylinder(Vector3.Zero, 3, 3)),
				3 => Shape.Frustum(new Frustum(Matrix4x4.Identity)),
				4 => Shape.LineSegment3D(new LineSegment3D(Vector3.Zero, Vector3.One)),
				5 => Shape.Obb(new Obb(Vector3.Zero, Vector3.One, Matrix3.Identity)),
				6 => Shape.Ray(new Ray(Vector3.Zero, Vector3.One)),
				7 => Shape.Sphere(new Sphere(Vector3.Zero, 1)),
				8 => Shape.SphereCast(new SphereCast(Vector3.Zero, Vector3.One, 1)),
				9 => Shape.Triangle3D(new Triangle3D(Vector3.Zero, Vector3.UnitX, Vector3.UnitY)),
				_ => throw new InvalidOperationException($"Invalid shape index: {shapeIndex}"),
			};
		}
	}
}
