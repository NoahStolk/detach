using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using Detach.Numerics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;

namespace Demos.Collisions.Interactable.Services.States;

internal sealed class CollisionAlgorithmState
{
	public Delegate? SelectedAlgorithm { get; private set; }
	public List<object?> Arguments { get; } = [];
	public object? ReturnValue { get; private set; }

	[MemberNotNullWhen(true, nameof(SelectedAlgorithm))]
	public bool StateIsValid => SelectedAlgorithm != null && !Arguments.Contains(null) && !Arguments.Contains(DBNull.Value);

	public void SelectAlgorithm(Delegate algorithm)
	{
		SelectedAlgorithm = algorithm;

		Arguments.Clear();
		foreach (ParameterInfo parameter in algorithm.Method.GetParameters())
			Arguments.Add(GetDefault(parameter.ParameterType));
	}

	public void ExecuteAlgorithm()
	{
		if (!StateIsValid)
			return;

		MethodInfo method = SelectedAlgorithm.Method;

		if (method.ReturnType == typeof(bool))
			ReturnValue = ExecuteAlgorithm<bool>();
		else if (method.ReturnType == typeof(Vector3))
			ReturnValue = ExecuteAlgorithm<Vector3>();
		else
			ReturnValue = null;
	}

	private TResult ExecuteAlgorithm<TResult>()
	{
		if (!StateIsValid)
			throw new InvalidOperationException("The algorithm or its arguments are not set.");

		object?[] argsArray = Arguments.ToArray(); // Must use array because of params.
		TResult? result = (TResult?)SelectedAlgorithm.DynamicInvoke(argsArray);
		if (result is null)
			throw new InvalidOperationException("The algorithm did not return a result.");

		return result;
	}

	private static object? GetDefault(Type type)
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

			_ => Activator.CreateInstance(type),
		};
	}
}
