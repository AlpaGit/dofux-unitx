using UnityEngine;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x0200006A RID: 106
	public class GAFAtlasElementData
	{
		// Token: 0x060002EB RID: 747 RVA: 0x0000FE30 File Offset: 0x0000E030
		public GAFAtlasElementData(uint _ID, float _PivotX, float _PivotY, float _X, float _Y, float _Width, float _Height, uint _AtlasID, float _Scale, Rect _Scale9GridRect)
		{
			this.m_ID = _ID;
			this.m_PivotX = _PivotX;
			this.m_PivotY = _PivotY;
			this.m_X = _X;
			this.m_Y = _Y;
			this.m_Width = _Width;
			this.m_Height = _Height;
			this.m_AtlasID = _AtlasID;
			this.m_ScaleX = _Scale;
			this.m_ScaleY = _Scale;
			this.m_Scale9GridRect = _Scale9GridRect;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000FE98 File Offset: 0x0000E098
		public GAFAtlasElementData(uint _ID, float _PivotX, float _PivotY, float _X, float _Y, float _Width, float _Height, uint _AtlasID, float _ScaleX, float _ScaleY, sbyte _Rotation, string _LinkageName, Rect _Scale9GridRect)
		{
			this.m_ID = _ID;
			this.m_PivotX = _PivotX;
			this.m_PivotY = _PivotY;
			this.m_X = _X;
			this.m_Y = _Y;
			this.m_Width = _Width;
			this.m_Height = _Height;
			this.m_AtlasID = _AtlasID;
			this.m_ScaleX = _ScaleX;
			this.m_ScaleY = _ScaleY;
			this.m_Rotation = _Rotation;
			this.m_LinkageName = _LinkageName;
			this.m_Scale9GridRect = _Scale9GridRect;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000FF10 File Offset: 0x0000E110
		public uint id
		{
			get
			{
				return this.m_ID;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000FF18 File Offset: 0x0000E118
		public float pivotX
		{
			get
			{
				return this.m_PivotX;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000FF20 File Offset: 0x0000E120
		public float pivotY
		{
			get
			{
				return this.m_PivotY;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000FF28 File Offset: 0x0000E128
		public float x
		{
			get
			{
				return this.m_X;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000FF30 File Offset: 0x0000E130
		public float y
		{
			get
			{
				return this.m_Y;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000FF38 File Offset: 0x0000E138
		public float width
		{
			get
			{
				return this.m_Width;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000FF40 File Offset: 0x0000E140
		public float height
		{
			get
			{
				return this.m_Height;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000FF48 File Offset: 0x0000E148
		public uint atlasID
		{
			get
			{
				return this.m_AtlasID;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000FF50 File Offset: 0x0000E150
		public float scaleX
		{
			get
			{
				return this.m_ScaleX;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000FF58 File Offset: 0x0000E158
		public float scaleY
		{
			get
			{
				return this.m_ScaleY;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000FF60 File Offset: 0x0000E160
		public sbyte rotation
		{
			get
			{
				return this.m_Rotation;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000FF68 File Offset: 0x0000E168
		public string linkageName
		{
			get
			{
				return this.m_LinkageName;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x0000FF70 File Offset: 0x0000E170
		public Rect scale9GridRect
		{
			get
			{
				return this.m_Scale9GridRect;
			}
		}

		// Token: 0x0400016E RID: 366
		private uint m_ID;

		// Token: 0x0400016F RID: 367
		private float m_PivotX;

		// Token: 0x04000170 RID: 368
		private float m_PivotY;

		// Token: 0x04000171 RID: 369
		private float m_X;

		// Token: 0x04000172 RID: 370
		private float m_Y;

		// Token: 0x04000173 RID: 371
		private float m_Width;

		// Token: 0x04000174 RID: 372
		private float m_Height;

		// Token: 0x04000175 RID: 373
		private uint m_AtlasID;

		// Token: 0x04000176 RID: 374
		private float m_ScaleX;

		// Token: 0x04000177 RID: 375
		private float m_ScaleY;

		// Token: 0x04000178 RID: 376
		private Rect m_Scale9GridRect;

		// Token: 0x04000179 RID: 377
		private string m_LinkageName;

		// Token: 0x0400017A RID: 378
		private sbyte m_Rotation;
	}
}
