using System;
using System.Collections.Generic;
using System.Linq;
using GAF.Managed.GAFInternal.Assets;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GAF.Managed.Editor.Assets
{
	// Token: 0x02000034 RID: 52
	[CanEditMultipleObjects]
	[CustomEditor(typeof(GAFAnimationAssetInternal))]
	public class GAFAnimationAssetInternalEditor<TypeOfResource> : UnityEditor.Editor where TypeOfResource : GAFTexturesResourceInternal
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00002906 File Offset: 0x00000B06
		protected GAFAnimationAssetInternal target
		{
			get
			{
				return base.target as GAFAnimationAssetInternal;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00002913 File Offset: 0x00000B13
		private List<GAFAnimationAssetInternal> targets
		{
			get
			{
				return base.targets.ToList<Object>().ConvertAll<GAFAnimationAssetInternal>((Object _target) => _target as GAFAnimationAssetInternal);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00002944 File Offset: 0x00000B44
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x0000294C File Offset: 0x00000B4C
		protected AssetState state { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00002955 File Offset: 0x00000B55
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x0000295D File Offset: 0x00000B5D
		private int clipTypeSelectionIndex
		{
			get
			{
				return m_ClipTypeSelectionIndex;
			}
			set
			{
				m_ClipTypeSelectionIndex = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00002966 File Offset: 0x00000B66
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x0000296E File Offset: 0x00000B6E
		private int clipObjectsTypeSelectionIndex { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00002977 File Offset: 0x00000B77
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x0000297F File Offset: 0x00000B7F
		protected GAFTimelineData currentTimeline { get; set; }

		// Token: 0x060000F9 RID: 249 RVA: 0x00008658 File Offset: 0x00006858
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			GUILayout.Space(5f);
			bool enabled = GUI.enabled;
			GUI.enabled = true;
			SerializedProperty serializedProperty = serializedObject.FindProperty("m_EnableSequenceCaching");
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = serializedProperty.hasMultipleDifferentValues;
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUIStyle guistyle = (serializedProperty.isInstantiatedPrefab && serializedProperty.prefabOverride) ? EditorStyles.boldLabel : EditorStyles.label;
			EditorGUILayout.LabelField("Enable Sequence Caching", guistyle, Array.Empty<GUILayoutOption>());
			EditorGUILayout.PropertyField(serializedProperty, new GUIContent(""), true, Array.Empty<GUILayoutOption>());
			EditorGUILayout.EndHorizontal();
			EditorGUI.showMixedValue = false;
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				foreach (GAFAnimationAssetInternal gafanimationAssetInternal in this.targets)
				{
					gafanimationAssetInternal.defineKeyFrames();
				}
			}
			GUI.enabled = enabled;
			if (m_Timelines == null)
			{
				m_Timelines = target.getTimelines();
				if (m_Timelines.Count > 0)
				{
					currentTimeline = m_Timelines[0];
					m_CurrentTimelineIndex = 0;
					m_TimelinesCount = m_Timelines.Count;
				}
			}
			GUI.enabled = true;
			List<GAFAnimationAssetInternal> targets = this.targets;
			int num = (from _target in targets
			where _target.isLoaded
			select _target).Count<GAFAnimationAssetInternal>();
			state = ((targets.Count > 1) ? AssetState.Multiple : AssetState.Single);
			state |= ((num == targets.Count) ? AssetState.Loaded : ((num == 0) ? AssetState.NotLoaded : AssetState.Mixed));
			drawResources();
			if (state == AssetState.SingleLoaded)
			{
				drawAudios();
			}
			drawGAFData();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00008838 File Offset: 0x00006A38
		private void drawResources()
		{
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("Resources: ", Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			AssetState state = this.state;
			switch (state)
			{
			case AssetState.SingleLoaded:
			case AssetState.SingleNotLoaded:
				drawResourcesData();
				goto IL_65;
			case AssetState.MultipleLoaded:
			case AssetState.MultipleNotLoaded:
				break;
			case AssetState.Single | AssetState.Multiple | AssetState.Loaded:
			case AssetState.NotLoaded:
				goto IL_65;
			default:
				if (state != AssetState.SomeNotLoaded)
				{
					goto IL_65;
				}
				break;
			}
			EditorGUILayout.HelpBox("Cannot view resources data for multiple assets.", MessageType.Info);
			IL_65:
			bool enabled = GUI.enabled;
			GUI.enabled = (enabled && (this.state == AssetState.SingleLoaded || this.state == AssetState.MultipleLoaded));
			if (GUILayout.Button("Rebuild GAF resources", Array.Empty<GUILayoutOption>()))
			{
				foreach (GAFAnimationAssetInternal asset in targets)
				{
					GAFResourceManagerInternal.instance.createResources<TypeOfResource>(asset);
				}
			}
			GUI.enabled = enabled;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00008934 File Offset: 0x00006B34
		private void drawMecanimResources()
		{
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("Mecanim resources: ", Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			AssetState state = this.state;
			switch (state)
			{
			case AssetState.SingleLoaded:
			case AssetState.SingleNotLoaded:
				drawMecanimResourcesData();
				goto IL_65;
			case AssetState.MultipleLoaded:
			case AssetState.MultipleNotLoaded:
				break;
			case AssetState.Single | AssetState.Multiple | AssetState.Loaded:
			case AssetState.NotLoaded:
				goto IL_65;
			default:
				if (state != AssetState.SomeNotLoaded)
				{
					goto IL_65;
				}
				break;
			}
			EditorGUILayout.HelpBox("Cannot view mecanim data for multiple assets.", MessageType.Info);
			IL_65:
			bool enabled = GUI.enabled;
			GUI.enabled = (enabled && (this.state == AssetState.SingleLoaded || this.state == AssetState.MultipleLoaded));
			if (GUILayout.Button("Rebuild mecanim resources", Array.Empty<GUILayoutOption>()))
			{
				foreach (GAFAnimationAssetInternal asset in targets)
				{
					GAFResourceManagerInternal.instance.createMecanimResources(asset);
				}
			}
			GUI.enabled = enabled;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00008A30 File Offset: 0x00006C30
		private void drawGAFData()
		{
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("GAF data:", Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			AssetState state = this.state;
			switch (state)
			{
			case AssetState.SingleLoaded:
				drawAssetData();
				drawTimelines();
				return;
			case AssetState.MultipleLoaded:
				EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
				EditorGUILayout.HelpBox("Cannot view GAF data for multiple assets.", MessageType.Info);
				GUILayout.Space(5f);
				if (targets.Any((GAFAnimationAssetInternal asset) => asset.hasNesting))
				{
					EditorGUILayout.HelpBox("Cannot view timeline creation options for multiple assets when one of them has nesting.", MessageType.Info);
				}
				else
				{
					EditorGUILayout.LabelField("Root timeline (if present) creation options:", Array.Empty<GUILayoutOption>());
					drawCreationMenuInternal(0);
				}
				EditorGUILayout.EndVertical();
				break;
			case AssetState.Single | AssetState.Multiple | AssetState.Loaded:
			case AssetState.NotLoaded:
				break;
			case AssetState.SingleNotLoaded:
				EditorGUILayout.HelpBox("Asset is not loaded! Please reload asset or reimport '.gaf' file.", MessageType.Error);
				return;
			case AssetState.MultipleNotLoaded:
				EditorGUILayout.HelpBox("Assets are not loaded! Please reload assets or reimport '.gaf' files.", MessageType.Error);
				return;
			default:
				if (state != AssetState.SomeNotLoaded)
				{
					return;
				}
				EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
				EditorGUILayout.HelpBox("Some of assets are not loaded! Please reload asset(s) or reimport '.gaf' file.", MessageType.Error);
				GUILayout.Space(5f);
				EditorGUILayout.LabelField("Root timeline (if present) creation options:", Array.Empty<GUILayoutOption>());
				drawCreationMenuInternal(0);
				EditorGUILayout.EndVertical();
				return;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00008B74 File Offset: 0x00006D74
		private void drawResourcesData()
		{
			EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
			foreach (string str in target.resourcesPaths)
			{
				string text = "Assets/" + str;
				GAFTexturesResourceInternal gaftexturesResourceInternal = AssetDatabase.LoadAssetAtPath(text, typeof(GAFTexturesResourceInternal)) as GAFTexturesResourceInternal;
				if (gaftexturesResourceInternal != null)
				{
					List<Texture2D> list = (from data in gaftexturesResourceInternal.data
					select data.sharedTexture).ToList<Texture2D>();
					List<Material> list2 = (from data in gaftexturesResourceInternal.data
					select data.sharedMaterial).ToList<Material>();
					EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Space(15f);
					EditorGUILayout.LabelField("Resource: " + text, Array.Empty<GUILayoutOption>());
					EditorGUILayout.EndHorizontal();
					GUILayout.Space(5f);
					EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Space(30f);
					EditorGUILayout.LabelField("Textures:", Array.Empty<GUILayoutOption>());
					EditorGUILayout.EndHorizontal();
					if (list != null && list.Count > 0)
					{
						for (int i = 0; i < list.Count; i++)
						{
							Texture2D texture2D = list[i];
							if (texture2D != null)
							{
								string assetPath = AssetDatabase.GetAssetPath(texture2D);
								EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								GUILayout.Space(45f);
								EditorGUILayout.LabelField((i + 1) + ") Found: " + assetPath, Array.Empty<GUILayoutOption>());
								EditorGUILayout.EndHorizontal();
							}
							else
							{
								EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								GUILayout.Space(45f);
								EditorGUILayout.LabelField(string.Concat(new string[]
								{
									(i + 1).ToString(),
									") Not found: ",
									gaftexturesResourceInternal.currentDataPath,
									gaftexturesResourceInternal.data[i].name,
									".png"
								}), Array.Empty<GUILayoutOption>());
								EditorGUILayout.EndHorizontal();
							}
						}
					}
					GUILayout.Space(3f);
					EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Space(30f);
					EditorGUILayout.LabelField("Materials:", Array.Empty<GUILayoutOption>());
					EditorGUILayout.EndHorizontal();
					if (list2 != null && list2.Count > 0)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							Material material = list2[j];
							if (material != null)
							{
								string assetPath2 = AssetDatabase.GetAssetPath(material);
								EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								GUILayout.Space(45f);
								EditorGUILayout.LabelField((j + 1) + ") Found: " + assetPath2, Array.Empty<GUILayoutOption>());
								EditorGUILayout.EndHorizontal();
							}
							else
							{
								EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								GUILayout.Space(45f);
								EditorGUILayout.LabelField(string.Concat(new string[]
								{
									(j + 1).ToString(),
									") Not found: ",
									gaftexturesResourceInternal.currentDataPath,
									gaftexturesResourceInternal.data[j].name,
									".mat"
								}), Array.Empty<GUILayoutOption>());
								EditorGUILayout.EndHorizontal();
							}
						}
					}
				}
				else
				{
					EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Space(15f);
					EditorGUILayout.HelpBox("There is no resource at path - " + text, MessageType.Error);
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndVertical();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000208D File Offset: 0x0000028D
		protected virtual void drawMecanimResourcesData()
		{
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00008F44 File Offset: 0x00007144
		private void drawAssetData()
		{
			EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
			GUILayout.Space(2f);
			EditorGUILayout.LabelField("GAF version: " + target.majorDataVersion + "." + target.minorDataVersion, Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("Available atlas scales: " + string.Join(",", target.scales.ConvertAll<string>((float scale) => scale.ToString()).ToArray()), Array.Empty<GUILayoutOption>());
			EditorGUILayout.LabelField("Available content scale factors: " + string.Join(",", target.csfs.ConvertAll<string>((float csf) => csf.ToString()).ToArray()), Array.Empty<GUILayoutOption>());
			EditorGUILayout.EndVertical();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00009064 File Offset: 0x00007264
		private void drawTimelines()
		{
			GUILayout.Space(5f);
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUIContent guicontent = new GUIContent("Timeline \"" + currentTimeline.linkageName + "\":", "Timeline \"" + currentTimeline.linkageName + "\"");
			float num = GUIStyle.none.CalcSize(guicontent).x + 3f;
			EditorGUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			EditorGUILayout.LabelField(guicontent, new GUILayoutOption[]
			{
				GUILayout.Width(num)
			});
			m_CurrentTimelineIndex = EditorGUILayout.IntField(m_CurrentTimelineIndex, new GUILayoutOption[]
			{
				GUILayout.Width(50f)
			});
			if (m_CurrentTimelineIndex <= 1)
			{
				m_CurrentTimelineIndex = 1;
			}
			else if (m_CurrentTimelineIndex > m_TimelinesCount)
			{
				m_CurrentTimelineIndex = m_TimelinesCount;
			}
			currentTimeline = m_Timelines[m_CurrentTimelineIndex - 1];
			num = GUIStyle.none.CalcSize(new GUIContent(" / " + m_TimelinesCount)).x + 3f;
			EditorGUILayout.LabelField(" / " + m_TimelinesCount, new GUILayoutOption[]
			{
				GUILayout.MaxWidth(num)
			});
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(5f);
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUI.enabled = (m_CurrentTimelineIndex > 1);
			if (GUILayout.Button("<", new GUILayoutOption[]
			{
				GUILayout.Width(35f)
			}))
			{
				m_CurrentTimelineIndex--;
			}
			GUI.enabled = true;
			GUI.enabled = (m_CurrentTimelineIndex != m_TimelinesCount);
			if (GUILayout.Button(">", new GUILayoutOption[]
			{
				GUILayout.Width(35f)
			}))
			{
				m_CurrentTimelineIndex++;
			}
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("ID - " + currentTimeline.id, Array.Empty<GUILayoutOption>());
			EditorGUILayout.LabelField("Linkage name - " + currentTimeline.linkageName, Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("Frame size: " + currentTimeline.frameSize, Array.Empty<GUILayoutOption>());
			EditorGUILayout.LabelField("Pivot: " + currentTimeline.pivot, Array.Empty<GUILayoutOption>());
			EditorGUILayout.LabelField("Frames count: " + currentTimeline.framesCount, Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("Available sequences: " + string.Join(",", currentTimeline.sequences.ConvertAll<string>((GAFSequenceData sequence) => sequence.name).ToArray()), Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("Objects count: " + currentTimeline.objects.Count, Array.Empty<GUILayoutOption>());
			EditorGUILayout.LabelField("Masks count: " + currentTimeline.masks.Count, Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			EditorGUILayout.LabelField("Creation options:", Array.Empty<GUILayoutOption>());
			drawCreationMenuInternal((int)currentTimeline.id);
			GUILayout.Space(5f);
			drawMecanimResourcesData();
			EditorGUILayout.EndVertical();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00009470 File Offset: 0x00007670
		private void drawAudios()
		{
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			m_AudioResourcesFoldout = EditorGUILayout.Foldout(m_AudioResourcesFoldout, "Audio resources: ");
			EditorGUILayout.EndHorizontal();
			if (m_AudioResourcesFoldout)
			{
				GUILayout.Space(5f);
				EditorGUILayout.BeginVertical(EditorStyles.textField, Array.Empty<GUILayoutOption>());
				for (int i = 0; i < target.audioResources.Count; i++)
				{
					GAFAnimationAssetInternal.GAFSound gafsound = target.audioResources[i];
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					EditorGUILayout.LabelField("ID:", new GUILayoutOption[]
					{
						GUILayout.Width(25f)
					});
					EditorGUILayout.LabelField(gafsound.ID.ToString(), new GUIStyle
					{
						fontStyle = FontStyle.Bold
					}, new GUILayoutOption[]
					{
						GUILayout.Width(20f)
					});
					EditorGUILayout.LabelField("Source:", new GUILayoutOption[]
					{
						GUILayout.Width(50f)
					});
					EditorGUILayout.ObjectField(gafsound.audio, typeof(AudioClip), true, Array.Empty<GUILayoutOption>());
					GUILayout.EndHorizontal();
					GUILayout.Space(5f);
				}
				EditorGUILayout.EndVertical();
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000095A4 File Offset: 0x000077A4
		private void drawCreationMenuInternal(int _TimelineID)
		{
			GUIStyle radioButton = EditorStyles.radioButton;
			float num = GUIStyle.none.CalcSize(new GUIContent("Movie clip")).x + 25f;
			float num2 = GUIStyle.none.CalcSize(new GUIContent("Animator")).x + 25f;
			float num3 = num + num2;
			string[] array = new string[]
			{
				"Movie clip",
				"Animator"
			};
			string[] array2 = new string[]
			{
				"Not baked",
				"Baked"
			};
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			clipTypeSelectionIndex = GUILayout.SelectionGrid(clipTypeSelectionIndex, array, array.Length, radioButton, new GUILayoutOption[]
			{
				GUILayout.Width(num3)
			});
			EditorGUILayout.EndHorizontal();
			if (!target.hasNesting)
			{
				clipObjectsTypeSelectionIndex = GUILayout.SelectionGrid(clipObjectsTypeSelectionIndex, array2, array2.Length, radioButton, new GUILayoutOption[]
				{
					GUILayout.Width(num3)
				});
			}
			ClipCreationOptions clipCreationOptions = (clipTypeSelectionIndex == 0) ? ClipCreationOptions.MovieClip : ClipCreationOptions.Animator;
			clipCreationOptions |= ((clipObjectsTypeSelectionIndex == 0) ? ClipCreationOptions.NotBaked : ClipCreationOptions.Baked);
			drawCreationMenu(_TimelineID, clipCreationOptions);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000095A4 File Offset: 0x000077A4
		private void drawCreationMenuCanvasInternal(int _TimelineID)
		{
			GUIStyle radioButton = EditorStyles.radioButton;
			float num = GUIStyle.none.CalcSize(new GUIContent("Movie clip")).x + 25f;
			float num2 = GUIStyle.none.CalcSize(new GUIContent("Animator")).x + 25f;
			float num3 = num + num2;
			string[] array = new string[]
			{
				"Movie clip",
				"Animator"
			};
			string[] array2 = new string[]
			{
				"Not baked",
				"Baked"
			};
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			clipTypeSelectionIndex = GUILayout.SelectionGrid(clipTypeSelectionIndex, array, array.Length, radioButton, new GUILayoutOption[]
			{
				GUILayout.Width(num3)
			});
			EditorGUILayout.EndHorizontal();
			if (!target.hasNesting)
			{
				clipObjectsTypeSelectionIndex = GUILayout.SelectionGrid(clipObjectsTypeSelectionIndex, array2, array2.Length, radioButton, new GUILayoutOption[]
				{
					GUILayout.Width(num3)
				});
			}
			ClipCreationOptions clipCreationOptions = (clipTypeSelectionIndex == 0) ? ClipCreationOptions.MovieClip : ClipCreationOptions.Animator;
			clipCreationOptions |= ((clipObjectsTypeSelectionIndex == 0) ? ClipCreationOptions.NotBaked : ClipCreationOptions.Baked);
			drawCreationMenu(_TimelineID, clipCreationOptions);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000208D File Offset: 0x0000028D
		protected virtual void drawCreationMenu(int _TimelineID, ClipCreationOptions _Option)
		{
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000096BC File Offset: 0x000078BC
		protected void drawCreateClip<TypeOfClip>(int _TimelineID, bool _IsBaked, bool _IsAnimator) where TypeOfClip : GAFBaseClip
		{
			EditorGUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			EditorGUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			List<GAFAnimationAssetInternal> targets = this.targets;
			if (GUILayout.Button("Add to scene", Array.Empty<GUILayoutOption>()))
			{
				foreach (GAFAnimationAssetInternal gafanimationAssetInternal in targets)
				{
					if (gafanimationAssetInternal.isLoaded)
					{
						addToScene<TypeOfClip>(gafanimationAssetInternal, _TimelineID);
					}
				}
			}
			GUILayout.Space(5f);
			if (GUILayout.Button("Create prefab", Array.Empty<GUILayoutOption>()))
			{
				foreach (GAFAnimationAssetInternal gafanimationAssetInternal2 in targets)
				{
					if (gafanimationAssetInternal2.isLoaded)
					{
						createPrefab<TypeOfClip>(gafanimationAssetInternal2, _TimelineID, _IsBaked, _IsAnimator);
					}
				}
			}
			GUILayout.Space(5f);
			if (GUILayout.Button("Prefab+instance", Array.Empty<GUILayoutOption>()))
			{
				foreach (GAFAnimationAssetInternal gafanimationAssetInternal3 in targets)
				{
					if (gafanimationAssetInternal3.isLoaded)
					{
						createPrefabPlusInstance<TypeOfClip>(gafanimationAssetInternal3, _TimelineID, _IsBaked, _IsAnimator);
					}
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00002988 File Offset: 0x00000B88
		public static GameObject createClip<TypeOfClip>(GAFAnimationAssetInternal _Asset, int _TimelineID) where TypeOfClip : GAFBaseClip
		{
			GameObject gameObject = new GameObject(_Asset.name);
			TypeOfClip typeOfClip = gameObject.AddComponent<TypeOfClip>();
			typeOfClip.initialize(_Asset, _TimelineID);
			typeOfClip.reload();
			return gameObject;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000029B2 File Offset: 0x00000BB2
		public static void addToScene<TypeOfClip>(GAFAnimationAssetInternal _Asset, int _TimelineID) where TypeOfClip : GAFBaseClip
		{
			createClip<TypeOfClip>(_Asset, _TimelineID);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00009818 File Offset: 0x00007A18
		public static void createPrefab<TypeOfClip>(GAFAnimationAssetInternal _Asset, int _TimelineID, bool _IsBaked, bool _IsAnimator) where TypeOfClip : GAFBaseClip
		{
			string text = AssetDatabase.GetAssetPath(_Asset);
			text = text.Substring(0, text.Length - _Asset.name.Length - ".asset".Length);
			string text2 = text + _Asset.name;
			if (_IsBaked)
			{
				text2 += "_baked";
			}
			if (_IsAnimator)
			{
				text2 += "_animator";
			}
			text2 += ".prefab";
			if (AssetDatabase.LoadAssetAtPath(text2, typeof(GameObject)) as GameObject == null)
			{
				GameObject gameObject = createClip<TypeOfClip>(_Asset, _TimelineID);
				Object @object = PrefabUtility.CreateEmptyPrefab(text2);
				@object = PrefabUtility.ReplacePrefab(gameObject, @object, ReplacePrefabOptions.ConnectToPrefab);
				DestroyImmediate(gameObject);
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000098C4 File Offset: 0x00007AC4
		public static void createPrefabPlusInstance<TypeOfClip>(GAFAnimationAssetInternal _Asset, int _TimelineID, bool _IsBaked, bool _IsAnimator) where TypeOfClip : GAFBaseClip
		{
			string text = AssetDatabase.GetAssetPath(_Asset);
			text = text.Substring(0, text.Length - _Asset.name.Length - ".asset".Length);
			string text2 = text + _Asset.name;
			if (_IsBaked)
			{
				text2 += "_baked";
			}
			if (_IsAnimator)
			{
				text2 += "_animator";
			}
			text2 += ".prefab";
			GameObject gameObject = AssetDatabase.LoadAssetAtPath(text2, typeof(GameObject)) as GameObject;
			if (gameObject == null)
			{
				GameObject gameObject2 = createClip<TypeOfClip>(_Asset, _TimelineID);
				Object @object = PrefabUtility.CreateEmptyPrefab(text2);
				@object = PrefabUtility.ReplacePrefab(gameObject2, @object, ReplacePrefabOptions.ConnectToPrefab);
				return;
			}
			PrefabUtility.InstantiatePrefab(gameObject);
		}

		// Token: 0x0400009D RID: 157
		private List<GAFTimelineData> m_Timelines;

		// Token: 0x0400009E RID: 158
		private int m_CurrentTimelineIndex;

		// Token: 0x0400009F RID: 159
		private int m_TimelinesCount;

		// Token: 0x040000A0 RID: 160
		protected bool m_MecanimResourcesFoldout;

		// Token: 0x040000A1 RID: 161
		protected bool m_AudioResourcesFoldout;

		// Token: 0x040000A2 RID: 162
		protected int m_ClipTypeSelectionIndex;

		// Token: 0x02000035 RID: 53
		[Flags]
		protected enum AssetState
		{
			// Token: 0x040000A7 RID: 167
			Single = 1,
			// Token: 0x040000A8 RID: 168
			Multiple = 2,
			// Token: 0x040000A9 RID: 169
			Loaded = 4,
			// Token: 0x040000AA RID: 170
			NotLoaded = 8,
			// Token: 0x040000AB RID: 171
			Mixed = 16,
			// Token: 0x040000AC RID: 172
			SingleLoaded = 5,
			// Token: 0x040000AD RID: 173
			MultipleLoaded = 6,
			// Token: 0x040000AE RID: 174
			SingleNotLoaded = 9,
			// Token: 0x040000AF RID: 175
			MultipleNotLoaded = 10,
			// Token: 0x040000B0 RID: 176
			SomeNotLoaded = 18
		}

		// Token: 0x02000036 RID: 54
		[Flags]
		protected enum ClipCreationOptions
		{
			// Token: 0x040000B2 RID: 178
			MovieClip = 1,
			// Token: 0x040000B3 RID: 179
			Animator = 2,
			// Token: 0x040000B4 RID: 180
			NotBaked = 4,
			// Token: 0x040000B5 RID: 181
			Baked = 8,
			// Token: 0x040000B6 RID: 182
			NotBakedMovieClip = 5,
			// Token: 0x040000B7 RID: 183
			BakedMovieClip = 9,
			// Token: 0x040000B8 RID: 184
			NotBakedAnimator = 6,
			// Token: 0x040000B9 RID: 185
			BakedAnimator = 10
		}
	}
}
