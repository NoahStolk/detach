using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace Detach.VisualTests;

public static class Graphics
{
	private static Glfw? _glfw;
	private static GL? _gl;

	public static Glfw Glfw => _glfw ?? throw new InvalidOperationException("GLFW is not initialized.");
	public static GL Gl => _gl ?? throw new InvalidOperationException("OpenGL is not initialized.");

	public static unsafe WindowHandle* Window { get; private set; }

	public static Action<int, int>? OnChangeWindowSize { get; set; }

	public static unsafe void CreateWindow()
	{
		_glfw = Glfw.GetApi();
		_glfw.Init();

		_glfw.WindowHint(WindowHintInt.ContextVersionMajor, 3);
		_glfw.WindowHint(WindowHintInt.ContextVersionMinor, 3);
		_glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);

		_glfw.WindowHint(WindowHintBool.Focused, true);
		_glfw.WindowHint(WindowHintBool.Resizable, true);

		Window = _glfw.CreateWindow(Constants.WindowWidth, Constants.WindowHeight, "Detach - Visual Tests", null, null);
		if (Window == null)
			throw new InvalidOperationException("Could not create window.");

		_glfw.SetFramebufferSizeCallback(Window, (_, w, h) => SetWindowSize(w, h));
		_glfw.SetCursorPosCallback(Window, (_, x, y) => Input.GlfwInput.CursorPosCallback(x, y));
		_glfw.SetScrollCallback(Window, (_, _, y) => Input.GlfwInput.MouseWheelCallback(y));
		_glfw.SetMouseButtonCallback(Window, (_, button, state, _) => Input.GlfwInput.MouseButtonCallback(button, state));
		_glfw.SetKeyCallback(Window, (_, keys, _, state, _) => Input.GlfwInput.KeyCallback(keys, state));
		_glfw.SetCharCallback(Window, (_, codepoint) => Input.GlfwInput.CharCallback(codepoint));

		_glfw.MakeContextCurrent(Window);
		_gl = GL.GetApi(_glfw.GetProcAddress);
	}

	private static void SetWindowSize(int width, int height)
	{
		Gl.Viewport(0, 0, (uint)width, (uint)height);
		OnChangeWindowSize?.Invoke(width, height);
	}
}
