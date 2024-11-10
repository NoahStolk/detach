﻿using Silk.NET.GLFW;
using System.Text;

namespace Detach.GlfwExtensions;

public static class GlfwExtensions
{
	public static unsafe void CheckError(this Glfw glfw)
	{
		ErrorCode errorCode = glfw.GetError(out byte* c);
		if (errorCode == ErrorCode.NoError || c == (byte*)0)
			return;

		StringBuilder errorBuilder = new();
		while (*c != 0x00)
			errorBuilder.Append((char)*c++);

		throw new InvalidOperationException($"GLFW {errorCode}: {errorBuilder}");
	}
}
