using Detach.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Tests.Extensions;

[TestClass]
public class VectorBinaryTests
{
	[DataTestMethod]
	[DataRow(-1, 2)]
	[DataRow(0, 0)]
	[DataRow(1, 2)]
	[DataRow(-49.5f, 5.2f)]
	[DataRow(float.MinValue, float.MinValue)]
	[DataRow(float.MaxValue, float.MaxValue)]
	public void TestBinaryConversion_Vector2(float x, float y)
	{
		Vector2 vector = new(x, y);

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(vector);

		ms.Position = 0;
		using BinaryReader br = new(ms);
		Assert.AreEqual(vector, br.ReadVector2());
	}

	[DataTestMethod]
	[DataRow(-1, 2, 3)]
	[DataRow(0, 0, 0)]
	[DataRow(1, 2, 3)]
	[DataRow(-49.5f, 5.2f, 60)]
	[DataRow(float.MinValue, float.MinValue, float.MinValue)]
	[DataRow(float.MaxValue, float.MaxValue, float.MaxValue)]
	public void TestBinaryConversion_Vector3(float x, float y, float z)
	{
		Vector3 vector = new(x, y, z);

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(vector);

		ms.Position = 0;
		using BinaryReader br = new(ms);
		Assert.AreEqual(vector, br.ReadVector3());
	}
}
