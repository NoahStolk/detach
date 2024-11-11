using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests;

[TestClass]
public class InlineStringInterpolationTests
{
	[TestMethod]
	public void Utf8Formatting()
	{
		AssertionUtils.SequenceEqual("Inline 1.10"u8, Inline.Utf8($"Inline {1.1:0.00}"));
		AssertionUtils.SequenceEqual("Inline 1"u8, Inline.Utf8($"Inline {1:0}"));
		Assert.AreEqual(0x00, Inline.BufferUtf8["Inline 1"u8.Length]);
	}

	[TestMethod]
	public void Utf16Formatting()
	{
		AssertionUtils.SequenceEqual("Inline 1.10", Inline.Utf16($"Inline {1.1:0.00}"));
		AssertionUtils.SequenceEqual("Inline 1", Inline.Utf16($"Inline {1:0}"));
		Assert.AreEqual('\0', Inline.BufferUtf16["Inline 1".Length]);
	}

	[TestMethod]
	public void Utf8NoAllocations()
	{
		long bytes = GC.GetAllocatedBytesForCurrentThread();
		Inline.Utf8($"Inline {1.1:0.00}");
		Assert.AreEqual(bytes, GC.GetAllocatedBytesForCurrentThread());
	}

	[TestMethod]
	public void Utf16NoAllocations()
	{
		long bytes = GC.GetAllocatedBytesForCurrentThread();
		Inline.Utf16($"Inline {1.1:0.00}");
		Assert.AreEqual(bytes, GC.GetAllocatedBytesForCurrentThread());
	}
}
