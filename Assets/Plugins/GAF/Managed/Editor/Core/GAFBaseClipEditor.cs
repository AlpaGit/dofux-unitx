using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GAF.Managed.Editor.Assets;
using GAF.Managed.Editor.Tracking;
using GAF.Managed.GAFInternal.Assets;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GAF.Managed.Editor.Core
{
	// Token: 0x02000028 RID: 40
	[CustomEditor(typeof(GAFBaseClip))]
	public class GAFBaseClipEditor<TypeOfResource> : UnityEditor.Editor where TypeOfResource : GAFTexturesResourceInternal
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000025CC File Offset: 0x000007CC
		protected List<GAFBaseClip> targets
		{
			get
			{
				return base.targets.ToList<Object>().ConvertAll<GAFBaseClip>((Object target) => (GAFBaseClip)target);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000025FD File Offset: 0x000007FD
		protected GAFBaseClip target
		{
			get
			{
				return base.target as GAFBaseClip;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000678C File Offset: 0x0000498C
		protected void drawAsset()
		{
			SerializedProperty serializedProperty = base.serializedObject.FindProperty("m_IsInitialized");
			SerializedProperty serializedProperty2 = base.serializedObject.FindProperty("m_GAFAsset");
			if (!serializedProperty.hasMultipleDifferentValues)
			{
				if (!serializedProperty.boolValue)
				{
					if (this.hasPrefabs())
					{
						GUILayout.Space(10f);
						EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
						EditorGUILayout.HelpBox("Cannot init movie clip in prefab!", MessageType.Warning);
						EditorGUILayout.EndVertical();
						return;
					}
					GUILayout.Space(10f);
					EditorGUILayout.PropertyField(serializedProperty2, new GUIContent("Asset:"), Array.Empty<GUILayoutOption>());
					if (serializedProperty2.objectReferenceValue != null && !serializedProperty2.hasMultipleDifferentValues)
					{
						GAFAnimationAssetInternal gafanimationAssetInternal = (GAFAnimationAssetInternal)serializedProperty2.objectReferenceValue;
						if (!gafanimationAssetInternal.isLoaded)
						{
							this.drawAssetIsNotLoaded(gafanimationAssetInternal);
						}
						else
						{
							this.drawChooseTimeline(gafanimationAssetInternal);
							this.drawInitMovieClipButton(gafanimationAssetInternal);
						}
						this.m_AssetPath = AssetDatabase.GetAssetPath(gafanimationAssetInternal);
						return;
					}
				}
				else
				{
					if (serializedProperty2.hasMultipleDifferentValues)
					{
						GUILayout.Space(10f);
						EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
						EditorGUILayout.HelpBox("Multiple assets...", MessageType.Info);
						EditorGUILayout.EndVertical();
						return;
					}
					GAFAnimationAssetInternal gafanimationAssetInternal2 = (GAFAnimationAssetInternal)serializedProperty2.objectReferenceValue;
					if (!(gafanimationAssetInternal2 != null))
					{
						if (!string.IsNullOrEmpty(this.m_AssetPath))
						{
							GAFAnimationAssetInternal gafanimationAssetInternal3 = AssetDatabase.LoadAssetAtPath<GAFAnimationAssetInternal>(this.m_AssetPath);
							if (!(gafanimationAssetInternal3 != null))
							{
								return;
							}
							gafanimationAssetInternal3.load();
							using (List<GAFBaseClip>.Enumerator enumerator = this.targets.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									GAFBaseClip gafbaseClip = enumerator.Current;
									gafbaseClip.initialize(gafanimationAssetInternal3, gafbaseClip.timelineID);
									gafbaseClip.reload();
								}
								return;
							}
						}
						GUILayout.Space(10f);
						EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
						EditorGUILayout.LabelField("Asset is not found!", EditorStyles.boldLabel, Array.Empty<GUILayoutOption>());
						EditorGUILayout.EndVertical();
						return;
					}
					if (!gafanimationAssetInternal2.isLoaded)
					{
						this.drawAssetIsNotLoaded(gafanimationAssetInternal2);
						this.m_AssetPath = AssetDatabase.GetAssetPath(gafanimationAssetInternal2);
						return;
					}
					GUILayout.Space(10f);
					GAFAnimationAssetInternal gafanimationAssetInternal4 = EditorGUILayout.ObjectField("Asset: ", gafanimationAssetInternal2, typeof(GAFAnimationAssetInternal), false, Array.Empty<GUILayoutOption>()) as GAFAnimationAssetInternal;
					if (gafanimationAssetInternal4 != gafanimationAssetInternal2)
					{
						foreach (GAFBaseClip gafbaseClip2 in this.targets)
						{
							gafbaseClip2.clear(true);
						}
						if (gafanimationAssetInternal4 != null && gafanimationAssetInternal4.isLoaded)
						{
							foreach (GAFBaseClip gafbaseClip3 in this.targets)
							{
								gafbaseClip3.initialize(gafanimationAssetInternal4, gafbaseClip3.timelineID);
								gafbaseClip3.reload();
							}
						}
						this.m_AssetPath = AssetDatabase.GetAssetPath(gafanimationAssetInternal4);
						return;
					}
				}
			}
			else
			{
				GUILayout.Space(10f);
				EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
				EditorGUILayout.HelpBox("Different clip states...", MessageType.Info);
				EditorGUILayout.EndVertical();
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00006AC0 File Offset: 0x00004CC0
		protected void drawResourcesState()
		{
			if ((from clip in this.targets
			where clip.isInitialized
			select clip).Count<GAFBaseClip>() == this.targets.Count)
			{
				IEnumerable<GAFBaseClip> source = from clip in this.targets
				where clip.resource != null && clip.resource.isValid && clip.resource.isReady
				select clip;
				if (source.Count<GAFBaseClip>() == 0)
				{
					this.drawResourcesMissing();
					return;
				}
				if (source.Count<GAFBaseClip>() != this.targets.Count)
				{
					this.drawDifferentResourcesState();
					return;
				}
				this.drawCorrectResourcesState();
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00006B64 File Offset: 0x00004D64
		protected void drawPlaceholder()
		{
			GUILayout.Space(3f);
			this.m_ShowPlaceholder = EditorGUILayout.Foldout(this.m_ShowPlaceholder, "Placeholder management");
			if (this.m_ShowPlaceholder)
			{
				EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
				SerializedProperty serializedProperty = base.serializedObject.FindProperty("m_UsePlaceholder");
				SerializedProperty serializedProperty2 = base.serializedObject.FindProperty("m_PlaceholderMaterial");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(serializedProperty, new GUIContent("Enable placeholder mode:"), Array.Empty<GUILayoutOption>());
				if (EditorGUI.EndChangeCheck())
				{
					foreach (GAFBaseClip gafbaseClip in this.targets)
					{
						gafbaseClip.usePlaceholder = serializedProperty.boolValue;
					}
					base.serializedObject.ApplyModifiedProperties();
				}
				if (!serializedProperty.hasMultipleDifferentValues && serializedProperty.boolValue)
				{
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(serializedProperty2, new GUIContent("Placeholder material:"), Array.Empty<GUILayoutOption>());
					if (EditorGUI.EndChangeCheck())
					{
						base.serializedObject.ApplyModifiedProperties();
						InternalEditorUtility.RepaintAllViews();
					}
					SerializedProperty serializedProperty3 = base.serializedObject.FindProperty("m_PlaceholderSize");
					if (!serializedProperty3.hasMultipleDifferentValues)
					{
						Vector2 vector = EditorGUILayout.Vector2Field("Placeholder size:", serializedProperty3.vector2Value, Array.Empty<GUILayoutOption>());
						if (!(vector != serializedProperty3.vector2Value))
						{
							goto IL_190;
						}
						using (List<GAFBaseClip>.Enumerator enumerator = this.targets.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								GAFBaseClip gafbaseClip2 = enumerator.Current;
								gafbaseClip2.placeholderSize = vector;
							}
							goto IL_190;
						}
					}
					EditorGUILayout.HelpBox("Different size values!", MessageType.Info);
					IL_190:
					SerializedProperty serializedProperty4 = base.serializedObject.FindProperty("m_PlaceholderOffset");
					if (!serializedProperty4.hasMultipleDifferentValues)
					{
						Vector2 vector2 = EditorGUILayout.Vector2Field("Placeholder offset:", serializedProperty4.vector2Value, Array.Empty<GUILayoutOption>());
						if (!(vector2 != serializedProperty4.vector2Value))
						{
							goto IL_214;
						}
						using (List<GAFBaseClip>.Enumerator enumerator = this.targets.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								GAFBaseClip gafbaseClip3 = enumerator.Current;
								gafbaseClip3.placeholderOffset = vector2;
							}
							goto IL_214;
						}
					}
					EditorGUILayout.HelpBox("Different offset values!", MessageType.Info);
				}
				IL_214:
				EditorGUILayout.EndVertical();
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00006DB4 File Offset: 0x00004FB4
		protected void drawResourceManagement()
		{
			if (!Application.isPlaying)
			{
				SerializedProperty serializedProperty = base.serializedObject.FindProperty("m_GetResourceDelegate");
				SerializedProperty serializedProperty2 = serializedProperty.FindPropertyRelative("m_Data");
				SerializedProperty serializedProperty3 = serializedProperty.FindPropertyRelative("m_ReloadFunc");
				SerializedProperty serializedProperty4 = base.serializedObject.FindProperty("m_UseCustomDelegate");
				GUILayout.Space(3f);
				this.m_ShowResourceManagement = EditorGUILayout.Foldout(this.m_ShowResourceManagement, "Resource management");
				if (this.m_ShowResourceManagement)
				{
					EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(serializedProperty4, new GUIContent("Use custom resource loader"), Array.Empty<GUILayoutOption>());
					if (EditorGUI.EndChangeCheck())
					{
						base.serializedObject.ApplyModifiedProperties();
						foreach (GAFBaseClip gafbaseClip in this.targets)
						{
							if (!serializedProperty4.boolValue)
							{
								gafbaseClip.customGetResourceDelegate = null;
							}
							gafbaseClip.resource = null;
							gafbaseClip.reload();
						}
					}
					if (!serializedProperty4.hasMultipleDifferentValues && serializedProperty4.boolValue)
					{
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(serializedProperty2, new GUIContent(""), true, Array.Empty<GUILayoutOption>());
						if (EditorGUI.EndChangeCheck())
						{
							serializedProperty3.boolValue = true;
							base.serializedObject.ApplyModifiedProperties();
							foreach (GAFBaseClip gafbaseClip2 in this.targets)
							{
								gafbaseClip2.resource = null;
								gafbaseClip2.reload();
							}
						}
					}
					EditorGUILayout.EndVertical();
				}
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00006F60 File Offset: 0x00005160
		protected void drawDataButtons()
		{
			SerializedProperty serializedProperty = base.serializedObject.FindProperty("m_IsInitialized");
			SerializedProperty serializedProperty2 = base.serializedObject.FindProperty("m_GAFAsset");
			if (!serializedProperty.hasMultipleDifferentValues && serializedProperty.boolValue)
			{
				GUILayout.Space(7f);
				if (!serializedProperty2.hasMultipleDifferentValues && serializedProperty2.objectReferenceValue != null)
				{
					GAFAnimationAssetInternal gafanimationAssetInternal = (GAFAnimationAssetInternal)serializedProperty2.objectReferenceValue;
					if (gafanimationAssetInternal != null && gafanimationAssetInternal.isLoaded)
					{
						this.drawBuildResources(gafanimationAssetInternal);
					}
				}
				this.drawReloadAnimationButton();
			}
			this.drawClearButtons();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006FF0 File Offset: 0x000051F0
		protected void drawAssetIsNotLoaded(GAFAnimationAssetInternal _Asset)
		{
			GUILayout.Space(3f);
			EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
			EditorGUILayout.LabelField("Asset '" + _Asset.name + "' is not loaded properly! Try to reimport .GAF file!", Array.Empty<GUILayoutOption>());
			EditorGUILayout.EndVertical();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000260A File Offset: 0x0000080A
		protected void drawResourcesMissing()
		{
			GUILayout.Space(3f);
			EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
			EditorGUILayout.HelpBox("Your animation(s) missing resources! \nImport necessary textures OR Build resources OR Ensure your custom delegate works!", MessageType.Error);
			EditorGUILayout.EndVertical();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00002637 File Offset: 0x00000837
		protected void drawDifferentResourcesState()
		{
			GUILayout.Space(3f);
			EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
			EditorGUILayout.HelpBox("Some of selected movie clips misses resource!", MessageType.Error);
			EditorGUILayout.EndVertical();
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00002664 File Offset: 0x00000864
		protected void drawCorrectResourcesState()
		{
			GUILayout.Space(3f);
			EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
			EditorGUILayout.HelpBox("Resources are available!", MessageType.Info);
			EditorGUILayout.EndVertical();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00007040 File Offset: 0x00005240
		protected void drawBuildResources(GAFAnimationAssetInternal _Asset)
		{
			SerializedProperty serializedProperty = base.serializedObject.FindProperty("m_UseCustomDelegate");
			bool enabled = GUI.enabled;
			GUI.enabled = (!serializedProperty.hasMultipleDifferentValues && !serializedProperty.boolValue);
			GUILayout.Space(3f);
			if (GUILayout.Button("Build GAF resources", Array.Empty<GUILayoutOption>()))
			{
				GAFResourceManagerInternal.instance.createResources<TypeOfResource>(_Asset);
				foreach (GAFBaseClip gafbaseClip in this.targets)
				{
					gafbaseClip.reload();
				}
			}
			GUI.enabled = enabled;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000070F0 File Offset: 0x000052F0
		protected void drawChooseTimeline(GAFAnimationAssetInternal _Asset)
		{
			if (_Asset.getTimelines().Count > 1)
			{
				GUILayout.Space(6f);
				EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
				EditorGUILayout.LabelField("Choose timeline ID:", Array.Empty<GUILayoutOption>());
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				string[] array = _Asset.getTimelines().ConvertAll<string>((GAFTimelineData timeline) => timeline.id.ToString() + ((timeline.linkageName.Length > 0) ? (" - " + timeline.linkageName) : "")).ToArray();
				int num = GUILayout.SelectionGrid(this.m_TimelineIndex, array, (array.Length < 4) ? array.Length : 4, Array.Empty<GUILayoutOption>());
				if (num != this.m_TimelineIndex)
				{
					this.m_TimelineIndex = num;
					string text = array[num];
					this.m_TimelineID = ((text.IndexOf(" - ") > 0) ? int.Parse(text.Split(new char[]
					{
						'-'
					})[0]) : int.Parse(text));
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
				return;
			}
			GUILayout.Space(6f);
			EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
			EditorGUILayout.LabelField("Timeline ID: 0 - rootTimeline", Array.Empty<GUILayoutOption>());
			this.m_TimelineID = 0;
			this.m_TimelineIndex = 0;
			EditorGUILayout.EndVertical();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00007228 File Offset: 0x00005428
		protected void drawInitMovieClipButton(GAFAnimationAssetInternal _Asset)
		{
			GUILayout.Space(3f);
			if (GUILayout.Button("Create GAF movie clip", Array.Empty<GUILayoutOption>()))
			{
				foreach (GAFBaseClip gafbaseClip in this.targets)
				{
					gafbaseClip.initialize(_Asset, this.m_TimelineID);
					gafbaseClip.reload();
					GAFTracking.sendMovieClipCreatedRequest(_Asset.name);
				}
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000072AC File Offset: 0x000054AC
		protected void drawSortingID()
		{
			SerializedProperty serializedProperty = base.serializedObject.FindProperty("m_Settings");
			SerializedProperty sortinglayerIDProperty = serializedProperty.FindPropertyRelative("m_SpriteLayerID");
			SerializedProperty serializedProperty2 = serializedProperty.FindPropertyRelative("m_SpriteLayerName");
			List<string> list = this.getSortingLayerNames().ToList<string>();
			List<int> list2 = this.getSortingLayerUniqueIDs().ToList<int>();
			int num = -1;
			if (sortinglayerIDProperty.hasMultipleDifferentValues)
			{
				list.Insert(0, "—");
			}
			else
			{
				num = list2.FindIndex((int __id) => __id == sortinglayerIDProperty.intValue);
			}
			if (num < 0)
			{
				num = list2.FindIndex((int __id) => __id == 0);
			}
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUIStyle guistyle = (sortinglayerIDProperty.isInstantiatedPrefab && sortinglayerIDProperty.prefabOverride) ? EditorStyles.boldLabel : EditorStyles.label;
			EditorGUILayout.LabelField(new GUIContent("Sorting layer: ", "The layer used to define this animation’s overlay priority during rendering.​"), guistyle, Array.Empty<GUILayoutOption>());
			int num2 = EditorGUILayout.Popup(num, list.ToArray(), Array.Empty<GUILayoutOption>());
			if (num != num2)
			{
				int index = sortinglayerIDProperty.hasMultipleDifferentValues ? (num2 - 1) : num2;
				sortinglayerIDProperty.intValue = list2[index];
				serializedProperty2.stringValue = list[index];
				base.serializedObject.ApplyModifiedProperties();
				this.reloadTargets();
			}
			EditorGUILayout.EndHorizontal();
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00007424 File Offset: 0x00005624
		protected void drawScales(SerializedProperty _AssetProperty)
		{
			GAFAnimationAssetInternal gafanimationAssetInternal = (GAFAnimationAssetInternal)_AssetProperty.objectReferenceValue;
			if (gafanimationAssetInternal != null && gafanimationAssetInternal.isLoaded)
			{
				SerializedProperty serializedProperty = base.serializedObject.FindProperty("m_Settings");
				SerializedProperty scaleProperty = serializedProperty.FindPropertyRelative("m_Scale");
				List<string> list = gafanimationAssetInternal.scales.ConvertAll<string>((float __scale) => __scale.ToString());
				int num = 0;
				if (scaleProperty.hasMultipleDifferentValues)
				{
					list.Insert(0, "—");
				}
				else
				{
					num = gafanimationAssetInternal.scales.FindIndex((float __scale) => __scale == scaleProperty.floatValue);
				}
				GUILayout.Space(3f);
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUIStyle guistyle = (scaleProperty.isInstantiatedPrefab && scaleProperty.prefabOverride) ? EditorStyles.boldLabel : EditorStyles.label;
				EditorGUILayout.LabelField(new GUIContent("Texture atlas scale: ", "Ability to change animation’s scale if you convert your animation with at least two scales. [float value]​"), guistyle, Array.Empty<GUILayoutOption>());
				int num2 = EditorGUILayout.Popup(num, list.ToArray(), Array.Empty<GUILayoutOption>());
				if (num != num2)
				{
					scaleProperty.floatValue = gafanimationAssetInternal.scales[scaleProperty.hasMultipleDifferentValues ? (num2 - 1) : num2];
					base.serializedObject.ApplyModifiedProperties();
					this.reloadTargets();
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00007598 File Offset: 0x00005798
		protected void drawCsfs(SerializedProperty _AssetProperty)
		{
			GAFAnimationAssetInternal gafanimationAssetInternal = (GAFAnimationAssetInternal)_AssetProperty.objectReferenceValue;
			if (gafanimationAssetInternal != null && gafanimationAssetInternal.isLoaded)
			{
				SerializedProperty serializedProperty = base.serializedObject.FindProperty("m_Settings");
				SerializedProperty csfProperty = serializedProperty.FindPropertyRelative("m_CSF");
				List<string> list = gafanimationAssetInternal.csfs.ConvertAll<string>((float __csf) => __csf.ToString());
				int num = 0;
				if (csfProperty.hasMultipleDifferentValues)
				{
					list.Insert(0, "—");
				}
				else
				{
					num = gafanimationAssetInternal.csfs.FindIndex((float __csf) => __csf == csfProperty.floatValue);
				}
				GUILayout.Space(3f);
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUIStyle guistyle = (csfProperty.isInstantiatedPrefab && csfProperty.prefabOverride) ? EditorStyles.boldLabel : EditorStyles.label;
				EditorGUILayout.LabelField(new GUIContent("Content scale factor: ", "Ability to use bigger textures in the same mesh if you convert your animation with two scale factors (for example 1 and 2 for non retina and retina). [integer value]​"), guistyle, Array.Empty<GUILayoutOption>());
				int num2 = EditorGUILayout.Popup(num, list.ToArray(), Array.Empty<GUILayoutOption>());
				if (num != num2)
				{
					csfProperty.floatValue = gafanimationAssetInternal.csfs[csfProperty.hasMultipleDifferentValues ? (num2 - 1) : num2];
					base.serializedObject.ApplyModifiedProperties();
					this.reloadTargets();
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00002691 File Offset: 0x00000891
		protected void drawReloadAnimationButton()
		{
			GUILayout.Space(3f);
			if (GUILayout.Button("Reload animation", Array.Empty<GUILayoutOption>()))
			{
				this.reloadTargets();
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000770C File Offset: 0x0000590C
		protected void drawClearButtons()
		{
			GUILayout.Space(3f);
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Clear animation", Array.Empty<GUILayoutOption>()))
			{
				foreach (GAFBaseClip gafbaseClip in this.targets)
				{
					if (gafbaseClip.isInitialized && PrefabUtility.GetPrefabType(gafbaseClip.gameObject) != PrefabType.Prefab)
					{
						gafbaseClip.clear(false);
						EditorUtility.SetDirty(gafbaseClip);
					}
				}
				this.m_TimelineIndex = 0;
				this.m_TimelineID = 0;
			}
			if (GUILayout.Button("Clear animation (delete children)", Array.Empty<GUILayoutOption>()))
			{
				foreach (GAFBaseClip gafbaseClip2 in this.targets)
				{
					if (gafbaseClip2.isInitialized && PrefabUtility.GetPrefabType(gafbaseClip2.gameObject) != PrefabType.Prefab)
					{
						gafbaseClip2.clear(true);
						EditorUtility.SetDirty(gafbaseClip2);
					}
				}
				this.m_TimelineIndex = 0;
				this.m_TimelineID = 0;
			}
			EditorGUILayout.EndHorizontal();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00007834 File Offset: 0x00005A34
		protected void drawProperty(SerializedProperty _Property, GUIContent _Content)
		{
			EditorGUI.showMixedValue = _Property.hasMultipleDifferentValues;
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUIStyle guistyle = (_Property.isInstantiatedPrefab && _Property.prefabOverride) ? EditorStyles.boldLabel : EditorStyles.label;
			EditorGUILayout.LabelField(_Content, guistyle, Array.Empty<GUILayoutOption>());
			EditorGUILayout.PropertyField(_Property, new GUIContent(""), true, Array.Empty<GUILayoutOption>());
			EditorGUILayout.EndHorizontal();
			EditorGUI.showMixedValue = false;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000078A8 File Offset: 0x00005AA8
		protected void drawDecomposeFlashTransform(SerializedProperty _AssetProperty, SerializedProperty _SettingsProperty)
		{
			if (this.targets.All((GAFBaseClip clip) => clip.asset != null && clip.asset.isLoaded))
			{
				GUI.enabled = true;
				SerializedProperty serializedProperty = _SettingsProperty.FindPropertyRelative("m_DecomposeFlashTransform");
				EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUIStyle guistyle = (serializedProperty.isInstantiatedPrefab && serializedProperty.prefabOverride) ? EditorStyles.boldLabel : EditorStyles.label;
				EditorGUILayout.LabelField(new GUIContent("Decompose Flash transform: ", "​"), guistyle, Array.Empty<GUILayoutOption>());
				EditorGUILayout.PropertyField(serializedProperty, new GUIContent(""), true, Array.Empty<GUILayoutOption>());
				EditorGUILayout.EndHorizontal();
				EditorGUI.showMixedValue = false;
				GUI.enabled = true;
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00007960 File Offset: 0x00005B60
		protected void reloadTargets()
		{
			foreach (GAFBaseClip gafbaseClip in this.targets)
			{
				if (gafbaseClip.isInitialized && PrefabUtility.GetPrefabType(gafbaseClip.gameObject) != PrefabType.Prefab)
				{
					gafbaseClip.reload();
					EditorUtility.SetDirty(gafbaseClip);
				}
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000079D0 File Offset: 0x00005BD0
		protected void reloadDecompose()
		{
			foreach (GAFBaseClip gafbaseClip in this.targets)
			{
				if (gafbaseClip.isInitialized && PrefabUtility.GetPrefabType(gafbaseClip.gameObject) != PrefabType.Prefab)
				{
					GAFBaseClip[] componentsInChildren = gafbaseClip.GetComponentsInChildren<GAFBaseClip>();
					for (int i = 1; i < componentsInChildren.Length; i++)
					{
						SerializedObject serializedObject = new SerializedObject(componentsInChildren[i]);
						serializedObject.FindProperty("m_Settings").FindPropertyRelative("m_DecomposeFlashTransform").boolValue = gafbaseClip.settings.decomposeFlashTransform;
						serializedObject.ApplyModifiedProperties();
					}
					gafbaseClip.reload();
					EditorUtility.SetDirty(gafbaseClip);
				}
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00007A88 File Offset: 0x00005C88
		protected bool hasPrefabs()
		{
			bool result = false;
			using (List<GAFBaseClip>.Enumerator enumerator = this.targets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (PrefabUtility.GetPrefabType(enumerator.Current.gameObject) == PrefabType.Prefab)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000026B5 File Offset: 0x000008B5
		private List<string> getSortingLayerNames()
		{
			return ((string[])typeof(InternalEditorUtility).GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, Array.Empty<object>())).ToList<string>();
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000026E3 File Offset: 0x000008E3
		private List<int> getSortingLayerUniqueIDs()
		{
			return ((int[])typeof(InternalEditorUtility).GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, Array.Empty<object>())).ToList<int>();
		}

		// Token: 0x04000081 RID: 129
		private string m_AssetPath;

		// Token: 0x04000082 RID: 130
		private int m_TimelineIndex;

		// Token: 0x04000083 RID: 131
		private int m_TimelineID;

		// Token: 0x04000084 RID: 132
		private bool m_ShowResourceManagement = true;

		// Token: 0x04000085 RID: 133
		private bool m_ShowPlaceholder = true;
	}
}
