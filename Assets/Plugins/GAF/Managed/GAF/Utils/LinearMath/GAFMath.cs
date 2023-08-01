using System;

namespace GAF.Managed.GAF.Utils.LinearMath
{
	// Token: 0x02000008 RID: 8
	internal class GAFMath
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00003388 File Offset: 0x00001588
		internal static double absComplex(Complex z)
		{
			var num = Math.Abs(z.x);
			var num2 = Math.Abs(z.y);
			var num3 = (num > num2) ? num : num2;
			var num4 = (num < num2) ? num : num2;
			if (num4 == 0.0)
			{
				return num3;
			}
			var num5 = num4 / num3;
			return num3 * Math.Sqrt(1.0 + num5 * num5);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000033E8 File Offset: 0x000015E8
		internal static Complex conjugate(Complex _Z)
		{
			return new Complex(_Z.x, -_Z.y);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000033FC File Offset: 0x000015FC
		internal static double sqrt(double _X)
		{
			return _X * _X;
		}

		// Token: 0x04000017 RID: 23
		internal const double maxRealNumber = 1E+300;

		// Token: 0x04000018 RID: 24
		internal const double minRealNumber = 1E-300;
	}
}
