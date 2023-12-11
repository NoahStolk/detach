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

	public static Matrix3 operator *(Matrix3 left, float right)
	{
		return new(
			left.M11 * right, left.M12 * right, left.M13 * right,
			left.M21 * right, left.M22 * right, left.M23 * right,
			left.M31 * right, left.M32 * right, left.M33 * right);
	}

	public static Matrix3 operator *(Matrix3 left, Matrix3 right)
	{
		return new(
			left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31,
			left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32,
			left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33,
			left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31,
			left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32,
			left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33,
			left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31,
			left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32,
			left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33);
	}
}
