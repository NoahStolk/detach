using Detach.Utils;
using System.Runtime.InteropServices;
using System.Text.Unicode;

namespace Detach.Numerics;

public record struct Matrix2 : IMatrixOperations<Matrix2>, ISpanFormattable, IUtf8SpanFormattable
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
		return new Matrix2(
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
		return new Matrix2(
			matrix.M11, matrix.M21,
			matrix.M12, matrix.M22);
	}

	public static float Determinant(Matrix2 matrix)
	{
		return matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21;
	}

	public static Matrix2 Minor(Matrix2 matrix)
	{
		return new Matrix2(matrix.M22, matrix.M21, matrix.M12, matrix.M11);
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

	public static Matrix2 FastInverse(Matrix2 matrix)
	{
		float determinant = matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21;
		if (determinant == 0)
			return Identity;

		float invDet = 1.0f / determinant;
		float m11 = matrix.M22 * invDet;
		float m12 = -matrix.M12 * invDet;
		float m21 = -matrix.M21 * invDet;
		float m22 = matrix.M11 * invDet;
		return new Matrix2(m11, m12, m21, m22);
	}

	public static Matrix2 Default()
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

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		bytesWritten = 0;
		return
			SpanFormattableUtils.TryFormatLiteral("<"u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M11, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M12, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral("> <"u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M21, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M22, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(">"u8, utf8Destination, ref bytesWritten);
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		charsWritten = 0;
		return
			SpanFormattableUtils.TryFormatLiteral("<", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M11, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M12, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral("> <", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M21, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M22, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(">", destination, ref charsWritten);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		FormattableString formattable = $"<{M11.ToString(format, formatProvider)}, {M12.ToString(format, formatProvider)}> <{M21.ToString(format, formatProvider)}, {M22.ToString(format, formatProvider)}>";
		return formattable.ToString(formatProvider);
	}

	public override string ToString()
	{
		return $"<{M11}, {M12}> <{M21}, {M22}>";
	}
}
