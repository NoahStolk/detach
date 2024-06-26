using Detach.Numerics;
using Detach.Utils;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace Detach.Parsers.Texture.TgaFormat;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct TgaImageData
{
	private readonly ushort _width;
	private readonly ushort _height;
	private readonly ImmutableArray<byte> _data;
	private readonly bool _rightToLeft;
	private readonly bool _topToBottom;
	private readonly bool _runLengthEncoding;
	private readonly TgaPixelDepth _pixelDepth;

	public TgaImageData(ushort width, ushort height, ImmutableArray<byte> data, bool rightToLeft, bool topToBottom, bool runLengthEncoding, TgaPixelDepth pixelDepth)
	{
		if (!Enum.IsDefined(typeof(TgaPixelDepth), pixelDepth))
			throw new ArgumentOutOfRangeException(nameof(pixelDepth), pixelDepth, "Invalid pixel depth.");

		_width = width;
		_height = height;
		_data = data;
		_rightToLeft = rightToLeft;
		_topToBottom = topToBottom;
		_runLengthEncoding = runLengthEncoding;
		_pixelDepth = pixelDepth;
	}

	public byte[] Read()
	{
		int bytesPerPixel = _pixelDepth.BytesPerPixel();
		int rowStart = _topToBottom ? 0 : _height - 1;
		int rowIncrement = _topToBottom ? 1 : -1;
		int columnStart = _rightToLeft ? _width - 1 : 0;
		int columnIncrement = _rightToLeft ? -1 : 1;

		// Output is always RGBA (top to bottom, left to right).
		byte[] bytes = new byte[_width * _height * 4];

		if (_runLengthEncoding)
		{
			int readPosition = 0;

			for (int i = rowStart; _topToBottom ? i < _height : i >= 0; i += rowIncrement)
			{
				for (int j = columnStart; _rightToLeft ? j >= 0 : j < _width;)
				{
					// Read the packet header.
					// In case of RLE packet, there is one color which is repeated packetLength times.
					// In case of raw packet, there are packetLength colors.
					bool isRlePacket = BitUtils.IsBitSet(_data[readPosition], 7);
					int packetLength = (_data[readPosition++] & 0b0111_1111) + 1;

					if (isRlePacket)
					{
						Rgba rgba = ReadRgba(_pixelDepth, _data.Slice(readPosition, bytesPerPixel).AsSpan());
						readPosition += bytesPerPixel;

						for (int k = 0; k < packetLength; k++)
						{
							int pixelWriteIndex = (i * _width + j) * 4;
							bytes[pixelWriteIndex + 0] = rgba.R;
							bytes[pixelWriteIndex + 1] = rgba.G;
							bytes[pixelWriteIndex + 2] = rgba.B;
							bytes[pixelWriteIndex + 3] = rgba.A;

							j += columnIncrement;
						}
					}
					else
					{
						for (int k = 0; k < packetLength; k++)
						{
							Rgba rgba = ReadRgba(_pixelDepth, _data.Slice(readPosition, bytesPerPixel).AsSpan());
							readPosition += bytesPerPixel;

							int pixelWriteIndex = (i * _width + j) * 4;
							bytes[pixelWriteIndex + 0] = rgba.R;
							bytes[pixelWriteIndex + 1] = rgba.G;
							bytes[pixelWriteIndex + 2] = rgba.B;
							bytes[pixelWriteIndex + 3] = rgba.A;

							j += columnIncrement;
						}
					}
				}
			}
		}
		else
		{
			int writePosition = 0;

			for (int i = rowStart; _topToBottom ? i < _height : i >= 0; i += rowIncrement)
			{
				for (int j = columnStart; _rightToLeft ? j >= 0 : j < _width; j += columnIncrement)
				{
					int pixelReadIndex = (i * _width + j) * bytesPerPixel;
					Rgba rgba = ReadRgba(_pixelDepth, _data.Slice(pixelReadIndex, bytesPerPixel).AsSpan());

					bytes[writePosition + 0] = rgba.R;
					bytes[writePosition + 1] = rgba.G;
					bytes[writePosition + 2] = rgba.B;
					bytes[writePosition + 3] = rgba.A;
					writePosition += 4;
				}
			}
		}

		return bytes;
	}

	public void Write(BinaryWriter binaryWriter)
	{
		if (!_runLengthEncoding)
			throw new NotImplementedException("Only RLE encoding is implemented.");

		if (_rightToLeft || !_topToBottom)
			throw new NotImplementedException("Only top to bottom, left to right image encoding is implemented.");

		int bytesPerPixel = _pixelDepth.BytesPerPixel();

		// Write the image data using RLE compression.
		// Always write top to bottom, left to right.
		for (int i = 0; i < _height; i++)
		{
			for (int j = 0; j < _width; j++)
			{
				Rgba currentRgba = ReadRgba(_pixelDepth, _data.Slice((i * _width + j) * bytesPerPixel, bytesPerPixel).AsSpan());

				int amountOfIdenticalPixels = 1;
				while (j + amountOfIdenticalPixels < _width && amountOfIdenticalPixels < 128)
				{
					Rgba nextRgba = ReadRgba(_pixelDepth, _data.Slice((i * _width + j + amountOfIdenticalPixels) * bytesPerPixel, bytesPerPixel).AsSpan());
					if (currentRgba == nextRgba)
						amountOfIdenticalPixels++;
					else
						break;
				}

				j += amountOfIdenticalPixels - 1;

				// Write the packet header.
				// In case of RLE packet, there is one color which is repeated packetLength times.
				// In case of raw packet, there are packetLength colors.
				bool isRlePacket = amountOfIdenticalPixels > 1;
				if (isRlePacket)
				{
					binaryWriter.Write((byte)(0b1000_0000 | (amountOfIdenticalPixels - 1)));
					binaryWriter.Write(currentRgba.R);
					binaryWriter.Write(currentRgba.G);
					binaryWriter.Write(currentRgba.B);
					binaryWriter.Write(currentRgba.A);
				}
				else
				{
					// TODO: Optimize this. We can write multiple pixels at once.
					binaryWriter.Write((byte)(amountOfIdenticalPixels - 1));

					for (int k = 0; k < amountOfIdenticalPixels; k++)
					{
						binaryWriter.Write(currentRgba.R);
						binaryWriter.Write(currentRgba.G);
						binaryWriter.Write(currentRgba.B);
						binaryWriter.Write(currentRgba.A);
					}
				}
			}
		}
	}

	private static Rgba ReadRgba(TgaPixelDepth pixelDepth, ReadOnlySpan<byte> span)
	{
		byte b = span[0];
		byte g = span[1];
		byte r = span[2];
		byte a = pixelDepth == TgaPixelDepth.Bgra ? span[3] : (byte)0xFF;
		return new Rgba(r, g, b, a);
	}
}
