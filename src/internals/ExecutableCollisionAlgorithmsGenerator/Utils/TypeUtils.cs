namespace ExecutableCollisionAlgorithmsGenerator.Utils;

internal static class TypeUtils
{
	public static string FormatTypeName(Type type)
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
