
// File:			GAFBakedAnimatorInternalEditor.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using System.Collections.Generic;
using System.Linq;
using GAF.Managed.Editor.Core;
using GAF.Managed.GAFInternal.Assets;
using GAF.Managed.GAFInternal.Objects;
using GAF.Scripts.Core.Interfaces;
using UnityEditor;
using UnityEngine;

namespace GAF.Scripts.Core.Editor.Interfaces
{
	[CustomEditor(typeof(GAFAnimatorInternal<GAFBaseObjectsManager>))]
	[CanEditMultipleObjects]
	public class GAFBakedAnimatorInternalEditor<TypeOfManager, TypeOfResource> : GAFBaseClipEditor<TypeOfResource>
		where TypeOfManager : GAFBaseObjectsManager
		where TypeOfResource : GAFTexturesResourceInternal
	{
		#region Members
		
		private bool m_ShowSettings	= true;
		private bool m_ShowPlayback	= true;
		
		#endregion // Members
		
		#region Properties
		
		new public List<GAFAnimatorInternal<TypeOfManager>> targets
		{
			get
			{
				return base.targets.ToList().ConvertAll<GAFAnimatorInternal<TypeOfManager>>(target => (GAFAnimatorInternal<TypeOfManager>)target);
			}
		}
		
		new public GAFAnimatorInternal<TypeOfManager> target
		{
			get
			{
				return base.target as GAFAnimatorInternal<TypeOfManager>;
			}
		}
		
		#endregion // Properties
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			
			drawAsset();
			drawResourcesState();
			drawPlaceholder();
			drawResourceManagement();
			drawSettings();
			drawPlayback();
			drawDataButtons();
			
			serializedObject.ApplyModifiedProperties();
		}
		
