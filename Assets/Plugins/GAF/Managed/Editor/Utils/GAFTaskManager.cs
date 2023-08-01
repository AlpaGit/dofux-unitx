using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace GAF.Managed.Editor.Utils
{
	// Token: 0x02000009 RID: 9
	public class GAFTaskManager
	{
		// Token: 0x06000011 RID: 17 RVA: 0x0000208F File Offset: 0x0000028F
		public GAFTaskManager()
		{
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(this.update));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00003334 File Offset: 0x00001534
		~GAFTaskManager()
		{
			if (this.m_Queues.Any<GAFTaskQueue>())
			{
				this.m_Queues.ForEach(delegate(GAFTaskQueue queue)
				{
					queue.interrupt();
				});
				this.m_Queues.Clear();
			}
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(this.update));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000033BC File Offset: 0x000015BC
		public GAFTaskQueue queue()
		{
			GAFTaskQueue gaftaskQueue = new GAFTaskQueue();
			this.m_Queues.Add(gaftaskQueue);
			return gaftaskQueue;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000033DC File Offset: 0x000015DC
		public GAFTaskQueue waitFor(float _Seconds)
		{
			GAFTaskQueue gaftaskQueue = new GAFTaskQueue();
			this.m_Queues.Add(gaftaskQueue);
			return gaftaskQueue.thenWaitFor(_Seconds);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00003404 File Offset: 0x00001604
		public GAFTaskQueue waitUntil(Func<float, bool> _Condition)
		{
			GAFTaskQueue gaftaskQueue = new GAFTaskQueue();
			this.m_Queues.Add(gaftaskQueue);
			return gaftaskQueue.thenWaitUntil(_Condition);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000020C2 File Offset: 0x000002C2
		public void cancelTask(GAFTaskQueue _Task)
		{
			if (this.m_Queues.Contains(_Task))
			{
				_Task.interrupt();
				this.m_Queues.Remove(_Task);
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000342C File Offset: 0x0000162C
		private void update()
		{
			if (this.m_Queues.Any<GAFTaskQueue>())
			{
				this.m_Queues.ForEach(delegate(GAFTaskQueue queue)
				{
					queue.update(0.01f);
				});
				this.m_Queues.RemoveAll((GAFTaskQueue queue) => queue.isCompleted);
			}
		}

		// Token: 0x04000002 RID: 2
		private readonly List<GAFTaskQueue> m_Queues = new List<GAFTaskQueue>();
	}
}
