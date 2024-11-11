namespace Detach.Parsers.Model;

public record MeshData(string ObjectName, string GroupName, string MaterialName, IReadOnlyList<Face> Faces);
