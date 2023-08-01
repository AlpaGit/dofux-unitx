using System;
using System.Collections.Generic;
using System.Linq;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using GAF.Managed.GAFInternal.Objects;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GAF.Managed.Editor.Objects
{
	// Token: 0x0200001E RID: 30
	[CanEditMultipleObjects]
	[CustomEditor(typeof(GAFObjectInternal))]
	public class GAFObjectInternalEditor : UnityEditor.Editor
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000073 RID: 115 RVA: 0x0000236E File Offset: 0x0000056E
		private GAFObjectInternal target
		{
			get
			{
				return base.target as GAFObjectInternal;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000074 RID: 116 RVA: 0x0000237B File Offset: 0x0000057B
		private List<GAFObjectInternal> targets
		{
			get
			{
				return base.targets.ToList<Object>().ConvertAll<GAFObjectInternal>((Object _target) => _target as GAFObjectInternal);
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000023AC File Offset: 0x000005AC
		public void OnEnable()
		{
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(this.Update));
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000023CE File Offset: 0x000005CE
		public void OnDisable()
		{
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(this.Update));
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005974 File Offset: 0x00003B74
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (this.targets.Count == 1)
			{
				GAFObjectInternal _target = this.targets[0];
				GUI.enabled = _target.serializedProperties.useCustomAtlasTextureRect;
				GUILayout.Space(5f);
				GUILayout.Space(5f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Select custom region", Array.Empty<GUILayoutOption>()))
				{
					GAFCustomSelectionWindow.GetWindow(_target.currentMaterial.mainTexture as Texture2D).onRectChange += delegate(Rect rect)
					{
						_target.serializedProperties.atlasTextureRect = rect;
						_target.serializedProperties.clip.reload();
					};
				}
				if (GUILayout.Button("Return defaults", Array.Empty<GUILayoutOption>()))
				{
					_target.serializedProperties.atlasTextureRect = this.getDefaultAtlasTextureData(_target);
					_target.reload();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.Space(5f);
			GUI.enabled = true;
			if (this.targets.Count == 1)
			{
				GAFObjectInternal _target = this.targets[0];
				GAFBaseClip clip = _target.serializedProperties.clip;
				Dictionary<float, List<GAFAtlasElementData>> customRegions = clip.asset.sharedData.customRegions;
				if (_target.type == GAFObjectType.Texture && customRegions != null && customRegions.Count > 0)
				{
					if (this.m_CustomRegions == null)
					{
						float scale = clip.settings.scale;
						this.m_CustomRegions = customRegions[scale].ToList<GAFAtlasElementData>();
						this.m_CustomRegionsNames = (from x in this.m_CustomRegions
						select x.linkageName).ToList<string>();
						this.m_CustomRegionsNames.Insert(0, "—");
					}
					else
					{
						int num = this.m_CustomRegions.FindIndex((GAFAtlasElementData x) => (ulong)x.id == (ulong)((long)_target.serializedProperties.atlasCustomElementID));
						if (num < 0)
						{
							num = 0;
						}
						else
						{
							num++;
						}
						EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUIContent guicontent = new GUIContent("Custom region:");
						Vector2 vector = GUIStyle.none.CalcSize(guicontent);
						EditorGUILayout.LabelField(new GUIContent("Custom region:", "​"), new GUILayoutOption[]
						{
							GUILayout.MaxWidth(vector.x + 5f)
						});
						int num2 = EditorGUILayout.Popup(num, this.m_CustomRegionsNames.ToArray(), Array.Empty<GUILayoutOption>());
						if (num != num2)
						{
							if (num2 == 0)
							{
								_target.serializedProperties.atlasCustomElementID = -1;
							}
							else
							{
								uint id = this.m_CustomRegions[num2 - 1].id;
								_target.serializedProperties.atlasCustomElementID = (int)id;
							}
							base.serializedObject.ApplyModifiedProperties();
							_target.reload();
						}
						EditorGUILayout.EndHorizontal();
						GUILayout.Space(5f);
						GUI.enabled = false;
						GUILayout.Space(100f);
					}
				}
				GUI.enabled = true;
			}
			foreach (GAFObjectInternal gafobjectInternal in this.targets)
			{
				if (gafobjectInternal.currentState != null)
				{
					GAFBaseClip clip2 = gafobjectInternal.serializedProperties.clip;
					Vector2 pivotOffset = clip2.settings.pivotOffset;
					float num3 = clip2.settings.pixelsPerUnit / clip2.settings.scale;
					Vector3 zero = Vector3.zero;
					if (!gafobjectInternal.serializedProperties.clip.settings.flipX)
					{
						zero.x = gafobjectInternal.currentState.localPosition.x / num3 + pivotOffset.x + gafobjectInternal.serializedProperties.offset.x + gafobjectInternal.serializedProperties.flip.x;
						zero.y = -gafobjectInternal.currentState.localPosition.y / num3 + pivotOffset.y + gafobjectInternal.serializedProperties.offset.y + gafobjectInternal.serializedProperties.flip.y;
						zero.z = gafobjectInternal.transform.localPosition.z;
					}
					else
					{
						zero.x = gafobjectInternal.currentState.localPosition.x / num3 - pivotOffset.x - gafobjectInternal.serializedProperties.offset.x + gafobjectInternal.serializedProperties.flip.x;
						zero.y = -gafobjectInternal.currentState.localPosition.y / num3 - pivotOffset.y - gafobjectInternal.serializedProperties.offset.y + gafobjectInternal.serializedProperties.flip.y;
						zero.z = gafobjectInternal.transform.localPosition.z;
					}
					if (zero != gafobjectInternal.transform.localPosition)
					{
						gafobjectInternal.transform.localPosition = zero;
						gafobjectInternal.serializedProperties.clip.reload();
						EditorUtility.SetDirty(gafobjectInternal);
					}
				}
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000208D File Offset: 0x0000028D
		private void recalculateTexture()
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005EB0 File Offset: 0x000040B0
		private Rect getDefaultAtlasTextureData(GAFObjectInternal _Object)
		{
			GAFBaseClip clip = _Object.serializedProperties.clip;
			uint atlasElementID = _Object.serializedProperties.atlasElementID;
			GAFAtlasElementData element = clip.asset.getAtlases(clip.timelineID).Find((GAFAtlasData atlas) => atlas.scale == clip.settings.scale).getElement(atlasElementID);
			float x = element.x;
			float y = element.y;
			float width = element.width;
			float height = element.height;
			return new Rect(x, y, width, height);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005F3C File Offset: 0x0000413C
		private void Update()
		{
			foreach (GAFObjectInternal gafobjectInternal in this.targets)
			{
				if (gafobjectInternal.currentState != null && gafobjectInternal.currentState != GAFObjectStateData.defaultState)
				{
					GAFBaseClip clip = gafobjectInternal.serializedProperties.clip;
					Vector2 pivotOffset = clip.settings.pivotOffset;
					float num = clip.settings.pixelsPerUnit / clip.settings.scale;
					Vector3 zero = Vector3.zero;
					if (!gafobjectInternal.serializedProperties.clip.settings.flipX)
					{
						zero.x = gafobjectInternal.currentState.localPosition.x / num + pivotOffset.x + gafobjectInternal.serializedProperties.offset.x + gafobjectInternal.serializedProperties.flip.x;
						zero.y = -gafobjectInternal.currentState.localPosition.y / num + pivotOffset.y + gafobjectInternal.serializedProperties.offset.y + gafobjectInternal.serializedProperties.flip.y;
						zero.z = gafobjectInternal.transform.localPosition.z;
					}
					else
					{
						zero.x = gafobjectInternal.currentState.localPosition.x / num - pivotOffset.x - gafobjectInternal.serializedProperties.offset.x + gafobjectInternal.serializedProperties.flip.x;
						zero.y = -gafobjectInternal.currentState.localPosition.y / num - pivotOffset.y - gafobjectInternal.serializedProperties.offset.y + gafobjectInternal.serializedProperties.flip.y;
						zero.z = gafobjectInternal.transform.localPosition.z;
					}
					if (zero != gafobjectInternal.transform.localPosition)
					{
						if (!gafobjectInternal.serializedProperties.clip.settings.flipX)
						{
							gafobjectInternal.serializedProperties.offset = new Vector2(gafobjectInternal.transform.localPosition.x - gafobjectInternal.currentState.localPosition.x / num - pivotOffset.x - gafobjectInternal.serializedProperties.flip.x, gafobjectInternal.transform.localPosition.y + gafobjectInternal.currentState.localPosition.y / num - pivotOffset.y - gafobjectInternal.serializedProperties.flip.y);
						}
						else
						{
							gafobjectInternal.serializedProperties.offset = new Vector2(gafobjectInternal.transform.localPosition.x - gafobjectInternal.currentState.localPosition.x / num + pivotOffset.x - gafobjectInternal.serializedProperties.flip.x, gafobjectInternal.transform.localPosition.y + gafobjectInternal.currentState.localPosition.y / num + pivotOffset.y - gafobjectInternal.serializedProperties.flip.y);
						}
						gafobjectInternal.serializedProperties.clip.reload();
						EditorUtility.SetDirty(gafobjectInternal);
					}
				}
			}
		}

		// Token: 0x04000067 RID: 103
		private List<GAFAtlasElementData> m_CustomRegions;

		// Token: 0x04000068 RID: 104
		private List<string> m_CustomRegionsNames;

		// Token: 0x04000069 RID: 105
		private Texture2D m_Texture;
	}
}
