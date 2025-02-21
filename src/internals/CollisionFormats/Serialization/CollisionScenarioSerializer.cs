using CollisionFormats.Extensions;
using Detach.Extensions;

namespace CollisionFormats.Serialization;

public static class CollisionScenarioSerializer
{
	private static ReadOnlySpan<byte> Identifier => "CSID00"u8; // Collision Scenario Input Data

	public static byte[] Serialize(List<CollisionScenario> collisionScenarios)
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		bw.Write(Identifier);
		bw.Write(collisionScenarios.Count);
		foreach (CollisionScenario collisionScenario in collisionScenarios)
			SerializeCollisionScenario(collisionScenario, bw);

		return ms.ToArray();
	}

	public static List<CollisionScenario> Deserialize(byte[] data)
	{
		using MemoryStream ms = new(data);
		using BinaryReader br = new(ms);

		ReadOnlySpan<byte> identifier = br.ReadBytes(Identifier.Length);
		if (!identifier.SequenceEqual(Identifier))
			throw new FormatException("Invalid Collision Scenario Input Data identifier.");

		int scenarioCount = br.ReadInt32();
		List<CollisionScenario> collisionScenarios = new(scenarioCount);
		for (int i = 0; i < scenarioCount; i++)
			collisionScenarios.Add(DeserializeCollisionScenario(br));

		return collisionScenarios;
	}

	private static void SerializeCollisionScenario(CollisionScenario collisionScenario, BinaryWriter bw)
	{
		if (collisionScenario.Parameters.Length > byte.MaxValue)
			throw new ArgumentException($"Too many parameters: {collisionScenario.Parameters.Length}");

		bw.Write((byte)collisionScenario.Parameters.Length);

		foreach (CollisionParameter parameter in collisionScenario.Parameters)
		{
			bw.Write((byte)parameter.CaseIndex);
			parameter.Switch(
				circle: bw.Write,
				circleCast: bw.Write,
				lineSegment2D: bw.Write,
				orientedRectangle: bw.Write,
				point2D: bw.Write,
				rectangle: bw.Write,
				triangle2D: bw.Write,
				aabb: bw.Write,
				coneFrustum: bw.Write,
				cylinder: bw.Write,
				lineSegment3D: bw.Write,
				obb: bw.Write,
				orientedPyramid: bw.Write,
				point3D: bw.Write,
				pyramid: bw.Write,
				ray: bw.Write,
				sphere: bw.Write,
				sphereCast: bw.Write,
				triangle3D: bw.Write,
				viewFrustum: _ => throw new NotSupportedException("Serializing ViewFrustum is not supported right now."));
		}
	}

	private static CollisionScenario DeserializeCollisionScenario(BinaryReader br)
	{
		int count = br.ReadByte();
		CollisionParameter[] parameters = new CollisionParameter[count];
		for (int i = 0; i < count; i++)
		{
			int caseIndex = br.ReadByte();
			parameters[i] = caseIndex switch
			{
				CollisionParameter.CircleIndex => CollisionParameter.Circle(br.ReadCircle()),
				CollisionParameter.CircleCastIndex => CollisionParameter.CircleCast(br.ReadCircleCast()),
				CollisionParameter.LineSegment2DIndex => CollisionParameter.LineSegment2D(br.ReadLineSegment2D()),
				CollisionParameter.OrientedRectangleIndex => CollisionParameter.OrientedRectangle(br.ReadOrientedRectangle()),
				CollisionParameter.Point2DIndex => CollisionParameter.Point2D(br.ReadVector2()),
				CollisionParameter.RectangleIndex => CollisionParameter.Rectangle(br.ReadRectangle()),
				CollisionParameter.Triangle2DIndex => CollisionParameter.Triangle2D(br.ReadTriangle2D()),
				CollisionParameter.AabbIndex => CollisionParameter.Aabb(br.ReadAabb()),
				CollisionParameter.ConeFrustumIndex => CollisionParameter.ConeFrustum(br.ReadConeFrustum()),
				CollisionParameter.CylinderIndex => CollisionParameter.Cylinder(br.ReadCylinder()),
				CollisionParameter.LineSegment3DIndex => CollisionParameter.LineSegment3D(br.ReadLineSegment3D()),
				CollisionParameter.ObbIndex => CollisionParameter.Obb(br.ReadObb()),
				CollisionParameter.OrientedPyramidIndex => CollisionParameter.OrientedPyramid(br.ReadOrientedPyramid()),
				CollisionParameter.Point3DIndex => CollisionParameter.Point3D(br.ReadVector3()),
				CollisionParameter.PyramidIndex => CollisionParameter.Pyramid(br.ReadPyramid()),
				CollisionParameter.RayIndex => CollisionParameter.Ray(br.ReadRay()),
				CollisionParameter.SphereIndex => CollisionParameter.Sphere(br.ReadSphere()),
				CollisionParameter.SphereCastIndex => CollisionParameter.SphereCast(br.ReadSphereCast()),
				CollisionParameter.Triangle3DIndex => CollisionParameter.Triangle3D(br.ReadTriangle3D()),
				_ => throw new NotSupportedException($"Case index {caseIndex} is not supported."),
			};
		}

		return new CollisionScenario(parameters);
	}
}
