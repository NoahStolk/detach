using Hexa.NET.ImGui;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Demos.Collisions.Interactable.Services.Ui;

internal sealed class SceneWindow(GL gl, SceneRenderer sceneRenderer, Camera camera)
{
	private const float _nearPlaneDistance = 0.05f;
	private const float _farPlaneDistance = 10_000f;

	private readonly Framebuffer _framebuffer = new(gl);

	public void Render(float frameTime)
	{
		if (ImGui.Begin("Scene"))
		{
			Vector2 windowSize = ImGui.GetWindowSize();

			float aspectRatio = windowSize.X / windowSize.Y;
			Matrix4x4 viewMatrix = Matrix4x4.CreateLookAt(camera.Position, camera.Target, Vector3.UnitY);
			Matrix4x4 projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(float.DegreesToRadians(camera.FieldOfView), aspectRatio, _nearPlaneDistance, _farPlaneDistance);

			Vector2 framebufferSize = ImGui.GetContentRegionAvail();
			_framebuffer.Initialize(framebufferSize);
			_framebuffer.Bind();
			sceneRenderer.Render(viewMatrix, projectionMatrix);
			_framebuffer.Unbind();

			ImDrawListPtr drawList = ImGui.GetWindowDrawList();
			Vector2 cursorScreenPos = ImGui.GetCursorScreenPos();
			drawList.AddImage(_framebuffer.TextureId, cursorScreenPos, cursorScreenPos + framebufferSize, Vector2.UnitY, Vector2.UnitX);

			if (ImGui.IsMouseHoveringRect(cursorScreenPos, cursorScreenPos + framebufferSize))
			{
				camera.Update(frameTime, !ImGui.IsAnyItemHovered());
			}
		}

		ImGui.End();
	}
}
