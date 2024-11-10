using Detach.Buffers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests.Buffers;

[TestClass]
public class EqualityTests
{
	[TestMethod]
	public void Buffer4Equality()
	{
		Buffer4<int> buffer1 = Buffer4<int>.FromSpan([1, 2, 3]);
		Buffer4<int> buffer2 = Buffer4<int>.FromSpan([1, 2, 3, 0]);
		Buffer4<int> buffer3 = Buffer4<int>.FromSpan([1, 12]);
		Buffer4<int> buffer4 = Buffer4<int>.FromSpan([1, 2, 3, 4]);

		Assert.AreEqual(buffer1, buffer2);
		Assert.AreNotEqual(buffer1, buffer3);
		Assert.AreNotEqual(buffer1, buffer4);

		Assert.AreNotEqual(buffer2, buffer3);
		Assert.AreNotEqual(buffer2, buffer4);

		Assert.AreNotEqual(buffer3, buffer4);
	}
}
