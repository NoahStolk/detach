using Demos.ImGuiBackend.GlfwHexa;
using Demos.ImGuiBackend.GlfwHexa.Services;
using StrongInject;

using Container container = new();
using Owned<App> app = container.Resolve();
app.Value.Run();
