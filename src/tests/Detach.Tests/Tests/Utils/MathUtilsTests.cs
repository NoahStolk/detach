using Detach.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests.Utils;

[TestClass]
public sealed class MathUtilsTests
{
	[DataTestMethod]
	[DataRow(0, 0)]
	[DataRow(0, 1)]
	[DataRow(0, -1)]
	[DataRow(0.5f, 0.5f)]
	[DataRow(-0.5f, -0.5f)]
	[DataRow(0.25f, 0.25f)]
	[DataRow(-0.25f, -0.25f)]
	[DataRow(0.75f, 0.75f)]
	[DataRow(-0.75f, -0.75f)]
	[DataRow(0.125f, 10.125f)]
	[DataRow(-0.125f, -10.125f)]
	public void Fraction(float expected, float value)
	{
		float result = MathUtils.Fraction(value);
		Assert.AreEqual(expected, result, 0.000001f);
	}
}
