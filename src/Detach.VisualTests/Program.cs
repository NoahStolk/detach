using Detach.VisualTests;
using System.Diagnostics;

Graphics.CreateWindow();

const string projectionMatrix = nameof(projectionMatrix);
const string image = nameof(image);
string exeDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) ?? throw new InvalidOperationException("Could not get current process directory.");
string shadersDirectory = Path.Combine(exeDirectory, "Content", "Shaders");

uint shaderId = ShaderLoader.Load(File.ReadAllText(Path.Combine(shadersDirectory, "Ui.vert")), File.ReadAllText(Path.Combine(shadersDirectory, "Ui.frag")));

int projectionMatrixUniform = Graphics.Gl.GetUniformLocation(shaderId, projectionMatrix);
if (projectionMatrixUniform == -1)
	throw new InvalidOperationException($"Could not find '{projectionMatrix}' uniform location.");

int textureUniform = Graphics.Gl.GetUniformLocation(shaderId, image);
if (textureUniform == -1)
	throw new InvalidOperationException($"Could not find '{image}' uniform location.");

ImGuiController.Initialize(shaderId, projectionMatrixUniform, textureUniform);

App.Run();
