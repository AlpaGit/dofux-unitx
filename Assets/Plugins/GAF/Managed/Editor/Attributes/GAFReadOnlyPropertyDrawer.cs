using GAF.Managed.GAFInternal.Attributes;
using UnityEditor;
using UnityEngine;

namespace GAF.Managed.Editor.Attributes
{
	// Token: 0x02000032 RID: 50
	[CustomPropertyDrawer(typeof(GAFReadOnly))]
	internal class GAFReadOnlyPropertyDrawer : DecoratorDrawer
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x000027C9 File Offset: 0x000009C9
		public override float GetHeight()
		{
			return 0f;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000028CD File Offset: 0x00000ACD
		public override void OnGUI(Rect position)
		{
			GUI.enabled = !(base.attribute as GAFReadOnly).enabled;
		}
	}
}
