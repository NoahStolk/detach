using Silk.NET.OpenAL;

namespace Detach.AlExtensions;

public static class BufferFormatUtils
{
	public static BufferFormat GetBufferFormat(int channels, int bitsPerSample)
	{
		return (channels, bitsPerSample) switch
		{
			(1, 16) => BufferFormat.Mono16,
			(2, 16) => BufferFormat.Stereo16,
			(1, 8) => BufferFormat.Mono8,
			(2, 8) => BufferFormat.Stereo8,
			_ => throw new NotSupportedException($"There is no OpenAL buffer format for {channels} channels and {bitsPerSample} bits per sample."),
		};
	}
}
