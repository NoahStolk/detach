using Demos.Collisions;
using Demos.Collisions.Services;
using StrongInject;

using Container container = new();
using Owned<App> app = container.Resolve();
app.Value.Run();
