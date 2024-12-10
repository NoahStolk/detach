using Tools.Generator.Internals;

namespace Tools.Generator.Generators;

internal sealed class BinaryWriterExtensionsIntVectorGenerator : IGenerator
{
	private readonly string[] _typeNames =
	[
		"sbyte",
		"byte",
		"short",
		"ushort",
		"int",
		"uint",
		"long",
		"ulong",
	];
	private readonly string[] _components = ["X", "Y", "Z", "W"];

	public string Generate()
	{
		CodeWriter codeWriter = new();

		codeWriter.WriteLine(GeneratorConstants.Header);
		codeWriter.WriteLine();
		codeWriter.WriteLine("using Detach.Numerics;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("namespace Detach.Extensions;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("public static partial class BinaryWriterExtensions");
		codeWriter.StartBlock();

		for (int i = 2; i <= 4; i++)
		{
			string intVectorTypeName = $"IntVector{i}";

			foreach (string builtInTypeName in _typeNames)
			{
				codeWriter.WriteLine($"public static void Write(this BinaryWriter binaryWriter, {intVectorTypeName}<{builtInTypeName}> value)");
				codeWriter.StartBlock();
				for (int j = 0; j < i; j++)
					codeWriter.WriteLine($"binaryWriter.Write(value.{_components[j]});");
				codeWriter.EndBlock();

				codeWriter.WriteLine();
			}
		}

		codeWriter.EndBlock();

		return codeWriter.ToString();
	}
}
