using System;

namespace GAF.Managed.GAF.Utils.LinearMath
{
	// Token: 0x02000005 RID: 5
	internal class GAFCReflections
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002468 File Offset: 0x00000668
		internal static void complexGenerateReflection(ref Complex[] _InputMatrix, int _N, ref Complex _Tau)
		{
			Complex complex = 0.0;
			Complex complex2 = 0.0;
			Complex lhs = 0.0;
			_Tau = 0.0;
			if (_N <= 0)
			{
				_Tau = 0.0;
				return;
			}
			var num = 0.0;
			for (var i = 1; i <= _N; i++)
			{
				num = Math.Max(GAFMath.absComplex(_InputMatrix[i]), num);
			}
			var num2 = 1.0;
			if (num != 0.0)
			{
				if (num < 1.0)
				{
					num2 = Math.Sqrt(1E-300);
					lhs = 1.0 / num2;
					for (var j = 1; j <= _N; j++)
					{
						_InputMatrix[j] = lhs * _InputMatrix[j];
					}
				}
				else
				{
					num2 = Math.Sqrt(1E+300);
					lhs = 1.0 / num2;
					for (var j = 1; j <= _N; j++)
					{
						_InputMatrix[j] = lhs * _InputMatrix[j];
					}
				}
			}
			complex = _InputMatrix[1];
			num = 0.0;
			for (var i = 2; i <= _N; i++)
			{
				num = Math.Max(GAFMath.absComplex(_InputMatrix[i]), num);
			}
			var num3 = 0.0;
			if (num != 0.0)
			{
				for (var i = 2; i <= _N; i++)
				{
					complex2 = _InputMatrix[i] / num;
					num3 += (complex2 * GAFMath.conjugate(complex2)).x;
				}
				num3 = Math.Sqrt(num3) * num;
			}
			var x = complex.x;
			var y = complex.y;
			if (num3 == 0.0 && y == 0.0)
			{
				_Tau = 0.0;
				_InputMatrix[1] = _InputMatrix[1] * num2;
				return;
			}
			num = Math.Max(Math.Abs(x), Math.Abs(y));
			num = Math.Max(num, Math.Abs(num3));
			var num4 = -(num * Math.Sqrt(GAFMath.sqrt(x / num) + GAFMath.sqrt(y / num) + GAFMath.sqrt(num3 / num)));
			if (x < 0.0)
			{
				num4 = -num4;
			}
			_Tau.x = (num4 - x) / num4;
			_Tau.y = -(y / num4);
			complex = 1.0 / (complex - num4);
			if (_N > 1)
			{
				for (var j = 2; j <= _N; j++)
				{
					_InputMatrix[j] = complex * _InputMatrix[j];
				}
			}
			complex = num4;
			_InputMatrix[1] = complex;
			_InputMatrix[1] = _InputMatrix[1] * num2;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000027E4 File Offset: 0x000009E4
		internal static void complexApplyReflectionFromTheLeft(ref Complex[,] _InputMatrix, Complex _Tau, Complex[] _V, int _M1, int _M2, int _N1, int _N2, ref Complex[] _Work)
		{
			Complex lhs = 0.0;
			if (_Tau == 0.0 || _N1 > _N2 || _M1 > _M2)
			{
				return;
			}
			for (var i = _N1; i <= _N2; i++)
			{
				_Work[i] = 0.0;
			}
			for (var i = _M1; i <= _M2; i++)
			{
				lhs = GAFMath.conjugate(_V[i + 1 - _M1]);
				for (var j = _N1; j <= _N2; j++)
				{
					_Work[j] += lhs * _InputMatrix[i, j];
				}
			}
			for (var i = _M1; i <= _M2; i++)
			{
				lhs = _V[i - _M1 + 1] * _Tau;
				for (var j = _N1; j <= _N2; j++)
				{
					_InputMatrix[i, j] -= lhs * _Work[j];
				}
			}
		}
	}
}
