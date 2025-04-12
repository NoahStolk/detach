using Detach.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Extensions;

[TestClass]
public sealed class VectorExtensionsTests
{
	[DataTestMethod]
	[DataRow(0f, 0f, true)]
	[DataRow(float.NaN, 0f, false)]
	[DataRow(0f, float.PositiveInfinity, false)]
	[DataRow(float.NegativeInfinity, 0f, false)]
	[DataRow(1.5f, -2.3f, true)]
	public void Vector2IsFinite(float x, float y, bool expected)
	{
		Vector2 vector = new(x, y);
		Assert.AreEqual(expected, vector.IsFinite());
	}

	[DataTestMethod]
	[DataRow(0f, 0f, 0f, true)]
	[DataRow(float.NaN, 0f, 0f, false)]
	[DataRow(0f, float.PositiveInfinity, 0f, false)]
	[DataRow(0f, 0f, float.NegativeInfinity, false)]
	[DataRow(1.5f, -2.3f, 3.7f, true)]
	public void Vector3IsFinite(float x, float y, float z, bool expected)
	{
		Vector3 vector = new(x, y, z);
		Assert.AreEqual(expected, vector.IsFinite());
	}

	[DataTestMethod]
	[DataRow(0f, 0f, 0f, 0f, true)]
	[DataRow(float.NaN, 0f, 0f, 0f, false)]
	[DataRow(0f, float.PositiveInfinity, 0f, 0f, false)]
	[DataRow(0f, 0f, float.NegativeInfinity, 0f, false)]
	[DataRow(1.5f, -2.3f, 3.7f, 4.1f, true)]
	public void Vector4IsFinite(float x, float y, float z, float w, bool expected)
	{
		Vector4 vector = new(x, y, z, w);
		Assert.AreEqual(expected, vector.IsFinite());
	}
}
