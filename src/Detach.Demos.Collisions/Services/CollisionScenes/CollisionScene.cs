namespace Detach.Demos.Collisions.Services.CollisionScenes;

public abstract class CollisionScene<T1, T2> : ICollisionScene
	where T1 : struct
	where T2 : struct
{
	protected bool HasCollision { get; set; }

	protected float TotalTime { get; private set; }

	protected T1 A { get; set; }

	protected T2 B { get; set; }

	public virtual void Update(float dt)
	{
		TotalTime += dt;
	}

	public abstract void Render();
}
