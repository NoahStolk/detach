using Tools.Generator.Internals;

namespace Tools.Generator.Generators;

internal sealed class VectorExtensionsRoundingOperationsGenerator : IGenerator
{
	private readonly (string PrimitiveTypeName, string MethodNamePart)[] _types =
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
	private readonly string[] _roundingOperationNames = ["Round", "Floor", "Ceiling"];
	private readonly string[] _vectorComponentNames = ["X", "Y", "Z", "W"];

	private string GenerateArgumentList(int count, string prepend, string append)
	{
		string[] arguments = new string[count];
		for (int i = 0; i < count; i++)
			arguments[i] = $"{prepend}{_vectorComponentNames[i]}{append}";

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

			foreach (string roundingOperationName in _roundingOperationNames)
			{
				foreach ((string primitiveTypeName, string methodNamePart) in _types)
				{
					codeWriter.WriteLine($"public static {intVectorTypeName}<{primitiveTypeName}> {roundingOperationName}To{intVectorTypeName}Of{methodNamePart}(this Vector{i} vector)");
					codeWriter.StartBlock();
					codeWriter.WriteLine($"return new {intVectorTypeName}<{primitiveTypeName}>({GenerateArgumentList(i, $"({primitiveTypeName})MathF.{roundingOperationName}(vector.", ")")});");
					codeWriter.EndBlock();

					codeWriter.WriteLine();
				}
			}
		}

		for (int i = 2; i <= 4; i++)
		{
			foreach ((string primitiveTypeName, _) in _types)
			{
				codeWriter.WriteLine($"public static Vector{i} ToVector{i}(this IntVector{i}<{primitiveTypeName}> vector)");
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
