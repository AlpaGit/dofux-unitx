using UnityEngine;

namespace GAF.Managed.GAFInternal.Attributes
{
	// Token: 0x02000025 RID: 37
	public class GAFFolderAttribute : PropertyAttribute
	{
		// Token: 0x060000CC RID: 204 RVA: 0x00005504 File Offset: 0x00003704
		public GAFFolderAttribute(string _InitialPath, string _PropertyTitle, string _LabelText, string _PopupLabelText, string _ResourcesRelocatorType, bool _IgnoreEmptyString)
		{
			this.initialPath = _InitialPath;
			this.propertyTitle = _PropertyTitle;
			this.labelText = _LabelText;
			this.popupLabelText = _PopupLabelText;
			this.resourcesRelocatorType = _ResourcesRelocatorType;
			this.ignoreEmptyString = _IgnoreEmptyString;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00005539 File Offset: 0x00003739
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00005541 File Offset: 0x00003741
		public string initialPath { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000554A File Offset: 0x0000374A
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00005552 File Offset: 0x00003752
		public string propertyTitle { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000555B File Offset: 0x0000375B
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00005563 File Offset: 0x00003763
		public string labelText { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x0000556C File Offset: 0x0000376C
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00005574 File Offset: 0x00003774
		public string popupLabelText { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000557D File Offset: 0x0000377D
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00005585 File Offset: 0x00003785
		public string resourcesRelocatorType { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x0000558E File Offset: 0x0000378E
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00005596 File Offset: 0x00003796
		public bool ignoreEmptyString { get; private set; }
	}
}
