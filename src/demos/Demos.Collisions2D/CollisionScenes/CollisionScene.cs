using System.Diagnostics;

namespace Demos.Collisions2D.CollisionScenes;

internal abstract class CollisionScene<T1, T2>(Func<T1, T2, bool> collisionFunction) : ICollisionScene
	where T1 : struct
	where T2 : struct
{
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
		HasCollision = collisionFunction(A, B);
		ExecutionTime = Stopwatch.GetElapsedTime(startTimestamp);
		AllocatedBytes = GC.GetAllocatedBytesForCurrentThread() - allocatedBytesStart;
	}

	public abstract void Render();
}
