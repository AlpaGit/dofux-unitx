using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GAF.Managed.Editor.Utils
{
	// Token: 0x02000005 RID: 5
	public static class GAFGuiUtil
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002E0C File Offset: 0x0000100C
		public static void drawDirectorySelector(ref string _DrawPath, string _CurrentPath, string _BasePath, string _LabelText, string _PopupLabelText, Action<string> _DirectoryWasSet, Action _DirectoryWasNotSet)
		{
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUIContent guicontent = new GUIContent(_LabelText);
			float x = EditorStyles.label.CalcSize(guicontent).x;
			EditorGUILayout.LabelField(guicontent, new GUILayoutOption[]
			{
				GUILayout.Width(x)
			});
			GUI.SetNextControlName("SelectFolder");
			if (EditorGUILayout.TextField(_DrawPath, Array.Empty<GUILayoutOption>()).KeyPressed("SelectFolder", KeyCode.Return, out _DrawPath))
			{
				GUI.FocusControl("");
				while (_DrawPath.StartsWith("/") || _DrawPath.StartsWith("\\"))
				{
					_DrawPath = _DrawPath.Remove(0, 1);
				}
				while (_DrawPath.EndsWith("/") || _DrawPath.EndsWith("\\"))
				{
					_DrawPath = _DrawPath.Remove(_DrawPath.Length - 1, 1);
				}
				_DrawPath += "/";
				if (Directory.Exists(_BasePath + _DrawPath))
				{
					if (_DirectoryWasSet != null)
					{
						_DirectoryWasSet(_DrawPath);
					}
				}
				else if (_DirectoryWasNotSet != null)
				{
					_DirectoryWasNotSet();
				}
			}
			GUIContent guicontent2 = new GUIContent("...");
			float x2 = EditorStyles.miniButton.CalcSize(guicontent2).x;
			if (GUILayout.Button(guicontent2, EditorStyles.miniButton, new GUILayoutOption[]
			{
				GUILayout.Width(x2)
			}))
			{
				GUI.FocusControl("");
				string text = EditorUtility.OpenFolderPanel(_PopupLabelText, _CurrentPath, "");
				if (!string.IsNullOrEmpty(text) && text != _CurrentPath)
				{
					text += "/";
					if (text.StartsWith(_BasePath))
					{
						if (_DirectoryWasSet != null)
						{
							_DirectoryWasSet(text.Substring(_BasePath.Length, text.Length - _BasePath.Length));
						}
					}
					else if (_DirectoryWasNotSet != null)
					{
						_DirectoryWasNotSet();
					}
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002FCC File Offset: 0x000011CC
		public static void drawLine(Color _Color, float _Thickness)
		{
			GUIStyle guistyle = new GUIStyle();
			guistyle.normal.background = EditorGUIUtility.whiteTexture;
			guistyle.stretchWidth = true;
			guistyle.margin = new RectOffset(0, 0, 1, 1);
			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, guistyle, new GUILayoutOption[]
			{
				GUILayout.Height(_Thickness)
			});
			if (Event.current.type == EventType.Repaint)
			{
				Color color = GUI.color;
				GUI.color = _Color;
				guistyle.Draw(rect, false, false, false, false);
				GUI.color = color;
			}
		}
	}
}
