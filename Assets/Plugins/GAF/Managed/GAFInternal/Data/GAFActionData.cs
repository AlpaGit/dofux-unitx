using System.Collections.Generic;

namespace GAF.Managed.GAFInternal.Data
{
	// Token: 0x02000063 RID: 99
	public class GAFActionData
	{
		// Token: 0x060002CF RID: 719 RVA: 0x0000FC8B File Offset: 0x0000DE8B
		public GAFActionData(GAFActionData.ActionType _Type, string _Scope)
		{
			this.m_Type = _Type;
			this.m_Scope = _Scope;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000FCB3 File Offset: 0x0000DEB3
		public GAFActionData.ActionType type
		{
			get
			{
				return this.m_Type;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000FCBB File Offset: 0x0000DEBB
		public List<string> parameters
		{
			get
			{
				return this.m_Parameters;
			}
		}

		// Token: 0x04000145 RID: 325
		private GAFActionData.ActionType m_Type = GAFActionData.ActionType.None;

		// Token: 0x04000146 RID: 326
		private List<string> m_Parameters = new List<string>();

		// Token: 0x04000147 RID: 327
		private string m_Scope;

		// Token: 0x02000064 RID: 100
		public enum ActionType
		{
			// Token: 0x04000149 RID: 329
			None = -1,
			// Token: 0x0400014A RID: 330
			Stop,
			// Token: 0x0400014B RID: 331
			Play,
			// Token: 0x0400014C RID: 332
			GotoAndStop,
			// Token: 0x0400014D RID: 333
			GotoAndPlay,
			// Token: 0x0400014E RID: 334
			DispatchEvent
		}
	}
}
