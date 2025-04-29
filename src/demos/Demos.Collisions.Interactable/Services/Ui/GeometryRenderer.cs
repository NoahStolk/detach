using Demos.Collisions.Interactable.Services.States;
using Demos.Collisions.Interactable.Shaders;
using Demos.Collisions.Interactable.Utils;
using Detach.Buffers;
using Detach.Collisions.Primitives2D;
using Detach.Collisions.Primitives3D;
using Detach.GlExtensions;
using Detach.Utils;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Demos.Collisions.Interactable.Services.Ui;

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

		foreach (object arg in _collisionAlgorithmState.Arguments)
			RenderShape(lineProgram, arg);
	}

	private void RenderShape(CachedProgram lineProgram, object? arg)
	{
		const float y2dOffset = 0.01f; // Prevent Z-fighting in 2D.

		switch (arg)
		{
			// 2D
			case Vector2 point2D:
				_gl.BindVertexArray(_sphereVao);
				_gl.LineWidth(8);
				RenderSphere(lineProgram, new Sphere(new Vector3(point2D.X, y2dOffset, point2D.Y), 0.02f));
				_gl.LineWidth(1);
				break;
			case Circle circle:
				_gl.BindVertexArray(_centeredLineVao);
				RenderCircle(lineProgram, new Vector3(circle.Center.X, y2dOffset, circle.Center.Y), circle.Radius, 16);
				break;
			case CircleCast circleCast:
				RenderCircleCast(lineProgram, circleCast);
				break;
			case LineSegment2D lineSegment2D:
				_gl.BindVertexArray(_centeredLineVao);
				RenderLine(lineProgram, new LineSegment3D(new Vector3(lineSegment2D.Start.X, y2dOffset, lineSegment2D.Start.Y), new Vector3(lineSegment2D.End.X, y2dOffset, lineSegment2D.End.Y)));
				break;
			case OrientedRectangle orientedRectangle:
				_gl.BindVertexArray(_centeredLineVao);
				Buffer4<Vector2> vertices = orientedRectangle.GetVertices();
				RenderLine(lineProgram, new LineSegment3D(new Vector3(vertices[0].X, y2dOffset, vertices[0].Y), new Vector3(vertices[1].X, y2dOffset, vertices[1].Y)));
				RenderLine(lineProgram, new LineSegment3D(new Vector3(vertices[1].X, y2dOffset, vertices[1].Y), new Vector3(vertices[2].X, y2dOffset, vertices[2].Y)));
				RenderLine(lineProgram, new LineSegment3D(new Vector3(vertices[2].X, y2dOffset, vertices[2].Y), new Vector3(vertices[3].X, y2dOffset, vertices[3].Y)));
				RenderLine(lineProgram, new LineSegment3D(new Vector3(vertices[3].X, y2dOffset, vertices[3].Y), new Vector3(vertices[0].X, y2dOffset, vertices[0].Y)));
				break;
			case Rectangle rectangle:
				_gl.BindVertexArray(_centeredLineVao);

				Vector2 min = rectangle.GetMin();
				Vector2 max = rectangle.GetMax();

				Vector3 topLeft = new(min.X, y2dOffset, min.Y);
				Vector3 bottomRight = new(max.X, y2dOffset, max.Y);
				Vector3 topRight = new(max.X, y2dOffset, min.Y);
				Vector3 bottomLeft = new(min.X, y2dOffset, max.Y);

				RenderLine(lineProgram, new LineSegment3D(topLeft, topRight));
				RenderLine(lineProgram, new LineSegment3D(topRight, bottomRight));
				RenderLine(lineProgram, new LineSegment3D(bottomRight, bottomLeft));
				RenderLine(lineProgram, new LineSegment3D(bottomLeft, topLeft));
				break;
			case Triangle2D triangle2D:
				_gl.BindVertexArray(_centeredLineVao);
				RenderLine(lineProgram, new LineSegment3D(new Vector3(triangle2D.A.X, y2dOffset, triangle2D.A.Y), new Vector3(triangle2D.B.X, y2dOffset, triangle2D.B.Y)));
				RenderLine(lineProgram, new LineSegment3D(new Vector3(triangle2D.B.X, y2dOffset, triangle2D.B.Y), new Vector3(triangle2D.C.X, y2dOffset, triangle2D.C.Y)));
				RenderLine(lineProgram, new LineSegment3D(new Vector3(triangle2D.C.X, y2dOffset, triangle2D.C.Y), new Vector3(triangle2D.A.X, y2dOffset, triangle2D.A.Y)));
				break;

			// 3D
			case Vector3 point:
				_gl.BindVertexArray(_sphereVao);
				_gl.LineWidth(8);
				RenderSphere(lineProgram, new Sphere(point, 0.02f));
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
			case OrientedPyramid orientedPyramid:
				RenderPyramid(lineProgram, orientedPyramid.BaseVertices, orientedPyramid.ApexVertex);
				break;
			case Plane plane:
				Vector3 pointOnPlane = plane.Normal * plane.D;

				_gl.BindVertexArray(_centeredLineVao);
				RenderLine(lineProgram, new LineSegment3D(pointOnPlane, pointOnPlane + plane.Normal));

				Vector3 cross = Vector3.Cross(plane.Normal, Vector3.UnitX);
				if (cross.LengthSquared() < 0.01f)
					cross = Vector3.Cross(plane.Normal, Vector3.UnitY);

				Vector3 right = Vector3.Normalize(Vector3.Cross(plane.Normal, cross));
				RenderLine(lineProgram, new LineSegment3D(pointOnPlane - cross, pointOnPlane + cross));
				RenderLine(lineProgram, new LineSegment3D(pointOnPlane - right, pointOnPlane + right));

				_gl.BindVertexArray(_sphereVao);
				RenderSphere(lineProgram, new Sphere(pointOnPlane, 0.02f));
				RenderSphere(lineProgram, new Sphere(pointOnPlane + plane.Normal, 0.02f));
				break;
			case Pyramid pyramid:
				RenderPyramid(lineProgram, pyramid.BaseVertices, pyramid.ApexVertex);
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

				Quaternion rotation = QuaternionUtils.CreateFromRotationBetween(Vector3.UnitZ, Vector3.Normalize(sphereCast.End - sphereCast.Start));
				Matrix4x4 orientationMatrix = Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, MathF.PI * 0.5f) * Matrix4x4.CreateFromQuaternion(rotation);

				const int lineSegments = 8;
				for (int i = 0; i < lineSegments; i++)
				{
					float angle = float.DegreesToRadians(i / (float)lineSegments * 360);
					Vector3 offset = Vector3.Transform(new Vector3(MathF.Sin(angle), 0, MathF.Cos(angle)) * sphereCast.Radius, orientationMatrix);
					Vector3 start = sphereCast.Start + offset;
					Vector3 end = sphereCast.End + offset;
					RenderLine(lineProgram, new LineSegment3D(start, end));
				}

				_gl.BindVertexArray(_sphereVao);

				_gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), Matrix4x4.CreateScale(sphereCast.Radius) * orientationMatrix * Matrix4x4.CreateTranslation(sphereCast.Start));
				_gl.DrawArrays(PrimitiveType.Lines, 0, (uint)_sphereVertices.Length);

				_gl.UniformMatrix4x4(lineProgram.GetUniformLocation("model"), Matrix4x4.CreateScale(sphereCast.Radius) * orientationMatrix * Matrix4x4.CreateTranslation(sphereCast.End));
				_gl.DrawArrays(PrimitiveType.Lines, 0, (uint)_sphereVertices.Length);
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

	private void RenderPyramid(CachedProgram lineProgram, Buffer4<Vector3> baseVertices, Vector3 apexVertex)
	{
		_gl.BindVertexArray(_centeredLineVao);

		RenderLine(lineProgram, new LineSegment3D(baseVertices[0], baseVertices[1]));
		RenderLine(lineProgram, new LineSegment3D(baseVertices[1], baseVertices[2]));
		RenderLine(lineProgram, new LineSegment3D(baseVertices[2], baseVertices[3]));
		RenderLine(lineProgram, new LineSegment3D(baseVertices[3], baseVertices[0]));

		RenderLine(lineProgram, new LineSegment3D(apexVertex, baseVertices[0]));
		RenderLine(lineProgram, new LineSegment3D(apexVertex, baseVertices[1]));
		RenderLine(lineProgram, new LineSegment3D(apexVertex, baseVertices[2]));
		RenderLine(lineProgram, new LineSegment3D(apexVertex, baseVertices[3]));
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

	private void RenderCircleCast(CachedProgram lineProgram, CircleCast circleCast)
	{
		Vector3 start = new(circleCast.Start.X, 0, circleCast.Start.Y);
		Vector3 end = new(circleCast.End.X, 0, circleCast.End.Y);

		_gl.BindVertexArray(_centeredLineVao);
		RenderCircle(lineProgram, start, circleCast.Radius, 16);
		RenderCircle(lineProgram, end, circleCast.Radius, 16);

		Vector2 direction = Vector2.Normalize(circleCast.End - circleCast.Start);
		Vector2 rotated = VectorUtils.RotateVector(direction, MathF.PI * 0.5f) * circleCast.Radius;
		Vector3 rotated3D = new(rotated.X, 0, rotated.Y);
		RenderLine(lineProgram, new LineSegment3D(start + rotated3D, end + rotated3D));
		RenderLine(lineProgram, new LineSegment3D(start - rotated3D, end - rotated3D));
	}

	private static Vector3 GetCirclePoint(Vector3 center, float radius, int segments, int index)
	{
		float singleSegmentAngle = MathF.PI * 2 / segments;
		float angle = singleSegmentAngle * index;
		return center + new Vector3(MathF.Cos(angle), 0, MathF.Sin(angle)) * radius;
	}
}
