using Detach.Collisions;
using Detach.Collisions.Primitives3D;
using System.Numerics;

namespace Detach.Demos.Collisions.CollisionScenes.ThreeDimensional;

public sealed class PointInSphere : CollisionScene<Vector3, Sphere>
{
	private const float _pointOffset = 64;
	private const float _circleOffset = 128;

	public PointInSphere()
		: base(Geometry3D.PointInSphere)
	{
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		float doubleTime = TotalTime * 2;
		A = new Vector3(MathF.Cos(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset, MathF.Sin(doubleTime) * _pointOffset);
		B = new Sphere(
			new Vector3(MathF.Cos(TotalTime) * _circleOffset, MathF.Sin(TotalTime) * _circleOffset, MathF.Sin(TotalTime) * _circleOffset),
			64 + MathF.Sin(TotalTime) * 32);
	}

	public override void Render()
	{

	}
}
