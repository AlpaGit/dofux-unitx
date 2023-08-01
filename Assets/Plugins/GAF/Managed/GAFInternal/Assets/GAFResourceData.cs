using System;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Assets
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public class GAFResourceData
	{
		// Token: 0x06000070 RID: 112 RVA: 0x00004446 File Offset: 0x00002646
		public GAFResourceData(string _Name)
		{
			this.m_SearchName = _Name;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004460 File Offset: 0x00002660
		public void set(Texture2D _Texture, Material _Material)
		{
			this.m_SharedTexture = _Texture;
			this.m_SharedMaterial = _Material;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004470 File Offset: 0x00002670
		public void reset()
		{
			Resources.UnloadAsset(this.m_SharedMaterial);
			Resources.UnloadAsset(this.m_SharedTexture);
			this.m_SharedMaterial = null;
			this.m_SharedTexture = null;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00004496 File Offset: 0x00002696
		public bool isValid
		{
			get
			{
				return this.m_SharedTexture != null && this.m_SharedMaterial != null && this.m_SharedMaterial.mainTexture == this.m_SharedTexture;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000044CC File Offset: 0x000026CC
		public string name
		{
			get
			{
				return this.m_SearchName;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000044D4 File Offset: 0x000026D4
		public Texture2D sharedTexture
		{
			get
			{
				return this.m_SharedTexture;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000044DC File Offset: 0x000026DC
		public Material sharedMaterial
		{
			get
			{
				return this.m_SharedMaterial;
			}
		}

		// Token: 0x04000048 RID: 72
		[HideInInspector]
		[SerializeField]
		private string m_SearchName = string.Empty;

		// Token: 0x04000049 RID: 73
		[HideInInspector]
		[SerializeField]
		private Texture2D m_SharedTexture;

		// Token: 0x0400004A RID: 74
		[HideInInspector]
		[SerializeField]
		private Material m_SharedMaterial;
	}
}
