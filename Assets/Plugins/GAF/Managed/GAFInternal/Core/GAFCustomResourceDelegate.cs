using System;
using GAF.Managed.GAFInternal.Assets;
using GAF.Managed.GAFInternal.Attributes;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x02000085 RID: 133
	[Serializable]
	public class GAFCustomResourceDelegate
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00012163 File Offset: 0x00010363
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x00012181 File Offset: 0x00010381
		public Func<string, GAFTexturesResourceInternal> func
		{
			get
			{
				if (this.m_Func == null || this.m_ReloadFunc)
				{
					this.loadFunc();
				}
				return this.m_Func;
			}
			set
			{
				this.m_Func = value;
				if (value == null)
				{
					this.m_Data.clear();
				}
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00012198 File Offset: 0x00010398
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x000121A0 File Offset: 0x000103A0
		public GAFFuncData funcData
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				if (this.m_Data != value)
				{
					this.m_Data = value;
					this.m_Func = null;
				}
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000121BC File Offset: 0x000103BC
		private void loadFunc()
		{
			this.m_ReloadFunc = false;
			if (this.m_Data.target != null && this.m_Data.method != "None")
			{
				try
				{
					this.m_Func = GAFCustomResourceDelegate.DelegateCreator(this.m_Data.target, this.m_Data.method);
					return;
				}
				catch
				{
					this.m_Data.clear();
					this.m_Func = null;
					return;
				}
			}
			if (this.m_Data.targetType != null && this.m_Data.method != "None")
			{
				try
				{
					this.m_Func = GAFCustomResourceDelegate.StaticDelegateCreator(this.m_Data.targetType, this.m_Data.method);
					return;
				}
				catch
				{
					this.m_Data.clear();
					this.m_Func = null;
					return;
				}
			}
			this.m_Func = null;
		}

		// Token: 0x04000211 RID: 529
		[SerializeField]
		[GAFFunc(typeof(GAFTexturesResourceInternal), new Type[]
		{
			typeof(string)
		})]
		[HideInInspector]
		private GAFFuncData m_Data = new GAFFuncData();

		// Token: 0x04000212 RID: 530
		[SerializeField]
		[HideInInspector]
		private bool m_ReloadFunc;

		// Token: 0x04000213 RID: 531
		private Func<string, GAFTexturesResourceInternal> m_Func;

		// Token: 0x04000214 RID: 532
		public static Func<object, string, Func<string, GAFTexturesResourceInternal>> DelegateCreator;

		// Token: 0x04000215 RID: 533
		public static Func<string, string, Func<string, GAFTexturesResourceInternal>> StaticDelegateCreator;
	}
}
