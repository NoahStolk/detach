namespace CollisionFormats.Model;

public sealed record CollisionAlgorithmScenario(
	List<object> Arguments,
	List<object> OutArguments,
	object? ReturnValue,
	bool Incorrect)
{
#pragma warning disable SA1401 // Fields should be private
	public bool Incorrect = Incorrect;
#pragma warning restore SA1401

	public CollisionAlgorithmScenario DeepCopy()
	{
		List<object> arguments = [];
		arguments.AddRange(Arguments);

		List<object> outArguments = [];
		outArguments.AddRange(OutArguments);

		return this with { Arguments = arguments, OutArguments = outArguments };
	}
}
