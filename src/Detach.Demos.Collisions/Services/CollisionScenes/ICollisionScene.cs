namespace Detach.Demos.Collisions.Services.CollisionScenes;

public interface ICollisionScene
{
	long AllocatedBytes { get; }

	TimeSpan ExecutionTime { get; }

	void Update(float dt);

	void Collide();

	void Render();
}
