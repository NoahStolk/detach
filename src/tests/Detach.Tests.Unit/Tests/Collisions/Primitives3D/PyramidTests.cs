using Detach.Buffers;
using Detach.Collisions.Primitives3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Collisions.Primitives3D;

[TestClass]
public sealed class PyramidTests
{
	[TestMethod]
	public void BaseVertices()
	{
		Vector3 center = new(0, 0, 0);
		Vector3 size = new(1, 1, 1);
		Pyramid pyramid = new(center, size);

		Buffer4<Vector3> baseVertices = pyramid.BaseVertices;

		Assert.AreEqual(new Vector3(-0.5f, -0.5f, -0.5f), baseVertices[0]);
		Assert.AreEqual(new Vector3(0.5f, -0.5f, -0.5f), baseVertices[1]);
		Assert.AreEqual(new Vector3(0.5f, -0.5f, 0.5f), baseVertices[2]);
		Assert.AreEqual(new Vector3(-0.5f, -0.5f, 0.5f), baseVertices[3]);
	}

	[TestMethod]
	public void Faces()
	{
		Vector3 center = new(0, 0, 0);
		Vector3 size = new(1, 1, 1);
		Pyramid pyramid = new(center, size);

		Buffer6<Triangle3D> faces = pyramid.Faces;

		Assert.AreEqual(new Triangle3D(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0, 0.5f, 0)), faces[0]);
		Assert.AreEqual(new Triangle3D(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0, 0.5f, 0)), faces[1]);
		Assert.AreEqual(new Triangle3D(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0, 0.5f, 0)), faces[2]);
		Assert.AreEqual(new Triangle3D(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0, 0.5f, 0)), faces[3]);

		// Base
		Assert.AreEqual(new Triangle3D(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, 0.5f)), faces[4]);
		Assert.AreEqual(new Triangle3D(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(-0.5f, -0.5f, -0.5f)), faces[5]);
	}
}
