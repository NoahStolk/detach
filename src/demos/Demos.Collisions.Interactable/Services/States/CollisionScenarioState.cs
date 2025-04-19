using CollisionFormats;
using CollisionFormats.Execution;
using CollisionFormats.Model;

namespace Demos.Collisions.Interactable.Services.States;

internal sealed class CollisionScenarioState
{
	private const string _baseDirectory = @"C:\Users\NOAH\source\repos\detach\src\tests\Detach.Tests.Snapshot\Resources";

	public CollisionScenarioState()
	{
		CollectAlgorithms();
	}

	public List<CollisionAlgorithm> CollisionAlgorithms { get; } = [];

	public CollisionAlgorithm? GetAlgorithm(int index)
	{
		if (index < 0 || index >= CollisionAlgorithms.Count)
			return null;

		return CollisionAlgorithms[index];
	}

	private void CollectAlgorithms()
	{
		foreach (IExecutableCollisionAlgorithm executableCollisionAlgorithm in ExecutableCollisionAlgorithms.All)
		{
			List<CollisionAlgorithmParameter> parameters = executableCollisionAlgorithm.Parameters.Select(p => new CollisionAlgorithmParameter(p.Type.FullName ?? p.Type.Name, p.Name)).ToList();
			List<CollisionAlgorithmParameter> outParameters = executableCollisionAlgorithm.OutParameters.Select(p => new CollisionAlgorithmParameter(p.Type.FullName ?? p.Type.Name, p.Name)).ToList();

			string methodSignature = executableCollisionAlgorithm.Name;
			List<CollisionAlgorithmScenario> scenarios = [];
			string scenariosFilePath = Path.Combine(_baseDirectory, $"{methodSignature}.txt");
			if (File.Exists(scenariosFilePath))
			{
				string text = File.ReadAllText(scenariosFilePath);
				CollisionAlgorithm algorithm = CollisionAlgorithmSerializer.DeserializeText(text);
				scenarios = algorithm.Scenarios;

#if RESERIALIZE
				string newText = CollisionAlgorithmSerializer.SerializeText(algorithm);
				if (text != newText)
				{
					File.WriteAllText(scenariosFilePath, newText);
					Console.WriteLine($"Updated {methodSignature}");
				}
#endif
			}

			CollisionAlgorithm collisionAlgorithm = new(
				MethodSignature: methodSignature,
				Parameters: parameters,
				OutParameters: outParameters,
				ReturnTypeName: executableCollisionAlgorithm.ReturnType.FullName ?? throw new InvalidOperationException($"The type {executableCollisionAlgorithm.ReturnType.Name} does not have a full name."),
				Scenarios: scenarios);
			CollisionAlgorithms.Add(collisionAlgorithm);
		}
	}

	public void AddScenario(string algorithmName, CollisionAlgorithmScenario scenario)
	{
		CollisionAlgorithm? algorithm = CollisionAlgorithms.Find(ca => ca.MethodSignature == algorithmName);
		if (algorithm == null)
			return;

		algorithm.Scenarios.Add(scenario.DeepCopy());
		SaveFile(algorithm);
	}

	public void RemoveScenarioAt(string algorithmName, int index)
	{
		CollisionAlgorithm? algorithm = CollisionAlgorithms.Find(ca => ca.MethodSignature == algorithmName);
		if (algorithm == null)
			return;

		algorithm.Scenarios.RemoveAt(index);
		SaveFile(algorithm);
	}

	public static void SaveFile(CollisionAlgorithm algorithm)
	{
		string text = CollisionAlgorithmSerializer.SerializeText(algorithm);
		string path = Path.Combine(_baseDirectory, $"{algorithm.MethodSignature}.txt");
		File.WriteAllText(path, text);
	}
}
