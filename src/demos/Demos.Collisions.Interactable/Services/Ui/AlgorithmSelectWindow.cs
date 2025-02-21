using Demos.Collisions.Interactable.Services.States;
using Detach;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using Detach.Numerics;
using Hexa.NET.ImGui;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Demos.Collisions.Interactable.Services.Ui;

internal sealed class AlgorithmSelectWindow
{
	private readonly Delegate[] _algorithms;
	private readonly string _algorithmsComboString;

	private readonly CollisionAlgorithmState _collisionAlgorithmState;

	private int _selectedAlgorithmIndex;

	public AlgorithmSelectWindow(CollisionAlgorithmState collisionAlgorithmState)
	{
		_collisionAlgorithmState = collisionAlgorithmState;

		// TODO: Remove methods containing ref structs or ref/out parameters.
		const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public;
		MethodInfo[] methods = typeof(Geometry2D).GetMethods(bindingFlags).Concat(typeof(Geometry3D).GetMethods(bindingFlags)).ToArray();
		_algorithms = new Delegate[methods.Length];
		for (int i = 0; i < methods.Length; i++)
			_algorithms[i] = CreateDelegate(methods[i]);

		_algorithmsComboString = string.Join("\0", methods.Select(m => $"{m.Name} ({string.Join(", ", m.GetParameters().Select(p => $"{p.ParameterType.Name}"))})"));
		_algorithmsComboString += "\0";

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
		ImGui.SetNextWindowSizeConstraints(new Vector2(960, 320), new Vector2(4096));
		if (ImGui.Begin("Algorithm Selector"))
			RenderAlgorithmSelector();

		ImGui.End();
	}

	private void RenderAlgorithmSelector()
	{
		if (ImGui.Combo("Algorithm", ref _selectedAlgorithmIndex, _algorithmsComboString, 50))
			_collisionAlgorithmState.SelectAlgorithm(_algorithms[_selectedAlgorithmIndex]);

		if (_collisionAlgorithmState.SelectedAlgorithm == null)
			return;

		ParameterInfo[] parameters = _collisionAlgorithmState.SelectedAlgorithm.Method.GetParameters();
		for (int i = 0; i < parameters.Length; i++)
		{
			ParameterInfo parameter = parameters[i];
			RenderParameter(parameter.Name, i, parameter.ParameterType, ref CollectionsMarshal.AsSpan(_collisionAlgorithmState.Arguments)[i]);
		}

		ImGui.SeparatorText("Return value");
		if (_collisionAlgorithmState.ReturnValue is bool or Vector3)
			ImGui.Text(_collisionAlgorithmState.ReturnValue?.ToString());
		else
			ImGui.TextColored(Rgba.Red, "Unsupported type");
	}

