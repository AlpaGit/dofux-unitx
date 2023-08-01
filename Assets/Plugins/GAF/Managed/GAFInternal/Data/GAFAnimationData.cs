using System.Collections.Generic;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x02000068 RID: 104
	public class GAFAnimationData
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000FCC3 File Offset: 0x0000DEC3
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x0000FCCB File Offset: 0x0000DECB
		public ushort majorVersion
		{
			get
			{
				return this.m_MajorVersion;
			}
			set
			{
				this.m_MajorVersion = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000FCD4 File Offset: 0x0000DED4
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000FCDC File Offset: 0x0000DEDC
		public ushort minorVersion
		{
			get
			{
				return this.m_MinorVersion;
			}
			set
			{
				this.m_MinorVersion = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000FCE5 File Offset: 0x0000DEE5
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000FCED File Offset: 0x0000DEED
		public ushort fps
		{
			get
			{
				return this.m_FPS;
			}
			set
			{
				this.m_FPS = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000FCF6 File Offset: 0x0000DEF6
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000FCFE File Offset: 0x0000DEFE
		public Color color
		{
			get
			{
				return this.m_Color;
			}
			set
			{
				this.m_Color = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000FD07 File Offset: 0x0000DF07
		// (set) Token: 0x060002DB RID: 731 RVA: 0x0000FD0F File Offset: 0x0000DF0F
		public ushort width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000FD18 File Offset: 0x0000DF18
		// (set) Token: 0x060002DD RID: 733 RVA: 0x0000FD20 File Offset: 0x0000DF20
		public ushort height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000FD29 File Offset: 0x0000DF29
		public List<float> scales
		{
			get
			{
				return this.m_Scales;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000FD31 File Offset: 0x0000DF31
		public List<float> csfs
		{
			get
			{
				return this.m_CSFs;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000FD39 File Offset: 0x0000DF39
		public Dictionary<int, GAFTimelineData> timelines
		{
			get
			{
				return this.m_Timelines;
			}
		}
		public Dictionary<int, GAFTimelineData> secondaryTimelines
		{
			get
			{
				return this.m_SecondaryTimelines;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000FD41 File Offset: 0x0000DF41
		public GAFTimelineData rootTimeline
		{
			get
			{
				return this.m_Timelines[0];
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000FD4F File Offset: 0x0000DF4F
		public List<GAFSoundData> audioClips
		{
			get
			{
				return this.m_AudioClips;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000FD57 File Offset: 0x0000DF57
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x0000FD5F File Offset: 0x0000DF5F
		public Dictionary<float, List<GAFAtlasElementData>> customRegions
		{
			get
			{
				return this.m_CustomRegions;
			}
			set
			{
				this.m_CustomRegions = value;
			}
		}

		// Token: 0x04000160 RID: 352
		private ushort m_MajorVersion;

		// Token: 0x04000161 RID: 353
		private ushort m_MinorVersion;

		// Token: 0x04000162 RID: 354
		private ushort m_FPS = 30;

		// Token: 0x04000163 RID: 355
		private Color m_Color = Color.white;

		// Token: 0x04000164 RID: 356
		private ushort m_Width;

		// Token: 0x04000165 RID: 357
		private ushort m_Height;

		// Token: 0x04000166 RID: 358
		private List<float> m_Scales = new List<float>();

		// Token: 0x04000167 RID: 359
		private List<float> m_CSFs = new List<float>();

		// Token: 0x04000168 RID: 360
		private List<GAFSoundData> m_AudioClips = new List<GAFSoundData>();

		// Token: 0x04000169 RID: 361
		private Dictionary<float, List<GAFAtlasElementData>> m_CustomRegions = new Dictionary<float, List<GAFAtlasElementData>>();

		// Token: 0x0400016A RID: 362
		private Dictionary<int, GAFTimelineData> m_Timelines = new Dictionary<int, GAFTimelineData>();
		private Dictionary<int, GAFTimelineData> m_SecondaryTimelines = new Dictionary<int, GAFTimelineData>();

	}
}
