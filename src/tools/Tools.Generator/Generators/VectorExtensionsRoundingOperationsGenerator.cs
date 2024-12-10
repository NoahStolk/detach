using Tools.Generator.Internals;

namespace Tools.Generator.Generators;

internal sealed class VectorExtensionsRoundingOperationsGenerator
{
	private readonly (string BuiltInTypeName, string MethodName)[] _typeNames =
	[
		("sbyte", "Int8"),
		("byte", "UInt8"),
		("short", "Int16"),
		("ushort", "UInt16"),
		("int", "Int32"),
		("uint", "UInt32"),
		("long", "Int64"),
		("ulong", "UInt64"),
	];
	private readonly string[] _operations = ["Round", "Floor", "Ceiling"];
	private readonly string[] _components = ["X", "Y", "Z", "W"];

	private string GenerateArgumentList(int count, string prepend, string append)
	{
		string[] arguments = new string[count];
		for (int i = 0; i < count; i++)
			arguments[i] = $"{prepend}{_components[i]}{append}";

		return string.Join(", ", arguments);
	}

	public string Generate()
	{
		CodeWriter codeWriter = new();

		codeWriter.WriteLine(GeneratorConstants.Header);
		codeWriter.WriteLine();
		codeWriter.WriteLine("using Detach.Numerics;");
		codeWriter.WriteLine("using System.Numerics;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("namespace Detach.Extensions;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("public static partial class VectorExtensions");
		codeWriter.StartBlock();

		for (int i = 2; i <= 4; i++)
		{
			string intVectorTypeName = $"IntVector{i}";

			foreach (string operation in _operations)
			{
				foreach ((string builtInTypeName, string methodName) in _typeNames)
				{
					codeWriter.WriteLine($"public static {intVectorTypeName}<{builtInTypeName}> {operation}To{intVectorTypeName}Of{methodName}(this Vector{i} vector)");
					codeWriter.StartBlock();
					codeWriter.WriteLine($"return new {intVectorTypeName}<{builtInTypeName}>({GenerateArgumentList(i, $"({builtInTypeName})MathF.{operation}(vector.", ")")});");
					codeWriter.EndBlock();

					codeWriter.WriteLine();
				}
			}
		}

		for (int i = 2; i <= 4; i++)
		{
			foreach ((string builtInTypeName, _) in _typeNames)
			{
				codeWriter.WriteLine($"public static Vector{i} ToVector{i}(this IntVector{i}<{builtInTypeName}> vector)");
				codeWriter.StartBlock();
				codeWriter.WriteLine($"return new Vector{i}({GenerateArgumentList(i, "vector.", string.Empty)});");
				codeWriter.EndBlock();

				codeWriter.WriteLine();
			}
		}

		codeWriter.EndBlock();

		return codeWriter.ToString();
	}
}
