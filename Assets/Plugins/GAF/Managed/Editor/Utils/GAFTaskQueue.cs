using System;
using System.Collections.Generic;
using System.Linq;

namespace GAF.Managed.Editor.Utils
{
	// Token: 0x0200000B RID: 11
	public class GAFTaskQueue
	{
		// Token: 0x0600001D RID: 29 RVA: 0x0000210E File Offset: 0x0000030E
		internal GAFTaskQueue()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002121 File Offset: 0x00000321
		public bool isCompleted
		{
			get
			{
				return !this.m_Steps.Any<GAFTaskQueue.Step>();
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000349C File Offset: 0x0000169C
		public GAFTaskQueue then(Action _Action)
		{
			GAFTaskQueue.Step step = GAFTaskQueue.newStep();
			step.type = GAFTaskQueue.StepType.Action;
			step.action = _Action;
			this.m_Steps.Enqueue(step);
			return this;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000034CC File Offset: 0x000016CC
		public GAFTaskQueue thenWaitFor(float _Seconds)
		{
			GAFTaskQueue.Step step = GAFTaskQueue.newStep();
			step.type = GAFTaskQueue.StepType.TimeWait;
			step.length = _Seconds;
			this.m_Steps.Enqueue(step);
			return this;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000034FC File Offset: 0x000016FC
		public GAFTaskQueue thenWaitUntil(Func<float, bool> _Condition)
		{
			GAFTaskQueue.Step step = GAFTaskQueue.newStep();
			step.type = GAFTaskQueue.StepType.ConditionWait;
			step.condition = _Condition;
			this.m_Steps.Enqueue(step);
			return this;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002131 File Offset: 0x00000331
		private static GAFTaskQueue.Step newStep()
		{
			GAFTaskQueue.Step step = GAFTaskQueue.m_StepCache.Any<GAFTaskQueue.Step>() ? GAFTaskQueue.m_StepCache.Pop() : new GAFTaskQueue.Step();
			step.timer = 0f;
			return step;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000352C File Offset: 0x0000172C
		internal void interrupt()
		{
			while (this.m_Steps.Count > 0)
			{
				GAFTaskQueue.Step step = this.m_Steps.Dequeue();
				step.action = null;
				step.condition = null;
				GAFTaskQueue.m_StepCache.Push(step);
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003570 File Offset: 0x00001770
		internal void update(float _Delta)
		{
			if (this.m_Steps.Count == 0)
			{
				return;
			}
			GAFTaskQueue.Step step = this.m_Steps.Peek();
			step.timer += _Delta;
			switch (step.type)
			{
			case GAFTaskQueue.StepType.Action:
				step.action.Invoke();
				step.action = null;
				GAFTaskQueue.m_StepCache.Push(this.m_Steps.Dequeue());
				return;
			case GAFTaskQueue.StepType.TimeWait:
				if (step.timer >= step.length)
				{
					GAFTaskQueue.m_StepCache.Push(this.m_Steps.Dequeue());
					return;
				}
				break;
			case GAFTaskQueue.StepType.ConditionWait:
				if (step.condition.Invoke(step.timer))
				{
					step.condition = null;
					GAFTaskQueue.m_StepCache.Push(this.m_Steps.Dequeue());
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x04000007 RID: 7
		private static readonly Stack<GAFTaskQueue.Step> m_StepCache = new Stack<GAFTaskQueue.Step>();

		// Token: 0x04000008 RID: 8
		private readonly Queue<GAFTaskQueue.Step> m_Steps = new Queue<GAFTaskQueue.Step>();

		// Token: 0x0200000C RID: 12
		private enum StepType
		{
			// Token: 0x0400000A RID: 10
			Action,
			// Token: 0x0400000B RID: 11
			TimeWait,
			// Token: 0x0400000C RID: 12
			ConditionWait
		}

		// Token: 0x0200000D RID: 13
		private class Step
		{
			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000026 RID: 38 RVA: 0x00002167 File Offset: 0x00000367
			// (set) Token: 0x06000027 RID: 39 RVA: 0x0000216F File Offset: 0x0000036F
			public GAFTaskQueue.StepType type { get; set; }

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x06000028 RID: 40 RVA: 0x00002178 File Offset: 0x00000378
			// (set) Token: 0x06000029 RID: 41 RVA: 0x00002180 File Offset: 0x00000380
			public float timer { get; set; }

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x0600002A RID: 42 RVA: 0x00002189 File Offset: 0x00000389
			// (set) Token: 0x0600002B RID: 43 RVA: 0x00002191 File Offset: 0x00000391
			public Action action { get; set; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x0600002C RID: 44 RVA: 0x0000219A File Offset: 0x0000039A
			// (set) Token: 0x0600002D RID: 45 RVA: 0x000021A2 File Offset: 0x000003A2
			public float length { get; set; }

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x0600002E RID: 46 RVA: 0x000021AB File Offset: 0x000003AB
			// (set) Token: 0x0600002F RID: 47 RVA: 0x000021B3 File Offset: 0x000003B3
			public Func<float, bool> condition { get; set; }
		}
	}
}
