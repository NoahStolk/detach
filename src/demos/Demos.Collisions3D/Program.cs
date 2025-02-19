using Demos.Collisions3D;
using Demos.Collisions3D.Services;
using StrongInject;

using Container container = new();
using Owned<App> app = container.Resolve();
app.Value.Run();
