using Detach.Demos.Glfw.ImGuiBackend.ImGuiNET;
using Detach.Demos.Glfw.ImGuiBackend.ImGuiNET.Services;
using StrongInject;

using Container container = new();
using Owned<App> app = container.Resolve();
app.Value.Run();
