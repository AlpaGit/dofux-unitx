using System.Collections.Generic;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x02000074 RID: 116
	public class GAFTexturesData
	{
		// Token: 0x06000350 RID: 848 RVA: 0x00010768 File Offset: 0x0000E968
		public GAFTexturesData(uint _ID, Dictionary<float, string> _Files)
		{
			this.m_ID = _ID;
			this.m_Files = _Files;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0001077E File Offset: 0x0000E97E
		public string getFileName(float _CSF)
		{
			if (this.m_Files.ContainsKey(_CSF))
			{
				return this.m_Files[_CSF];
			}
			return string.Empty;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000352 RID: 850 RVA: 0x000107A0 File Offset: 0x0000E9A0
		public uint id
		{
			get
			{
				return this.m_ID;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000353 RID: 851 RVA: 0x000107A8 File Offset: 0x0000E9A8
		public Dictionary<float, string> files
		{
			get
			{
				return this.m_Files;
			}
		}

		// Token: 0x040001B4 RID: 436
		private uint m_ID;

		// Token: 0x040001B5 RID: 437
		private Dictionary<float, string> m_Files;
	}
}
