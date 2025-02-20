using Detach;
using Detach.Collisions;
using Detach.Collisions.Primitives3D;
using Detach.Numerics;
using Hexa.NET.ImGui;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Demos.Collisions3D.Services.Ui;

internal sealed class AlgorithmSelectWindow
{
	private readonly Delegate[] _algorithms;
	private readonly string _algorithmsComboString;

	private readonly CollisionAlgorithmState _collisionAlgorithmState;

	private int _selectedAlgorithmIndex;

	public AlgorithmSelectWindow(CollisionAlgorithmState collisionAlgorithmState)
	{
		_collisionAlgorithmState = collisionAlgorithmState;

		const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public;
		MethodInfo[] methods = typeof(Geometry2D).GetMethods(bindingFlags).Concat(typeof(Geometry3D).GetMethods(bindingFlags)).ToArray();
		_algorithms = new Delegate[methods.Length];
		for (int i = 0; i < methods.Length; i++)
			_algorithms[i] = CreateDelegate(methods[i]);

		_algorithmsComboString = string.Join("\0", methods.Select(m => m.Name));

		static Delegate CreateDelegate(MethodInfo method)
		{
			return Delegate.CreateDelegate(
				type: Expression.GetDelegateType(
					method.GetParameters()
						.Select(p => p.ParameterType)
						.Concat([method.ReturnType])
						.ToArray()),
				firstArgument: null,
				method: method);
		}
	}

	public void Render()
	{
		if (ImGui.Begin("Algorithm Selector"))
		{
			RenderAlgorithmSelector();
		}

		ImGui.End();
	}

	private void RenderAlgorithmSelector()
	{
		if (ImGui.Combo("Algorithm", ref _selectedAlgorithmIndex, _algorithmsComboString, 100))
			_collisionAlgorithmState.SelectAlgorithm(_algorithms[_selectedAlgorithmIndex]);

		if (_collisionAlgorithmState.SelectedAlgorithm == null)
			return;

		ParameterInfo[] parameters = _collisionAlgorithmState.SelectedAlgorithm.Method.GetParameters();
		if (parameters.Length == 0)
			return;

		for (int i = 0; i < parameters.Length; i++)
		{
			ParameterInfo parameter = parameters[i];
			RenderParameter(parameter.Name, i, parameter.ParameterType, ref CollectionsMarshal.AsSpan(_collisionAlgorithmState.Arguments)[i]);
		}
	}

