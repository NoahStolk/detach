using Detach.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Utils;

[TestClass]
public class VectorUtilsTests
{
	[DataTestMethod]
	[DataRow(0, "1,0", "1,0")]
	[DataRow(0, "1,0", "0,0")]
	[DataRow(MathF.PI, "1,0", "-1,0")]
	[DataRow(MathF.PI / 2, "1,0", "0,1")]
	[DataRow(MathF.PI / 4, "1,0", "1,1")]
	[DataRow(MathF.PI * 3 / 4, "1,0", "-1,1")]
	[DataRow(-MathF.PI * 3 / 4, "1,0", "-1,-1")]
	[DataRow(-MathF.PI * 1 / 4, "1,0", "1,-1")]
	public void TestVector2GetAngle(double expectedAngle, string a, string b)
	{
		Vector2 vecA = StringUtils.ParseVector2(a);
		Vector2 vecB = StringUtils.ParseVector2(b);
		double angle = VectorUtils.GetAngleBetween(vecA, vecB);
		Assert.AreEqual(expectedAngle, angle, 0.0001);
	}

	[DataTestMethod]
	[DataRow(0, "1,0,0", "1,0,0")]
	[DataRow(0, "1,0,0", "0,0,0")]
	[DataRow(MathF.PI, "1,0,0", "-1,0,0")]
	[DataRow(MathF.PI / 2, "1,0,0", "0,1,0")]
	[DataRow(MathF.PI / 4, "1,0,0", "1,1,0")]
	[DataRow(MathF.PI * 3 / 4, "1,0,0", "-1,1,0")]
	[DataRow(MathF.PI * 3 / 4, "1,0,0", "-1,-1,0")]
	[DataRow(MathF.PI * 1 / 4, "1,0,0", "1,-1,0")]
	[DataRow(MathF.PI / 2, "1,0,0", "0,0,1")]
	[DataRow(MathF.PI / 4, "1,0,0", "1,0,1")]
	[DataRow(MathF.PI * 3 / 4, "1,0,0", "-1,0,1")]
	[DataRow(MathF.PI * 3 / 4, "1,0,0", "-1,0,-1")]
	[DataRow(MathF.PI * 1 / 4, "1,0,0", "1,0,-1")]
	[DataRow(MathF.PI / 2, "1,0,0", "0,1,1")]
	[DataRow(0.955316603183746, "1,0,0", "1,1,1")]
	[DataRow(2.186275959014893, "1,0,0", "-1,1,1")]
	[DataRow(2.186275959014893, "1,0,0", "-1,1,-1")]
	[DataRow(0.955316603183746, "1,0,0", "1,1,-1")]
	[DataRow(MathF.PI / 2, "1,0,0", "0,-1,1")]
	[DataRow(0.955316603183746, "1,0,0", "1,-1,1")]
	[DataRow(2.186275959014893, "1,0,0", "-1,-1,1")]
	[DataRow(2.186275959014893, "1,0,0", "-1,-1,-1")]
	[DataRow(0.955316603183746, "1,0,0", "1,-1,-1")]
	public void TestVector3GetAngle(double expectedAngle, string a, string b)
	{
		Vector3 vecA = StringUtils.ParseVector3(a);
		Vector3 vecB = StringUtils.ParseVector3(b);
		double angle = VectorUtils.GetAngleBetween(vecA, vecB);
		Assert.AreEqual(expectedAngle, angle, 0.0001);
	}

	// TODO: Finish tests.
	[DataTestMethod]
	[DataRow("1,0.5", "1,0", 0.5f, 1.5f)]
	[DataRow("0.5,0.25", "1,0", 0.25f, 0.5f)]
	[DataRow("1,0.5", "1,0", 0.5f, 1.0f)]
	[DataRow("0.5,0.5", "1,0", 0.5f, 0.5f)]
	public void TestVector2Clamp(string expected, string input, float min, float max)
	{
		Vector2 vecExpected = StringUtils.ParseVector2(expected);
		Vector2 vecInput = StringUtils.ParseVector2(input);
		Vector2 vecResult = VectorUtils.Clamp(vecInput, min, max);
		Assert.AreEqual(vecExpected, vecResult);
	}
}
