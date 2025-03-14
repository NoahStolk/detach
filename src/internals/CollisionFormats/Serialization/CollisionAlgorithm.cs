namespace CollisionFormats.Serialization;

public sealed record CollisionAlgorithm(
	string FullMethodName,
	List<CollisionAlgorithmParameter> Parameters,
	List<CollisionAlgorithmParameter> OutParameters,
	string ReturnTypeName,
	List<CollisionAlgorithmScenario> Scenarios);
