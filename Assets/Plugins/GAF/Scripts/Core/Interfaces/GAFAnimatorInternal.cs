
// File:			GAFAnimatorInternal.cs
// Version:			5.2
// Last changed:	2017/3/31 09:45
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using System;
using System.Collections.Generic;
using System.Linq;
using GAF.Managed.GAFInternal;
using GAF.Managed.GAFInternal.Assets;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using GAF.Managed.GAFInternal.Objects;
using UnityEngine;

namespace GAF.Scripts.Core.Interfaces
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(Animator))]
	[ExecuteInEditMode]
	public class GAFAnimatorInternal<ObjectsManagerType> :
						GAFBaseClip
							where ObjectsManagerType : GAFBaseObjectsManager
	{
		#region Members

		[HideInInspector]
		[SerializeField]
		private int m_AnimatorVersion = 0;
		[HideInInspector]
		[SerializeField]
		private Animator m_Animator = null;
		[HideInInspector]
		[SerializeField]
		private ObjectsManagerType m_ObjectsManager = null;

		#endregion // Members

		/// <summary>
		/// Occurs when [on_frame_changed].
		/// </summary>
		public event Action<GAFAnimatorInternal<ObjectsManagerType>, uint, uint> on_frame_changed;
		/// <summary>
		/// Occurs when [on_clear].
		/// </summary>
		public event Action<GAFAnimatorInternal<ObjectsManagerType>> on_clear;

		#region Interface

		/// <summary>
		/// Initialize animation clip.
		/// <para />First operation that has to be performed to work with GAF playback.
		/// <para />Creating serializable data e.g. game objects of animation children.
		/// </summary>
		/// <param name="_Asset">Animation asset file.</param>
		/// <param name="_TimelineID">ID of timeline.</param>
		public override void initialize(GAFAnimationAssetInternal _Asset, int _TimelineID)
		{
			if (!isInitialized)
			{
				settings.init(_Asset);

                if (_Asset.rootClip == null)
                {
                    _Asset.rootClip = this;
                }
                else
                {
                    useCustomDelegate = _Asset.rootClip.useCustomDelegate;
                    customDelegate = _Asset.rootClip.customDelegate;
                }

                isInitialized = true;

				m_AnimatorVersion = GAFSystem.AnimatorVersion;
				asset = _Asset;
				timelineID = _TimelineID;
				m_Animator = GetComponent<Animator>();
				m_ObjectsManager = GetComponent<ObjectsManagerType>();
				m_Animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
				//m_Animator.hideFlags = HideFlags.NotEditable;
				m_Animator.runtimeAnimatorController = _Asset.getAnimatorController(timelineID);

				m_ObjectsManager.initialize();
			}
		}

		/// <summary>
		/// Reload animation view.
		/// <para />Second operation that has to be performed to work with GAF playback.
		/// <para />Creating non-serializable data e.g. meshes, runtime materials.
		/// <para />Updating animation view to a currently selected frame.
		/// <para />Should be called every time when animation view has been changed e.g. setting animation color.
		/// </summary>
		public override void reload()
		{
			if (!System.Object.Equals(asset, null) &&
				isInitialized &&
				!usePlaceholder)
			{
				if (!asset.isLoaded)
					asset.load();

				if (asset.isLoaded)
				{
				    if (asset.rootClip != null)
				    {
				        useCustomDelegate = asset.rootClip.useCustomDelegate;
				        customDelegate = asset.rootClip.customDelegate;
				    }

					if (m_AnimatorVersion < GAFSystem.AnimatorVersion)
					{
						upgrade();
					}

					if (m_AnimatorVersion == GAFSystem.AnimatorVersion)
					{
						initResources(asset);

						if (resource != null &&
							resource.isReady)
						{
							isLoaded = true;

                            gafTransform.onTransformChanged -= onTransformChanged;
							gafTransform.onTransformChanged += onTransformChanged;

							if (gafTransform.gafParent != null &&
								!settings.hasIndividualMaterial)
							{
								settings.hasIndividualMaterial = true;
							}

							m_ObjectsManager.reload();

							if (!asset.areStatesCached(timelineID) && settings.cacheStates)
							{
								cacheAllStates();
							}

							updateToFrameAnimatorWithRefresh((int)internalFrameNumber);
						}
                    }

					foreach (var mat in getSharedMaterials())
					{
						var name = mat.Value.shader.name;
						mat.Value.shader = Shader.Find(name);
					}
				}
			}
		}

		#region TEST

		public void cacheAllStates()
		{
			var states = asset.getAllStates(timelineID);
			var objects = m_ObjectsManager.objectsDict;

			if (asset.enableSequenceCaching)
			{
				for (var i = 0; i < asset.keyFrames.Count; i++)
				{
					var timeline = asset.keyFrames[i];
					for (var j = 0; j < timeline.frames.Count; j++)
					{
						var frame = timeline.frames[j];
						for (var k = 0; k < frame.states.Count; k++)
						{
							var currentState = frame.states[k];

							setState(ref currentState, objects);
						}
					}
				}
			}

			foreach (var stateList in states)
			{
				for (var i = 0; i < stateList.Value.Count; i++)
				{
					var currentState = stateList.Value[i];

					setState(ref currentState, objects);
				}
			}
		}

		protected virtual void setState(ref GAFObjectStateData _State, Dictionary<uint, IGAFObject> _Objects)
		{
			
		}

		#endregion // TEST

		/// <summary>
		/// Clear all information about objects in animation.
		/// <para />Moves animation to not initialized state.
		/// </summary>
		/// <param name="destroyChildren">Destroy game objects from scene.</param>
		public override void clear(bool destroyChildren)
		{
			if (on_clear != null)
				on_clear(this);

			if (m_ObjectsManager == null)
				m_ObjectsManager = GetComponent<ObjectsManagerType>();

			if (destroyChildren)
				m_ObjectsManager.deepClear();
			else
				m_ObjectsManager.clear();

			cleanView();

			isInitialized = false;
			asset = null;
			resource = null;
			settings = new GAFAnimationPlayerSettings();

			var frames = getFrames();
			if (frames != null)
			{
				internalFrameNumber = getFrames().Keys.First();
			}
		}

		/// <summary>
		/// Clean up animation non-serialized data.
		/// <para />Calling reload resets animation state.
		/// </summary>
		public override void cleanView()
		{
			if (m_ObjectsManager != null)
			{
				m_ObjectsManager.cleanView();
			}

			clearMaterials(m_RegisteredMaterials);
		}

		/// <summary>
		/// Getting reference to an object by ID
		/// </summary>
		/// <param name="_ID">ID of animation suboject e.g "2_27" - "27" is object ID.</param>
		/// <returns>IGAFObject.</returns>
		public override IGAFObject getObject(uint _ID)
		{
			return m_ObjectsManager.objectsDict[_ID];
		}

		/// <summary>
		/// Get number of animation objects
		/// </summary>
		/// <returns>System.Int32.</returns>
		public override int getObjectsCount()
		{
			return m_ObjectsManager.objects.Count();
		}

		/// <summary>
		/// Updates to frame animator.
		/// </summary>
		/// <param name="_FrameNumber">The frame number.</param>
		public void updateToFrameAnimator(int _FrameNumber)
		{
			if (isInitialized && isLoaded && gafTransform.localVisible)
			{
				if (internalFrameNumber != _FrameNumber)
				{
					currentFrameNumber = (uint)_FrameNumber;
					var tempFrame = getFrame((uint)_FrameNumber);
					for (var i = 0; i < tempFrame.events.Count; i++)
					{
						var tempEvent = tempFrame.events[i];
						if (tempEvent.type != GAFBaseEvent.ActionType.SoundEvent)
							tempEvent.execute(this);
						else
						{
							var soundEvent = tempEvent as GAFSoundEvent;

							if (soundEvent.action == GAFSoundEvent.SoundAction.Stop)
							{
								foreach (var frameAudioData in m_FramesAudioData)
								{
									for (int frameIndex = 0, count = frameAudioData.audios.Count; frameIndex < count; frameIndex++)
									{
										var frameAudio = frameAudioData.audios[frameIndex];

										if (frameAudio.ID == soundEvent.id)
										{
											frameAudio.source.Stop();
										}
									}
								}
							}

							else
							{
								var audioFrame = m_FramesAudioData.Find(x => x.frameNumber == tempFrame.frameNumber);
								if (audioFrame != null)
								{
									var data = audioFrame.audios.Find(x => x.ID == soundEvent.id);

									if (data != null)
									{
										switch (soundEvent.action)
										{
											case GAFSoundEvent.SoundAction.Continue:
												data.source.Play();
												break;
											case GAFSoundEvent.SoundAction.Start:
												if (!data.source.isPlaying)
												{
													data.source.Play();
												}
												break;
										}
									}
								}
							}
						}
					}

					if (m_Triggers != null && m_Triggers.ContainsKey(tempFrame.frameNumber))
					{
						m_Triggers[tempFrame.frameNumber](this);
					}

					var states = getStates((uint)_FrameNumber, false);
					if (states != null)
					{
						m_ObjectsManager.updateToFrame(states, false);
					}

					if (on_frame_changed != null)
						on_frame_changed(this, internalFrameNumber, (uint)_FrameNumber);

					internalFrameNumber = (uint)_FrameNumber;
				}
			}
		}

		/// <summary>
		/// Updates to frame animator with refresh.
		/// </summary>
		/// <param name="_FrameNumber">The frame number.</param>
		public void updateToFrameAnimatorWithRefresh(int _FrameNumber)
		{
			if (isInitialized && isLoaded && gafTransform.localVisible)
			{
				currentFrameNumber = (uint)_FrameNumber;
				var tempFrame = getFrame((uint)_FrameNumber);
				for (var i = 0; i < tempFrame.events.Count; i++)
				{
					var tempEvent = tempFrame.events[i];
					if (tempEvent.type != GAFBaseEvent.ActionType.SoundEvent)
						tempEvent.execute(this);
					else if (Application.isPlaying)
					{
						var soundEvent = tempEvent as GAFSoundEvent;

						if (soundEvent.action == GAFSoundEvent.SoundAction.Stop)
						{
							foreach (var frameAudioData in m_FramesAudioData)
							{
								for (int frameIndex = 0, count = frameAudioData.audios.Count; frameIndex < count; frameIndex++)
								{
									var frameAudio = frameAudioData.audios[frameIndex];

									if (frameAudio.ID == soundEvent.id)
									{
										frameAudio.source.Stop();
									}
								}
							}
						}

						else
						{
							var audioFrame = m_FramesAudioData.Find(x => x.frameNumber == tempFrame.frameNumber);
							if (audioFrame != null)
							{
								var data = audioFrame.audios.Find(x => x.ID == soundEvent.id);

								if (data != null)
								{
									switch (soundEvent.action)
									{
										case GAFSoundEvent.SoundAction.Continue:
										case GAFSoundEvent.SoundAction.Start:
											if (!data.source.isPlaying)
											data.source.Play();
											break;
									}
								}
							}
						}
					}
				}

				if (m_Triggers != null && m_Triggers.ContainsKey(tempFrame.frameNumber))
				{
					m_Triggers[tempFrame.frameNumber](this);
				}

				if (asset.enableSequenceCaching)
				{
					var keyFrameStates = asset.getKeyFrameStates(timelineID, (uint)_FrameNumber);

					if (keyFrameStates != null)
					{
						m_ObjectsManager.updateToKeyFrame(keyFrameStates);
                    }
					else
					{
						objManagerUpdateToFrame((uint)_FrameNumber, true);
					}
				}
				else
				{
					objManagerUpdateToFrame((uint)_FrameNumber, true);
				}

				if (on_frame_changed != null)
					on_frame_changed(this, internalFrameNumber, (uint)_FrameNumber);

				internalFrameNumber = (uint)_FrameNumber;
			}
		}

		private void objManagerUpdateToFrame(uint _FrameNumber, bool _RefreshStates)
		{
			var states = getStates(_FrameNumber, _RefreshStates);
			if (states != null)
			{
				m_ObjectsManager.updateToFrame(states, _RefreshStates);
			}
		}

#endregion // Interface

#region MonoBehaviour

		protected override void Start()
		{
			defineFramesEvents();
		}

#endregion // MonoBehaviour

#region Implementation

	    public override void onTransformChanged(GAFTransform.TransformType _Type)
		{
			switch (_Type)
			{
				case GAFTransform.TransformType.Geometry:
					onGeometryChanged();
					break;

				case GAFTransform.TransformType.Visibility:
					onVisibilityChanged();
					break;

				case GAFTransform.TransformType.Color:
					onColorChanged();
					break;

				case GAFTransform.TransformType.Masking:
					onMaskingChanged();
					break;
			}
		}

	    public override void onGeometryChanged()
		{
			m_ObjectsManager.reload();
			updateToFrameAnimatorWithRefresh((int)internalFrameNumber);
		}

	    public override void onVisibilityChanged()
		{
			gafTransform.localVisible = gafTransform.gafParent.visible;

			foreach (var _child in gafTransform.gafChilds.Values)
			{
				_child.localVisible = gafTransform.localVisible && _child.realVisibility;
			}
		}

	    public override void onColorChanged()
		{
			if (!settings.hasIndividualMaterial)
			{
				settings.hasIndividualMaterial = true;
				setupMaterials();
				m_ObjectsManager.reload();
				updateToFrameAnimatorWithRefresh((int)internalFrameNumber);
			}
			else
			{
				//var multiplier = gafTransform.colorMultiplier * settings.animationColorMultiplier;
				//var accumulated = gafTransform.colorOffset;
				//accumulated.x *= settings.animationColorMultiplier.r;
				//accumulated.y *= settings.animationColorMultiplier.g;
				//accumulated.z *= settings.animationColorMultiplier.b;
				//accumulated.w *= settings.animationColorMultiplier.a;
				//var offset = settings.animationColorOffset + accumulated;

				//foreach (var _pair in m_BaseMaterials)
				//{
				//	_pair.Value.SetColor("_CustomColorMultiplier", multiplier);
				//	_pair.Value.SetVector("_CustomColorOffset", offset);
				//}

				//foreach (var _material in m_RegisteredMaterials)
				//{
				//	_material.SetColor("_CustomColorMultiplier", multiplier);
				//	_material.SetVector("_CustomColorOffset", offset);
				//}

				var multiplier	= GAFTransform.combineColor(gafTransform.colorMultiplier, settings.animationColorMultiplier);
				var offset		= GAFTransform.combineColorOffset(gafTransform.colorOffset, settings.animationColorOffset, settings.animationColorMultiplier);

				if (parent == null)
				{
					foreach (var _pair in m_BaseMaterials)
					{
						_pair.Value.SetColor("_CustomColorMultiplier", multiplier);
						_pair.Value.SetVector("_CustomColorOffset", offset);
					}

					foreach (var _material in m_RegisteredMaterials)
					{
						_material.SetColor("_CustomColorMultiplier", multiplier);
						_material.SetVector("_CustomColorOffset", offset);
					}
				}
				else
				{
					foreach (var _pair in parent.baseMaterials)
					{
						_pair.Value.SetColor("_CustomColorMultiplier", multiplier);
						_pair.Value.SetVector("_CustomColorOffset", offset);
					}

					foreach (var _material in parent.registeredMaterials)
					{
						_material.SetColor("_CustomColorMultiplier", multiplier);
						_material.SetVector("_CustomColorOffset", offset);
					}
				}
			}
		}

	    public override void onMaskingChanged()
		{
			m_ObjectsManager.reload();
			updateToFrameAnimatorWithRefresh((int)internalFrameNumber);
		}

		private void upgrade()
		{
			var _asset = asset;
			var _timelineID = timelineID;
			var _settings = settings;
			var _currentFrameNumber = internalFrameNumber;

			clear(true);

			settings = _settings;
			internalFrameNumber = _currentFrameNumber;

			initialize(_asset, _timelineID);
		}

#endregion // Implementation
	}
}
