using System;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x02000070 RID: 112
	[Serializable]
	public class GAFObjectStateData
	{
		// Token: 0x0600031B RID: 795 RVA: 0x00010468 File Offset: 0x0000E668
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			bool result;
			try
			{
				var gafobjectStateData = (GAFObjectStateData)obj;
				result = (this.a == gafobjectStateData.a && this.b == gafobjectStateData.b && this.c == gafobjectStateData.c && this.d == gafobjectStateData.d && this.localPosition == gafobjectStateData.localPosition);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000104E8 File Offset: 0x0000E6E8
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000104F0 File Offset: 0x0000E6F0
		public GAFObjectStateData(uint _ID)
		{
			this.m_ID = (int)_ID;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600031E RID: 798 RVA: 0x00010527 File Offset: 0x0000E727
		// (set) Token: 0x0600031F RID: 799 RVA: 0x0001052F File Offset: 0x0000E72F
		public Vector3[] vertices
		{
			get
			{
				return this.m_Vertices;
			}
			set
			{
				this.m_Vertices = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000320 RID: 800 RVA: 0x00010538 File Offset: 0x0000E738
		public uint id
		{
			get
			{
				return (uint)this.m_ID;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00010540 File Offset: 0x0000E740
		// (set) Token: 0x06000322 RID: 802 RVA: 0x00010548 File Offset: 0x0000E748
		public int zOrder
		{
			get
			{
				return this.m_ZOrder;
			}
			set
			{
				this.m_ZOrder = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00010551 File Offset: 0x0000E751
		// (set) Token: 0x06000324 RID: 804 RVA: 0x00010559 File Offset: 0x0000E759
		public float a
		{
			get
			{
				return this.m_A;
			}
			set
			{
				this.m_A = value;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000325 RID: 805 RVA: 0x00010562 File Offset: 0x0000E762
		// (set) Token: 0x06000326 RID: 806 RVA: 0x0001056A File Offset: 0x0000E76A
		public float b
		{
			get
			{
				return this.m_B;
			}
			set
			{
				this.m_B = value;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00010573 File Offset: 0x0000E773
		// (set) Token: 0x06000328 RID: 808 RVA: 0x0001057B File Offset: 0x0000E77B
		public float c
		{
			get
			{
				return this.m_C;
			}
			set
			{
				this.m_C = value;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000329 RID: 809 RVA: 0x00010584 File Offset: 0x0000E784
		// (set) Token: 0x0600032A RID: 810 RVA: 0x0001058C File Offset: 0x0000E78C
		public float d
		{
			get
			{
				return this.m_D;
			}
			set
			{
				this.m_D = value;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600032B RID: 811 RVA: 0x00010595 File Offset: 0x0000E795
		// (set) Token: 0x0600032C RID: 812 RVA: 0x0001059D File Offset: 0x0000E79D
		public Vector2 localPosition
		{
			get
			{
				return this.m_LocalPosition;
			}
			set
			{
				this.m_LocalPosition = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600032D RID: 813 RVA: 0x000105A6 File Offset: 0x0000E7A6
		// (set) Token: 0x0600032E RID: 814 RVA: 0x000105AE File Offset: 0x0000E7AE
		public Vector3 scale
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				this.m_Scale = value;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600032F RID: 815 RVA: 0x000105B7 File Offset: 0x0000E7B7
		// (set) Token: 0x06000330 RID: 816 RVA: 0x000105BF File Offset: 0x0000E7BF
		public Quaternion rotation
		{
			get
			{
				return this.m_Rotation;
			}
			set
			{
				this.m_Rotation = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000331 RID: 817 RVA: 0x000105C8 File Offset: 0x0000E7C8
		// (set) Token: 0x06000332 RID: 818 RVA: 0x000105D0 File Offset: 0x0000E7D0
		public bool useForceGeometry
		{
			get
			{
				return this.m_UseForceGeometry;
			}
			set
			{
				this.m_UseForceGeometry = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000333 RID: 819 RVA: 0x000105D9 File Offset: 0x0000E7D9
		// (set) Token: 0x06000334 RID: 820 RVA: 0x000105E1 File Offset: 0x0000E7E1
		public bool useColorTransform
		{
			get
			{
				return this.m_UseColorTransform;
			}
			set
			{
				this.m_UseColorTransform = value;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000335 RID: 821 RVA: 0x000105EA File Offset: 0x0000E7EA
		// (set) Token: 0x06000336 RID: 822 RVA: 0x000105F2 File Offset: 0x0000E7F2
		public bool useFilterData
		{
			get
			{
				return this.m_UseFilterData;
			}
			set
			{
				this.m_UseFilterData = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000337 RID: 823 RVA: 0x000105FB File Offset: 0x0000E7FB
		// (set) Token: 0x06000338 RID: 824 RVA: 0x00010603 File Offset: 0x0000E803
		public float alpha
		{
			get
			{
				return this.m_Alpha;
			}
			set
			{
				this.m_Alpha = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0001060C File Offset: 0x0000E80C
		// (set) Token: 0x0600033A RID: 826 RVA: 0x00010614 File Offset: 0x0000E814
		public int maskID
		{
			get
			{
				return this.m_MaskID;
			}
			set
			{
				this.m_MaskID = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0001061D File Offset: 0x0000E81D
		// (set) Token: 0x0600033C RID: 828 RVA: 0x00010634 File Offset: 0x0000E834
		public GAFColorTransformData colorTransformData
		{
			get
			{
				if (!this.m_UseColorTransform)
				{
					this.m_ColorTransformation = null;
				}
				return this.m_ColorTransformation;
			}
			set
			{
				this.m_ColorTransformation = value;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0001063D File Offset: 0x0000E83D
		// (set) Token: 0x0600033E RID: 830 RVA: 0x00010654 File Offset: 0x0000E854
		public GAFFilterData filterData
		{
			get
			{
				if (!this.m_UseFilterData)
				{
					this.m_FilterData = null;
				}
				return this.m_FilterData;
			}
			set
			{
				this.m_FilterData = value;
			}
		}

		// Token: 0x04000195 RID: 405
		public static GAFObjectStateData defaultState = new GAFObjectStateData(uint.MaxValue);

		// Token: 0x04000196 RID: 406
		[HideInInspector]
		[SerializeField]
		private int m_ID;

		// Token: 0x04000197 RID: 407
		[SerializeField]
		[HideInInspector]
		private int m_ZOrder;

		// Token: 0x04000198 RID: 408
		[SerializeField]
		[HideInInspector]
		private float m_A;

		// Token: 0x04000199 RID: 409
		[SerializeField]
		[HideInInspector]
		private float m_B;

		// Token: 0x0400019A RID: 410
		[SerializeField]
		[HideInInspector]
		private float m_C;

		// Token: 0x0400019B RID: 411
		[SerializeField]
		[HideInInspector]
		private float m_D;

		// Token: 0x0400019C RID: 412
		[HideInInspector]
		[SerializeField]
		private float m_Alpha;

		// Token: 0x0400019D RID: 413
		[SerializeField]
		[HideInInspector]
		private int m_MaskID = -1;

		// Token: 0x0400019E RID: 414
		[SerializeField]
		[HideInInspector]
		private bool m_UseForceGeometry;

		// Token: 0x0400019F RID: 415
		[HideInInspector]
		[SerializeField]
		private Vector2 m_LocalPosition = Vector2.zero;

		// Token: 0x040001A0 RID: 416
		[HideInInspector]
		[SerializeField]
		private Vector3 m_Scale = Vector3.zero;

		// Token: 0x040001A1 RID: 417
		[HideInInspector]
		[SerializeField]
		private Quaternion m_Rotation = Quaternion.identity;

		// Token: 0x040001A2 RID: 418
		[HideInInspector]
		[SerializeField]
		private Vector3[] m_Vertices;

		// Token: 0x040001A3 RID: 419
		[HideInInspector]
		[SerializeField]
		private GAFColorTransformData m_ColorTransformation;

		// Token: 0x040001A4 RID: 420
		[SerializeField]
		[HideInInspector]
		private GAFFilterData m_FilterData;

		// Token: 0x040001A5 RID: 421
		[SerializeField]
		[HideInInspector]
		private bool m_UseColorTransform;

		// Token: 0x040001A6 RID: 422
		[SerializeField]
		[HideInInspector]
		private bool m_UseFilterData;
	}
}
