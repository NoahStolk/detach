using Tools.Generator.Internals;

namespace Tools.Generator.Generators;

internal sealed class BinaryReaderExtensionsBufferGenerator : IGenerator
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
		("Half", "Float16"),
		("float", "Float32"),
		("double", "Float64"),
	];

	private static string GetSizeOfPrimitive(string primitiveTypeName)
	{
		return primitiveTypeName == "Half" ? "ushort" : primitiveTypeName;
	}

	public string Generate()
	{
		CodeWriter codeWriter = new();

		codeWriter.WriteLine(GeneratorConstants.Header);
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
			foreach ((string builtInTypeName, string methodNamePart) in _types)
			{
				string bufferTypeName = $"Buffer{bufferSize}<{builtInTypeName}>";
				string readBufferMethodName = $"ReadBuffer{bufferSize}Of{methodNamePart}";

				codeWriter.WriteLine($"public static {bufferTypeName} {readBufferMethodName}(this BinaryReader binaryReader)");
				codeWriter.StartBlock();

				codeWriter.WriteLine($"Span<byte> buffer = stackalloc byte[{bufferTypeName}.Size * sizeof({GetSizeOfPrimitive(builtInTypeName)})];");
				codeWriter.WriteLine("Debug.Assert(binaryReader.Read(buffer) == buffer.Length);");
				codeWriter.WriteLine($"return new {bufferTypeName}(MemoryMarshal.Cast<byte, {builtInTypeName}>(buffer));");

				codeWriter.EndBlock();

				codeWriter.WriteLine();
			}
		}

		codeWriter.EndBlock();

		return codeWriter.ToString();
	}
}
