namespace ExecutableCollisionAlgorithmsGenerator.Model;

internal sealed record Parameter
{
	public Parameter(Type type, string name)
	{
		Name = name;
		FormattableTypeName = FormatTypeName(type);
	}

	public string Name { get; }
	public string FormattableTypeName { get; }

	private static string FormatTypeName(Type type)
	{
		string typeName = type.Name.Replace("&", string.Empty, StringComparison.Ordinal);
		if (!type.IsGenericType)
			return typeName;

		int index = typeName.IndexOf('`', StringComparison.Ordinal);
		if (index > 0)
			typeName = typeName[..index];
		return $"{typeName}<{string.Join(',', type.GetGenericArguments().Select(FormatTypeName))}>";
	}
}
