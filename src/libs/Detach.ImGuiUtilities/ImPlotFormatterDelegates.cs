namespace Detach.ImGuiUtilities;

public static unsafe class ImPlotFormatterDelegates
{
	public static int Format(double value, byte* buff, int size, void* userData, ReadOnlySpan<char> format)
	{
		Span<byte> span = stackalloc byte[size];
		value.TryFormat(span, out int bytesWritten, format, null);
		for (int i = 0; i < bytesWritten; i++)
			*buff++ = span[i];
		*buff = 0x00;
		return bytesWritten;
	}
}
