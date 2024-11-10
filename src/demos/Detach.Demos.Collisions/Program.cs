using Detach.Demos.Collisions;
using Detach.Demos.Collisions.Services;
using StrongInject;

using Container container = new();
using Owned<App> app = container.Resolve();
app.Value.Run();
