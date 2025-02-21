namespace CollisionFormats.Serialization;

public sealed record CollisionScenario<TParam1, TParam2>
	where TParam1 : struct
	where TParam2 : struct
{
	public CollisionScenario(string algorithmName, List<TParam1> params1, List<TParam2> params2)
	{
		if (params1.Count != params2.Count)
			throw new ArgumentException("Parameter arrays must be the same length.");

		AlgorithmName = algorithmName;
		Params1 = params1;
		Params2 = params2;
	}

	public string AlgorithmName { get; init; }
	public List<TParam1> Params1 { get; init; }
	public List<TParam2> Params2 { get; init; }
}
