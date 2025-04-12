using Detach.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Extensions;

[TestClass]
public sealed class BinaryExtensionsTests
{
	[DataTestMethod]
	[DataRow(-1, 2)]
	[DataRow(0, 0)]
	[DataRow(1, 2)]
	[DataRow(-49.5f, 5.2f)]
	[DataRow(float.MinValue, float.MinValue)]
	[DataRow(float.MaxValue, float.MaxValue)]
	public void Vector2(float x, float y)
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
	public void Vector3(float x, float y, float z)
	{
		Vector3 vector = new(x, y, z);

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(vector);

		ms.Position = 0;
		using BinaryReader br = new(ms);
		Assert.AreEqual(vector, br.ReadVector3());
	}

	[DataTestMethod]
	[DataRow(-1, 2, 3, 4)]
	[DataRow(0, 0, 0, 0)]
	[DataRow(1, 2, 3, 4)]
	[DataRow(-49.5f, 5.2f, 60, -1)]
	[DataRow(float.MinValue, float.MinValue, float.MinValue, float.MinValue)]
	[DataRow(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue)]
	public void Vector4(float x, float y, float z, float w)
	{
		Vector4 vector = new(x, y, z, w);

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(vector);

		ms.Position = 0;
		using BinaryReader br = new(ms);
		Assert.AreEqual(vector, br.ReadVector4());
	}

	[DataTestMethod]
	[DataRow(-1, 2, 3, 4)]
	[DataRow(0, 0, 0, 0)]
	[DataRow(1, 2, 3, 4)]
	[DataRow(-49.5f, 5.2f, 60, -1)]
	[DataRow(float.MinValue, float.MinValue, float.MinValue, float.MinValue)]
	[DataRow(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue)]
	public void Quaternion(float x, float y, float z, float w)
	{
		Quaternion quaternion = new(x, y, z, w);

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(quaternion);

		ms.Position = 0;
		using BinaryReader br = new(ms);
		Assert.AreEqual(quaternion, br.ReadQuaternion());
	}

	[DataTestMethod]
	[DataRow(-1, 2, 3, 4, 5, 6)]
	[DataRow(0, 0, 0, 0, 0, 0)]
	[DataRow(1, 2, 3, 4, 5, 6)]
	[DataRow(-49.5f, 5.2f, 60, -1, 0, 0)]
	[DataRow(float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue)]
	[DataRow(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue)]

	// ReSharper disable once InconsistentNaming
	public void Matrix3x2(float m11, float m12, float m21, float m22, float m31, float m32)
	{
		Matrix3x2 matrix = new(m11, m12, m21, m22, m31, m32);

		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write(matrix);

		ms.Position = 0;
		using BinaryReader br = new(ms);
		Assert.AreEqual(matrix, br.ReadMatrix3x2());
	}
}
