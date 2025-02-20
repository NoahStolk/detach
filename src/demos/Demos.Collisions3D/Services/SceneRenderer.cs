﻿using Demos.Collisions3D.Extensions;
using Demos.Collisions3D.Shaders;
using Demos.Collisions3D.Utils;
using Detach.Buffers;
using Detach.Collisions.Primitives3D;
using Detach.GlExtensions;
using Detach.Numerics;
using Detach.Utils;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using System.Numerics;
using System.Reflection;

namespace Demos.Collisions3D.Services;

internal sealed unsafe class SceneRenderer
{
	private const float _nearPlaneDistance = 0.05f;
	private const float _farPlaneDistance = 10_000f;

	private readonly WindowHandle* _window;
	private readonly Glfw _glfw;
	private readonly GL _gl;
	private readonly Camera _camera;
	private readonly LazyProgramContainer _lazyProgramContainer;
	private readonly CollisionAlgorithmState _collisionAlgorithmState;

	private readonly Vector3[] _centeredLineVertices = VertexUtils.GetCenteredLinePositions();
	private readonly Vector3[] _cubeVertices = VertexUtils.GetCubePositions();
	private readonly Vector3[] _sphereVertices = VertexUtils.GetSpherePositions(6, 8, 1);

	private readonly uint _lineVao;
	private readonly uint _centeredLineVao;
	private readonly uint _cubeVao;
	private readonly uint _sphereVao;

	private readonly Vector3 _clearColor = new(0, 0, 0);

	private Matrix4x4 _viewMatrix;
	private Matrix4x4 _projectionMatrix;

	public SceneRenderer(WindowHandle* window, Glfw glfw, GL gl, Camera camera, LazyProgramContainer lazyProgramContainer, CollisionAlgorithmState collisionAlgorithmState)
	{
		_window = window;
		_glfw = glfw;
		_gl = gl;
		_camera = camera;
		_lazyProgramContainer = lazyProgramContainer;
		_collisionAlgorithmState = collisionAlgorithmState;

		_lineVao = CreateLineVao(gl, [Vector3.Zero, Vector3.UnitZ]);
		_centeredLineVao = CreateLineVao(gl, _centeredLineVertices);
		_cubeVao = CreateLineVao(gl, _cubeVertices);
		_sphereVao = CreateLineVao(gl, _sphereVertices);
	}

	private static uint CreateLineVao(GL gl, Vector3[] vertices)
	{
		uint lineVao = gl.GenVertexArray();
		gl.BindVertexArray(lineVao);

		uint vbo = gl.GenBuffer();
		gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);

		fixed (Vector3* v = &vertices[0])
			gl.BufferData(BufferTargetARB.ArrayBuffer, (uint)(vertices.Length * sizeof(Vector3)), v, BufferUsageARB.StaticDraw);

