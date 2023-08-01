using System;
using System.Collections.Generic;
using System.Linq;
using GAF.Managed.GAFInternal.Attributes;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using GAF.Managed.GAFInternal.Objects;
using GAF.Managed.GAFInternal.Reader;
using GAF.Managed.GAFInternal.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GAF.Managed.GAFInternal.Assets
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public abstract class GAFAnimationAssetInternal : ScriptableObject
	{
		/// <summary>
		/// Initialize asset data with .gaf file binary data content and GUID.
		/// <para />Gaf animation initial point.
		/// </summary>
		/// <param name="_GAFData">GAF asset binary data.</param>
		/// <param name="_GUID">Unique identifier of asset file.</param>
		/// <param name="_Path">Path of the asset file.</param>
		// Token: 0x06000028 RID: 40 RVA: 0x00003434 File Offset: 0x00001634
		public void initialize(byte[] _GAFData, string _GUID, string _Path)
		{
			this.m_AssetData = _GAFData;
			this.m_AssetVersion = GAFSystem.AssetVersion;
			this.m_SharedData = null;
			this.m_GUID = _GUID;
			this.m_Path = _Path;
			this.m_ResourcesDirectory = ((!string.IsNullOrEmpty(this.m_ResourcesDirectory)) ? this.m_ResourcesDirectory : GAFSystem.CachePath);
			this.m_AudioResourcesDirectory = _Path;
			this.load();
			this.m_HasNesting = this.getTimelines().Any((GAFTimelineData timeline) => timeline.objects.Any((GAFInternal.Data.GAFObjectData obj) => obj.type == GAFObjectType.Timeline));
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000034C8 File Offset: 0x000016C8
		public bool areStatesCached(int _TimelineID)
		{
			var vertices = this.getTimeline(_TimelineID).states.First((KeyValuePair<uint, List<GAFObjectStateData>> x) => x.Value.Count > 0).Value[0].vertices;
			return vertices != null && (!(vertices[0] == Vector3.zero) || !(vertices[1] == Vector3.zero) || !(vertices[2] == Vector3.zero) || !(vertices[3] == Vector3.zero));
		}

		/// <summary>
		/// Load GAF asset binary data if it is not loaded. 
		/// <para />Extract GAF-data into Unity form.
		/// </summary>
		// Token: 0x0600002A RID: 42 RVA: 0x0000356C File Offset: 0x0000176C
		public void load()
		{
			var locker = this.m_Locker;
			lock (locker)
			{
				if (!this.isLoaded && this.m_AssetData != null)
				{
					if (this.m_AssetVersion < GAFSystem.AssetVersion)
					{
						this.upgrade();
					}
					if (this.m_AssetVersion == GAFSystem.AssetVersion)
					{
						var gafreader = new GAFReader();
						try
						{
							var gafheader = new GAFHeader();
							gafreader.Load(this.m_AssetData, ref this.m_SharedData, ref gafheader);
						}
						catch (Exception ex)
						{
							GAFUtils.Error(ex.Message);
							this.m_SharedData = null;
						}
						if (this.isLoaded && !this.m_IsExternalDataCollected)
						{
							this.collectExternalData();
						}
					}
					else
					{
						GAFUtils.Log("Asset \"" + base.name + "\" was not upgraged!", string.Empty);
					}
				}
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003650 File Offset: 0x00001850
		public void defineKeyFrames()
		{
			if (this.m_EnableSequenceCaching && (this.m_KeyFrames == null || this.m_KeyFrames.Count == 0))
			{
				this.m_KeyFrames = new List<GAFAnimationAssetInternal.TimelineFrames>();
				using (var enumerator = this.m_SharedData.timelines.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var gaftimelineData = enumerator.Current;
						var timelineFrames = new GAFAnimationAssetInternal.TimelineFrames
						{
							timelineID = (int)gaftimelineData.id,
							frames = new List<GAFAnimationAssetInternal.KeyFrame>()
						};
						var dictionary = new Dictionary<uint, GAFObjectStateData>();
						var states = gaftimelineData.states;
						var list = (from x in gaftimelineData.sequences
									where x.name != "Default"
									select x into y
									select y).ToList<GAFSequenceData>();
						var num = 1U;
						for (var i = 0; i < list.Count; i++)
						{
							var gafsequenceData = list[i];
							for (var num2 = num; num2 <= gafsequenceData.startFrame; num2 += 1U)
							{
								var list2 = states[num2];
								for (var j = 0; j < list2.Count; j++)
								{
									dictionary[list2[j].id] = list2[j];
								}
							}
							var states2 = dictionary.Values.ToList<GAFObjectStateData>();
							var item = new GAFAnimationAssetInternal.KeyFrame
							{
								ID = (int)list[i].startFrame,
								states = states2
							};
							num = gafsequenceData.startFrame + 1U;
							timelineFrames.frames.Add(item);
						}
						this.m_KeyFrames.Add(timelineFrames);
					}
					return;
				}
			}
			this.m_KeyFrames = null;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003850 File Offset: 0x00001A50
		public List<GAFObjectStateData> getKeyFrameStates(int _TimelineID, uint _FrameNumber)
		{
			return this.keyFrames.Find((GAFAnimationAssetInternal.TimelineFrames x) => x.timelineID == _TimelineID).frames.FirstOrDefault((GAFAnimationAssetInternal.KeyFrame x) => (long)x.ID == (long)((ulong)_FrameNumber)).states;
		}

		/// <summary>
		/// Find resource by scale and content scale factor values.
		/// </summary>
		/// <param name="_Scale">Scale of desired resource.</param>
		/// <param name="_CSF">Content scale factor of desired resource.</param>
		/// <returns></returns>
		// Token: 0x0600002D RID: 45 RVA: 0x000038A4 File Offset: 0x00001AA4
		public GAFTexturesResourceInternal getResource(float _Scale, float _CSF)
		{
			var key = new KeyValuePair<float, float>(_Scale, _CSF);
			GAFTexturesResourceInternal gaftexturesResourceInternal;
			if (this.m_LoadedResources.ContainsKey(key) && this.m_LoadedResources[key] != null)
			{
				gaftexturesResourceInternal = this.m_LoadedResources[key];
			}
			else
			{
				gaftexturesResourceInternal = Resources.Load<GAFTexturesResourceInternal>("Cache/" + this.getResourceName(_Scale, _CSF));
				if (gaftexturesResourceInternal != null)
				{
					this.m_LoadedResources[key] = gaftexturesResourceInternal;
				}
			}
			return gaftexturesResourceInternal;
		}

		/// <summary>
		/// Get runtime animator controller associated with current animation.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x0600002E RID: 46 RVA: 0x0000391C File Offset: 0x00001B1C
		public RuntimeAnimatorController getAnimatorController(int _TimelineID)
		{
			RuntimeAnimatorController result = null;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				var timeline = this.m_SharedData.timelines[_TimelineID];
				result = this.m_AnimatorControllers.Find((RuntimeAnimatorController _controller) => _controller.name == "[" + this.name + "]_Timeline_" + timeline.linkageName);
			}
			return result;
		}

		/// <summary>
		/// Generate resource name by scale and content scale factor values.
		/// </summary>
		/// <param name="_Scale">Scale of desired resource.</param>
		/// <param name="_CSF">Content scale factor of desired resource.</param>
		/// <returns></returns>
		// Token: 0x0600002F RID: 47 RVA: 0x00003984 File Offset: 0x00001B84
		public string getResourceName(float _Scale, float _CSF)
		{
			return string.Concat(new string[]
			{
				base.name,
				this.m_GUID,
				"_",
				_Scale.ToString(),
				"_",
				_CSF.ToString()
			});
		}

		/// <summary>
		/// Get all timelines data from current animation.
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000030 RID: 48 RVA: 0x000039E2 File Offset: 0x00001BE2
		public List<GAFTimelineData> getTimelines()
		{
			if (!this.isLoaded)
			{
				return null;
			}
			return this.m_SharedData.timelines.Values.ToList<GAFTimelineData>();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003A03 File Offset: 0x00001C03
		public GAFTimelineData getTimeline(int _TimelineID)
		{
			if (!this.isLoaded)
			{
				return null;
			}
			return this.m_SharedData.timelines[_TimelineID];
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003A20 File Offset: 0x00001C20
		public GAFAtlasElementData getCustomRegion(uint _ID, float _Scale)
		{
			if (this.isLoaded && this.m_SharedData.customRegions != null && this.m_SharedData.customRegions.Count > 0)
			{
				return this.m_SharedData.customRegions[_Scale].Find((GAFAtlasElementData x) => x.id == _ID);
			}
			return null;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003A88 File Offset: 0x00001C88
		public GAFAtlasElementData getCustomRegion(string _Name, float _Scale)
		{
			if (this.isLoaded && this.m_SharedData.customRegions != null && this.m_SharedData.customRegions.Count > 0)
			{
				return this.m_SharedData.customRegions[_Scale].Find((GAFAtlasElementData x) => x.linkageName == _Name);
			}
			return null;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003AEE File Offset: 0x00001CEE
		public List<GAFAtlasElementData> getCustomRegions(float _Scale)
		{
			if (this.isLoaded && this.m_SharedData.customRegions != null && this.m_SharedData.customRegions.Count > 0)
			{
				return this.m_SharedData.customRegions[_Scale];
			}
			return null;
		}

		/// <summary>
		/// Get all atlas textures data used in timeline.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x06000035 RID: 53 RVA: 0x00003B2C File Offset: 0x00001D2C
		public List<GAFAtlasData> getAtlases(int _TimelineID)
		{
			List<GAFAtlasData> result = null;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				result = this.m_SharedData.timelines[_TimelineID].atlases;
			}
			return result;
		}

		/// <summary>
		/// Get all animation objects data used in timeline.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x06000036 RID: 54 RVA: 0x00003B70 File Offset: 0x00001D70
		public List<GAFInternal.Data.GAFObjectData> getObjects(int _TimelineID)
		{
			List<GAFInternal.Data.GAFObjectData> result = null;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				result = this.m_SharedData.timelines[_TimelineID].objects;
			}
			return result;
		}

		/// <summary>
		/// Get all animation masks data used in timeline.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x06000037 RID: 55 RVA: 0x00003BB4 File Offset: 0x00001DB4
		public List<GAFInternal.Data.GAFObjectData> getMasks(int _TimelineID)
		{
			List<GAFInternal.Data.GAFObjectData> result = null;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				result = this.m_SharedData.timelines[_TimelineID].masks;
			}
			return result;
		}

		/// <summary>
		/// Get all animation frames data used in timeline.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x06000038 RID: 56 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public Dictionary<uint, GAFFrameData> getFrames(int _TimelineID)
		{
			Dictionary<uint, GAFFrameData> result = null;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				result = this.m_SharedData.timelines[_TimelineID].frames;
			}
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003C3C File Offset: 0x00001E3C
		public Dictionary<uint, List<GAFObjectStateData>> getAllStates(int _TimelineID)
		{
			Dictionary<uint, List<GAFObjectStateData>> result = null;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				result = this.m_SharedData.timelines[_TimelineID].states;
			}
			return result;
		}

		/// <summary>
		/// Get all sequences data from timeline.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x0600003A RID: 58 RVA: 0x00003C80 File Offset: 0x00001E80
		public List<GAFSequenceData> getSequences(int _TimelineID)
		{
			List<GAFSequenceData> result = null;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				result = this.m_SharedData.timelines[_TimelineID].sequences;
			}
			return result;
		}

		/// <summary>
		/// Get all sequences IDs for this timeline ID.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x0600003B RID: 59 RVA: 0x00003CC4 File Offset: 0x00001EC4
		public List<string> getSequenceIDs(int _TimelineID)
		{
			List<string> result = null;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				result = (from sequence in this.m_SharedData.timelines[_TimelineID].sequences
				select sequence.name).ToList<string>();
			}
			return result;
		}

		/// <summary>
		/// Get data, that contains correspondence between objects IDs and names.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x0600003C RID: 60 RVA: 0x00003D30 File Offset: 0x00001F30
		public List<GAFNamedPartData> getNamedParts(int _TimelineID)
		{
			List<GAFNamedPartData> result = null;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				result = this.m_SharedData.timelines[_TimelineID].namedParts;
			}
			else if(this.isLoaded && m_SharedData.secondaryTimelines.ContainsKey(_TimelineID))
			{
				result = m_SharedData.secondaryTimelines[_TimelineID].namedParts;
			}
			return result;
		}	
		
		public string getNamedPart(int _objectID)
		{
			string result = string.Empty;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_objectID))
			{
				result = this.m_SharedData.timelines[_objectID].linkageName;
			}
			else if(this.isLoaded && m_SharedData.secondaryTimelines.ContainsKey(_objectID))
			{
				result = m_SharedData.secondaryTimelines[_objectID].linkageName;
			}
			return result;
		}

		/// <summary>
		/// Get count of frames of desired timeline.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x0600003D RID: 61 RVA: 0x00003D74 File Offset: 0x00001F74
		public uint getFramesCount(int _TimelineID)
		{
			var result = 0U;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				result = this.m_SharedData.timelines[_TimelineID].framesCount;
			}
			return result;
		}

		/// <summary>
		/// Get general viewport size of desired timeline.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x0600003E RID: 62 RVA: 0x00003DB8 File Offset: 0x00001FB8
		public Rect getFrameSize(int _TimelineID)
		{
			var frameSize = GAFAnimationAssetInternal.badRect;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				frameSize = this.m_SharedData.timelines[_TimelineID].frameSize;
			}
			return frameSize;
		}

		/// <summary>
		/// Get pivot of desired timeline.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x0600003F RID: 63 RVA: 0x00003E00 File Offset: 0x00002000
		public Vector2 getPivot(int _TimelineID)
		{
			var result = Vector2.zero;
			if (this.isLoaded && this.m_SharedData.timelines.ContainsKey(_TimelineID))
			{
				result = this.m_SharedData.timelines[_TimelineID].pivot;
			}
			return result;
		}

		/// <summary>
		/// Get data, that contains external information about desired timeline.
		/// </summary>
		/// <param name="_TimelineID">ID of animation timeline.</param>
		/// <returns></returns>
		// Token: 0x06000040 RID: 64 RVA: 0x00003E48 File Offset: 0x00002048
		public GAFAssetExternalData getExternalData(int _TimelineID)
		{
			GAFAssetExternalData result = null;
			if (this.m_IsExternalDataCollected)
			{
				result = this.m_ExternalData.Find((GAFAssetExternalData data) => data.timelineID == _TimelineID);
			}
			return result;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003E88 File Offset: 0x00002088
		public void dropLoadedTexturesReferences()
		{
			foreach (var keyValuePair in this.m_LoadedResources)
			{
				keyValuePair.Value.dropData();
				Resources.UnloadAsset(keyValuePair.Value);
			}
			if (this.m_LoadedResources.Count > 0)
			{
				this.m_LoadedResources.Clear();
			}
			GC.Collect();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00003F0C File Offset: 0x0000210C
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00003F14 File Offset: 0x00002114
		public bool enableSequenceCaching
		{
			get
			{
				return this.m_EnableSequenceCaching;
			}
			set
			{
				this.m_EnableSequenceCaching = value;
				this.defineKeyFrames();
			}
		}

		/// <summary>
		/// Returns true if animation asset data was loaded.
		/// </summary>
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003F23 File Offset: 0x00002123
		public bool isLoaded
		{
			get
			{
				return this.m_SharedData != null;
			}
		}

		/// <summary>
		/// Returns Unity version of current animation asset.
		/// </summary>
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00003F2E File Offset: 0x0000212E
		public int assetVersion
		{
			get
			{
				return this.m_AssetVersion;
			}
		}

		/// <summary>
		/// Returns FPS of animation.
		/// </summary>
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003F36 File Offset: 0x00002136
		public ushort fps
		{
			get
			{
				if (!this.isLoaded)
				{
					return 30;
				}
				return this.m_SharedData.fps;
			}
		}

		/// <summary>
		/// Returns GAF major version of GAF format.
		/// </summary>
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003F4E File Offset: 0x0000214E
		public ushort majorDataVersion
		{
			get
			{
				if (!this.isLoaded)
				{
					return 0;
				}
				return this.m_SharedData.majorVersion;
			}
		}

		/// <summary>
		/// Returns GAF minor version of GAF format.
		/// </summary>
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003F65 File Offset: 0x00002165
		public ushort minorDataVersion
		{
			get
			{
				if (!this.isLoaded)
				{
					return 0;
				}
				return this.m_SharedData.minorVersion;
			}
		}

		/// <summary>
		/// Returns list with paths to animation resources assets.
		/// </summary>
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00003F7C File Offset: 0x0000217C
		public List<string> resourcesPaths
		{
			get
			{
				var list = new List<string>();
				foreach (var scale in this.scales)
				{
					foreach (var csf in this.csfs)
					{
						var resourcesDirectory = this.resourcesDirectory;
						var str = this.getResourceName(scale, csf) + ".asset";
						list.Add(resourcesDirectory + str);
					}
				}
				return list;
			}
		}

		/// <summary>
		/// Returns list with references to mecanim controllers.
		/// </summary>
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00004038 File Offset: 0x00002238
		public List<RuntimeAnimatorController> animatorControllers
		{
			get
			{
				return this.m_AnimatorControllers;
			}
		}

		/// <summary>
		/// Returns list with references to audio clips.
		/// </summary>
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00004040 File Offset: 0x00002240
		public List<GAFAnimationAssetInternal.GAFSound> audioResources
		{
			get
			{
				return this.m_AudioResources;
			}
		}

		/// <summary>
		/// Returns list of GAF convertion scales that can be used in animation clips.
		/// </summary>
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00004048 File Offset: 0x00002248
		public List<float> scales
		{
			get
			{
				if (!this.isLoaded)
				{
					return null;
				}
				return this.m_SharedData.scales;
			}
		}

		/// <summary>
		/// Returns list of GAF content scale factors that can be used in animation clips 
		/// </summary>
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000405F File Offset: 0x0000225F
		public List<float> csfs
		{
			get
			{
				if (!this.isLoaded)
				{
					return null;
				}
				return this.m_SharedData.csfs;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00004076 File Offset: 0x00002276
		// (set) Token: 0x0600004F RID: 79 RVA: 0x0000407E File Offset: 0x0000227E
		public bool hasNesting
		{
			get
			{
				return this.m_HasNesting;
			}
			private set
			{
				this.m_HasNesting = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00004087 File Offset: 0x00002287
		public List<GAFAnimationAssetInternal.TimelineFrames> keyFrames
		{
			get
			{
				return this.m_KeyFrames;
			}
		}

		/// <summary>
		/// Returns path to custom directory for audio resources storing.
		/// </summary>
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000051 RID: 81 RVA: 0x0000408F File Offset: 0x0000228F
		public string audioResourcesDirectory
		{
			get
			{
				return this.m_AudioResourcesDirectory;
			}
		}

		/// <summary>
		/// Returns path to custom directory for animation resources storing.
		/// </summary>
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00004097 File Offset: 0x00002297
		// (set) Token: 0x06000053 RID: 83 RVA: 0x0000409F File Offset: 0x0000229F
		public string resourcesDirectory
		{
			get
			{
				return this.m_ResourcesDirectory;
			}
			set
			{
				this.m_ResourcesDirectory = value;
			}
		}

		/// <summary>
		/// Returns path to custom directory for mecanim resources storing.
		/// </summary>
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000040A8 File Offset: 0x000022A8
		public string mecanimResourcesDirectory
		{
			get
			{
				return this.m_MecanimResourcesDirectory;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000040B0 File Offset: 0x000022B0
		public GAFAnimationData sharedData
		{
			get
			{
				if (this.isLoaded)
				{
					return this.m_SharedData;
				}
				return null;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000040C2 File Offset: 0x000022C2
		private void OnEnable()
		{
			this.load();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000040CA File Offset: 0x000022CA
		private void upgrade()
		{
			this.m_AssetVersion = GAFSystem.AssetVersion;
			this.m_IsExternalDataCollected = false;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000040E0 File Offset: 0x000022E0
		private void collectExternalData()
		{
			if (this.isLoaded && !this.m_IsExternalDataCollected)
			{
				this.m_IsExternalDataCollected = true;
				this.m_ExternalData.Clear();
				this.defineKeyFrames();
				foreach (var gaftimelineData in this.m_SharedData.timelines.Values)
				{
					var list = new List<ObjectBehaviourType>();
					using (var enumerator2 = gaftimelineData.objects.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							var obj = enumerator2.Current;
							var objectBehaviourType = ObjectBehaviourType.Simple;
							foreach (var gafobjectStateData in gaftimelineData.states.Select(keyValuePair => keyValuePair.Value)
																			  .Select(value => value.FirstOrDefault(((GAFObjectStateData x) => x.id == obj.id)))
																			  .Where(gafobjectStateData => gafobjectStateData != null))
							{
								if (gafobjectStateData.maskID > 0)
								{
									objectBehaviourType |= ObjectBehaviourType.Masked;
								}
								if (gafobjectStateData.filterData != null)
								{
									objectBehaviourType |= ObjectBehaviourType.Filtered;
								}
							}
							list.Add(objectBehaviourType);
						}
					}
					this.m_ExternalData.Add(new GAFAssetExternalData((int)gaftimelineData.id, list));
				}
			}
		}

		// Token: 0x0400001F RID: 31
		public GAFBaseClip rootClip;

		// Token: 0x04000020 RID: 32
		[HideInInspector]
		[SerializeField]
		private bool m_EnableSequenceCaching;

		// Token: 0x04000021 RID: 33
		[GAFReadOnly(true)]
		[Space(5f)]
		[SerializeField]
		private int m_AssetVersion;

		// Token: 0x04000022 RID: 34
		[Space(5f)]
		[SerializeField]
		[GAFReadOnly(false)]
		[GAFFolder(".", "Resources location", "Save to: Assets/", "Select destination folder", "GAFEditorInternal.Utils.GAFResourcesRelocator", false)]
		private string m_ResourcesDirectory = string.Empty;

		// Token: 0x04000023 RID: 35
		[GAFFolder(".", "Mecanim resources location", "Save to: Assets/", "Select destination folder", "GAFEditorInternal.Utils.GAFMecanimResourcesRelocatorIntenal", false)]
		[SerializeField]
		[Space(5f)]
		private string m_MecanimResourcesDirectory = string.Empty;

		// Token: 0x04000024 RID: 36
		[Space(5f)]
		[GAFFolder(".", "Audio resources location", "Save to: Assets/", "Select destination folder", "GAFEditorInternal.Utils.GAFAudioResourcesRelocator", true)]
		[SerializeField]
		private string m_AudioResourcesDirectory = string.Empty;

		// Token: 0x04000025 RID: 37
		[HideInInspector]
		[SerializeField]
		private byte[] m_AssetData;

		// Token: 0x04000026 RID: 38
		[SerializeField]
		[HideInInspector]
		private string m_GUID = string.Empty;

		// Token: 0x04000027 RID: 39
		[HideInInspector]
		[SerializeField]
		private string m_Path = string.Empty;

		// Token: 0x04000028 RID: 40
		[HideInInspector]
		[SerializeField]
		private bool m_IsExternalDataCollected;

		// Token: 0x04000029 RID: 41
		[SerializeField]
		[HideInInspector]
		private List<GAFAssetExternalData> m_ExternalData = new List<GAFAssetExternalData>();

		// Token: 0x0400002A RID: 42
		[HideInInspector]
		[SerializeField]
		private bool m_HasNesting;

		// Token: 0x0400002B RID: 43
		[SerializeField]
		[HideInInspector]
		private List<GAFAnimationAssetInternal.TimelineFrames> m_KeyFrames;

		// Token: 0x0400002C RID: 44
		[SerializeField]
		[HideInInspector]
		private List<GAFAnimationAssetInternal.GAFSound> m_AudioResources = new List<GAFAnimationAssetInternal.GAFSound>();

		// Token: 0x0400002D RID: 45
		[SerializeField]
		[HideInInspector]
		private List<RuntimeAnimatorController> m_AnimatorControllers = new List<RuntimeAnimatorController>();

		// Token: 0x0400002E RID: 46
		private GAFAnimationData m_SharedData;

		// Token: 0x0400002F RID: 47
		private Dictionary<KeyValuePair<float, float>, GAFTexturesResourceInternal> m_LoadedResources = new Dictionary<KeyValuePair<float, float>, GAFTexturesResourceInternal>();

		// Token: 0x04000030 RID: 48
		private static readonly Rect badRect = new Rect(0f, 0f, 0f, 0f);

		// Token: 0x04000031 RID: 49
		private Object m_Locker = new Object();

		// Token: 0x0200000B RID: 11
		[Serializable]
		public struct GAFSound
		{
			// Token: 0x04000032 RID: 50
			[SerializeField]
			public int ID;

			// Token: 0x04000033 RID: 51
			[SerializeField]
			public AudioClip audio;
		}

		// Token: 0x0200000C RID: 12
		[Serializable]
		public struct TimelineFrames
		{
			// Token: 0x04000034 RID: 52
			[SerializeField]
			public int timelineID;

			// Token: 0x04000035 RID: 53
			[SerializeField]
			public List<GAFAnimationAssetInternal.KeyFrame> frames;
		}

		// Token: 0x0200000D RID: 13
		[Serializable]
		public struct KeyFrame
		{
			// Token: 0x04000036 RID: 54
			[SerializeField]
			public int ID;

			// Token: 0x04000037 RID: 55
			[SerializeField]
			public List<GAFObjectStateData> states;
		}
	}
}
