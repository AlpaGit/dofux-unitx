using System;
using System.Collections.Generic;

namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x02000090 RID: 144
	public class GAFGoToEvent : GAFBaseEvent
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x00012CA1 File Offset: 0x00010EA1
		internal GAFGoToEvent(uint _FrameNumber, GAFBaseEvent.ActionType _ActionType, string _Target) : base(_ActionType, _Target)
		{
			this.m_FrameNumber = _FrameNumber;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00012CB4 File Offset: 0x00010EB4
		internal void subscribe(Action<uint> _Callback, int _ClipInstanceID)
		{
			if (this.m_Callbacks == null)
			{
				this.m_Callbacks = new Dictionary<int, Action<uint>>();
			}
			if (this.m_Callbacks.ContainsKey(_ClipInstanceID))
			{
				var callbacks = this.m_Callbacks;
				callbacks[_ClipInstanceID] = (Action<uint>)Delegate.Combine(callbacks[_ClipInstanceID], _Callback);
				return;
			}
			this.m_Callbacks[_ClipInstanceID] = _Callback;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00012D14 File Offset: 0x00010F14
		internal void unsubscribe(Action<uint> _Callback, int _ClipInstanceID)
		{
			if (this.m_Callbacks != null && this.m_Callbacks.ContainsKey(_ClipInstanceID))
			{
				var callbacks = this.m_Callbacks;
				callbacks[_ClipInstanceID] = (Action<uint>)Delegate.Remove(callbacks[_ClipInstanceID], _Callback);
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00012D5C File Offset: 0x00010F5C
		public override void execute(GAFBaseClip _Clip)
		{
			var instanceID = _Clip.gameObject.GetInstanceID();
			if (this.m_Callbacks != null && this.m_Callbacks.ContainsKey(instanceID) && this.m_Callbacks[instanceID] != null)
			{
				this.m_Callbacks[instanceID](this.m_FrameNumber);
			}
		}

		// Token: 0x0400023E RID: 574
		private Dictionary<int, Action<uint>> m_Callbacks;

		// Token: 0x0400023F RID: 575
		private uint m_FrameNumber;
	}
}
