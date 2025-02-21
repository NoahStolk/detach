using Demos.Collisions.Interactable.Extensions;
using Detach.GlfwExtensions;
using Silk.NET.GLFW;
using System.Numerics;

namespace Demos.Collisions.Interactable.Services;

internal sealed unsafe class Camera
{
	private const MouseButton _lookButton = MouseButton.Right;
	private const MouseButton _panButton = MouseButton.Middle;

	private readonly GlfwInput _glfwInput;
	private readonly Glfw _glfw;
	private readonly WindowHandle* _window;

	private CameraMode _mode = CameraMode.None;
	private Vector2 _originalCursor;
	private float _yaw = MathF.PI * 0.25f;
	private float _pitch = -0.5f;
	private Quaternion _rotation;

	public Vector3 Position = new(0, 4, -4);
	public Vector3 Target = Vector3.Zero;
	public float FieldOfView = 75;
	public float Zoom = 4;

	public Camera(GlfwInput glfwInput, Glfw glfw, WindowHandle* window)
	{
		_glfwInput = glfwInput;
		_glfw = glfw;
		_window = window;

		_originalCursor = _glfwInput.CursorPosition;
		_rotation = Quaternion.CreateFromYawPitchRoll(_yaw, -_pitch, 0);
	}

	private enum CameraMode
	{
		None,
		Look,
		Pan,
	}

	public void Update(float dt, bool activateControls)
	{
		if (activateControls)
		{
			HandleMouse();
			HandleKeyboard(dt);
		}

		Position = Target + Vector3.Transform(new Vector3(0, 0, -Zoom), _rotation);
	}

	private void HandleMouse()
	{
		float scroll = _glfwInput.MouseWheelY;
		if (!scroll.IsZero() && !_glfwInput.IsKeyDown(Keys.ControlLeft) && !_glfwInput.IsKeyDown(Keys.ControlRight))
			Zoom = Math.Clamp(Zoom - scroll, 1, 50);

		Vector2 cursor = _glfwInput.CursorPosition;

		if (_mode == CameraMode.None && (_glfwInput.IsMouseButtonDown(_lookButton) || _glfwInput.IsMouseButtonDown(_panButton)))
		{
			_glfw.SetInputMode(_window, CursorStateAttribute.Cursor, CursorModeValue.CursorHidden);
			_originalCursor = cursor;
			_mode = _glfwInput.IsMouseButtonDown(_lookButton) ? CameraMode.Look : CameraMode.Pan;
		}
		else if (_mode != CameraMode.None && !_glfwInput.IsMouseButtonDown(_lookButton) && !_glfwInput.IsMouseButtonDown(_panButton))
		{
			_glfw.SetInputMode(_window, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
			_mode = CameraMode.None;
		}

		if (_mode == CameraMode.None)
			return;

		Vector2 delta = cursor - _originalCursor;
		if (_mode == CameraMode.Look)
		{
			const float lookSpeed = 20;
			_yaw -= lookSpeed * delta.X * 0.0001f;
			_pitch -= lookSpeed * delta.Y * 0.0001f;
			_pitch = Math.Clamp(_pitch, -MathF.PI * 0.4999f, MathF.PI * 0.4999f);
			_rotation = Quaternion.CreateFromYawPitchRoll(_yaw, -_pitch, 0);

			_glfw.SetCursorPos(_window, _originalCursor.X, _originalCursor.Y);
		}
		else if (_mode == CameraMode.Pan)
		{
			float multiplier = 0.0005f * Zoom;
			Target -= Vector3.Transform(new Vector3(-delta.X * multiplier, -delta.Y * multiplier, 0), _rotation);

			_glfw.SetCursorPos(_window, _originalCursor.X, _originalCursor.Y);
		}
	}

	private void HandleKeyboard(float dt)
	{
		const float speed = 15f;
		if (_glfwInput.IsKeyDown(Keys.A))
			Target += Vector3.Transform(new Vector3(speed, 0, 0), _rotation) * dt;
		if (_glfwInput.IsKeyDown(Keys.D))
			Target -= Vector3.Transform(new Vector3(speed, 0, 0), _rotation) * dt;
		if (_glfwInput.IsKeyDown(Keys.W))
			Target += Vector3.Transform(new Vector3(0, 0, speed), _rotation) * dt;
		if (_glfwInput.IsKeyDown(Keys.S))
			Target -= Vector3.Transform(new Vector3(0, 0, speed), _rotation) * dt;
		if (_glfwInput.IsKeyDown(Keys.Space))
			Target += Vector3.Transform(new Vector3(0, speed, 0), _rotation) * dt;
		if (_glfwInput.IsKeyDown(Keys.ShiftLeft))
			Target -= Vector3.Transform(new Vector3(0, speed, 0), _rotation) * dt;
	}
}
