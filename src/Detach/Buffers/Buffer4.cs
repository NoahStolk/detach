﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Detach.Buffers;

[InlineArray(_size)]
[DebuggerDisplay("Items = {Items}")]
[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:Field names should begin with lower-case letter", Justification = "InlineArray")]
[SuppressMessage("Roslynator", "RCS1213:Remove unused member declaration", Justification = "InlineArray")]
public struct Buffer4<T> : IEquatable<Buffer4<T>>
	where T : struct
{
	private const int _size = 4;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private T _0;

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	internal T[] Items => this[.._size].ToArray();

	public static bool operator ==(Buffer4<T> left, Buffer4<T> right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Buffer4<T> left, Buffer4<T> right)
	{
		return !left.Equals(right);
	}

	public bool Equals(Buffer4<T> other)
	{
		for (int i = 0; i < _size; i++)
		{
			if (!this[i].Equals(other[i]))
				return false;
		}

		return true;
	}

	public override bool Equals(object? obj)
	{
		return obj is Buffer4<T> other && Equals(other);
	}

	public override int GetHashCode()
	{
		HashCode hash = default;
		for (int i = 0; i < _size; i++)
			hash.Add(this[i]);
		return hash.ToHashCode();
	}

	public static Buffer4<T> FromSpan(ReadOnlySpan<T> values)
	{
		Buffer4<T> buffer = default;
		for (int i = 0; i < Math.Min(values.Length, _size); i++)
			buffer[i] = values[i];
		return buffer;
	}
}
