using System;
using System.Collections.Generic;
using System.Linq;
using GAF.Managed.GAFInternal.Assets;
using GAF.Managed.GAFInternal.Data;
using GAF.Managed.GAFInternal.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x0200007B RID: 123
	[DisallowMultipleComponent]
	[RequireComponent(typeof(GAFTransform))]
	[Serializable]
	public abstract class GAFBaseClip : GAFBehaviour
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00010CEC File Offset: 0x0000EEEC
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x00010CF4 File Offset: 0x0000EEF4
		public GAFTransform parent
		{
			get
			{
				return m_Parent;
			}
			set
			{
				m_Parent = value;
			}
		}

		/// <summary>
		/// Reference to asset file used in this clip.
		/// </summary>
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00010CFD File Offset: 0x0000EEFD
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x00010D05 File Offset: 0x0000EF05
		public GAFAnimationAssetInternal asset
		{
			get
			{
				return m_GAFAsset;
			}
			set
			{
				m_GAFAsset = value;
			}
		}

		/// <summary>
		/// Timeline ID of this clip.
		/// </summary>
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00010D0E File Offset: 0x0000EF0E
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x00010D16 File Offset: 0x0000EF16
		public int timelineID
		{
			get
			{
				return m_TimelineID;
			}
			protected set
			{
				m_TimelineID = value;
			}
		}

		/// <summary>
		/// Returns true if this clip already initialized.
		/// </summary>
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00010D1F File Offset: 0x0000EF1F
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00010D27 File Offset: 0x0000EF27
		public bool isInitialized
		{
			get
			{
				return m_IsInitialized;
			}
			protected set
			{
				m_IsInitialized = value;
			}
		}

		/// <summary>
		/// Returns true if clip is ready to be played.
		/// </summary>
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00010D30 File Offset: 0x0000EF30
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x00010D38 File Offset: 0x0000EF38
		public bool isLoaded { get; protected set; }

		/// <summary>
		/// GAF transformation component.
		/// <para />Contains redundant data for nested clips transforms.
		/// </summary>
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x00010D44 File Offset: 0x0000EF44
		public new GAFTransform gafTransform
		{
			get
			{
				if (m_GAFTransform == null)
				{
					m_GAFTransform = GetComponent<GAFTransform>();
					if (m_GAFTransform == null)
					{
						m_GAFTransform = gameObject.AddComponent<GAFTransform>();
					}
				}
				return m_GAFTransform;
			}
		}

		/// <summary>
		/// Returns settings of current clip.
		/// </summary>
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003AA RID: 938 RVA: 0x00010D90 File Offset: 0x0000EF90
		// (set) Token: 0x060003AB RID: 939 RVA: 0x00010D98 File Offset: 0x0000EF98
		public GAFAnimationPlayerSettings settings
		{
			get
			{
				return m_Settings;
			}
			set
			{
				m_Settings = value;
			}
		}

		/// <summary>
		/// Returns current frame number.
		/// </summary>
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00010DA1 File Offset: 0x0000EFA1
		// (set) Token: 0x060003AD RID: 941 RVA: 0x00010DA9 File Offset: 0x0000EFA9
		public uint currentFrameNumber
		{
			get
			{
				return (uint)m_CurrentFrameNumber;
			}
			protected set
			{
				m_CurrentFrameNumber = (int)value;
			}
		}

		/// <summary>
		/// Returns current frame number.
		/// </summary>
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003AE RID: 942 RVA: 0x00010DB2 File Offset: 0x0000EFB2
		// (set) Token: 0x060003AF RID: 943 RVA: 0x00010DBA File Offset: 0x0000EFBA
		public uint internalFrameNumber
		{
			get
			{
				return (uint)m_InternalFrameNumber;
			}
			set
			{
				m_InternalFrameNumber = (int)value;
			}
		}

		/// <summary>
		/// Returns reference to resource file of current animation.
		/// </summary>
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00010DC3 File Offset: 0x0000EFC3
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x00010DCB File Offset: 0x0000EFCB
		public GAFTexturesResourceInternal resource { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00010DD4 File Offset: 0x0000EFD4
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x00010DDC File Offset: 0x0000EFDC
		public GAFCustomResourceDelegate customDelegate
		{
			get
			{
				return m_GetResourceDelegate;
			}
			set
			{
				m_GetResourceDelegate = value;
			}
		}

		/// <summary>
		/// Returns true if current clip uses custom method for getting resources.
		/// </summary>
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x00010DE5 File Offset: 0x0000EFE5
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x00010DED File Offset: 0x0000EFED
		public bool useCustomDelegate
		{
			get
			{
				return m_UseCustomDelegate;
			}
			set
			{
				m_UseCustomDelegate = value;
			}
		}

		/// <summary>
		/// Returns delegate that contains custom method for getting resources.
		/// </summary>
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00010DF6 File Offset: 0x0000EFF6
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x00010E03 File Offset: 0x0000F003
		public Func<string, GAFTexturesResourceInternal> customGetResourceDelegate
		{
			get
			{
				return m_GetResourceDelegate.func;
			}
			set
			{
				m_GetResourceDelegate.func = value;
			}
		}

		/// <summary>
		/// Returns true if animation will use placeholder texture
		/// <para />instead of animation resources or during resources loading.
		/// </summary>
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00010E11 File Offset: 0x0000F011
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x00010E1C File Offset: 0x0000F01C
		public bool usePlaceholder
		{
			get
			{
				return m_UsePlaceholder;
			}
			set
			{
				if (m_UsePlaceholder != value)
				{
					m_UsePlaceholder = value;
					cleanView();
					if (m_UsePlaceholder)
					{
						cachedRenderer.enabled = true;
						cachedRenderer.sharedMaterial = m_PlaceholderMaterial;
						rebuildPlaceholderMesh();
						return;
					}
					reload();
				}
			}
		}

		/// <summary>
		/// Returns placeholder size.
		/// </summary>
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00010E71 File Offset: 0x0000F071
		// (set) Token: 0x060003BB RID: 955 RVA: 0x00010E7C File Offset: 0x0000F07C
		public Vector2 placeholderSize
		{
			get
			{
				return m_PlaceholderSize;
			}
			set
			{
				if (m_PlaceholderSize != value)
				{
					m_PlaceholderSize = new Vector2(Mathf.Clamp(value.x, 0.01f, value.x), Mathf.Clamp(value.y, 0.01f, value.y));
					if (usePlaceholder)
					{
						rebuildPlaceholderMesh();
					}
				}
			}
		}

		/// <summary>
		/// Returns offset of placeholder position.
		/// </summary>
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00010EDC File Offset: 0x0000F0DC
		// (set) Token: 0x060003BD RID: 957 RVA: 0x00010EE4 File Offset: 0x0000F0E4
		public Vector2 placeholderOffset
		{
			get
			{
				return m_PlaceholderOffset;
			}
			set
			{
				if (m_PlaceholderOffset != value)
				{
					m_PlaceholderOffset = value;
					if (usePlaceholder)
					{
						rebuildPlaceholderMesh();
					}
				}
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00006E9C File Offset: 0x0000509C
		public virtual void onTransformChanged(GAFTransform.TransformType _Type)
		{
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00006E9C File Offset: 0x0000509C
		public virtual void onGeometryChanged()
		{
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00006E9C File Offset: 0x0000509C
		public virtual void onVisibilityChanged()
		{
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00006E9C File Offset: 0x0000509C
		public virtual void onColorChanged()
		{
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00006E9C File Offset: 0x0000509C
		public virtual void onMaskingChanged()
		{
		}

		/// <summary>
		/// Initialize animation clip with it's root timeline. Crashes if root timeline is not present.
		/// <para />First operation that has to be performed to work with GAF playback.
		/// <para />Creating serializable data e.g. game objects of animation children.
		/// </summary>
		/// <param name="_Asset">Animation asset file.</param>
		// Token: 0x060003C3 RID: 963 RVA: 0x00010F09 File Offset: 0x0000F109
		public virtual void initialize(GAFAnimationAssetInternal _Asset)
		{
			initialize(_Asset, (int)_Asset.getTimelines()[0].id);
		}

		/// <summary>
		/// Initialize animation clip.
		/// <para />First operation that has to be performed to work with GAF playback.
		/// <para />Creating serializable data e.g. game objects of animation children.
		/// </summary>
		/// <param name="_Asset">Animation asset file.</param>
		/// <param name="_TimelineID">ID of timeline.</param>
		// Token: 0x060003C4 RID: 964
		public abstract void initialize(GAFAnimationAssetInternal _Asset, int _TimelineID);

		/// <summary>
		/// Reload animation view.
		/// <para />Second operation that has to be performed to work with GAF playback.
		/// <para />Creating non-serializable data e.g. meshes, runtime materials.
		/// <para />Updating animation view to a currently selected frame.
		/// <para />Should be called every time when animation view has been changed e.g. setting animation color.
		/// </summary>
		// Token: 0x060003C5 RID: 965
		public abstract void reload();

		/// <summary>
		/// Getting reference to an object by ID
		/// </summary>
		/// <param name="_ID">ID of animation suboject e.g "2_27" - "27" is object ID.</param>
		// Token: 0x060003C6 RID: 966
		public abstract IGAFObject getObject(uint _ID);

		/// <summary>
		/// Get object by name. If your object has custom name in inspector you can get it here.
		/// </summary>
		/// <param name="_PartName">Name of part.</param>
		/// <returns></returns>
		// Token: 0x060003C7 RID: 967 RVA: 0x00010F23 File Offset: 0x0000F123
		public virtual IGAFObject getObject(string _PartName)
		{
			return getObject(partNameToObjectID(_PartName));
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00010F32 File Offset: 0x0000F132
		public GAFFrameData getFrame(uint _FrameNumber)
		{
			return getFrames()[_FrameNumber];
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00010F40 File Offset: 0x0000F140
		public Dictionary<uint, GAFFrameData> getFrames()
		{
			if (asset == null)
			{
				return null;
			}
			return asset.getFrames(timelineID);
		}

		/// <summary>
		/// Add event trigger on a desired frame.
		/// <para />Used for defining callbacks e.g. sounds, custom effects.
		/// <para />Returns ID of current event.
		/// </summary>
		/// <param name="_Callback">Event will been call on selected frame.</param>
		/// <param name="_FrameNumber">Frame number.</param>
		/// <returns>System.String.</returns>
		// Token: 0x060003CA RID: 970 RVA: 0x00010F64 File Offset: 0x0000F164
		public void addTrigger(Action<GAFBaseClip> _Callback, uint _FrameNumber)
		{
			if ((ulong)_FrameNumber <= (ulong)((long)getFrames().Count) && _FrameNumber > 0U)
			{
				if (m_Triggers == null)
				{
					m_Triggers = new Dictionary<uint, Action<GAFBaseClip>>();
				}
				if (m_Triggers.ContainsKey(_FrameNumber))
				{
					var triggers = m_Triggers;
					triggers[_FrameNumber] = (Action<GAFBaseClip>)Delegate.Combine(triggers[_FrameNumber], _Callback);
					return;
				}
				m_Triggers[_FrameNumber] = _Callback;
			}
		}

		/// <summary>
		/// Remove callback from desired frame by frame ID.
		/// </summary>
		/// <param name="_Callback">Callback-method.</param>
		/// <param name="_FrameNumber">Frame number.</param>
		// Token: 0x060003CB RID: 971 RVA: 0x00010FD8 File Offset: 0x0000F1D8
		public void removeTrigger(Action<GAFBaseClip> _Callback, uint _FrameNumber)
		{
			if ((ulong)_FrameNumber < (ulong)((long)getFrames().Count) && m_Triggers != null && m_Triggers.ContainsKey(_FrameNumber))
			{
				var triggers = m_Triggers;
				triggers[_FrameNumber] = (Action<GAFBaseClip>)Delegate.Remove(triggers[_FrameNumber], _Callback);
				if (m_Triggers[_FrameNumber] == null)
				{
					m_Triggers.Remove(_FrameNumber);
				}
			}
		}

		/// <summary>
		/// Remove all event triggers on selected frame.
		/// </summary>
		/// <param name="_FrameNumber">Frame number.</param>
		// Token: 0x060003CC RID: 972 RVA: 0x00011048 File Offset: 0x0000F248
		public void removeAllTriggers(uint _FrameNumber)
		{
			if ((ulong)_FrameNumber < (ulong)((long)getFrames().Count) && m_Triggers != null && m_Triggers.ContainsKey(_FrameNumber))
			{
				m_Triggers.Remove(_FrameNumber);
			}
		}

		/// <summary>
		/// Remove all event triggers in current animation.
		/// </summary>
		// Token: 0x060003CD RID: 973 RVA: 0x0001107D File Offset: 0x0000F27D
		public void removeAllTriggers()
		{
			if (m_Triggers != null && m_Triggers.Count > 0)
			{
				m_Triggers.Clear();
			}
		}

		/// <summary>
		/// Remove event trigger by its ID.
		/// </summary>
		/// <param name="_ID">ID of trigger.</param>
		// Token: 0x060003CE RID: 974 RVA: 0x000110A0 File Offset: 0x0000F2A0
		[Obsolete("This method is obsolete, use removeTrigger(uint _FrameNumber) instead.")]
		public void removeTrigger(int _ID)
		{
			getFrames().Values.First(_frame => 
			{
				var events = _frame.events;
				return events.RemoveAll(evt => evt.id == _ID) == 1;
			});
		}

		/// <summary>
		/// Subscribe to dispatchEvent's callback on desired frame if it has dispatchEvent.
		/// <remarks>
		/// <para />It is recommended to subscribe to dispatchEvent's callback in your Start() function.
		/// <para />If you have multiple dispatch events in one frame, you can override this method.
		/// </remarks>
		/// </summary>
		/// <param name="_Callback">Callback for the dispatchEvent</param>
		/// <param name="_FrameNumber">Frame number</param>
		// Token: 0x060003CF RID: 975 RVA: 0x000110D8 File Offset: 0x0000F2D8
		public void subscribeToDispatchEvent(Action<List<string>, GAFBaseClip> _Callback, uint _FrameNumber)
		{
			var gafdispatchEvent = getFrame(_FrameNumber).events.Find((GAFBaseEvent evt) => evt.type == GAFBaseEvent.ActionType.DispatchEvent) as GAFDispatchEvent;
			if (gafdispatchEvent != null)
			{
				gafdispatchEvent.subscribe(_Callback, gameObject.GetInstanceID());
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00011130 File Offset: 0x0000F330
		public void unsubscribeEventFromDispatchEvent(Action<List<string>, GAFBaseClip> _Callback, uint _FrameNumber)
		{
			var gafdispatchEvent = getFrame(_FrameNumber).events.Find((GAFBaseEvent evt) => evt.type == GAFBaseEvent.ActionType.DispatchEvent) as GAFDispatchEvent;
			if (gafdispatchEvent != null)
			{
				gafdispatchEvent.unsubscribe(_Callback, gameObject.GetInstanceID());
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00011188 File Offset: 0x0000F388
		public void clearDispatchEvent(uint _FrameNumber)
		{
			var gafdispatchEvent = getFrame(_FrameNumber).events.Find((GAFBaseEvent evt) => evt.type == GAFBaseEvent.ActionType.DispatchEvent) as GAFDispatchEvent;
			if (gafdispatchEvent != null)
			{
				gafdispatchEvent.clear(gameObject.GetInstanceID());
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x000111E0 File Offset: 0x0000F3E0
		protected void defineFramesEvents(Action _StopEvent, Action _PlayEvent, Action<string, bool> _SetSequenceEvent, Action<uint> _GoToAndStopEvent, Action<uint> _GoToAndPlayEvent)
		{
			foreach (var gafframeData in getFrames().Values)
			{
				for (var i = 0; i < gafframeData.events.Count; i++)
				{
					var gafbaseEvent = gafframeData.events[i];
					switch (gafbaseEvent.type)
					{
					case GAFBaseEvent.ActionType.Stop:
						((GAFPlaybackEvent)gafbaseEvent).subscribe(_StopEvent, gameObject.GetInstanceID());
						break;
					case GAFBaseEvent.ActionType.Play:
						((GAFPlaybackEvent)gafbaseEvent).subscribe(_PlayEvent, gameObject.GetInstanceID());
						break;
					case GAFBaseEvent.ActionType.GotoAndStop:
						if (gafbaseEvent is GAFGoToEvent)
						{
							((GAFGoToEvent)gafbaseEvent).subscribe(_GoToAndStopEvent, gameObject.GetInstanceID());
						}
						else if (gafbaseEvent is GAFSetSequenceEvent)
						{
							((GAFSetSequenceEvent)gafbaseEvent).subscribe(_SetSequenceEvent, gameObject.GetInstanceID());
						}
						break;
					case GAFBaseEvent.ActionType.GotoAndPlay:
						if (gafbaseEvent is GAFGoToEvent)
						{
							((GAFGoToEvent)gafbaseEvent).subscribe(_GoToAndPlayEvent, gameObject.GetInstanceID());
						}
						else if (gafbaseEvent is GAFSetSequenceEvent)
						{
							((GAFSetSequenceEvent)gafbaseEvent).subscribe(_SetSequenceEvent, gameObject.GetInstanceID());
						}
						break;
					case GAFBaseEvent.ActionType.DispatchEvent:
						if (!((GAFDispatchEvent)gafbaseEvent).hasCallbacks(gameObject.GetInstanceID()))
						{
							Debug.LogWarning(string.Format("Dispatch event with null callback found in frame {0} in movie clip \"{1}\". You can use method subscribeOnDispatchEvent(System.Action<List<string>> _Callback, uint _FrameNumber) for subscribing on dispatch event callback.", gafframeData.frameNumber, name));
						}
						break;
					case GAFBaseEvent.ActionType.SoundEvent:
						defineSoundEvent(gafbaseEvent, gafframeData);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Used for defining event in GAFAnimator.
		/// </summary>
		// Token: 0x060003D3 RID: 979 RVA: 0x000113A8 File Offset: 0x0000F5A8
		protected void defineFramesEvents()
		{
			foreach (var gafframeData in getFrames().Values)
			{
				for (var i = 0; i < gafframeData.events.Count; i++)
				{
					var gafbaseEvent = gafframeData.events[i];
					if (gafbaseEvent.type == GAFBaseEvent.ActionType.DispatchEvent)
					{
						if (!((GAFDispatchEvent)gafbaseEvent).hasCallbacks(gameObject.GetInstanceID()))
						{
							Debug.LogWarning(string.Format("Dispatch event with null callback found in frame {0} in movie clip \"{1}\". You can use method subscribeOnDispatchEvent(System.Action<List<string>> _Callback, uint _FrameNumber) for subscribing on dispatch event callback.", gafframeData.frameNumber, name));
						}
					}
					else if (gafbaseEvent.type == GAFBaseEvent.ActionType.SoundEvent)
					{
						defineSoundEvent(gafbaseEvent, gafframeData);
					}
				}
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00011474 File Offset: 0x0000F674
		private void defineSoundEvent(GAFBaseEvent _FrameEvent, GAFFrameData _Frame)
		{
			var soundEvent = (GAFSoundEvent)_FrameEvent;
			if (soundEvent.action == GAFSoundEvent.SoundAction.Continue || soundEvent.action == GAFSoundEvent.SoundAction.Start)
			{
				if (m_FramesAudioData == null)
				{
					m_FramesAudioData = new List<FrameAudioData>();
				}
				var frameAudioData3 = m_FramesAudioData.Find((FrameAudioData x) => (long)x.frameNumber == (long)((ulong)_Frame.frameNumber));
				if (frameAudioData3 == null)
				{
					frameAudioData3 = new FrameAudioData((int)_Frame.frameNumber);
					m_FramesAudioData.Add(frameAudioData3);
				}
				var data = frameAudioData3.audios.Find((AudioData x) => x.ID == soundEvent.id);
				if (data == null)
				{
					data = new AudioData(soundEvent.id);
				}
				if (soundEvent.action == GAFSoundEvent.SoundAction.Start)
				{
					if (frameAudioData3.audios.Count == 0)
					{
						var audioSource = gameObject.AddComponent<AudioSource>();
						var gafsound = asset.audioResources.Find((GAFAnimationAssetInternal.GAFSound x) => x.ID == soundEvent.id);
						if (gafsound.ID != 0)
						{
							audioSource.playOnAwake = false;
							audioSource.clip = gafsound.audio;
						}
						data.source = audioSource;
						frameAudioData3.audios.Add(data);
						return;
					}
				}
				else
				{
					var frameAudioData2 = m_FramesAudioData.FirstOrDefault(delegate(FrameAudioData frameAudioData)
					{
						IEnumerable<AudioData> audios = frameAudioData.audios;
						return audios.Any((x => x.ID == data.ID));
					});
					if (frameAudioData2 != null)
					{
						var audioData = frameAudioData2.audios.Find((AudioData x) => x.ID == data.ID);
						data.source = audioData.source;
						frameAudioData3.audios.Add(data);
						return;
					}
					if (frameAudioData3.audios.Count == 0)
					{
						var audioSource2 = gameObject.AddComponent<AudioSource>();
						var gafsound2 = asset.audioResources.Find((GAFAnimationAssetInternal.GAFSound x) => x.ID == soundEvent.id);
						if (gafsound2.ID != 0)
						{
							audioSource2.playOnAwake = false;
							audioSource2.clip = gafsound2.audio;
						}
						data.source = audioSource2;
						frameAudioData3.audios.Add(data);
					}
				}
			}
		}

		/// <summary>
		/// Get object name by it's ID.
		/// </summary>
		/// <param name="_ID">ID of object.</param>
		/// <returns></returns>
		// Token: 0x060003D5 RID: 981 RVA: 0x00011698 File Offset: 0x0000F898
		public virtual string objectIDToPartName(uint _ID)
		{
			if (!(asset != null) || !asset.isLoaded)
			{
				return string.Empty;
			}
			var gafnamedPartData = asset.getNamedParts(timelineID).Find((GAFNamedPartData part) => part.objectID == _ID);
			if (gafnamedPartData == null)
			{
				return string.Empty;
			}
			return gafnamedPartData.name;
		}

		/// <summary>
		/// Get object id by name.
		/// </summary>
		/// <param name="_PartName">Named part name.</param>
		/// <returns></returns>
		// Token: 0x060003D6 RID: 982 RVA: 0x00011708 File Offset: 0x0000F908
		public virtual uint partNameToObjectID(string _PartName)
		{
			if (!(asset != null) || !asset.isLoaded)
			{
				return uint.MaxValue;
			}
			var gafnamedPartData = asset.getNamedParts(timelineID).Find((GAFNamedPartData part) => part.name == _PartName);
			if (gafnamedPartData == null)
			{
				return uint.MaxValue;
			}
			return gafnamedPartData.objectID;
		}

		/// <summary>
		/// Get number of animation objects
		/// </summary>
		/// <returns></returns>
		// Token: 0x060003D7 RID: 983
		public abstract int getObjectsCount();

		/// <summary>
		/// Clear all information about objects in animation.
		/// <para />Moves animation to not initialized state.
		/// </summary>
		/// <param name="destroyChildren">Destroy game objects from scene.</param>
		// Token: 0x060003D8 RID: 984
		public abstract void clear(bool destroyChildren);

		/// <summary>
		/// Clean up animation non-serialized data.
		/// <para />Calling reload resets animation state.
		/// </summary>
		// Token: 0x060003D9 RID: 985
		public abstract void cleanView();

		/// <summary>
		/// Returns reference to a shared material
		/// </summary>
		/// <param name="_Name">Name of material (without .mat)</param>
		/// <returns></returns>
		// Token: 0x060003DA RID: 986 RVA: 0x0001176D File Offset: 0x0000F96D
		public Material getSharedMaterial(string _Name)
		{
			parent = null;
			if (parent == null)
			{
				return m_BaseMaterials[_Name];
			}
			return parent.getMaterial(_Name);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001179D File Offset: 0x0000F99D
		public Dictionary<string, Material> getSharedMaterials()
		{
			return m_BaseMaterials;
		}

		/// <summary>
		/// Registering material as one that is used in this clip
		/// </summary>
		/// <param name="_Material">Material to register</param>
		// Token: 0x060003DC RID: 988 RVA: 0x000117A5 File Offset: 0x0000F9A5
		public void registerExternalMaterial(Material _Material)
		{
			if (parent == null)
			{
				if (!m_RegisteredMaterials.Contains(_Material))
				{
					m_RegisteredMaterials.Add(_Material);
					return;
				}
			}
			else
			{
				parent.registerExternalMaterial(_Material);
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000117DC File Offset: 0x0000F9DC
		public void setColorAndOffset(Color _Color, Vector4 _Offset)
		{
			settings.animationColorMultiplier = _Color;
			settings.animationColorOffset = _Offset;
			foreach (var keyValuePair in m_BaseMaterials)
			{
				keyValuePair.Value.SetColor("_CustomColorMultiplier", _Color);
				keyValuePair.Value.SetVector("_CustomColorOffset", _Offset);
			}
			foreach (var material in m_RegisteredMaterials)
			{
				material.SetColor("_CustomColorMultiplier", _Color);
				material.SetVector("_CustomColorOffset", _Offset);
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000118B8 File Offset: 0x0000FAB8
		protected internal void initResources(GAFAnimationAssetInternal _Asset)
		{
			if (useCustomDelegate)
			{
				if (customGetResourceDelegate != null)
				{
					resource = customGetResourceDelegate(_Asset.getResourceName(settings.scale, settings.csf));
					if (resource != null && (resource.asset != _Asset || resource.scale != settings.scale || resource.csf != settings.csf))
					{
						resource = null;
					}
				}
			}
			else
			{
				resource = _Asset.getResource(settings.scale, settings.csf);
			}
			setupMaterials();
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001198C File Offset: 0x0000FB8C
		protected internal Dictionary<uint, GAFObjectStateData> getStates(uint _FrameNumber, bool _RefreshStates)
		{
			m_ResultStates.Clear();
			var allStates = asset.getAllStates(timelineID);
			if (!_RefreshStates)
			{
				_RefreshStates = (_FrameNumber < internalFrameNumber);
			}
			if (_RefreshStates)
			{
				fillStates(1U, _FrameNumber, allStates);
			}
			else
			{
				fillStates(internalFrameNumber, _FrameNumber, allStates);
			}
			return m_ResultStates;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x000119E8 File Offset: 0x0000FBE8
		protected internal void setupMaterials()
		{
			if (resource != null && resource.isReady)
			{
				parent = null;
				if (parent == null)
				{
					m_BaseMaterials.Clear();
					clearMaterials(m_RegisteredMaterials);
					using (var enumerator = resource.data.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							var gafresourceData = enumerator.Current;
							if (settings.hasIndividualMaterial)
							{
								var material = new Material(resource.getSharedMaterial(gafresourceData.name));
								material.renderQueue = 3000;
								material.SetInt("_StencilID", GAFTransform.combineStencil(gafTransform.stencilValue, settings.stencilValue));
								material.SetColor("_CustomColorMultiplier", GAFTransform.combineColor(gafTransform.colorMultiplier, settings.animationColorMultiplier));
								material.SetVector("_CustomColorOffset", GAFTransform.combineColorOffset(gafTransform.colorOffset, settings.animationColorOffset, settings.animationColorMultiplier));
								m_BaseMaterials.Add(gafresourceData.name, material);
							}
							else
							{
								m_BaseMaterials.Add(gafresourceData.name, resource.getSharedMaterial(gafresourceData.name));
							}
						}
						return;
					}
				}
				parent.setupMaterials(resource, settings.stencilValue);
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00011B9C File Offset: 0x0000FD9C
		protected void clearMaterials(List<Material> _Materials)
		{
			for (var i = 0; i < _Materials.Count; i++)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(_Materials[i]);
				}
				else
				{
					Object.DestroyImmediate(_Materials[i], true);
				}
			}
			_Materials.Clear();
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00011BE4 File Offset: 0x0000FDE4
		protected void clearMaterials(Dictionary<string, Material> _Materials)
		{
			foreach (var keyValuePair in _Materials)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(keyValuePair.Value);
				}
				else
				{
					Object.DestroyImmediate(keyValuePair.Value);
				}
			}
			_Materials.Clear();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00011C54 File Offset: 0x0000FE54
		protected void rebuildPlaceholderMesh()
		{
			cachedFilter.sharedMesh = createPlaceholderMesh();
			cachedFilter.sharedMesh.RecalculateBounds();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00011C78 File Offset: 0x0000FE78
		private Mesh createPlaceholderMesh()
		{
			var mesh = new Mesh
			{
				name = "Placeholder"
			};
			var array = new Vector3[4];
			var num = placeholderSize.y * 0.5f;
			var num2 = placeholderSize.x * 0.5f;
			array[0] = new Vector3(-num2 + m_PlaceholderOffset.x, -num + m_PlaceholderOffset.y, 0f);
			array[1] = new Vector3(-num2 + m_PlaceholderOffset.x, num + m_PlaceholderOffset.y, 0f);
			array[2] = new Vector3(num2 + m_PlaceholderOffset.x, -num + m_PlaceholderOffset.y, 0f);
			array[3] = new Vector3(num2 + m_PlaceholderOffset.x, num + m_PlaceholderOffset.y, 0f);
			var array2 = new Vector2[array.Length];
			array2[0] = new Vector2(0f, 0f);
			array2[1] = new Vector2(0f, 1f);
			array2[2] = new Vector2(1f, 0f);
			array2[3] = new Vector2(1f, 1f);
			var triangles = new int[]
			{
				0,
				1,
				2,
				3,
				2,
				1
			};
			var array3 = new Vector3[array.Length];
			for (var i = 0; i < array3.Length; i++)
			{
				array3[i] = Vector3.back;
			}
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.triangles = triangles;
			if (settings.useLights)
			{
				mesh.normals = array3;
			}
			return mesh;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00011E44 File Offset: 0x00010044
		private void fillStates(uint _StartFrame, uint _EndFrame, Dictionary<uint, List<GAFObjectStateData>> _States)
		{
			for (var num = _StartFrame; num <= _EndFrame; num += 1U)
			{
				if (_States.ContainsKey(num))
				{
					var list = _States[num];
					var count = list.Count;
					for (var i = 0; i < count; i++)
					{
						var gafobjectStateData = list[i];
						m_ResultStates[gafobjectStateData.id] = gafobjectStateData;
					}
				}
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00006E9C File Offset: 0x0000509C
		protected virtual void Start()
		{
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00011E9E File Offset: 0x0001009E
		protected void OnEnable()
		{
			if (gafTransform.gafParent == null)
			{
				reload();
			}
		}

		// Token: 0x040001E6 RID: 486
		[SerializeField]
		private GAFTransform m_Parent;

		// Token: 0x040001E7 RID: 487
		[SerializeField]
		private GAFAnimationAssetInternal m_GAFAsset;

		// Token: 0x040001E8 RID: 488
		[SerializeField]
		private bool m_IsInitialized;

		// Token: 0x040001E9 RID: 489
		[SerializeField]
		private int m_TimelineID;

		// Token: 0x040001EA RID: 490
		[SerializeField]
		private GAFAnimationPlayerSettings m_Settings = new GAFAnimationPlayerSettings();

		// Token: 0x040001EB RID: 491
		[SerializeField]
		private int m_CurrentFrameNumber;

		// Token: 0x040001EC RID: 492
		[SerializeField]
		private int m_InternalFrameNumber = 1;

		// Token: 0x040001ED RID: 493
		[SerializeField]
		protected bool m_IsActive = true;

		// Token: 0x040001EE RID: 494
		[SerializeField]
		protected List<FrameAudioData> m_FramesAudioData;

		// Token: 0x040001EF RID: 495
		[SerializeField]
		private bool m_UseCustomDelegate;

		// Token: 0x040001F0 RID: 496
		[SerializeField]
		protected GAFCustomResourceDelegate m_GetResourceDelegate = new GAFCustomResourceDelegate();

		// Token: 0x040001F1 RID: 497
		[SerializeField]
		private bool m_UsePlaceholder;

		// Token: 0x040001F2 RID: 498
		[SerializeField]
		private Material m_PlaceholderMaterial;

		// Token: 0x040001F3 RID: 499
		[SerializeField]
		private Vector2 m_PlaceholderSize = new Vector2(100f, 100f);

		// Token: 0x040001F4 RID: 500
		[SerializeField]
		private Vector2 m_PlaceholderOffset = new Vector2(0f, 0f);

		// Token: 0x040001F5 RID: 501
		[HideInInspector]
		[NonSerialized]
		private GAFTransform m_GAFTransform;

		// Token: 0x040001F6 RID: 502
		[NonSerialized]
		protected Dictionary<string, Material> m_BaseMaterials = new Dictionary<string, Material>();

		// Token: 0x040001F7 RID: 503
		[NonSerialized]
		protected Dictionary<uint, GAFObjectStateData> m_ResultStates = new Dictionary<uint, GAFObjectStateData>();

		// Token: 0x040001F8 RID: 504
		[HideInInspector]
		[NonSerialized]
		protected List<Material> m_RegisteredMaterials = new List<Material>();

		// Token: 0x040001F9 RID: 505
		[NonSerialized]
		protected Dictionary<uint, Action<GAFBaseClip>> m_Triggers;

		// Token: 0x040001FA RID: 506
		protected string m_AssetPath;

		// Token: 0x0200007C RID: 124
		[Serializable]
		protected class AudioData
		{
			// Token: 0x17000137 RID: 311
			// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00011F3E File Offset: 0x0001013E
			public int ID
			{
				get
				{
					return m_ID;
				}
			}

			// Token: 0x17000138 RID: 312
			// (get) Token: 0x060003EA RID: 1002 RVA: 0x00011F46 File Offset: 0x00010146
			// (set) Token: 0x060003EB RID: 1003 RVA: 0x00011F4E File Offset: 0x0001014E
			public AudioSource source
			{
				get
				{
					return m_Source;
				}
				set
				{
					m_Source = value;
				}
			}

			// Token: 0x060003EC RID: 1004 RVA: 0x00011F57 File Offset: 0x00010157
			public AudioData(int _ID)
			{
				m_ID = _ID;
			}

			// Token: 0x040001FD RID: 509
			[SerializeField]
			private int m_ID;

			// Token: 0x040001FE RID: 510
			[SerializeField]
			private AudioSource m_Source;
		}

		// Token: 0x0200007D RID: 125
		[Serializable]
		protected class FrameAudioData
		{
			// Token: 0x17000139 RID: 313
			// (get) Token: 0x060003ED RID: 1005 RVA: 0x00011F66 File Offset: 0x00010166
			public int frameNumber
			{
				get
				{
					return m_FrameNumber;
				}
			}

			// Token: 0x1700013A RID: 314
			// (get) Token: 0x060003EE RID: 1006 RVA: 0x00011F6E File Offset: 0x0001016E
			public List<AudioData> audios
			{
				get
				{
					if (m_Audios == null)
					{
						m_Audios = new List<AudioData>();
					}
					return m_Audios;
				}
			}

			// Token: 0x060003EF RID: 1007 RVA: 0x00011F89 File Offset: 0x00010189
			public FrameAudioData(int _ID)
			{
				m_FrameNumber = _ID;
			}

			// Token: 0x040001FF RID: 511
			[SerializeField]
			private int m_FrameNumber;

			// Token: 0x04000200 RID: 512
			[SerializeField]
			private List<AudioData> m_Audios;
		}
	}
}
