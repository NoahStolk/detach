using CollisionFormats.Model;
using Detach.Collisions;
using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using Detach.Numerics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace CollisionFormats;

public static class CollisionAlgorithmSerializer
{
	public static string SerializeText(CollisionAlgorithm collisionAlgorithm)
	{
		StringBuilder sb = new();
		sb.Append(collisionAlgorithm.MethodSignature);
		sb.Append(';');
		sb.AppendJoin(',', collisionAlgorithm.Parameters.Select(p => $"{p.TypeName} {p.Name}"));
		sb.Append(';');
		sb.AppendJoin(',', collisionAlgorithm.OutParameters.Select(p => $"{p.TypeName} {p.Name}"));
		sb.Append(';');
		sb.Append(collisionAlgorithm.ReturnTypeName);
		sb.AppendLine();
		foreach (CollisionAlgorithmScenario scenario in collisionAlgorithm.Scenarios)
		{
			sb.AppendJoin(',', scenario.Arguments.Select(SerializeValue));
			sb.Append(';');
			sb.Append(scenario.Incorrect ? "INCORRECT" : "OK");
			sb.AppendLine();
		}

		return sb.ToString();
	}

	public static CollisionAlgorithm DeserializeText(string text)
	{
		text = text.Replace("\r", string.Empty, StringComparison.InvariantCulture);

		string[] lines = text.Split('\n');
		string[] header = lines[0].Split(';');
		string fullMethodName = header[0];
		string[] parameters = header[1].Length == 0 ? [] : header[1].Split(',');
		string[] outParameters = header[2].Length == 0 ? [] : header[2].Split(',');
		string returnTypeName = header[3];

		List<CollisionAlgorithmScenario> scenarios = [];
		int i = 1;
		while (true)
		{
			string[] scenario = lines[i].Split(';');
			if (scenario.Length <= 1)
				break;

			string[] arguments = scenario[0].Length == 0 ? [] : scenario[0].Split(',');
			string incorrect = scenario[1];

			scenarios.Add(new CollisionAlgorithmScenario(
				arguments.Select((a, j) => GetValue(parameters[j].Split(' ')[0], a)).ToList(),
				incorrect == "INCORRECT"));

			i++;
		}

		return new CollisionAlgorithm(
			fullMethodName,
			parameters.Select(p => new CollisionAlgorithmParameter(p.Split(' ', 2)[0], p.Split(' ', 2)[1])).ToList(),
			outParameters.Select(p => new CollisionAlgorithmParameter(p.Split(' ', 2)[0], p.Split(' ', 2)[1])).ToList(),
			returnTypeName,
			scenarios);
	}

	private static string SerializeValue(object value)
	{
		return value.GetType() switch
		{
			_ when value is bool boolean => Serializer.Write(boolean),
			_ when value is float single => Serializer.Write(single),
			_ when value is Vector2 vector2 => Serializer.Write(vector2),
			_ when value is Vector3 vector3 => Serializer.Write(vector3),

			_ when value is Circle circle => Serializer.Write(circle),
			_ when value is CircleCast circleCast => Serializer.Write(circleCast),
			_ when value is LineSegment2D lineSegment2D => Serializer.Write(lineSegment2D),
			_ when value is OrientedRectangle orientedRectangle => Serializer.Write(orientedRectangle),
			_ when value is Rectangle rectangle => Serializer.Write(rectangle),
			_ when value is Triangle2D triangle2D => Serializer.Write(triangle2D),

			_ when value is Aabb aabb => Serializer.Write(aabb),
			_ when value is ConeFrustum coneFrustum => Serializer.Write(coneFrustum),
			_ when value is Cylinder cylinder => Serializer.Write(cylinder),
			_ when value is LineSegment3D lineSegment3D => Serializer.Write(lineSegment3D),
			_ when value is Obb obb => Serializer.Write(obb),
			_ when value is OrientedPyramid orientedPyramid => Serializer.Write(orientedPyramid),
			_ when value is Plane plane => Serializer.Write(plane),
			_ when value is Pyramid pyramid => Serializer.Write(pyramid),
			_ when value is Ray ray => Serializer.Write(ray),
			_ when value is Sphere sphere => Serializer.Write(sphere),
			_ when value is SphereCast sphereCast => Serializer.Write(sphereCast),
			_ when value is Triangle3D triangle3D => Serializer.Write(triangle3D),

			_ when value is IntersectionResult intersectionResult => Serializer.Write(intersectionResult),

			_ => throw new NotSupportedException($"The type {value.GetType()} is not supported."),
		};
	}

