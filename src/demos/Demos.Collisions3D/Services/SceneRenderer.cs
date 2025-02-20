using Demos.Collisions3D.Extensions;
using Demos.Collisions3D.Services.Ui;
using Demos.Collisions3D.Shaders;
using Demos.Collisions3D.Utils;
using Detach.GlExtensions;
using Detach.Numerics;
using Detach.Utils;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Demos.Collisions3D.Services;

internal sealed unsafe class SceneRenderer(
	WindowHandle* window,
	Glfw glfw,
	GL gl,
	Camera camera,
	LazyProgramContainer lazyProgramContainer,
	GeometryRenderer geometryRenderer)
{
	private const float _nearPlaneDistance = 0.05f;
	private const float _farPlaneDistance = 10_000f;

	private readonly uint _lineVao = VaoUtils.CreateLineVao(gl, [Vector3.Zero, Vector3.UnitZ]);

	private readonly Vector3 _clearColor = new(0, 0, 0);

	public void Render()
	{
		gl.ClearColor(_clearColor.X, _clearColor.Y, _clearColor.Z, 0);
		gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		gl.Enable(EnableCap.CullFace);
		gl.Enable(EnableCap.DepthTest);

		glfw.GetWindowSize(window, out int width, out int height);

		float aspectRatio = width / (float)height;
		Matrix4x4 viewMatrix = Matrix4x4.CreateLookAt(camera.Position, camera.Target, Vector3.UnitY);
		Matrix4x4 projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathUtils.ToRadians(camera.FieldOfView), aspectRatio, _nearPlaneDistance, _farPlaneDistance);

		const float fadeOutMinDistance = 10;
		const float fadeOutMaxDistance = 50;

		CachedProgram lineProgram = lazyProgramContainer.GetProgram("line_editor");
		gl.UseProgram(lineProgram.ProgramId);

		gl.UniformMatrix4x4(lineProgram.GetUniformLocation("view"), viewMatrix);
		gl.UniformMatrix4x4(lineProgram.GetUniformLocation("projection"), projectionMatrix);
		gl.Uniform3(lineProgram.GetUniformLocation("cameraPosition"), camera.Position);
		gl.Uniform1(lineProgram.GetUniformLocation("fadeMinDistance"), fadeOutMinDistance);
		gl.Uniform1(lineProgram.GetUniformLocation("fadeMaxDistance"), fadeOutMaxDistance);

		gl.BindVertexArray(_lineVao);

		gl.Uniform1(lineProgram.GetUniformLocation("fadeOut"), 0);
		RenderOrigin(lineProgram);
		RenderTarget(lineProgram, camera.Target, 0.25f, new Vector4(0.6f, 0, 1, 1));

		gl.Uniform1(lineProgram.GetUniformLocation("fadeOut"), 1);

		Vector3 lineColor = Rgb.Invert(Rgb.FromVector3(_clearColor));
		RenderGrid(lineProgram, Vector3.Zero, new Vector4(lineColor, 0.25f), fadeOutMaxDistance, 1);

		geometryRenderer.RenderGeometry(lineProgram);
	}

	private void RenderTarget(CachedProgram lineProgram, Vector3 position, float size, Vector4 color)
	{
		float halfSize = size * 0.5f;
		Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(1, 1, size);

		gl.LineWidth(4);
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateTranslation(position - new Vector3(0, 0, halfSize)), color);
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, MathF.PI / 2) * Matrix4x4.CreateTranslation(position - new Vector3(halfSize, 0, 0)), color);
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * 1.5f) * Matrix4x4.CreateTranslation(position - new Vector3(0, halfSize, 0)), color);
	}

	private void RenderLine(CachedProgram lineProgram, Matrix4x4 modelMatrix, Vector4 color)
	{
		gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), modelMatrix);
		gl.Uniform4(lineProgram.GetUniformLocation("color"), color);
		gl.DrawArrays(PrimitiveType.Lines, 0, 2);
	}

	private void RenderOrigin(CachedProgram lineProgram)
	{
		Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(1, 1, 256);

		gl.LineWidth(4);
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, MathF.PI / 2), new Vector4(1, 0, 0, 1));
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * 1.5f), new Vector4(0, 1, 0, 1));
		RenderLine(lineProgram, scaleMatrix, new Vector4(0, 0, 1, 1));

		gl.LineWidth(2);
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, -MathF.PI / 2), new Vector4(1, 0, 0, 0.5f));
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, MathF.PI / 2), new Vector4(0, 1, 0, 0.5f));
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, MathF.PI), new Vector4(0, 0, 1, 0.5f));
	}

	private void RenderGrid(CachedProgram lineProgram, Vector3 origin, Vector4 color, float cellCount, int interval)
	{
		interval = Math.Max(1, interval);

		gl.Uniform4(lineProgram.GetUniformLocation("color"), color);
		int lineWidthCache = 1; // Prevents unnecessary calls to Gl.LineWidth.

		int min = (int)-cellCount;
		int max = (int)cellCount;
		Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(new Vector3(1, 1, max - min));
		Vector3 offset = new(MathF.Round(origin.X), 0, MathF.Round(origin.Z));

		for (int i = min; i <= max; i++)
		{
			// Prevent rendering grid lines on top of origin lines (Z-fighting).
			if (!origin.Y.IsZero() || !(i + offset.X).IsZero())
			{
				UpdateLineWidth(i + (int)offset.X);

				gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), scaleMatrix * Matrix4x4.CreateTranslation(new Vector3(i, origin.Y, min) + offset));
				gl.DrawArrays(PrimitiveType.Lines, 0, 2);
			}

			if (!origin.Y.IsZero() || !(i + offset.Z).IsZero())
			{
				UpdateLineWidth(i + (int)offset.Z);

				gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, MathF.PI / 2) * Matrix4x4.CreateTranslation(new Vector3(min, origin.Y, i) + offset));
				gl.DrawArrays(PrimitiveType.Lines, 0, 2);
			}
		}

		void UpdateLineWidth(int i)
		{
			int newLineWidth = i % interval == 0 ? 2 : 1;
			if (newLineWidth != lineWidthCache)
			{
				gl.LineWidth(newLineWidth);
				lineWidthCache = newLineWidth;
			}
		}
	}
}
