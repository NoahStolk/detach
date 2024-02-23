using System.Globalization;
using System.Numerics;
using System.Text;

namespace Detach.Parsers.Model.ObjFormat;

public static class ObjParser
{
	public static ModelData Parse(byte[] fileContents)
	{
		string text = Encoding.UTF8.GetString(fileContents);
		string[] lines = text.Split('\n');

		ModelBuildingContext context = new();

		string currentObject = string.Empty;
		string currentGroup = string.Empty;
		string currentMaterial = string.Empty;
		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			string[] values = line.Split(' ');

			switch (values[0])
			{
				case "v": context.Positions.Add(new(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]), ParseVertexFloat(values[3]))); break;
				case "vt": context.Textures.Add(new(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]))); break;
				case "vn": context.Normals.Add(new(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]), ParseVertexFloat(values[3]))); break;
				case "o": currentObject = values[1].Trim(); break;
				case "g": currentGroup = values[1].Trim(); break;
				case "usemtl": currentMaterial = values[1].Trim(); break;
				case "f":
					if (values.Length < 4) // Invalid face.
						break;

					string[] rawIndices = values[1..];
					List<Face> faces = [];
					for (int j = 0; j < rawIndices.Length; j++)
					{
						string[] indexEntries = rawIndices[j].Split('/');
						faces.Add(new(ushort.Parse(indexEntries[0]), ushort.TryParse(indexEntries[1], out ushort texture) ? texture : (ushort)0, ushort.Parse(indexEntries[2])));

						if (j >= 3)
						{
							faces.Add(faces[0]);
							faces.Add(faces[j - 1]);
						}
					}

					foreach (Face face in faces)
					{
						MeshBuildingContext? mesh = context.Meshes.Find(m => m.ObjectName == currentObject);
						if (mesh == null)
						{
							mesh = new()
							{
								GroupName = currentGroup,
								MaterialName = currentMaterial,
								ObjectName = currentObject,
							};
							context.Meshes.Add(mesh);
						}

						mesh.Faces.Add(face);
					}

					break;
			}
		}

		List<MeshData> meshes = context.Meshes.ConvertAll(mbc => new MeshData(mbc.ObjectName, mbc.GroupName, mbc.MaterialName, mbc.Faces));
		return new(context.Positions, context.Textures, context.Normals, meshes);
	}

	private static float ParseVertexFloat(string value)
	{
		return (float)double.Parse(value, NumberStyles.Float);
	}

	private sealed class ModelBuildingContext
	{
		public List<Vector3> Positions { get; } = [];
		public List<Vector2> Textures { get; } = [];
		public List<Vector3> Normals { get; } = [];
		public List<MeshBuildingContext> Meshes { get; } = [];
	}

	private sealed class MeshBuildingContext
	{
		public required string ObjectName { get; init; }
		public required string GroupName { get; init; }
		public required string MaterialName { get; init; }
		public List<Face> Faces { get; } = [];
	}
}
