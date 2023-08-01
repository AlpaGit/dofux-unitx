using System;
using System.Collections.Generic;
using GAF.Managed.GAFInternal.Attributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GAF.Managed.Editor.Attributes
{
	// Token: 0x0200002E RID: 46
	[CustomPropertyDrawer(typeof(GAFFuncAttribute))]
	internal class GAFFuncAttributePropertyDrawer : PropertyDrawer
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x000027C9 File Offset: 0x000009C9
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 0f;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00007F90 File Offset: 0x00006190
		public override void OnGUI(Rect _Pos, SerializedProperty _Property, GUIContent _Label)
		{
			SerializedProperty serializedProperty = _Property.FindPropertyRelative("m_Target");
			SerializedProperty targetTypeProperty = _Property.FindPropertyRelative("m_TargetType");
			SerializedProperty methodNameProperty = _Property.FindPropertyRelative("m_Method");
			SerializedProperty serializedProperty2 = _Property.FindPropertyRelative("m_ReturnType");
			SerializedProperty serializedProperty3 = _Property.FindPropertyRelative("m_ArgumentTypes");
			int num = 0;
			bool flag = serializedProperty2.stringValue == this.returnType.ToString();
			flag &= (flag && serializedProperty3.arraySize == this.argumentTypes.Count);
			if (flag)
			{
				var enumerator = serializedProperty3.GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (((SerializedProperty)enumerator.Current).stringValue != this.argumentTypes[num++].ToString())
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				num = 0;
				serializedProperty2.stringValue = this.returnType.ToString();
				serializedProperty3.arraySize = this.argumentTypes.Count;
				foreach (object obj in serializedProperty3)
				{
					((SerializedProperty)obj).stringValue = this.argumentTypes[num++].ToString();
				}
				GUI.changed = true;
			}
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(serializedProperty, Array.Empty<GUILayoutOption>());
			if (EditorGUI.EndChangeCheck())
			{
				targetTypeProperty.stringValue = "None";
				methodNameProperty.stringValue = "None";
				GUI.changed = true;
			}
			if (!serializedProperty.hasMultipleDifferentValues)
			{
				Object objectReferenceValue = serializedProperty.objectReferenceValue;
				if (objectReferenceValue == null)
				{
					if (this.m_PossibleTypes == null)
					{
						this.initPossibleTypes();
					}
					if (this.m_PossibleTypes.Count > 0)
					{
						Type type = null;
						int num2 = 0;
						List<string> list = this.m_PossibleTypes.ConvertAll<string>((Type _type) => _type.ToString());
						list.Insert(0, "None");
						if (targetTypeProperty.hasMultipleDifferentValues)
						{
							list.Insert(0, "—");
						}
						else
						{
							num2 = list.FindIndex((string typename) => typename == targetTypeProperty.stringValue);
							if (num2 > 0)
							{
								type = this.m_PossibleTypes[num2 - 1];
							}
						}
						int num3 = EditorGUILayout.Popup("Select type: ", num2, list.ToArray(), Array.Empty<GUILayoutOption>());
						if (num3 != num2)
						{
							int num4 = targetTypeProperty.hasMultipleDifferentValues ? (num3 - 2) : (num3 - 1);
							if (num4 >= 0)
							{
								type = this.m_PossibleTypes[num4];
							}
							int num5 = targetTypeProperty.hasMultipleDifferentValues ? (num3 - 1) : num3;
							if (num5 >= 0)
							{
								targetTypeProperty.stringValue = null;
								targetTypeProperty.stringValue = list[num5];
								GUI.changed = true;
							}
						}
						if (type != null)
						{
							int num6 = 0;
							List<string> staticCandidatesList = GAFFuncData.getStaticCandidatesList(type, this.returnType, this.argumentTypes);
							staticCandidatesList.Insert(0, "None");
							if (methodNameProperty.hasMultipleDifferentValues)
							{
								staticCandidatesList.Insert(0, "—");
							}
							else
							{
								num6 = staticCandidatesList.FindIndex((string name) => name == methodNameProperty.stringValue);
							}
							int num7 = EditorGUILayout.Popup("Method (" + type.ToString() + ")", num6, staticCandidatesList.ToArray(), Array.Empty<GUILayoutOption>());
							if (num7 != num6 && (!methodNameProperty.hasMultipleDifferentValues || num7 != 0))
							{
								methodNameProperty.stringValue = null;
								methodNameProperty.stringValue = staticCandidatesList[num7];
								GUI.changed = true;
								return;
							}
						}
					}
				}
				else
				{
					EditorGUILayout.LabelField("Target type: ", objectReferenceValue.GetType().ToString(), Array.Empty<GUILayoutOption>());
					List<string> candidatesList = GAFFuncData.getCandidatesList(objectReferenceValue.GetType(), this.returnType, this.argumentTypes);
					if (candidatesList.Count > 0)
					{
						int num8 = 0;
						candidatesList.Insert(0, "None");
						if (methodNameProperty.hasMultipleDifferentValues)
						{
							candidatesList.Insert(0, "—");
						}
						else
						{
							num8 = candidatesList.FindIndex((string name) => name == methodNameProperty.stringValue);
						}
						int num9 = EditorGUILayout.Popup("Method (" + objectReferenceValue.GetType().ToString() + ")", num8, candidatesList.ToArray(), Array.Empty<GUILayoutOption>());
						if (num9 != num8 && (!methodNameProperty.hasMultipleDifferentValues || num9 != 0))
						{
							methodNameProperty.stringValue = null;
							methodNameProperty.stringValue = candidatesList[num9];
							GUI.changed = true;
							return;
						}
					}
					else
					{
						EditorGUILayout.LabelField("Method", "None", Array.Empty<GUILayoutOption>());
					}
				}
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00002813 File Offset: 0x00000A13
		private GAFFuncAttribute attribute
		{
			get
			{
				if (base.attribute != null)
				{
					return base.attribute as GAFFuncAttribute;
				}
				return null;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000282A File Offset: 0x00000A2A
		private Type returnType
		{
			get
			{
				if (this.attribute == null)
				{
					return typeof(void);
				}
				return this.attribute.returnType;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000848C File Offset: 0x0000668C
		private List<Type> argumentTypes
		{
			get
			{
				if (this.attribute == null || this.attribute.argumentTypes == null || this.attribute.argumentTypes.Count <= 0)
				{
					return new List<Type>
					{
						typeof(void)
					};
				}
				return this.attribute.argumentTypes;
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000084E4 File Offset: 0x000066E4
		private static List<Type> getScriptAssetsOfType()
		{
			MonoScript[] array = Resources.FindObjectsOfTypeAll<MonoScript>();
			List<Type> list = new List<Type>();
			MonoScript[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Type @class = array2[i].GetClass();
				if (@class != null)
				{
					list.Add(@class);
				}
			}
			return list;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000284A File Offset: 0x00000A4A
		private void initPossibleTypes()
		{
			this.m_PossibleTypes = GAFFuncAttributePropertyDrawer.getScriptAssetsOfType();
			this.m_PossibleTypes.RemoveAll((Type type) => GAFFuncData.getStaticCandidatesList(type, this.returnType, this.argumentTypes).Count == 0);
		}

		// Token: 0x04000098 RID: 152
		private List<Type> m_PossibleTypes;
	}
}
