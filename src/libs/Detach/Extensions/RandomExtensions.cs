using Detach.Numerics;
using System.Numerics;

namespace Detach.Extensions;

public static class RandomExtensions
{
	/// <summary>
	/// Returns a random <see cref="byte"/> that is greater than or equal to 0, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="byte"/>.</returns>
	public static byte RandomByte(this Random random, byte maxValue)
	{
		return (byte)random.RandomInt(maxValue);
	}

	/// <summary>
	/// Returns a random <see cref="byte"/> that is greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="byte"/>.</returns>
	public static byte RandomByte(this Random random, byte minValue, byte maxValue)
	{
		return (byte)random.RandomInt(minValue, maxValue);
	}

	/// <summary>
	/// Returns a random <see cref="int"/> that is greater than or equal to 0, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="int"/>.</returns>
	public static int RandomInt(this Random random, int maxValue)
	{
		return random.Next(maxValue);
	}

	/// <summary>
	/// Returns a random <see cref="int"/> that is greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="int"/>.</returns>
	public static int RandomInt(this Random random, int minValue, int maxValue)
#pragma warning disable S2234 // Parameters should be passed in the correct order
		=> minValue > maxValue ? random.Next(maxValue, minValue) : random.Next(minValue, maxValue);
#pragma warning restore S2234 // Parameters should be passed in the correct order

	/// <summary>
	/// Returns a random <see cref="float"/> that is greater than or equal to 0.0f, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="float"/>.</returns>
	public static float RandomFloat(this Random random, float maxValue)
	{
		return (float)random.NextDouble() * maxValue;
	}

	/// <summary>
	/// Returns a random <see cref="float"/> that is greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="float"/>.</returns>
	public static float RandomFloat(this Random random, float minValue, float maxValue)
	{
		return (float)random.NextDouble() * (maxValue - minValue) + minValue;
	}

	/// <summary>
	/// Returns a random <see cref="double"/> that is greater than or equal to 0.0, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="double"/>.</returns>
	public static double RandomDouble(this Random random, double maxValue)
	{
		return random.NextDouble() * maxValue;
	}

	/// <summary>
	/// Returns a random <see cref="double"/> that is greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="double"/>.</returns>
	public static double RandomDouble(this Random random, double minValue, double maxValue)
	{
		return random.NextDouble() * (maxValue - minValue) + minValue;
	}

	/// <summary>
	/// Returns a random item from the given <paramref name="options"/> span.
	/// </summary>
	/// <typeparam name="T">The type of the items in the span.</typeparam>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="options">The span to choose from.</param>
	/// <returns>The randomly chosen item.</returns>
	public static T Choose<T>(this Random random, params Span<T> options)
	{
		if (options.Length == 0)
			throw new ArgumentException("options cannot be empty", nameof(options));

		return options[random.Next(options.Length)];
	}

	/// <summary>
	/// Returns true or false based on the <paramref name="percentage"/> parameter.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="percentage">The percentage ranging from 0 to 100.</param>
	public static bool Chance(this Random random, float percentage)
	{
		return random.RandomFloat(100) < percentage;
	}

