using Tools.Generator.Internals;

namespace Tools.Generator.Generators;

internal sealed class BufferGenerator : IGenerator
{
	private readonly int[] _bufferSizes = [ 4, 6, 8, 12, 16, 24, 32 ];

	public string Generate()
	{
		CodeWriter codeWriter = new();

		codeWriter.WriteLine(GeneratorConstants.Header);
		codeWriter.WriteLine();
		codeWriter.WriteLine("using System.Diagnostics;");
		codeWriter.WriteLine("using System.Diagnostics.CodeAnalysis;");
		codeWriter.WriteLine("using System.Runtime.CompilerServices;");
		codeWriter.WriteLine();
		codeWriter.WriteLine("namespace Detach.Buffers;");
		codeWriter.WriteLine();

		foreach (int bufferSize in _bufferSizes)
		{
			codeWriter.WriteLine("[InlineArray(Size)]");
			codeWriter.WriteLine("""[DebuggerDisplay("Items = {Items}")]""");
			codeWriter.WriteLine("""[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:Field names should begin with lower-case letter", Justification = "InlineArray")]""");
			codeWriter.WriteLine("""[SuppressMessage("Roslynator", "RCS1213:Remove unused member declaration", Justification = "InlineArray")]""");
			codeWriter.WriteLine($"public struct Buffer{bufferSize}<T> : IEquatable<Buffer{bufferSize}<T>>");
			codeWriter.StartIndent();
			codeWriter.WriteLine("where T : struct");
			codeWriter.EndIndent();

			codeWriter.StartBlock();

			codeWriter.WriteLine($"public const int Size = {bufferSize};");
			codeWriter.WriteLine();
			codeWriter.WriteLine("[DebuggerBrowsable(DebuggerBrowsableState.Never)]");
			codeWriter.WriteLine("private T _0;");
			codeWriter.WriteLine();
			codeWriter.WriteLine($"public Buffer{bufferSize}(params ReadOnlySpan<T> values)");
			codeWriter.StartBlock();
			codeWriter.WriteLine("for (int i = 0; i < Math.Min(values.Length, Size); i++)");
			codeWriter.StartIndent();
			codeWriter.WriteLine("this[i] = values[i];");
			codeWriter.EndIndent();
			codeWriter.EndBlock();
			codeWriter.WriteLine();
			codeWriter.WriteLine("[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]");
			codeWriter.WriteLine("internal T[] Items => this[..Size].ToArray();");
			codeWriter.WriteLine();
			codeWriter.WriteLine($"public static bool operator ==(Buffer{bufferSize}<T> left, Buffer{bufferSize}<T> right)");
			codeWriter.StartBlock();
			codeWriter.WriteLine("return left.Equals(right);");
			codeWriter.EndBlock();
			codeWriter.WriteLine();
			codeWriter.WriteLine($"public static bool operator !=(Buffer{bufferSize}<T> left, Buffer{bufferSize}<T> right)");
			codeWriter.StartBlock();
			codeWriter.WriteLine("return !left.Equals(right);");
			codeWriter.EndBlock();
			codeWriter.WriteLine();
			codeWriter.WriteLine($"public bool Equals(Buffer{bufferSize}<T> other)");
			codeWriter.StartBlock();
			codeWriter.WriteLine("for (int i = 0; i < Size; i++)");
			codeWriter.StartBlock();
			codeWriter.WriteLine("if (!this[i].Equals(other[i]))");
			codeWriter.StartIndent();
			codeWriter.WriteLine("return false;");
			codeWriter.EndIndent();
			codeWriter.EndBlock();
			codeWriter.WriteLine();
			codeWriter.WriteLine("return true;");
			codeWriter.EndBlock();
			codeWriter.WriteLine();
			codeWriter.WriteLine("public override bool Equals(object? obj)");
			codeWriter.StartBlock();
			codeWriter.WriteLine($"return obj is Buffer{bufferSize}<T> other && Equals(other);");
			codeWriter.EndBlock();
			codeWriter.WriteLine();
			codeWriter.WriteLine("public override int GetHashCode()");
			codeWriter.StartBlock();
			codeWriter.WriteLine("HashCode hash = default;");
			codeWriter.WriteLine("for (int i = 0; i < Size; i++)");
			codeWriter.StartIndent();
			codeWriter.WriteLine("hash.Add(this[i]);");
			codeWriter.EndIndent();
			codeWriter.WriteLine("return hash.ToHashCode();");
			codeWriter.EndBlock();

			codeWriter.EndBlock();

			codeWriter.WriteLine();
		}

		return codeWriter.ToString();
	}
}
