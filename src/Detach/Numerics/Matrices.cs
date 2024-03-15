using System.Numerics;

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

	public static Matrix3 Cut(Matrix4 matrix4, int row, int col)
	{
		Matrix3 result = default;
		int index = 0;

		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if (i == row || j == col)
					continue;

				int target = index++;
				int source = 4 * i + j;
				result[target] = matrix4[source];
			}
		}

		return result;
	}

	public static Vector3 MultiplyPoint(Vector3 vector3, Matrix4 matrix4)
	{
		float x = vector3.X * matrix4.M11 + vector3.Y * matrix4.M21 + vector3.Z * matrix4.M31 + matrix4.M41;
		float y = vector3.X * matrix4.M12 + vector3.Y * matrix4.M22 + vector3.Z * matrix4.M32 + matrix4.M42;
		float z = vector3.X * matrix4.M13 + vector3.Y * matrix4.M23 + vector3.Z * matrix4.M33 + matrix4.M43;
		return new(x, y, z);
	}

	public static Vector3 MultiplyVector(Vector3 vector3, Matrix4 matrix4)
	{
		float x = vector3.X * matrix4.M11 + vector3.Y * matrix4.M21 + vector3.Z * matrix4.M31;
		float y = vector3.X * matrix4.M12 + vector3.Y * matrix4.M22 + vector3.Z * matrix4.M32;
		float z = vector3.X * matrix4.M13 + vector3.Y * matrix4.M23 + vector3.Z * matrix4.M33;
		return new(x, y, z);
	}

	public static Vector3 MultiplyVector(Vector3 vector3, Matrix3 matrix3)
	{
		float x = Vector3.Dot(vector3, new(matrix3.M11, matrix3.M21, matrix3.M31));
		float y = Vector3.Dot(vector3, new(matrix3.M12, matrix3.M22, matrix3.M32));
		float z = Vector3.Dot(vector3, new(matrix3.M13, matrix3.M23, matrix3.M33));
		return Vector3.Normalize(new(x, y, z));
	}

	public static Vector2 Multiply(Vector2 vector, Matrix2 matrix)
	{
		Vector2 result = default;
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
				result[i] += vector[j] * matrix[2 * j + i];
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

		TMatrixOut matrixResult = TMatrixOut.Default();
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
		TMatrixOut result = TMatrixOut.Default();
		for (int i = 0; i < TMatrix.Rows; i++)
		{
			for (int j = 0; j < TMatrix.Cols; j++)
			{
				int index = TMatrix.Cols * j + i;
				float sign = MathF.Pow(-1f, i + j);
				TMatrixOut.Set(ref result, index, TMatrix.Get(matrixMinor, index) * sign);
			}
		}

		return result;
	}

	public static TMatrix Adjugate<TMatrix>(TMatrix matrix)
		where TMatrix : IMatrixOperations<TMatrix>
	{
		return TMatrix.Transpose(TMatrix.Cofactor(matrix));
	}

	public static TMatrix Inverse<TMatrix>(TMatrix matrix)
		where TMatrix : IMatrixOperations<TMatrix>
	{
		float determinant = TMatrix.Determinant(matrix);
		if (determinant == 0)
			return TMatrix.Identity;

		return TMatrix.Adjugate(matrix) * (1f / determinant);
	}
}
