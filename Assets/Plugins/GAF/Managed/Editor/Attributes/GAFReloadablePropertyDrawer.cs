using GAF.Managed.GAFInternal.Attributes;
using GAF.Managed.GAFInternal.Core;
using UnityEditor;
using UnityEngine;

namespace GAF.Managed.Editor.Attributes
{
	// Token: 0x02000033 RID: 51
	[CustomPropertyDrawer(typeof(GAFReloadable))]
	internal class GAFReloadablePropertyDrawer : PropertyDrawer
	{
		// Token: 0x060000EB RID: 235 RVA: 0x000085B4 File Offset: 0x000067B4
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float result = 0f;
			SerializedProperty serializedProperty = property.serializedObject.FindProperty(this.attribute.clipPropertyPath);
			if (serializedProperty != null && !serializedProperty.hasMultipleDifferentValues)
			{
				result = EditorGUI.GetPropertyHeight(property, label, true);
			}
			return result;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000085F4 File Offset: 0x000067F4
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty serializedProperty = property.serializedObject.FindProperty(this.attribute.clipPropertyPath);
			if (serializedProperty != null && !serializedProperty.hasMultipleDifferentValues)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.PropertyField(position, property, label, true);
				if (EditorGUI.EndChangeCheck())
				{
					property.serializedObject.ApplyModifiedProperties();
					(serializedProperty.objectReferenceValue as GAFBaseClip).reload();
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000ED RID: 237 RVA: 0x000028EF File Offset: 0x00000AEF
		private GAFReloadable attribute
		{
			get
			{
				if (base.attribute != null)
				{
					return base.attribute as GAFReloadable;
				}
				return null;
			}
		}
	}
}
