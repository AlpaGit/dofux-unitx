namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x02000072 RID: 114
	public class GAFSequenceData
	{
		// Token: 0x06000348 RID: 840 RVA: 0x000106FE File Offset: 0x0000E8FE
		public GAFSequenceData(string _Name, uint _StartFrame, uint _EndFrame)
		{
			this.m_Name = _Name;
			this.m_StartFrame = _StartFrame;
			this.m_EndFrame = _EndFrame;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0001071B File Offset: 0x0000E91B
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00010723 File Offset: 0x0000E923
		public uint startFrame
		{
			get
			{
				return this.m_StartFrame;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0001072B File Offset: 0x0000E92B
		public uint endFrame
		{
			get
			{
				return this.m_EndFrame;
			}
		}

		// Token: 0x040001AE RID: 430
		private string m_Name;

		// Token: 0x040001AF RID: 431
		private uint m_StartFrame;

		// Token: 0x040001B0 RID: 432
		private uint m_EndFrame;
	}
}
