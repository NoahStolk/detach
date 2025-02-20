using Silk.NET.OpenGL;
using System.Numerics;

namespace Demos.Collisions3D.Utils;

internal static class VaoUtils
{
	public static unsafe uint CreateLineVao(GL gl, Vector3[] vertices)
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
}
