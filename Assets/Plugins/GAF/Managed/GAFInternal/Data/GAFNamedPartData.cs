namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x0200006E RID: 110
	public class GAFNamedPartData
	{
		// Token: 0x06000314 RID: 788 RVA: 0x00010403 File Offset: 0x0000E603
		public GAFNamedPartData(uint _ObjectID, string _Name)
		{
			this.m_ObjectID = _ObjectID;
			this.m_Name = _Name;
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00010419 File Offset: 0x0000E619
		public uint objectID
		{
			get
			{
				return this.m_ObjectID;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000316 RID: 790 RVA: 0x00010421 File Offset: 0x0000E621
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x04000190 RID: 400
		private uint m_ObjectID;

		// Token: 0x04000191 RID: 401
		private string m_Name;
	}
}
