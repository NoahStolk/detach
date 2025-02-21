using Demos.Collisions.Interactable.Utils;
using Silk.NET.OpenGL;

namespace Demos.Collisions.Interactable.Shaders;

internal sealed class LazyProgramContainer(GL gl)
{
	private readonly Dictionary<string, CachedProgram> _programs = new();

	public CachedProgram GetProgram(string programName)
	{
		if (_programs.TryGetValue(programName, out CachedProgram? cachedProgram))
			return cachedProgram;

		cachedProgram = LoadFromDisk(programName);
		_programs.Add(programName, cachedProgram);
		return cachedProgram;
	}

	private CachedProgram LoadFromDisk(string programName)
	{
		string vertPath = Path.ChangeExtension(Path.Combine(AssemblyUtils.GetExecutableDirectory(), "Content", "Shaders", programName), ".vert");
		string fragPath = Path.ChangeExtension(vertPath, ".frag");
		if (!File.Exists(vertPath))
			throw new FileNotFoundException($"Vertex shader '{programName}' not found at '{vertPath}' or '{fragPath}'.");
		if (!File.Exists(fragPath))
			throw new FileNotFoundException($"Fragment shader '{programName}' not found at '{fragPath}' or '{vertPath}'.");

		string vertexCode = File.ReadAllText(vertPath);
		string fragmentCode = File.ReadAllText(fragPath);
		uint programId = LoadProgram(vertexCode, fragmentCode);
		return new CachedProgram(gl, programName, programId);
	}

	private uint LoadProgram(string vertexCode, string fragmentCode)
	{
		vertexCode = $"#version 330 core\n{vertexCode}";
		fragmentCode = $"#version 330 core\n{fragmentCode}";

		uint vs = gl.CreateShader(ShaderType.VertexShader);
		gl.ShaderSource(vs, vertexCode);
		gl.CompileShader(vs);
		CheckShaderStatus(ShaderType.VertexShader, vs);

		uint fs = gl.CreateShader(ShaderType.FragmentShader);
		gl.ShaderSource(fs, fragmentCode);
		gl.CompileShader(fs);
		CheckShaderStatus(ShaderType.FragmentShader, fs);

		uint programId = gl.CreateProgram();

		gl.AttachShader(programId, vs);
		gl.AttachShader(programId, fs);

		gl.LinkProgram(programId);

		gl.DetachShader(programId, vs);
		gl.DetachShader(programId, fs);

		gl.DeleteShader(vs);
		gl.DeleteShader(fs);

		return programId;

		void CheckShaderStatus(ShaderType shaderType, uint shaderId)
		{
			string infoLog = gl.GetShaderInfoLog(shaderId);
			if (!string.IsNullOrWhiteSpace(infoLog))
				throw new InvalidOperationException($"{shaderType} compile error: {infoLog}");
		}
	}
}
