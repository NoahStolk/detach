using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Detach.Buffers;

[InlineArray(8)]
[DebuggerDisplay("Items = {Items}")]
public struct Buffer8<T>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private T _0;

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	internal T[] Items => this[..8].ToArray();
}
