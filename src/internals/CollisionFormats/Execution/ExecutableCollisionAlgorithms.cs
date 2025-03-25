using Detach.Collisions;
using Detach.Collisions.Primitives2D;

namespace CollisionFormats.Execution;

public static class ExecutableCollisionAlgorithms
{
	private static readonly List<IExecutableCollisionAlgorithm> _all =
	[
		new ExecutableCircleCircle(),
		new ExecutableCircleRectangle(),
	];

	public static IReadOnlyList<IExecutableCollisionAlgorithm> All => _all;

	private sealed class ExecutableCircleCircle : IExecutableCollisionAlgorithm
	{
		public string Name => "Detach.Collisions.Geometry2D.CircleCircle";

		public IReadOnlyList<(Type Type, string Name)> Parameters { get; } = new List<(Type Type, string Name)>
		{
			(typeof(Circle), "circle1"),
			(typeof(Circle), "circle2"),
		};

		public IReadOnlyList<(Type Type, string Name)> OutParameters { get; } = [];
		public Type ReturnType { get; } = typeof(bool);

		public ExecutionResult Execute(List<object> nonOutArguments)
		{
			if (nonOutArguments.Count != Parameters.Count)
				throw new ArgumentException($"The number of arguments must be {Parameters.Count}.");

			Circle argument1 = (Circle)nonOutArguments[0];
			Circle argument2 = (Circle)nonOutArguments[1];
			bool returnValue = Geometry2D.CircleCircle(argument1, argument2);
			return new ExecutionResult(returnValue, []);
		}
	}

	private sealed class ExecutableCircleRectangle : IExecutableCollisionAlgorithm
	{
		public string Name => "Detach.Collisions.Geometry2D.CircleRectangle";

		public IReadOnlyList<(Type Type, string Name)> Parameters { get; } = new List<(Type Type, string Name)>
		{
			(typeof(Circle), "circle"),
			(typeof(Rectangle), "rectangle"),
		};

		public IReadOnlyList<(Type Type, string Name)> OutParameters { get; } = [];
		public Type ReturnType { get; } = typeof(bool);

		public ExecutionResult Execute(List<object> nonOutArguments)
		{
			if (nonOutArguments.Count != Parameters.Count)
				throw new ArgumentException($"The number of arguments must be {Parameters.Count}.");

			Circle argument1 = (Circle)nonOutArguments[0];
			Rectangle argument2 = (Rectangle)nonOutArguments[1];
			bool returnValue = Geometry2D.CircleRectangle(argument1, argument2);
			return new ExecutionResult(returnValue, []);
		}
	}
}
