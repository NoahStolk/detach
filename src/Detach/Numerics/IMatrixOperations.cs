namespace Detach.Numerics;

public interface IMatrixOperations<TSelf>
	where TSelf : IMatrixOperations<TSelf>
{
	static abstract TSelf CreateDefault();

	static abstract float Get(TSelf matrix, int index);

	static abstract float Get(TSelf matrix, int row, int col);

	static abstract void Set(ref TSelf matrix, int index, float value);

	static abstract void Set(ref TSelf matrix, int row, int col, float value);
}
