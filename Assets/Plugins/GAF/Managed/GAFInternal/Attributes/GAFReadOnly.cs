using UnityEngine;

namespace GAF.Managed.GAFInternal.Attributes
{
	// Token: 0x02000027 RID: 39
	public class GAFReadOnly : PropertyAttribute
	{
		// Token: 0x060000DE RID: 222 RVA: 0x000055D7 File Offset: 0x000037D7
		public GAFReadOnly(bool _Enabled)
		{
			this.enabled = _Enabled;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000055E6 File Offset: 0x000037E6
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x000055EE File Offset: 0x000037EE
		public bool enabled { get; private set; }
	}
}
