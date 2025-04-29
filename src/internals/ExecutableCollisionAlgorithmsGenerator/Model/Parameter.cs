using ExecutableCollisionAlgorithmsGenerator.Utils;

namespace ExecutableCollisionAlgorithmsGenerator.Model;

internal sealed record Parameter
{
	public Parameter(Type type, string name)
	{
		Name = name;
		FormattableTypeName = TypeUtils.FormatTypeName(type);
	}

	public string Name { get; }
	public string FormattableTypeName { get; }
}
