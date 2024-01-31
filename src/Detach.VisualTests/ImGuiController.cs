using Detach.VisualTests.Extensions;
using ImGuiNET;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Detach.VisualTests;

public static class ImGuiController
{
	private static readonly IReadOnlyList<Keys> _allKeys = (Keys[])Enum.GetValues(typeof(Keys));
	private static readonly IEnumerable<Keys> _controlKeys = new List<Keys>
	{
		Keys.Left,
		Keys.Right,
		Keys.Up,
		Keys.Down,
		Keys.Home,
		Keys.End,
		Keys.PageUp,
		Keys.PageDown,
		Keys.Delete,
		Keys.Backspace,
		Keys.Enter,
		Keys.Escape,
		Keys.Tab,
	};

	private static readonly List<char> _pressedChars = [];
	private static readonly List<Keys> _pressedControlKeys = [];

	private static uint _vbo;
	private static uint _ebo;
	private static uint _vao;

	private static int _windowWidth;
	private static int _windowHeight;

	private static bool _frameBegun;

	private static IntPtr _context;

	private static uint _uiShaderId;
	private static int _projMtx;
	private static int _texture;

	public static void Initialize(uint uiShaderId, int projMtx, int texture)
	{
		_windowWidth = 1920;
		_windowHeight = 1080;

		_context = ImGui.CreateContext();
		ImGui.SetCurrentContext(_context);
		ImGui.StyleColorsDark();

		ImGuiIOPtr io = ImGui.GetIO();
		io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

		_vbo = Graphics.Gl.GenBuffer();
		_ebo = Graphics.Gl.GenBuffer();

		RecreateFontDeviceTexture();

		SetPerFrameImGuiData(1f / 60f);
		ImGui.NewFrame();
		_frameBegun = true;

		_uiShaderId = uiShaderId;
		_projMtx = projMtx;
		_texture = texture;
	}

	private static unsafe void RecreateFontDeviceTexture()
	{
		ImGuiIOPtr io = ImGui.GetIO();

		io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out int bytesPerPixel);

		byte[] data = new byte[width * height * bytesPerPixel];
		Marshal.Copy(pixels, data, 0, data.Length);
		uint textureId = Graphics.Gl.GenTexture();

		Graphics.Gl.BindTexture(TextureTarget.Texture2D, textureId);

