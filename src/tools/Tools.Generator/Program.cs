using Tools.Generator.Generators;
using Tools.Generator.Internals;

if (args.Length == 0)
{
	Console.WriteLine("Please provide the libs directory of the project.");
	Environment.Exit(1);
}

string libsDirectory = args[0];

ExecuteGenerator<BufferGenerator>("Detach", "Extensions", "VectorExtensions.RoundingOperations.g.cs");
ExecuteGenerator<BinaryReaderExtensionsBufferGenerator>("Detach", "Extensions", "BinaryReaderExtensions.Buffer.g.cs");
ExecuteGenerator<BinaryReaderExtensionsIntVectorGenerator>("Detach", "Extensions", "BinaryReaderExtensions.IntVector.g.cs");
ExecuteGenerator<BinaryWriterExtensionsBufferGenerator>("Detach", "Extensions", "BinaryWriterExtensions.Buffer.g.cs");
ExecuteGenerator<BinaryWriterExtensionsIntVectorGenerator>("Detach", "Extensions", "BinaryWriterExtensions.IntVector.g.cs");
ExecuteGenerator<VectorExtensionsRoundingOperationsGenerator>("Detach", "Extensions", "VectorExtensions.RoundingOperations.g.cs");

void ExecuteGenerator<TGenerator>(params string[] pathParts)
	where TGenerator : IGenerator, new()
{
	TGenerator generator = new();
	string generatedCode = generator.Generate();

	string outputPath = Path.Combine(libsDirectory, Path.Combine(pathParts));
	File.WriteAllText(outputPath, generatedCode);

	Console.WriteLine($"Generated {outputPath} using {generator.GetType().Name}");
}
