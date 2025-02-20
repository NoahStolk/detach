using Demos.Collisions3D.Shaders;
using Demos.Collisions3D.Utils;
using Detach.Buffers;
using Detach.Collisions.Primitives3D;
using Detach.GlExtensions;
using Detach.Utils;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Demos.Collisions3D.Services.Ui;

internal sealed class GeometryRenderer
{
	private readonly Vector3[] _centeredLineVertices = VertexUtils.GetCenteredLinePositions();
	private readonly Vector3[] _cubeVertices = VertexUtils.GetCubePositions();
	private readonly Vector3[] _sphereVertices = VertexUtils.GetSpherePositions(6, 8, 1);

	private readonly uint _centeredLineVao;
	private readonly uint _cubeVao;
	private readonly uint _sphereVao;

	private readonly GL _gl;
	private readonly CollisionAlgorithmState _collisionAlgorithmState;

	public GeometryRenderer(GL gl, CollisionAlgorithmState collisionAlgorithmState)
	{
		_gl = gl;
		_collisionAlgorithmState = collisionAlgorithmState;
		_centeredLineVao = VaoUtils.CreateLineVao(gl, _centeredLineVertices);
		_cubeVao = VaoUtils.CreateLineVao(gl, _cubeVertices);
		_sphereVao = VaoUtils.CreateLineVao(gl, _sphereVertices);
	}

	public void RenderGeometry(CachedProgram lineProgram)
	{
		_gl.Uniform4(lineProgram.GetUniformLocation("color"), new Vector4(0.1f, 1, 0, 1));
		RenderShape(lineProgram, _collisionAlgorithmState.ReturnValue);

		Vector4 collideColor = _collisionAlgorithmState.ReturnValue is true ? Vector4.One : Vector4.Zero;
		_gl.Uniform4(lineProgram.GetUniformLocation("color"), new Vector4(0.5f, 0.0f, 1, 1) + collideColor);

		foreach (object? arg in _collisionAlgorithmState.Arguments)
			RenderShape(lineProgram, arg);
	}

	private void RenderShape(CachedProgram lineProgram, object? arg)
	{
		switch (arg)
		{
			case Vector3 point:
				_gl.BindVertexArray(_sphereVao);
				_gl.LineWidth(8);
				RenderSphere(lineProgram, new Sphere(point, 0.05f));
				_gl.LineWidth(1);
				break;
			case Aabb aabb:
				_gl.BindVertexArray(_cubeVao);
				RenderAabb(lineProgram, aabb);
				break;
			case ConeFrustum coneFrustum:
				RenderConeFrustum(lineProgram, coneFrustum);
				break;
			case Cylinder cylinder:
				RenderCylinder(lineProgram, cylinder);
				break;
			case LineSegment3D lineSegment:
				_gl.BindVertexArray(_centeredLineVao);
				RenderLine(lineProgram, lineSegment);
				break;
			case Obb obb:
				_gl.BindVertexArray(_cubeVao);
				RenderObb(lineProgram, obb);
				break;
			case Pyramid pyramid:
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
				break;
			case Ray ray:
				_gl.BindVertexArray(_centeredLineVao);
				RenderLine(lineProgram, new LineSegment3D(ray.Origin, ray.Origin + ray.Direction * 1000));
				break;
			case Sphere sphere:
				_gl.BindVertexArray(_sphereVao);
				RenderSphere(lineProgram, sphere);
				break;
			case SphereCast sphereCast:
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
				break;
			case Triangle3D triangle3D:
				_gl.BindVertexArray(_centeredLineVao);
				RenderLine(lineProgram, new LineSegment3D(triangle3D.A, triangle3D.B));
				RenderLine(lineProgram, new LineSegment3D(triangle3D.B, triangle3D.C));
				RenderLine(lineProgram, new LineSegment3D(triangle3D.C, triangle3D.A));
				break;
		}
	}

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
}
