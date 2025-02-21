using CollisionFormats.Serialization;
using Detach.Collisions.Primitives2D;

namespace Demos.Collisions.Interactable.Services.States;

internal sealed class CollisionScenarioState
{
	public CollisionScenario<Circle, Circle> CircleCircle { get; } = new("CircleCircle", [], []);
	public CollisionScenario<Circle, Rectangle> CircleRectangle { get; } = new("CircleRectangle", [], []);
	public CollisionScenario<Circle, OrientedRectangle> CircleOrientedRectangle { get; } = new("CircleOrientedRectangle", [], []);
	public CollisionScenario<Circle, Triangle2D> CircleTriangle { get; } = new("CircleTriangle", [], []);

	public CollisionScenario<Rectangle, Rectangle> RectangleRectangle { get; } = new("RectangleRectangle", [], []);
	public CollisionScenario<Rectangle, OrientedRectangle> RectangleOrientedRectangle { get; } = new("RectangleOrientedRectangle", [], []);
}
