using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Assets
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	public class GAFTexturesResourceInternal : ScriptableObject
	{
		// Token: 0x06000077 RID: 119 RVA: 0x000044E4 File Offset: 0x000026E4
		public void initialize(GAFAnimationAssetInternal _Asset, List<string> _Names, float _Scale, float _CSF, string _DataPath)
		{
			this.m_Asset = _Asset;
			this.m_Scale = _Scale;
			this.m_CSF = _CSF;
			this.currentDataPath = _DataPath;
			foreach (var name in _Names)
			{
				this.m_Data.Add(new GAFResourceData(name));
			}
		}

		/// <summary>
		/// Set texture and material for desired data.
		/// </summary>
		/// <param name="_SearchName">Name of desired data.</param>
		/// <param name="_SharedTexture">New texture for data.</param>
		/// <param name="_SharedMaterial">New material for data.</param>
		// Token: 0x06000078 RID: 120 RVA: 0x0000455C File Offset: 0x0000275C
		public void setData(string _SearchName, Texture2D _SharedTexture, Material _SharedMaterial)
		{
			var gafresourceData = this.m_Data.Find((GAFResourceData _data) => _data.name == _SearchName);
			if (gafresourceData != null)
			{
				gafresourceData.set(_SharedTexture, _SharedMaterial);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000459C File Offset: 0x0000279C
		public void dropData()
		{
			if (this.data != null)
			{
				for (var i = 0; i < this.data.Count; i++)
				{
					this.data[i].reset();
				}
			}
		}

		/// <summary>
		/// Check if data exists and have a texture and material.
		/// </summary>
		/// <param name="_SearchName">Name of desired data.</param>
		/// <returns></returns>
		// Token: 0x0600007A RID: 122 RVA: 0x000045D8 File Offset: 0x000027D8
		public bool isDataValid(string _SearchName)
		{
			var gafresourceData = this.m_Data.Find((GAFResourceData _data) => _data.name == _SearchName);
			return gafresourceData != null && gafresourceData.isValid;
		}

		/// <summary>
		/// Get shared texture from data.
		/// </summary>
		/// <param name="_Name">Name of desired data.</param>
		/// <returns></returns>
		// Token: 0x0600007B RID: 123 RVA: 0x00004618 File Offset: 0x00002818
		public Texture2D getSharedTexture(string _Name)
		{
			var gafresourceData = this.m_Data.Find((GAFResourceData _data) => _data.name == _Name);
			if (gafresourceData == null || !gafresourceData.isValid)
			{
				return null;
			}
			return gafresourceData.sharedTexture;
		}

		/// <summary>
		/// Get shared material from data.
		/// </summary>
		/// <param name="_Name">Name of desired data.</param>
		/// <returns></returns>
		// Token: 0x0600007C RID: 124 RVA: 0x00004660 File Offset: 0x00002860
		public Material getSharedMaterial(string _Name)
		{
			Material material = null;
			var gafresourceData = this.m_Data.Find((GAFResourceData _data) => _data.name == _Name);
			if (gafresourceData != null && gafresourceData.isValid)
			{
				material = gafresourceData.sharedMaterial;
			}
			material.EnableKeyword("GAF_VERTICES_TRANSFORM_ON");
			material.DisableKeyword("GAF_VERTICES_TRANSFORM_OFF");
			material.renderQueue = 3000;
			return material;
		}

		/// <summary>
		/// Return animation asset file associated with current resource.
		/// </summary>
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000046C8 File Offset: 0x000028C8
		public GAFAnimationAssetInternal asset
		{
			get
			{
				return this.m_Asset;
			}
		}

		/// <summary>
		/// Returns true if asset associated with current resource is not null.
		/// </summary>
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000046D0 File Offset: 0x000028D0
		public bool isValid
		{
			get
			{
				return this.m_Asset != null;
			}
		}

		/// <summary>
		/// Returns true if resource is valid and all his data is valid too.
		/// </summary>
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000046DE File Offset: 0x000028DE
		public bool isReady
		{
			get
			{
				if (this.isValid)
				{
					return this.m_Data.All((GAFResourceData data) => data.isValid);
				}
				return false;
			}
		}

		/// <summary>
		/// Returns resource's scale.
		/// </summary>
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00004714 File Offset: 0x00002914
		public float scale
		{
			get
			{
				return this.m_Scale;
			}
		}

		/// <summary>
		/// Returns resource's content scale factor.
		/// </summary>
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000471C File Offset: 0x0000291C
		public float csf
		{
			get
			{
				return this.m_CSF;
			}
		}

		/// <summary>
		/// Returns all data of current resource.
		/// </summary>
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00004724 File Offset: 0x00002924
		public List<GAFResourceData> data
		{
			get
			{
				return this.m_Data;
			}
		}

		/// <summary>
		/// Returns only valid data of current resource.
		/// </summary>
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000472C File Offset: 0x0000292C
		public List<GAFResourceData> validData
		{
			get
			{
				return (from data in this.m_Data
				where data.isValid
				select data).ToList<GAFResourceData>();
			}
		}

		/// <summary>
		/// Returns only invalid data of current resource.
		/// </summary>
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000084 RID: 132 RVA: 0x0000478C File Offset: 0x0000298C
		public List<GAFResourceData> invalidData
		{
			get
			{
				return (from data in this.m_Data
				where !data.isValid
				select data).ToList<GAFResourceData>();
			}
		}

		/// <summary>
		/// Returns path used to find shared texture and shared material.
		/// </summary>
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000047EC File Offset: 0x000029EC
		// (set) Token: 0x06000086 RID: 134 RVA: 0x000047F4 File Offset: 0x000029F4
		public string currentDataPath
		{
			get
			{
				return this.m_CurrentDataPath;
			}
			set
			{
				this.m_CurrentDataPath = value;
			}
		}

		// Token: 0x0400004B RID: 75
		[HideInInspector]
		[SerializeField]
		private GAFAnimationAssetInternal m_Asset;

		// Token: 0x0400004C RID: 76
		[SerializeField]
		[HideInInspector]
		private float m_Scale = 1f;

		// Token: 0x0400004D RID: 77
		[SerializeField]
		[HideInInspector]
		private float m_CSF = 1f;

		// Token: 0x0400004E RID: 78
		[SerializeField]
		[HideInInspector]
		private string m_CurrentDataPath = string.Empty;

		// Token: 0x0400004F RID: 79
		[HideInInspector]
		[SerializeField]
		private List<GAFResourceData> m_Data = new List<GAFResourceData>();
	}
}
