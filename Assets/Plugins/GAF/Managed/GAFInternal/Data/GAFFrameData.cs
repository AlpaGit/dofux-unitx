using System.Collections.Generic;
using GAF.Managed.GAFInternal.Core;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x0200006D RID: 109
	public class GAFFrameData
	{
		// Token: 0x06000311 RID: 785 RVA: 0x000103D9 File Offset: 0x0000E5D9
		public GAFFrameData(uint _FrameNumber)
		{
			this.m_FrameNumber = _FrameNumber;
			this.m_Events = new List<GAFBaseEvent>();
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000312 RID: 786 RVA: 0x000103F3 File Offset: 0x0000E5F3
		public uint frameNumber
		{
			get
			{
				return this.m_FrameNumber;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000313 RID: 787 RVA: 0x000103FB File Offset: 0x0000E5FB
		public List<GAFBaseEvent> events
		{
			get
			{
				return this.m_Events;
			}
		}

		// Token: 0x0400018E RID: 398
		private uint m_FrameNumber;

		// Token: 0x0400018F RID: 399
		private List<GAFBaseEvent> m_Events;
	}
}
