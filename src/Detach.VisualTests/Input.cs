using Silk.NET.GLFW;
using System.Numerics;

namespace Detach.VisualTests;

public static class Input
{
	private const int _maxMouseButtons = 8;

	private static readonly bool[] _mouseButtonsCurrent = new bool[_maxMouseButtons];
	private static double _mouseWheel;

	public static int Scroll => _mouseWheel > 0 ? 1 : _mouseWheel < 0 ? -1 : 0;

	public static void ButtonCallback(MouseButton mouseButton, InputAction inputState)
	{
		if (mouseButton >= 0 && (int)mouseButton < _maxMouseButtons && inputState is InputAction.Press or InputAction.Release)
		{
			_mouseButtonsCurrent[(int)mouseButton] = inputState == InputAction.Press;
		}
	}

	public static bool IsButtonHeld(MouseButton mouseButton)
	{
		return _mouseButtonsCurrent[(int)mouseButton];
	}

	public static void MouseWheelCallback(double delta)
	{
		_mouseWheel = delta;
	}

	public static unsafe Vector2 GetMousePosition()
	{
		Graphics.Glfw.GetCursorPos(Graphics.Window, out double x, out double y);
		return new((float)x, (float)y);
	}
}
