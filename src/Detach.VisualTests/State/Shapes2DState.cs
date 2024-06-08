using Detach.Collisions.Primitives;
using System.Numerics;

namespace Detach.VisualTests.State;

public static class Shapes2DState
{
	public static List<LineSegment2D> LineSegments { get; } = [];
	public static List<Circle> Circles { get; } = [];
	public static List<Rectangle> Rectangles { get; } = [];
	public static List<OrientedRectangle> OrientedRectangles { get; } = [];

	public static SelectedShapeType SelectedShapeType { get; set; }

	public static bool IsCreatingShape { get; set; }
	public static Vector2 ShapeStart { get; set; }
}
