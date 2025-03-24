using CollisionFormats;
using CollisionFormats.Model;
using Detach.Collisions;
using System.Reflection;

namespace Demos.Collisions.Interactable.Services.States;

internal sealed class CollisionScenarioState
{
	private const string _baseDirectory = @"C:\Users\NOAH\source\repos\detach\src\tests\Detach.Tests.Snapshot\Resources";

	public CollisionScenarioState()
	{
		CollectAlgorithms(typeof(Geometry2D));
		CollectAlgorithms(typeof(Geometry3D));

		ComboString = string.Join("\0", CollisionAlgorithms.Select(ca => ca.FullMethodName));
		ComboString += "\0";
	}

	public List<CollisionAlgorithm> CollisionAlgorithms { get; } = [];
	public string ComboString { get; }

	private void CollectAlgorithms(Type type)
	{
		const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public;
		foreach (MethodInfo method in type.GetMethods(bindingFlags))
		{
			List<CollisionAlgorithmParameter> parameters = [];
			List<CollisionAlgorithmParameter> outParameters = [];

			ParameterInfo[] parameterInfos = method.GetParameters();
			foreach (ParameterInfo parameterInfo in parameterInfos)
			{
				if (parameterInfo.Name == null)
					continue;

				CollisionAlgorithmParameter parameter = new(parameterInfo.ParameterType.FullName ?? parameterInfo.ParameterType.Name, parameterInfo.Name);
				if (parameterInfo.IsOut)
					outParameters.Add(parameter);
				else
					parameters.Add(parameter);
			}

			string fullMethodName = $"{type.FullName}.{method.Name}";
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
				method.ReturnType.FullName ?? method.ReturnType.Name,
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
