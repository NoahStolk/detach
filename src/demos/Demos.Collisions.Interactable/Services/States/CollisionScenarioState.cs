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
	}

	private void CollectAlgorithms(Type type)
	{
		foreach (MethodInfo method in type.GetMethods())
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

	public List<CollisionAlgorithm> CollisionAlgorithms { get; } = [];
}
