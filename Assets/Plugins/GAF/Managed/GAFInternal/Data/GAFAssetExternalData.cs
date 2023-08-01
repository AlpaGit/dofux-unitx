using System;
using System.Collections.Generic;
using GAF.Managed.GAFInternal.Objects;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x02000076 RID: 118
	[Serializable]
	public class GAFAssetExternalData
	{
		// Token: 0x06000365 RID: 869 RVA: 0x000108DE File Offset: 0x0000EADE
		public GAFAssetExternalData(int _TimelineID, List<ObjectBehaviourType> _ObjectTypeFlags)
		{
			this.m_TimelineID = _TimelineID;
			this.m_ObjectTypeFlags = _ObjectTypeFlags;
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00010906 File Offset: 0x0000EB06
		public int timelineID
		{
			get
			{
				return this.m_TimelineID;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0001090E File Offset: 0x0000EB0E
		public List<ObjectBehaviourType> objectTypeFlags
		{
			get
			{
				return this.m_ObjectTypeFlags;
			}
		}

		// Token: 0x040001C4 RID: 452
		[HideInInspector]
		[SerializeField]
		private int m_TimelineID = -1;

		// Token: 0x040001C5 RID: 453
		[SerializeField]
		[HideInInspector]
		private List<ObjectBehaviourType> m_ObjectTypeFlags = new List<ObjectBehaviourType>();
	}
}
