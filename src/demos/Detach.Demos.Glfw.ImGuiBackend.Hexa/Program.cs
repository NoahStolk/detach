using Detach.Demos.Glfw.ImGuiBackend.Hexa;
using Detach.Demos.Glfw.ImGuiBackend.Hexa.Services;
using StrongInject;

using Container container = new();
using Owned<App> app = container.Resolve();
app.Value.Run();
