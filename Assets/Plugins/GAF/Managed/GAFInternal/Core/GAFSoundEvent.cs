namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x02000078 RID: 120
	public class GAFSoundEvent : GAFBaseEvent
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00010A3E File Offset: 0x0000EC3E
		public GAFSoundEvent.SoundAction action
		{
			get
			{
				return this.m_Action;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00010A46 File Offset: 0x0000EC46
		public int repeat
		{
			get
			{
				return this.m_Repeat;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00010A4E File Offset: 0x0000EC4E
		public string linkage
		{
			get
			{
				return this.m_Linkage;
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00010A56 File Offset: 0x0000EC56
		internal GAFSoundEvent(int _ID, GAFSoundEvent.SoundAction _Action, int _Repeat, string _Linkage, GAFBaseEvent.ActionType _ActionType, string _Target) : base(_ActionType, _Target)
		{
			this.m_ID = _ID;
			this.m_Action = _Action;
			this.m_Repeat = _Repeat;
			this.m_Linkage = _Linkage;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00006E9C File Offset: 0x0000509C
		public override void execute(GAFBaseClip _Clip)
		{
		}

		// Token: 0x040001C9 RID: 457
		private GAFSoundEvent.SoundAction m_Action;

		// Token: 0x040001CA RID: 458
		private int m_Repeat;

		// Token: 0x040001CB RID: 459
		private string m_Linkage = string.Empty;

		// Token: 0x02000079 RID: 121
		public enum SoundAction
		{
			// Token: 0x040001CD RID: 461
			Stop = 1,
			// Token: 0x040001CE RID: 462
			Start,
			// Token: 0x040001CF RID: 463
			Continue
		}
	}
}
