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

	public static Matrix2 operator *(Matrix2 left, float right)
	{
		return new(
			left.M11 * right, left.M12 * right,
			left.M21 * right, left.M22 * right);
	}

	public static Matrix2 operator *(Matrix2 left, Matrix2 right)
	{
		return new(
			left.M11 * right.M11 + left.M12 * right.M21,
			left.M11 * right.M12 + left.M12 * right.M22,
			left.M21 * right.M11 + left.M22 * right.M21,
			left.M21 * right.M12 + left.M22 * right.M22);
	}
}
