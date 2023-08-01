namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x0200003E RID: 62
	public class TagRecord
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00007283 File Offset: 0x00005483
		// (set) Token: 0x06000126 RID: 294 RVA: 0x0000728B File Offset: 0x0000548B
		public long expectedStreamPosition
		{
			get
			{
				return this.m_ExpectedStreamPos;
			}
			set
			{
				this.m_ExpectedStreamPos = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00007294 File Offset: 0x00005494
		// (set) Token: 0x06000128 RID: 296 RVA: 0x0000729C File Offset: 0x0000549C
		public long tagSize
		{
			get
			{
				return this.m_TagSize;
			}
			set
			{
				this.m_TagSize = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000072A5 File Offset: 0x000054A5
		// (set) Token: 0x0600012A RID: 298 RVA: 0x000072AD File Offset: 0x000054AD
		public TagBase.TagType type
		{
			get
			{
				return this.m_TagType;
			}
			set
			{
				this.m_TagType = value;
			}
		}

		// Token: 0x0400009E RID: 158
		private long m_ExpectedStreamPos;

		// Token: 0x0400009F RID: 159
		private long m_TagSize;

		// Token: 0x040000A0 RID: 160
		private TagBase.TagType m_TagType;
	}
}
