using CollisionFormats.Serialization;
using Detach.Collisions;
using System.Reflection;

namespace Demos.Collisions.Interactable.Services.States;

internal sealed class CollisionScenarioState
{
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

			CollisionAlgorithm collisionAlgorithm = new(
				$"{type.FullName}.{method.Name}",
				parameters,
				outParameters,
				method.ReturnType.FullName ?? method.ReturnType.Name,
				[]);
			CollisionAlgorithms.Add(collisionAlgorithm);
		}
	}

	public void AddScenario(string algorithmName, CollisionAlgorithmScenario scenario)
	{
		CollisionAlgorithm? algorithm = CollisionAlgorithms.Find(ca => ca.FullMethodName == algorithmName);
		algorithm?.Scenarios.Add(scenario);
	}
}
