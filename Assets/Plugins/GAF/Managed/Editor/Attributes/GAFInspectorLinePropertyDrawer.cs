using GAF.Managed.GAFInternal.Attributes;
using UnityEditor;
using UnityEngine;

namespace GAF.Managed.Editor.Attributes
{
	// Token: 0x02000031 RID: 49
	[CustomPropertyDrawer(typeof(GAFInspectorLine))]
	internal class GAFInspectorLinePropertyDrawer : PropertyDrawer
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x000027C9 File Offset: 0x000009C9
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 0f;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00008520 File Offset: 0x00006720
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			GAFInspectorLine gafinspectorLine = base.attribute as GAFInspectorLine;
			GUIStyle guistyle = new GUIStyle();
			guistyle.normal.background = EditorGUIUtility.whiteTexture;
			guistyle.stretchWidth = true;
			guistyle.margin = new RectOffset(0, 0, 1, 1);
			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, guistyle, new GUILayoutOption[]
			{
				GUILayout.Height(gafinspectorLine.thickness)
			});
			if (Event.current.type == EventType.Repaint)
			{
				Color color = GUI.color;
				GUI.color = gafinspectorLine.color;
				guistyle.Draw(rect, false, false, false, false);
				GUI.color = color;
			}
		}
	}
}
