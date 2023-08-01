using System;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x0200006C RID: 108
	[Serializable]
	public class GAFFilterData : IEquatable<GAFFilterData>
	{
		// Token: 0x06000300 RID: 768 RVA: 0x0001008C File Offset: 0x0000E28C
		public static GAFFilterData createBlurData(float _BlurX, float _BlurY)
		{
			return new GAFFilterData(GAFFilterType.GFT_Blur)
			{
				m_BlurX = _BlurX,
				m_BlurY = _BlurY
			};
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000100A2 File Offset: 0x0000E2A2
		public static GAFFilterData createGlowData(uint _Color, float _BlurX, float _BlurY, float _Strength, bool _Inner, bool _Knockout)
		{
			return new GAFFilterData(GAFFilterType.GFT_Glow)
			{
				m_Color = _Color,
				m_BlurX = _BlurX,
				m_BlurY = _BlurY,
				m_Strength = _Strength,
				m_Inner = _Inner,
				m_Knockout = _Knockout
			};
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000100D8 File Offset: 0x0000E2D8
		public static GAFFilterData createDropShadowData(uint _Color, float _BlurX, float _BlurY, float _Angle, float _Distance, float _Strength, bool _Inner, bool _Knockout)
		{
			return new GAFFilterData(GAFFilterType.GFT_DropShadow)
			{
				m_Color = _Color,
				m_BlurX = _BlurX,
				m_BlurY = _BlurY,
				m_Angle = _Angle,
				m_Distance = _Distance,
				m_Strength = _Strength,
				m_Inner = _Inner,
				m_Knockout = _Knockout
			};
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00010128 File Offset: 0x0000E328
		public static GAFFilterData createColorMtxData(float[] _ColorMtx)
		{
			var gaffilterData = new GAFFilterData(GAFFilterType.GFT_ColorMatrix);
			gaffilterData.m_ColorMtx = default(Matrix4x4);
			gaffilterData.m_ColorMtx.m00 = _ColorMtx[0];
			gaffilterData.m_ColorMtx.m01 = _ColorMtx[1];
			gaffilterData.m_ColorMtx.m02 = _ColorMtx[2];
			gaffilterData.m_ColorMtx.m03 = _ColorMtx[3];
			gaffilterData.m_ColorMtx.m10 = _ColorMtx[5];
			gaffilterData.m_ColorMtx.m11 = _ColorMtx[6];
			gaffilterData.m_ColorMtx.m12 = _ColorMtx[7];
			gaffilterData.m_ColorMtx.m13 = _ColorMtx[8];
			gaffilterData.m_ColorMtx.m20 = _ColorMtx[10];
			gaffilterData.m_ColorMtx.m21 = _ColorMtx[11];
			gaffilterData.m_ColorMtx.m22 = _ColorMtx[12];
			gaffilterData.m_ColorMtx.m23 = _ColorMtx[13];
			gaffilterData.m_ColorMtx.m30 = _ColorMtx[15];
			gaffilterData.m_ColorMtx.m31 = _ColorMtx[16];
			gaffilterData.m_ColorMtx.m32 = _ColorMtx[17];
			gaffilterData.m_ColorMtx.m33 = _ColorMtx[18];
			gaffilterData.m_ColorOffset = new Vector4(_ColorMtx[4] / 255f, _ColorMtx[9] / 255f, _ColorMtx[14] / 255f, _ColorMtx[19] / 255f);
			return gaffilterData;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00010264 File Offset: 0x0000E464
		public bool Equals(GAFFilterData _Other)
		{
			return this.color == _Other.color && this.blurX == _Other.blurX && this.blurY == _Other.blurY && this.angle == _Other.angle && this.distance == _Other.distance && this.strength == _Other.strength && this.inner == _Other.inner && this.knockout == _Other.knockout && this.colorMtx == _Other.colorMtx && this.colorOffset == _Other.colorOffset;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0001030A File Offset: 0x0000E50A
		public GAFFilterType type
		{
			get
			{
				return this.m_Type;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00010312 File Offset: 0x0000E512
		public uint color
		{
			get
			{
				return this.m_Color;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0001031A File Offset: 0x0000E51A
		public float blurX
		{
			get
			{
				return this.m_BlurX;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000308 RID: 776 RVA: 0x00010322 File Offset: 0x0000E522
		public float blurY
		{
			get
			{
				return this.m_BlurY;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0001032A File Offset: 0x0000E52A
		public float angle
		{
			get
			{
				return this.m_Angle;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600030A RID: 778 RVA: 0x00010332 File Offset: 0x0000E532
		public float distance
		{
			get
			{
				return this.m_Distance;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600030B RID: 779 RVA: 0x0001033A File Offset: 0x0000E53A
		public float strength
		{
			get
			{
				return this.m_Strength;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00010342 File Offset: 0x0000E542
		public bool inner
		{
			get
			{
				return this.m_Inner;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0001034A File Offset: 0x0000E54A
		public bool knockout
		{
			get
			{
				return this.m_Knockout;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600030E RID: 782 RVA: 0x00010352 File Offset: 0x0000E552
		public Matrix4x4 colorMtx
		{
			get
			{
				return this.m_ColorMtx;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0001035A File Offset: 0x0000E55A
		public Vector4 colorOffset
		{
			get
			{
				return this.m_ColorOffset;
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00010364 File Offset: 0x0000E564
		private GAFFilterData(GAFFilterType _Type)
		{
			this.m_Type = _Type;
		}

		// Token: 0x04000183 RID: 387
		[HideInInspector]
		[SerializeField]
		private GAFFilterType m_Type = GAFFilterType.GFT_None;

		// Token: 0x04000184 RID: 388
		[SerializeField]
		[HideInInspector]
		private uint m_Color = uint.MaxValue;

		// Token: 0x04000185 RID: 389
		[HideInInspector]
		[SerializeField]
		private float m_BlurX = -1f;

		// Token: 0x04000186 RID: 390
		[SerializeField]
		[HideInInspector]
		private float m_BlurY = -1f;

		// Token: 0x04000187 RID: 391
		[HideInInspector]
		[SerializeField]
		private float m_Angle = -1f;

		// Token: 0x04000188 RID: 392
		[HideInInspector]
		[SerializeField]
		private float m_Distance = -1f;

		// Token: 0x04000189 RID: 393
		[HideInInspector]
		[SerializeField]
		private float m_Strength = -1f;

		// Token: 0x0400018A RID: 394
		[HideInInspector]
		[SerializeField]
		private bool m_Inner;

		// Token: 0x0400018B RID: 395
		[SerializeField]
		[HideInInspector]
		private bool m_Knockout;

		// Token: 0x0400018C RID: 396
		[HideInInspector]
		[SerializeField]
		private Matrix4x4 m_ColorMtx = Matrix4x4.identity;

		// Token: 0x0400018D RID: 397
		[HideInInspector]
		[SerializeField]
		private Vector4 m_ColorOffset = Vector4.zero;
	}
}
