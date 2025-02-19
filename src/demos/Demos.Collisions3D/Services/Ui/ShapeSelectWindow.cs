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
		ImGui.SeparatorText(Inline.Utf8($"Shape {index}"));

		int shapeIndex = shape.CaseIndex;
		if (ImGui.Combo(Inline.Utf8($"Shape{index}"), ref shapeIndex, "Aabb\0ConeFrustum\0Cylinder\0LineSegment3D\0Obb\0Ray\0Sphere\0SphereCast\0Triangle3D\0"u8))
		{
			shape = shapeIndex switch
			{
				0 => Shape.Aabb(new Aabb(Vector3.Zero, Vector3.One)),
				1 => Shape.ConeFrustum(new ConeFrustum(Vector3.Zero, 4, 2, 4)),
				2 => Shape.Cylinder(new Cylinder(Vector3.Zero, 3, 3)),
				3 => Shape.LineSegment3D(new LineSegment3D(Vector3.Zero, Vector3.One)),
				4 => Shape.Obb(new Obb(Vector3.Zero, Vector3.One, Matrix3.Identity)),
				5 => Shape.Ray(new Ray(Vector3.Zero, Vector3.One)),
				6 => Shape.Sphere(new Sphere(Vector3.Zero, 1)),
				7 => Shape.SphereCast(new SphereCast(Vector3.Zero, Vector3.One, 1)),
				8 => Shape.Triangle3D(new Triangle3D(Vector3.Zero, Vector3.UnitX, Vector3.UnitY)),
				_ => throw new InvalidOperationException($"Invalid shape index: {shapeIndex}"),
			};
		}

		switch (shape.CaseIndex)
		{
			case Shape.AabbIndex:
				ImGui.SliderFloat3(Inline.Utf8($"Center##{index}"), ref shape.AabbData.Center.X, -10, 10);
				ImGui.SliderFloat3(Inline.Utf8($"Size##{index}"), ref shape.AabbData.Size.X, 0, 10);
				break;
			case Shape.ConeFrustumIndex:
				ImGui.SliderFloat3(Inline.Utf8($"BottomCenter##{index}"), ref shape.ConeFrustumData.BottomCenter.X, -10, 10);
				ImGui.SliderFloat(Inline.Utf8($"BottomRadius##{index}"), ref shape.ConeFrustumData.BottomRadius, 0, 10);
				ImGui.SliderFloat(Inline.Utf8($"TopRadius##{index}"), ref shape.ConeFrustumData.TopRadius, 0, 10);
				ImGui.SliderFloat(Inline.Utf8($"Height##{index}"), ref shape.ConeFrustumData.Height, 0, 10);
				break;
			case Shape.CylinderIndex:
				ImGui.SliderFloat3(Inline.Utf8($"BottomCenter##{index}"), ref shape.CylinderData.BottomCenter.X, -10, 10);
				ImGui.SliderFloat(Inline.Utf8($"Radius##{index}"), ref shape.CylinderData.Radius, 0, 10);
				ImGui.SliderFloat(Inline.Utf8($"Height##{index}"), ref shape.CylinderData.Height, 0, 10);
				break;
			case Shape.LineSegment3DIndex:
				ImGui.SliderFloat3(Inline.Utf8($"Start##{index}"), ref shape.LineSegment3DData.Start.X, -10, 10);
				ImGui.SliderFloat3(Inline.Utf8($"End##{index}"), ref shape.LineSegment3DData.End.X, -10, 10);
				break;
			case Shape.ObbIndex:
				ImGui.SliderFloat3(Inline.Utf8($"Center##{index}"), ref shape.ObbData.Center.X, -10, 10);
				ImGui.SliderFloat3(Inline.Utf8($"HalfExtents##{index}"), ref shape.ObbData.HalfExtents.X, 0, 10);
				ImGui.SliderFloat3(Inline.Utf8($"Orientation##{index}_1"), ref shape.ObbData.Orientation.M11, -1, 1);
				ImGui.SliderFloat3(Inline.Utf8($"Orientation##{index}_2"), ref shape.ObbData.Orientation.M21, -1, 1);
				ImGui.SliderFloat3(Inline.Utf8($"Orientation##{index}_3"), ref shape.ObbData.Orientation.M31, -1, 1);
				break;
			case Shape.RayIndex:
				ImGui.SliderFloat3(Inline.Utf8($"Origin##{index}"), ref shape.RayData.Origin.X, -10, 10);
				ImGui.SliderFloat3(Inline.Utf8($"Direction##{index}"), ref shape.RayData.Direction.X, -1, 1);
				if (ImGui.Button(Inline.Utf8($"Normalize direction##{index}")))
					shape.RayData.Direction = Vector3.Normalize(shape.RayData.Direction);
				break;
			case Shape.SphereIndex:
				ImGui.SliderFloat3(Inline.Utf8($"Center##{index}"), ref shape.SphereData.Center.X, -10, 10);
				ImGui.SliderFloat(Inline.Utf8($"Radius##{index}"), ref shape.SphereData.Radius, 0, 10);
				break;
			case Shape.SphereCastIndex:
				ImGui.SliderFloat3(Inline.Utf8($"Start##{index}"), ref shape.SphereCastData.Start.X, -10, 10);
				ImGui.SliderFloat3(Inline.Utf8($"End##{index}"), ref shape.SphereCastData.End.X, -10, 10);
				ImGui.SliderFloat(Inline.Utf8($"Radius##{index}"), ref shape.SphereCastData.Radius, 0, 10);
				break;
			case Shape.Triangle3DIndex:
				ImGui.SliderFloat3(Inline.Utf8($"Position A##{index}"), ref shape.Triangle3DData.A.X, -10, 10);
				ImGui.SliderFloat3(Inline.Utf8($"Position B##{index}"), ref shape.Triangle3DData.B.X, -10, 10);
				ImGui.SliderFloat3(Inline.Utf8($"Position C##{index}"), ref shape.Triangle3DData.C.X, -10, 10);
				break;
		}
	}
}
