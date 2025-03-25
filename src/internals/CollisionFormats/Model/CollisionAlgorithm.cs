namespace CollisionFormats.Model;

public sealed record CollisionAlgorithm(
	string MethodSignature,
	List<CollisionAlgorithmParameter> Parameters,
	List<CollisionAlgorithmParameter> OutParameters,
	string ReturnTypeName,
	List<CollisionAlgorithmScenario> Scenarios);