		gl.EnableVertexAttribArray(0);
		gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vector3), (void*)0);

		gl.BindVertexArray(0);
		gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
		gl.DeleteBuffer(vbo);

		return lineVao;
	}

	public void Render(float dt)
	{
		_camera.Update(dt, true);

		_gl.ClearColor(_clearColor.X, _clearColor.Y, _clearColor.Z, 0);
		_gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		_gl.Enable(EnableCap.CullFace);
		_gl.Enable(EnableCap.DepthTest);

		_glfw.GetWindowSize(_window, out int width, out int height);

		float aspectRatio = width / (float)height;
		_viewMatrix = Matrix4x4.CreateLookAt(_camera.Position, _camera.Target, Vector3.UnitY);
		_projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathUtils.ToRadians(_camera.FieldOfView), aspectRatio, _nearPlaneDistance, _farPlaneDistance);

		bool collide = CheckCollisions();
		RenderGeometry(collide);
	}

	private bool CheckCollisions()
	{
		if (!_collisionAlgorithmState.StateIsValid)
			return false;

		MethodInfo method = _collisionAlgorithmState.SelectedAlgorithm.Method;

		if (method.ReturnType != typeof(bool))
			return false;

		return _collisionAlgorithmState.ExecuteAlgorithm<bool>();
	}

	private void RenderGeometry(bool collide)
	{
		const float fadeOutMinDistance = 10;
		const float fadeOutMaxDistance = 50;

		CachedProgram lineProgram = _lazyProgramContainer.GetProgram("line_editor");
		_gl.UseProgram(lineProgram.ProgramId);

		_gl.UniformMatrix4x4(lineProgram.GetUniformLocation("view"), _viewMatrix);
		_gl.UniformMatrix4x4(lineProgram.GetUniformLocation("projection"), _projectionMatrix);
		_gl.Uniform3(lineProgram.GetUniformLocation("cameraPosition"), _camera.Position);
		_gl.Uniform1(lineProgram.GetUniformLocation("fadeMinDistance"), fadeOutMinDistance);
		_gl.Uniform1(lineProgram.GetUniformLocation("fadeMaxDistance"), fadeOutMaxDistance);

		_gl.BindVertexArray(_lineVao);

		_gl.Uniform1(lineProgram.GetUniformLocation("fadeOut"), 0);
		RenderOrigin(lineProgram);
		RenderTarget(lineProgram, _camera.Target, 0.25f, new Vector4(0.6f, 0, 1, 1));

		_gl.Uniform1(lineProgram.GetUniformLocation("fadeOut"), 1);

		Vector3 lineColor = Rgb.Invert(Rgb.FromVector3(_clearColor));
		RenderGrid(lineProgram, Vector3.Zero, new Vector4(lineColor, 0.25f), fadeOutMaxDistance, 1);

		// Render shapes here.
		Vector4 collideColor = collide ? Vector4.One : Vector4.Zero;
		_gl.Uniform4(lineProgram.GetUniformLocation("color"), new Vector4(0.5f, 0.0f, 1, 1) + collideColor);

		foreach (object? arg in _collisionAlgorithmState.Arguments)
		{
			if (arg is Vector3 point)
			{
				_gl.BindVertexArray(_sphereVao);
				RenderSphere(lineProgram, new Sphere(point, 0.1f));
			}
			else if (arg is Aabb aabb)
			{
				_gl.BindVertexArray(_cubeVao);
				RenderAabb(lineProgram, aabb);
			}
			else if (arg is ConeFrustum coneFrustum)
			{
				RenderConeFrustum(lineProgram, coneFrustum);
			}
			else if (arg is Cylinder cylinder)
			{
				RenderCylinder(lineProgram, cylinder);
			}
			else if (arg is LineSegment3D lineSegment)
			{
				_gl.BindVertexArray(_centeredLineVao);
				RenderLine(lineProgram, lineSegment);
			}
			else if (arg is Obb obb)
			{
				_gl.BindVertexArray(_cubeVao);
				RenderObb(lineProgram, obb);
			}
			else if (arg is Pyramid pyramid)
			{
				_gl.BindVertexArray(_centeredLineVao);

				Buffer4<Vector3> vertices = pyramid.BaseVertices;
				RenderLine(lineProgram, new LineSegment3D(vertices[0], vertices[1]));
				RenderLine(lineProgram, new LineSegment3D(vertices[1], vertices[2]));
				RenderLine(lineProgram, new LineSegment3D(vertices[2], vertices[3]));
				RenderLine(lineProgram, new LineSegment3D(vertices[3], vertices[0]));

				RenderLine(lineProgram, new LineSegment3D(pyramid.ApexVertex, vertices[0]));
				RenderLine(lineProgram, new LineSegment3D(pyramid.ApexVertex, vertices[1]));
				RenderLine(lineProgram, new LineSegment3D(pyramid.ApexVertex, vertices[2]));
				RenderLine(lineProgram, new LineSegment3D(pyramid.ApexVertex, vertices[3]));
			}
			else if (arg is Ray ray)
			{
				_gl.BindVertexArray(_centeredLineVao);
				RenderLine(lineProgram, new LineSegment3D(ray.Origin, ray.Origin + ray.Direction * 1000));
			}
			else if (arg is Sphere sphere)
			{
				_gl.BindVertexArray(_sphereVao);
				RenderSphere(lineProgram, sphere);
			}
			else if (arg is SphereCast sphereCast)
			{
				_gl.BindVertexArray(_centeredLineVao);

				Vector3 offsetX = new(sphereCast.Radius, 0, 0);
				RenderLine(lineProgram, new LineSegment3D(sphereCast.Start + offsetX, sphereCast.End + offsetX));
				RenderLine(lineProgram, new LineSegment3D(sphereCast.Start - offsetX, sphereCast.End - offsetX));

				Vector3 offsetY = new(0, sphereCast.Radius, 0);
				RenderLine(lineProgram, new LineSegment3D(sphereCast.Start + offsetY, sphereCast.End + offsetY));
				RenderLine(lineProgram, new LineSegment3D(sphereCast.Start - offsetY, sphereCast.End - offsetY));

				Vector3 offsetZ = new(0, 0, sphereCast.Radius);
				RenderLine(lineProgram, new LineSegment3D(sphereCast.Start + offsetZ, sphereCast.End + offsetZ));
				RenderLine(lineProgram, new LineSegment3D(sphereCast.Start - offsetZ, sphereCast.End - offsetZ));

				_gl.BindVertexArray(_sphereVao);
				RenderSphere(lineProgram, new Sphere(sphereCast.Start, sphereCast.Radius));
				RenderSphere(lineProgram, new Sphere(sphereCast.End, sphereCast.Radius));
			}
			else if (arg is Triangle3D triangle3D)
			{
				_gl.BindVertexArray(_centeredLineVao);
				RenderLine(lineProgram, new LineSegment3D(triangle3D.A, triangle3D.B));
				RenderLine(lineProgram, new LineSegment3D(triangle3D.B, triangle3D.C));
				RenderLine(lineProgram, new LineSegment3D(triangle3D.C, triangle3D.A));
			}
		}
	}

	#region Shapes

	private void RenderLine(CachedProgram lineProgram, LineSegment3D line)
	{
		Vector3 center = (line.Start + line.End) / 2f;
		Quaternion rotation = QuaternionUtils.CreateFromRotationBetween(Vector3.UnitZ, Vector3.Normalize(line.Start - line.End));
		_gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), Matrix4x4.CreateScale(line.Length) * Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateTranslation(center));
		_gl.DrawArrays(PrimitiveType.Lines, 0, (uint)_centeredLineVertices.Length);
	}

	private void RenderObb(CachedProgram lineProgram, Obb obb)
	{
		_gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), Matrix4x4.CreateScale(obb.HalfExtents * 2) * obb.Orientation.ToMatrix4x4() * Matrix4x4.CreateTranslation(obb.Center));
		_gl.DrawArrays(PrimitiveType.Lines, 0, (uint)_cubeVertices.Length);
	}

	private void RenderAabb(CachedProgram lineProgram, Aabb aabb)
	{
		_gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), Matrix4x4.CreateScale(aabb.Size) * Matrix4x4.CreateTranslation(aabb.Center));
		_gl.DrawArrays(PrimitiveType.Lines, 0, (uint)_cubeVertices.Length);
	}

	private void RenderSphere(CachedProgram lineProgram, Sphere sphere)
	{
		_gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), Matrix4x4.CreateScale(sphere.Radius) * Matrix4x4.CreateTranslation(sphere.Center));
		_gl.DrawArrays(PrimitiveType.Lines, 0, (uint)_sphereVertices.Length);
	}

	private void RenderCylinder(in CachedProgram lineProgram, Cylinder cylinder)
	{
		RenderConeFrustum(lineProgram, cylinder.BottomCenter, cylinder.Radius, cylinder.Radius, cylinder.Height);
	}

	private void RenderConeFrustum(in CachedProgram lineProgram, ConeFrustum coneFrustum)
	{
		RenderConeFrustum(lineProgram, coneFrustum.BottomCenter, coneFrustum.BottomRadius, coneFrustum.TopRadius, coneFrustum.Height);
	}

	private void RenderConeFrustum(in CachedProgram lineProgram, Vector3 bottomCenter, float bottomRadius, float topRadius, float height)
	{
		const int circleSegments = 16;
		Vector3 topCenter = bottomCenter + new Vector3(0, height, 0);

		_gl.BindVertexArray(_centeredLineVao);
		RenderCircle(lineProgram, bottomCenter, bottomRadius, circleSegments);
		RenderCircle(lineProgram, topCenter, topRadius, circleSegments);

		for (int i = 0; i < circleSegments; i++)
		{
			Vector3 bottomStart = GetCirclePoint(bottomCenter, bottomRadius, circleSegments, i);
			Vector3 topStart = GetCirclePoint(topCenter, topRadius, circleSegments, i);
			RenderLine(lineProgram, new LineSegment3D(bottomStart, topStart));
		}
	}

	private void RenderCircle(in CachedProgram lineProgram, Vector3 center, float radius, int segments)
	{
		for (int i = 0; i < segments; i++)
		{
			Vector3 startOffset = GetCirclePoint(center, radius, segments, i);
			Vector3 endOffset = GetCirclePoint(center, radius, segments, (i + 1) % segments);
			RenderLine(lineProgram, new LineSegment3D(startOffset, endOffset));
		}
	}

	private static Vector3 GetCirclePoint(Vector3 center, float radius, int segments, int index)
	{
		float singleSegmentAngle = MathF.PI * 2 / segments;
		float angle = singleSegmentAngle * index;
		return center + new Vector3(MathF.Cos(angle), 0, MathF.Sin(angle)) * radius;
	}

	#endregion Shapes

	private void RenderTarget(CachedProgram lineProgram, Vector3 position, float size, Vector4 color)
	{
		float halfSize = size * 0.5f;
		Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(1, 1, size);

		_gl.LineWidth(4);
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateTranslation(position - new Vector3(0, 0, halfSize)), color);
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, MathF.PI / 2) * Matrix4x4.CreateTranslation(position - new Vector3(halfSize, 0, 0)), color);
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * 1.5f) * Matrix4x4.CreateTranslation(position - new Vector3(0, halfSize, 0)), color);
	}

	private void RenderLine(CachedProgram lineProgram, Matrix4x4 modelMatrix, Vector4 color)
	{
		_gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), modelMatrix);
		_gl.Uniform4(lineProgram.GetUniformLocation("color"), color);
		_gl.DrawArrays(PrimitiveType.Lines, 0, 2);
	}

	private void RenderOrigin(CachedProgram lineProgram)
	{
		Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(1, 1, 256);

		_gl.LineWidth(4);
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, MathF.PI / 2), new Vector4(1, 0, 0, 1));
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * 1.5f), new Vector4(0, 1, 0, 1));
		RenderLine(lineProgram, scaleMatrix, new Vector4(0, 0, 1, 1));

		_gl.LineWidth(2);
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, -MathF.PI / 2), new Vector4(1, 0, 0, 0.5f));
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, MathF.PI / 2), new Vector4(0, 1, 0, 0.5f));
		RenderLine(lineProgram, scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, MathF.PI), new Vector4(0, 0, 1, 0.5f));
	}

	private void RenderGrid(CachedProgram lineProgram, Vector3 origin, Vector4 color, float cellCount, int interval)
	{
		interval = Math.Max(1, interval);

		_gl.Uniform4(lineProgram.GetUniformLocation("color"), color);
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

				_gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), scaleMatrix * Matrix4x4.CreateTranslation(new Vector3(i, origin.Y, min) + offset));
				_gl.DrawArrays(PrimitiveType.Lines, 0, 2);
			}

			if (!origin.Y.IsZero() || !(i + offset.Z).IsZero())
			{
				UpdateLineWidth(i + (int)offset.Z);

				_gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), scaleMatrix * Matrix4x4.CreateFromAxisAngle(Vector3.UnitY, MathF.PI / 2) * Matrix4x4.CreateTranslation(new Vector3(min, origin.Y, i) + offset));
				_gl.DrawArrays(PrimitiveType.Lines, 0, 2);
			}
		}

		void UpdateLineWidth(int i)
		{
			int newLineWidth = i % interval == 0 ? 2 : 1;
			if (newLineWidth != lineWidthCache)
			{
				_gl.LineWidth(newLineWidth);
				lineWidthCache = newLineWidth;
			}
		}
	}
}
