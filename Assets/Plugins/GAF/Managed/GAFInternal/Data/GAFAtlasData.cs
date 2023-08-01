using System.Collections.Generic;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x02000069 RID: 105
	public class GAFAtlasData
	{
		// Token: 0x060002E6 RID: 742 RVA: 0x0000FDC5 File Offset: 0x0000DFC5
		public GAFAtlasData(float _Scale, Dictionary<uint, GAFTexturesData> _TexturesData, Dictionary<uint, GAFAtlasElementData> _Elements)
		{
			this.m_Scale = _Scale;
			this.m_TexturesData = _TexturesData;
			this.m_Elements = _Elements;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000FDE2 File Offset: 0x0000DFE2
		public GAFTexturesData getAtlas(uint _AtlasID)
		{
			if (this.m_TexturesData.ContainsKey(_AtlasID))
			{
				return this.m_TexturesData[_AtlasID];
			}
			return null;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000FE00 File Offset: 0x0000E000
		public GAFAtlasElementData getElement(uint _ElementID)
		{
			if (this.m_Elements.ContainsKey(_ElementID))
			{
				return this.m_Elements[_ElementID];
			}
			return null;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000FE1E File Offset: 0x0000E01E
		public float scale
		{
			get
			{
				return this.m_Scale;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000FE26 File Offset: 0x0000E026
		public Dictionary<uint, GAFTexturesData> texturesData
		{
			get
			{
				return this.m_TexturesData;
			}
		}

		// Token: 0x0400016B RID: 363
		private float m_Scale;

		// Token: 0x0400016C RID: 364
		private Dictionary<uint, GAFTexturesData> m_TexturesData;

		// Token: 0x0400016D RID: 365
		private Dictionary<uint, GAFAtlasElementData> m_Elements;
	}
}
