using Detach.CodeGeneration;

namespace Tools.Generator.Generators;

internal sealed class BinaryReaderExtensionsIntVectorGenerator : IGenerator
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
	];

	public string Generate()
	{
		CodeWriter codeWriter = new();
		codeWriter.WriteHeader();
		codeWriter.WriteLine();
		codeWriter.WriteLine("using Detach.Numerics;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("namespace Detach.Extensions;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("public static partial class BinaryReaderExtensions");
		codeWriter.StartBlock();

		for (int i = 2; i <= 4; i++)
		{
			string intVectorTypeName = $"IntVector{i}";

			foreach ((string primitiveTypeName, string methodNamePart, string readerMethodName) in _types)
			{
				string readIntVectorMethodName = $"Read{intVectorTypeName}Of{methodNamePart}";

				codeWriter.WriteLine($"public static {intVectorTypeName}<{primitiveTypeName}> {readIntVectorMethodName}(this BinaryReader binaryReader)");
				codeWriter.StartBlock();
				codeWriter.WriteLine($"return new {intVectorTypeName}<{primitiveTypeName}>({string.Join(", ", Enumerable.Range(0, i).Select(_ => $"binaryReader.{readerMethodName}()"))});");
				codeWriter.EndBlock();

				codeWriter.WriteLine();
			}
		}

		codeWriter.EndBlock();

		return codeWriter.ToString();
	}
}
