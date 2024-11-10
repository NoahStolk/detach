using Silk.NET.OpenAL;
using System.Runtime.CompilerServices;

namespace Detach.AlExtensions;

public static class AlExtensions
{
	public static void CheckError(this AL al, [CallerMemberName] string caller = "", [CallerLineNumber] int line = 0)
	{
		AudioError error = al.GetError();
		if (error != AudioError.NoError)
			throw new InvalidOperationException($"OpenAL error '{error}'. First noted in '{caller}' on line {line}.");
	}
}
