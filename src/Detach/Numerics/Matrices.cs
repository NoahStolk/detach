﻿namespace Detach.Numerics;

public static class Matrices
{
	public static Matrix2 Transpose(Matrix2 matrix2)
	{
		return new(
			matrix2.M11, matrix2.M21,
			matrix2.M12, matrix2.M22);
	}

	public static Matrix3 Transpose(Matrix3 matrix3)
	{
		return new(
			matrix3.M11, matrix3.M21, matrix3.M31,
			matrix3.M12, matrix3.M22, matrix3.M32,
			matrix3.M13, matrix3.M23, matrix3.M33);
	}

	public static Matrix4 Transpose(Matrix4 matrix4)
	{
		return new(
			matrix4.M11, matrix4.M21, matrix4.M31, matrix4.M41,
			matrix4.M12, matrix4.M22, matrix4.M32, matrix4.M42,
			matrix4.M13, matrix4.M23, matrix4.M33, matrix4.M43,
			matrix4.M14, matrix4.M24, matrix4.M34, matrix4.M44);
	}

	public static float Determinant(Matrix2 matrix2)
	{
		return matrix2.M11 * matrix2.M22 - matrix2.M12 * matrix2.M21;
	}

	public static float Determinant(Matrix3 matrix3)
	{
		return
			matrix3.M11 * matrix3.M22 * matrix3.M33
			+ matrix3.M12 * matrix3.M23 * matrix3.M31
			+ matrix3.M13 * matrix3.M21 * matrix3.M32
			- matrix3.M13 * matrix3.M22 * matrix3.M31
			- matrix3.M12 * matrix3.M21 * matrix3.M33
			- matrix3.M11 * matrix3.M23 * matrix3.M32;
	}

	public static float Determinant(Matrix4 matrix4)
	{
		return
			matrix4.M11 * matrix4.M22 * matrix4.M33 * matrix4.M44 + matrix4.M11 * matrix4.M23 * matrix4.M34 * matrix4.M42 + matrix4.M11 * matrix4.M24 * matrix4.M32 * matrix4.M43
			+ matrix4.M12 * matrix4.M21 * matrix4.M34 * matrix4.M43 + matrix4.M12 * matrix4.M23 * matrix4.M31 * matrix4.M44 + matrix4.M12 * matrix4.M24 * matrix4.M33 * matrix4.M41
			+ matrix4.M13 * matrix4.M21 * matrix4.M32 * matrix4.M44 + matrix4.M13 * matrix4.M22 * matrix4.M34 * matrix4.M41 + matrix4.M13 * matrix4.M24 * matrix4.M31 * matrix4.M42
			+ matrix4.M14 * matrix4.M21 * matrix4.M33 * matrix4.M42 + matrix4.M14 * matrix4.M22 * matrix4.M31 * matrix4.M43 + matrix4.M14 * matrix4.M23 * matrix4.M32 * matrix4.M41
			- matrix4.M11 * matrix4.M22 * matrix4.M34 * matrix4.M43 - matrix4.M11 * matrix4.M23 * matrix4.M32 * matrix4.M44 - matrix4.M11 * matrix4.M24 * matrix4.M33 * matrix4.M42
			- matrix4.M12 * matrix4.M21 * matrix4.M33 * matrix4.M44 - matrix4.M12 * matrix4.M23 * matrix4.M34 * matrix4.M41 - matrix4.M12 * matrix4.M24 * matrix4.M31 * matrix4.M43
			- matrix4.M13 * matrix4.M21 * matrix4.M34 * matrix4.M42 - matrix4.M13 * matrix4.M22 * matrix4.M31 * matrix4.M44 - matrix4.M13 * matrix4.M24 * matrix4.M32 * matrix4.M41
			- matrix4.M14 * matrix4.M21 * matrix4.M32 * matrix4.M43 - matrix4.M14 * matrix4.M22 * matrix4.M33 * matrix4.M41 - matrix4.M14 * matrix4.M23 * matrix4.M31 * matrix4.M42;
	}

	public static Matrix2 Cut(Matrix3 matrix3, int row, int col)
	{
		Matrix2 result = default;

		int i = 0;
		for (int r = 0; r < 3; r++)
		{
			if (r == row)
				continue;

			int j = 0;
			for (int c = 0; c < 3; c++)
			{
				if (c == col)
					continue;

				result[i, j] = matrix3[r, c];
				j++;
			}

			i++;
		}

		return result;
	}

	public static Matrix2 Minor(Matrix2 matrix2)
	{
		return new(matrix2.M22, matrix2.M21, matrix2.M12, matrix2.M11);
	}

	public static Matrix3 Minor(Matrix3 matrix3)
	{
		Matrix3 result = default;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				result[i, j] = Determinant(Cut(matrix3, i, j));
			}
		}

		return result;
	}

	public static Matrix2 Cofactor(Matrix2 matrix2)
	{
		Matrix2 minor = Minor(matrix2);
		Matrix2 result = default;
		for (int i = 0; i < 2; i++)
		{
			int sign = i % 2 == 0 ? 1 : -1;
			for (int j = 0; j < 2; j++)
			{
				result[i, j] = minor[i, j] * sign;
				sign *= -1;
			}
		}

		return result;
	}

	public static Matrix3 Cofactor(Matrix3 matrix3)
	{
		Matrix3 minor = Minor(matrix3);
		Matrix3 result = default;
		for (int i = 0; i < 3; i++)
		{
			int sign = i % 2 == 0 ? 1 : -1;
			for (int j = 0; j < 3; j++)
			{
				result[i, j] = minor[i, j] * sign;
				sign *= -1;
			}
		}

		return result;
	}

	public static TMatrixOut Multiply<TMatrixA, TMatrixB, TMatrixOut>(TMatrixA matrixA, int aRows, int aCols, TMatrixB matrixB, int bRows, int bCols)
		where TMatrixA : IMatrixOperations<TMatrixA>
		where TMatrixB : IMatrixOperations<TMatrixB>
		where TMatrixOut : IMatrixOperations<TMatrixOut>
	{
		if (aCols != bRows)
			throw new ArgumentException("The number of columns in the first matrix must be equal to the number of rows in the second matrix.");

		TMatrixOut matrixResult = TMatrixOut.CreateDefault();
		for (int i = 0; i < aRows; i++)
		{
			for (int j = 0; j < bCols; j++)
			{
				TMatrixOut.Set(ref matrixResult, bCols * i + j, 0.0f);
				for (int k = 0; k < bRows; k++)
				{
					int aIndex = aCols * i + k;
					int bIndex = bCols * k + j;

					float value = TMatrixOut.Get(matrixResult, bCols * i + j);

					TMatrixOut.Set(ref matrixResult, bCols * i + j, value + TMatrixA.Get(matrixA, aIndex) * TMatrixB.Get(matrixB, bIndex));
				}
			}
		}

		return matrixResult;
	}
}