		protected virtual void drawPlayback()
		{
			var initializedProperty 	= serializedObject.FindProperty("m_IsInitialized");
			var assetProperty 			= serializedObject.FindProperty("m_GAFAsset");
			var timelineProperty 		= serializedObject.FindProperty("m_TimelineID");
			
			if (!initializedProperty.hasMultipleDifferentValues &&
			    initializedProperty.boolValue &&
			    !assetProperty.hasMultipleDifferentValues &&
			    assetProperty.objectReferenceValue != null &&
			    !timelineProperty.hasMultipleDifferentValues)
			{
				var asset = (GAFAnimationAssetInternal)assetProperty.objectReferenceValue;
				if (asset != null && asset.isLoaded)
				{
					var timelineID = timelineProperty.intValue;
					var sequences = asset.getSequences(timelineID);
					var currentSequence = sequences.First(sequence => sequence.name == "Default");
					
					GUILayout.Space(3f);
					EditorGUILayout.LabelField("Sequence: " + currentSequence.name);
					
					var currentFrameProperty = serializedObject.FindProperty("m_InternalFrameNumber");
					
					GUILayout.Space(3f);
					m_ShowPlayback = EditorGUILayout.Foldout(m_ShowPlayback, "Playback");
					if (m_ShowPlayback)
					{
						EditorGUILayout.BeginVertical(EditorStyles.textField);
						{
							if (!Application.isPlaying)
							{
								EditorGUI.BeginChangeCheck();
								EditorGUI.showMixedValue = currentFrameProperty.hasMultipleDifferentValues;
								EditorGUILayout.IntSlider(currentFrameProperty, (int)currentSequence.startFrame, (int)currentSequence.endFrame, new GUIContent(""));
								EditorGUI.showMixedValue = false;
								if (EditorGUI.EndChangeCheck())
								{
									foreach (var target in targets)
									{
										target.updateToFrameAnimatorWithRefresh(currentFrameProperty.intValue);
									}
									serializedObject.ApplyModifiedProperties();
								}
							}
							else
							{
								GUILayout.Space(3f);
								EditorGUILayout.LabelField("Frames timeline:");
								
								if (!currentFrameProperty.hasMultipleDifferentValues)
									EditorGUILayout.IntSlider(currentFrameProperty.intValue, (int)currentSequence.startFrame, (int)currentSequence.endFrame);
							}
							
							if (!currentFrameProperty.hasMultipleDifferentValues)
							{
								EditorGUILayout.BeginHorizontal();
								{
									var currentEnabled = GUI.enabled;
									GUI.enabled = currentEnabled && (uint)currentFrameProperty.intValue > currentSequence.startFrame;
									if (GUILayout.Button("<<", EditorStyles.miniButtonLeft))
									{
										foreach (var target in targets)
										{
											target.updateToFrameAnimatorWithRefresh((int)currentSequence.startFrame);
											EditorUtility.SetDirty(target);
										}
									}
									GUI.enabled = currentEnabled;
									
									GUI.enabled = currentEnabled && (uint)currentFrameProperty.intValue > currentSequence.startFrame;
									if (GUILayout.Button("<", EditorStyles.miniButtonMid))
									{
										foreach (var target in targets)
										{
											target.updateToFrameAnimatorWithRefresh((int)(currentFrameProperty.intValue - 1));
											EditorUtility.SetDirty(target);
										}
									}
									
									GUI.enabled = currentEnabled && (uint)currentFrameProperty.intValue < currentSequence.endFrame;
									if (GUILayout.Button(">", EditorStyles.miniButtonMid))
									{
										foreach (var target in targets)
										{
											target.updateToFrameAnimatorWithRefresh((int)(currentFrameProperty.intValue + 1));
											EditorUtility.SetDirty(target);
										}
									}
									GUI.enabled = currentEnabled;
									
									GUI.enabled = currentEnabled && (uint)currentFrameProperty.intValue < currentSequence.endFrame;
									if (GUILayout.Button(">>", EditorStyles.miniButtonRight))
									{
										foreach (var target in targets)
										{
											target.updateToFrameAnimatorWithRefresh((int)currentSequence.endFrame);
											EditorUtility.SetDirty(target);
										}
									}
									GUI.enabled = currentEnabled;
								}
								EditorGUILayout.EndHorizontal();
							}
						}
						EditorGUILayout.EndVertical();
					}
				}
			}
		}
		
		protected virtual void drawSettings()
		{
			GUILayout.Space(3f);
			m_ShowSettings = EditorGUILayout.Foldout(m_ShowSettings, "Settings");
			
			if (m_ShowSettings)
			{
				var settingProperty = serializedObject.FindProperty("m_Settings");
				EditorGUILayout.BeginVertical(EditorStyles.textField);
				{
					GUILayout.Space(10f);

					GUILayout.Space(3f);
					EditorGUI.BeginChangeCheck();
					var useCache = settingProperty.FindPropertyRelative("m_CacheStates");
					drawProperty(useCache, new GUIContent("Cache states: ", "​"));
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();

						foreach (var _target in targets)
						{
							_target.cacheAllStates();
						}
					}

					GUILayout.Space(3f);
					EditorGUI.BeginChangeCheck();
					var hasIndividualMaterialProperty = settingProperty.FindPropertyRelative("m_HasIndividualMaterial");
					drawProperty(hasIndividualMaterialProperty, new GUIContent("Has individual material: ", "​"));
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						
						foreach (var _target in targets)
						{
							_target.reload();
						}
					}
					
					var previouseState = GUI.enabled;
					GUI.enabled = previouseState && !hasIndividualMaterialProperty.hasMultipleDifferentValues && hasIndividualMaterialProperty.boolValue;
					EditorGUI.BeginChangeCheck();
					var animationColorProperty = settingProperty.FindPropertyRelative("m_AnimationColor");
					drawProperty(animationColorProperty, new GUIContent("Animation color: ", "​"));
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						
						foreach (var _target in targets)
						{
							_target.reload();
						}
					}

