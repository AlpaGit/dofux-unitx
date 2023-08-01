using System;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x02000084 RID: 132
	[AddComponentMenu("")]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class GAFBehaviour : MonoBehaviour
	{
		/// <summary>
		/// GAF transformation component.
		/// <para />Contains redundant data for nested clips transforms.
		/// </summary>
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x000120AC File Offset: 0x000102AC
		public GAFTransform gafTransform
		{
			get
			{
				if (this.m_GAFTransform == null)
				{
					this.m_GAFTransform = base.GetComponent<GAFTransform>();
					if (this.m_GAFTransform == null)
					{
						this.m_GAFTransform = base.gameObject.AddComponent<GAFTransform>();
					}
				}
				return this.m_GAFTransform;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x000120F8 File Offset: 0x000102F8
		public Transform cachedTransform
		{
			get
			{
				if (!this.m_CachedTransform)
				{
					this.m_CachedTransform = base.GetComponent<Transform>();
				}
				return this.m_CachedTransform;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00012119 File Offset: 0x00010319
		public MeshRenderer cachedRenderer
		{
			get
			{
				if (!this.m_CachedRenderer)
				{
					this.m_CachedRenderer = base.GetComponent<MeshRenderer>();
				}
				return this.m_CachedRenderer;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0001213A File Offset: 0x0001033A
		public MeshFilter cachedFilter
		{
			get
			{
				if (!this.m_CachedFilter)
				{
					this.m_CachedFilter = base.GetComponent<MeshFilter>();
				}
				return this.m_CachedFilter;
			}
		}

		// Token: 0x0400020D RID: 525
		private Transform m_CachedTransform;

		// Token: 0x0400020E RID: 526
		private MeshRenderer m_CachedRenderer;

		// Token: 0x0400020F RID: 527
		private MeshFilter m_CachedFilter;

		// Token: 0x04000210 RID: 528
		[HideInInspector]
		[NonSerialized]
		private GAFTransform m_GAFTransform;
	}
}
