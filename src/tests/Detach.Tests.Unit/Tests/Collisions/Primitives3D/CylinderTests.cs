using Detach.Collisions;
using Detach.Collisions.Primitives3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Collisions.Primitives3D;

[TestClass]
public class CylinderTests
{
	[DataTestMethod]
	[DataRow(true, 0, 0, 0)]
	[DataRow(true, 0, 0, 0.5f)]
	[DataRow(true, 0, 0, -0.5f)]
	[DataRow(true, 0, 0.5f, 0)]
	[DataRow(false, 0, -0.5f, 0)]
	[DataRow(true, 0.5f, 0, 0)]
	[DataRow(true, -0.5f, 0, 0)]
	[DataRow(true, 0.5f, 0.5f, 0)]
	[DataRow(true, -0.5f, 0.5f, 0)]
	[DataRow(false, 0.5f, -0.5f, 0)]
	[DataRow(false, -0.5f, -0.5f, 0)]
	[DataRow(true, 0.5f, 0, 0.5f)]
	[DataRow(true, -0.5f, 0, 0.5f)]
	[DataRow(true, 0.5f, 0, -0.5f)]
	[DataRow(true, -0.5f, 0, -0.5f)]
	[DataRow(true, 0, 0.5f, 0.5f)]
	[DataRow(false, 0, -0.5f, 0.5f)]
	[DataRow(true, 0, 0.5f, -0.5f)]
	[DataRow(false, 0, -0.5f, -0.5f)]
	[DataRow(true, 0.5f, 0.5f, 0.5f)]
	[DataRow(true, -0.5f, 0.5f, 0.5f)]
	[DataRow(false, 0.5f, -0.5f, 0.5f)]
	[DataRow(false, -0.5f, -0.5f, 0.5f)]
	[DataRow(true, 0.5f, 0.5f, -0.5f)]
	[DataRow(true, -0.5f, 0.5f, -0.5f)]
	[DataRow(false, 0.5f, -0.5f, -0.5f)]
	[DataRow(false, -0.5f, -0.5f, -0.5f)]
	[DataRow(false, 1, 1, 1)]
	[DataRow(false, 1, 1, 0.5f)]
	[DataRow(true, 1, 0.5f, 0)]
	[DataRow(false, 1.0001f, 0.5f, 0)]
	[DataRow(false, 1, 0.5f, 0.01f)]
	[DataRow(false, 1, 0.5f, -0.01f)]
	public void PointInCylinder(bool expectedContains, float pointX, float pointY, float pointZ)
	{
		Cylinder cylinder = new(new Vector3(0, 0, 0), 1, 1);
		Assert.AreEqual(expectedContains, Geometry3D.PointInCylinder(new Vector3(pointX, pointY, pointZ), cylinder));
	}
}
