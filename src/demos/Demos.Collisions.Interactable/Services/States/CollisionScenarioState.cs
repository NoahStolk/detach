﻿using CollisionFormats;
using CollisionFormats.Execution;
using CollisionFormats.Model;

namespace Demos.Collisions.Interactable.Services.States;

internal sealed class CollisionScenarioState
{
	private const string _baseDirectory = @"C:\Users\NOAH\source\repos\detach\src\tests\Detach.Tests.Snapshot\Resources";

	public CollisionScenarioState()
	{
		CollectAlgorithms();

		ComboString = string.Join("\0", CollisionAlgorithms.Select(ca => ca.FullMethodName));
		ComboString += "\0";
	}

	public List<CollisionAlgorithm> CollisionAlgorithms { get; } = [];
	public string ComboString { get; }

	private void CollectAlgorithms()
	{
		foreach (IExecutableCollisionAlgorithm executableCollisionAlgorithm in ExecutableCollisionAlgorithms.All)
		{
			List<CollisionAlgorithmParameter> parameters = executableCollisionAlgorithm.Parameters.Select(p => new CollisionAlgorithmParameter(p.Type.FullName ?? p.Type.Name, p.Name)).ToList();
			List<CollisionAlgorithmParameter> outParameters = executableCollisionAlgorithm.OutParameters.Select(p => new CollisionAlgorithmParameter(p.Type.FullName ?? p.Type.Name, p.Name)).ToList();

			string fullMethodName = executableCollisionAlgorithm.Name;
			List<CollisionAlgorithmScenario> scenarios = [];
			string scenariosFilePath = Path.Combine(_baseDirectory, $"{fullMethodName}.txt");
			if (File.Exists(scenariosFilePath))
			{
				string json = File.ReadAllText(scenariosFilePath);
				CollisionAlgorithm algorithm = CollisionAlgorithmSerializer.DeserializeText(json);
				scenarios = algorithm.Scenarios;
			}

			CollisionAlgorithm collisionAlgorithm = new(
				fullMethodName,
				parameters,
				outParameters,
				executableCollisionAlgorithm.ReturnType.FullName ?? throw new InvalidOperationException($"The type {executableCollisionAlgorithm.ReturnType.Name} does not have a full name."),
				scenarios);
			CollisionAlgorithms.Add(collisionAlgorithm);
		}
	}

	public void AddScenario(string algorithmName, CollisionAlgorithmScenario scenario)
	{
		CollisionAlgorithm? algorithm = CollisionAlgorithms.Find(ca => ca.FullMethodName == algorithmName);
		if (algorithm == null)
			return;

		algorithm.Scenarios.Add(scenario.DeepCopy());

		string json = CollisionAlgorithmSerializer.SerializeText(algorithm);
		string path = Path.Combine(_baseDirectory, $"{algorithm.FullMethodName}.txt");
		File.WriteAllText(path, json);
	}
}
