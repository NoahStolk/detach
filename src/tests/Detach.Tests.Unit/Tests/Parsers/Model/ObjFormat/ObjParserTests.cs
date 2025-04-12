using Detach.Parsers.Model;
using Detach.Parsers.Model.ObjFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Parsers.Model.ObjFormat;

[TestClass]
public sealed class ObjParserTests
{
	[DataTestMethod]
	[DataRow("Cube.obj")]
	[DataRow("CubeDuplicateSpaces.obj")]
	public void ParseCubeModel(string fileName)
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath(fileName));

		ModelData modelData = ObjParser.Parse(bytes);
		Assert.AreEqual(1, modelData.MaterialLibraries.Count);
		Assert.AreEqual("Cube.mtl", modelData.MaterialLibraries[0]);
		Assert.AreEqual(8, modelData.Positions.Count);
		Assert.AreEqual(21, modelData.Textures.Count);
		Assert.AreEqual(8, modelData.Normals.Count);
		Assert.AreEqual(1, modelData.Meshes.Count);

		Assert.AreEqual(new Vector3(-0.5f, -0.5f, -0.5f), modelData.Positions[0]);
		Assert.AreEqual(new Vector2(0, 1 / 3f), modelData.Textures[0]);
		Assert.AreEqual(new Vector3(-0.57735027f, -0.57735027f, -0.57735027f), modelData.Normals[0]);

		MeshData meshData = modelData.Meshes[0];
		Assert.AreEqual("Cube1", meshData.ObjectName);
		Assert.AreEqual("Cube1", meshData.GroupName);
		Assert.AreEqual("Cube1_auv", meshData.MaterialName);
		Assert.AreEqual(36, meshData.Faces.Count);

		Assert.AreEqual(1, meshData.Faces[0].Position);
		Assert.AreEqual(17, meshData.Faces[0].Texture);
		Assert.AreEqual(1, meshData.Faces[0].Normal);

		Assert.AreEqual(5, meshData.Faces[1].Position);
		Assert.AreEqual(13, meshData.Faces[1].Texture);
		Assert.AreEqual(5, meshData.Faces[1].Normal);
	}

	[TestMethod]
	public void ParseMultipleMeshesModel()
	{
		byte[] bytes = File.ReadAllBytes(ResourceUtils.GetResourcePath("MultipleMeshes.obj"));

		ModelData modelData = ObjParser.Parse(bytes);
		Assert.AreEqual(1, modelData.MaterialLibraries.Count);
		Assert.AreEqual("Test.mtl", modelData.MaterialLibraries[0]);
		Assert.AreEqual(34, modelData.Positions.Count);
		Assert.AreEqual(86, modelData.Textures.Count);
		Assert.AreEqual(34, modelData.Normals.Count);
		Assert.AreEqual(4, modelData.Meshes.Count);

		Assert.AreEqual(new Vector3(3.8270212e-17f, 3.75000000f, 0.0000000e+0f), modelData.Positions[0]);
		Assert.AreEqual(new Vector2(0, 0.43176447f), modelData.Textures[0]);
		Assert.AreEqual(new Vector3(-0.23942607f, 0.96762892f, -7.9808688e-2f), modelData.Normals[0]);

		MeshData torus = modelData.Meshes[0];
		Assert.AreEqual("Torus", torus.ObjectName);
		Assert.AreEqual("Torus", torus.GroupName);
		Assert.AreEqual("Torus4_auv", torus.MaterialName);
		Assert.AreEqual(108, torus.Faces.Count);

		MeshData armSecondary = modelData.Meshes[1];
		Assert.AreEqual("ArmSecondary", armSecondary.ObjectName);
		Assert.AreEqual("ArmSecondary", armSecondary.GroupName);
		Assert.AreEqual("cone2_copy3_auv", armSecondary.MaterialName);
		Assert.AreEqual(12, armSecondary.Faces.Count);

		MeshData armPrimary = modelData.Meshes[2];
		Assert.AreEqual("ArmPrimary", armPrimary.ObjectName);
		Assert.AreEqual("ArmPrimary", armPrimary.GroupName);
		Assert.AreEqual("cone2_auv", armPrimary.MaterialName);
		Assert.AreEqual(12, armPrimary.Faces.Count);

		MeshData @base = modelData.Meshes[3];
		Assert.AreEqual("Base", @base.ObjectName);
		Assert.AreEqual("Base", @base.GroupName);
		Assert.AreEqual("sphere1_auv", @base.MaterialName);
		Assert.AreEqual(36, @base.Faces.Count);
	}
}
