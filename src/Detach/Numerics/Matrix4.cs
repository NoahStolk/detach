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
		return new(
			left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31 + left.M14 * right.M41,
			left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32 + left.M14 * right.M42,
			left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33 + left.M14 * right.M43,
			left.M11 * right.M14 + left.M12 * right.M24 + left.M13 * right.M34 + left.M14 * right.M44,
			left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31 + left.M24 * right.M41,
			left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32 + left.M24 * right.M42,
			left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33 + left.M24 * right.M43,
			left.M21 * right.M14 + left.M22 * right.M24 + left.M23 * right.M34 + left.M24 * right.M44,
			left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31 + left.M34 * right.M41,
			left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32 + left.M34 * right.M42,
			left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33 + left.M34 * right.M43,
			left.M31 * right.M14 + left.M32 * right.M24 + left.M33 * right.M34 + left.M34 * right.M44,
			left.M41 * right.M11 + left.M42 * right.M21 + left.M43 * right.M31 + left.M44 * right.M41,
			left.M41 * right.M12 + left.M42 * right.M22 + left.M43 * right.M32 + left.M44 * right.M42,
			left.M41 * right.M13 + left.M42 * right.M23 + left.M43 * right.M33 + left.M44 * right.M43,
			left.M41 * right.M14 + left.M42 * right.M24 + left.M43 * right.M34 + left.M44 * right.M44);
	}
}
