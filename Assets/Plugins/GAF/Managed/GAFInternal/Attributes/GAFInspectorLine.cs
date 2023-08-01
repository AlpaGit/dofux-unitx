using UnityEngine;

namespace GAF.Managed.GAFInternal.Attributes
{
	// Token: 0x02000026 RID: 38
	public class GAFInspectorLine : PropertyAttribute
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x0000559F File Offset: 0x0000379F
		public GAFInspectorLine(Color _Color, float _Thickness)
		{
			this.color = _Color;
			this.thickness = _Thickness;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000055B5 File Offset: 0x000037B5
		// (set) Token: 0x060000DB RID: 219 RVA: 0x000055BD File Offset: 0x000037BD
		public Color color { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000055C6 File Offset: 0x000037C6
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000055CE File Offset: 0x000037CE
		public float thickness { get; private set; }
	}
}
