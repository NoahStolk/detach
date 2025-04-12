using Detach.Parsers.Model;
using Detach.Parsers.Model.MtlFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Parsers.Model.MtlFormat;

[TestClass]
public sealed class MtlParserTests
{
	[TestMethod]
	public void ParseMaterial()
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath("Material.mtl"));

		MaterialsData materialsData = MtlParser.Parse(bytes);
		Assert.AreEqual(2, materialsData.Materials.Count);

		MaterialData material1 = materialsData.Materials[0];
		Assert.AreEqual("Material.012", material1.Name);
		Assert.AreEqual(new Vector3(1, 1, 1), material1.AmbientColor);
		Assert.AreEqual(Vector3.Zero, material1.DiffuseColor); // TODO: Is there a default value according to the MTL spec?
		Assert.AreEqual(new Vector3(0.5f, 0.5f, 0.5f), material1.SpecularColor);
		Assert.AreEqual(Vector3.Zero, material1.EmissiveCoefficient);
		Assert.AreEqual(250, material1.SpecularExponent);
		Assert.AreEqual(1.45f, material1.OpticalDensity);
		Assert.AreEqual(1, material1.Alpha);
		Assert.AreEqual("../tex/test1.tga", material1.DiffuseMap);

		MaterialData material2 = materialsData.Materials[1];
		Assert.AreEqual("Material.013", material2.Name);
		Assert.AreEqual(new Vector3(1, 1, 1), material2.AmbientColor);
		Assert.AreEqual(Vector3.Zero, material2.DiffuseColor);
		Assert.AreEqual(new Vector3(0.5f, 0.5f, 0.5f), material2.SpecularColor);
		Assert.AreEqual(Vector3.Zero, material1.EmissiveCoefficient);
		Assert.AreEqual(250, material2.SpecularExponent);
		Assert.AreEqual(1.45f, material2.OpticalDensity);
		Assert.AreEqual(1, material2.Alpha);
		Assert.AreEqual("../tex/test2.tga", material2.DiffuseMap);
	}
}
