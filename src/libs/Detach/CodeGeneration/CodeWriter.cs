using System.Text;

namespace Detach.CodeGeneration;

public sealed class CodeWriter
{
	private readonly StringBuilder _sb = new();
	private int _indentLevel;

	public override string ToString()
	{
		return _sb.ToString();
	}

	public void WriteHeader()
	{
		WriteLine(GeneratorConstants.Header);
	}

	public void WriteRaw(string text)
	{
		_sb.Append(text);
	}

	public void WriteLineRaw(string line)
	{
		WriteRaw(line);
		_sb.Append(GeneratorConstants.NewLine);
	}

	public void Write(string text)
	{
		for (int i = 0; i < _indentLevel; i++)
			_sb.Append(GeneratorConstants.Indentation);

		WriteRaw(text);
	}

	public void WriteLine()
	{
		_sb.Append(GeneratorConstants.NewLine);
	}

	public void WriteLine(string line)
	{
		Write(line);
		_sb.Append(GeneratorConstants.NewLine);
	}

	public void StartBlock()
	{
		WriteLine("{");
		_indentLevel++;
	}

	public void EndBlock()
	{
		_indentLevel--;
		WriteLine("}");
	}

	public void EndBlockWithSemicolon()
	{
		_indentLevel--;
		WriteLine("};");
	}

	public void StartIndent()
	{
		_indentLevel++;
	}

	public void EndIndent()
	{
		_indentLevel--;
	}
}
