using System.Runtime.InteropServices;

namespace Detach.Numerics;

public record struct Matrix4 : IMatrixOperations<Matrix4>
{
	public float M11;
	public float M12;
	public float M13;
	public float M14;
	public float M21;
	public float M22;
	public float M23;
	public float M24;
	public float M31;
	public float M32;
	public float M33;
	public float M34;
	public float M41;
	public float M42;
	public float M43;
	public float M44;

	public Matrix4(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44)
	{
		M11 = m11;
		M12 = m12;
		M13 = m13;
		M14 = m14;
		M21 = m21;
		M22 = m22;
		M23 = m23;
		M24 = m24;
		M31 = m31;
		M32 = m32;
		M33 = m33;
		M34 = m34;
		M41 = m41;
		M42 = m42;
		M43 = m43;
		M44 = m44;
	}

	public Matrix4(Span<float> span)
	{
		if (span.Length != 16)
			throw new ArgumentException("Span must have length of 16.", nameof(span));

		M11 = span[0];
		M12 = span[1];
		M13 = span[2];
		M14 = span[3];
		M21 = span[4];
		M22 = span[5];
		M23 = span[6];
		M24 = span[7];
		M31 = span[8];
		M32 = span[9];
		M33 = span[10];
		M34 = span[11];
		M41 = span[12];
		M42 = span[13];
		M43 = span[14];
		M44 = span[15];
	}

