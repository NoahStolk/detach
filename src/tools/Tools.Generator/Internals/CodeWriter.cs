using System.Text;

namespace Tools.Generator.Internals;

internal sealed class CodeWriter
{
	private readonly StringBuilder _sb = new();
	private int _indentLevel;

	public override string ToString()
	{
		return _sb.ToString();
	}

	public void WriteLine(string line)
	{
		WriteIndentedLine(line);
	}

	public void WriteLine()
	{
		_sb.Append(GeneratorConstants.NewLine);
	}

	public void StartBlock()
	{
		WriteIndentedLine("{");
		_indentLevel++;
	}

	public void EndBlock()
	{
		_indentLevel--;
		WriteIndentedLine("}");
	}

	public void EndBlockWithSemicolon()
	{
		_indentLevel--;
		WriteIndentedLine("};");
	}

	public void StartIndent()
	{
		_indentLevel++;
	}

	public void EndIndent()
	{
		_indentLevel--;
	}

	private void WriteIndentedLine(string line)
	{
		for (int i = 0; i < _indentLevel; i++)
			_sb.Append(GeneratorConstants.Indentation);

		_sb.Append(line);
		_sb.Append(GeneratorConstants.NewLine);
	}
}
