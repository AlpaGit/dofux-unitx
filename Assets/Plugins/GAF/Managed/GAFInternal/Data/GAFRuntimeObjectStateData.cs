using UnityEngine;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x02000071 RID: 113
	public class GAFRuntimeObjectStateData
	{
		// Token: 0x06000340 RID: 832 RVA: 0x0001066C File Offset: 0x0000E86C
		public GAFRuntimeObjectStateData(int _ID, Vector3 _StatePosition, Vector3[] _Vertices, Color32[] _Colors, Vector4[] _ColorsOffset, int _MaskID, GAFFilterData _FilterData)
		{
			this.m_ID = _ID;
			this.m_StatePosition = _StatePosition;
			this.m_Vertices = _Vertices;
			this.m_Colors = _Colors;
			this.m_ColorsOffset = _ColorsOffset;
			this.m_MaskID = _MaskID;
			this.m_FilterData = _FilterData;
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000341 RID: 833 RVA: 0x000106C6 File Offset: 0x0000E8C6
		public int id
		{
			get
			{
				return this.m_ID;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000342 RID: 834 RVA: 0x000106CE File Offset: 0x0000E8CE
		public Vector3 statePosition
		{
			get
			{
				return this.m_StatePosition;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000343 RID: 835 RVA: 0x000106D6 File Offset: 0x0000E8D6
		public Vector3[] vertices
		{
			get
			{
				return this.m_Vertices;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000344 RID: 836 RVA: 0x000106DE File Offset: 0x0000E8DE
		public Color32[] colors
		{
			get
			{
				return this.m_Colors;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000345 RID: 837 RVA: 0x000106E6 File Offset: 0x0000E8E6
		public Vector4[] colorsOffset
		{
			get
			{
				return this.m_ColorsOffset;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000346 RID: 838 RVA: 0x000106EE File Offset: 0x0000E8EE
		public int maskID
		{
			get
			{
				return this.m_MaskID;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000347 RID: 839 RVA: 0x000106F6 File Offset: 0x0000E8F6
		public GAFFilterData filterData
		{
			get
			{
				return this.m_FilterData;
			}
		}

		// Token: 0x040001A7 RID: 423
		private int m_ID;

		// Token: 0x040001A8 RID: 424
		private Vector3 m_StatePosition = Vector3.zero;

		// Token: 0x040001A9 RID: 425
		private Vector3[] m_Vertices;

		// Token: 0x040001AA RID: 426
		private Color32[] m_Colors;

		// Token: 0x040001AB RID: 427
		private Vector4[] m_ColorsOffset;

		// Token: 0x040001AC RID: 428
		private int m_MaskID = -1;

		// Token: 0x040001AD RID: 429
		private GAFFilterData m_FilterData;
	}
}