	private static void RenderParameter(string? parameterName, int index, Type type, ref object? value)
	{
		ImGui.SeparatorText(parameterName);

		if (value == null || value == DBNull.Value)
		{
			ImGui.TextColored(Rgba.Red, "Parameter value is null.");
			return;
		}

		if (type == typeof(float))
		{
			float cast = (float)value;
			ImGui.SliderFloat($"{parameterName}##{index}", ref cast, -10, 10);
			value = cast;
		}
		else if (type == typeof(Vector3))
		{
			Vector3 cast = (Vector3)value;
			ImGui.SliderFloat3($"{parameterName}##{index}", ref cast, -10, 10);
			value = cast;
		}
		else if (type == typeof(Aabb))
		{
			Aabb cast = (Aabb)value;
			ImGui.SliderFloat3($"{parameterName}.Center##{index}", ref cast.Center.X, -10, 10);
			ImGui.SliderFloat3($"{parameterName}.Size##{index}", ref cast.Size.X, 0, 10);
			value = cast;
		}
		else if (type == typeof(ConeFrustum))
		{
			ConeFrustum cast = (ConeFrustum)value;
			ImGui.SliderFloat3($"{parameterName}BottomCenter##{index}", ref cast.BottomCenter.X, -10, 10);
			ImGui.SliderFloat($"{parameterName}BottomRadius##{index}", ref cast.BottomRadius, 0, 10);
			ImGui.SliderFloat($"{parameterName}TopRadius##{index}", ref cast.TopRadius, 0, 10);
			ImGui.SliderFloat($"{parameterName}Height##{index}", ref cast.Height, 0, 10);
			value = cast;
		}
		else if (type == typeof(Cylinder))
		{
			Cylinder cast = (Cylinder)value;
			ImGui.SliderFloat3(Inline.Utf8($"BottomCenter##{index}"), ref cast.BottomCenter.X, -10, 10);
			ImGui.SliderFloat(Inline.Utf8($"Radius##{index}"), ref cast.Radius, 0, 10);
			ImGui.SliderFloat(Inline.Utf8($"Height##{index}"), ref cast.Height, 0, 10);
			value = cast;
		}
		else if (type == typeof(LineSegment3D))
		{
			LineSegment3D cast = (LineSegment3D)value;
			ImGui.SliderFloat3(Inline.Utf8($"Start##{index}"), ref cast.Start.X, -10, 10);
			ImGui.SliderFloat3(Inline.Utf8($"End##{index}"), ref cast.End.X, -10, 10);
			value = cast;
		}
		else if (type == typeof(Obb))
		{
			Obb cast = (Obb)value;
			ImGui.SliderFloat3(Inline.Utf8($"Center##{index}"), ref cast.Center.X, -10, 10);
			ImGui.SliderFloat3(Inline.Utf8($"HalfExtents##{index}"), ref cast.HalfExtents.X, 0, 10);
			ImGui.SliderFloat3(Inline.Utf8($"Orientation##{index}_1"), ref cast.Orientation.M11, -1, 1);
			ImGui.SliderFloat3(Inline.Utf8($"Orientation##{index}_2"), ref cast.Orientation.M21, -1, 1);
			ImGui.SliderFloat3(Inline.Utf8($"Orientation##{index}_3"), ref cast.Orientation.M31, -1, 1);
			value = cast;
		}
		else if (type == typeof(Pyramid))
		{
			Pyramid cast = (Pyramid)value;
			ImGui.SliderFloat3(Inline.Utf8($"Center##{index}"), ref cast.Center.X, -10, 10);
			ImGui.SliderFloat3(Inline.Utf8($"Size##{index}"), ref cast.Size.X, -1, 1);
			value = cast;
		}
		else if (type == typeof(Ray))
		{
			Ray cast = (Ray)value;
			ImGui.SliderFloat3(Inline.Utf8($"Origin##{index}"), ref cast.Origin.X, -10, 10);
			ImGui.SliderFloat3(Inline.Utf8($"Direction##{index}"), ref cast.Direction.X, -1, 1);
			if (ImGui.Button(Inline.Utf8($"Normalize direction##{index}")))
				cast.Direction = Vector3.Normalize(cast.Direction);
			value = cast;
		}
		else if (type == typeof(Sphere))
		{
			Sphere cast = (Sphere)value;
			ImGui.SliderFloat3(Inline.Utf8($"Center##{index}"), ref cast.Center.X, -10, 10);
			ImGui.SliderFloat(Inline.Utf8($"Radius##{index}"), ref cast.Radius, 0, 10);
			value = cast;
		}
		else if (type == typeof(SphereCast))
		{
			SphereCast cast = (SphereCast)value;
			ImGui.SliderFloat3(Inline.Utf8($"Start##{index}"), ref cast.Start.X, -10, 10);
			ImGui.SliderFloat3(Inline.Utf8($"End##{index}"), ref cast.End.X, -10, 10);
			ImGui.SliderFloat(Inline.Utf8($"Radius##{index}"), ref cast.Radius, 0, 10);
			value = cast;
		}
		else if (type == typeof(Triangle3D))
		{
			Triangle3D cast = (Triangle3D)value;
			ImGui.SliderFloat3(Inline.Utf8($"Position A##{index}"), ref cast.A.X, -10, 10);
			ImGui.SliderFloat3(Inline.Utf8($"Position B##{index}"), ref cast.B.X, -10, 10);
			ImGui.SliderFloat3(Inline.Utf8($"Position C##{index}"), ref cast.C.X, -10, 10);
			value = cast;
		}
		else
		{
			ImGui.TextColored(Rgba.Red, $"Unsupported parameter type: {type.Name}");
		}
	}
}
