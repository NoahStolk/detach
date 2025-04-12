using Detach.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Extensions;

[TestClass]
public sealed class QuaternionExtensionsTests
{
	[DataTestMethod]
	[DataRow(0f, 0f, 0f, 0f, true)]
	[DataRow(float.NaN, 0f, 0f, 0f, false)]
	[DataRow(0f, float.PositiveInfinity, 0f, 0f, false)]
	[DataRow(0f, 0f, float.NegativeInfinity, 0f, false)]
	[DataRow(1.5f, -2.3f, 3.7f, 4.1f, true)]
	public void IsFinite(float x, float y, float z, float w, bool expected)
	{
		Quaternion quaternion = new(x, y, z, w);
		Assert.AreEqual(expected, quaternion.IsFinite());
	}
}
