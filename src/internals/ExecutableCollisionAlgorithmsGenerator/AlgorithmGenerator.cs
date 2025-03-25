using Detach.CodeGeneration;
using ExecutableCollisionAlgorithmsGenerator.Model;

namespace ExecutableCollisionAlgorithmsGenerator;

internal static class AlgorithmGenerator
{
	public static string Generate(List<Algorithm> algorithms)
	{
		CodeWriter codeWriter = new();
		codeWriter.WriteHeader();

		codeWriter.WriteLine("using Detach.Collisions;");
		codeWriter.WriteLine("using Detach.Collisions.Primitives2D;");
		codeWriter.WriteLine("using Detach.Collisions.Primitives3D;");
		codeWriter.WriteLine("using System.Numerics;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("namespace CollisionFormats.Execution;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("public static class ExecutableCollisionAlgorithms");
		codeWriter.StartBlock();
		codeWriter.WriteLine("private static readonly List<IExecutableCollisionAlgorithm> _all =");
		codeWriter.WriteLine("[");
		codeWriter.StartIndent();
		foreach (Algorithm algorithm in algorithms)
			codeWriter.WriteLine($"new {algorithm.UniqueClassName}(),");
		codeWriter.EndIndent();
		codeWriter.WriteLine("];");
		codeWriter.WriteLine();
		codeWriter.WriteLine("public static IReadOnlyList<IExecutableCollisionAlgorithm> All => _all;");
		codeWriter.WriteLine();

		foreach (Algorithm algorithm in algorithms)
			WriteAlgorithmClass(codeWriter, algorithm);

		codeWriter.EndBlock();

		return codeWriter.ToString();
	}

	private static void WriteAlgorithmClass(CodeWriter codeWriter, Algorithm algorithm)
	{
		codeWriter.WriteLine($"private sealed class {algorithm.UniqueClassName} : IExecutableCollisionAlgorithm");
		codeWriter.StartBlock();
		codeWriter.WriteLine($"public string Name => \"{algorithm.MethodSignature}\";");
		codeWriter.WriteLine();
		codeWriter.WriteLine("public IReadOnlyList<(Type Type, string Name)> Parameters { get; } = new List<(Type Type, string Name)>");
		codeWriter.StartBlock();
		foreach (Parameter parameter in algorithm.Parameters)
			codeWriter.WriteLine($"(typeof({parameter.FormattableTypeName}), \"{parameter.Name}\"),");
		codeWriter.EndBlockWithSemicolon();
		codeWriter.WriteLine();
		codeWriter.WriteLine("public IReadOnlyList<(Type Type, string Name)> OutParameters { get; } = new List<(Type Type, string Name)>");
		codeWriter.StartBlock();
		foreach (Parameter parameter in algorithm.OutParameters)
			codeWriter.WriteLine($"(typeof({parameter.FormattableTypeName}), \"{parameter.Name}\"),");
		codeWriter.EndBlockWithSemicolon();
		codeWriter.WriteLine();
		codeWriter.WriteLine($$"""public Type ReturnType { get; } = typeof({{algorithm.ReturnValue.FullName}});""");
		codeWriter.WriteLine();
		codeWriter.WriteLine("public ExecutionResult Execute(List<object> nonOutArguments)");
		codeWriter.StartBlock();
		codeWriter.WriteLine("if (nonOutArguments.Count != Parameters.Count)");
		codeWriter.StartIndent();
		codeWriter.WriteLine($"throw new ArgumentException(\"The number of arguments must be {algorithm.Parameters.Count}.\");");
		codeWriter.EndIndent();
		codeWriter.WriteLine();
		for (int i = 0; i < algorithm.Parameters.Count; i++)
			codeWriter.WriteLine($"{algorithm.Parameters[i].FormattableTypeName} argument{i} = ({algorithm.Parameters[i].FormattableTypeName})nonOutArguments[{i}];");
		codeWriter.WriteLine();

		string arguments = string.Join(", ", Enumerable.Range(0, algorithm.Parameters.Count).Select(i => $"argument{i}"));
		if (algorithm.OutParameters.Count > 0)
			arguments += $", {string.Join(", ", Enumerable.Range(0, algorithm.OutParameters.Count).Select(i => $"out {algorithm.OutParameters[i].FormattableTypeName} outArgument{i}"))}";

		codeWriter.WriteLine($"{algorithm.ReturnValue.FullName} returnValue = {algorithm.MethodCall}({arguments});");
		if (algorithm.OutParameters.Count > 0)
			codeWriter.WriteLine("return new ExecutionResult(returnValue, [" + string.Join(", ", Enumerable.Range(0, algorithm.OutParameters.Count).Select(i => $"outArgument{i}")) + "]);");
		else
			codeWriter.WriteLine("return new ExecutionResult(returnValue, []);");
		codeWriter.EndBlock();
		codeWriter.EndBlock();
		codeWriter.WriteLine();
	}
}
