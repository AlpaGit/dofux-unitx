
// File:			GAFMovieClipInternalEditor.cs
// Version:			5.2
// Last changed:	2017/3/28 14:41
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using System.Collections.Generic;
using System.Linq;
using GAF.Managed.Editor.Core;
using GAF.Managed.GAFInternal.Assets;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Objects;
using GAF.Scripts.Core.Interfaces;
using UnityEditor;
using UnityEngine;

namespace GAF.Scripts.Core.Editor.Interfaces
{
	[CustomEditor(typeof(GAFMovieClipInternal<GAFBaseObjectsManager>))]
	[CanEditMultipleObjects]
	public class GAFMovieClipInternalEditor<TypeOfManager, TypeOfResource> : GAFBaseClipEditor<TypeOfResource>
		where TypeOfManager : GAFBaseObjectsManager
		where TypeOfResource : GAFTexturesResourceInternal
	{
		#region Members
		
		private bool m_ShowSequences 	= true;
		private bool m_ShowSettings		= true;
		private bool m_ShowPlayback		= true;
		
		#endregion // Members
		
		#region Properties
		
		new protected List<GAFMovieClipInternal<TypeOfManager>> targets
		{
			get
			{
				return base.targets.ToList().ConvertAll<GAFMovieClipInternal<TypeOfManager>>(target => (GAFMovieClipInternal<TypeOfManager>)target);
			}
		}
		
		new protected GAFMovieClipInternal<TypeOfManager> target
		{
			get
			{
				return base.target as GAFMovieClipInternal<TypeOfManager>;
			}
		}
		
		#endregion // Properties
		
		#region Interface
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			
			drawAsset();
			drawResourcesState();
			drawPlaceholder();
			drawResourceManagement();
			drawSettings();
			drawSequences();
			drawPlayback();
			drawDataButtons();
			
			serializedObject.ApplyModifiedProperties();
		}
		
		
		#endregion // Interface
		
		#region Implementation
		
		protected virtual void drawSettings()
		{
			GUILayout.Space(3f);
			m_ShowSettings = EditorGUILayout.Foldout(m_ShowSettings, "Settings");
			
			if (m_ShowSettings)
			{
				var assetProperty 	= serializedObject.FindProperty("m_GAFAsset");
				var settingProperty = serializedObject.FindProperty("m_Settings");

				EditorGUILayout.BeginVertical(EditorStyles.textField);
				{
					//GUILayout.Space(3f);
					//EditorGUI.BeginChangeCheck();

					//drawProperty(settingProperty.FindPropertyRelative("m_FlipX"), new GUIContent("Flip X: ", "​Flip current animation by x"));
					//if (EditorGUI.EndChangeCheck())
					//{
					//	serializedObject.ApplyModifiedProperties();
					//	reloadTargets();
					//}

					GUILayout.Space(3f);
					drawProperty(settingProperty.FindPropertyRelative("m_PlayAutomatically"), new GUIContent("Play automatically: ", "Should the animation be played automatically when starting the scene? (Defines whether the animation should be played automatically when starting the scene)​"));
					
					GUILayout.Space(3f);
					drawProperty(settingProperty.FindPropertyRelative("m_IgnoreTimeScale"), new GUIContent("Ignore time scale: ", "Animation will be played even if timescale == 0.​"));
					
					GUILayout.Space(3f);
					drawProperty(settingProperty.FindPropertyRelative("m_PerfectTiming"), new GUIContent("Perfect timing (possible frames skip): ", "Additional precision for proper animation duration.​"));
					
					GUILayout.Space(3f);
					drawProperty(settingProperty.FindPropertyRelative("m_PlayInBackground"), new GUIContent("Play in backgound: ", "Animation will still be playing even if the application loses focus (for example minimize)​"));

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

					var guiEnabled = GUI.enabled;
                    GUI.enabled = targets.Any(x => !x.asset) || targets.All(x => !x.asset.hasNesting);
					drawProperty(settingProperty.FindPropertyRelative("m_DecomposeFlashTransform"), new GUIContent("Decompose Flash transform: ", "​")); 
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
						//if (targets.All((clip) => clip.asset.hasNesting))
						//{
						//	reloadDecompose();
						//}
						//else
						//{
                        	reloadTargets();
						//}
                    }
					GUI.enabled = guiEnabled;
                    GUILayout.Space(3f);
					EditorGUI.BeginChangeCheck();
					drawProperty(settingProperty.FindPropertyRelative("m_UseLights"), new GUIContent("Use lights: ", "When this property is true, animation starts using normals.​"));
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
						reloadTargets();
					}

					GUILayout.Space(10f);
					EditorGUI.BeginChangeCheck();
					var hasIndividualMaterialProperty = settingProperty.FindPropertyRelative("m_HasIndividualMaterial");
					drawProperty(hasIndividualMaterialProperty, new GUIContent("Has individual material: ", "​"));
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
                        reloadTargets();
					}
					
