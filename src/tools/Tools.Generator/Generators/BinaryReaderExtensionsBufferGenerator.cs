using Detach.CodeGeneration;

namespace Tools.Generator.Generators;

internal sealed class BinaryReaderExtensionsBufferGenerator : IGenerator
{
	private readonly (string PrimitiveTypeName, string MethodNamePart, string ReaderMethodName)[] _types =
	[
		("sbyte", "Int8", "ReadSByte"),
		("byte", "UInt8", "ReadByte"),
		("short", "Int16", "ReadInt16"),
		("ushort", "UInt16", "ReadUInt16"),
		("int", "Int32", "ReadInt32"),
		("uint", "UInt32", "ReadUInt32"),
		("long", "Int64", "ReadInt64"),
		("ulong", "UInt64", "ReadUInt64"),
		("Half", "Float16", "ReadHalf"),
		("float", "Float32", "ReadSingle"),
		("double", "Float64", "ReadDouble"),
	];

	public string Generate()
	{
		CodeWriter codeWriter = new();
		codeWriter.WriteHeader();
		codeWriter.WriteLine();
		codeWriter.WriteLine("using Detach.Buffers;");
		codeWriter.WriteLine("using Detach.Numerics;");
		codeWriter.WriteLine("using System.Diagnostics;");
		codeWriter.WriteLine("using System.Runtime.InteropServices;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("namespace Detach.Extensions;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("public static partial class BinaryReaderExtensions");
		codeWriter.StartBlock();

		foreach (int bufferSize in BufferConstants.Sizes)
		{
			foreach ((string builtInTypeName, string methodNamePart, string readerMethodName) in _types)
			{
				string bufferTypeName = $"Buffer{bufferSize}<{builtInTypeName}>";
				string readBufferMethodName = $"ReadBuffer{bufferSize}Of{methodNamePart}";

				codeWriter.WriteLine($"public static {bufferTypeName} {readBufferMethodName}(this BinaryReader binaryReader)");
				codeWriter.StartBlock();

				codeWriter.WriteLine($"Span<{builtInTypeName}> buffer = stackalloc {builtInTypeName}[{bufferTypeName}.Size];");
				codeWriter.WriteLine($"for (int i = 0; i < {bufferTypeName}.Size; i++)");
				codeWriter.StartIndent();
				codeWriter.WriteLine($"buffer[i] = binaryReader.{readerMethodName}();");
				codeWriter.EndIndent();
				codeWriter.WriteLine($"return new {bufferTypeName}(buffer);");

				codeWriter.EndBlock();

				codeWriter.WriteLine();
			}
		}

		codeWriter.EndBlock();

		return codeWriter.ToString();
	}
}
