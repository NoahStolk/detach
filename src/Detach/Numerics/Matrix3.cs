using System.Numerics;
using System.Runtime.InteropServices;

namespace Detach.Numerics;

public record struct Matrix3 : IMatrixOperations<Matrix3>
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

	public Matrix3(Span<float> span)
	{
		if (span.Length != 9)
			throw new ArgumentException("Span must have length of 9.", nameof(span));

		M11 = span[0];
		M12 = span[1];
		M13 = span[2];
		M21 = span[3];
		M22 = span[4];
		M23 = span[5];
		M31 = span[6];
		M32 = span[7];
		M33 = span[8];
	}

	public static Matrix3 Identity => new(1, 0, 0, 0, 1, 0, 0, 0, 1);
	public static int Rows => 3;
	public static int Cols => 3;

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

	public static Matrix3 operator *(Matrix3 left, float right)
	{
		return new Matrix3(
			left.M11 * right, left.M12 * right, left.M13 * right,
			left.M21 * right, left.M22 * right, left.M23 * right,
			left.M31 * right, left.M32 * right, left.M33 * right);
	}

	public static Matrix3 operator *(Matrix3 left, Matrix3 right)
	{
		return Matrices.Multiply<Matrix3, Matrix3, Matrix3>(left, right);
	}

	public Span<float> AsSpan()
	{
		return MemoryMarshal.CreateSpan(ref M11, 9);
	}

	public static Matrix3 Transpose(Matrix3 matrix)
	{
		return new Matrix3(
			matrix.M11, matrix.M21, matrix.M31,
			matrix.M12, matrix.M22, matrix.M32,
			matrix.M13, matrix.M23, matrix.M33);
	}

	public static float Determinant(Matrix3 matrix)
	{
		float result = 0;
		Matrix3 cofactor = Cofactor(matrix);
		for (int i = 0; i < 3; i++)
			result += matrix[i] * cofactor[0, i];

		return result;
	}

	public static Matrix3 Minor(Matrix3 matrix)
	{
		Matrix3 result = default;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
				result[i, j] = Matrix2.Determinant(Matrices.Cut(matrix, i, j));
		}

		return result;
	}

	public static Matrix3 Cofactor(Matrix3 matrix)
	{
		return Matrices.Cofactor<Matrix3, Matrix3>(Minor(matrix));
	}

	public static Matrix3 Adjugate(Matrix3 matrix)
	{
		return Matrices.Adjugate(matrix);
	}

	public static Matrix3 Inverse(Matrix3 matrix)
	{
		return Matrices.Inverse(matrix);
	}

	public static Matrix3 Rotation(float yaw, float pitch, float roll)
	{
		return RotationZ(roll) * RotationX(pitch) * RotationY(yaw);
	}

	public static Matrix3 RotationZ(float angleInRadians)
	{
		float sin = MathF.Sin(angleInRadians);
		float cos = MathF.Cos(angleInRadians);
		return new Matrix3(
			cos, sin, 0,
			-sin, cos, 0,
			0, 0, 1);
	}

	public static Matrix3 RotationX(float angleInRadians)
	{
		float sin = MathF.Sin(angleInRadians);
		float cos = MathF.Cos(angleInRadians);
		return new Matrix3(
			1, 0, 0,
			0, cos, sin,
			0, -sin, cos);
	}

	public static Matrix3 RotationY(float angleInRadians)
	{
		float sin = MathF.Sin(angleInRadians);
		float cos = MathF.Cos(angleInRadians);
		return new Matrix3(
			cos, 0, -sin,
			0, 1, 0,
			sin, 0, cos);
	}

	public static Matrix3 AxisAngle(Vector3 axis, float angleInRadians)
	{
		float sin = MathF.Sin(angleInRadians);
		float cos = MathF.Cos(angleInRadians);
		float t = 1 - cos;

		axis = Vector3.Normalize(axis);

		float x = axis.X;
		float y = axis.Y;
		float z = axis.Z;

		return new Matrix3(
			t * x * x + cos, t * x * y + sin * z, t * x * z - sin * y,
			t * x * y - sin * z, t * y * y + cos, t * y * z + sin * x,
			t * x * z + sin * y, t * y * z - sin * x, t * z * z + cos);
	}

	public static Matrix3 Default()
	{
		return default;
	}

	public static float Get(Matrix3 matrix, int index)
	{
		return index switch
		{
			0 => matrix.M11,
			1 => matrix.M12,
			2 => matrix.M13,
			3 => matrix.M21,
			4 => matrix.M22,
			5 => matrix.M23,
			6 => matrix.M31,
			7 => matrix.M32,
			8 => matrix.M33,
			_ => throw new IndexOutOfRangeException(),
		};
	}

	public static float Get(Matrix3 matrix, int row, int col)
	{
		return row switch
		{
			0 => col switch
			{
				0 => matrix.M11,
				1 => matrix.M12,
				2 => matrix.M13,
				_ => throw new IndexOutOfRangeException(),
			},
			1 => col switch
			{
				0 => matrix.M21,
				1 => matrix.M22,
				2 => matrix.M23,
				_ => throw new IndexOutOfRangeException(),
			},
			2 => col switch
			{
				0 => matrix.M31,
				1 => matrix.M32,
				2 => matrix.M33,
				_ => throw new IndexOutOfRangeException(),
			},
			_ => throw new IndexOutOfRangeException(),
		};
	}

	public static void Set(ref Matrix3 matrix, int index, float value)
	{
		switch (index)
		{
			case 0: matrix.M11 = value; break;
			case 1: matrix.M12 = value; break;
			case 2: matrix.M13 = value; break;
			case 3: matrix.M21 = value; break;
			case 4: matrix.M22 = value; break;
			case 5: matrix.M23 = value; break;
			case 6: matrix.M31 = value; break;
			case 7: matrix.M32 = value; break;
			case 8: matrix.M33 = value; break;
			default: throw new IndexOutOfRangeException();
		}
	}

	public static void Set(ref Matrix3 matrix, int row, int col, float value)
	{
		switch (row)
		{
			case 0:
				switch (col)
				{
					case 0: matrix.M11 = value; break;
					case 1: matrix.M12 = value; break;
					case 2: matrix.M13 = value; break;
					default: throw new IndexOutOfRangeException();
				}

				break;
			case 1:
				switch (col)
				{
					case 0: matrix.M21 = value; break;
					case 1: matrix.M22 = value; break;
					case 2: matrix.M23 = value; break;
					default: throw new IndexOutOfRangeException();
				}

				break;
			case 2:
				switch (col)
				{
					case 0: matrix.M31 = value; break;
					case 1: matrix.M32 = value; break;
					case 2: matrix.M33 = value; break;
					default: throw new IndexOutOfRangeException();
				}

				break;
			default:
				throw new IndexOutOfRangeException();
		}
	}
}
