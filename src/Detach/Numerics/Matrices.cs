namespace Detach.Numerics;

public static class Matrices
{
	public static Matrix2 Cut(Matrix3 matrix3, int row, int col)
	{
		Matrix2 result = default;
		int index = 0;

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (i == row || j == col)
					continue;

				int target = index++;
				int source = 3 * i + j;
				result[target] = matrix3[source];
			}
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
				result[i, j] = Matrix2.Determinant(Cut(matrix3, i, j));
		}

		return result;
	}

	public static TMatrixOut Multiply<TMatrixA, TMatrixB, TMatrixOut>(TMatrixA matrixA, TMatrixB matrixB)
		where TMatrixA : IMatrixOperations<TMatrixA>
		where TMatrixB : IMatrixOperations<TMatrixB>
		where TMatrixOut : IMatrixOperations<TMatrixOut>
	{
		if (TMatrixA.Cols != TMatrixB.Rows)
			throw new ArgumentException("The number of columns in the first matrix must be equal to the number of rows in the second matrix.");

		TMatrixOut matrixResult = TMatrixOut.CreateDefault();
		for (int i = 0; i < TMatrixA.Rows; i++)
		{
			for (int j = 0; j < TMatrixB.Cols; j++)
			{
				TMatrixOut.Set(ref matrixResult, TMatrixB.Cols * i + j, 0.0f);
				for (int k = 0; k < TMatrixB.Rows; k++)
				{
					int aIndex = TMatrixA.Cols * i + k;
					int bIndex = TMatrixB.Cols * k + j;

					float value = TMatrixOut.Get(matrixResult, TMatrixB.Cols * i + j);

					TMatrixOut.Set(ref matrixResult, TMatrixB.Cols * i + j, value + TMatrixA.Get(matrixA, aIndex) * TMatrixB.Get(matrixB, bIndex));
				}
			}
		}

		return matrixResult;
	}

	public static TMatrixOut Cofactor<TMatrix, TMatrixOut>(TMatrix matrixMinor)
		where TMatrix : IMatrixOperations<TMatrix>
		where TMatrixOut : IMatrixOperations<TMatrixOut>
	{
		TMatrixOut result = TMatrixOut.CreateDefault();
		for (int i = 0; i < TMatrix.Rows; i++)
		{
			for (int j = 0; j < TMatrix.Cols; j++)
			{
				// float value = TMatrix.Get(matrix, i, j);
				// if ((i + j) % 2 == 1)
				// 	value = -value;
				//
				// TMatrixOut.Set(ref result, TMatrix.Cols * i + j, value);

				int t = TMatrix.Cols * j + i;
				int s = TMatrix.Cols * i + j; // ??
				float sign = MathF.Pow(-1f, i + j); // (t % 2 == 0) ? 1.0f : -1.0f;
				TMatrixOut.Set(ref result, t, TMatrix.Get(matrixMinor, s) * sign);
			}
		}

		return result;
	}
}
