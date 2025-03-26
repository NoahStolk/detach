using CollisionFormats.Execution;
using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using Detach.Numerics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Demos.Collisions.Interactable.Services.States;

internal sealed class CollisionAlgorithmState
{
	public IExecutableCollisionAlgorithm? SelectedAlgorithm { get; private set; }
	public List<object> Arguments { get; } = [];
	public List<object> OutArguments { get; } = [];
	public object? ReturnValue { get; private set; }

	public void SelectAlgorithm(IExecutableCollisionAlgorithm algorithm)
	{
		SelectedAlgorithm = algorithm;

		Arguments.Clear();
		foreach ((Type Type, string Name) parameter in algorithm.Parameters)
			Arguments.Add(GetDefault(parameter.Type));
	}

	public void ExecuteAlgorithm()
	{
		if (SelectedAlgorithm == null || Arguments.Contains(DBNull.Value))
			return;

		ExecutionResult executionResult = SelectedAlgorithm.Execute(Arguments);
		OutArguments.Clear();
		OutArguments.AddRange(executionResult.OutArguments);
		ReturnValue = executionResult.ReturnValue;
	}

	private static object GetDefault(Type type)
	{
		return type switch
		{
			_ when type == typeof(Circle) => new Circle(Vector2.Zero, 1),
			_ when type == typeof(CircleCast) => new CircleCast(Vector2.Zero, Vector2.One, 1),
			_ when type == typeof(LineSegment2D) => new LineSegment2D(Vector2.Zero, Vector2.One),
			_ when type == typeof(OrientedRectangle) => new OrientedRectangle(Vector2.Zero, Vector2.One, 0),
			_ when type == typeof(Rectangle) => Rectangle.FromCenter(Vector2.Zero, Vector2.One),
			_ when type == typeof(Triangle2D) => new Triangle2D(Vector2.Zero, Vector2.One, Vector2.UnitX),

			_ when type == typeof(Aabb) => new Aabb(Vector3.Zero, Vector3.One),
			_ when type == typeof(ConeFrustum) => new ConeFrustum(Vector3.Zero, 1, 0.5f, 1),
			_ when type == typeof(Cylinder) => new Cylinder(Vector3.Zero, 1, 1),
			_ when type == typeof(LineSegment3D) => new LineSegment3D(Vector3.Zero, Vector3.One),
			_ when type == typeof(Obb) => new Obb(Vector3.Zero, Vector3.One, Matrix3.Identity),
			_ when type == typeof(OrientedPyramid) => new OrientedPyramid(Vector3.Zero, Vector3.One, Matrix3.Identity),
			_ when type == typeof(Plane) => new Plane(Vector3.UnitY, 1),
			_ when type == typeof(Pyramid) => new Pyramid(Vector3.Zero, Vector3.One),
			_ when type == typeof(Ray) => new Ray(Vector3.Zero, Vector3.UnitX),
			_ when type == typeof(Sphere) => new Sphere(Vector3.Zero, 1),
			_ when type == typeof(SphereCast) => new SphereCast(Vector3.Zero, Vector3.One, 1),
			_ when type == typeof(Triangle3D) => new Triangle3D(Vector3.Zero, Vector3.One, Vector3.UnitX),
			_ when type == typeof(ViewFrustum) => new ViewFrustum(Matrix4x4.Identity),

			_ => Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Could not create default instance of type {type}."),
		};
	}
}