		Graphics.Gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
		Graphics.Gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);
		Graphics.Gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
		Graphics.Gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);

		fixed (byte* b = data)
			Graphics.Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)width, (uint)height, 0, GLEnum.Rgba, PixelType.UnsignedByte, b);

		io.Fonts.SetTexID((IntPtr)textureId);
	}

	public static void Destroy()
	{
		Graphics.Gl.DeleteBuffer(_vbo);
		Graphics.Gl.DeleteBuffer(_ebo);
		Graphics.Gl.DeleteVertexArray(_vao);

		ImGui.DestroyContext(_context);
	}

	public static void WindowResized(int width, int height)
	{
		_windowWidth = width;
		_windowHeight = height;
	}

	public static void Render()
	{
		if (!_frameBegun)
			return;

		_frameBegun = false;
		ImGui.Render();
		RenderImDrawData(ImGui.GetDrawData());
	}

	public static void Update(float deltaSeconds)
	{
		if (_frameBegun)
			ImGui.Render();

		SetPerFrameImGuiData(deltaSeconds);
		UpdateImGuiInput();

		_frameBegun = true;
		ImGui.NewFrame();
	}

	private static void SetPerFrameImGuiData(float dt)
	{
		ImGuiIOPtr io = ImGui.GetIO();
		io.DisplaySize = new(_windowWidth, _windowHeight);
		io.DisplayFramebufferScale = Vector2.One;
		io.DeltaTime = dt;
	}

	private static void UpdateImGuiInput()
	{
		ImGuiIOPtr io = ImGui.GetIO();

		io.MouseDown[0] = Input.IsButtonHeld(MouseButton.Left);
		io.MouseDown[1] = Input.IsButtonHeld(MouseButton.Right);
		io.MouseDown[2] = Input.IsButtonHeld(MouseButton.Middle);
		io.MousePos = Input.GetMousePosition();
		io.MouseWheel = Input.GetScroll();

		for (int i = 0; i < _allKeys.Count; i++)
		{
			Keys key = _allKeys[i];
			int keyValue = (int)key;
			if (keyValue < 0)
				continue;

			if (_controlKeys.Contains(key))
				io.AddKeyEvent(GetImGuiKey(key), _pressedControlKeys.Contains(key));
			else
				io.AddKeyEvent(GetImGuiKey(key), Input.IsKeyHeld(key));
		}

		_pressedControlKeys.Clear();

		for (int i = 0; i < _pressedChars.Count; i++)
			io.AddInputCharacter(_pressedChars[i]);

		_pressedChars.Clear();

		io.KeyCtrl = Input.IsKeyHeld(Keys.ControlLeft) || Input.IsKeyHeld(Keys.ControlRight);
		io.KeyAlt = Input.IsKeyHeld(Keys.AltLeft) || Input.IsKeyHeld(Keys.AltRight);
		io.KeyShift = Input.IsKeyHeld(Keys.ShiftLeft) || Input.IsKeyHeld(Keys.ShiftRight);
		io.KeySuper = Input.IsKeyHeld(Keys.SuperLeft) || Input.IsKeyHeld(Keys.SuperRight);
	}

	public static void PressKey(Keys keys, InputAction state)
	{
		if (state is not (InputAction.Press or InputAction.Repeat))
			return;

		if (_controlKeys.Contains(keys))
		{
			_pressedControlKeys.Add(keys);
		}
		else
		{
			ImGuiIOPtr io = ImGui.GetIO();
			bool shift = io.KeyShift;
			char? c = keys.GetChar(shift);

			if (c.HasValue)
				_pressedChars.Add(c.Value);
		}
	}

	private static unsafe void SetUpRenderState(ImDrawDataPtr drawDataPtr)
	{
		Graphics.Gl.Enable(GLEnum.Blend);
		Graphics.Gl.BlendEquation(GLEnum.FuncAdd);
		Graphics.Gl.BlendFuncSeparate(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha, GLEnum.One, GLEnum.OneMinusSrcAlpha);
		Graphics.Gl.Disable(GLEnum.CullFace);
		Graphics.Gl.Disable(GLEnum.DepthTest);
		Graphics.Gl.Disable(GLEnum.StencilTest);
		Graphics.Gl.Enable(GLEnum.ScissorTest);
		Graphics.Gl.Disable(GLEnum.PrimitiveRestart);
		Graphics.Gl.PolygonMode(GLEnum.FrontAndBack, GLEnum.Fill);

		Matrix4x4 orthographicProjection = Matrix4x4.CreateOrthographicOffCenter(
			left: drawDataPtr.DisplayPos.X,
			right: drawDataPtr.DisplayPos.X + drawDataPtr.DisplaySize.X,
			bottom: drawDataPtr.DisplayPos.Y + drawDataPtr.DisplaySize.Y,
			top: drawDataPtr.DisplayPos.Y,
			zNearPlane: -1,
			zFarPlane: 1);

		Graphics.Gl.UseProgram(_uiShaderId);
		Graphics.Gl.Uniform1(_texture, 0);
		Graphics.Gl.UniformMatrix4x4(_projMtx, orthographicProjection);

		Graphics.Gl.BindSampler(0, 0);

		_vao = Graphics.Gl.GenVertexArray();
		Graphics.Gl.BindVertexArray(_vao);

		Graphics.Gl.BindBuffer(GLEnum.ArrayBuffer, _vbo);
		Graphics.Gl.BindBuffer(GLEnum.ElementArrayBuffer, _ebo);

		Graphics.Gl.EnableVertexAttribArray(0);
		Graphics.Gl.EnableVertexAttribArray(1);
		Graphics.Gl.EnableVertexAttribArray(2);

		Graphics.Gl.VertexAttribPointer(0, 2, GLEnum.Float, false, (uint)sizeof(ImDrawVert), (void*)0);
		Graphics.Gl.VertexAttribPointer(1, 2, GLEnum.Float, false, (uint)sizeof(ImDrawVert), (void*)8);
		Graphics.Gl.VertexAttribPointer(2, 4, GLEnum.UnsignedByte, true, (uint)sizeof(ImDrawVert), (void*)16);
	}

	private static unsafe void RenderImDrawData(ImDrawDataPtr drawDataPtr)
	{
		int framebufferWidth = (int)(drawDataPtr.DisplaySize.X * drawDataPtr.FramebufferScale.X);
		int framebufferHeight = (int)(drawDataPtr.DisplaySize.Y * drawDataPtr.FramebufferScale.Y);
		if (framebufferWidth <= 0 || framebufferHeight <= 0)
			return;

		SetUpRenderState(drawDataPtr);

		Vector2 clipOff = drawDataPtr.DisplayPos;
		Vector2 clipScale = drawDataPtr.FramebufferScale;

		for (int n = 0; n < drawDataPtr.CmdListsCount; n++)
		{
			ImDrawListPtr cmdListPtr = drawDataPtr.CmdLists[n];

			Graphics.Gl.BufferData(GLEnum.ArrayBuffer, (nuint)(cmdListPtr.VtxBuffer.Size * sizeof(ImDrawVert)), (void*)cmdListPtr.VtxBuffer.Data, GLEnum.StreamDraw);
			Graphics.Gl.BufferData(GLEnum.ElementArrayBuffer, (nuint)(cmdListPtr.IdxBuffer.Size * sizeof(ushort)), (void*)cmdListPtr.IdxBuffer.Data, GLEnum.StreamDraw);

			for (int cmdI = 0; cmdI < cmdListPtr.CmdBuffer.Size; cmdI++)
			{
				ImDrawCmdPtr cmdPtr = cmdListPtr.CmdBuffer[cmdI];
				if (cmdPtr.UserCallback != IntPtr.Zero)
					throw new NotImplementedException();

				Vector4 clipRect;
				clipRect.X = (cmdPtr.ClipRect.X - clipOff.X) * clipScale.X;
				clipRect.Y = (cmdPtr.ClipRect.Y - clipOff.Y) * clipScale.Y;
				clipRect.Z = (cmdPtr.ClipRect.Z - clipOff.X) * clipScale.X;
				clipRect.W = (cmdPtr.ClipRect.W - clipOff.Y) * clipScale.Y;

				if (clipRect.X >= framebufferWidth || clipRect.Y >= framebufferHeight || clipRect.Z < 0.0f || clipRect.W < 0.0f)
					continue;

				Graphics.Gl.Scissor((int)clipRect.X, (int)(framebufferHeight - clipRect.W), (uint)(clipRect.Z - clipRect.X), (uint)(clipRect.W - clipRect.Y));

				Graphics.Gl.BindTexture(GLEnum.Texture2D, (uint)cmdPtr.TextureId);
				Graphics.Gl.DrawElementsBaseVertex(GLEnum.Triangles, cmdPtr.ElemCount, GLEnum.UnsignedShort, (void*)(cmdPtr.IdxOffset * sizeof(ushort)), (int)cmdPtr.VtxOffset);
			}
		}

		Graphics.Gl.DeleteVertexArray(_vao);
		_vao = 0;
		Graphics.Gl.Disable(EnableCap.ScissorTest);
	}

	private static ImGuiKey GetImGuiKey(Keys key)
	{
		return key switch
		{
			>= Keys.Number0 and <= Keys.Number9 => key - Keys.D0 + ImGuiKey._0,
			>= Keys.A and <= Keys.Z => key - Keys.A + ImGuiKey.A,
			>= Keys.Keypad0 and <= Keys.Keypad9 => key - Keys.Keypad0 + ImGuiKey.Keypad0,
			>= Keys.F1 and <= Keys.F24 => key - Keys.F1 + ImGuiKey.F24,
			_ => key switch
			{
				Keys.Tab => ImGuiKey.Tab,
				Keys.Left => ImGuiKey.LeftArrow,
				Keys.Right => ImGuiKey.RightArrow,
				Keys.Up => ImGuiKey.UpArrow,
				Keys.Down => ImGuiKey.DownArrow,
				Keys.PageUp => ImGuiKey.PageUp,
				Keys.PageDown => ImGuiKey.PageDown,
				Keys.Home => ImGuiKey.Home,
				Keys.End => ImGuiKey.End,
				Keys.Insert => ImGuiKey.Insert,
				Keys.Delete => ImGuiKey.Delete,
				Keys.Backspace => ImGuiKey.Backspace,
				Keys.Space => ImGuiKey.Space,
				Keys.Enter => ImGuiKey.Enter,
				Keys.Escape => ImGuiKey.Escape,
				Keys.Apostrophe => ImGuiKey.Apostrophe,
				Keys.Comma => ImGuiKey.Comma,
				Keys.Minus => ImGuiKey.Minus,
				Keys.Period => ImGuiKey.Period,
				Keys.Slash => ImGuiKey.Slash,
				Keys.Semicolon => ImGuiKey.Semicolon,
				Keys.Equal => ImGuiKey.Equal,
				Keys.LeftBracket => ImGuiKey.LeftBracket,
				Keys.BackSlash => ImGuiKey.Backslash,
				Keys.RightBracket => ImGuiKey.RightBracket,
				Keys.GraveAccent => ImGuiKey.GraveAccent,
				Keys.CapsLock => ImGuiKey.CapsLock,
				Keys.ScrollLock => ImGuiKey.ScrollLock,
				Keys.NumLock => ImGuiKey.NumLock,
				Keys.PrintScreen => ImGuiKey.PrintScreen,
				Keys.Pause => ImGuiKey.Pause,
				Keys.KeypadDecimal => ImGuiKey.KeypadDecimal,
				Keys.KeypadDivide => ImGuiKey.KeypadDivide,
				Keys.KeypadMultiply => ImGuiKey.KeypadMultiply,
				Keys.KeypadSubtract => ImGuiKey.KeypadSubtract,
				Keys.KeypadAdd => ImGuiKey.KeypadAdd,
				Keys.KeypadEnter => ImGuiKey.KeypadEnter,
				Keys.KeypadEqual => ImGuiKey.KeypadEqual,
				Keys.ShiftLeft => ImGuiKey.LeftShift,
				Keys.ControlLeft => ImGuiKey.LeftCtrl,
				Keys.AltLeft => ImGuiKey.LeftAlt,
				Keys.SuperLeft => ImGuiKey.LeftSuper,
				Keys.ShiftRight => ImGuiKey.RightShift,
				Keys.ControlRight => ImGuiKey.RightCtrl,
				Keys.AltRight => ImGuiKey.RightAlt,
				Keys.SuperRight => ImGuiKey.RightSuper,
				Keys.Menu => ImGuiKey.Menu,
				_ => ImGuiKey.None,
			},
		};
	}
}
