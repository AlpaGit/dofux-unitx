using System;
using System.Collections.Generic;

namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x02000091 RID: 145
	public class GAFPlaybackEvent : GAFBaseEvent
	{
		// Token: 0x06000467 RID: 1127 RVA: 0x00012DB0 File Offset: 0x00010FB0
		internal GAFPlaybackEvent(GAFBaseEvent.ActionType _ActionType, string _Target) : base(_ActionType, _Target)
		{
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00012DBC File Offset: 0x00010FBC
		internal void subscribe(Action _Callback, int _ClipInstanceID)
		{
			if (this.m_Callbacks == null)
			{
				this.m_Callbacks = new Dictionary<int, Action>();
			}
			if (this.m_Callbacks.ContainsKey(_ClipInstanceID))
			{
				var callbacks = this.m_Callbacks;
				callbacks[_ClipInstanceID] = (Action)Delegate.Combine(callbacks[_ClipInstanceID], _Callback);
				return;
			}
			this.m_Callbacks[_ClipInstanceID] = _Callback;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00012E1C File Offset: 0x0001101C
		internal void unsubscribe(Action _Callback, int _ClipInstanceID)
		{
			if (this.m_Callbacks != null && this.m_Callbacks.ContainsKey(_ClipInstanceID))
			{
				var callbacks = this.m_Callbacks;
				callbacks[_ClipInstanceID] = (Action)Delegate.Remove(callbacks[_ClipInstanceID], _Callback);
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00012E64 File Offset: 0x00011064
		public override void execute(GAFBaseClip _Clip)
		{
			var instanceID = _Clip.gameObject.GetInstanceID();
			if (this.m_Callbacks != null && this.m_Callbacks.ContainsKey(instanceID) && this.m_Callbacks[instanceID] != null)
			{
				this.m_Callbacks[instanceID]();
			}
		}

		// Token: 0x04000240 RID: 576
		private Dictionary<int, Action> m_Callbacks;
	}
}
