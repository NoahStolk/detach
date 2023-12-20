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
		Assert.AreEqual(new(1, 3, 2, 4), Matrix2.Transpose(matrix2));
	}

	[TestMethod]
	public void Matrix3Transpose()
	{
		Matrix3 matrix3 = new(1, 2, 3, 4, 5, 6, 7, 8, 9);
		Assert.AreEqual(new(1, 4, 7, 2, 5, 8, 3, 6, 9), Matrix3.Transpose(matrix3));
	}

	[TestMethod]
	public void Matrix4Transpose()
	{
		Matrix4 matrix4 = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
		Assert.AreEqual(new(1, 5, 9, 13, 2, 6, 10, 14, 3, 7, 11, 15, 4, 8, 12, 16), Matrix4.Transpose(matrix4));
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
		Assert.AreEqual(0, Matrix3.Determinant(new(
			1, 2, 3,
			4, 5, 6,
			7, 8, 9)));
		Assert.AreEqual(-33, Matrix3.Determinant(new(
			4, 3, 2,
			1, 9, 7,
			8, 6, 3)));
	}

	[TestMethod]
	public void Matrix4Determinant()
	{
		Matrix4 matrix4 = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -11, 12, 13, 14, 15, 16);

		// TODO: This is wrong.
		Assert.AreEqual(0, Matrix4.Determinant(matrix4));
	}

	[TestMethod]
	public void Matrix3Cut()
	{
		Matrix3 matrix3 = new(
			1, 2, 3,
			4, 5, 6,
			7, 8, 9);
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
		Assert.AreEqual(new(4, 3, 2, 1), Matrix2.Minor(matrix2));
	}

	[TestMethod]
	public void Matrix3Minor()
	{
		Assert.AreEqual(
			new(
				9, -24, -19,
				44, 25, 2,
				83, -79, 62),
			Matrix3.Minor(new(
				4, 6, -13,
				-7, 5, 3,
				1, 2, 3)));
	}

	[TestMethod]
	public void Matrix2Cofactor()
	{
		Assert.AreEqual(new(6, 2, -3, 5), Matrix2.Cofactor(new(5, 3, -2, 6)));
		Assert.AreEqual(new(3, 2, -5, 6), Matrix2.Cofactor(new(6, 5, -2, 3)));
	}

	[TestMethod]
	public void Matrix3Cofactor()
	{
		Assert.AreEqual(new(-65, 15, -22, 125, -56, -12, 8, -29, 19), Matrix3.Cofactor(new(4, 7, 9, -1, 3, 5, 4, 10, -5)));
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
		Matrix4 matrix4 = new(
			1, 2, 3, 4,
			5, 6, 7, 8,
			9, 10, -11, 12,
			13, 14, 15, 16);

		Assert.AreEqual(
			new(
				2, 4, 6, 8,
				10, 12, 14, 16,
				18, 20, -22, 24,
				26, 28, 30, 32),
			matrix4 * 2);
		Assert.AreEqual(
			new(
				3, 6, 9, 12,
				15, 18, 21, 24,
				27, 30, -33, 36,
				39, 42, 45, 48),
			matrix4 * 3);

		Assert.AreEqual(
			new(
				90, 100, 44, 120,
				202, 228, 100, 280,
				116, 136, 398, 176,
				426, 484, 212, 600),
			matrix4 * matrix4);
	}
}
