using System;
using System.Collections.Generic;
using System.Text;
using GAF.Managed.Editor.Utils;
using GAF.Managed.GAFInternal;
using GAF.Managed.GAFInternal.Utils;
using UnityEditor;
using UnityEngine;

namespace GAF.Managed.Editor.Interactive
{
	// Token: 0x02000025 RID: 37
	[InitializeOnLoad]
	public static class GAFServerConnection
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000024A6 File Offset: 0x000006A6
		public static bool isConnected
		{
			get
			{
				return !string.IsNullOrEmpty(GAFServerConnection.m_URL);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000024B5 File Offset: 0x000006B5
		public static bool isAlpha
		{
			get
			{
				return !GAFServerConnection.isConnected || GAFServerConnection.m_URL.Contains("alpha");
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000024CF File Offset: 0x000006CF
		public static string apiURL
		{
			get
			{
				if (GAFServerConnection.isConnected)
				{
					return GAFServerConnection.m_URL;
				}
				return string.Empty;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000024E3 File Offset: 0x000006E3
		public static string urlBase
		{
			get
			{
				if (GAFServerConnection.isConnected)
				{
					return GAFServerConnection.m_URL.Substring(0, GAFServerConnection.m_URL.LastIndexOf("/") + 1);
				}
				return string.Empty;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000250E File Offset: 0x0000070E
		public static string urlPricing
		{
			get
			{
				if (GAFServerConnection.isConnected)
				{
					return GAFServerConnection.urlBase + "pricing";
				}
				return string.Empty;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000091 RID: 145 RVA: 0x0000252C File Offset: 0x0000072C
		public static string urlHelpdesk
		{
			get
			{
				if (GAFServerConnection.isConnected)
				{
					return GAFServerConnection.urlBase + "helpdesk";
				}
				return string.Empty;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000254A File Offset: 0x0000074A
		public static string urlAssetStore_GAFfree
		{
			get
			{
				return "https://www.assetstore.unity3d.com/en/#!/content/13593";
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00002551 File Offset: 0x00000751
		public static string urlAssetStore_GAFpro
		{
			get
			{
				return "https://www.assetstore.unity3d.com/en/#!/content/26880";
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00002558 File Offset: 0x00000758
		public static bool isActual
		{
			get
			{
				return GAFServerConnection.m_IsActual;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000255F File Offset: 0x0000075F
		public static bool isProduction
		{
			get
			{
				return GAFServerConnection.m_IsProduction;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00002566 File Offset: 0x00000766
		public static string currentVersion
		{
			get
			{
				return GAFServerConnection.m_CurrentVersion;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000097 RID: 151 RVA: 0x0000256D File Offset: 0x0000076D
		public static string currentReleaseNotes
		{
			get
			{
				return GAFServerConnection.m_ReleaseNotes;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00006324 File Offset: 0x00004524
		private static bool needToStart
		{
			get
			{
				DateTime d = DateTime.Now.AddSeconds(-EditorApplication.timeSinceStartup);
				if (!EditorPrefs.HasKey("GAFServerConnection_startInitVersionTime"))
				{
					EditorPrefs.SetString("GAFServerConnection_startInitVersionTime", d.ToString("o"));
					return true;
				}
				DateTime d2 = DateTime.Parse(EditorPrefs.GetString("GAFServerConnection_startInitVersionTime"));
				if (Math.Abs((d - d2).TotalSeconds) > 2.0)
				{
					EditorPrefs.SetString("GAFServerConnection_startInitVersionTime", d.ToString("o"));
					return true;
				}
				return false;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000063B4 File Offset: 0x000045B4
		static GAFServerConnection()
		{
			if (GAFServerConnection.needToStart || EditorPrefs.GetString("GAFServerConnection_URLBase", string.Empty) == string.Empty)
			{
				GAFServerConnection.start();
				return;
			}
			GAFServerConnection.m_URL = EditorPrefs.GetString("GAFServerConnection_URLBase");
			GAFServerConnection.m_IsActual = EditorPrefs.GetBool("GAFServerConnection_isActual");
			GAFServerConnection.m_IsProduction = EditorPrefs.GetBool("GAFServerConnection_isProduction");
			GAFServerConnection.m_CurrentVersion = EditorPrefs.GetString("GAFServerConnection_currentVersion");
			GAFServerConnection.m_ReleaseNotes = EditorPrefs.GetString("GAFServerConnection_releaseNotes");
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002574 File Offset: 0x00000774
		private static void start()
		{
			EditorPrefs.SetString("GAFServerConnection_URLBase", string.Empty);
			GAFServerConnection.m_Queue = GAFServerConnection.m_TaskManager.queue();
			GAFServerConnection.settingsRequestLoop();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00006470 File Offset: 0x00004670
		private static void settingsRequestLoop()
		{
			GAFServerConnection.sendGetSettingsRequest(new Action<WWW>(GAFServerConnection.parseResponse));
			GAFServerConnection.m_Queue.thenWaitFor(GAFServerConnection.isConnected ? 600f : 10f).then(delegate
			{
				GAFServerConnection.settingsRequestLoop();
			});
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000064D0 File Offset: 0x000046D0
		private static void sendGetSettingsRequest(Action<WWW> _Callback)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2["version"] = GAFSystem.VersionString;
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			dictionary3["type"] = "unity_pro";
			dictionary["common"] = dictionary2;
			dictionary["data"] = dictionary3;
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("cmd", "init_version");
			wwwform.AddField("data", Json.Serialize(dictionary));
			WWW www = new WWW("https://gafmedia.com/api2.php", wwwform);
			GAFServerConnection.m_Queue.thenWaitUntil((float elapsed) => www.isDone).then(delegate
			{
				_Callback(www);
			});
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00006598 File Offset: 0x00004798
		private static void parseResponse(WWW _Request)
		{
			if (string.IsNullOrEmpty(_Request.error))
			{
				Dictionary<string, object> dictionary = Json.Deserialize(new UTF8Encoding().GetString(_Request.bytes)) as Dictionary<string, object>;
				if (dictionary != null && dictionary.ContainsKey("status") && dictionary["status"] as string == "ok" && dictionary.ContainsKey("data"))
				{
					Dictionary<string, object> dictionary2 = dictionary["data"] as Dictionary<string, object>;
					if (dictionary2 != null && dictionary2.ContainsKey("url") && dictionary2.ContainsKey("is_actual") && dictionary2.ContainsKey("is_production") && dictionary2.ContainsKey("current_version") && dictionary2.ContainsKey("current_release_notes"))
					{
						string text = dictionary2["url"] as string;
						string text2 = dictionary2["is_actual"] as string;
						string text3 = dictionary2["is_production"] as string;
						string text4 = dictionary2["current_version"] as string;
						string text5 = dictionary2["current_release_notes"] as string;
						if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3) && !string.IsNullOrEmpty(text4))
						{
							EditorPrefs.SetString("GAFServerConnection_URLBase", text);
							EditorPrefs.SetBool("GAFServerConnection_isActual", text2 == "1");
							EditorPrefs.SetBool("GAFServerConnection_isProduction", text3 == "1");
							EditorPrefs.SetString("GAFServerConnection_currentVersion", text4);
							EditorPrefs.SetString("GAFServerConnection_releaseNotes", text5);
							GAFServerConnection.m_URL = text;
							GAFServerConnection.m_IsActual = (text2 == "1");
							GAFServerConnection.m_IsProduction = (text3 == "1");
							GAFServerConnection.m_CurrentVersion = text4;
							GAFServerConnection.m_ReleaseNotes = text5;
						}
					}
				}
			}
			_Request.Dispose();
		}

		// Token: 0x04000076 RID: 118
		private static GAFTaskManager m_TaskManager = new GAFTaskManager();

		// Token: 0x04000077 RID: 119
		private static GAFTaskQueue m_Queue = null;

		// Token: 0x04000078 RID: 120
		private static string m_URL = string.Empty;

		// Token: 0x04000079 RID: 121
		private static bool m_IsActual = true;

		// Token: 0x0400007A RID: 122
		private static bool m_IsProduction = true;

		// Token: 0x0400007B RID: 123
		private static string m_CurrentVersion = string.Empty;

		// Token: 0x0400007C RID: 124
		private static string m_ReleaseNotes = string.Empty;
	}
}
