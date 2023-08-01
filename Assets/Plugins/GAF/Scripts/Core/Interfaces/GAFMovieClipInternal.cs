
// File:			GAFMovieClipInternal.cs
// Version:			5.2
// Last changed:	2017/3/31 09:45
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


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
	[ExecuteInEditMode]
	public class GAFMovieClipInternal<ObjectsManagerType> :
		GAFBaseClip
			, IGAFMovieClip
			where ObjectsManagerType : GAFBaseObjectsManager
	{
		#region Members

		[HideInInspector][SerializeField] private int m_ClipVersion = 0;
		[HideInInspector][SerializeField] private int m_SequenceIndex = 0;
		[HideInInspector][SerializeField] private ObjectsManagerType m_ObjectsManager = null;

		//private Dictionary<uint, List<GAFFrameTrigger>> m_FrameEvents = new Dictionary<uint, List<GAFFrameTrigger>>();

		private bool m_IsPlaying = false;
		private bool m_ContiniousPlaying = false;
		private bool m_IsFirstFrame = false;

		private float	m_Stopwatch				= 0f;
		private float	m_StoredTime			= 0f;
		private float	m_PreviouseUpdateTime	= 0f;
		private uint	m_TargetFrame			= 0;

		#endregion // Members


		#region IGAFMovieClip

		/// <summary>
		/// Start playing animation.
		/// </summary>
		public void play()
		{
			m_ContiniousPlaying = true;

			setPlaying(true);
		}

		/// <summary>
		/// Pause animation.
		/// </summary>
		public void pause()
		{
			m_ContiniousPlaying = false;
			setPlaying(false);
		}

		/// <summary>
		/// Stop playing animation. Moving playback to a first frame in a current sequence.
		/// </summary>
		public void stop()
		{
			updateToFrame(currentSequence.startFrame, true);
			m_ContiniousPlaying = false;
			setPlaying(false);
		}

		/// <summary>
		/// Go to the desired frame.
		/// </summary>
		/// <param name="_FrameNumber">Number of frame.</param>
		public void gotoAndStop(uint _FrameNumber)
		{
			currentFrameNumber = _FrameNumber;

			_FrameNumber = (uint)Mathf.Clamp(
				(int)_FrameNumber
				, (int)currentSequence.startFrame
				, (int)currentSequence.endFrame);

			updateToFrame(_FrameNumber, true);

			if (on_goto != null)
				on_goto(this);

			m_ContiniousPlaying = false;
			setPlaying(false);
		}

		/// <summary>
		/// Go to the desired frame and start playing.
		/// </summary>
		/// <param name="_FrameNumber">Number of frame.</param>
		public void gotoAndPlay(uint _FrameNumber)
		{
			currentFrameNumber = int.MaxValue;

			m_TargetFrame = _FrameNumber;

			m_IsFirstFrame = true;

			_FrameNumber = (uint)Mathf.Clamp(
				(int)_FrameNumber
				, (int)currentSequence.startFrame
				, (int)currentSequence.endFrame);

			updateToFrame(_FrameNumber, true);

			if (on_goto != null)
				on_goto(this);

			m_ContiniousPlaying = true;
			setPlaying(true);
		}

		/// <summary>
		/// Get sequence name by index.
		/// </summary>
		/// <param name="_Index">Index of sequence.</param>
		/// <returns>System.String.</returns>
		public string sequenceIndexToName(uint _Index)
		{
			if (asset != null && asset.isLoaded)
			{
				var sequences = asset.getSequences(timelineID);
				return sequences[Mathf.Clamp((int)_Index, 0, sequences.Count - 1)].name;
			}
			else
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Get sequence index by name.
		/// </summary>
		/// <param name="_Name">Name of sequence.</param>
		/// <returns>System.UInt32.</returns>
		public uint sequenceNameToIndex(string _Name)
		{
			if (asset != null && asset.isLoaded)
			{
				var index = asset.getSequences(timelineID).FindIndex(__sequence => __sequence.name == _Name);
				return index < 0 ? uint.MaxValue : (uint)index;
			}
			else
			{
				return uint.MaxValue;
			}
		}

		/// <summary>
		/// Set sequence for playing.
		/// </summary>
		/// <param name="_SequenceName">Name of desired sequence.</param>
		/// <param name="_PlayImmediately">Run this sequence immediately.</param>
		public void setSequence(string _SequenceName, bool _PlayImmediately)
		{
			var sequenceIndex = sequenceNameToIndex(_SequenceName);

			if ((sequenceIndex != uint.MaxValue &&
				 currentSequenceIndex != sequenceIndex) ||
				(sequenceIndex != uint.MaxValue && !isPlaying()))
			{
				currentSequenceIndex = sequenceIndex;

				currentFrameNumber = _PlayImmediately ? int.MaxValue : currentSequence.startFrame;

				m_TargetFrame = currentSequence.startFrame;

				m_IsFirstFrame = true;

				updateToFrame(currentSequence.startFrame, true);

				if (on_sequence_change != null)
					on_sequence_change(this);

				m_ContiniousPlaying = _PlayImmediately;
				setPlaying(_PlayImmediately);
			}
		}

		/// <summary>
		/// Set playback to a default sequence.
		/// </summary>
		/// <param name="_PlayImmediately">Run this sequence immediately.</param>
		public void setDefaultSequence(bool _PlayImmediately)
		{
			setSequence("Default", _PlayImmediately);
		}

		/// <summary>
		/// Get index of current sequence.
		/// </summary>
		/// <returns>System.UInt32.</returns>
		public uint getCurrentSequenceIndex()
		{
			return currentSequenceIndex;
		}

		/// <summary>
		/// Get current frame number.
		/// </summary>
		/// <returns>System.UInt32.</returns>
		[System.Obsolete("This method is obsolete. You can use property currentFrameNumber instead.")]
		public uint getCurrentFrameNumber()
		{
			return currentFrameNumber;
		}

		/// <summary>
		/// Get count of frames in current timeline.
		/// </summary>
		/// <returns>System.UInt32.</returns>
		public uint getFramesCount()
		{
			return asset.getFramesCount(timelineID);
		}

		/// <summary>
		/// Get wrap mode.
		/// </summary>
		/// <returns>GAFWrapMode.</returns>
		public GAFWrapMode getAnimationWrapMode()
		{
			return settings.wrapMode;
		}

		/// <summary>
		/// Set wrap mode.
		/// </summary>
		/// <param name="_Mode">Type of wrap mode.</param>
		public void setAnimationWrapMode(GAFWrapMode _Mode)
		{
			settings.wrapMode = _Mode;
		}

		/// <summary>
		/// Check if animation is playing.
		/// </summary>
		/// <returns><c>true</c> if this instance is playing; otherwise, <c>false</c>.</returns>
		public bool isPlaying()
		{
			return m_IsPlaying;
		}

		/// <summary>
		/// Get duration of current sequence.
		/// </summary>
		/// <returns>System.Single.</returns>
		public float duration()
		{
			return (currentSequence.endFrame - currentSequence.startFrame) * settings.targetSPF;
		}

		#endregion // Movie clip interface

		#region Events

		/// <summary>
		/// Occurs when [on_start_play].
		/// </summary>
		public event System.Action<IGAFMovieClip> on_start_play;

		/// <summary>
		/// Occurs when [on_stop_play].
		/// </summary>
		public event System.Action<IGAFMovieClip> on_stop_play;

		/// <summary>
		/// Occurs when [on_goto].
		/// </summary>
		public event System.Action<IGAFMovieClip> on_goto;

		/// <summary>
		/// Occurs when [on_sequence_change].
		/// </summary>
		public event System.Action<IGAFMovieClip> on_sequence_change;

		/// <summary>
		/// Occurs when [on_clear].
		/// </summary>
		public event System.Action<IGAFMovieClip> on_clear;

		#endregion // Events

		#region GAFClip interface

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

				isInitialized = true;

			    if (_Asset.rootClip != null)
			    {
			        useCustomDelegate = _Asset.rootClip.useCustomDelegate;
			        customDelegate = _Asset.rootClip.customDelegate;
			    }
			    else
			    {
			        _Asset.rootClip = this;
			    }

				m_ClipVersion = GAFSystem.MovieClipVersion;
				asset = _Asset;
				timelineID = _TimelineID;
				m_ObjectsManager = GetComponent<ObjectsManagerType>();

				m_SequenceIndex = asset.getSequences(_TimelineID).Count - 1;

				m_ObjectsManager.initialize();

				cacheAllStates();

				defineFramesEvents(stop, play, setSequence, gotoAndStop, gotoAndPlay);
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
			if (!System.Object.Equals(asset, null)
				&& isInitialized
				&& !usePlaceholder
				)
			{
				if (!asset.isLoaded)
				{
					asset.load();
				}

				if (asset.isLoaded)
				{
					if (m_ClipVersion < GAFSystem.MovieClipVersion)
					{
						upgrade();
					}

                    if (asset.rootClip != null)
                    {
                        useCustomDelegate = asset.rootClip.useCustomDelegate;
                        customDelegate = asset.rootClip.customDelegate;
                    }

                    if (m_ClipVersion == GAFSystem.MovieClipVersion)
					{
						initResources(asset);

						if (resource != null &&
							resource.isReady)
						{
							isLoaded = true;

                            gafTransform.onTransformChanged -= onTransformChanged;
							gafTransform.onTransformChanged += onTransformChanged;

							//if (gafTransform.gafParent != null &&
							//	!settings.hasIndividualMaterial)
							//{
							//	settings.hasIndividualMaterial = true;
							//}

							m_ObjectsManager.reload();

							if (!asset.areStatesCached(timelineID) && settings.cacheStates)
							{
								cacheAllStates();
							}

							updateToFrame(internalFrameNumber, true);
						}
					}

					foreach (var mat in getSharedMaterials())
					{
						var name = mat.Value.shader.name;
						mat.Value.shader = Shader.Find(name);
					}
				}
			}
			else if (usePlaceholder) rebuildPlaceholderMesh(); // TODO: Placeholder method refactor.
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
		/// Clear all information about objects in animation.
		/// <para />Moves animation to not initialized state.
		/// </summary>
		/// <param name="destroyChildren">Destroy game objects from scene.</param>
		public override void clear(bool destroyChildren)
		{
			var frames = getFrames();
			if (frames != null)
			{
				internalFrameNumber = frames != null ? frames.Keys.First() : 1;
			}

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
			
			m_SequenceIndex = 0;
			m_Stopwatch = 0.0f;
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

			if (parent == null)
				clearMaterials(m_RegisteredMaterials);
			else
			{
				clearMaterials(parent.registeredMaterials);
			}
		}

		#endregion // Behavior interface

		#region Properties

		/// <summary>
		/// Returns index of current sequence.
		/// </summary>
		/// <value>The index of the current sequence.</value>
		public uint currentSequenceIndex
		{
			get
			{
				return (uint)m_SequenceIndex;
			}

			protected set
			{
				m_SequenceIndex = (int)value;
			}
		}

		/// <summary>
		/// Returns data of current sequence.
		/// </summary>
		/// <value>The current sequence.</value>
		public GAFSequenceData currentSequence
		{
			get
			{
				if (asset != null &&
					asset.isLoaded)
				{
					return asset.getSequences(timelineID)[(int)getCurrentSequenceIndex()];
				}

				return null;
			}
		}

		#endregion // Properties

		#region MonoBehaviour

		private void FixedUpdate()
        {
			if (   asset != null
                && asset.isLoaded 
                && isLoaded 
                && isPlaying() 
                && !settings.ignoreTimeScale 
			    && gafTransform.localVisible//m_IsActive
				&& !usePlaceholder
                )
            {
            	OnUpdate(Time.deltaTime);
            }
        }
        
        private void Update()
        {
            if (   asset != null 
                && asset.isLoaded 
                && isLoaded 
                && isPlaying() 
                && settings.ignoreTimeScale 
			    && gafTransform.localVisible//m_IsActive
				&& !usePlaceholder
                )
            {
                OnUpdate(Mathf.Clamp(Time.realtimeSinceStartup - m_PreviouseUpdateTime, 0f, Time.maximumDeltaTime));
                
                m_PreviouseUpdateTime = Time.realtimeSinceStartup;
            }
        }
        
        protected override void Start()
        {
            if (Application.isPlaying 
                && isLoaded 
                && !usePlaceholder
                )
            {
                defineFramesEvents(stop, play, setSequence, gotoAndStop, gotoAndPlay);
				m_IsActive = true;
				m_IsFirstFrame = true;

				if (!m_ContiniousPlaying)
				{
					m_ContiniousPlaying = settings.playAutomatically;
					setPlaying(settings.playAutomatically);
				}
				else
				{
					setPlaying(true);
				}
			}
        }
        
        private void OnDestroy()
        {
            clear(true);
        }
        
        private void OnApplicationFocus(bool _FocusStatus)
        {
            if (   isLoaded 
                && !settings.playInBackground 
                && !usePlaceholder
                )
            {
                setPlaying(_FocusStatus && m_ContiniousPlaying);
            }
        }
        
        private void OnApplicationPause(bool _PauseStatus)
        {
            if ( isLoaded 
                && !settings.playInBackground 
                && !usePlaceholder
                )
            {
                setPlaying(_PauseStatus);
            }
        }
        
        #endregion // MonoBehaviour
        
        #region Implementation

        private void EditorUpdate()
        {
			if (  asset != null 
                && asset.isLoaded 
                && isLoaded 
                && isPlaying() 
                && !usePlaceholder)
            {
                if (gafTransform.localVisible)//m_IsActive
					OnUpdate(Mathf.Clamp(Time.realtimeSinceStartup - m_PreviouseUpdateTime, 0f, Time.maximumDeltaTime));
                
                m_PreviouseUpdateTime = Time.realtimeSinceStartup;
            }
        }

        private void OnUpdate(float _TimeDelta)
        {
            m_Stopwatch += _TimeDelta;

			if (m_Stopwatch >= settings.targetSPF)
			{
				var framesCount = 1;
				if (settings.perfectTiming)
				{
					m_StoredTime += m_Stopwatch - settings.targetSPF;
					if (m_StoredTime > settings.targetSPF)
					{
						var additionalFrames = Mathf.FloorToInt(m_StoredTime / settings.targetSPF);
						m_StoredTime = m_StoredTime - (additionalFrames * settings.targetSPF);
						framesCount += additionalFrames;
					}
				}

				m_Stopwatch = 0f;

				if (internalFrameNumber > 1)
				{
					currentFrameNumber = !m_IsFirstFrame ? internalFrameNumber : internalFrameNumber - 1;
					m_IsFirstFrame = false;
				}
				else
				{
					currentFrameNumber = currentSequence.startFrame - 1;
					m_IsFirstFrame = false;
				}

				m_TargetFrame = internalFrameNumber + (uint)framesCount;

				GAFFrameData tempFrame = null;
				
				for (currentFrameNumber = currentFrameNumber + 1; currentFrameNumber <= m_TargetFrame; currentFrameNumber++)
                {
					if (currentFrameNumber > currentSequence.endFrame)
					{
						currentFrameNumber = currentSequence.endFrame;
						break;
					}

					tempFrame = getFrame(currentFrameNumber);

					for (var j = 0; j < tempFrame.events.Count; j++)
					{
						var tempEvent = tempFrame.events[j];
						if (tempEvent.type != GAFBaseEvent.ActionType.SoundEvent)
							tempEvent.execute(this);
						else
						{
							var soundEvent = tempEvent as GAFSoundEvent;

							if (soundEvent.action == GAFSoundEvent.SoundAction.Stop)
							{
								foreach (var frameAudioData in m_FramesAudioData)
								{
									for (int i = 0, count = frameAudioData.audios.Count; i < count; i++)
									{
										var frameAudio = frameAudioData.audios[i];

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
                }

				if (m_TargetFrame > currentSequence.endFrame)
				{
					switch (settings.wrapMode)
					{
						case GAFWrapMode.Once:
							updateToFrame(currentSequence.endFrame, true);
							m_ContiniousPlaying = false;

							setPlaying(false);
							return;

						case GAFWrapMode.Loop:
							updateToFrame(currentSequence.startFrame, true);
							currentFrameNumber = 0;
							m_IsFirstFrame = true;

							if (on_stop_play != null)
								on_stop_play(this);

							if (on_start_play != null)
								on_start_play(this);

							return;

						default:
							m_ContiniousPlaying = false;
							setPlaying(false);
							return;
					}
				}

				updateToFrame(m_TargetFrame, false);
			}
		}
        
        private void updateToFrame(uint _FrameNumber, bool _RefreshStates)
        {
			if (isInitialized && isLoaded)
            {
                if (internalFrameNumber != _FrameNumber || _RefreshStates)
                {
					if (asset.enableSequenceCaching && _RefreshStates)
					{
						var states = asset.getKeyFrameStates(timelineID, _FrameNumber);
						if (states != null)
						{
							m_ObjectsManager.updateToKeyFrame(states);
						}
						else
						{
							objManagerUpdateToFrame(_FrameNumber, _RefreshStates);
                        }
					}
					else
					{
						objManagerUpdateToFrame(_FrameNumber, _RefreshStates);
                    }

					internalFrameNumber = _FrameNumber;
                }
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
        
        private void setPlaying(bool _IsPlay) 
        {
			if (m_IsPlaying != _IsPlay)
            {
                m_IsPlaying = _IsPlay;
                
                if (m_IsPlaying)
                {
                    if (on_start_play != null)
                        on_start_play(this);
                    
                    m_Stopwatch = 0.0f;
                    m_PreviouseUpdateTime = 0f;
                }
                else
                {
                    if (on_stop_play != null)
                        on_stop_play(this);
                    
                    m_Stopwatch = 0.0f;
                    m_PreviouseUpdateTime = 0f;
                }
            }
        }

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
			updateToFrame(internalFrameNumber, true);
		}

	    public override void onVisibilityChanged()
		{
			gafTransform.localVisible = m_IsActive = gafTransform.gafParent.visible;

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
                updateToFrame(internalFrameNumber, true);
            }
            else
            {
                // TODO: Wait for first bugs. 

                var multiplier      = GAFTransform.combineColor(gafTransform.colorMultiplier, settings.animationColorMultiplier);
                var offset          = GAFTransform.combineColorOffset(gafTransform.colorOffset, settings.animationColorOffset, settings.animationColorMultiplier);

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
						_pair.Value.SetVector("_CustomColorOffset",offset);
					}

					foreach (var _material in parent.registeredMaterials)
					{
						_material.SetColor("_CustomColorMultiplier", multiplier);
						_material.SetVector("_CustomColorOffset", offset);
					}
				}
            }

			//foreach (var item in m_ObjectsManager.timelines)
			//{
			//	foreach (var child in item.gafChilds)
			//	{
			//		var movieClip = child.Value.GetComponent<GAFMovieClipInternal<ObjectsManagerType>>();
			//		movieClip.onColorChanged();
			//	}
			//}
		}

	    public override void onMaskingChanged()
        {
            if (!settings.hasIndividualMaterial)
            {
                settings.hasIndividualMaterial = true;
                setupMaterials();
                m_ObjectsManager.reload();
                updateToFrame(internalFrameNumber, true);
            }
            else
            {
                var combinedStencil = GAFTransform.combineStencil(gafTransform.stencilValue, settings.stencilValue);

				if (parent == null)
				{
					foreach (var _pair in m_BaseMaterials)
					{
						_pair.Value.SetInt("_StencilID", combinedStencil);
					}

					foreach (var _material in m_RegisteredMaterials)
					{
						_material.SetInt("_StencilID", combinedStencil);
					}
				}
				else
				{
					foreach (var _pair in parent.baseMaterials)
					{
						_pair.Value.SetInt("_StencilID", combinedStencil);
					}

					foreach (var _material in parent.registeredMaterials)
					{
						_material.SetInt("_StencilID", combinedStencil);
					}
				}
            }
        }
        
        private void upgrade()
        {
            var _asset              = asset;
            var _timelineID         = timelineID;
            var _settings           = settings;
            var _sequenceIndex      = getCurrentSequenceIndex();
            var _currentFrameNumber = internalFrameNumber;
            
            clear(true);
            
            settings                = _settings;
            currentSequenceIndex    = _sequenceIndex;
			internalFrameNumber		= _currentFrameNumber;
            
            initialize(_asset, _timelineID);
        }
        
        #endregion // Implementation
    }
}