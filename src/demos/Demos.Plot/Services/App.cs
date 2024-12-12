using Demos.Plot.Services.Ui;
using Demos.Plot.Utils;
using Detach.GlfwExtensions;
using Detach.ImGuiBackend.GlfwHexa;
using Detach.Metrics;
using Hexa.NET.ImGui;
using Hexa.NET.ImPlot;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace Demos.Plot.Services;

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
	private readonly RenderingMetricsWindow _renderingMetricsWindow;
	private readonly HeapAllocationMetricsWindow _heapAllocationMetricsWindow;
	private readonly PlotWindow _plotWindow;

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
		RenderingMetricsWindow renderingMetricsWindow,
		HeapAllocationMetricsWindow heapAllocationMetricsWindow,
		PlotWindow plotWindow)
	{
		_glfw = glfw;
		_gl = gl;
		_window = window;
		_glfwInput = glfwInput;
		_imGuiController = imGuiController;
		_frameCounter = frameCounter;
		_heapAllocationCounter = heapAllocationCounter;
		_renderingMetricsWindow = renderingMetricsWindow;
		_heapAllocationMetricsWindow = heapAllocationMetricsWindow;
		_plotWindow = plotWindow;

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
		_heapAllocationCounter.UpdateAllocatedBytesForCurrentThread();

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
		ImPlot.ShowDemoWindow();

		_renderingMetricsWindow.Render();
		_heapAllocationMetricsWindow.Render();
		_plotWindow.Render();

		_imGuiController.Render();

		_glfwInput.EndFrame();
	}
}
