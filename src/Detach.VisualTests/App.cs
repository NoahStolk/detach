using Detach.VisualTests.Ui;
using Detach.VisualTests.Ui.Windows;
using ImGuiGlfw;
using Silk.NET.OpenGL;

namespace Detach.VisualTests;

public static class App
{
	private const float _maxMainDelta = 0.25f;

	private const double _updateRate = 60;
	private const double _mainLoopRate = 300;

	private const double _updateLength = 1 / _updateRate;
	private const double _mainLoopLength = 1 / _mainLoopRate;

	private static double _currentTime = Graphics.Glfw.GetTime();
	private static double _accumulator;
	private static double _frameTime;

	private static ImGuiController? _imGuiController;

	public static ImGuiController ImGuiController => _imGuiController ?? throw new InvalidOperationException("ImGuiController is not initialized.");

	public static unsafe void Run(ImGuiController imGuiController)
	{
		_imGuiController = imGuiController;

		while (!Graphics.Glfw.WindowShouldClose(Graphics.Window))
		{
			double expectedNextFrame = Graphics.Glfw.GetTime() + _mainLoopLength;
			MainLoop();

			while (Graphics.Glfw.GetTime() < expectedNextFrame)
				Thread.Yield();
		}

		ImGuiController.Destroy();
		Graphics.Glfw.Terminate();
	}

	private static unsafe void MainLoop()
	{
		double mainStartTime = Graphics.Glfw.GetTime();

		_frameTime = mainStartTime - _currentTime;
		if (_frameTime > _maxMainDelta)
			_frameTime = _maxMainDelta;

		_currentTime = mainStartTime;
		_accumulator += _frameTime;

		Graphics.Glfw.PollEvents();

		while (_accumulator >= _updateLength)
			_accumulator -= _updateLength;

		Render();

		Graphics.Glfw.SwapBuffers(Graphics.Window);
	}

	private static void Render()
	{
		ImGuiController.Update((float)_frameTime);

		Graphics.Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		Shapes2DWindow.Render();

		ImGuiController.Render();

		Input.GlfwInput.PostRender();
	}
}