	/// <summary>
	/// Returns a random <see cref="Vector2"/> greater than or equal to <see cref="Vector2.Zero"/>, and less than a <see cref="Vector2"/> with axes that do not exceed <paramref name="maxValue"/>. Both axes for the returned <see cref="Vector2"/> are equal.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector2"/>.</returns>
	public static Vector2 RandomEqualVector2(this Random random, float maxValue)
	{
		return new Vector2(random.RandomFloat(maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector2"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>. Both axes for the returned <see cref="Vector2"/> are equal.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector2"/>.</returns>
	public static Vector2 RandomEqualVector2(this Random random, float minValue, float maxValue)
	{
		return new Vector2(random.RandomFloat(minValue, maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector2"/> greater than or equal to <see cref="Vector2.Zero"/>, and less than a <see cref="Vector2"/> with axes that do not exceed <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector2"/>.</returns>
	public static Vector2 RandomVector2(this Random random, float maxValue)
	{
		return new Vector2(random.RandomFloat(maxValue), random.RandomFloat(maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector2"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector2"/>.</returns>
	public static Vector2 RandomVector2(this Random random, float minValue, float maxValue)
	{
		return new Vector2(random.RandomFloat(minValue, maxValue), random.RandomFloat(minValue, maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector2"/> with axes that are greater than or equal to the corresponding min parameters, and less than the corresponding max parameters.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValueX">The minimum X value for the <see cref="Vector2"/>.</param>
	/// <param name="maxValueX">The maximum X value for the <see cref="Vector2"/>.</param>
	/// <param name="minValueY">The minimum Y value for the <see cref="Vector2"/>.</param>
	/// <param name="maxValueY">The maximum Y value for the <see cref="Vector2"/>.</param>
	/// <returns>The random <see cref="Vector2"/>.</returns>
	public static Vector2 RandomVector2(this Random random, float minValueX, float maxValueX, float minValueY, float maxValueY)
	{
		return new Vector2(random.RandomFloat(minValueX, maxValueX), random.RandomFloat(minValueY, maxValueY));
	}

	/// <summary>
	/// Returns a random <see cref="Vector2"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector2"/>.</returns>
	public static Vector2 RandomVector2(this Random random, Vector2 minValue, Vector2 maxValue)
	{
		return RandomVector2(random, minValue.X, maxValue.X, minValue.Y, maxValue.Y);
	}

	/// <summary>
	/// Returns a random <see cref="Vector3"/> greater than or equal to <see cref="Vector3.Zero"/>, and less than a <see cref="Vector3"/> with axes that do not exceed <paramref name="maxValue"/>. All axes for the returned <see cref="Vector3"/> are equal.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomEqualVector3(this Random random, float maxValue)
	{
		return new Vector3(random.RandomFloat(maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector3"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>. All axes for the returned <see cref="Vector3"/> are equal.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomEqualVector3(this Random random, float minValue, float maxValue)
	{
		return new Vector3(random.RandomFloat(minValue, maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector3"/> greater than or equal to <see cref="Vector3.Zero"/>, and less than a <see cref="Vector3"/> with axes that do not exceed <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomVector3(this Random random, float maxValue)
	{
		return new Vector3(random.RandomFloat(maxValue), random.RandomFloat(maxValue), random.RandomFloat(maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector3"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomVector3(this Random random, float minValue, float maxValue)
	{
		return new Vector3(random.RandomFloat(minValue, maxValue), random.RandomFloat(minValue, maxValue), random.RandomFloat(minValue, maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector3"/> greater than or equal to a <see cref="Vector3"/> with axes {-<paramref name="x"/>, -<paramref name="y"/>, -<paramref name="z"/>}, and less than a <see cref="Vector3"/> with axes {<paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/>}.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="x">The X value for the <see cref="Vector3"/>.</param>
	/// <param name="y">The Y value for the <see cref="Vector3"/>.</param>
	/// <param name="z">The Z value for the <see cref="Vector3"/>.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomVector3(this Random random, float x, float y, float z)
	{
		return new Vector3(random.RandomFloat(-x, x), random.RandomFloat(-y, y), random.RandomFloat(-z, z));
	}

	/// <summary>
	/// Returns a random <see cref="Vector3"/> with axes that are greater than or equal to the corresponding min parameters, and less than the corresponding max parameters.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValueX">The minimum X value for the <see cref="Vector3"/>.</param>
	/// <param name="maxValueX">The maximum X value for the <see cref="Vector3"/>.</param>
	/// <param name="minValueY">The minimum Y value for the <see cref="Vector3"/>.</param>
	/// <param name="maxValueY">The maximum Y value for the <see cref="Vector3"/>.</param>
	/// <param name="minValueZ">The minimum Z value for the <see cref="Vector3"/>.</param>
	/// <param name="maxValueZ">The maximum Z value for the <see cref="Vector3"/>.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomVector3(this Random random, float minValueX, float maxValueX, float minValueY, float maxValueY, float minValueZ, float maxValueZ)
	{
		return new Vector3(random.RandomFloat(minValueX, maxValueX), random.RandomFloat(minValueY, maxValueY), random.RandomFloat(minValueZ, maxValueZ));
	}

	/// <summary>
	/// Returns a random <see cref="Vector3"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector3"/>.</returns>
	public static Vector3 RandomVector3(this Random random, Vector3 minValue, Vector3 maxValue)
	{
		return RandomVector3(random, minValue.X, maxValue.X, minValue.Y, maxValue.Y, minValue.Z, maxValue.Z);
	}

	/// <summary>
	/// Returns a random <see cref="Vector4"/> greater than or equal to <see cref="Vector4.Zero"/>, and less than a <see cref="Vector4"/> with axes that do not exceed <paramref name="maxValue"/>. All axes for the returned <see cref="Vector4"/> are equal.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomEqualVector4(this Random random, float maxValue)
	{
		return new Vector4(random.RandomFloat(maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector4"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>. All axes for the returned <see cref="Vector4"/> are equal.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomEqualVector4(this Random random, float minValue, float maxValue)
	{
		return new Vector4(random.RandomFloat(minValue, maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector4"/> greater than or equal to <see cref="Vector4.Zero"/>, and less than a <see cref="Vector4"/> with axes that do not exceed <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomVector4(this Random random, float maxValue)
	{
		return new Vector4(random.RandomFloat(maxValue), random.RandomFloat(maxValue), random.RandomFloat(maxValue), random.RandomFloat(maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector4"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomVector4(this Random random, float minValue, float maxValue)
	{
		return new Vector4(random.RandomFloat(minValue, maxValue), random.RandomFloat(minValue, maxValue), random.RandomFloat(minValue, maxValue), random.RandomFloat(minValue, maxValue));
	}

	/// <summary>
	/// Returns a random <see cref="Vector4"/> greater than or equal to a <see cref="Vector4"/> with axes {-<paramref name="x"/>, -<paramref name="y"/>, -<paramref name="z"/>, -<paramref name="w"/>}, and less than a <see cref="Vector4"/> with axes {<paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/>, <paramref name="w"/>}.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="x">The X value for the <see cref="Vector4"/>.</param>
	/// <param name="y">The Y value for the <see cref="Vector4"/>.</param>
	/// <param name="z">The Z value for the <see cref="Vector4"/>.</param>
	/// <param name="w">The W value for the <see cref="Vector4"/>.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomVector4(this Random random, float x, float y, float z, float w)
	{
		return new Vector4(random.RandomFloat(-x, x), random.RandomFloat(-y, y), random.RandomFloat(-z, z), random.RandomFloat(-w, w));
	}

	/// <summary>
	/// Returns a random <see cref="Vector4"/> with axes that are greater than or equal to the corresponding min parameters, and less than the corresponding max parameters.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValueX">The minimum X value for the <see cref="Vector4"/>.</param>
	/// <param name="maxValueX">The maximum X value for the <see cref="Vector4"/>.</param>
	/// <param name="minValueY">The minimum Y value for the <see cref="Vector4"/>.</param>
	/// <param name="maxValueY">The maximum Y value for the <see cref="Vector4"/>.</param>
	/// <param name="minValueZ">The minimum Z value for the <see cref="Vector4"/>.</param>
	/// <param name="maxValueZ">The maximum Z value for the <see cref="Vector4"/>.</param>
	/// <param name="minValueW">The minimum W value for the <see cref="Vector4"/>.</param>
	/// <param name="maxValueW">The maximum W value for the <see cref="Vector4"/>.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomVector4(this Random random, float minValueX, float maxValueX, float minValueY, float maxValueY, float minValueZ, float maxValueZ, float minValueW, float maxValueW)
	{
		return new Vector4(random.RandomFloat(minValueX, maxValueX), random.RandomFloat(minValueY, maxValueY), random.RandomFloat(minValueZ, maxValueZ), random.RandomFloat(minValueW, maxValueW));
	}

	/// <summary>
	/// Returns a random <see cref="Vector4"/> with axes that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Vector4"/>.</returns>
	public static Vector4 RandomVector4(this Random random, Vector4 minValue, Vector4 maxValue)
	{
		return RandomVector4(random, minValue.X, maxValue.X, minValue.Y, maxValue.Y, minValue.Z, maxValue.Z, minValue.W, maxValue.W);
	}

	/// <summary>
	/// Returns a random <see cref="Rgb"/> with values that are greater than or equal to the corresponding min parameters, and less than the corresponding max parameters.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValueR">The minimum R value for the <see cref="Rgb"/>.</param>
	/// <param name="maxValueR">The maximum R value for the <see cref="Rgb"/>.</param>
	/// <param name="minValueG">The minimum G value for the <see cref="Rgb"/>.</param>
	/// <param name="maxValueG">The maximum G value for the <see cref="Rgb"/>.</param>
	/// <param name="minValueB">The minimum B value for the <see cref="Rgb"/>.</param>
	/// <param name="maxValueB">The maximum B value for the <see cref="Rgb"/>.</param>
	/// <returns>The random <see cref="Rgb"/>.</returns>
	public static Rgb RandomRgb(this Random random, byte minValueR, byte maxValueR, byte minValueG, byte maxValueG, byte minValueB, byte maxValueB)
	{
		return new Rgb(random.RandomByte(minValueR, maxValueR), random.RandomByte(minValueG, maxValueG), random.RandomByte(minValueB, maxValueB));
	}

	/// <summary>
	/// Returns a random <see cref="Rgb"/> with values that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Rgb"/>.</returns>
	public static Rgb RandomRgb(this Random random, Rgb minValue, Rgb maxValue)
	{
		return random.RandomRgb(minValue.R, maxValue.R, minValue.G, maxValue.G, minValue.B, maxValue.B);
	}

	/// <summary>
	/// Returns a random <see cref="Rgba"/> with values that are greater than or equal to the corresponding min parameters, and less than the corresponding max parameters.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValueR">The minimum R value for the <see cref="Rgba"/>.</param>
	/// <param name="maxValueR">The maximum R value for the <see cref="Rgba"/>.</param>
	/// <param name="minValueG">The minimum G value for the <see cref="Rgba"/>.</param>
	/// <param name="maxValueG">The maximum G value for the <see cref="Rgba"/>.</param>
	/// <param name="minValueB">The minimum B value for the <see cref="Rgba"/>.</param>
	/// <param name="maxValueB">The maximum B value for the <see cref="Rgba"/>.</param>
	/// <param name="minValueA">The minimum A value for the <see cref="Rgba"/>.</param>
	/// <param name="maxValueA">The maximum A value for the <see cref="Rgba"/>.</param>
	/// <returns>The random <see cref="Rgba"/>.</returns>
	public static Rgba RandomRgba(this Random random, byte minValueR, byte maxValueR, byte minValueG, byte maxValueG, byte minValueB, byte maxValueB, byte minValueA, byte maxValueA)
	{
		return new Rgba(random.RandomByte(minValueR, maxValueR), random.RandomByte(minValueG, maxValueG), random.RandomByte(minValueB, maxValueB), random.RandomByte(minValueA, maxValueA));
	}

	/// <summary>
	/// Returns a random <see cref="Rgba"/> with values that are all greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="minValue">The minimum value.</param>
	/// <param name="maxValue">The maximum value.</param>
	/// <returns>The random <see cref="Rgba"/>.</returns>
	public static Rgba RandomRgba(this Random random, Rgba minValue, Rgba maxValue)
	{
		return random.RandomRgba(minValue.R, maxValue.R, minValue.G, maxValue.G, minValue.B, maxValue.B, minValue.A, maxValue.A);
	}

	/// <summary>
	/// Randomly flips the axes of the <see cref="Vector2"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="vector">The vector.</param>
	/// <returns>The randomly flipped <see cref="Vector2"/>.</returns>
	public static Vector2 RandomFlip(this Random random, Vector2 vector)
	{
		if (random.Chance(50))
			vector.X = -vector.X;
		if (random.Chance(50))
			vector.Y = -vector.Y;

		return vector;
	}

	/// <summary>
	/// Randomly flips the axes of the <see cref="Vector3"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="vector">The vector.</param>
	/// <returns>The randomly flipped <see cref="Vector3"/>.</returns>
	public static Vector3 RandomFlip(this Random random, Vector3 vector)
	{
		if (random.Chance(50))
			vector.X = -vector.X;
		if (random.Chance(50))
			vector.Y = -vector.Y;
		if (random.Chance(50))
			vector.Z = -vector.Z;

		return vector;
	}

	/// <summary>
	/// Randomly flips the axes of the <see cref="Vector4"/>.
	/// </summary>
	/// <param name="random">The <see cref="Random"/> instance.</param>
	/// <param name="vector">The vector.</param>
	/// <returns>The randomly flipped <see cref="Vector4"/>.</returns>
	public static Vector4 RandomFlip(this Random random, Vector4 vector)
	{
		if (random.Chance(50))
			vector.X = -vector.X;
		if (random.Chance(50))
			vector.Y = -vector.Y;
		if (random.Chance(50))
			vector.Z = -vector.Z;
		if (random.Chance(50))
			vector.W = -vector.W;

		return vector;
	}

	public static Quaternion RandomQuaternion(this Random random)
	{
		float x, y, z, u, v, w;
		do
		{
			x = random.RandomFloat(-1, 1);
			y = random.RandomFloat(-1, 1);
			z = x * x + y * y;
		}
		while (z > 1);
		do
		{
			u = random.RandomFloat(-1, 1);
			v = random.RandomFloat(-1, 1);
			w = u * u + v * v;
		}
		while (w > 1);
		float s = MathF.Sqrt((1 - z) / w);
		return new Quaternion(x, y, s * u, s * v);
	}

	public static Quaternion RandomAxisAlignedQuaternion(this Random random)
	{
		Span<Vector3> axes = stackalloc Vector3[3];
		axes[0] = Vector3.UnitX;
		axes[1] = Vector3.UnitY;
		axes[2] = Vector3.UnitZ;

		Span<float> angles = stackalloc float[3];
		angles[0] = MathF.PI / 2;
		angles[1] = 0;
		angles[2] = MathF.PI / 2;
		return Quaternion.CreateFromAxisAngle(random.Choose(axes), random.Choose(angles));
	}
}
