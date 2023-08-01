using System;
using System.Collections.Generic;

namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x0200008F RID: 143
	public class GAFDispatchEvent : GAFBaseEvent
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x00012B4B File Offset: 0x00010D4B
		internal GAFDispatchEvent(List<string> _Parameters, GAFBaseEvent.ActionType _ActionType, string _Target) : base(_ActionType, _Target)
		{
			this.m_Parameters = _Parameters;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00012B68 File Offset: 0x00010D68
		internal void subscribe(Action<List<string>, GAFBaseClip> _Callback, int _ClipInstanceID)
		{
			if (this.m_Callbacks == null)
			{
				this.m_Callbacks = new Dictionary<int, Action<List<string>, GAFBaseClip>>();
			}
			if (this.m_Callbacks.ContainsKey(_ClipInstanceID))
			{
				var callbacks = this.m_Callbacks;
				callbacks[_ClipInstanceID] = (Action<List<string>, GAFBaseClip>)Delegate.Combine(callbacks[_ClipInstanceID], _Callback);
				return;
			}
			this.m_Callbacks[_ClipInstanceID] = _Callback;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00012BC8 File Offset: 0x00010DC8
		internal void unsubscribe(Action<List<string>, GAFBaseClip> _Callback, int _ClipInstanceID)
		{
			if (this.m_Callbacks != null && this.m_Callbacks.ContainsKey(_ClipInstanceID))
			{
				var callbacks = this.m_Callbacks;
				callbacks[_ClipInstanceID] = (Action<List<string>, GAFBaseClip>)Delegate.Remove(callbacks[_ClipInstanceID], _Callback);
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00012C0D File Offset: 0x00010E0D
		internal void clear(int _ClipInstanceID)
		{
			if (this.m_Callbacks != null && this.m_Callbacks.ContainsKey(_ClipInstanceID))
			{
				this.m_Callbacks.Remove(_ClipInstanceID);
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00012C32 File Offset: 0x00010E32
		internal bool hasCallbacks(int _InstanceID)
		{
			return this.m_Callbacks != null && this.m_Callbacks.ContainsKey(_InstanceID);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00012C4C File Offset: 0x00010E4C
		public override void execute(GAFBaseClip _Clip)
		{
			var instanceID = _Clip.gameObject.GetInstanceID();
			if (this.m_Callbacks != null && this.m_Callbacks.ContainsKey(instanceID) && this.m_Callbacks[instanceID] != null)
			{
				this.m_Callbacks[instanceID](this.m_Parameters, _Clip);
			}
		}

		// Token: 0x0400023C RID: 572
		private List<string> m_Parameters = new List<string>();

		// Token: 0x0400023D RID: 573
		private Dictionary<int, Action<List<string>, GAFBaseClip>> m_Callbacks;
	}
}
