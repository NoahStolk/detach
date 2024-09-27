using System.Diagnostics;

namespace Detach.Demos.Collisions.CollisionScenes;

public abstract class CollisionScene<T1, T2> : ICollisionScene
	where T1 : struct
	where T2 : struct
{
	private readonly Func<T1, T2, bool> _collisionFunction;

	protected CollisionScene(Func<T1, T2, bool> collisionFunction)
	{
		_collisionFunction = collisionFunction;
	}

	protected bool HasCollision { get; private set; }

	protected float TotalTime { get; private set; }

	protected T1 A { get; set; }

	protected T2 B { get; set; }

	public long AllocatedBytes { get; private set; }

	public TimeSpan ExecutionTime { get; private set; }

	public virtual void Update(float dt)
	{
		TotalTime += dt;
	}

	public void Collide()
	{
		long allocatedBytesStart = GC.GetAllocatedBytesForCurrentThread();
		long startTimestamp = Stopwatch.GetTimestamp();
		HasCollision = _collisionFunction(A, B);
		ExecutionTime = Stopwatch.GetElapsedTime(startTimestamp);
		AllocatedBytes = GC.GetAllocatedBytesForCurrentThread() - allocatedBytesStart;
	}

	public abstract void Render();
}
