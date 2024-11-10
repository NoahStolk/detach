using Detach.Buffers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests.Buffers;

[TestClass]
public class FromSpanTests
{
	[TestMethod]
	public void Buffer4FromSpan()
	{
		Buffer4<int> buffer = Buffer4<int>.FromSpan([1, 2, 3, 4]);
		Assert.AreEqual(1, buffer[0]);
		Assert.AreEqual(2, buffer[1]);
		Assert.AreEqual(3, buffer[2]);
		Assert.AreEqual(4, buffer[3]);

		buffer = Buffer4<int>.FromSpan([1, 2, 3, 0]);
		Assert.AreEqual(1, buffer[0]);
		Assert.AreEqual(2, buffer[1]);
		Assert.AreEqual(3, buffer[2]);
		Assert.AreEqual(0, buffer[3]);

		buffer = Buffer4<int>.FromSpan([1, 2, 3]);
		Assert.AreEqual(1, buffer[0]);
		Assert.AreEqual(2, buffer[1]);
		Assert.AreEqual(3, buffer[2]);
		Assert.AreEqual(0, buffer[3]);

		buffer = Buffer4<int>.FromSpan([]);
		Assert.AreEqual(0, buffer[0]);
		Assert.AreEqual(0, buffer[1]);
		Assert.AreEqual(0, buffer[2]);
		Assert.AreEqual(0, buffer[3]);

		buffer = Buffer4<int>.FromSpan([1, 2, 3, 4, 5]);
		Assert.AreEqual(1, buffer[0]);
		Assert.AreEqual(2, buffer[1]);
		Assert.AreEqual(3, buffer[2]);
		Assert.AreEqual(4, buffer[3]);
	}
}
