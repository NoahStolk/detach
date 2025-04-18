﻿using Demos.ImGuiBackend.GlfwImGuiNET.Services.Ui;
using Demos.ImGuiBackend.GlfwImGuiNET.Utils;
using Detach.GlfwExtensions;
using Detach.ImGuiBackend.GlfwImGuiNET;
using Detach.Metrics;
using ImGuiNET;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace Demos.ImGuiBackend.GlfwImGuiNET.Services;

internal sealed class App
{
	private const float _maxMainDelta = 0.25f;

	private const double _updateRate = 60;
	private const double _mainLoopRate = 300;

	private const double _updateLength = 1 / _updateRate;
	private const double _mainLoopLength = 1 / _mainLoopRate;

	private readonly Glfw _glfw;
	private readonly GL _gl;
	private readonly unsafe WindowHandle* _window;
	private readonly GlfwInput _glfwInput;
	private readonly ImGuiController _imGuiController;
	private readonly FrameCounter _frameCounter;
	private readonly HeapAllocationCounter _heapAllocationCounter;
	private readonly InputTestingWindow _inputTestingWindow;
	private readonly KeyboardInputWindow _keyboardInputWindow;
	private readonly MouseInputWindow _mouseInputWindow;
	private readonly PerformanceWindow _performanceWindow;
	private readonly SettingsWindow _settingsWindow;

	private double _currentTime;
	private double _accumulator;
	private double _frameTime;

	public unsafe App(
		Glfw glfw,
		GL gl,
		WindowHandle* window,
		GlfwInput glfwInput,
		ImGuiController imGuiController,
		FrameCounter frameCounter,
		HeapAllocationCounter heapAllocationCounter,
		InputTestingWindow inputTestingWindow,
		KeyboardInputWindow keyboardInputWindow,
		MouseInputWindow mouseInputWindow,
		PerformanceWindow performanceWindow,
		SettingsWindow settingsWindow)
	{
		_glfw = glfw;
		_gl = gl;
		_window = window;
		_glfwInput = glfwInput;
		_imGuiController = imGuiController;
		_frameCounter = frameCounter;
		_heapAllocationCounter = heapAllocationCounter;
		_inputTestingWindow = inputTestingWindow;
		_keyboardInputWindow = keyboardInputWindow;
		_mouseInputWindow = mouseInputWindow;
		_performanceWindow = performanceWindow;
		_settingsWindow = settingsWindow;

		_currentTime = glfw.GetTime();

		gl.Viewport(0, 0, WindowConstants.WindowWidth, WindowConstants.WindowHeight);
		glfw.SwapInterval(0); // Turns VSync off.

		glfw.SetFramebufferSizeCallback(window, (_, w, h) =>
		{
			gl.Viewport(0, 0, (uint)w, (uint)h);
			imGuiController.WindowResized(w, h);
		});
	}

	public unsafe void Run()
	{
		while (!_glfw.WindowShouldClose(_window))
		{
			double expectedNextFrame = _glfw.GetTime() + _mainLoopLength;
			MainLoop();

			while (_glfw.GetTime() < expectedNextFrame)
				Thread.Yield();
		}

		_imGuiController.Destroy();
		_glfw.Terminate();
	}

	private unsafe void MainLoop()
	{
		double mainStartTime = _glfw.GetTime();
		_frameTime = mainStartTime - _currentTime;
		if (_frameTime > _maxMainDelta)
			_frameTime = _maxMainDelta;

		_frameCounter.Update(mainStartTime, _frameTime);

		_currentTime = mainStartTime;
		_accumulator += _frameTime;

		_glfw.PollEvents();

		while (_accumulator >= _updateLength)
			_accumulator -= _updateLength;

		Render();

		_glfw.SwapBuffers(_window);
	}

	private void Render()
	{
		_imGuiController.Update((float)_frameTime);

		ImGui.DockSpaceOverViewport(0, null, ImGuiDockNodeFlags.PassthruCentralNode);

		_gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		ImGui.ShowDemoWindow();

		_heapAllocationCounter.UpdateAllocatedBytesForCurrentThread();

		_inputTestingWindow.Render();
		_keyboardInputWindow.Render();
		_mouseInputWindow.Render();
		_performanceWindow.Render();
		_settingsWindow.Render();

		_imGuiController.Render();

		_glfwInput.EndFrame();
	}
}
