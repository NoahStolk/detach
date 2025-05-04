using CollisionFormats;
using CollisionFormats.Execution;
using CollisionFormats.Model;

namespace Detach.Tests.Snapshot.Tests;

[TestClass]
[UsesVerify]
public partial class CollisionAlgorithmTests
{
	[DataTestMethod]
	[DataRow("Detach.Collisions.Geometry2D.CircleCircle(Circle,Circle).txt")]
	[DataRow("Detach.Collisions.Geometry2D.CircleRectangle(Circle,Rectangle).txt")]
	[DataRow("Detach.Collisions.Geometry2D.ClosestPointOnLine(Vector2,LineSegment2D).txt")]
	[DataRow("Detach.Collisions.Geometry3D.AabbAabb(Aabb,Aabb).txt")]
	[DataRow("Detach.Collisions.Geometry3D.AabbCylinder(Aabb,Cylinder).txt")]
	[DataRow("Detach.Collisions.Geometry3D.AabbObbSat(Aabb,Obb).txt")]
	[DataRow("Detach.Collisions.Geometry3D.ClosestPointInOrientedPyramid(Vector3,OrientedPyramid).txt")]
	[DataRow("Detach.Collisions.Geometry3D.ClosestPointInPyramid(Vector3,Pyramid).txt")]
	[DataRow("Detach.Collisions.Geometry3D.ClosestPointOnLine(Vector3,LineSegment3D).txt")]
	[DataRow("Detach.Collisions.Geometry3D.ClosestPointOnPlane(Vector3,Plane).txt")]
	[DataRow("Detach.Collisions.Geometry3D.ClosestPointOnTriangle(Vector3,Triangle3D).txt")]
	[DataRow("Detach.Collisions.Geometry3D.CylinderCylinder(Cylinder,Cylinder).txt")]
	[DataRow("Detach.Collisions.Geometry3D.Linetest(Aabb,LineSegment3D).txt")]
	[DataRow("Detach.Collisions.Geometry3D.Linetest(Obb,LineSegment3D).txt")]
	[DataRow("Detach.Collisions.Geometry3D.Linetest(Sphere,LineSegment3D).txt")]
	[DataRow("Detach.Collisions.Geometry3D.Linetest(Triangle3D,LineSegment3D).txt")]
	[DataRow("Detach.Collisions.Geometry3D.PointInAabb(Vector3,Aabb).txt")]
	[DataRow("Detach.Collisions.Geometry3D.PointInObb(Vector3,Obb).txt")]
	[DataRow("Detach.Collisions.Geometry3D.PointInOrientedPyramid(Vector3,OrientedPyramid).txt")]
	[DataRow("Detach.Collisions.Geometry3D.PointInPyramid(Vector3,Pyramid).txt")]
	[DataRow("Detach.Collisions.Geometry3D.PointInSphere(Vector3,Sphere).txt")]
	[DataRow("Detach.Collisions.Geometry3D.PointInTriangle(Vector3,Triangle3D).txt")]
	[DataRow("Detach.Collisions.Geometry3D.PointOnLine(Vector3,LineSegment3D).txt")]
	[DataRow("Detach.Collisions.Geometry3D.PointOnPlane(Vector3,Plane).txt")]
	[DataRow("Detach.Collisions.Geometry3D.Raycast(Sphere,Ray,Single).txt")]
	[DataRow("Detach.Collisions.Geometry3D.SphereCastObb(SphereCast,Obb,IntersectionResult).txt")]
	[DataRow("Detach.Collisions.Geometry3D.SphereCastOrientedPyramid(SphereCast,OrientedPyramid).txt")]
	[DataRow("Detach.Collisions.Geometry3D.SphereCastPoint(SphereCast,Vector3).txt")]
	[DataRow("Detach.Collisions.Geometry3D.SphereCastPyramid(SphereCast,Pyramid).txt")]
	[DataRow("Detach.Collisions.Geometry3D.SphereCastSphere(SphereCast,Sphere,Single,IntersectionResult).txt")]
	[DataRow("Detach.Collisions.Geometry3D.SphereCastTriangle(SphereCast,Triangle3D).txt")]
	[DataRow("Detach.Collisions.Geometry3D.SphereConeFrustum(Sphere,ConeFrustum,IntersectionResult).txt")]
	[DataRow("Detach.Collisions.Geometry3D.SphereCylinder(Sphere,Cylinder,IntersectionResult).txt")]
	[DataRow("Detach.Collisions.Geometry3D.SphereObb(Sphere,Obb,IntersectionResult).txt")]
	[DataRow("Detach.Collisions.Geometry3D.SphereOrientedPyramid(Sphere,OrientedPyramid).txt")]
	[DataRow("Detach.Collisions.Geometry3D.SphereTriangle(Sphere,Triangle3D,IntersectionResult).txt")]
	[DataRow("Detach.Collisions.Geometry3D.StandingCapsuleTriangle(StandingCapsule,Triangle3D,IntersectionResult).txt")]
	[DataRow("Detach.Collisions.Geometry3D.TriangleSphere(Triangle3D,Sphere).txt")]
	public Task TestCollisionAlgorithmScenarios(string fileName)
	{
#pragma warning disable CA1849
		CollisionAlgorithm algorithm = CollisionAlgorithmSerializer.DeserializeText(File.ReadAllText(Path.Combine("Resources", fileName)));
#pragma warning restore CA1849

		IExecutableCollisionAlgorithm? executableCollisionAlgorithm = ExecutableCollisionAlgorithms.All.FirstOrDefault(a => a.Name == algorithm.MethodSignature);
		if (executableCollisionAlgorithm == null)
			Assert.Fail($"The algorithm {algorithm.MethodSignature} was not found.");

		List<Result> results = algorithm.Scenarios.ConvertAll(s =>
		{
			ExecutionResult executionResult = executableCollisionAlgorithm.Execute(s.Arguments);
			return new Result(s.Incorrect ? "Incorrect" : "OK", s.Arguments.ConvertAll(a => a.ToString() ?? "<NULL>"), executionResult);
		});

		return Verify(results);
	}

	private sealed record Result(string Status, List<string> Arguments, ExecutionResult ExecutionResult);
}
