namespace Detach.Numerics;

public struct Matrix2
{
	public float M11;
	public float M12;
	public float M21;
	public float M22;

	public Matrix2(float m11, float m12, float m21, float m22)
	{
		M11 = m11;
		M12 = m12;
		M21 = m21;
		M22 = m22;
	}

	public static Matrix2 Identity => new(1, 0, 0, 1);

	public float this[int row, int col]
	{
		get => row switch
		{
			0 => col switch
			{
				0 => M11,
				1 => M12,
				_ => throw new IndexOutOfRangeException(),
			},
			1 => col switch
			{
				0 => M21,
				1 => M22,
				_ => throw new IndexOutOfRangeException(),
			},
			_ => throw new IndexOutOfRangeException(),
		};
		set
		{
			switch (row)
			{
				case 0:
					switch (col)
					{
						case 0: M11 = value; break;
						case 1: M12 = value; break;
						default: throw new IndexOutOfRangeException();
					}

					break;
				case 1:
					switch (col)
					{
						case 0: M21 = value; break;
						case 1: M22 = value; break;
						default: throw new IndexOutOfRangeException();
					}

					break;
				default:
					throw new IndexOutOfRangeException();
			}
		}
	}

	public static Matrix2 operator *(Matrix2 matrix, float scalar)
	{
		return new(
			matrix.M11 * scalar, matrix.M12 * scalar,
			matrix.M21 * scalar, matrix.M22 * scalar);
	}

	public static Matrix2 operator *(Matrix2 matrixA, Matrix2 matrixB)
	{
		return new(
			matrixA.M11 * matrixB.M11 + matrixA.M12 * matrixB.M21,
			matrixA.M11 * matrixB.M12 + matrixA.M12 * matrixB.M22,
			matrixA.M21 * matrixB.M11 + matrixA.M22 * matrixB.M21,
			matrixA.M21 * matrixB.M12 + matrixA.M22 * matrixB.M22);
	}

	public Matrix2 Transpose()
	{
		return new(
			M11, M21,
			M12, M22);
	}

	public float Determinant()
	{
		return M11 * M22 - M12 * M21;
	}

	public static Matrix2 Cut(Matrix3 matrix3, int row, int col)
	{
		Matrix2 result = default;

		int i = 0;
		int j = 0;
		for (int r = 0; r < 3; r++)
		{
			if (r == row)
				continue;

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

	public Matrix2 Minor()
	{
		return new(M22, M21, M12, M11);
	}

	public Matrix2 Cofactor()
	{
		Matrix2 minor = Minor();
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
}
