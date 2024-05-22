namespace Detach.Parsers.Texture;

public class TextureParseException : Exception
{
	public TextureParseException()
	{
	}

	public TextureParseException(string? message)
		: base(message)
	{
	}

	public TextureParseException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}
}
