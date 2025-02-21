using Demos.Collisions.Interactable;
using Demos.Collisions.Interactable.Services;
using StrongInject;

using Container container = new();
using Owned<App> app = container.Resolve();
app.Value.Run();
