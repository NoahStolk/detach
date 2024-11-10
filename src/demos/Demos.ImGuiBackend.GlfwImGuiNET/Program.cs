using Demos.ImGuiBackend.GlfwImGuiNET;
using Demos.ImGuiBackend.GlfwImGuiNET.Services;
using StrongInject;

using Container container = new();
using Owned<App> app = container.Resolve();
app.Value.Run();
