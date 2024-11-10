using Silk.NET.GLFW;
using System.Text;
using Monitor = Silk.NET.GLFW.Monitor;

namespace Detach.Demos.Glfw.ImGuiBackend.ImGuiNET.Extensions;

public static class GlfwExtensions
{
	public static unsafe void CheckError(this Silk.NET.GLFW.Glfw glfw)
	{
		ErrorCode errorCode = glfw.GetError(out byte* c);
		if (errorCode == ErrorCode.NoError || c == (byte*)0)
			return;

		StringBuilder errorBuilder = new();
		while (*c != 0x00)
			errorBuilder.Append((char)*c++);

		throw new InvalidOperationException($"GLFW {errorCode}: {errorBuilder}");
	}

	public static unsafe (int X, int Y) GetInitialWindowPos(this Silk.NET.GLFW.Glfw glfw, int windowWidth, int windowHeight)
	{
		Monitor* primaryMonitor = glfw.GetPrimaryMonitor();
		if (primaryMonitor == null)
			return (0, 0);

		glfw.GetMonitorWorkarea(primaryMonitor, out _, out _, out int primaryMonitorWidth, out int primaryMonitorHeight);
		return ((primaryMonitorWidth - windowWidth) / 2, (primaryMonitorHeight - windowHeight) / 2);
	}
}
