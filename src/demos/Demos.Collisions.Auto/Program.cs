using Demos.Collisions.Auto;
using Demos.Collisions.Auto.Services;
using StrongInject;

using Container container = new();
using Owned<App> app = container.Resolve();
app.Value.Run();
