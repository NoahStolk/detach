using Demos.Collisions2D;
using Demos.Collisions2D.Services;
using StrongInject;

using Container container = new();
using Owned<App> app = container.Resolve();
app.Value.Run();
