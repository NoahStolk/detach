namespace Detach.Numerics;

public struct Matrix3
{
	public float M11;
	public float M12;
	public float M13;
	public float M21;
	public float M22;
	public float M23;
	public float M31;
	public float M32;
	public float M33;

	public Matrix3(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
	{
		M11 = m11;
		M12 = m12;
		M13 = m13;
		M21 = m21;
		M22 = m22;
		M23 = m23;
		M31 = m31;
		M32 = m32;
		M33 = m33;
	}

	public static Matrix3 Identity => new(1, 0, 0, 0, 1, 0, 0, 0, 1);

	public float this[int row, int col]
	{
		get => row switch
		{
			0 => col switch
			{
				0 => M11,
				1 => M12,
				2 => M13,
				_ => throw new IndexOutOfRangeException(),
			},
			1 => col switch
			{
				0 => M21,
				1 => M22,
				2 => M23,
				_ => throw new IndexOutOfRangeException(),
			},
			2 => col switch
			{
				0 => M31,
				1 => M32,
				2 => M33,
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
						case 2: M13 = value; break;
						default: throw new IndexOutOfRangeException();
					}

					break;
				case 1:
					switch (col)
					{
						case 0: M21 = value; break;
						case 1: M22 = value; break;
						case 2: M23 = value; break;
						default: throw new IndexOutOfRangeException();
					}

					break;
				case 2:
					switch (col)
					{
						case 0: M31 = value; break;
						case 1: M32 = value; break;
						case 2: M33 = value; break;
						default: throw new IndexOutOfRangeException();
					}

					break;
				default:
					throw new IndexOutOfRangeException();
			}
		}
	}

	public static Matrix3 operator *(Matrix3 matrix, float scalar)
	{
		return new(
			matrix.M11 * scalar, matrix.M12 * scalar, matrix.M13 * scalar,
			matrix.M21 * scalar, matrix.M22 * scalar, matrix.M23 * scalar,
			matrix.M31 * scalar, matrix.M32 * scalar, matrix.M33 * scalar);
	}

	public static Matrix3 operator *(Matrix3 matrixA, Matrix3 matrixB)
	{
		return new(
			matrixA.M11 * matrixB.M11 + matrixA.M12 * matrixB.M21 + matrixA.M13 * matrixB.M31,
			matrixA.M11 * matrixB.M12 + matrixA.M12 * matrixB.M22 + matrixA.M13 * matrixB.M32,
			matrixA.M11 * matrixB.M13 + matrixA.M12 * matrixB.M23 + matrixA.M13 * matrixB.M33,
			matrixA.M21 * matrixB.M11 + matrixA.M22 * matrixB.M21 + matrixA.M23 * matrixB.M31,
			matrixA.M21 * matrixB.M12 + matrixA.M22 * matrixB.M22 + matrixA.M23 * matrixB.M32,
			matrixA.M21 * matrixB.M13 + matrixA.M22 * matrixB.M23 + matrixA.M23 * matrixB.M33,
			matrixA.M31 * matrixB.M11 + matrixA.M32 * matrixB.M21 + matrixA.M33 * matrixB.M31,
			matrixA.M31 * matrixB.M12 + matrixA.M32 * matrixB.M22 + matrixA.M33 * matrixB.M32,
			matrixA.M31 * matrixB.M13 + matrixA.M32 * matrixB.M23 + matrixA.M33 * matrixB.M33);
	}

	public Matrix3 Transpose()
	{
		return new(
			M11, M21, M31,
			M12, M22, M32,
			M13, M23, M33);
	}

	public float Determinant()
	{
		return M11 * M22 * M33 + M12 * M23 * M31 + M13 * M21 * M32 - M13 * M22 * M31 - M12 * M21 * M33 - M11 * M23 * M32;
	}

	public Matrix3 Minor()
	{
		Matrix3 result = default;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				result[i, j] = Matrix2.Cut(this, i, j).Determinant();
			}
		}

		return result;
	}

	public Matrix3 Cofactor()
	{
		Matrix3 minor = Minor();
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
}
