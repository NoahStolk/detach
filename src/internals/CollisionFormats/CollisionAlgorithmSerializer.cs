using CollisionFormats.Model;
using System.Text.Json;

namespace CollisionFormats;

public static class CollisionAlgorithmSerializer
{
	private static readonly JsonSerializerOptions _jsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
		IncludeFields = true,
	};

	public static string SerializeJson(CollisionAlgorithm collisionAlgorithm)
	{
		return JsonSerializer.Serialize(collisionAlgorithm, _jsonOptions);
	}

	public static CollisionAlgorithm DeserializeJson(string json)
	{
		return JsonSerializer.Deserialize<CollisionAlgorithm>(json, _jsonOptions) ?? throw new FormatException("Invalid JSON format.");
	}
}
