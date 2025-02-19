namespace Demos.Collisions2D.CollisionScenes;

internal interface ICollisionScene
{
	long AllocatedBytes { get; }

	TimeSpan ExecutionTime { get; }

	void Update(float dt);

	void Collide();

	void Render();
}
