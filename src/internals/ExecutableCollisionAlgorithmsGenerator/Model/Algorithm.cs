namespace ExecutableCollisionAlgorithmsGenerator.Model;

internal sealed record Algorithm(string MethodSignature, List<Parameter> Parameters, List<Parameter> OutParameters, Type ReturnValue)
{
	public string UniqueClassName => MethodSignature
		.Replace(".", "_", StringComparison.Ordinal)
		.Replace(",", "_", StringComparison.Ordinal)
		.Replace(" ", "_", StringComparison.Ordinal)
		.Replace("(", "_", StringComparison.Ordinal)
		.Replace(")", "_", StringComparison.Ordinal)
		.Replace("<", "_", StringComparison.Ordinal)
		.Replace(">", "_", StringComparison.Ordinal)
		.Replace("&", "_", StringComparison.Ordinal);

	public string MethodCall => MethodSignature[..MethodSignature.IndexOf('(', StringComparison.Ordinal)];
}
