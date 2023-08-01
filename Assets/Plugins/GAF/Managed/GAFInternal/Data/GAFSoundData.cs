namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x02000073 RID: 115
	public class GAFSoundData
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00010733 File Offset: 0x0000E933
		public uint id
		{
			get
			{
				return this.m_ID;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0001073B File Offset: 0x0000E93B
		public string fileName
		{
			get
			{
				return this.m_FileName;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00010743 File Offset: 0x0000E943
		public string linkage
		{
			get
			{
				return this.m_Linkage;
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0001074B File Offset: 0x0000E94B
		public GAFSoundData(uint _ID, string _FileName, string _Linkage)
		{
			this.m_ID = _ID;
			this.m_FileName = _FileName;
			this.m_Linkage = _Linkage;
		}

		// Token: 0x040001B1 RID: 433
		private uint m_ID;

		// Token: 0x040001B2 RID: 434
		private string m_FileName;

		// Token: 0x040001B3 RID: 435
		private string m_Linkage;
	}
}
