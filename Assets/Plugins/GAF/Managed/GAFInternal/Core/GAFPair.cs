namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x02000089 RID: 137
	public class GAFPair<TKey, TValue>
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x00012ABE File Offset: 0x00010CBE
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x00012AC6 File Offset: 0x00010CC6
		public TKey Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x00012ACF File Offset: 0x00010CCF
		// (set) Token: 0x0600043D RID: 1085 RVA: 0x00012AD7 File Offset: 0x00010CD7
		public TValue Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000023EE File Offset: 0x000005EE
		public GAFPair()
		{
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00012AE0 File Offset: 0x00010CE0
		public GAFPair(TKey _Key, TValue _Value)
		{
			this._key = _Key;
			this._value = _Value;
		}

		// Token: 0x0400022B RID: 555
		private TKey _key;

		// Token: 0x0400022C RID: 556
		private TValue _value;
	}
}
