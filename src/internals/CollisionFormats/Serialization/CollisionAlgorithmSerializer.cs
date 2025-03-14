using CollisionFormats.Extensions;
using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using Detach.Extensions;
using System.Numerics;
using System.Text.Json;

namespace CollisionFormats.Serialization;

public static class CollisionAlgorithmSerializer
{
	private static readonly JsonSerializerOptions _jsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
		IncludeFields = true,
	};

	private static ReadOnlySpan<byte> Identifier => "CAID"u8; // Collision Algorithm Input Data

	public static string SerializeJson(CollisionAlgorithm collisionAlgorithm)
	{
		return JsonSerializer.Serialize(collisionAlgorithm, _jsonOptions);
	}

	public static CollisionAlgorithm DeserializeJson(string json)
	{
		return JsonSerializer.Deserialize<CollisionAlgorithm>(json, _jsonOptions) ?? throw new FormatException("Invalid JSON format.");
	}

	public static byte[] Serialize(CollisionAlgorithm collisionAlgorithm)
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		bw.Write(Identifier);
		bw.Write(collisionAlgorithm.FullMethodName);

		bw.Write((byte)collisionAlgorithm.Parameters.Count);
		foreach (CollisionAlgorithmParameter parameter in collisionAlgorithm.Parameters)
		{
			bw.Write(parameter.TypeName);
			bw.Write(parameter.Name);
		}

		bw.Write((byte)collisionAlgorithm.OutParameters.Count);
		foreach (CollisionAlgorithmParameter parameter in collisionAlgorithm.OutParameters)
		{
			bw.Write(parameter.TypeName);
			bw.Write(parameter.Name);
		}

		bw.Write(collisionAlgorithm.ReturnTypeName);

		bw.Write(collisionAlgorithm.Scenarios.Count);
		foreach (CollisionAlgorithmScenario scenario in collisionAlgorithm.Scenarios)
		{
			for (int i = 0; i < collisionAlgorithm.Parameters.Count; i++)
				Write(bw, collisionAlgorithm.Parameters[i].TypeName, scenario.Arguments[i]);

			for (int i = 0; i < collisionAlgorithm.OutParameters.Count; i++)
				Write(bw, collisionAlgorithm.OutParameters[i].TypeName, scenario.OutArguments[i]);

			if (scenario.ReturnValue is not null)
				Write(bw, collisionAlgorithm.ReturnTypeName, scenario.ReturnValue);
		}

		return ms.ToArray();
	}

	public static CollisionAlgorithm Deserialize(byte[] bytes)
	{
		using MemoryStream ms = new(bytes);
		using BinaryReader br = new(ms);

		ReadOnlySpan<byte> identifier = br.ReadBytes(Identifier.Length);
		if (!identifier.SequenceEqual(Identifier))
			throw new FormatException("Invalid format identifier.");

		string fullMethodName = br.ReadString();

		byte parameterCount = br.ReadByte();
		List<CollisionAlgorithmParameter> parameters = [];
		for (int i = 0; i < parameterCount; i++)
			parameters.Add(new CollisionAlgorithmParameter(br.ReadString(), br.ReadString()));

		byte outParameterCount = br.ReadByte();
		List<CollisionAlgorithmParameter> outParameters = [];
		for (int i = 0; i < outParameterCount; i++)
			outParameters.Add(new CollisionAlgorithmParameter(br.ReadString(), br.ReadString()));

		string returnTypeName = br.ReadString();

		int scenarioCount = br.ReadInt32();
		List<CollisionAlgorithmScenario> scenarios = [];
		for (int i = 0; i < scenarioCount; i++)
		{
			List<object> arguments = [];
			for (int j = 0; j < parameterCount; j++)
				arguments.Add(Read(br, parameters[j].TypeName));

			List<object> outArguments = [];
			for (int j = 0; j < outParameterCount; j++)
				outArguments.Add(Read(br, outParameters[j].TypeName));

			object? returnValue = null;
			if (returnTypeName != "void")
				returnValue = Read(br, returnTypeName);

			scenarios.Add(new CollisionAlgorithmScenario(arguments, outArguments, returnValue));
		}

		return new CollisionAlgorithm(fullMethodName, parameters, outParameters, returnTypeName, scenarios);
	}

	private static void Write(BinaryWriter bw, string parameterTypeName, object data)
	{
		switch (parameterTypeName)
		{
			case "Circle": bw.Write((Circle)data); break;
			case "CircleCast": bw.Write((CircleCast)data); break;
			case "LineSegment2D": bw.Write((LineSegment2D)data); break;
			case "OrientedRectangle": bw.Write((OrientedRectangle)data); break;
			case "Vector2": bw.Write((Vector2)data); break;
			case "Rectangle": bw.Write((Rectangle)data); break;
			case "Triangle2D": bw.Write((Triangle2D)data); break;
			case "Aabb": bw.Write((Aabb)data); break;
			case "ConeFrustum": bw.Write((ConeFrustum)data); break;
			case "Cylinder": bw.Write((Cylinder)data); break;
			case "LineSegment3D": bw.Write((LineSegment3D)data); break;
			case "Obb": bw.Write((Obb)data); break;
			case "OrientedPyramid": bw.Write((OrientedPyramid)data); break;
			case "Vector3": bw.Write((Vector3)data); break;
			case "Pyramid": bw.Write((Pyramid)data); break;
			case "Ray": bw.Write((Ray)data); break;
			case "Sphere": bw.Write((Sphere)data); break;
			case "SphereCast": bw.Write((SphereCast)data); break;
			case "Triangle3D": bw.Write((Triangle3D)data); break;
			default: throw new NotSupportedException($"Type '{parameterTypeName}' is not supported.");
		}
	}

	private static object Read(BinaryReader br, string parameterTypeName)
	{
		return parameterTypeName switch
		{
			"Circle" => br.ReadCircle(),
			"CircleCast" => br.ReadCircleCast(),
			"LineSegment2D" => br.ReadLineSegment2D(),
			"OrientedRectangle" => br.ReadOrientedRectangle(),
			"Vector2" => br.ReadVector2(),
			"Rectangle" => br.ReadRectangle(),
			"Triangle2D" => br.ReadTriangle2D(),
			"Aabb" => br.ReadAabb(),
			"ConeFrustum" => br.ReadConeFrustum(),
			"Cylinder" => br.ReadCylinder(),
			"LineSegment3D" => br.ReadLineSegment3D(),
			"Obb" => br.ReadObb(),
			"OrientedPyramid" => br.ReadOrientedPyramid(),
			"Vector3" => br.ReadVector3(),
			"Pyramid" => br.ReadPyramid(),
			"Ray" => br.ReadRay(),
			"Sphere" => br.ReadSphere(),
			"SphereCast" => br.ReadSphereCast(),
			"Triangle3D" => br.ReadTriangle3D(),
			_ => throw new NotSupportedException($"Type '{parameterTypeName}' is not supported."),
		};
	}
}
