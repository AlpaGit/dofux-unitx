using System;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x0200006B RID: 107
	[Serializable]
	public class GAFColorTransformData
	{
		// Token: 0x060002FA RID: 762 RVA: 0x0000FF78 File Offset: 0x0000E178
		public GAFColorTransformData()
		{
			this.multipliers = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			this.offsets = new Vector4(255f, 255f, 255f, 255f);
			this.m_OffsetA = 0f;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000FFD4 File Offset: 0x0000E1D4
		public GAFColorTransformData(Color32 _Multipliers, Vector4 _Offsets)
		{
			this.multipliers = _Multipliers;
			this.offsets = _Offsets;
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000FFEA File Offset: 0x0000E1EA
		// (set) Token: 0x060002FD RID: 765 RVA: 0x00010009 File Offset: 0x0000E209
		public Color32 multipliers
		{
			get
			{
				return new Color32(this.m_MultiplierR, this.m_MultiplierG, this.m_MultiplierB, this.m_MultiplierA);
			}
			set
			{
				this.m_MultiplierR = value.r;
				this.m_MultiplierG = value.g;
				this.m_MultiplierB = value.b;
				this.m_MultiplierA = value.a;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0001003B File Offset: 0x0000E23B
		// (set) Token: 0x060002FF RID: 767 RVA: 0x0001005A File Offset: 0x0000E25A
		public Vector4 offsets
		{
			get
			{
				return new Vector4(this.m_OffsetR, this.m_OffsetG, this.m_OffsetB, this.m_OffsetA);
			}
			set
			{
				this.m_OffsetR = value.x;
				this.m_OffsetG = value.y;
				this.m_OffsetB = value.z;
				this.m_OffsetA = value.w;
			}
		}

		// Token: 0x0400017B RID: 379
		[SerializeField]
		[HideInInspector]
		private byte m_MultiplierR;

		// Token: 0x0400017C RID: 380
		[HideInInspector]
		[SerializeField]
		private byte m_MultiplierG;

		// Token: 0x0400017D RID: 381
		[SerializeField]
		[HideInInspector]
		private byte m_MultiplierB;

		// Token: 0x0400017E RID: 382
		[SerializeField]
		[HideInInspector]
		private byte m_MultiplierA;

		// Token: 0x0400017F RID: 383
		[SerializeField]
		[HideInInspector]
		private float m_OffsetR;

		// Token: 0x04000180 RID: 384
		[HideInInspector]
		[SerializeField]
		private float m_OffsetG;

		// Token: 0x04000181 RID: 385
		[HideInInspector]
		[SerializeField]
		private float m_OffsetB;

		// Token: 0x04000182 RID: 386
		[SerializeField]
		[HideInInspector]
		private float m_OffsetA;
	}
}
