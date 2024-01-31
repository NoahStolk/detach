using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Detach.Buffers;

[InlineArray(4)]
[DebuggerDisplay("Items = {Items}")]
public struct Buffer4<T>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private T _0;

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	internal T[] Items => this[..4].ToArray();
}
