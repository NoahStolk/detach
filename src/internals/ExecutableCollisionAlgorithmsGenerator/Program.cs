using ExecutableCollisionAlgorithmsGenerator;
using ExecutableCollisionAlgorithmsGenerator.Model;

List<Algorithm> models = AlgorithmDiscovery.Discover();
foreach (Algorithm model in models)
	Console.WriteLine(model);