	private static object GetValue(string typeName, string argument)
	{
		return typeName switch
		{
			_ when typeName == typeof(bool).FullName => Serializer.ReadBool(argument),
			_ when typeName == typeof(float).FullName => Serializer.ReadFloat(argument),
			_ when typeName == typeof(Vector2).FullName => Serializer.ReadVector2(argument),
			_ when typeName == typeof(Vector3).FullName => Serializer.ReadVector3(argument),

			_ when typeName == typeof(Circle).FullName => Serializer.ReadCircle(argument),
			_ when typeName == typeof(CircleCast).FullName => Serializer.ReadCircleCast(argument),
			_ when typeName == typeof(LineSegment2D).FullName => Serializer.ReadLineSegment2D(argument),
			_ when typeName == typeof(OrientedRectangle).FullName => Serializer.ReadOrientedRectangle(argument),
			_ when typeName == typeof(Rectangle).FullName => Serializer.ReadRectangle(argument),
			_ when typeName == typeof(Triangle2D).FullName => Serializer.ReadTriangle2D(argument),

			_ when typeName == typeof(Aabb).FullName => Serializer.ReadAabb(argument),
			_ when typeName == typeof(ConeFrustum).FullName => Serializer.ReadConeFrustum(argument),
			_ when typeName == typeof(Cylinder).FullName => Serializer.ReadCylinder(argument),
			_ when typeName == typeof(LineSegment3D).FullName => Serializer.ReadLineSegment3D(argument),
			_ when typeName == typeof(Obb).FullName => Serializer.ReadObb(argument),
			_ when typeName == typeof(OrientedPyramid).FullName => Serializer.ReadOrientedPyramid(argument),
			_ when typeName == typeof(Plane).FullName => Serializer.ReadPlane(argument),
			_ when typeName == typeof(Pyramid).FullName => Serializer.ReadPyramid(argument),
			_ when typeName == typeof(Ray).FullName => Serializer.ReadRay(argument),
			_ when typeName == typeof(Sphere).FullName => Serializer.ReadSphere(argument),
			_ when typeName == typeof(SphereCast).FullName => Serializer.ReadSphereCast(argument),
			_ when typeName == typeof(Triangle3D).FullName => Serializer.ReadTriangle3D(argument),

			_ when typeName == typeof(IntersectionResult).FullName => Serializer.ReadIntersectionResult(argument),

			_ => throw new NotSupportedException($"The type {typeName} is not supported."),
		};
	}

	[SuppressMessage("Minor Code Smell", "S4136:Method overloads should be grouped together", Justification = "Don't care")]
	private static class Serializer
	{
		private const char _separator = ' ';

		public static string Write(bool value)
		{
			return value.ToString();
		}

		public static bool ReadBool(string value)
		{
			return bool.Parse(value);
		}

