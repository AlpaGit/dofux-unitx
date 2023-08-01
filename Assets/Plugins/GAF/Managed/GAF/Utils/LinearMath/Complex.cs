using System;

namespace GAF.Managed.GAF.Utils.LinearMath
{
	// Token: 0x02000007 RID: 7
	internal struct Complex
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002FD5 File Offset: 0x000011D5
		public Complex(double _X)
		{
			this.x = _X;
			this.y = 0.0;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002FED File Offset: 0x000011ED
		public Complex(double _X, double _Y)
		{
			this.x = _X;
			this.y = _Y;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002FFD File Offset: 0x000011FD
		public static implicit operator Complex(double _X)
		{
			return new Complex(_X);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003005 File Offset: 0x00001205
		public static bool operator ==(Complex _LHS, Complex _RHS)
		{
			return _LHS.x == _RHS.x & _LHS.y == _RHS.y;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003024 File Offset: 0x00001224
		public static bool operator !=(Complex _LHS, Complex _RHS)
		{
			return _LHS.x != _RHS.x | _LHS.y != _RHS.y;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003049 File Offset: 0x00001249
		public static Complex operator +(Complex _LHS)
		{
			return _LHS;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000304C File Offset: 0x0000124C
		public static Complex operator -(Complex _LHS)
		{
			return new Complex(-_LHS.x, -_LHS.y);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003061 File Offset: 0x00001261
		public static Complex operator +(Complex _LHS, Complex _RHS)
		{
			return new Complex(_LHS.x + _RHS.x, _LHS.y + _RHS.y);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00003082 File Offset: 0x00001282
		public static Complex operator -(Complex _LHS, Complex _RHS)
		{
			return new Complex(_LHS.x - _RHS.x, _LHS.y - _RHS.y);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000030A3 File Offset: 0x000012A3
		public static Complex operator *(Complex _LHS, Complex _RHS)
		{
			return new Complex(_LHS.x * _RHS.x - _LHS.y * _RHS.y, _LHS.x * _RHS.y + _LHS.y * _RHS.x);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000030E0 File Offset: 0x000012E0
		public static Complex operator /(Complex _LHS, Complex _RHS)
		{
			Complex result;
			if (Math.Abs(_RHS.y) < Math.Abs(_RHS.x))
			{
				var num = _RHS.y / _RHS.x;
				var num2 = _RHS.x + _RHS.y * num;
				result.x = (_LHS.x + _LHS.y * num) / num2;
				result.y = (_LHS.y - _LHS.x * num) / num2;
			}
			else
			{
				var num = _RHS.x / _RHS.y;
				var num2 = _RHS.y + _RHS.x * num;
				result.x = (_LHS.y + _LHS.x * num) / num2;
				result.y = (-_LHS.x + _LHS.y * num) / num2;
			}
			return result;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000031A5 File Offset: 0x000013A5
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000031C0 File Offset: 0x000013C0
		public override bool Equals(object _Obj)
		{
			if (_Obj is byte)
			{
				return this.Equals(new Complex((double)((byte)_Obj)));
			}
			if (_Obj is sbyte)
			{
				return this.Equals(new Complex((double)((sbyte)_Obj)));
			}
			if (_Obj is short)
			{
				return this.Equals(new Complex((double)((short)_Obj)));
			}
			if (_Obj is ushort)
			{
				return this.Equals(new Complex((double)((ushort)_Obj)));
			}
			if (_Obj is int)
			{
				return this.Equals(new Complex((double)((int)_Obj)));
			}
			if (_Obj is uint)
			{
				return this.Equals(new Complex((uint)_Obj));
			}
			if (_Obj is long)
			{
				return this.Equals(new Complex((double)((long)_Obj)));
			}
			if (_Obj is ulong)
			{
				return this.Equals(new Complex((ulong)_Obj));
			}
			if (_Obj is float)
			{
				return this.Equals(new Complex((double)((float)_Obj)));
			}
			if (_Obj is double)
			{
				return this.Equals(new Complex((double)_Obj));
			}
			if (_Obj is decimal)
			{
				return this.Equals(new Complex((double)((decimal)_Obj)));
			}
			return base.Equals(_Obj);
		}

		// Token: 0x04000015 RID: 21
		public double x;

		// Token: 0x04000016 RID: 22
		public double y;
	}
}
