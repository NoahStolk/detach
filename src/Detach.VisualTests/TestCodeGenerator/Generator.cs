using Detach.Collisions.Primitives2D;
using Detach.VisualTests.Collisions;
using Detach.VisualTests.State;
using System.Text;

namespace Detach.VisualTests.TestCodeGenerator;

public static class Generator
{
	public static string BuildTestCode()
	{
		StringBuilder sb = new();

		for (int i = 0; i < Shapes2DState.LineSegments.Count; i++)
		{
			LineSegment2D lineSegment = Shapes2DState.LineSegments[i];
			sb.AppendLine($"LineSegment2D {GetLocalName(lineSegment, i)} = new(new Vector2({lineSegment.Start.X}f, {lineSegment.Start.Y}f), new Vector2({lineSegment.End.X}f, {lineSegment.End.Y}f));");
		}

		for (int i = 0; i < Shapes2DState.Circles.Count; i++)
		{
			Circle circle = Shapes2DState.Circles[i];
			sb.AppendLine($"Circle {GetLocalName(circle, i)} = new(new Vector2({circle.Position.X}f, {circle.Position.Y}f), {circle.Radius}f);");
		}

		for (int i = 0; i < Shapes2DState.Rectangles.Count; i++)
		{
			Rectangle rectangle = Shapes2DState.Rectangles[i];
			sb.AppendLine($"Rectangle {GetLocalName(rectangle, i)} = new(new Vector2({rectangle.Position.X}f, {rectangle.Position.Y}f), new Vector2({rectangle.Size.X}f, {rectangle.Size.Y}f));");
		}

		for (int i = 0; i < Shapes2DState.OrientedRectangles.Count; i++)
		{
			OrientedRectangle orientedRectangle = Shapes2DState.OrientedRectangles[i];
			sb.AppendLine($"OrientedRectangle {GetLocalName(orientedRectangle, i)} = new(new Vector2({orientedRectangle.Position.X}f, {orientedRectangle.Position.Y}f), new Vector2({orientedRectangle.HalfExtents.X}f, {orientedRectangle.HalfExtents.Y}f), {orientedRectangle.RotationInRadians}f);");
		}

		foreach (CollisionResult cr in CollisionHandler.Collisions)
		{
			string assertion = cr.IsColliding ? "Assert.IsTrue" : "Assert.IsFalse";
			string functionName = $"{GetTypeName(cr.A)}{GetTypeName(cr.B)}";
			if (functionName is "RectangleOrientedRectangle" or "OrientedRectangleOrientedRectangle")
				functionName += "Sat";

			string aName = GetLocalName(cr.A, cr.IndexA);
			string bName = GetLocalName(cr.B, cr.IndexB);
			sb.AppendLine($"{assertion}(Geometry2D.{functionName}({aName}, {bName}));");
		}

		return sb.ToString();

		static string GetLocalName(object obj, int index)
		{
			return FirstCharToLower(GetTypeName(obj)) + index;
		}

		static string GetTypeName(object obj)
		{
			return obj switch
			{
				LineSegment2D => "Line",
				Circle => "Circle",
				Rectangle => "Rectangle",
				OrientedRectangle => "OrientedRectangle",
				_ => "Unknown",
			};
		}

		static string FirstCharToLower(string str)
		{
			return str.Length switch
			{
				0 => string.Empty,
				1 => char.ToLower(str[0]).ToString(),
				_ => char.ToLower(str[0]) + str[1..],
			};
		}
	}
}
