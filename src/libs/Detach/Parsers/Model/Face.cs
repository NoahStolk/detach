namespace Detach.Parsers.Model;

// TODO: This is not a face, this is only one of three parts of a face.
public readonly record struct Face(ushort Position, ushort Texture, ushort Normal)
{
	public override string ToString()
	{
		return $"{Position}/{Texture}/{Normal}";
	}
}
