# Detach

A set of libraries for building lightweight, cross-platform, real-time applications and games in .NET 8.

Built on top of:
- OpenGL
- GLFW
- ImGui

Uses bindings from:
- Silk.NET
- Hexa.NET.ImGui
- ImGui.NET

## Libraries

| Library                           | Features                                                                                   | Dependencies                                           | NuGet                                                                                                                     |
|-----------------------------------|--------------------------------------------------------------------------------------------|--------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| Detach                            | Parsers for various formats, maths, collision algorithms, numerics, extension methods, etc | -                                                      | [![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Detach.svg)](https://www.nuget.org/packages/NoahStolk.Detach/) |
| Detach.GlfwExtensions             | Extensions for GLFW                                                                        | Silk.NET.GLFW                                          |                                                                                                                           |
| Detach.Glfw.ImGuiBackend.ImGuiNET | ImGui backend for GLFW using ImGui.NET bindings                                            | Silk.NET.OpenGL, Detach.GlfwExtensions, ImGui.NET      |                                                                                                                           |
| Detach.Glfw.ImGuiBackend.Hexa     | ImGui backend for GLFW using Hexa.NET.ImGui bindings                                       | Silk.NET.OpenGL, Detach.GlfwExtensions, Hexa.NET.ImGui |                                                                                                                           |
|                                   |                                                                                            |                                                        |                                                                                                                           |
|                                   |                                                                                            |                                                        |                                                                                                                           |
|                                   |                                                                                            |                                                        |                                                                                                                           |

## Promises

- Semantic versioning
- Synchronized versioning across all libraries (meaning every library is re-released, even if only one contained changes)
- Synchronized transitive dependency versions 
