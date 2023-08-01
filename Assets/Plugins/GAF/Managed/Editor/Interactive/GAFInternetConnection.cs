using GAF.Managed.Editor.Utils;
using UnityEngine;

namespace GAF.Managed.Editor.Interactive
{
	// Token: 0x02000023 RID: 35
	public static class GAFInternetConnection
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000062A4 File Offset: 0x000044A4
		public static bool isConnected
		{
			get
			{
				if (GAFInternetConnection.m_LastPing == null)
				{
					GAFInternetConnection.m_LastPing = new WWW("google.com");
					GAFInternetConnection.m_TaskManager = new GAFTaskManager();
					GAFInternetConnection.m_TaskManager.waitUntil((float elapsed) => GAFInternetConnection.m_LastPing.isDone).then(delegate
					{
						GAFInternetConnection.m_IsConnected = string.IsNullOrEmpty(GAFInternetConnection.m_LastPing.error);
						GAFInternetConnection.m_LastPing = null;
					});
				}
				return GAFInternetConnection.m_IsConnected;
			}
		}

		// Token: 0x04000070 RID: 112
		private static bool m_IsConnected;

		// Token: 0x04000071 RID: 113
		private static WWW m_LastPing;

		// Token: 0x04000072 RID: 114
		private static GAFTaskManager m_TaskManager;
	}
}
