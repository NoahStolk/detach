using Detach.Buffers;
using Detach.Collisions.Primitives3D;
using Detach.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Unit.Tests.Collisions.Primitives3D;

[TestClass]
public sealed class ObbTests
{
	private const float _tolerance = 0.0001f;

	[TestMethod]
	public void ObbGeometry()
	{
		Obb obb = new(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Matrix3.Identity);

		Buffer6<Plane> planes = obb.GetPlanes();
		AssertPlaneExists(planes, new Plane(new Vector3(1, 0, 0), 1));
		AssertPlaneExists(planes, new Plane(new Vector3(0, 1, 0), 1));
		AssertPlaneExists(planes, new Plane(new Vector3(0, 0, 1), 1));
		AssertPlaneExists(planes, new Plane(new Vector3(-1, 0, 0), 1));
		AssertPlaneExists(planes, new Plane(new Vector3(0, -1, 0), 1));
		AssertPlaneExists(planes, new Plane(new Vector3(0, 0, -1), 1));

		Buffer8<Vector3> vertices = obb.GetVertices();
		AssertVertexExists(vertices, new Vector3(1, 1, 1));
		AssertVertexExists(vertices, new Vector3(-1, 1, 1));
		AssertVertexExists(vertices, new Vector3(1, -1, 1));
		AssertVertexExists(vertices, new Vector3(1, 1, -1));
		AssertVertexExists(vertices, new Vector3(-1, -1, 1));
		AssertVertexExists(vertices, new Vector3(-1, 1, -1));
		AssertVertexExists(vertices, new Vector3(1, -1, -1));
		AssertVertexExists(vertices, new Vector3(-1, -1, -1));

		Buffer12<LineSegment3D> edges = obb.GetEdges();
		AssertEdgeExists(edges, new LineSegment3D(new Vector3( 1,  1,  1), new Vector3(-1,  1,  1)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3( 1,  1,  1), new Vector3( 1, -1,  1)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3( 1,  1,  1), new Vector3( 1,  1, -1)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(-1,  1,  1), new Vector3(-1, -1,  1)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(-1,  1,  1), new Vector3(-1,  1, -1)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3( 1, -1,  1), new Vector3(-1, -1,  1)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3( 1, -1,  1), new Vector3( 1, -1, -1)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3( 1,  1, -1), new Vector3(-1,  1, -1)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3( 1,  1, -1), new Vector3( 1, -1, -1)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(-1, -1,  1), new Vector3(-1, -1, -1)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(-1,  1, -1), new Vector3(-1, -1, -1)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3( 1, -1, -1), new Vector3(-1, -1, -1)));

		Obb obbMoved = new(new Vector3(1, 1, 1), new Vector3(1, 1, 1), Matrix3.Identity);

		planes = obbMoved.GetPlanes();
		AssertPlaneExists(planes, new Plane(new Vector3(1, 0, 0), 2));
		AssertPlaneExists(planes, new Plane(new Vector3(0, 1, 0), 2));
		AssertPlaneExists(planes, new Plane(new Vector3(0, 0, 1), 2));
		AssertPlaneExists(planes, new Plane(new Vector3(-1, 0, 0), 0));
		AssertPlaneExists(planes, new Plane(new Vector3(0, -1, 0), 0));
		AssertPlaneExists(planes, new Plane(new Vector3(0, 0, -1), 0));

		vertices = obbMoved.GetVertices();
		AssertVertexExists(vertices, new Vector3(2, 2, 2));
		AssertVertexExists(vertices, new Vector3(0, 2, 2));
		AssertVertexExists(vertices, new Vector3(2, 0, 2));
		AssertVertexExists(vertices, new Vector3(2, 2, 0));
		AssertVertexExists(vertices, new Vector3(0, 0, 2));
		AssertVertexExists(vertices, new Vector3(0, 2, 0));
		AssertVertexExists(vertices, new Vector3(2, 0, 0));
		AssertVertexExists(vertices, new Vector3(0, 0, 0));

		edges = obbMoved.GetEdges();
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(2, 2, 2), new Vector3(0, 2, 2)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(2, 2, 2), new Vector3(2, 0, 2)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(2, 2, 2), new Vector3(2, 2, 0)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(0, 2, 2), new Vector3(0, 0, 2)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(0, 2, 2), new Vector3(0, 2, 0)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(2, 0, 2), new Vector3(0, 0, 2)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(2, 0, 2), new Vector3(2, 0, 0)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(2, 2, 0), new Vector3(0, 2, 0)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(2, 2, 0), new Vector3(2, 0, 0)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(0, 0, 2), new Vector3(0, 0, 0)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(0, 2, 0), new Vector3(0, 0, 0)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(2, 0, 0), new Vector3(0, 0, 0)));

		Obb obbTilted = new(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Matrix3.RotationX(MathF.PI * 0.25f));

		planes = obbTilted.GetPlanes();
		AssertPlaneExists(planes, new Plane(new Vector3(1, 0, 0), 1));
		AssertPlaneExists(planes, new Plane(new Vector3(-1, 0, 0), 1));
		AssertPlaneExists(planes, new Plane(new Vector3(0, 0.70710678118f, 0.70710678118f), 1));
		AssertPlaneExists(planes, new Plane(new Vector3(0, -0.70710678118f, 0.70710678118f), 1));
		AssertPlaneExists(planes, new Plane(new Vector3(0, 0.70710678118f, -0.70710678118f), 1));
		AssertPlaneExists(planes, new Plane(new Vector3(0, -0.70710678118f, -0.70710678118f), 1));

		vertices = obbTilted.GetVertices();
		AssertVertexExists(vertices, new Vector3(1, 0, 1.41421356237f));
		AssertVertexExists(vertices, new Vector3(-1, 0, 1.41421356237f));
		AssertVertexExists(vertices, new Vector3(1, 0, -1.41421356237f));
		AssertVertexExists(vertices, new Vector3(-1, 0, -1.41421356237f));
		AssertVertexExists(vertices, new Vector3(1, 1.41421356237f, 0));
		AssertVertexExists(vertices, new Vector3(-1, 1.41421356237f, 0));
		AssertVertexExists(vertices, new Vector3(1, -1.41421356237f, 0));
		AssertVertexExists(vertices, new Vector3(-1, -1.41421356237f, 0));

		edges = obbTilted.GetEdges();
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(-1, 1.41421356237f, 0), new Vector3(-1, 0, 1.41421356237f)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(-1, 1.41421356237f, 0), new Vector3(1, 1.41421356237f, 0)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(-1, 1.41421356237f, 0), new Vector3(-1, 0, -1.41421356237f)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(1, -1.41421356237f, 0), new Vector3(-1, -1.41421356237f, 0)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(1, -1.41421356237f, 0), new Vector3(1, 0, -1.41421356237f)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(1, -1.41421356237f, 0), new Vector3(1, 0, 1.41421356237f)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(1, 0, 1.41421356237f), new Vector3(-1, 0, 1.41421356237f)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(1, 0, 1.41421356237f), new Vector3(1, 1.41421356237f, 0)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(-1, -1.41421356237f, 0), new Vector3(-1, 0, 1.41421356237f)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(-1, -1.41421356237f, 0), new Vector3(-1, 0, -1.41421356237f)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(-1, 0, -1.41421356237f), new Vector3(1, 0, -1.41421356237f)));
		AssertEdgeExists(edges, new LineSegment3D(new Vector3(1, 0, -1.41421356237f), new Vector3(1, 1.41421356237f, 0)));
	}

	private static void AssertPlaneExists(Buffer6<Plane> planes, Plane plane)
	{
		for (int i = 0; i < 6; i++)
		{
			bool normalEqual = AreVectorsEqual(planes[i].Normal, plane.Normal);
			bool distanceEqual = Math.Abs(planes[i].D - plane.D) < _tolerance;
			if (normalEqual && distanceEqual)
				return;
		}

		Assert.Fail($"Plane {plane} does not exist.");
	}

	private static void AssertVertexExists(Buffer8<Vector3> vertices, Vector3 vertex)
	{
		for (int i = 0; i < 8; i++)
		{
			if (AreVectorsEqual(vertices[i], vertex))
				return;
		}

		Assert.Fail($"Vertex {vertex} does not exist.");
	}

	private static void AssertEdgeExists(Buffer12<LineSegment3D> edges, LineSegment3D edge)
	{
		for (int i = 0; i < 12; i++)
		{
			bool startEqual = AreVectorsEqual(edges[i].Start, edge.Start);
			bool endEqual = AreVectorsEqual(edges[i].End, edge.End);
			if (startEqual && endEqual)
				return;

			bool startEqualFlipped = AreVectorsEqual(edges[i].Start, edge.End);
			bool endEqualFlipped = AreVectorsEqual(edges[i].End, edge.Start);
			if (startEqualFlipped && endEqualFlipped)
				return;
		}

		Assert.Fail($"Edge {edge} does not exist.");
	}

	private static bool AreVectorsEqual(Vector3 vertex1, Vector3 vertex2)
	{
		bool xEqual = Math.Abs(vertex1.X - vertex2.X) < _tolerance;
		bool yEqual = Math.Abs(vertex1.Y - vertex2.Y) < _tolerance;
		bool zEqual = Math.Abs(vertex1.Z - vertex2.Z) < _tolerance;
		return xEqual && yEqual && zEqual;
	}
}
