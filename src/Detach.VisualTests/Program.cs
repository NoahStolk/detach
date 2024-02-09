using Detach.VisualTests;
using ImGuiGlfw;

Graphics.CreateWindow();

ImGuiController imGuiController = new(Graphics.Gl, Input.GlfwInput, Constants.WindowWidth, Constants.WindowHeight);

Graphics.OnChangeWindowSize = (width, height) => imGuiController.WindowResized(width, height);

App.Run(imGuiController);
