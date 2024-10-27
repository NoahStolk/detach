using Detach.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Unicode;

namespace Detach.Numerics;

public record struct Matrix4 : IMatrixOperations<Matrix4>, ISpanFormattable, IUtf8SpanFormattable
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

	public Vector3 Translation
	{
		get => new(M41, M42, M43);
		set
		{
			M41 = value.X;
			M42 = value.Y;
			M43 = value.Z;
		}
	}

	public Vector3 Scale
	{
		get => new(M11, M22, M33);
		set
		{
			M11 = value.X;
			M22 = value.Y;
			M33 = value.Z;
		}
	}

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
		return new Matrix4(
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
		return new Matrix4(
			matrix.M11, matrix.M21, matrix.M31, matrix.M41,
			matrix.M12, matrix.M22, matrix.M32, matrix.M42,
			matrix.M13, matrix.M23, matrix.M33, matrix.M43,
			matrix.M14, matrix.M24, matrix.M34, matrix.M44);
	}

	public static float Determinant(Matrix4 matrix)
	{
		float result = 0;
		Matrix4 cofactor = Cofactor(matrix);
		for (int i = 0; i < 4; i++)
			result += matrix[i] * cofactor[0, i];

		return result;
	}

	public static Matrix4 Minor(Matrix4 matrix)
	{
		Matrix4 result = default;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
				result[i, j] = Matrix3.Determinant(Matrices.Cut(matrix, i, j));
		}

		return result;
	}

	public static Matrix4 Cofactor(Matrix4 matrix)
	{
		return Matrices.Cofactor<Matrix4, Matrix4>(Minor(matrix));
	}

	public static Matrix4 Adjugate(Matrix4 matrix)
	{
		return Matrices.Adjugate(matrix);
	}

	public static Matrix4 Inverse(Matrix4 matrix)
	{
		return Matrices.Inverse(matrix);
	}

	public static Matrix4 CreateTranslation(float x, float y, float z)
	{
		return new Matrix4(
			1, 0, 0, 0,
			0, 1, 0, 0,
			0, 0, 1, 0,
			x, y, z, 1);
	}

	public static Matrix4 CreateTranslation(Vector3 translation)
	{
		return CreateTranslation(translation.X, translation.Y, translation.Z);
	}

	public static Matrix4 CreateScale(float x, float y, float z)
	{
		return new Matrix4(
			x, 0, 0, 0,
			0, y, 0, 0,
			0, 0, z, 0,
			0, 0, 0, 1);
	}

	public static Matrix4 CreateScale(Vector3 scale)
	{
		return CreateScale(scale.X, scale.Y, scale.Z);
	}

	public static Matrix4 Rotation(float yaw, float pitch, float roll)
	{
		return RotationZ(roll) * RotationX(pitch) * RotationY(yaw);
	}

	public static Matrix4 RotationZ(float angleInRadians)
	{
		float sin = MathF.Sin(angleInRadians);
		float cos = MathF.Cos(angleInRadians);
		return new Matrix4(
			cos, sin, 0, 0,
			-sin, cos, 0, 0,
			0, 0, 1, 0,
			0, 0, 0, 1);
	}

	public static Matrix4 RotationX(float angleInRadians)
	{
		float sin = MathF.Sin(angleInRadians);
		float cos = MathF.Cos(angleInRadians);
		return new Matrix4(
			1, 0, 0, 0,
			0, cos, sin, 0,
			0, -sin, cos, 0,
			0, 0, 0, 1);
	}

	public static Matrix4 RotationY(float angleInRadians)
	{
		float sin = MathF.Sin(angleInRadians);
		float cos = MathF.Cos(angleInRadians);
		return new Matrix4(
			cos, 0, -sin, 0,
			0, 1, 0, 0,
			sin, 0, cos, 0,
			0, 0, 0, 1);
	}

	public static Matrix4 AxisAngle(Vector3 axis, float angleInRadians)
	{
		float sin = MathF.Sin(angleInRadians);
		float cos = MathF.Cos(angleInRadians);
		float t = 1 - cos;

		axis = Vector3.Normalize(axis);

		float x = axis.X;
		float y = axis.Y;
		float z = axis.Z;

		return new Matrix4(
			t * x * x + cos, t * x * y + sin * z, t * x * z - sin * y, 0,
			t * x * y - sin * z, t * y * y + cos, t * y * z + sin * x, 0,
			t * x * z + sin * y, t * y * z - sin * x, t * z * z + cos, 0,
			0, 0, 0, 1);
	}

	public static Matrix4 Transform(Vector3 scale, Vector3 rotationInRadians, Vector3 translation)
	{
		return CreateScale(scale) * Rotation(rotationInRadians.X, rotationInRadians.Y, rotationInRadians.Z) * CreateTranslation(translation);
	}

	public static Matrix4 Transform(Vector3 scale, Vector3 rotationAxis, float rotationAngleInRadians, Vector3 translation)
	{
		return CreateScale(scale) * AxisAngle(rotationAxis, rotationAngleInRadians) * CreateTranslation(translation);
	}

	public static Matrix4 LookAt(Vector3 position, Vector3 target, Vector3 up)
	{
		Vector3 zAxis = Vector3.Normalize(target - position);
		Vector3 xAxis = Vector3.Normalize(Vector3.Cross(up, zAxis));
		Vector3 yAxis = Vector3.Normalize(Vector3.Cross(zAxis, xAxis));

		return new Matrix4(
			xAxis.X, yAxis.X, zAxis.X, 0,
			xAxis.Y, yAxis.Y, zAxis.Y, 0,
			xAxis.Z, yAxis.Z, zAxis.Z, 0,
			-Vector3.Dot(xAxis, position), -Vector3.Dot(yAxis, position), -Vector3.Dot(zAxis, position), 1);
	}

	public static Matrix4 Projection(float fovInRadians, float aspectRatio, float near, float far)
	{
		float yScale = 1 / MathF.Tan(fovInRadians / 2);
		float xScale = yScale / aspectRatio;
		float zScale = far / (far - near);
		float zTranslation = -near * zScale;

		return new Matrix4(
			xScale, 0, 0, 0,
			0, yScale, 0, 0,
			0, 0, zScale, 1,
			0, 0, zTranslation, 0);
	}

	public static Matrix4 Orthographic(float left, float right, float bottom, float top, float near, float far)
	{
		float xScale = 2 / (right - left);
		float yScale = 2 / (top - bottom);
		float zScale = 1 / (far - near);
		float xTranslation = (left + right) / (left - right);
		float yTranslation = (top + bottom) / (bottom - top);
		float zTranslation = near / (near - far);

		return new Matrix4(
			xScale, 0, 0, 0,
			0, yScale, 0, 0,
			0, 0, zScale, 0,
			xTranslation, yTranslation, zTranslation, 1);
	}

	public static Matrix4 Default()
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

	public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		bytesWritten = 0;
		return
			SpanFormattableUtils.TryFormatLiteral("<"u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M11, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M12, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M13, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M14, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral("> <"u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M21, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M22, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M23, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M24, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral("> <"u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M31, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M32, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M33, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M34, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral("> <"u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M41, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M42, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M43, utf8Destination, ref bytesWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", "u8, utf8Destination, ref bytesWritten) &&
			SpanFormattableUtils.TryFormat(M44, utf8Destination, ref bytesWritten, format, provider) &&
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
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M13, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M14, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral("> <", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M21, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M22, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M23, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M24, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral("> <", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M31, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M32, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M33, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M34, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral("> <", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M41, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M42, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M43, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(", ", destination, ref charsWritten) &&
			SpanFormattableUtils.TryFormat(M44, destination, ref charsWritten, format, provider) &&
			SpanFormattableUtils.TryFormatLiteral(">", destination, ref charsWritten);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		FormattableString formattable = FormattableStringFactory.Create(
			"<{0}, {1}, {2}, {3}> <{4}, {5}, {6}, {7}> <{8}, {9}, {10}, {11}> <{12}, {13}, {14}, {15}>",
			M11.ToString(format, formatProvider),
			M12.ToString(format, formatProvider),
			M13.ToString(format, formatProvider),
			M14.ToString(format, formatProvider),
			M21.ToString(format, formatProvider),
			M22.ToString(format, formatProvider),
			M23.ToString(format, formatProvider),
			M24.ToString(format, formatProvider),
			M31.ToString(format, formatProvider),
			M32.ToString(format, formatProvider),
			M33.ToString(format, formatProvider),
			M34.ToString(format, formatProvider),
			M41.ToString(format, formatProvider),
			M42.ToString(format, formatProvider),
			M43.ToString(format, formatProvider),
			M44.ToString(format, formatProvider));
		return formattable.ToString(formatProvider);
	}

	public override string ToString()
	{
		return $"<{M11}, {M12}, {M13}, {M14}> <{M21}, {M22}, {M23}, {M24}> <{M31}, {M32}, {M33}, {M34}> <{M41}, {M42}, {M43}, {M44}>";
	}
}
