using ImGuiNET;
using Silk.NET.GLFW;

namespace Detach.Glfw.ImGuiBackend.ImGuiNET.Internals;

internal static class KeysExtensions
{
	public static ImGuiKey GetImGuiInputKey(this Keys key)
	{
		return key switch
		{
			>= Keys.F1 and <= Keys.F24 => ConvertRange(key, Keys.F1, ImGuiKey.F1),
			>= Keys.Keypad0 and <= Keys.Keypad9 => ConvertRange(key, Keys.Keypad0, ImGuiKey.Keypad0),
			>= Keys.A and <= Keys.Z => ConvertRange(key, Keys.A, ImGuiKey.A),
			>= Keys.Number0 and <= Keys.Number9 => ConvertRange(key, Keys.Number0, ImGuiKey._0),
			Keys.ShiftLeft or Keys.ShiftRight => ImGuiKey.ModShift,
			Keys.ControlLeft or Keys.ControlRight => ImGuiKey.ModCtrl,
			Keys.AltLeft or Keys.AltRight => ImGuiKey.ModAlt,
			Keys.SuperLeft or Keys.SuperRight => ImGuiKey.ModSuper,
			Keys.Menu => ImGuiKey.Menu,
			Keys.Up => ImGuiKey.UpArrow,
			Keys.Down => ImGuiKey.DownArrow,
			Keys.Left => ImGuiKey.LeftArrow,
			Keys.Right => ImGuiKey.RightArrow,
			Keys.Enter => ImGuiKey.Enter,
			Keys.Escape => ImGuiKey.Escape,
			Keys.Space => ImGuiKey.Space,
			Keys.Tab => ImGuiKey.Tab,
			Keys.Backspace => ImGuiKey.Backspace,
			Keys.Insert => ImGuiKey.Insert,
			Keys.Delete => ImGuiKey.Delete,
			Keys.PageUp => ImGuiKey.PageUp,
			Keys.PageDown => ImGuiKey.PageDown,
			Keys.Home => ImGuiKey.Home,
			Keys.End => ImGuiKey.End,
			Keys.CapsLock => ImGuiKey.CapsLock,
			Keys.ScrollLock => ImGuiKey.ScrollLock,
			Keys.PrintScreen => ImGuiKey.PrintScreen,
			Keys.Pause => ImGuiKey.Pause,
			Keys.NumLock => ImGuiKey.NumLock,
			Keys.KeypadDivide => ImGuiKey.KeypadDivide,
			Keys.KeypadMultiply => ImGuiKey.KeypadMultiply,
			Keys.KeypadSubtract => ImGuiKey.KeypadSubtract,
			Keys.KeypadAdd => ImGuiKey.KeypadAdd,
			Keys.KeypadDecimal => ImGuiKey.KeypadDecimal,
			Keys.KeypadEnter => ImGuiKey.KeypadEnter,
			Keys.GraveAccent => ImGuiKey.GraveAccent,
			Keys.Minus => ImGuiKey.Minus,
			Keys.Equal => ImGuiKey.Equal,
			Keys.LeftBracket => ImGuiKey.LeftBracket,
			Keys.RightBracket => ImGuiKey.RightBracket,
			Keys.Semicolon => ImGuiKey.Semicolon,
			Keys.Apostrophe => ImGuiKey.Apostrophe,
			Keys.Comma => ImGuiKey.Comma,
			Keys.Period => ImGuiKey.Period,
			Keys.Slash => ImGuiKey.Slash,
			Keys.BackSlash => ImGuiKey.Backslash,
			_ => ImGuiKey.None,
		};

		static ImGuiKey ConvertRange(Keys key, Keys startKey, ImGuiKey startImGuiKey)
		{
			int diff = (int)key - (int)startKey;
			return startImGuiKey + diff;
		}
	}
}
