namespace GAF.Managed.GAF.Utils.LinearMath
{
	// Token: 0x02000004 RID: 4
	internal class GAFABLAS
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000240C File Offset: 0x0000060C
		internal static void matrixCopy(int _M, int _N, Complex[,] _MatrixA, int _RowA, int _ColumnA, ref Complex[,] _MatrixB, int _RowB, int _ColumnB)
		{
			if (_M == 0 || _N == 0)
			{
				return;
			}
			for (var i = 0; i <= _M - 1; i++)
			{
				var num = _ColumnA - _ColumnB;
				for (var j = _ColumnB; j <= _ColumnB + _N - 1; j++)
				{
					_MatrixB[_RowB + i, j] = _MatrixA[_RowA + i, j + num];
				}
			}
		}

		// Token: 0x0400000B RID: 11
		internal const int ablasComplexBlockSize = 24;
	}
}
