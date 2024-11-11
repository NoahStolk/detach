# Detach

A set of libraries for developing lightweight, cross-platform, real-time applications and games in .NET 8.

Built on top of:
- [OpenGL](https://www.opengl.org/)
- [GLFW](https://www.glfw.org/)
- [Dear ImGui](https://github.com/ocornut/imgui)

Uses bindings from:
- [Silk.NET](https://github.com/dotnet/Silk.NET)
- [Hexa.NET.ImGui](https://github.com/HexaEngine/Hexa.NET.ImGui)
- [ImGui.NET](https://github.com/ImGuiNET/ImGui.NET)

## Libraries

All libraries are currently targeting .NET 8.0.

| Library                          | Features                                                                                   | Dependencies                                                       | NuGet                                                                                                                                                                         |
|----------------------------------|--------------------------------------------------------------------------------------------|--------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Detach                           | Parsers for various formats, maths, collision algorithms, numerics, extension methods, etc | None                                                               | [![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Detach.svg)](https://www.nuget.org/packages/NoahStolk.Detach/)                                                     |
| Detach.AlExtensions              | Extensions for OpenAL                                                                      | **Silk.NET.OpenAL**                                                | [![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Detach.AlExtensions.svg)](https://www.nuget.org/packages/NoahStolk.Detach.AlExtensions/)                           |
| Detach.GlExtensions              | Extensions for OpenGL                                                                      | **Silk.NET.OpenGL**                                                | [![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Detach.GlExtensions.svg)](https://www.nuget.org/packages/NoahStolk.Detach.GlExtensions/)                           |
| Detach.GlfwExtensions            | Extensions for GLFW                                                                        | **Silk.NET.GLFW**                                                  | [![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Detach.GlfwExtensions.svg)](https://www.nuget.org/packages/NoahStolk.Detach.GlfwExtensions/)                       |
| Detach.ImGuiBackend.GlfwHexa     | ImGui backend for GLFW using Hexa.NET.ImGui bindings                                       | *Detach.GlExtensions*, *Detach.GlfwExtensions*, **Hexa.NET.ImGui** | [![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Detach.ImGuiBackend.GlfwHexa.svg)](https://www.nuget.org/packages/NoahStolk.Detach.ImGuiBackend.GlfwHexa/)         |
| Detach.ImGuiBackend.GlfwImGuiNET | ImGui backend for GLFW using ImGui.NET bindings                                            | *Detach.GlExtensions*, *Detach.GlfwExtensions*, **ImGui.NET**      | [![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Detach.ImGuiBackend.GlfwImGuiNET.svg)](https://www.nuget.org/packages/NoahStolk.Detach.ImGuiBackend.GlfwImGuiNET/) |

## Promises

- Semantic versioning
- Synchronized transitive dependency versions 
- Minimal dependencies, no dependencies for the main library
- Latest .NET version
