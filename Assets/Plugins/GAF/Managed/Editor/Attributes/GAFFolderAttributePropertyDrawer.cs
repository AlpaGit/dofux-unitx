using System;
using System.IO;
using System.Linq;
using System.Reflection;
using GAF.Managed.Editor.Utils;
using GAF.Managed.GAFInternal.Attributes;
using UnityEditor;
using UnityEngine;

namespace GAF.Managed.Editor.Attributes
{
	// Token: 0x0200002D RID: 45
	[CustomPropertyDrawer(typeof(GAFFolderAttribute))]
	internal class GAFFolderAttributePropertyDrawer : PropertyDrawer
	{
		// Token: 0x060000CF RID: 207 RVA: 0x000027C9 File Offset: 0x000009C9
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 0f;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00007B30 File Offset: 0x00005D30
		public override void OnGUI(Rect _Pos, SerializedProperty _Property, GUIContent _Label)
		{
			if (_Property.propertyType == SerializedPropertyType.String)
			{
				if (!this.m_IsEnabled)
				{
					this.m_IsEnabled = true;
					this.m_AttributeInitialPath = this.normalizePath(this.attribute.initialPath);
					if (!Directory.Exists(Application.dataPath + "/" + this.m_AttributeInitialPath))
					{
						this.m_InitialPath = Application.dataPath + "/";
					}
					else
					{
						this.m_InitialPath = Application.dataPath + "/" + this.m_AttributeInitialPath;
					}
					if (!_Property.hasMultipleDifferentValues && string.IsNullOrEmpty(_Property.stringValue) && !this.attribute.ignoreEmptyString)
					{
						string cachePath = GAFSystemEditor.getCachePath();
						_Property.stringValue = cachePath.Substring("Assets/".Length, cachePath.Length - "Assets/".Length);
					}
					if (!_Property.hasMultipleDifferentValues && !Directory.Exists(this.m_InitialPath + _Property.stringValue))
					{
						_Property.stringValue = string.Empty;
					}
					string resourcesRelocatorType = this.attribute.resourcesRelocatorType;
					if (!string.IsNullOrEmpty(resourcesRelocatorType))
					{
						this.m_TypeofRelocator = Type.GetType(resourcesRelocatorType);
						if (this.m_TypeofRelocator != null && this.m_TypeofRelocator.GetInterface("IGAFResourcesRelocator") != null)
						{
							Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
							for (int i = 0; i < assemblies.Length; i++)
							{
								Type type = assemblies[i].GetExportedTypes().FirstOrDefault((Type t) => t.IsSubclassOf(this.m_TypeofRelocator));
								if (type != null)
								{
									this.m_Relocator = (Activator.CreateInstance(type) as IGAFResourcesRelocator);
								}
							}
							if (this.m_Relocator == null)
							{
								this.m_Relocator = (Activator.CreateInstance(this.m_TypeofRelocator) as IGAFResourcesRelocator);
							}
						}
					}
				}
				if (this.m_InitialEditablePath == null && !_Property.hasMultipleDifferentValues)
				{
					this.m_InitialEditablePath = _Property.stringValue;
				}
				EditorGUILayout.LabelField(this.attribute.propertyTitle, Array.Empty<GUILayoutOption>());
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUIContent guicontent = new GUIContent(this.attribute.labelText);
				float x = EditorStyles.label.CalcSize(guicontent).x;
				EditorGUILayout.LabelField(guicontent, new GUILayoutOption[]
				{
					GUILayout.Width(x)
				});
				string text = _Property.hasMultipleDifferentValues ? "—" : _Property.stringValue;
				string text2 = EditorGUILayout.TextField(text, Array.Empty<GUILayoutOption>());
				if (text2 != text)
				{
					text2 = this.normalizePath(text2);
					if (Directory.Exists(this.m_InitialPath + text2))
					{
						if (this.m_Relocator != null)
						{
							this.m_Relocator.relocate(_Property.serializedObject, text2);
						}
						_Property.stringValue = text2;
						this.m_InitialEditablePath = null;
					}
					else if (this.m_InitialEditablePath != null)
					{
						_Property.stringValue = this.m_InitialEditablePath;
					}
				}
				string text3 = this.m_InitialPath + _Property.stringValue;
				GUIContent guicontent2 = new GUIContent("...");
				float x2 = EditorStyles.miniButton.CalcSize(guicontent2).x;
				if (GUILayout.Button(guicontent2, EditorStyles.miniButton, new GUILayoutOption[]
				{
					GUILayout.Width(x2)
				}))
				{
					GUI.FocusControl("");
					string text4 = EditorUtility.OpenFolderPanel(this.attribute.popupLabelText, text3, "");
					if (!string.IsNullOrEmpty(text4) && text4 != text3)
					{
						text4 += "/";
						if (text4.StartsWith(this.m_InitialPath))
						{
							string text5 = text4.Substring(this.m_InitialPath.Length, text4.Length - this.m_InitialPath.Length);
							if (this.m_Relocator != null)
							{
								this.m_Relocator.relocate(_Property.serializedObject, text5);
							}
							_Property.stringValue = text5;
							this.m_InitialEditablePath = null;
						}
						else
						{
							_Property.stringValue = this.m_InitialEditablePath;
						}
					}
				}
				EditorGUILayout.EndHorizontal();
				return;
			}
			base.OnGUI(_Pos, _Property, _Label);
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000027D0 File Offset: 0x000009D0
		private GAFFolderAttribute attribute
		{
			get
			{
				if (base.attribute != null)
				{
					return base.attribute as GAFFolderAttribute;
				}
				return null;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00007F00 File Offset: 0x00006100
		private string normalizePath(string _Path)
		{
			string text = _Path;
			if (text.Length > 0)
			{
				while (text[0] == Path.DirectorySeparatorChar)
				{
					text = text.Remove(0, 1);
				}
				while (text[text.Length - 1] == Path.DirectorySeparatorChar)
				{
					text = text.Remove(text.Length - 1, 1);
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (text == "." || text.Contains(".."))
				{
					text = "";
				}
				else
				{
					text += "/";
				}
			}
			return text;
		}

		// Token: 0x04000092 RID: 146
		private bool m_IsEnabled;

		// Token: 0x04000093 RID: 147
		private string m_AttributeInitialPath = string.Empty;

		// Token: 0x04000094 RID: 148
		private string m_InitialPath = string.Empty;

		// Token: 0x04000095 RID: 149
		private string m_InitialEditablePath;

		// Token: 0x04000096 RID: 150
		private Type m_TypeofRelocator;

		// Token: 0x04000097 RID: 151
		private IGAFResourcesRelocator m_Relocator;
	}
}
