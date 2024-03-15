using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Detach.Buffers;

[InlineArray(12)]
[DebuggerDisplay("Items = {Items}")]
public struct Buffer12<T>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private T _0;

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	internal T[] Items => this[..12].ToArray();
}
