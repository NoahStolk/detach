namespace ExecutableCollisionAlgorithmsGenerator.Model;

internal sealed record Algorithm(string MethodSignature, List<Parameter> Parameters, List<Parameter> OutParameters, Type ReturnValue)
{
	public override string ToString()
	{
		// TODO: Fix generic parameter types.
		return $"{MethodSignature} | {string.Join(", ", Parameters.Select(p => $"{p.Type.Name} {p.Name}"))} | {string.Join(", ", OutParameters.Select(p => $"out {p.Type.Name} {p.Name}"))} | Return: {ReturnValue}";
	}
}
