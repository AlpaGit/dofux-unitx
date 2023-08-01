using System;

namespace GAF.Managed.GAF.Utils.LinearMath
{
	// Token: 0x02000006 RID: 6
	internal class GAFLinearMath
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000028F0 File Offset: 0x00000AF0
		internal static void matrixQR(ref Complex[,] _InputMatrix, out Complex[] _Tau)
		{
			var num = 2;
			var num2 = 0;
			var num3 = 2;
			var num4 = 2;
			_Tau = GAFLinearMath.m_Tau;
			while (num2 != num)
			{
				var num5 = num - num2;
				var num6 = num3 - num2;
				GAFABLAS.matrixCopy(num6, num5, _InputMatrix, num2, num2, ref GAFLinearMath.m_TmpA, 0, 0);
				GAFLinearMath.matrixQRBaseCase(ref GAFLinearMath.m_TmpA, num6, num5, ref GAFLinearMath.m_Work, ref GAFLinearMath.m_T, ref GAFLinearMath.m_TauBuf);
				GAFABLAS.matrixCopy(num6, num5, GAFLinearMath.m_TmpA, 0, 0, ref _InputMatrix, num2, num2);
				var num7 = 0 - num2;
				for (var i = num2; i <= num2 + num5 - 1; i++)
				{
					_Tau[i] = GAFLinearMath.m_TauBuf[i + num7];
				}
				if (num2 + num5 <= num4 - 1)
				{
					for (var j = 0; j <= num5 - 1; j++)
					{
						num7 = j - 1;
						for (var i = 1; i <= num6 - j; i++)
						{
							GAFLinearMath.m_T[i] = GAFLinearMath.m_TmpA[i + num7, j];
						}
						GAFLinearMath.m_T[1] = 1.0;
						GAFCReflections.complexApplyReflectionFromTheLeft(ref _InputMatrix, GAFMath.conjugate(GAFLinearMath.m_TauBuf[j]), GAFLinearMath.m_T, num2 + j, num3 - 1, num2 + num5, num4 - 1, ref GAFLinearMath.m_Work);
					}
				}
				num2 += num5;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002A48 File Offset: 0x00000C48
		internal static void matrixQRUnpackQ(Complex[,] _InputMatrix, Complex[] _Tau, out Complex[,] _Q)
		{
			var num = 2;
			var num2 = 2;
			var num3 = 2;
			_Q = GAFLinearMath.m_MatrixQ;
			for (var i = 0; i <= num3 - 1; i++)
			{
				for (var j = 0; j <= num - 1; j++)
				{
					if (i == j)
					{
						_Q[i, j] = 1.0;
					}
					else
					{
						_Q[i, j] = 0.0;
					}
				}
			}
			var k = 24 * (num2 / 24);
			var num4 = num2 - k;
			while (k >= 0)
			{
				var num5 = num3 - k;
				if (num4 > 0)
				{
					GAFABLAS.matrixCopy(num5, num4, _InputMatrix, k, k, ref GAFLinearMath.m_TmpA, 0, 0);
					var num6 = k;
					for (var l = 0; l <= num4 - 1; l++)
					{
						GAFLinearMath.m_TauBuf[l] = _Tau[l + num6];
					}
					for (var i = num4 - 1; i >= 0; i--)
					{
						num6 = i - 1;
						for (var l = 1; l <= num5 - i; l++)
						{
							GAFLinearMath.m_T[l] = GAFLinearMath.m_TmpA[l + num6, i];
						}
						GAFLinearMath.m_T[1] = 1.0;
						GAFCReflections.complexApplyReflectionFromTheLeft(ref _Q, GAFLinearMath.m_TauBuf[i], GAFLinearMath.m_T, k + i, num3 - 1, 0, num - 1, ref GAFLinearMath.m_Work);
					}
				}
				k -= 24;
				num4 = 24;
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002BD0 File Offset: 0x00000DD0
		internal static void matrixQRUnpackR(Complex[,] _InputMatrix, out Complex[,] _R)
		{
			var num = 2;
			var num2 = 2;
			var num3 = 2;
			_R = GAFLinearMath.m_MatrixR;
			for (var i = 0; i <= num3 - 1; i++)
			{
				_R[0, i] = 0.0;
			}
			for (var i = 1; i <= num2 - 1; i++)
			{
				for (var j = 0; j <= num3 - 1; j++)
				{
					_R[i, j] = _R[0, j];
				}
			}
			for (var i = 0; i <= num - 1; i++)
			{
				for (var j = i; j <= num3 - 1; j++)
				{
					_R[i, j] = _InputMatrix[i, j];
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002C7C File Offset: 0x00000E7C
		private static void matrixQRBaseCase(ref Complex[,] _InputMatrix, int _M, int _N, ref Complex[] _Work, ref Complex[] _T, ref Complex[] _Tau)
		{
			Complex complex = 0.0;
			if (Math.Min(_M, _N) <= 0)
			{
				return;
			}
			var num = Math.Min(_M, _N);
			for (var i = 0; i <= num - 1; i++)
			{
				var num2 = _M - i;
				var num3 = i - 1;
				for (var j = 1; j <= num2; j++)
				{
					_T[j] = _InputMatrix[j + num3, i];
				}
				GAFCReflections.complexGenerateReflection(ref _T, num2, ref complex);
				_Tau[i] = complex;
				num3 = 1 - i;
				for (var j = i; j <= _M - 1; j++)
				{
					_InputMatrix[j, i] = _T[j + num3];
				}
				_T[1] = 1.0;
				if (i < _N - 1)
				{
					GAFCReflections.complexApplyReflectionFromTheLeft(ref _InputMatrix, GAFMath.conjugate(_Tau[i]), _T, i, _M - 1, i + 1, _N - 1, ref _Work);
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002D74 File Offset: 0x00000F74
		private static void matrixBlockReflector(ref Complex[,] _MatrixA, ref Complex[] _Tau, bool _ColumnWiseA, int _LengthA, int _BlockSize, ref Complex[,] _T, ref Complex[] _Work)
		{
			Complex complex = 0.0;
			for (var i = 0; i <= _BlockSize - 1; i++)
			{
				if (_ColumnWiseA)
				{
					for (var j = 0; j <= i - 1; j++)
					{
						_MatrixA[j, i] = 0.0;
					}
				}
				else
				{
					for (var j = 0; j <= i - 1; j++)
					{
						_MatrixA[i, j] = 0.0;
					}
				}
				_MatrixA[i, i] = 1.0;
				for (var j = 0; j <= i - 1; j++)
				{
					if (_ColumnWiseA)
					{
						complex = 0.0;
						for (var k = i; k <= _LengthA - 1; k++)
						{
							complex += GAFMath.conjugate(_MatrixA[k, j]) * _MatrixA[k, i];
						}
					}
					else
					{
						complex = 0.0;
						for (var k = i; k <= _LengthA - 1; k++)
						{
							complex += _MatrixA[j, k] * GAFMath.conjugate(_MatrixA[i, k]);
						}
					}
					_Work[j] = complex;
				}
				for (var j = 0; j <= i - 1; j++)
				{
					complex = 0.0;
					for (var k = j; k <= i - 1; k++)
					{
						complex += _T[j, k] * _Work[k];
					}
					_T[j, i] = -(_Tau[i] * complex);
				}
				_T[i, i] = -_Tau[i];
				for (var j = i + 1; j <= _BlockSize - 1; j++)
				{
					_T[j, i] = 0.0;
				}
			}
		}

		// Token: 0x0400000C RID: 12
		private static Complex[] m_Work = new Complex[3];

		// Token: 0x0400000D RID: 13
		private static Complex[] m_T = new Complex[3];

		// Token: 0x0400000E RID: 14
		private static Complex[] m_Tau = new Complex[2];

		// Token: 0x0400000F RID: 15
		private static Complex[] m_TauBuf = new Complex[2];

		// Token: 0x04000010 RID: 16
		private static Complex[,] m_TmpA = new Complex[2, 24];

		// Token: 0x04000011 RID: 17
		private static Complex[,] m_TmpT = new Complex[24, 24];

		// Token: 0x04000012 RID: 18
		private static Complex[,] m_TmpR = new Complex[48, 2];

		// Token: 0x04000013 RID: 19
		private static Complex[,] m_MatrixQ = new Complex[2, 2];

		// Token: 0x04000014 RID: 20
		private static Complex[,] m_MatrixR = new Complex[2, 2];
	}
}
