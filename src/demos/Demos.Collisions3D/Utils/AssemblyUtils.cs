using System.Diagnostics;

namespace Demos.Collisions3D.Utils;

internal static class AssemblyUtils
{
	public static string GetExecutableDirectory()
	{
		string? executableDirectory = Path.GetDirectoryName(AppContext.BaseDirectory);
		Debug.Assert(executableDirectory != null);
		return executableDirectory;
	}
}