	private static void RenderParameter(string? parameterName, int index, Type type, ref object? value)
	{
		ImGui.SeparatorText(parameterName);

		if (value == null || value == DBNull.Value)
		{
			ImGui.TextColored(Rgba.Red, "Parameter value is null.");
			return;
		}

		const float maxDistance = 5;
		const float maxSize = 3;

		if (type == typeof(float))
		{
			float cast = (float)value;
			ImGui.SliderFloat($"{parameterName}##{index}", ref cast, -maxDistance, maxDistance);
			value = cast;
		}
		else if (type == typeof(Vector2))
		{
			Vector2 cast = (Vector2)value;
			ImGui.SliderFloat2($"{parameterName}##{index}", ref cast, -maxDistance, maxDistance);
			value = cast;
		}
		else if (type == typeof(Circle))
		{
			Circle cast = (Circle)value;
			ImGui.SliderFloat2($"{parameterName}.Center##{index}", ref cast.Center.X, -maxDistance, maxDistance);
			ImGui.SliderFloat($"{parameterName}.Radius##{index}", ref cast.Radius, 0, maxSize);
			value = cast;
		}
		else if (type == typeof(CircleCast))
		{
			CircleCast cast = (CircleCast)value;
			ImGui.SliderFloat2($"{parameterName}.Start##{index}", ref cast.Start.X, -maxDistance, maxDistance);
			ImGui.SliderFloat2($"{parameterName}.End##{index}", ref cast.End.X, -maxDistance, maxDistance);
			ImGui.SliderFloat($"{parameterName}.Radius##{index}", ref cast.Radius, 0, maxSize);
			value = cast;
		}
		else if (type == typeof(LineSegment2D))
		{
			LineSegment2D cast = (LineSegment2D)value;
			ImGui.SliderFloat2($"{parameterName}.Start##{index}", ref cast.Start.X, -maxDistance, maxDistance);
			ImGui.SliderFloat2($"{parameterName}.End##{index}", ref cast.End.X, -maxDistance, maxDistance);
			value = cast;
		}
		else if (type == typeof(OrientedRectangle))
		{
			OrientedRectangle cast = (OrientedRectangle)value;
			ImGui.SliderFloat2($"{parameterName}.Center##{index}", ref cast.Center.X, -maxDistance, maxDistance);
			ImGui.SliderFloat2($"{parameterName}.HalfExtents##{index}", ref cast.HalfExtents.X, 0, maxSize);
			ImGui.SliderAngle($"{parameterName}.RotationInRadians##{index}", ref cast.RotationInRadians, -180, 180);
			value = cast;
		}
		else if (type == typeof(Rectangle))
		{
			Rectangle cast = (Rectangle)value;
			ImGui.SliderFloat2($"{parameterName}.Position##{index}", ref cast.Position.X, -maxDistance, maxDistance);
			ImGui.SliderFloat2($"{parameterName}.Size##{index}", ref cast.Size.X, 0, maxSize);
			value = cast;
		}
		else if (type == typeof(Triangle2D))
		{
			Triangle2D cast = (Triangle2D)value;
			ImGui.SliderFloat2($"{parameterName}.A##{index}", ref cast.A.X, -maxDistance, maxDistance);
			ImGui.SliderFloat2($"{parameterName}.B##{index}", ref cast.B.X, -maxDistance, maxDistance);
			ImGui.SliderFloat2($"{parameterName}.C##{index}", ref cast.C.X, -maxDistance, maxDistance);
			value = cast;
		}
		else if (type == typeof(Vector3))
		{
			Vector3 cast = (Vector3)value;
			ImGui.SliderFloat3($"{parameterName}##{index}", ref cast, -maxDistance, maxDistance);
			value = cast;
		}
		else if (type == typeof(Aabb))
		{
			Aabb cast = (Aabb)value;
			ImGui.SliderFloat3($"{parameterName}.Center##{index}", ref cast.Center.X, -maxDistance, maxDistance);
			ImGui.SliderFloat3($"{parameterName}.Size##{index}", ref cast.Size.X, 0, maxSize);
			value = cast;
		}
		else if (type == typeof(ConeFrustum))
		{
			ConeFrustum cast = (ConeFrustum)value;
			ImGui.SliderFloat3($"{parameterName}BottomCenter##{index}", ref cast.BottomCenter.X, -maxDistance, maxDistance);
			ImGui.SliderFloat($"{parameterName}BottomRadius##{index}", ref cast.BottomRadius, 0, maxSize);
			ImGui.SliderFloat($"{parameterName}TopRadius##{index}", ref cast.TopRadius, 0, maxSize);
			ImGui.SliderFloat($"{parameterName}Height##{index}", ref cast.Height, 0, maxSize);
			value = cast;
		}
		else if (type == typeof(Cylinder))
		{
			Cylinder cast = (Cylinder)value;
			ImGui.SliderFloat3(Inline.Utf8($"BottomCenter##{index}"), ref cast.BottomCenter.X, -maxDistance, maxDistance);
			ImGui.SliderFloat(Inline.Utf8($"Radius##{index}"), ref cast.Radius, 0, maxSize);
			ImGui.SliderFloat(Inline.Utf8($"Height##{index}"), ref cast.Height, 0, maxSize);
			value = cast;
		}
		else if (type == typeof(LineSegment3D))
		{
			LineSegment3D cast = (LineSegment3D)value;
			ImGui.SliderFloat3(Inline.Utf8($"Start##{index}"), ref cast.Start.X, -maxDistance, maxDistance);
			ImGui.SliderFloat3(Inline.Utf8($"End##{index}"), ref cast.End.X, -maxDistance, maxDistance);
			value = cast;
		}
		else if (type == typeof(Obb))
		{
			Obb cast = (Obb)value;
			ImGui.SliderFloat3(Inline.Utf8($"Center##{index}"), ref cast.Center.X, -maxDistance, maxDistance);
			ImGui.SliderFloat3(Inline.Utf8($"HalfExtents##{index}"), ref cast.HalfExtents.X, 0, maxSize);
			SlidersOrientation(index, ref cast.Orientation);
			value = cast;
		}
		else if (type == typeof(OrientedPyramid))
		{
			OrientedPyramid cast = (OrientedPyramid)value;
			ImGui.SliderFloat3(Inline.Utf8($"Center##{index}"), ref cast.Center.X, -maxDistance, maxDistance);
			ImGui.SliderFloat3(Inline.Utf8($"Size##{index}"), ref cast.Size.X, 0, maxSize);
			SlidersOrientation(index, ref cast.Orientation);
			value = cast;
		}
		else if (type == typeof(Plane))
		{
			Plane cast = (Plane)value;
			ImGui.SliderFloat3(Inline.Utf8($"Normal##{index}"), ref cast.Normal.X, -1, 1);
			if (ImGui.Button(Inline.Utf8($"Normalize normal##{index}")))
				cast.Normal = Vector3.Normalize(cast.Normal);
			ImGui.SliderFloat(Inline.Utf8($"D##{index}"), ref cast.D, 0, maxDistance);
			value = cast;
		}
		else if (type == typeof(Pyramid))
		{
			Pyramid cast = (Pyramid)value;
			ImGui.SliderFloat3(Inline.Utf8($"Center##{index}"), ref cast.Center.X, -maxDistance, maxDistance);
			ImGui.SliderFloat3(Inline.Utf8($"Size##{index}"), ref cast.Size.X, 0, maxSize);
			value = cast;
		}
		else if (type == typeof(Ray))
		{
			Ray cast = (Ray)value;
			ImGui.SliderFloat3(Inline.Utf8($"Origin##{index}"), ref cast.Origin.X, -maxDistance, maxDistance);
			ImGui.SliderFloat3(Inline.Utf8($"Direction##{index}"), ref cast.Direction.X, -1, 1);
			if (ImGui.Button(Inline.Utf8($"Normalize direction##{index}")))
				cast.Direction = Vector3.Normalize(cast.Direction);
			value = cast;
		}
		else if (type == typeof(Sphere))
		{
			Sphere cast = (Sphere)value;
			ImGui.SliderFloat3(Inline.Utf8($"Center##{index}"), ref cast.Center.X, -maxDistance, maxDistance);
			ImGui.SliderFloat(Inline.Utf8($"Radius##{index}"), ref cast.Radius, 0, maxSize);
			value = cast;
		}
		else if (type == typeof(SphereCast))
		{
			SphereCast cast = (SphereCast)value;
			ImGui.SliderFloat3(Inline.Utf8($"Start##{index}"), ref cast.Start.X, -maxDistance, maxDistance);
			ImGui.SliderFloat3(Inline.Utf8($"End##{index}"), ref cast.End.X, -maxDistance, maxDistance);
			ImGui.SliderFloat(Inline.Utf8($"Radius##{index}"), ref cast.Radius, 0, maxSize);
			value = cast;
		}
		else if (type == typeof(Triangle3D))
		{
			Triangle3D cast = (Triangle3D)value;
			ImGui.SliderFloat3(Inline.Utf8($"Position A##{index}"), ref cast.A.X, -maxDistance, maxDistance);
			ImGui.SliderFloat3(Inline.Utf8($"Position B##{index}"), ref cast.B.X, -maxDistance, maxDistance);
			ImGui.SliderFloat3(Inline.Utf8($"Position C##{index}"), ref cast.C.X, -maxDistance, maxDistance);
			value = cast;
		}
		else
		{
			ImGui.TextColored(Rgba.Red, $"Unsupported parameter type: {type.Name}");
		}
	}

	private static void SlidersOrientation(int index, ref Matrix3 orientation)
	{
		Matrix3.GetYawPitchRoll(orientation, out float yaw, out float pitch, out float roll);

		ImGui.SliderAngle(Inline.Utf8($"Orientation.Yaw##{index}"), ref yaw, -180, 180);
		ImGui.SliderAngle(Inline.Utf8($"Orientation.Pitch##{index}"), ref pitch, -90, 90);
		ImGui.SliderAngle(Inline.Utf8($"Orientation.Roll##{index}"), ref roll, -180, 180);

		orientation = Matrix3.Rotation(yaw, pitch, roll);
		if (ImGui.Button(Inline.Utf8($"Identity##{index}")))
			orientation = Matrix3.Identity;

		ImGui.Text(Inline.Utf8(orientation, "0.00"));
	}
}
