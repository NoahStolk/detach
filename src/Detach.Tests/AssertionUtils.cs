using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests;

public static class AssertionUtils
{
	public static void AreEqual(Vector3 expected, Vector3 actual, float delta = 0.0001f)
	{
		Assert.AreEqual(expected.X, actual.X, delta);
		Assert.AreEqual(expected.Y, actual.Y, delta);
		Assert.AreEqual(expected.Z, actual.Z, delta);
	}
}
