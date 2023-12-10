namespace Detach.Numerics;

public struct Matrix4
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

	public static Matrix4 Identity => new(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

	public float this[int row, int col]
	{
		get => row switch
		{
			0 => col switch
			{
				0 => M11,
				1 => M12,
				2 => M13,
				3 => M14,
				_ => throw new IndexOutOfRangeException(),
			},
			1 => col switch
			{
				0 => M21,
				1 => M22,
				2 => M23,
				3 => M24,
				_ => throw new IndexOutOfRangeException(),
			},
			2 => col switch
			{
				0 => M31,
				1 => M32,
				2 => M33,
				3 => M34,
				_ => throw new IndexOutOfRangeException(),
			},
			3 => col switch
			{
				0 => M41,
				1 => M42,
				2 => M43,
				3 => M44,
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
						case 3: M14 = value; break;
						default: throw new IndexOutOfRangeException();
					}

					break;
				case 1:
					switch (col)
					{
						case 0: M21 = value; break;
						case 1: M22 = value; break;
						case 2: M23 = value; break;
						case 3: M24 = value; break;
						default: throw new IndexOutOfRangeException();
					}

					break;
				case 2:
					switch (col)
					{
						case 0: M31 = value; break;
						case 1: M32 = value; break;
						case 2: M33 = value; break;
						case 3: M34 = value; break;
						default: throw new IndexOutOfRangeException();
					}

					break;
				case 3:
					switch (col)
					{
						case 0: M41 = value; break;
						case 1: M42 = value; break;
						case 2: M43 = value; break;
						case 3: M44 = value; break;
						default: throw new IndexOutOfRangeException();
					}

					break;
			}
		}
	}

	public static Matrix4 operator *(Matrix4 matrix, float scalar)
	{
		return new(
			matrix.M11 * scalar, matrix.M12 * scalar, matrix.M13 * scalar, matrix.M14 * scalar,
			matrix.M21 * scalar, matrix.M22 * scalar, matrix.M23 * scalar, matrix.M24 * scalar,
			matrix.M31 * scalar, matrix.M32 * scalar, matrix.M33 * scalar, matrix.M34 * scalar,
			matrix.M41 * scalar, matrix.M42 * scalar, matrix.M43 * scalar, matrix.M44 * scalar);
	}

	public static Matrix4 operator *(Matrix4 matrixA, Matrix4 matrixB)
	{
		return new(
			matrixA.M11 * matrixB.M11 + matrixA.M12 * matrixB.M21 + matrixA.M13 * matrixB.M31 + matrixA.M14 * matrixB.M41,
			matrixA.M11 * matrixB.M12 + matrixA.M12 * matrixB.M22 + matrixA.M13 * matrixB.M32 + matrixA.M14 * matrixB.M42,
			matrixA.M11 * matrixB.M13 + matrixA.M12 * matrixB.M23 + matrixA.M13 * matrixB.M33 + matrixA.M14 * matrixB.M43,
			matrixA.M11 * matrixB.M14 + matrixA.M12 * matrixB.M24 + matrixA.M13 * matrixB.M34 + matrixA.M14 * matrixB.M44,
			matrixA.M21 * matrixB.M11 + matrixA.M22 * matrixB.M21 + matrixA.M23 * matrixB.M31 + matrixA.M24 * matrixB.M41,
			matrixA.M21 * matrixB.M12 + matrixA.M22 * matrixB.M22 + matrixA.M23 * matrixB.M32 + matrixA.M24 * matrixB.M42,
			matrixA.M21 * matrixB.M13 + matrixA.M22 * matrixB.M23 + matrixA.M23 * matrixB.M33 + matrixA.M24 * matrixB.M43,
			matrixA.M21 * matrixB.M14 + matrixA.M22 * matrixB.M24 + matrixA.M23 * matrixB.M34 + matrixA.M24 * matrixB.M44,
			matrixA.M31 * matrixB.M11 + matrixA.M32 * matrixB.M21 + matrixA.M33 * matrixB.M31 + matrixA.M34 * matrixB.M41,
			matrixA.M31 * matrixB.M12 + matrixA.M32 * matrixB.M22 + matrixA.M33 * matrixB.M32 + matrixA.M34 * matrixB.M42,
			matrixA.M31 * matrixB.M13 + matrixA.M32 * matrixB.M23 + matrixA.M33 * matrixB.M33 + matrixA.M34 * matrixB.M43,
			matrixA.M31 * matrixB.M14 + matrixA.M32 * matrixB.M24 + matrixA.M33 * matrixB.M34 + matrixA.M34 * matrixB.M44,
			matrixA.M41 * matrixB.M11 + matrixA.M42 * matrixB.M21 + matrixA.M43 * matrixB.M31 + matrixA.M44 * matrixB.M41,
			matrixA.M41 * matrixB.M12 + matrixA.M42 * matrixB.M22 + matrixA.M43 * matrixB.M32 + matrixA.M44 * matrixB.M42,
			matrixA.M41 * matrixB.M13 + matrixA.M42 * matrixB.M23 + matrixA.M43 * matrixB.M33 + matrixA.M44 * matrixB.M43,
			matrixA.M41 * matrixB.M14 + matrixA.M42 * matrixB.M24 + matrixA.M43 * matrixB.M34 + matrixA.M44 * matrixB.M44);
	}

	public Matrix4 Transpose()
	{
		return new(
			M11, M21, M31, M41,
			M12, M22, M32, M42,
			M13, M23, M33, M43,
			M14, M24, M34, M44);
	}

	public float Determinant()
	{
		return
			M11 * M22 * M33 * M44 + M11 * M23 * M34 * M42 + M11 * M24 * M32 * M43
			+ M12 * M21 * M34 * M43 + M12 * M23 * M31 * M44 + M12 * M24 * M33 * M41
			+ M13 * M21 * M32 * M44 + M13 * M22 * M34 * M41 + M13 * M24 * M31 * M42
			+ M14 * M21 * M33 * M42 + M14 * M22 * M31 * M43 + M14 * M23 * M32 * M41
			- M11 * M22 * M34 * M43 - M11 * M23 * M32 * M44 - M11 * M24 * M33 * M42
			- M12 * M21 * M33 * M44 - M12 * M23 * M34 * M41 - M12 * M24 * M31 * M43
			- M13 * M21 * M34 * M42 - M13 * M22 * M31 * M44 - M13 * M24 * M32 * M41
			- M14 * M21 * M32 * M43 - M14 * M22 * M33 * M41 - M14 * M23 * M31 * M42;
	}
}
