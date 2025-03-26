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
	[DataRow("Detach.Collisions.Geometry3D.ClosestPointOnLine(Vector3,LineSegment3D).txt")]
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
			return new Result(s.Incorrect, s.Arguments.ConvertAll(a => a.ToString() ?? "<NULL>"), executionResult);
		});

		return Verify(results);
	}

	private sealed record Result(bool Incorrect, List<string> Arguments, ExecutionResult ExecutionResult);
}
