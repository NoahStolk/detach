using Tools.Generator.Generators;

const string baseDirectory = @"C:\Users\NOAH\source\repos\detach\src\libs\Detach";

BinaryReaderExtensionsIntVectorGenerator binaryReaderExtensionsIntVectorGenerator = new();
string binaryReaderExtensionsIntVectorCode = binaryReaderExtensionsIntVectorGenerator.Generate();
File.WriteAllText(Path.Combine(baseDirectory, "Extensions", "BinaryReaderExtensions.IntVector.g.cs"), binaryReaderExtensionsIntVectorCode);

BinaryWriterExtensionsIntVectorGenerator binaryWriterExtensionsIntVectorGenerator = new();
string binaryWriterExtensionsIntVectorCode = binaryWriterExtensionsIntVectorGenerator.Generate();
File.WriteAllText(Path.Combine(baseDirectory, "Extensions", "BinaryWriterExtensions.IntVector.g.cs"), binaryWriterExtensionsIntVectorCode);

VectorExtensionsRoundingOperationsGenerator vectorExtensionsRoundingOperationsGenerator = new();
string vectorExtensionsRoundingOperationsCode = vectorExtensionsRoundingOperationsGenerator.Generate();
File.WriteAllText(Path.Combine(baseDirectory, "Extensions", "VectorExtensions.RoundingOperations.g.cs"), vectorExtensionsRoundingOperationsCode);
