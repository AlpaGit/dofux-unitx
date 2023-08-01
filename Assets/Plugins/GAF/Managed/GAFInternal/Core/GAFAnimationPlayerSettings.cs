using System;
using GAF.Managed.GAFInternal.Assets;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x0200007A RID: 122
	[Serializable]
	public class GAFAnimationPlayerSettings
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00010A8A File Offset: 0x0000EC8A
		// (set) Token: 0x06000372 RID: 882 RVA: 0x00010A92 File Offset: 0x0000EC92
		public bool flipX
		{
			get
			{
				return this.m_FlipX;
			}
			set
			{
				this.m_FlipX = value;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00010A9B File Offset: 0x0000EC9B
		// (set) Token: 0x06000374 RID: 884 RVA: 0x00010AA3 File Offset: 0x0000ECA3
		public float scale
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

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00010AAC File Offset: 0x0000ECAC
		// (set) Token: 0x06000376 RID: 886 RVA: 0x00010AB4 File Offset: 0x0000ECB4
		public float csf
		{
			get
			{
				return this.m_CSF;
			}
			set
			{
				this.m_CSF = value;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00010ABD File Offset: 0x0000ECBD
		// (set) Token: 0x06000378 RID: 888 RVA: 0x00010AC5 File Offset: 0x0000ECC5
		public float pixelsPerUnit
		{
			get
			{
				return this.m_PixelsPerUnit;
			}
			set
			{
				this.m_PixelsPerUnit = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00010ACE File Offset: 0x0000ECCE
		// (set) Token: 0x0600037A RID: 890 RVA: 0x00010AD6 File Offset: 0x0000ECD6
		public bool playAutomatically
		{
			get
			{
				return this.m_PlayAutomatically;
			}
			set
			{
				this.m_PlayAutomatically = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00010ADF File Offset: 0x0000ECDF
		// (set) Token: 0x0600037C RID: 892 RVA: 0x00010AE7 File Offset: 0x0000ECE7
		public bool ignoreTimeScale
		{
			get
			{
				return this.m_IgnoreTimeScale;
			}
			set
			{
				this.m_IgnoreTimeScale = value;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00010AF0 File Offset: 0x0000ECF0
		// (set) Token: 0x0600037E RID: 894 RVA: 0x00010AF8 File Offset: 0x0000ECF8
		public bool perfectTiming
		{
			get
			{
				return this.m_PerfectTiming;
			}
			set
			{
				this.m_PerfectTiming = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00010B01 File Offset: 0x0000ED01
		// (set) Token: 0x06000380 RID: 896 RVA: 0x00010B09 File Offset: 0x0000ED09
		public bool playInBackground
		{
			get
			{
				return this.m_PlayInBackground;
			}
			set
			{
				this.m_PlayInBackground = value;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00010B12 File Offset: 0x0000ED12
		// (set) Token: 0x06000382 RID: 898 RVA: 0x00010B1A File Offset: 0x0000ED1A
		public GAFWrapMode wrapMode
		{
			get
			{
				return this.m_WrapMode;
			}
			set
			{
				this.m_WrapMode = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00010B23 File Offset: 0x0000ED23
		// (set) Token: 0x06000384 RID: 900 RVA: 0x00010B2B File Offset: 0x0000ED2B
		public uint targetFPS
		{
			get
			{
				return (uint)this.m_TargetFPS;
			}
			set
			{
				this.m_TargetFPS = (int)value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00010B34 File Offset: 0x0000ED34
		public float targetSPF
		{
			get
			{
				return 1f / (float)this.m_TargetFPS;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00010B43 File Offset: 0x0000ED43
		// (set) Token: 0x06000387 RID: 903 RVA: 0x00010B4B File Offset: 0x0000ED4B
		public int spriteLayerID
		{
			get
			{
				return this.m_SpriteLayerID;
			}
			set
			{
				this.m_SpriteLayerID = value;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00010B54 File Offset: 0x0000ED54
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00010B5C File Offset: 0x0000ED5C
		public string spriteLayerName
		{
			get
			{
				return this.m_SpriteLayerName;
			}
			set
			{
				this.m_SpriteLayerName = value;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00010B65 File Offset: 0x0000ED65
		// (set) Token: 0x0600038B RID: 907 RVA: 0x00010B6D File Offset: 0x0000ED6D
		public int spriteLayerValue
		{
			get
			{
				return this.m_SpriteLayerValue;
			}
			set
			{
				this.m_SpriteLayerValue = value;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00010B76 File Offset: 0x0000ED76
		// (set) Token: 0x0600038D RID: 909 RVA: 0x00010B7E File Offset: 0x0000ED7E
		public Vector2 pivotOffset
		{
			get
			{
				return this.m_PivotOffset;
			}
			set
			{
				this.m_PivotOffset = value;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00010B87 File Offset: 0x0000ED87
		// (set) Token: 0x0600038F RID: 911 RVA: 0x00010B8F File Offset: 0x0000ED8F
		public float zLayerScale
		{
			get
			{
				return this.m_ZLayerScale;
			}
			set
			{
				this.m_ZLayerScale = value;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00010B98 File Offset: 0x0000ED98
		public bool decomposeFlashTransform
		{
			get
			{
				return this.m_DecomposeFlashTransform;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00010BA0 File Offset: 0x0000EDA0
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00010BA8 File Offset: 0x0000EDA8
		public bool hasIndividualMaterial
		{
			get
			{
				return this.m_HasIndividualMaterial;
			}
			set
			{
				this.m_HasIndividualMaterial = value;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00010BB1 File Offset: 0x0000EDB1
		// (set) Token: 0x06000394 RID: 916 RVA: 0x00010BB9 File Offset: 0x0000EDB9
		public bool useLights
		{
			get
			{
				return this.m_UseLights;
			}
			protected set
			{
				this.m_UseLights = value;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00010BC2 File Offset: 0x0000EDC2
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00010BCA File Offset: 0x0000EDCA
		public bool cacheStates
		{
			get
			{
				return this.m_CacheStates;
			}
			set
			{
				this.m_CacheStates = value;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00010BD3 File Offset: 0x0000EDD3
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00010BDB File Offset: 0x0000EDDB
		public Color animationColorMultiplier
		{
			get
			{
				return this.m_AnimationColor;
			}
			set
			{
				this.m_AnimationColor = value;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00010BE4 File Offset: 0x0000EDE4
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00010BEC File Offset: 0x0000EDEC
		public Vector4 animationColorOffset
		{
			get
			{
				return this.m_AnimationColorOffset;
			}
			set
			{
				this.m_AnimationColorOffset = value;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00010BF5 File Offset: 0x0000EDF5
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00010BFD File Offset: 0x0000EDFD
		public int stencilValue
		{
			get
			{
				return this.m_StencilValue;
			}
			set
			{
				this.m_StencilValue = value;
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00010C08 File Offset: 0x0000EE08
		public void init(GAFAnimationAssetInternal _Asset)
		{
			this.scale = _Asset.scales[0];
			this.csf = _Asset.csfs[0];
			this.targetFPS = (uint)_Asset.fps;
			if (_Asset.hasNesting)
			{
				this.m_DecomposeFlashTransform = true;
			}
		}

		// Token: 0x040001D0 RID: 464
		[SerializeField]
		private bool m_FlipX;

		// Token: 0x040001D1 RID: 465
		[SerializeField]
		private float m_Scale = 1f;

		// Token: 0x040001D2 RID: 466
		[SerializeField]
		private float m_CSF = 1f;

		// Token: 0x040001D3 RID: 467
		[SerializeField]
		private float m_PixelsPerUnit = 1f;

		// Token: 0x040001D4 RID: 468
		[SerializeField]
		private bool m_PlayAutomatically = true;

		// Token: 0x040001D5 RID: 469
		[SerializeField]
		private bool m_IgnoreTimeScale;

		// Token: 0x040001D6 RID: 470
		[SerializeField]
		private bool m_PerfectTiming = true;

		// Token: 0x040001D7 RID: 471
		[SerializeField]
		private bool m_PlayInBackground = true;

		// Token: 0x040001D8 RID: 472
		[SerializeField]
		private GAFWrapMode m_WrapMode = GAFWrapMode.Loop;

		// Token: 0x040001D9 RID: 473
		[SerializeField]
		private int m_TargetFPS = 30;

		// Token: 0x040001DA RID: 474
		[SerializeField]
		private int m_SpriteLayerID;

		// Token: 0x040001DB RID: 475
		[SerializeField]
		private string m_SpriteLayerName = "Default";

		// Token: 0x040001DC RID: 476
		[SerializeField]
		private int m_SpriteLayerValue;

		// Token: 0x040001DD RID: 477
		[SerializeField]
		private Vector2 m_PivotOffset;

		// Token: 0x040001DE RID: 478
		[SerializeField]
		private bool m_HasIndividualMaterial;

		// Token: 0x040001DF RID: 479
		[SerializeField]
		private Color m_AnimationColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x040001E0 RID: 480
		[SerializeField]
		private Vector4 m_AnimationColorOffset = Vector4.zero;

		// Token: 0x040001E1 RID: 481
		[SerializeField]
		private int m_StencilValue;

		// Token: 0x040001E2 RID: 482
		[SerializeField]
		private float m_ZLayerScale = 1f;

		// Token: 0x040001E3 RID: 483
		[SerializeField]
		private bool m_DecomposeFlashTransform;

		// Token: 0x040001E4 RID: 484
		[SerializeField]
		private bool m_UseLights;

		// Token: 0x040001E5 RID: 485
		[SerializeField]
		private bool m_CacheStates;
	}
}
