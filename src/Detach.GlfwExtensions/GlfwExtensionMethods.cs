using Silk.NET.GLFW;
using System.Runtime.CompilerServices;
using System.Text;

namespace Detach.GlfwExtensions;

public static class GlfwExtensionMethods
{
	public static unsafe void CheckError(this Glfw glfw, [CallerMemberName] string caller = "", [CallerLineNumber] int line = 0)
	{
		ErrorCode errorCode = glfw.GetError(out byte* c);
		if (errorCode == ErrorCode.NoError)
			return;

		if (c == (byte*)0)
			throw new InvalidOperationException($"GLFW error '{errorCode}'. First noted in '{caller}' on line {line}. No error message was given.");

		StringBuilder errorBuilder = new();
		while (*c != 0x00)
			errorBuilder.Append((char)*c++);

		throw new InvalidOperationException($"GLFW error '{errorCode}'. First noted in '{caller}' on line {line}. Error message: {errorBuilder}");
	}
}
