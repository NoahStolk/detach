namespace CollisionFormats.Serialization;

public sealed record CollisionAlgorithmScenario(
	List<object> Arguments,
	List<object> OutArguments,
	object? ReturnValue);
