namespace Detach.Parsers.Model;

public class ModelParseException : Exception
{
	public ModelParseException()
	{
	}

	public ModelParseException(string? message)
		: base(message)
	{
	}

	public ModelParseException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}
}
