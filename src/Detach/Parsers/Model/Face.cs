namespace Detach.Parsers.Model;

public readonly record struct Face(ushort Position, ushort Texture, ushort Normal)
{
	public override string ToString()
	{
		return $"{Position}/{Texture}/{Normal}";
	}
}
