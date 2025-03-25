using System.Runtime.InteropServices;

namespace Detach.Parsers.Texture.TgaFormat;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct TgaHeader(byte idLength, TgaColorMapType colorMapType, TgaImageType imageType, ushort colorMapStartIndex, ushort colorMapLength, byte colorMapEntrySize, ushort originX, ushort originY, ushort width, ushort height, byte pixelDepth, byte imageDescriptor)
{
	public readonly byte IdLength = idLength;
	public readonly TgaColorMapType ColorMapType = colorMapType;
	public readonly TgaImageType ImageType = imageType;
	public readonly ushort ColorMapStartIndex = colorMapStartIndex;
	public readonly ushort ColorMapLength = colorMapLength;
	public readonly byte ColorMapEntrySize = colorMapEntrySize;
	public readonly ushort OriginX = originX;
	public readonly ushort OriginY = originY;
	public readonly ushort Width = width;
	public readonly ushort Height = height;
	public readonly byte PixelDepth = pixelDepth;
	public readonly byte ImageDescriptor = imageDescriptor;

	public static TgaHeader Read(BinaryReader br)
	{
		byte idLength = br.ReadByte();
		TgaColorMapType colorMapType = (TgaColorMapType)br.ReadByte();
		TgaImageType imageType = (TgaImageType)br.ReadByte();
		ushort colorMapStartIndex = br.ReadUInt16();
		ushort colorMapLength = br.ReadUInt16();
		byte colorMapEntrySize = br.ReadByte();
		ushort originX = br.ReadUInt16();
		ushort originY = br.ReadUInt16();
		ushort width = br.ReadUInt16();
		ushort height = br.ReadUInt16();
		byte pixelDepth = br.ReadByte();
		byte imageDescriptor = br.ReadByte();
		return new TgaHeader(idLength, colorMapType, imageType, colorMapStartIndex, colorMapLength, colorMapEntrySize, originX, originY, width, height, pixelDepth, imageDescriptor);
	}

	public void Write(BinaryWriter bw)
	{
		bw.Write(IdLength);
		bw.Write((byte)ColorMapType);
		bw.Write((byte)ImageType);
		bw.Write(ColorMapStartIndex);
		bw.Write(ColorMapLength);
		bw.Write(ColorMapEntrySize);
		bw.Write(OriginX);
		bw.Write(OriginY);
		bw.Write(Width);
		bw.Write(Height);
		bw.Write(PixelDepth);
		bw.Write(ImageDescriptor);
	}
}
