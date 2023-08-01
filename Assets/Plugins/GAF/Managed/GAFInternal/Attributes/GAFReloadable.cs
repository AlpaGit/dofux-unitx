using UnityEngine;

namespace GAF.Managed.GAFInternal.Attributes
{
	// Token: 0x02000028 RID: 40
	public class GAFReloadable : PropertyAttribute
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x000055F7 File Offset: 0x000037F7
		public GAFReloadable(string _ClipPropertyPath)
		{
			this.clipPropertyPath = _ClipPropertyPath;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00005606 File Offset: 0x00003806
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x0000560E File Offset: 0x0000380E
		public string clipPropertyPath { get; private set; }
	}
}