	public static Matrix4 Identity => new(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
	public static int Rows => 4;
	public static int Cols => 4;

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

	public static Matrix4 operator *(Matrix4 left, float right)
	{
		return new(
			left.M11 * right, left.M12 * right, left.M13 * right, left.M14 * right,
			left.M21 * right, left.M22 * right, left.M23 * right, left.M24 * right,
			left.M31 * right, left.M32 * right, left.M33 * right, left.M34 * right,
			left.M41 * right, left.M42 * right, left.M43 * right, left.M44 * right);
	}

	public static Matrix4 operator *(Matrix4 left, Matrix4 right)
	{
		return Matrices.Multiply<Matrix4, Matrix4, Matrix4>(left, right);
	}

	public Span<float> AsSpan()
	{
		return MemoryMarshal.CreateSpan(ref M11, 16);
	}

	public static Matrix4 Transpose(Matrix4 matrix)
	{
		return new(
			matrix.M11, matrix.M21, matrix.M31, matrix.M41,
			matrix.M12, matrix.M22, matrix.M32, matrix.M42,
			matrix.M13, matrix.M23, matrix.M33, matrix.M43,
			matrix.M14, matrix.M24, matrix.M34, matrix.M44);
	}

	public static float Determinant(Matrix4 matrix)
	{
		return
			matrix.M11 * matrix.M22 * matrix.M33 * matrix.M44 + matrix.M11 * matrix.M23 * matrix.M34 * matrix.M42 + matrix.M11 * matrix.M24 * matrix.M32 * matrix.M43
			+ matrix.M12 * matrix.M21 * matrix.M34 * matrix.M43 + matrix.M12 * matrix.M23 * matrix.M31 * matrix.M44 + matrix.M12 * matrix.M24 * matrix.M33 * matrix.M41
			+ matrix.M13 * matrix.M21 * matrix.M32 * matrix.M44 + matrix.M13 * matrix.M22 * matrix.M34 * matrix.M41 + matrix.M13 * matrix.M24 * matrix.M31 * matrix.M42
			+ matrix.M14 * matrix.M21 * matrix.M33 * matrix.M42 + matrix.M14 * matrix.M22 * matrix.M31 * matrix.M43 + matrix.M14 * matrix.M23 * matrix.M32 * matrix.M41
			- matrix.M11 * matrix.M22 * matrix.M34 * matrix.M43 - matrix.M11 * matrix.M23 * matrix.M32 * matrix.M44 - matrix.M11 * matrix.M24 * matrix.M33 * matrix.M42
			- matrix.M12 * matrix.M21 * matrix.M33 * matrix.M44 - matrix.M12 * matrix.M23 * matrix.M34 * matrix.M41 - matrix.M12 * matrix.M24 * matrix.M31 * matrix.M43
			- matrix.M13 * matrix.M21 * matrix.M34 * matrix.M42 - matrix.M13 * matrix.M22 * matrix.M31 * matrix.M44 - matrix.M13 * matrix.M24 * matrix.M32 * matrix.M41
			- matrix.M14 * matrix.M21 * matrix.M32 * matrix.M43 - matrix.M14 * matrix.M22 * matrix.M33 * matrix.M41 - matrix.M14 * matrix.M23 * matrix.M31 * matrix.M42;
	}

	public static Matrix4 CreateDefault()
	{
		return default;
	}

	public static float Get(Matrix4 matrix, int index)
	{
		return index switch
		{
			0 => matrix.M11,
			1 => matrix.M12,
			2 => matrix.M13,
			3 => matrix.M14,
			4 => matrix.M21,
			5 => matrix.M22,
			6 => matrix.M23,
			7 => matrix.M24,
			8 => matrix.M31,
			9 => matrix.M32,
			10 => matrix.M33,
			11 => matrix.M34,
			12 => matrix.M41,
			13 => matrix.M42,
			14 => matrix.M43,
			15 => matrix.M44,
			_ => throw new IndexOutOfRangeException(),
		};
	}

	public static float Get(Matrix4 matrix, int row, int col)
	{
		return row switch
		{
			0 => col switch
			{
				0 => matrix.M11,
				1 => matrix.M12,
				2 => matrix.M13,
				3 => matrix.M14,
				_ => throw new IndexOutOfRangeException(),
			},
			1 => col switch
			{
				0 => matrix.M21,
				1 => matrix.M22,
				2 => matrix.M23,
				3 => matrix.M24,
				_ => throw new IndexOutOfRangeException(),
			},
			2 => col switch
			{
				0 => matrix.M31,
				1 => matrix.M32,
				2 => matrix.M33,
				3 => matrix.M34,
				_ => throw new IndexOutOfRangeException(),
			},
			3 => col switch
			{
				0 => matrix.M41,
				1 => matrix.M42,
				2 => matrix.M43,
				3 => matrix.M44,
				_ => throw new IndexOutOfRangeException(),
			},
			_ => throw new IndexOutOfRangeException(),
		};
	}

	public static void Set(ref Matrix4 matrix, int index, float value)
	{
		switch (index)
		{
			case 0: matrix.M11 = value; break;
			case 1: matrix.M12 = value; break;
			case 2: matrix.M13 = value; break;
			case 3: matrix.M14 = value; break;
			case 4: matrix.M21 = value; break;
			case 5: matrix.M22 = value; break;
			case 6: matrix.M23 = value; break;
			case 7: matrix.M24 = value; break;
			case 8: matrix.M31 = value; break;
			case 9: matrix.M32 = value; break;
			case 10: matrix.M33 = value; break;
			case 11: matrix.M34 = value; break;
			case 12: matrix.M41 = value; break;
			case 13: matrix.M42 = value; break;
			case 14: matrix.M43 = value; break;
			case 15: matrix.M44 = value; break;
			default: throw new IndexOutOfRangeException();
		}
	}

	public static void Set(ref Matrix4 matrix, int row, int col, float value)
	{
		switch (row)
		{
			case 0:
				switch (col)
				{
					case 0: matrix.M11 = value; break;
					case 1: matrix.M12 = value; break;
					case 2: matrix.M13 = value; break;
					case 3: matrix.M14 = value; break;
					default: throw new IndexOutOfRangeException();
				}

				break;
			case 1:
				switch (col)
				{
					case 0: matrix.M21 = value; break;
					case 1: matrix.M22 = value; break;
					case 2: matrix.M23 = value; break;
					case 3: matrix.M24 = value; break;
					default: throw new IndexOutOfRangeException();
				}

				break;
			case 2:
				switch (col)
				{
					case 0: matrix.M31 = value; break;
					case 1: matrix.M32 = value; break;
					case 2: matrix.M33 = value; break;
					case 3: matrix.M34 = value; break;
					default: throw new IndexOutOfRangeException();
				}

				break;
			case 3:
				switch (col)
				{
					case 0: matrix.M41 = value; break;
					case 1: matrix.M42 = value; break;
					case 2: matrix.M43 = value; break;
					case 3: matrix.M44 = value; break;
					default: throw new IndexOutOfRangeException();
				}

				break;
		}
	}
}
