using System.Collections.Generic;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x02000075 RID: 117
	public class GAFTimelineData
	{
		// Token: 0x06000354 RID: 852 RVA: 0x000107B0 File Offset: 0x0000E9B0
		public GAFTimelineData(uint _ID, string _LinkageName, uint _FramesCount, Rect _FrameSize, Vector2 _Pivot)
		{
			this.m_ID = _ID;
			this.m_LinkageName = (string.IsNullOrEmpty(_LinkageName) ? _ID.ToString() : _LinkageName);
			this.m_FramesCount = _FramesCount;
			this.m_FrameSize = _FrameSize;
			this.m_Pivot = _Pivot;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0001085C File Offset: 0x0000EA5C
		public uint id
		{
			get
			{
				return this.m_ID;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00010864 File Offset: 0x0000EA64
		public string linkageName
		{
			get
			{
				return this.m_LinkageName;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0001086C File Offset: 0x0000EA6C
		public List<GAFAtlasData> atlases
		{
			get
			{
				return this.m_Atlases;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00010874 File Offset: 0x0000EA74
		public List<GAFObjectData> objects
		{
			get
			{
				return this.m_Objects;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0001087C File Offset: 0x0000EA7C
		public List<GAFObjectData> masks
		{
			get
			{
				return this.m_Masks;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00010884 File Offset: 0x0000EA84
		public Dictionary<uint, GAFFrameData> frames
		{
			get
			{
				return this.m_Frames;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0001088C File Offset: 0x0000EA8C
		public Dictionary<uint, List<GAFObjectStateData>> states
		{
			get
			{
				return this.m_States;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00010894 File Offset: 0x0000EA94
		public List<GAFSequenceData> sequences
		{
			get
			{
				return this.m_Sequences;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0001089C File Offset: 0x0000EA9C
		public List<GAFNamedPartData> namedParts
		{
			get
			{
				return this.m_NamedParts;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600035E RID: 862 RVA: 0x000108A4 File Offset: 0x0000EAA4
		public uint framesCount
		{
			get
			{
				return this.m_FramesCount;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600035F RID: 863 RVA: 0x000108AC File Offset: 0x0000EAAC
		public Rect frameSize
		{
			get
			{
				return this.m_FrameSize;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000360 RID: 864 RVA: 0x000108B4 File Offset: 0x0000EAB4
		public Vector2 pivot
		{
			get
			{
				return this.m_Pivot;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000361 RID: 865 RVA: 0x000108BC File Offset: 0x0000EABC
		// (set) Token: 0x06000362 RID: 866 RVA: 0x000108C4 File Offset: 0x0000EAC4
		public GAFTimelineData parent
		{
			get
			{
				return this.m_Parent;
			}
			internal set
			{
				this.m_Parent = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000363 RID: 867 RVA: 0x000108CD File Offset: 0x0000EACD
		// (set) Token: 0x06000364 RID: 868 RVA: 0x000108D5 File Offset: 0x0000EAD5
		public List<GAFTimelineData> childs
		{
			get
			{
				return this.m_Childs;
			}
			internal set
			{
				this.m_Childs = value;
			}
		}

		// Token: 0x040001B6 RID: 438
		private uint m_ID;

		// Token: 0x040001B7 RID: 439
		private uint m_FramesCount;

		// Token: 0x040001B8 RID: 440
		private string m_LinkageName = string.Empty;

		// Token: 0x040001B9 RID: 441
		private List<GAFAtlasData> m_Atlases = new List<GAFAtlasData>();

		// Token: 0x040001BA RID: 442
		private List<GAFObjectData> m_Objects = new List<GAFObjectData>();

		// Token: 0x040001BB RID: 443
		private List<GAFObjectData> m_Masks = new List<GAFObjectData>();

		// Token: 0x040001BC RID: 444
		private Dictionary<uint, GAFFrameData> m_Frames = new Dictionary<uint, GAFFrameData>();

		// Token: 0x040001BD RID: 445
		private Dictionary<uint, List<GAFObjectStateData>> m_States = new Dictionary<uint, List<GAFObjectStateData>>();

		// Token: 0x040001BE RID: 446
		private List<GAFSequenceData> m_Sequences = new List<GAFSequenceData>();

		// Token: 0x040001BF RID: 447
		private List<GAFNamedPartData> m_NamedParts = new List<GAFNamedPartData>();

		// Token: 0x040001C0 RID: 448
		private GAFTimelineData m_Parent;

		// Token: 0x040001C1 RID: 449
		private List<GAFTimelineData> m_Childs = new List<GAFTimelineData>();

		// Token: 0x040001C2 RID: 450
		private Rect m_FrameSize;

		// Token: 0x040001C3 RID: 451
		private Vector2 m_Pivot;
	}
}
