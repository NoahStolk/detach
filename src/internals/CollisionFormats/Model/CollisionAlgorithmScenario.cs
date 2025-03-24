namespace CollisionFormats.Model;

public sealed record CollisionAlgorithmScenario(
	List<object> Arguments,
	List<object> OutArguments,
	object? ReturnValue)
{
	public CollisionAlgorithmScenario DeepCopy()
	{
		List<object> arguments = [];
		arguments.AddRange(Arguments);

		List<object> outArguments = [];
		outArguments.AddRange(OutArguments);

		return new CollisionAlgorithmScenario(arguments, outArguments, ReturnValue);
	}
}
