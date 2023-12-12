﻿namespace Detach.Numerics;

public interface IMatrixOperations<TSelf>
	where TSelf : IMatrixOperations<TSelf>
{
	static abstract TSelf Identity { get; }

	static abstract int Rows { get; }

	static abstract int Cols { get; }

	static abstract TSelf Transpose(TSelf matrix);

	static abstract float Determinant(TSelf matrix);

	static abstract TSelf Minor(TSelf matrix);

	static abstract TSelf Cofactor(TSelf matrix);

	static abstract TSelf CreateDefault();

	static abstract float Get(TSelf matrix, int index);

	static abstract float Get(TSelf matrix, int row, int col);

	static abstract void Set(ref TSelf matrix, int index, float value);

	static abstract void Set(ref TSelf matrix, int row, int col, float value);
}
