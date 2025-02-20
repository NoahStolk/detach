using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Demos.Collisions3D.Services;

internal sealed class CollisionAlgorithmState
{
	public Delegate? SelectedAlgorithm { get; private set; }
	public List<object?> Arguments { get; } = [];

	[MemberNotNullWhen(true, nameof(SelectedAlgorithm))]
	public bool StateIsValid => SelectedAlgorithm != null && !Arguments.Contains(null) && !Arguments.Contains(DBNull.Value);

	public void SelectAlgorithm(Delegate algorithm)
	{
		SelectedAlgorithm = algorithm;

		Arguments.Clear();
		foreach (ParameterInfo parameter in algorithm.Method.GetParameters())
			Arguments.Add(Activator.CreateInstance(parameter.ParameterType));
	}

	public TResult ExecuteAlgorithm<TResult>()
	{
		if (!StateIsValid)
			throw new InvalidOperationException("The algorithm or its arguments are not set.");

		object?[] argsArray = Arguments.ToArray(); // Must use array because of params.
		TResult? result = (TResult?)SelectedAlgorithm.DynamicInvoke(argsArray);
		if (result is null)
			throw new InvalidOperationException("The algorithm did not return a result.");

		return result;
	}
}
