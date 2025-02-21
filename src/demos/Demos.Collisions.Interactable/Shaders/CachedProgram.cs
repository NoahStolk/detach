using Silk.NET.OpenGL;

namespace Demos.Collisions.Interactable.Shaders;

internal sealed class CachedProgram(GL gl, string programName, uint programId)
{
	private readonly Dictionary<string, int> _uniformLocations = new();

	public uint ProgramId { get; } = programId;

	public int GetUniformLocation(string uniformName)
	{
		if (_uniformLocations.TryGetValue(uniformName, out int location))
			return location;

		location = gl.GetUniformLocation(ProgramId, uniformName);
		if (location == -1)
			throw new InvalidOperationException($"Could not find uniform location for '{uniformName}' in program '{programName}'.");

		_uniformLocations.Add(uniformName, location);

		return location;
	}
}
