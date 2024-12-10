using Tools.Generator.Generators;
using Tools.Generator.Internals;

const string baseDirectory = @"C:\Users\NOAH\source\repos\detach\src\libs\Detach";

ExecuteGenerator<BufferGenerator>("Extensions", "VectorExtensions.RoundingOperations.g.cs");
ExecuteGenerator<BinaryReaderExtensionsIntVectorGenerator>("Extensions", "BinaryReaderExtensions.IntVector.g.cs");
ExecuteGenerator<BinaryWriterExtensionsIntVectorGenerator>("Extensions", "BinaryWriterExtensions.IntVector.g.cs");
ExecuteGenerator<VectorExtensionsRoundingOperationsGenerator>("Extensions", "VectorExtensions.RoundingOperations.g.cs");

static void ExecuteGenerator<TGenerator>(params string[] pathParts)
	where TGenerator : IGenerator, new()
{
	TGenerator generator = new();
	string generatedCode = generator.Generate();
	File.WriteAllText(Path.Combine(baseDirectory, Path.Combine(pathParts)), generatedCode);
}
