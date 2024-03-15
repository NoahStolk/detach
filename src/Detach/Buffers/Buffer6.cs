using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Detach.Buffers;

[InlineArray(6)]
[DebuggerDisplay("Items = {Items}")]
public struct Buffer6<T>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private T _0;

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	internal T[] Items => this[..6].ToArray();
}
