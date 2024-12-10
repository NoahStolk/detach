using Detach.Buffers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests.Buffers;

[TestClass]
public sealed class FromSpanTests
{
	[TestMethod]
	public void Buffer4FromSpan()
	{
		Buffer4<int> buffer = new(1, 2, 3, 4);
		Assert.AreEqual(1, buffer[0]);
		Assert.AreEqual(2, buffer[1]);
		Assert.AreEqual(3, buffer[2]);
		Assert.AreEqual(4, buffer[3]);

		buffer = new Buffer4<int>(1, 2, 3, 0);
		Assert.AreEqual(1, buffer[0]);
		Assert.AreEqual(2, buffer[1]);
		Assert.AreEqual(3, buffer[2]);
		Assert.AreEqual(0, buffer[3]);

		buffer = new Buffer4<int>(1, 2, 3);
		Assert.AreEqual(1, buffer[0]);
		Assert.AreEqual(2, buffer[1]);
		Assert.AreEqual(3, buffer[2]);
		Assert.AreEqual(0, buffer[3]);

		buffer = default;
		Assert.AreEqual(0, buffer[0]);
		Assert.AreEqual(0, buffer[1]);
		Assert.AreEqual(0, buffer[2]);
		Assert.AreEqual(0, buffer[3]);

		buffer = new Buffer4<int>(1, 2, 3, 4, 5);
		Assert.AreEqual(1, buffer[0]);
		Assert.AreEqual(2, buffer[1]);
		Assert.AreEqual(3, buffer[2]);
		Assert.AreEqual(4, buffer[3]);

		Span<int> span = [11, 21, 31, 41];
		buffer = new Buffer4<int>(span);
		Assert.AreEqual(11, buffer[0]);
		Assert.AreEqual(21, buffer[1]);
		Assert.AreEqual(31, buffer[2]);
		Assert.AreEqual(41, buffer[3]);

		span[1] = 0;
		Assert.AreEqual(21, buffer[1]);
	}
}