					var previouseState = GUI.enabled;
					GUI.enabled = previouseState && !hasIndividualMaterialProperty.hasMultipleDifferentValues && hasIndividualMaterialProperty.boolValue;
					EditorGUI.BeginChangeCheck();
					var animationColorProperty = settingProperty.FindPropertyRelative("m_AnimationColor");
					drawProperty(animationColorProperty, new GUIContent("Animation color: ", "​"));
					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.ApplyModifiedProperties();
                        reloadTargets();
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
                        reloadTargets();
					}

					EditorGUI.BeginChangeCheck();
					var stencilValueProperty = settingProperty.FindPropertyRelative("m_StencilValue");
					drawProperty(stencilValueProperty, new GUIContent("Stencil value: ", "​"));
					if (EditorGUI.EndChangeCheck())
					{
						stencilValueProperty.intValue = Mathf.Clamp(stencilValueProperty.intValue, 0, 255);
						serializedObject.ApplyModifiedProperties();
                        reloadTargets();
					}
					GUI.enabled = previouseState;
					
					GUILayout.Space(10f);
					drawProperty(settingProperty.FindPropertyRelative("m_WrapMode"), new GUIContent("Wrap mode: ", "loop - the animation will loop when it finishes playing.\nonce - the animation will be stopped when it finishes playing.​"));
					
					GUILayout.Space(3f);
					EditorGUI.BeginChangeCheck();
					var fpsProperty = settingProperty.FindPropertyRelative("m_TargetFPS");
					drawProperty(fpsProperty, new GUIContent("Target FPS: ", "Target FPS of your animation.​"));
					if (EditorGUI.EndChangeCheck())
					{
						fpsProperty.intValue = Mathf.Clamp(fpsProperty.intValue, 0, 1000);
						serializedObject.ApplyModifiedProperties();
					}
					
					GUILayout.Space(10f);
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

					guiEnabled = GUI.enabled;
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

					if (!assetProperty.hasMultipleDifferentValues &&
					    assetProperty.objectReferenceValue != null)
					{
						drawScales(assetProperty);
						drawCsfs(assetProperty);
					}
					
					GUILayout.Space(10f);
					EditorGUI.BeginChangeCheck();
					drawProperty(settingProperty.FindPropertyRelative("m_PivotOffset.x"), new GUIContent("Pivot offset X: ", "Animation pivot point offset X"));
					GUILayout.Space(3f);
					drawProperty(settingProperty.FindPropertyRelative("m_PivotOffset.y"), new GUIContent("Pivot offset Y: ", "Animation pivot point offset Y"));
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
		
		private void drawSequences()
		{
			var initializedProperty 	= serializedObject.FindProperty("m_IsInitialized");
			var assetProperty 			= serializedObject.FindProperty("m_GAFAsset");
			var timelineProperty 		= serializedObject.FindProperty("m_TimelineID");
			var currentSequenceIndex 	= serializedObject.FindProperty("m_SequenceIndex");
			
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
					var sequenceNames = sequences.ConvertAll(_sequence => _sequence.name);
					var currentIndex = currentSequenceIndex.intValue;
					
					if (currentSequenceIndex.hasMultipleDifferentValues)
					{
						sequenceNames.Insert(0, "—");
						currentIndex = 0;
					}
					
					GUILayout.Space(3f);
					m_ShowSequences = EditorGUILayout.Foldout(m_ShowSequences, "Sequences");
					if (m_ShowSequences)
					{
						EditorGUILayout.BeginVertical(EditorStyles.textField);
						{
							GUILayout.Space(3f);
							EditorGUILayout.BeginHorizontal();
							{
								var style = currentSequenceIndex.isInstantiatedPrefab && currentSequenceIndex.prefabOverride ? EditorStyles.boldLabel : EditorStyles.label;
								
								EditorGUILayout.LabelField(new GUIContent("Sequence:", "You can use frame labels to define different parts of animations (in your *.fla). Than you can select corresponding sequence to play it.​"), style);
								var index = EditorGUILayout.Popup(currentIndex, sequenceNames.ToArray());
								if (index != currentIndex)
								{
									index = currentSequenceIndex.hasMultipleDifferentValues ? index - 1 : index;
									foreach (var target in targets)
									{
										target.setSequence(sequences[index].name, false);
										EditorUtility.SetDirty(target);
									}
									
									currentSequenceIndex.intValue = index;
									
									var currentFrameProperty = serializedObject.FindProperty("m_InternalFrameNumber");
									currentFrameProperty.intValue = (int)sequences[index].startFrame;
									
									serializedObject.ApplyModifiedProperties();
								}
							}
							EditorGUILayout.EndHorizontal();
						}
						EditorGUILayout.EndVertical();
					}
				}
			}
		}
        		
		protected virtual void drawPlayback()
		{
			var initializedProperty 	= serializedObject.FindProperty("m_IsInitialized");
			var assetProperty 			= serializedObject.FindProperty("m_GAFAsset");
			var timelineProperty 		= serializedObject.FindProperty("m_TimelineID");
			var currentSequenceIndex 	= serializedObject.FindProperty("m_SequenceIndex");
			
			if (!initializedProperty.hasMultipleDifferentValues &&
			    initializedProperty.boolValue &&
			    !assetProperty.hasMultipleDifferentValues &&
			    assetProperty.objectReferenceValue != null &&
			    !timelineProperty.hasMultipleDifferentValues &&
			    !currentSequenceIndex.hasMultipleDifferentValues)
			{
				var asset = (GAFAnimationAssetInternal)assetProperty.objectReferenceValue;
				if (asset != null && asset.isLoaded)
				{
					var timelineID = timelineProperty.intValue;
					var sequences = asset.getSequences(timelineID);
					var currentSequence = sequences[currentSequenceIndex.intValue];
					
					var currentFrameProperty = serializedObject.FindProperty("m_InternalFrameNumber");
					
					GUILayout.Space(3f);
					m_ShowPlayback = EditorGUILayout.Foldout(m_ShowPlayback, "Playback");
					if (m_ShowPlayback)
					{
						EditorGUILayout.BeginVertical(EditorStyles.textField);
						{
							GUILayout.Space(3f);
							EditorGUILayout.LabelField("Frames timeline:");
							
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
										target.gotoAndStop((uint)currentFrameProperty.intValue);
									}
									serializedObject.ApplyModifiedProperties();
								}
							}
							else
							{
								if (!currentFrameProperty.hasMultipleDifferentValues)
									EditorGUILayout.IntSlider(currentFrameProperty.intValue, (int)currentSequence.startFrame, (int)currentSequence.endFrame);
							}
							
							if (!currentFrameProperty.hasMultipleDifferentValues)
							{
								var isPlaying = targets.All((item) => item.isPlaying());
								
								EditorGUILayout.BeginHorizontal();
								{
									var currentEnabled = GUI.enabled;
									GUI.enabled = currentEnabled && (uint)currentFrameProperty.intValue > currentSequence.startFrame && !isPlaying;
									if (GUILayout.Button("<<", EditorStyles.miniButtonLeft))
									{
										foreach (var target in targets)
										{
											target.gotoAndStop(currentSequence.startFrame);
											EditorUtility.SetDirty(target);
										}
									}
									GUI.enabled = currentEnabled;
									
									GUI.enabled = currentEnabled && (uint)currentFrameProperty.intValue > currentSequence.startFrame && !isPlaying;
									if (GUILayout.Button("<", EditorStyles.miniButtonMid))
									{
										foreach (var target in targets)
										{
											target.gotoAndStop((uint)(currentFrameProperty.intValue - 1));
											EditorUtility.SetDirty(target);
										}
									}
									GUI.enabled = currentEnabled;

									GUI.enabled = !Application.isPlaying;
									if (!isPlaying)
									{
										
										if (GUILayout.Button("PLAY", EditorStyles.miniButtonMid))
										{
											foreach (var target in targets)
											{
												EditorApplication.update += playInEditor;
												
												var frameNumber = (uint)currentFrameProperty.intValue;
												
												if (target.getAnimationWrapMode() == GAFWrapMode.Once &&
												    frameNumber == target.currentSequence.endFrame &&
												    !target.isPlaying())
												{
													frameNumber = target.currentSequence.startFrame;
												}
												
												target.gotoAndPlay(frameNumber);
												EditorUtility.SetDirty(target);
											}
										}
									}
									else
									{
										if (GUILayout.Button("STOP", EditorStyles.miniButtonMid))
										{
											foreach (var target in targets)
											{
												target.stop();
												EditorApplication.update -= playInEditor;
												EditorUtility.SetDirty(target);
											}
										}
										
										if (GUILayout.Button("PAUSE", EditorStyles.miniButtonMid))
										{
											foreach (var target in targets)
											{
												target.pause();
												EditorUtility.SetDirty(target);
											}
										}
									}
									GUI.enabled = currentEnabled;

									GUI.enabled = currentEnabled && (uint)currentFrameProperty.intValue < currentSequence.endFrame && !isPlaying;
									if (GUILayout.Button(">", EditorStyles.miniButtonMid))
									{
										foreach (var target in targets)
										{
											target.gotoAndStop((uint)(currentFrameProperty.intValue + 1));
											EditorUtility.SetDirty(target);
										}
									}
									GUI.enabled = currentEnabled;
									
									GUI.enabled = currentEnabled && (uint)currentFrameProperty.intValue < currentSequence.endFrame && !isPlaying;
									if (GUILayout.Button(">>", EditorStyles.miniButtonRight))
									{
										foreach (var target in targets)
										{
											target.gotoAndStop(currentSequence.endFrame);
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

		private void OnDisable()
		{
			EditorApplication.update -= playInEditor;
		}

		private void OnDestroy()
		{
			EditorApplication.update -= playInEditor;
		}

		private void playInEditor()
		{
			foreach (var clip in targets)
			{
				if (clip!= null)
					clip.SendMessage("EditorUpdate");
			}
		}

		#endregion // Implementation
	}
}