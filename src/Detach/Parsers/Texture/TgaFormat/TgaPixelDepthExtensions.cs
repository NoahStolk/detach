namespace Detach.Parsers.Texture.TgaFormat;

internal static class TgaPixelDepthExtensions
{
	public static int BytesPerPixel(this TgaPixelDepth pixelDepth)
	{
		return (int)pixelDepth / 8;
	}
}
