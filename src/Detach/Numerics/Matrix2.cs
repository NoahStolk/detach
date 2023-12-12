using System.Runtime.InteropServices;

namespace Detach.Numerics;

public record struct Matrix2 : IMatrixOperations<Matrix2>
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

	public Matrix2(Span<float> span)
	{
		if (span.Length != 4)
			throw new ArgumentException("Span must have length of 4.", nameof(span));

		M11 = span[0];
		M12 = span[1];
		M21 = span[2];
		M22 = span[3];
	}

	public static Matrix2 Identity => new(1, 0, 0, 1);
	public static int Rows => 2;
	public static int Cols => 2;

	public float this[int index]
	{
		get => Get(this, index);
		set => Set(ref this, index, value);
	}

	public float this[int row, int col]
	{
		get => Get(this, row, col);
		set => Set(ref this, row, col, value);
	}

	public static Matrix2 operator *(Matrix2 left, float right)
	{
		return new(
			left.M11 * right, left.M12 * right,
			left.M21 * right, left.M22 * right);
	}

	public static Matrix2 operator *(Matrix2 left, Matrix2 right)
	{
		return Matrices.Multiply<Matrix2, Matrix2, Matrix2>(left, right);
	}

	public Span<float> AsSpan()
	{
		return MemoryMarshal.CreateSpan(ref M11, 4);
	}

	public static Matrix2 Transpose(Matrix2 matrix)
	{
		return new(
			matrix.M11, matrix.M21,
			matrix.M12, matrix.M22);
	}

	public static float Determinant(Matrix2 matrix)
	{
		return matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21;
	}

	public static Matrix2 Minor(Matrix2 matrix)
	{
		return new(matrix.M22, matrix.M21, matrix.M12, matrix.M11);
	}

	public static Matrix2 Cofactor(Matrix2 matrix)
	{
		return Matrices.Cofactor<Matrix2, Matrix2>(Minor(matrix));
	}

	public static Matrix2 Adjugate(Matrix2 matrix)
	{
		return Matrices.Adjugate(matrix);
	}

	public static Matrix2 Inverse(Matrix2 matrix)
	{
		return Matrices.Inverse(matrix);
	}

	public static Matrix2 CreateDefault()
	{
		return default;
	}

	public static float Get(Matrix2 matrix, int index)
	{
		return index switch
		{
			0 => matrix.M11,
			1 => matrix.M12,
			2 => matrix.M21,
			3 => matrix.M22,
			_ => throw new IndexOutOfRangeException(),
		};
	}

	public static float Get(Matrix2 matrix, int row, int col)
	{
		return row switch
		{
			0 => col switch
			{
				0 => matrix.M11,
				1 => matrix.M12,
				_ => throw new IndexOutOfRangeException(),
			},
			1 => col switch
			{
				0 => matrix.M21,
				1 => matrix.M22,
				_ => throw new IndexOutOfRangeException(),
			},
			_ => throw new IndexOutOfRangeException(),
		};
	}

	public static void Set(ref Matrix2 matrix, int index, float value)
	{
		switch (index)
		{
			case 0: matrix.M11 = value; break;
			case 1: matrix.M12 = value; break;
			case 2: matrix.M21 = value; break;
			case 3: matrix.M22 = value; break;
			default: throw new IndexOutOfRangeException();
		}
	}

	public static void Set(ref Matrix2 matrix, int row, int col, float value)
	{
		switch (row)
		{
			case 0:
				switch (col)
				{
					case 0: matrix.M11 = value; break;
					case 1: matrix.M12 = value; break;
					default: throw new IndexOutOfRangeException();
				}

				break;
			case 1:
				switch (col)
				{
					case 0: matrix.M21 = value; break;
					case 1: matrix.M22 = value; break;
					default: throw new IndexOutOfRangeException();
				}

				break;
			default:
				throw new IndexOutOfRangeException();
		}
	}
}
