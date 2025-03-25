using Detach.Collisions;
using ExecutableCollisionAlgorithmsGenerator.Model;
using System.Reflection;

namespace ExecutableCollisionAlgorithmsGenerator;

internal static class AlgorithmDiscovery
{
	private static readonly string[] _excludedMethodNames = ["GetType", "GetHashCode", "Equals", "ToString"];

	public static List<Algorithm> Discover()
	{
		List<Algorithm> algorithms = [];
		algorithms.AddRange(Discover(typeof(Geometry2D)));
		algorithms.AddRange(Discover(typeof(Geometry3D)));
		return algorithms;
	}

	private static List<Algorithm> Discover(Type geometryType)
	{
		List<Algorithm> models = [];
		foreach (MethodInfo method in geometryType.GetMethods())
		{
			if (_excludedMethodNames.Any(s => s == method.Name))
				continue;

			ParameterInfo[] parameterInfos = method.GetParameters();
			if (parameterInfos.Any(pi => pi.ParameterType.IsByRefLike)) // Ignore ref structs for now.
				continue;

			List<Parameter> parameters = parameterInfos.Where(p => !p.IsOut).Select(p => new Parameter(p.ParameterType, p.Name ?? string.Empty)).ToList();
			List<Parameter> outParameters = parameterInfos.Where(p => p.IsOut).Select(p => new Parameter(p.ParameterType, p.Name ?? string.Empty)).ToList();
			Type returnValue = method.ReturnType;

			string methodSignature = $"{geometryType.FullName}.{method.Name}({string.Join(',', parameters.Concat(outParameters).Select(p => p.FormattableTypeName))})";
			models.Add(new Algorithm(methodSignature, parameters, outParameters, returnValue));
		}

		return models;
	}
}