					EditorGUI.BeginChangeCheck();
					var animationColorOffsetProperty = settingProperty.FindPropertyRelative("m_AnimationColorOffset");
					drawProperty(animationColorOffsetProperty, new GUIContent("Animation color offset: ", "​"));
					if (EditorGUI.EndChangeCheck())
					{
						var value = animationColorOffsetProperty.vector4Value;
						value.x = Mathf.Clamp(animationColorOffsetProperty.vector4Value.x, -1, 1);
						value.y = Mathf.Clamp(animationColorOffsetProperty.vector4Value.y, -1, 1);
						value.z = Mathf.Clamp(animationColorOffsetProperty.vector4Value.z, -1, 1);
						value.w = Mathf.Clamp(animationColorOffsetProperty.vector4Value.w, -1, 1);
						animationColorOffsetProperty.vector4Value = value;
						
						serializedObject.ApplyModifiedProperties();
						
						foreach (var _target in targets)
						{
							_target.reload();
						}
					}

					EditorGUI.BeginChangeCheck();
					var stencilValueProperty = settingProperty.FindPropertyRelative("m_StencilValue");
					drawProperty(stencilValueProperty, new GUIContent("Stencil value: ", "​"));
					if (EditorGUI.EndChangeCheck())
					{
						stencilValueProperty.intValue = Mathf.Clamp(stencilValueProperty.intValue, 0, 255);
						serializedObject.ApplyModifiedProperties();
						
						foreach (var _target in targets)
						{
							_target.reload();
						}
					}
					GUI.enabled = previouseState;
					
					GUILayout.Space(3f);
					
					drawSortingID();
					
					GUILayout.Space(3f);
					EditorGUI.BeginChangeCheck();
					var layerOrderProperty = settingProperty.FindPropertyRelative("m_SpriteLayerValue");
					drawProperty(layerOrderProperty, new GUIContent("Sorting layer order: ", "The overlay priority of this animation within its layer. Lower numbers are rendered first and subsequent numbers overlay those below.​"));
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						reloadTargets();
					}

					var guiEnabled = GUI.enabled;
					GUI.enabled = !useCache.boolValue;

					GUILayout.Space(10f);
					EditorGUI.BeginChangeCheck();
					var pixelsPerUnitProperty = settingProperty.FindPropertyRelative("m_PixelsPerUnit");
					drawProperty(pixelsPerUnitProperty, new GUIContent("Pixels per unit: ", "Ability to scale animation by changing the size of the mesh.​"));
					if (EditorGUI.EndChangeCheck())
					{
						pixelsPerUnitProperty.floatValue = Mathf.Clamp(pixelsPerUnitProperty.floatValue, 0.001f, 1000f);
						serializedObject.ApplyModifiedProperties();
						reloadTargets();
					}
					
					var assetProperty = serializedObject.FindProperty("m_GAFAsset");
					if (!assetProperty.hasMultipleDifferentValues &&
					    assetProperty.objectReferenceValue != null)
					{
						drawScales(assetProperty);
						drawCsfs(assetProperty);
					}
					
					GUILayout.Space(10f);
					EditorGUI.BeginChangeCheck();
					drawProperty(settingProperty.FindPropertyRelative("m_PivotOffset.x"), new GUIContent("Pivot offset X: ", ""));
					GUILayout.Space(3f);
					drawProperty(settingProperty.FindPropertyRelative("m_PivotOffset.y"), new GUIContent("Pivot offset Y: ", ""));
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						reloadTargets();
					}

					GUILayout.Space(10f);
					EditorGUI.BeginChangeCheck();
					drawProperty(settingProperty.FindPropertyRelative("m_ZLayerScale"), new GUIContent("Z Layer scale: ", "Multiplier for distace between subojects"));
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						reloadTargets();
					}

					GUI.enabled = guiEnabled;
				}
				EditorGUILayout.EndVertical();
			}
		}
	}
}
