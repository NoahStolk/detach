namespace CollisionFormats.Execution;

public interface IExecutableCollisionAlgorithm
{
	string Name { get; }

	IReadOnlyList<(Type Type, string Name)> Parameters { get; }

	IReadOnlyList<(Type Type, string Name)> OutParameters { get; }

	Type ReturnType { get; }

	ExecutionResult Execute(List<object> nonOutArguments);
}
