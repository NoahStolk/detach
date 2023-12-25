using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Detach.Buffers;

[InlineArray(24)]
[DebuggerDisplay("Items = {Items}")]
public struct Buffer24<T>
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private T _0;

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	internal T[] Items => this[..24].ToArray();
}
