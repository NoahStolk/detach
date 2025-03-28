using ExecutableCollisionAlgorithmsGenerator;
using ExecutableCollisionAlgorithmsGenerator.Model;

List<Algorithm> algorithms = AlgorithmDiscovery.Discover();
string generatedCode = AlgorithmGenerator.Generate(algorithms);
await File.WriteAllTextAsync("../../../../CollisionFormats/Execution/ExecutableCollisionAlgorithms.g.cs", generatedCode).ConfigureAwait(true);
