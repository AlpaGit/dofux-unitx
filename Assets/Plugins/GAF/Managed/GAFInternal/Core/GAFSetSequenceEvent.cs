using System;
using System.Collections.Generic;

namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x02000077 RID: 119
	public class GAFSetSequenceEvent : GAFBaseEvent
	{
		// Token: 0x06000368 RID: 872 RVA: 0x00010918 File Offset: 0x0000EB18
		internal void subscribe(Action<string, bool> _Callback, int _ClipInstanceID)
		{
			if (this.m_Callbacks == null)
			{
				this.m_Callbacks = new Dictionary<int, Action<string, bool>>();
			}
			if (this.m_Callbacks.ContainsKey(_ClipInstanceID))
			{
				var callbacks = this.m_Callbacks;
				callbacks[_ClipInstanceID] = (Action<string, bool>)Delegate.Combine(callbacks[_ClipInstanceID], _Callback);
				return;
			}
			this.m_Callbacks[_ClipInstanceID] = _Callback;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00010978 File Offset: 0x0000EB78
		internal void unsubscribe(Action<string, bool> _Callback, int _ClipInstanceID)
		{
			if (this.m_Callbacks != null && this.m_Callbacks.ContainsKey(_ClipInstanceID))
			{
				var callbacks = this.m_Callbacks;
				callbacks[_ClipInstanceID] = (Action<string, bool>)Delegate.Remove(callbacks[_ClipInstanceID], _Callback);
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000109BD File Offset: 0x0000EBBD
		internal GAFSetSequenceEvent(string _Sequence, bool _PlayImmediately, GAFBaseEvent.ActionType _ActionType, string _Target) : base(_ActionType, _Target)
		{
			this.m_SequenceName = _Sequence;
			this.m_PlayImmediately = _PlayImmediately;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x000109E4 File Offset: 0x0000EBE4
		public override void execute(GAFBaseClip _Clip)
		{
			var instanceID = _Clip.gameObject.GetInstanceID();
			if (this.m_Callbacks != null && this.m_Callbacks.ContainsKey(instanceID) && this.m_Callbacks[instanceID] != null)
			{
				this.m_Callbacks[instanceID](this.m_SequenceName, this.m_PlayImmediately);
			}
		}

		// Token: 0x040001C6 RID: 454
		private Dictionary<int, Action<string, bool>> m_Callbacks;

		// Token: 0x040001C7 RID: 455
		private string m_SequenceName = string.Empty;

		// Token: 0x040001C8 RID: 456
		private bool m_PlayImmediately;
	}
}
