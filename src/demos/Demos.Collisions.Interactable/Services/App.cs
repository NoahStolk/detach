using Demos.Collisions.Interactable.Services.States;
using Demos.Collisions.Interactable.Services.Ui;
using Demos.Collisions.Interactable.Utils;
using Detach.GlfwExtensions;
using Detach.ImGuiBackend.GlfwHexa;
using Hexa.NET.ImGui;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace Demos.Collisions.Interactable.Services;

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
	private readonly AlgorithmParametersWindow _algorithmParametersWindow;
	private readonly AlgorithmSelectWindow _algorithmSelectWindow;
	private readonly ScenarioDataWindow _scenarioDataWindow;
	private readonly SceneWindow _sceneWindow;
	private readonly CollisionAlgorithmState _collisionAlgorithmState;

	private double _currentTime;
	private double _accumulator;
	private double _frameTime;

	public unsafe App(
		Glfw glfw,
		GL gl,
		WindowHandle* window,
		GlfwInput glfwInput,
		ImGuiController imGuiController,
		AlgorithmParametersWindow algorithmParametersWindow,
		AlgorithmSelectWindow algorithmSelectWindow,
		ScenarioDataWindow scenarioDataWindow,
		SceneWindow sceneWindow,
		CollisionAlgorithmState collisionAlgorithmState)
	{
		_glfw = glfw;
		_gl = gl;
		_window = window;
		_glfwInput = glfwInput;
		_imGuiController = imGuiController;
		_algorithmParametersWindow = algorithmParametersWindow;
		_algorithmSelectWindow = algorithmSelectWindow;
		_scenarioDataWindow = scenarioDataWindow;
		_sceneWindow = sceneWindow;
		_collisionAlgorithmState = collisionAlgorithmState;

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
		float frameTime = (float)_frameTime;
		_imGuiController.Update(frameTime);

		ImGui.DockSpaceOverViewport(0, null, ImGuiDockNodeFlags.PassthruCentralNode);

		_gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

		_collisionAlgorithmState.ExecuteAlgorithm();

		_algorithmParametersWindow.Render();
		_algorithmSelectWindow.Render();
		_scenarioDataWindow.Render();
		_sceneWindow.Render(frameTime);

		_imGuiController.Render();

		_glfwInput.EndFrame();
	}
}
