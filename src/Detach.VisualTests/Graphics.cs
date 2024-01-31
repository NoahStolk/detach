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

	public static unsafe void CreateWindow()
	{
		_glfw = Glfw.GetApi();
		_glfw.Init();

		_glfw.WindowHint(WindowHintInt.ContextVersionMajor, 3);
		_glfw.WindowHint(WindowHintInt.ContextVersionMinor, 3);
		_glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);

		_glfw.WindowHint(WindowHintBool.Focused, true);
		_glfw.WindowHint(WindowHintBool.Resizable, true);

		Window = _glfw.CreateWindow(1920, 1080, "Detach - Visual Tests", null, null);
		if (Window == null)
			throw new InvalidOperationException("Could not create window.");

		_glfw.SetMouseButtonCallback(Window, (_, button, state, _) => Input.ButtonCallback(button, state));
		_glfw.SetScrollCallback(Window, (_, _, y) => Input.MouseWheelCallback(y));
		_glfw.SetFramebufferSizeCallback(Window, (_, w, h) => SetWindowSize(w, h));

		_glfw.MakeContextCurrent(Window);
		_gl = GL.GetApi(_glfw.GetProcAddress);
	}

	private static void SetWindowSize(int width, int height)
	{
		Gl.Viewport(0, 0, (uint)width, (uint)height);
		ImGuiController.WindowResized(width, height);
	}
}
