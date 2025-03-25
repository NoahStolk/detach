using Detach.CodeGeneration;
using Tools.Generator.Internals;

namespace Tools.Generator.Generators;

internal sealed class BinaryWriterExtensionsBufferGenerator : IGenerator
{
	private readonly string[] _primitiveTypeNames =
	[
		"sbyte",
		"byte",
		"short",
		"ushort",
		"int",
		"uint",
		"long",
		"ulong",
		"Half",
		"float",
		"double",
	];

	public string Generate()
	{
		CodeWriter codeWriter = new();
		codeWriter.WriteHeader();
		codeWriter.WriteLine();
		codeWriter.WriteLine("using Detach.Buffers;");
		codeWriter.WriteLine("using Detach.Numerics;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("namespace Detach.Extensions;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("public static partial class BinaryWriterExtensions");
		codeWriter.StartBlock();

		foreach (int bufferSize in BufferConstants.Sizes)
		{
			foreach (string builtInTypeName in _primitiveTypeNames)
			{
				string bufferTypeName = $"Buffer{bufferSize}<{builtInTypeName}>";

				codeWriter.WriteLine($"public static void Write(this BinaryWriter binaryWriter, {bufferTypeName} buffer)");
				codeWriter.StartBlock();
				codeWriter.WriteLine($"for (int i = 0; i < {bufferTypeName}.Size; i++)");
				codeWriter.StartIndent();
				codeWriter.WriteLine("binaryWriter.Write(buffer[i]);");
				codeWriter.EndIndent();
				codeWriter.EndBlock();

				codeWriter.WriteLine();
			}
		}

		codeWriter.EndBlock();

		return codeWriter.ToString();
	}
}
