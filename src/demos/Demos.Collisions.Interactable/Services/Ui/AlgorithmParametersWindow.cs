using CollisionFormats.Model;
using Demos.Collisions.Interactable.Services.States;
using Detach;
using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using Detach.GlfwExtensions;
using Detach.Numerics;
using Hexa.NET.ImGui;
using Silk.NET.GLFW;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Demos.Collisions.Interactable.Services.Ui;

internal sealed class AlgorithmParametersWindow(CollisionAlgorithmState collisionAlgorithmState, CollisionScenarioState collisionScenarioState, SelectionState selectionState, GlfwInput glfwInput)
{
	public void Render()
	{
		ImGui.SetNextWindowSizeConstraints(new Vector2(960, 320), new Vector2(4096));
		if (ImGui.Begin("Algorithm Parameters"))
			RenderAlgorithmParameters();

		ImGui.End();
	}

	private void RenderAlgorithmParameters()
	{
		if (collisionAlgorithmState.SelectedAlgorithm == null)
			return;

		IReadOnlyList<(Type Type, string Name)> parameters = collisionAlgorithmState.SelectedAlgorithm.Parameters;
		for (int i = 0; i < parameters.Count; i++)
		{
			(Type Type, string Name) parameter = parameters[i];
			RenderParameter(parameter.Name, i, parameter.Type, ref CollectionsMarshal.AsSpan(collisionAlgorithmState.Arguments)[i]);
		}

		ImGui.SeparatorText("Return value");
		ImGui.Text(collisionAlgorithmState.ReturnValue?.ToString() ?? "<NULL>");

		if (ImGui.Button("Add scenario") || glfwInput.IsKeyPressed(Keys.Q))
			AddScenario(false);

		if (ImGui.Button("Add incorrect scenario") || glfwInput.IsKeyPressed(Keys.E))
			AddScenario(true);
	}

	private void AddScenario(bool incorrect)
	{
		CollisionAlgorithm? algorithm = collisionScenarioState.GetAlgorithm(selectionState.SelectedAlgorithmIndex);
		if (algorithm != null)
		{
			CollisionAlgorithmScenario collisionAlgorithmScenario = new(
				collisionAlgorithmState.Arguments,
				collisionAlgorithmState.OutArguments,
				collisionAlgorithmState.ReturnValue,
				incorrect);
			collisionScenarioState.AddScenario(algorithm.MethodSignature, collisionAlgorithmScenario);
		}
	}

	private static void RenderParameter(string? parameterName, int index, Type type, ref object value)
	{
		ImGui.SeparatorText(parameterName);

		if (value == DBNull.Value)
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
			if (ImGui.Button($"Swap line positions##{index}"))
				cast = new LineSegment2D(cast.End, cast.Start);

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
			if (ImGui.Button($"Swap triangle positions##{index}"))
				cast = new Triangle2D(cast.C, cast.A, cast.B);

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
			if (ImGui.Button(Inline.Utf8($"Swap line positions##{index}")))
				cast = new LineSegment3D(cast.End, cast.Start);

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
			if (ImGui.Button(Inline.Utf8($"Swap sphere cast positions##{index}")))
				cast = new SphereCast(cast.End, cast.Start, cast.Radius);

			value = cast;
		}
		else if (type == typeof(Triangle3D))
		{
			Triangle3D cast = (Triangle3D)value;
			ImGui.SliderFloat3(Inline.Utf8($"Position A##{index}"), ref cast.A.X, -maxDistance, maxDistance);
			ImGui.SliderFloat3(Inline.Utf8($"Position B##{index}"), ref cast.B.X, -maxDistance, maxDistance);
			ImGui.SliderFloat3(Inline.Utf8($"Position C##{index}"), ref cast.C.X, -maxDistance, maxDistance);
			if (ImGui.Button(Inline.Utf8($"Swap triangle positions##{index}")))
				cast = new Triangle3D(cast.C, cast.A, cast.B);

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

		bool y = ImGui.SliderAngle(Inline.Utf8($"Orientation.Yaw##{index}"), ref yaw, -180, 180);
		bool p = ImGui.SliderAngle(Inline.Utf8($"Orientation.Pitch##{index}"), ref pitch, -90, 90);
		bool r = ImGui.SliderAngle(Inline.Utf8($"Orientation.Roll##{index}"), ref roll, -180, 180);
		if (y || p || r)
			orientation = Matrix3.Rotation(yaw, pitch, roll);

		if (ImGui.Button(Inline.Utf8($"Identity##{index}")))
			orientation = Matrix3.Identity;

		ImGui.Text(Inline.Utf8(orientation, "0.00"));

		ImGui.SliderFloat(Inline.Utf8($"Orientation.M11##{index}"), ref orientation.M11, -1, 1);
		ImGui.SliderFloat(Inline.Utf8($"Orientation.M12##{index}"), ref orientation.M12, -1, 1);
		ImGui.SliderFloat(Inline.Utf8($"Orientation.M13##{index}"), ref orientation.M13, -1, 1);
		ImGui.SliderFloat(Inline.Utf8($"Orientation.M21##{index}"), ref orientation.M21, -1, 1);
		ImGui.SliderFloat(Inline.Utf8($"Orientation.M22##{index}"), ref orientation.M22, -1, 1);
		ImGui.SliderFloat(Inline.Utf8($"Orientation.M23##{index}"), ref orientation.M23, -1, 1);
		ImGui.SliderFloat(Inline.Utf8($"Orientation.M31##{index}"), ref orientation.M31, -1, 1);
		ImGui.SliderFloat(Inline.Utf8($"Orientation.M32##{index}"), ref orientation.M32, -1, 1);
		ImGui.SliderFloat(Inline.Utf8($"Orientation.M33##{index}"), ref orientation.M33, -1, 1);
	}
}
