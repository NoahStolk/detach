using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Detach.Tests.Tests;

[TestClass]
public class InlineStringInterpolationTests
{
	[TestMethod]
	public void Utf8FormattingSpanFormattable()
	{
		AssertionUtils.SequenceEqual("Inline 1.10"u8, Inline.Utf8($"Inline {1.1:0.00}"));
		AssertionUtils.SequenceEqual("Inline 1"u8, Inline.Utf8($"Inline {1:0}"));
		Assert.AreEqual(0x00, Inline.BufferUtf8["Inline 1"u8.Length]);
	}

	[TestMethod]
	public void Utf8FormattingBool()
	{
		AssertionUtils.SequenceEqual("Value: True"u8, Inline.Utf8($"Value: {true}"));
		AssertionUtils.SequenceEqual("Value: False"u8, Inline.Utf8($"Value: {false}"));
		Assert.AreEqual(0x00, Inline.BufferUtf8["Value: False"u8.Length]);
	}

	[TestMethod]
	public void Utf8FormattingVector2()
	{
		AssertionUtils.SequenceEqual("Value: 1.10, 2.20"u8, Inline.Utf8($"Value: {new Vector2(1.1f, 2.2f):0.00}"));
		AssertionUtils.SequenceEqual("Value: 1, 2"u8, Inline.Utf8($"Value: {new Vector2(1.1f, 2.2f):0}"));
		Assert.AreEqual(0x00, Inline.BufferUtf8["Value: 1, 2"u8.Length]);
	}

	[TestMethod]
	public void Utf8FormattingVector3()
	{
		AssertionUtils.SequenceEqual("Value: 1.10, 2.20, 3.30"u8, Inline.Utf8($"Value: {new Vector3(1.1f, 2.2f, 3.3f):0.00}"));
		AssertionUtils.SequenceEqual("Value: 1, 2, 3"u8, Inline.Utf8($"Value: {new Vector3(1.1f, 2.2f, 3.3f):0}"));
		Assert.AreEqual(0x00, Inline.BufferUtf8["Value: 1, 2, 3"u8.Length]);
	}

	[TestMethod]
	public void Utf8FormattingVector4()
	{
		AssertionUtils.SequenceEqual("Value: 1.10, 2.20, 3.30, 4.40"u8, Inline.Utf8($"Value: {new Vector4(1.1f, 2.2f, 3.3f, 4.4f):0.00}"));
		AssertionUtils.SequenceEqual("Value: 1, 2, 3, 4"u8, Inline.Utf8($"Value: {new Vector4(1.1f, 2.2f, 3.3f, 4.4f):0}"));
		Assert.AreEqual(0x00, Inline.BufferUtf8["Value: 1, 2, 3, 4"u8.Length]);
	}

	[TestMethod]
	public void Utf8FormattingQuaternion()
	{
		AssertionUtils.SequenceEqual("Value: 1.10, 2.20, 3.30, 4.40"u8, Inline.Utf8($"Value: {new Quaternion(1.1f, 2.2f, 3.3f, 4.4f):0.00}"));
		AssertionUtils.SequenceEqual("Value: 1, 2, 3, 4"u8, Inline.Utf8($"Value: {new Quaternion(1.1f, 2.2f, 3.3f, 4.4f):0}"));
		Assert.AreEqual(0x00, Inline.BufferUtf8["Value: 1, 2, 3, 4"u8.Length]);
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
