using System.Globalization;
using System.Numerics;
using System.Text;

namespace Detach.Parsers.Model.ObjFormat;

public static class ObjParser
{
	public static ModelData Parse(byte[] fileContents)
	{
		string text = Encoding.UTF8.GetString(fileContents);
		string[] lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

		ModelBuildingContext context = new();

		string currentObjectName = string.Empty;
		string currentGroupName = string.Empty;
		string currentMaterialName = string.Empty;
		List<string> materialLibraries = [];
		List<Face> faces = [];
		foreach (string line in lines)
		{
			string[] values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

			switch (values[0])
			{
				case "mtllib": materialLibraries.Add(values[1].Trim()); break;
				case "v": context.Positions.Add(new Vector3(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]), ParseVertexFloat(values[3]))); break;
				case "vt": context.Textures.Add(new Vector2(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]))); break;
				case "vn": context.Normals.Add(new Vector3(ParseVertexFloat(values[1]), ParseVertexFloat(values[2]), ParseVertexFloat(values[3]))); break;
				case "o": currentObjectName = values[1].Trim(); break;
				case "g": currentGroupName = values[1].Trim(); break;
				case "usemtl": currentMaterialName = values[1].Trim(); break;
				case "f":
					if (values.Length < 4) // Invalid face.
						break;

					string[] rawIndices = values[1..];
					faces.Clear();
					for (int j = 0; j < rawIndices.Length; j++)
					{
						string[] indexEntries = rawIndices[j].Split('/');
						faces.Add(new Face(ushort.Parse(indexEntries[0], CultureInfo.InvariantCulture), ushort.TryParse(indexEntries[1], out ushort texture) ? texture : (ushort)0, ushort.Parse(indexEntries[2], CultureInfo.InvariantCulture)));

						if (j >= 3)
						{
							faces.Add(faces[0]);
							faces.Add(faces[j - 1]);
						}
					}

					foreach (Face face in faces)
					{
						MeshBuildingContext? mesh = context.Meshes.Find(m => m.ObjectName == currentObjectName);
						if (mesh == null)
						{
							mesh = new MeshBuildingContext
							{
								GroupName = currentGroupName,
								MaterialName = currentMaterialName,
								ObjectName = currentObjectName,
							};
							context.Meshes.Add(mesh);
						}

						mesh.Faces.Add(face);
					}

					break;
			}
		}

		List<MeshData> meshes = context.Meshes.ConvertAll(mbc => new MeshData(mbc.ObjectName, mbc.GroupName, mbc.MaterialName, mbc.Faces));
		return new ModelData(materialLibraries, context.Positions, context.Textures, context.Normals, meshes);
	}

	private static float ParseVertexFloat(string value)
	{
		return (float)double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
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
