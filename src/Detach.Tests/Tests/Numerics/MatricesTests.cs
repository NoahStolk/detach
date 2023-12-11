using Detach.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Detach.Tests.Tests.Numerics;

[TestClass]
public class MatricesTests
{
	[TestMethod]
	public void Matrix2Transpose()
	{
		Matrix2 matrix2 = new(1, 2, 3, 4);
		Assert.AreEqual(new(1, 3, 2, 4), Matrices.Transpose(matrix2));
	}

	[TestMethod]
	public void Matrix3Transpose()
	{
		Matrix3 matrix3 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);
		Assert.AreEqual(new(1, 4, 7, 2, 5, 8, 3, 6, 9), Matrices.Transpose(matrix3));
	}

	[TestMethod]
	public void Matrix4Transpose()
	{
		Matrix4 matrix4 = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
		Assert.AreEqual(new(1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16), Matrices.Transpose(matrix4));
	}

	[TestMethod]
	public void Matrix2Determinant()
	{
		Matrix2 matrix2 = new(1, 2, 3, 4);
		Assert.AreEqual(-2, Matrix2.Determinant(matrix2));
	}

	[TestMethod]
	public void Matrix3Determinant()
	{
		Matrix3 matrix3 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);
		Assert.AreEqual(0, Matrix3.Determinant(matrix3));

		Matrix3 matrix = new(4, 3, 2, 1, 9, 7, 8, 6, 3);

		// TODO: Check if this is correct.
		Assert.AreEqual(-45, Matrix3.Determinant(matrix));
	}

	[TestMethod]
	public void Matrix4Determinant()
	{
		Matrix4 matrix4 = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -11, 12, 13, 14, 15, 16);
		Assert.AreEqual(0, Matrix4.Determinant(matrix4));
	}

	[TestMethod]
	public void Matrix3Cut()
	{
		Matrix3 matrix3 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);
		Assert.AreEqual(new(5, 6, 8, 9), Matrices.Cut(matrix3, 0, 0));
		Assert.AreEqual(new(4, 6, 7, 9), Matrices.Cut(matrix3, 0, 1));
		Assert.AreEqual(new(4, 5, 7, 8), Matrices.Cut(matrix3, 0, 2));
		Assert.AreEqual(new(2, 3, 8, 9), Matrices.Cut(matrix3, 1, 0));
		Assert.AreEqual(new(1, 3, 7, 9), Matrices.Cut(matrix3, 1, 1));
		Assert.AreEqual(new(1, 2, 7, 8), Matrices.Cut(matrix3, 1, 2));
		Assert.AreEqual(new(2, 3, 5, 6), Matrices.Cut(matrix3, 2, 0));
		Assert.AreEqual(new(1, 3, 4, 6), Matrices.Cut(matrix3, 2, 1));
		Assert.AreEqual(new(1, 2, 4, 5), Matrices.Cut(matrix3, 2, 2));
	}

	[TestMethod]
	public void Matrix2Minor()
	{
		Matrix2 matrix2 = new(1, 2, 3, 4);
		Assert.AreEqual(new(4, 3, 2, 1), Matrices.Minor(matrix2));
	}

	[TestMethod]
	public void Matrix3Minor()
	{
		Matrix3 matrix3 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);

		// TODO: This is wrong.
		Assert.AreEqual(new(5, 4, 2, 1, 9, 7, 8, 6, 3), Matrices.Minor(matrix3));
	}

	[TestMethod]
	public void Matrix2Cofactor()
	{
		Matrix2 matrix2 = new(1, 2, 3, 4);
		Assert.AreEqual(new(4, -3, -2, 1), Matrix2.Cofactor(matrix2));
	}

	[TestMethod]
	public void Matrix3Cofactor()
	{
		Matrix3 matrix3 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);
		Assert.AreEqual(new(5, -4, 2, -1, -9, 7, -8, 6, 3), Matrix3.Cofactor(matrix3));
	}

	[TestMethod]
	public void Matrix2Multiply()
	{
		Matrix2 matrix2 = new(1, 2, 3, 4);

		Assert.AreEqual(new(2, 4, 6, 8), matrix2 * 2);
		Assert.AreEqual(new(3, 6, 9, 12), matrix2 * 3);

		Assert.AreEqual(new(7, 10, 15, 22), matrix2 * matrix2);
	}

	[TestMethod]
	public void Matrix3Multiply()
	{
		Matrix3 matrix3 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);

		Assert.AreEqual(new(2, 4, 6, 8, 10, 12, 14, 16, 18), matrix3 * 2);
		Assert.AreEqual(new(3, 6, 9, 12, 15, 18, 21, 24, 27), matrix3 * 3);

		Assert.AreEqual(new(30, 36, 42, 66, 81, 96, 102, 126, 150), matrix3 * matrix3);
	}

	[TestMethod]
	public void Matrix4Multiply()
	{
		Matrix4 matrix4 = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -11, 12, 13, 14, 15, 16);

		Assert.AreEqual(new(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, -22, 24, 26, 28, 30, 32), matrix4 * 2);
		Assert.AreEqual(new(3, 6, 9, 12, 15, 18, 21, 24, 27, 30, -33, 36, 39, 42, 45, 48), matrix4 * 3);

		// TODO: This is wrong.
		Assert.AreEqual(new(90, 100, 110, 120, 202, 228, 254, 280, 314, 356, 398, 440, 426, 484, 542, 600), matrix4 * matrix4);
	}
}
