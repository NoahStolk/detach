using System.Numerics;

namespace Detach.Parsers.Model;

/// <summary>
/// Note: this data is not complete yet.
/// </summary>
public record MaterialData(
	string Name,
	Vector3 AmbientColor,
	Vector3 DiffuseColor,
	Vector3 SpecularColor,
	Vector3 EmissiveCoefficient,
	float SpecularExponent,
	float OpticalDensity,
	float Alpha,
	string DiffuseMap);
