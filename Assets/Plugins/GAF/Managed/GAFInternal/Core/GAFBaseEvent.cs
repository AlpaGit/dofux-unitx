using Random = UnityEngine.Random;

namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x0200008D RID: 141
	public abstract class GAFBaseEvent
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00012AF6 File Offset: 0x00010CF6
		public GAFBaseEvent.ActionType type
		{
			get
			{
				return this.m_Type;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00012AFE File Offset: 0x00010CFE
		public string target
		{
			get
			{
				return this.m_Target;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00012B06 File Offset: 0x00010D06
		public int id
		{
			get
			{
				return this.m_ID;
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00012B0E File Offset: 0x00010D0E
		internal GAFBaseEvent()
		{
			this.m_ID = Random.Range(1, int.MaxValue);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00012B35 File Offset: 0x00010D35
		internal GAFBaseEvent(GAFBaseEvent.ActionType _ActionType, string _Target) : this()
		{
			this.m_Type = _ActionType;
			this.m_Target = _Target;
		}

		// Token: 0x0600045C RID: 1116
		public abstract void execute(GAFBaseClip _Clip);

		// Token: 0x04000230 RID: 560
		protected GAFBaseEvent.ActionType m_Type = GAFBaseEvent.ActionType.None;

		// Token: 0x04000231 RID: 561
		protected string m_Target;

		// Token: 0x04000232 RID: 562
		protected int m_ID = -1;

		// Token: 0x0200008E RID: 142
		public enum ActionType
		{
			// Token: 0x04000234 RID: 564
			None = -1,
			// Token: 0x04000235 RID: 565
			Stop,
			// Token: 0x04000236 RID: 566
			Play,
			// Token: 0x04000237 RID: 567
			GotoAndStop,
			// Token: 0x04000238 RID: 568
			GotoAndPlay,
			// Token: 0x04000239 RID: 569
			DispatchEvent,
			// Token: 0x0400023A RID: 570
			CustomEvent,
			// Token: 0x0400023B RID: 571
			SoundEvent
		}
	}
}
