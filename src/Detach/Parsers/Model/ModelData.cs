using System.Numerics;

namespace Detach.Parsers.Model;

/// <summary>
/// Represents data parsed from a model format, such as a .obj file.
/// </summary>
public record ModelData(IReadOnlyList<Vector3> Positions, IReadOnlyList<Vector2> Textures, IReadOnlyList<Vector3> Normals, IReadOnlyList<MeshData> Meshes);
