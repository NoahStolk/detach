using Silk.NET.OpenGL;
using System.Numerics;

namespace Demos.Collisions.Interactable;

internal sealed class Framebuffer(GL gl)
{
	private uint _framebufferId;
	private Vector2 _cachedFramebufferSize;
	private int _originalViewportX;
	private int _originalViewportY;
	private int _originalViewportWidth;
	private int _originalViewportHeight;

	public uint TextureId { get; private set; }

	public unsafe void Initialize(Vector2 framebufferSize)
	{
		if (framebufferSize.X < 1)
			framebufferSize.X = 1;
		if (framebufferSize.Y < 1)
			framebufferSize.Y = 1;

		if (_cachedFramebufferSize == framebufferSize)
			return;

		if (_framebufferId != 0)
			gl.DeleteFramebuffer(_framebufferId);

		if (TextureId != 0)
			gl.DeleteTexture(TextureId);

		_framebufferId = gl.GenFramebuffer();
		gl.BindFramebuffer(FramebufferTarget.Framebuffer, _framebufferId);

		TextureId = gl.GenTexture();
		gl.BindTexture(TextureTarget.Texture2D, TextureId);
		gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb, (uint)framebufferSize.X, (uint)framebufferSize.Y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, null);

#pragma warning disable CS9193
		gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
		gl.TexParameterI(TextureTarget.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Linear);
#pragma warning restore CS9193

		gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, TextureId, 0);

		uint rbo = gl.GenRenderbuffer();
		gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);

		gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.DepthComponent24, (uint)framebufferSize.X, (uint)framebufferSize.Y);
		gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, rbo);

		GLEnum framebufferStatus = gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
		if (framebufferStatus != GLEnum.FramebufferComplete)
		{
			GLEnum error = gl.GetError();
			throw new InvalidOperationException($"Framebuffer for scene is not complete. Framebuffer status: {framebufferStatus} Last error: {error}");
		}

		gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
		gl.DeleteRenderbuffer(rbo);

		_cachedFramebufferSize = framebufferSize;
	}

	public void Bind()
	{
		gl.BindFramebuffer(FramebufferTarget.Framebuffer, _framebufferId);

		Span<int> originalViewport = stackalloc int[4];
		gl.GetInteger(GLEnum.Viewport, originalViewport);
		_originalViewportX = originalViewport[0];
		_originalViewportY = originalViewport[1];
		_originalViewportWidth = originalViewport[2];
		_originalViewportHeight = originalViewport[3];

		gl.Viewport(0, 0, (uint)_cachedFramebufferSize.X, (uint)_cachedFramebufferSize.Y);
	}

	public void Unbind()
	{
		gl.Viewport(_originalViewportX, _originalViewportY, (uint)_originalViewportWidth, (uint)_originalViewportHeight);
		gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
	}
}
