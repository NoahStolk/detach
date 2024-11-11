using System.Globalization;
using System.Numerics;
using System.Text;

namespace Detach.Parsers.Model.MtlFormat;

/// <summary>
/// Note: this parser is not fully implemented yet.
/// </summary>
public static class MtlParser
{
	public static MaterialsData Parse(byte[] fileContents)
	{
		string text = Encoding.ASCII.GetString(fileContents);
		string[] lines = text.Split('\n');

		List<MaterialData> materials = [];

		MaterialBuildingContext context = new();
		foreach (string line in lines)
		{
			string[] values = line.Split(' ');

			switch (values[0])
			{
				case "newmtl":
					FinishMaterial();

					context.Name = values[1].Trim();
					context.IsInitialized = true;
					break;
				case "Ka": context.AmbientColor = new Vector3(ParseColorFloat(values[1]), ParseColorFloat(values[2]), ParseColorFloat(values[3])); break;
				case "Kd": context.DiffuseColor = new Vector3(ParseColorFloat(values[1]), ParseColorFloat(values[2]), ParseColorFloat(values[3])); break;
				case "Ks": context.SpecularColor = new Vector3(ParseColorFloat(values[1]), ParseColorFloat(values[2]), ParseColorFloat(values[3])); break;
				case "Ke": context.EmissiveCoefficient = new Vector3(ParseColorFloat(values[1]), ParseColorFloat(values[2]), ParseColorFloat(values[3])); break;
				case "Ns": context.SpecularExponent = ParseColorFloat(values[1]); break;
				case "Ni": context.OpticalDensity = ParseColorFloat(values[1]); break;
				case "d": context.Alpha = ParseColorFloat(values[1]); break;
				case "map_Kd": context.DiffuseMap = values[1].Trim(); break;
			}
		}

		FinishMaterial();

		return new MaterialsData(materials);

		void FinishMaterial()
		{
			if (!context.IsInitialized)
				return;

			materials.Add(BuildMaterial(context));
			context.Clear();
		}
	}

	private static float ParseColorFloat(string value)
	{
		return (float)double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
	}

	private static MaterialData BuildMaterial(MaterialBuildingContext context)
	{
		return new MaterialData(
			Name: context.Name,
			AmbientColor: context.AmbientColor,
			DiffuseColor: context.DiffuseColor,
			SpecularColor: context.SpecularColor,
			EmissiveCoefficient: context.EmissiveCoefficient,
			SpecularExponent: context.SpecularExponent,
			OpticalDensity: context.OpticalDensity,
			Alpha: context.Alpha,
			DiffuseMap: context.DiffuseMap);
	}

	private sealed record MaterialBuildingContext
	{
		public bool IsInitialized { get; set; }
		public string Name { get; set; } = string.Empty;
		public Vector3 AmbientColor { get; set; }
		public Vector3 DiffuseColor { get; set; }
		public Vector3 SpecularColor { get; set; }
		public Vector3 EmissiveCoefficient { get; set; }
		public float SpecularExponent { get; set; }
		public float OpticalDensity { get; set; }
		public float Alpha { get; set; }
		public string DiffuseMap { get; set; } = string.Empty;

		public void Clear()
		{
			IsInitialized = false;
			Name = string.Empty;
			AmbientColor = Vector3.Zero;
			DiffuseColor = Vector3.Zero;
			SpecularColor = Vector3.Zero;
			SpecularExponent = 0;
			Alpha = 0;
			DiffuseMap = string.Empty;
		}
	}
}
