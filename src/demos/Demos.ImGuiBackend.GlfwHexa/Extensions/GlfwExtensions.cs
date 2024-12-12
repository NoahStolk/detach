using Silk.NET.GLFW;
using Monitor = Silk.NET.GLFW.Monitor;

namespace Demos.ImGuiBackend.GlfwHexa.Extensions;

internal static class GlfwExtensions
{
	public static unsafe (int X, int Y) GetInitialWindowPos(this Glfw glfw, int windowWidth, int windowHeight)
	{
		Monitor* primaryMonitor = glfw.GetPrimaryMonitor();
		if (primaryMonitor == null)
			return (0, 0);

		glfw.GetMonitorWorkarea(primaryMonitor, out _, out _, out int primaryMonitorWidth, out int primaryMonitorHeight);
		return ((primaryMonitorWidth - windowWidth) / 2, (primaryMonitorHeight - windowHeight) / 2);
	}
}
