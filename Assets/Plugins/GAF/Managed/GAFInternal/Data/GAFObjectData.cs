namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x0200006F RID: 111
	public class GAFObjectData
	{
		// Token: 0x06000317 RID: 791 RVA: 0x00010429 File Offset: 0x0000E629
		public GAFObjectData(uint _ID, uint _AtlasElementID, GAFObjectType _Type)
		{
			this.m_ID = _ID;
			this.m_AtlasElementID = _AtlasElementID;
			this.m_ObjectType = _Type;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0001044D File Offset: 0x0000E64D
		public uint id
		{
			get
			{
				return this.m_ID;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00010455 File Offset: 0x0000E655
		public uint atlasElementID
		{
			get
			{
				return this.m_AtlasElementID;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0001045D File Offset: 0x0000E65D
		public GAFObjectType type
		{
			get
			{
				return this.m_ObjectType;
			}
		}

		// Token: 0x04000192 RID: 402
		private uint m_ID;

		// Token: 0x04000193 RID: 403
		private uint m_AtlasElementID;

		// Token: 0x04000194 RID: 404
		private GAFObjectType m_ObjectType = GAFObjectType.None;
	}
}
