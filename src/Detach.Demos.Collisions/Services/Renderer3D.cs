using Silk.NET.OpenGL;
using System.Numerics;

namespace Detach.Demos.Collisions.Services;

public sealed class Renderer3D
{
	private readonly GL _gl;

	private Vector2 _cachedFramebufferSize;
	private uint _framebufferId;

	public Renderer3D(GL gl)
	{
		_gl = gl;
	}

	public uint FramebufferTextureId { get; private set; }

	public unsafe void Initialize(Vector2 framebufferSize)
	{
		if (_cachedFramebufferSize == framebufferSize)
			return;

		if (_framebufferId != 0)
			_gl.DeleteFramebuffer(_framebufferId);

		if (FramebufferTextureId != 0)
			_gl.DeleteTexture(FramebufferTextureId);

		_framebufferId = _gl.GenFramebuffer();
		_gl.BindFramebuffer(FramebufferTarget.Framebuffer, _framebufferId);

		FramebufferTextureId = _gl.GenTexture();
		_gl.BindTexture(TextureTarget.Texture2D, FramebufferTextureId);
		_gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb, (uint)framebufferSize.X, (uint)framebufferSize.Y, 0, PixelFormat.Rgb, PixelType.UnsignedByte, null);
		_gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
		_gl.TexParameterI(TextureTarget.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Linear);
		_gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, FramebufferTextureId, 0);

		uint rbo = _gl.GenRenderbuffer();
		_gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);

		_gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.DepthComponent24, (uint)framebufferSize.X, (uint)framebufferSize.Y);
		_gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, rbo);

		if (_gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != GLEnum.FramebufferComplete)
			throw new InvalidOperationException("Framebuffer for scene is not complete.");

		_gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
		_gl.DeleteRenderbuffer(rbo);

		_cachedFramebufferSize = framebufferSize;
	}

	public unsafe void RenderFramebuffer(Vector2 size)
	{
		_gl.BindFramebuffer(FramebufferTarget.Framebuffer, _framebufferId);

		// Keep track of the original viewport, so we can restore it later.
		Span<int> originalViewport = stackalloc int[4];
		_gl.GetInteger(GLEnum.Viewport, originalViewport);
		_gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);

		_gl.ClearColor(0.3f, 0.3f, 0.3f, 0);
		_gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		_gl.Enable(EnableCap.DepthTest);
		_gl.Enable(EnableCap.Blend);
		_gl.Enable(EnableCap.CullFace);
		_gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		//SceneRenderer.RenderScene();

		_gl.Viewport(originalViewport[0], originalViewport[1], (uint)originalViewport[2], (uint)originalViewport[3]);
		_gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
	}
}