		public static string Write(float value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		public static float ReadFloat(string value)
		{
			return float.Parse(value, CultureInfo.InvariantCulture);
		}

		public static string Write(Circle value)
		{
			return $"{Write(value.Center)}{_separator}{value.Radius}";
		}

		public static Circle ReadCircle(string value)
		{
			string[] parts = value.Split(_separator);
			return new Circle(ReadVector2(parts[0], parts[1]), ReadFloat(parts[2]));
		}

		public static string Write(CircleCast value)
		{
			return $"{Write(value.Start)}{_separator}{Write(value.End)}{_separator}{value.Radius}";
		}

		public static CircleCast ReadCircleCast(string value)
		{
			string[] parts = value.Split(_separator);
			return new CircleCast(ReadVector2(parts[0], parts[1]), ReadVector2(parts[2], parts[3]), ReadFloat(parts[4]));
		}

		public static string Write(LineSegment2D value)
		{
			return $"{Write(value.Start)}{_separator}{Write(value.End)}";
		}

		public static LineSegment2D ReadLineSegment2D(string value)
		{
			string[] parts = value.Split(_separator);
			return new LineSegment2D(ReadVector2(parts[0], parts[1]), ReadVector2(parts[2], parts[3]));
		}

		public static string Write(OrientedRectangle value)
		{
			return $"{Write(value.Center)}{_separator}{Write(value.HalfExtents)}{_separator}{value.RotationInRadians}";
		}

		public static OrientedRectangle ReadOrientedRectangle(string value)
		{
			string[] parts = value.Split(_separator);
			return new OrientedRectangle(ReadVector2(parts[0], parts[1]), ReadVector2(parts[2], parts[3]), ReadFloat(parts[4]));
		}

		public static string Write(Rectangle value)
		{
			return $"{Write(value.Center)}{_separator}{Write(value.Size)}";
		}

		public static Rectangle ReadRectangle(string value)
		{
			string[] parts = value.Split(_separator);
			return Rectangle.FromCenter(ReadVector2(parts[0], parts[1]), ReadVector2(parts[2], parts[3]));
		}

		public static string Write(Triangle2D value)
		{
			return $"{Write(value.A)}{_separator}{Write(value.B)}{_separator}{Write(value.C)}";
		}

		public static Triangle2D ReadTriangle2D(string value)
		{
			string[] parts = value.Split(_separator);
			return new Triangle2D(ReadVector2(parts[0], parts[1]), ReadVector2(parts[2], parts[3]), ReadVector2(parts[4], parts[5]));
		}

		public static string Write(Aabb value)
		{
			return $"{Write(value.Center)}{_separator}{Write(value.Size)}";
		}

		public static Aabb ReadAabb(string value)
		{
			string[] parts = value.Split(_separator);
			return new Aabb(ReadVector3(parts[0], parts[1], parts[2]), ReadVector3(parts[3], parts[4], parts[5]));
		}

		public static string Write(ConeFrustum value)
		{
			return $"{Write(value.BottomCenter)}{_separator}{value.BottomRadius}{_separator}{value.TopRadius}{_separator}{value.Height}";
		}

		public static ConeFrustum ReadConeFrustum(string value)
		{
			string[] parts = value.Split(_separator);
			return new ConeFrustum(ReadVector3(parts[0], parts[1], parts[2]), ReadFloat(parts[3]), ReadFloat(parts[4]), ReadFloat(parts[5]));
		}

		public static string Write(Cylinder value)
		{
			return $"{Write(value.BottomCenter)}{_separator}{value.Radius}{_separator}{value.Height}";
		}

		public static Cylinder ReadCylinder(string value)
		{
			string[] parts = value.Split(_separator);
			return new Cylinder(ReadVector3(parts[0], parts[1], parts[2]), ReadFloat(parts[3]), ReadFloat(parts[4]));
		}

		public static string Write(LineSegment3D value)
		{
			return $"{Write(value.Start)}{_separator}{Write(value.End)}";
		}

		public static LineSegment3D ReadLineSegment3D(string value)
		{
			string[] parts = value.Split(_separator);
			return new LineSegment3D(ReadVector3(parts[0], parts[1], parts[2]), ReadVector3(parts[3], parts[4], parts[5]));
		}

		public static string Write(Obb value)
		{
			return $"{Write(value.Center)}{_separator}{Write(value.HalfExtents)}{_separator}{Write(value.Orientation)}";
		}

		public static Obb ReadObb(string value)
		{
			string[] parts = value.Split(_separator);
			return new Obb(ReadVector3(parts[0], parts[1], parts[2]), ReadVector3(parts[3], parts[4], parts[5]), ReadMatrix3(parts[6..]));
		}

		public static string Write(OrientedPyramid value)
		{
			return $"{Write(value.Center)}{_separator}{Write(value.Size)}{_separator}{Write(value.Orientation)}";
		}

		public static OrientedPyramid ReadOrientedPyramid(string value)
		{
			string[] parts = value.Split(_separator);
			return new OrientedPyramid(ReadVector3(parts[0], parts[1], parts[2]), ReadVector3(parts[3], parts[4], parts[5]), ReadMatrix3(parts[6..]));
		}

		public static string Write(Plane value)
		{
			return $"{Write(value.Normal)}{_separator}{value.D}";
		}

		public static Plane ReadPlane(string value)
		{
			string[] parts = value.Split(_separator);
			return new Plane(ReadVector3(parts[0], parts[1], parts[2]), ReadFloat(parts[3]));
		}

		public static string Write(Pyramid value)
		{
			return $"{Write(value.Center)}{_separator}{Write(value.Size)}";
		}

		public static Pyramid ReadPyramid(string value)
		{
			string[] parts = value.Split(_separator);
			return new Pyramid(ReadVector3(parts[0], parts[1], parts[2]), ReadVector3(parts[3], parts[4], parts[5]));
		}

		public static string Write(Ray value)
		{
			return $"{Write(value.Origin)}{_separator}{Write(value.Direction)}";
		}

		public static Ray ReadRay(string value)
		{
			string[] parts = value.Split(_separator);
			return new Ray(ReadVector3(parts[0], parts[1], parts[2]), ReadVector3(parts[3], parts[4], parts[5]));
		}

		public static string Write(Sphere value)
		{
			return $"{Write(value.Center)}{_separator}{value.Radius}";
		}

		public static Sphere ReadSphere(string value)
		{
			string[] parts = value.Split(_separator);
			return new Sphere(ReadVector3(parts[0], parts[1], parts[2]), ReadFloat(parts[3]));
		}

		public static string Write(SphereCast value)
		{
			return $"{Write(value.Start)}{_separator}{Write(value.End)}{_separator}{value.Radius}";
		}

		public static SphereCast ReadSphereCast(string value)
		{
			string[] parts = value.Split(_separator);
			return new SphereCast(ReadVector3(parts[0], parts[1], parts[2]), ReadVector3(parts[3], parts[4], parts[5]), ReadFloat(parts[6]));
		}

		public static string Write(Triangle3D value)
		{
			return $"{Write(value.A)}{_separator}{Write(value.B)}{_separator}{Write(value.C)}";
		}

		public static Triangle3D ReadTriangle3D(string value)
		{
			string[] parts = value.Split(_separator);
			return new Triangle3D(ReadVector3(parts[0], parts[1], parts[2]), ReadVector3(parts[3], parts[4], parts[5]), ReadVector3(parts[6], parts[7], parts[8]));
		}

		public static string Write(IntersectionResult intersectionResult)
		{
			return $"{Write(intersectionResult.Normal)}{_separator}{Write(intersectionResult.IntersectionPoint)}";
		}

		public static IntersectionResult ReadIntersectionResult(string value)
		{
			string[] parts = value.Split(_separator);
			return new IntersectionResult(ReadVector3(parts[0], parts[1], parts[2]), ReadVector3(parts[3], parts[4], parts[5]));
		}

		public static string Write(Vector2 value)
		{
			return $"{value.X}{_separator}{value.Y}";
		}

		public static Vector2 ReadVector2(string value)
		{
			string[] parts = value.Split(_separator);
			return new Vector2(float.Parse(parts[0], CultureInfo.InvariantCulture), float.Parse(parts[1], CultureInfo.InvariantCulture));
		}

		public static Vector2 ReadVector2(string x, string y)
		{
			return new Vector2(float.Parse(x, CultureInfo.InvariantCulture), float.Parse(y, CultureInfo.InvariantCulture));
		}

		public static string Write(Vector3 value)
		{
			return $"{value.X}{_separator}{value.Y}{_separator}{value.Z}";
		}

		public static Vector3 ReadVector3(string value)
		{
			string[] parts = value.Split(_separator);
			return new Vector3(float.Parse(parts[0], CultureInfo.InvariantCulture), float.Parse(parts[1], CultureInfo.InvariantCulture), float.Parse(parts[2], CultureInfo.InvariantCulture));
		}

		public static Vector3 ReadVector3(string x, string y, string z)
		{
			return new Vector3(float.Parse(x, CultureInfo.InvariantCulture), float.Parse(y, CultureInfo.InvariantCulture), float.Parse(z, CultureInfo.InvariantCulture));
		}

		private static string Write(Matrix3 value)
		{
			return $"{value.M11}{_separator}{value.M12}{_separator}{value.M13}{_separator}{value.M21}{_separator}{value.M22}{_separator}{value.M23}{_separator}{value.M31}{_separator}{value.M32}{_separator}{value.M33}";
		}

		private static Matrix3 ReadMatrix3(string[] values)
		{
			return new Matrix3(
				float.Parse(values[0], CultureInfo.InvariantCulture),
				float.Parse(values[1], CultureInfo.InvariantCulture),
				float.Parse(values[2], CultureInfo.InvariantCulture),
				float.Parse(values[3], CultureInfo.InvariantCulture),
				float.Parse(values[4], CultureInfo.InvariantCulture),
				float.Parse(values[5], CultureInfo.InvariantCulture),
				float.Parse(values[6], CultureInfo.InvariantCulture),
				float.Parse(values[7], CultureInfo.InvariantCulture),
				float.Parse(values[8], CultureInfo.InvariantCulture));
		}
	}
}
