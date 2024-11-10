# Detach

A set of libraries for developing lightweight, cross-platform, real-time applications and games in .NET 8.

Built on top of:
- OpenGL
- GLFW
- ImGui

Uses bindings from:
- Silk.NET
- Hexa.NET.ImGui
- ImGui.NET

## Libraries

| Library                           | Features                                                                                   | Dependencies                                               | NuGet                                                                                                                     |
|-----------------------------------|--------------------------------------------------------------------------------------------|------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| Detach                            | Parsers for various formats, maths, collision algorithms, numerics, extension methods, etc | -                                                          | [![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.Detach.svg)](https://www.nuget.org/packages/NoahStolk.Detach/) |
| Detach.GlExtensions               | Extensions for OpenGL                                                                      | Silk.NET.OpenGL                                            |                                                                                                                           |
| Detach.GlfwExtensions             | Extensions for GLFW                                                                        | Silk.NET.GLFW                                              |                                                                                                                           |
| Detach.Glfw.ImGuiBackend.Hexa     | ImGui backend for GLFW using Hexa.NET.ImGui bindings                                       | Detach.GlExtensions, Detach.GlfwExtensions, Hexa.NET.ImGui |                                                                                                                           |
| Detach.Glfw.ImGuiBackend.ImGuiNET | ImGui backend for GLFW using ImGui.NET bindings                                            | Detach.GlExtensions, Detach.GlfwExtensions, ImGui.NET      |                                                                                                                           |

## Promises

- Semantic versioning
- Synchronized transitive dependency versions 
- Minimal dependencies
- Latest .NET version
