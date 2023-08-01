using System;
using GAF.Managed.Editor.Interactive;
using GAF.Managed.Editor.Utils;
using GAF.Managed.GAFInternal;
using UnityEngine;

namespace GAF.Managed.Editor.Tracking
{
	// Token: 0x0200000F RID: 15
	public sealed class GAFTracking
	{
		// Token: 0x06000032 RID: 50 RVA: 0x0000363C File Offset: 0x0000183C
		public static void sendAssetCreatedRequest(string _AssetName)
		{
			if (GAFInternetConnection.isConnected)
			{
				try
				{
					var wwwform = new WWWForm();
					wwwform.AddField("ID", SystemInfo.deviceUniqueIdentifier);
					wwwform.AddField("TargetPlatform", Application.platform.ToString());
					wwwform.AddField("UserTime", DateTime.Now.ToString());
					wwwform.AddField("GAFVersion", GAFSystem.VersionString);
					wwwform.AddField("UnityVersion", Application.unityVersion);
					wwwform.AddField("HasUnityPro", Application.HasProLicense().ToString());
					wwwform.AddField("EventType", GAFTracking.GAFTrackingEventType.AssetCreated.ToString());
					wwwform.AddField("AssetName", _AssetName);
					var www = new WWW(string.Format("{0}", Array.Empty<object>()), wwwform);
					GAFTracking.m_TaskManager.waitUntil((float elapsed) => www.isDone);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003750 File Offset: 0x00001950
		public static void sendMovieClipCreatedRequest(string _UsedAssetName)
		{
			if (GAFInternetConnection.isConnected)
			{
				try
				{
					WWWForm wwwform = new WWWForm();
					wwwform.AddField("ID", SystemInfo.deviceUniqueIdentifier);
					wwwform.AddField("TargetPlatform", Application.platform.ToString());
					wwwform.AddField("UserTime", DateTime.Now.ToString());
					wwwform.AddField("GAFVersion", GAFSystem.VersionString);
					wwwform.AddField("UnityVersion", Application.unityVersion);
					wwwform.AddField("HasUnityPro", Application.HasProLicense().ToString());
					wwwform.AddField("EventType", GAFTracking.GAFTrackingEventType.MovieClipCreated.ToString());
					wwwform.AddField("AssetName", _UsedAssetName);
					WWW www = new WWW(GAFTracking.gafUrl, wwwform);
					GAFTracking.m_TaskManager.waitUntil((float elapsed) => www.isDone);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003858 File Offset: 0x00001A58
		public static void sendPromoButtonPressedRequest()
		{
			if (GAFInternetConnection.isConnected)
			{
				try
				{
					WWWForm wwwform = new WWWForm();
					wwwform.AddField("ID", SystemInfo.deviceUniqueIdentifier);
					wwwform.AddField("TargetPlatform", Application.platform.ToString());
					wwwform.AddField("UserTime", DateTime.Now.ToString());
					wwwform.AddField("GAFVersion", GAFSystem.VersionString);
					wwwform.AddField("UnityVersion", Application.unityVersion);
					wwwform.AddField("HasUnityPro", Application.HasProLicense().ToString());
					wwwform.AddField("EventType", GAFTracking.GAFTrackingEventType.PromoButtonPressed.ToString());
					WWW www = new WWW(GAFTracking.gafUrl, wwwform);
					GAFTracking.m_TaskManager.waitUntil((float elapsed) => www.isDone);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003954 File Offset: 0x00001B54
		public static void sendOpenConverterWindowAfterCopyRequest(uint _AnimationCount)
		{
			if (GAFInternetConnection.isConnected)
			{
				try
				{
					WWWForm wwwform = new WWWForm();
					wwwform.AddField("ID", SystemInfo.deviceUniqueIdentifier);
					wwwform.AddField("TargetPlatform", Application.platform.ToString());
					wwwform.AddField("UserTime", DateTime.Now.ToString());
					wwwform.AddField("GAFVersion", GAFSystem.VersionString);
					wwwform.AddField("UnityVersion", Application.unityVersion);
					wwwform.AddField("HasUnityPro", Application.HasProLicense().ToString());
					wwwform.AddField("EventType", GAFTracking.GAFTrackingEventType.OpenConverterWindowAfterCopy.ToString());
					wwwform.AddField("AnimationsCount", _AnimationCount.ToString());
					WWW www = new WWW(GAFTracking.gafUrl, wwwform);
					GAFTracking.m_TaskManager.waitUntil((float elapsed) => www.isDone);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003A60 File Offset: 0x00001C60
		public static void sendDragAndDropFilesToConverterRequest(uint _AnimationCount)
		{
			if (GAFInternetConnection.isConnected)
			{
				try
				{
					WWWForm wwwform = new WWWForm();
					wwwform.AddField("ID", SystemInfo.deviceUniqueIdentifier);
					wwwform.AddField("TargetPlatform", Application.platform.ToString());
					wwwform.AddField("UserTime", DateTime.Now.ToString());
					wwwform.AddField("GAFVersion", GAFSystem.VersionString);
					wwwform.AddField("UnityVersion", Application.unityVersion);
					wwwform.AddField("HasUnityPro", Application.HasProLicense().ToString());
					wwwform.AddField("EventType", GAFTracking.GAFTrackingEventType.DragAndDropFilesToConverter.ToString());
					wwwform.AddField("AnimationsCount", _AnimationCount.ToString());
					WWW www = new WWW(GAFTracking.gafUrl, wwwform);
					GAFTracking.m_TaskManager.waitUntil((float elapsed) => www.isDone);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003B6C File Offset: 0x00001D6C
		public static void sendCreatePrefabFromConverterRequest(uint _AnimationsCount)
		{
			if (GAFInternetConnection.isConnected)
			{
				try
				{
					WWWForm wwwform = new WWWForm();
					wwwform.AddField("ID", SystemInfo.deviceUniqueIdentifier);
					wwwform.AddField("TargetPlatform", Application.platform.ToString());
					wwwform.AddField("UserTime", DateTime.Now.ToString());
					wwwform.AddField("GAFVersion", GAFSystem.VersionString);
					wwwform.AddField("UnityVersion", Application.unityVersion);
					wwwform.AddField("HasUnityPro", Application.HasProLicense().ToString());
					wwwform.AddField("EventType", GAFTracking.GAFTrackingEventType.CreatePrefabFromConverter.ToString());
					wwwform.AddField("AnimationsCount", _AnimationsCount.ToString());
					WWW www = new WWW(GAFTracking.gafUrl, wwwform);
					GAFTracking.m_TaskManager.waitUntil((float elapsed) => www.isDone);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003C78 File Offset: 0x00001E78
		public static void sendCreateGameObjectFromConverterRequest(uint _AnimationsCount)
		{
			if (GAFInternetConnection.isConnected)
			{
				try
				{
					WWWForm wwwform = new WWWForm();
					wwwform.AddField("ID", SystemInfo.deviceUniqueIdentifier);
					wwwform.AddField("TargetPlatform", Application.platform.ToString());
					wwwform.AddField("UserTime", DateTime.Now.ToString());
					wwwform.AddField("GAFVersion", GAFSystem.VersionString);
					wwwform.AddField("UnityVersion", Application.unityVersion);
					wwwform.AddField("HasUnityPro", Application.HasProLicense().ToString());
					wwwform.AddField("EventType", GAFTracking.GAFTrackingEventType.CreateGameObjectFromConverter.ToString());
					wwwform.AddField("AnimationsCount", _AnimationsCount.ToString());
					WWW www = new WWW(GAFTracking.gafUrl, wwwform);
					GAFTracking.m_TaskManager.waitUntil((float elapsed) => www.isDone);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003D84 File Offset: 0x00001F84
		public static void sendReconvertButtonPressed()
		{
			if (GAFInternetConnection.isConnected)
			{
				try
				{
					WWWForm wwwform = new WWWForm();
					wwwform.AddField("ID", SystemInfo.deviceUniqueIdentifier);
					wwwform.AddField("TargetPlatform", Application.platform.ToString());
					wwwform.AddField("UserTime", DateTime.Now.ToString());
					wwwform.AddField("GAFVersion", GAFSystem.VersionString);
					wwwform.AddField("UnityVersion", Application.unityVersion);
					wwwform.AddField("HasUnityPro", Application.HasProLicense().ToString());
					wwwform.AddField("EventType", GAFTracking.GAFTrackingEventType.ReconvertButtonPressed.ToString());
					WWW www = new WWW(GAFTracking.gafUrl, wwwform);
					GAFTracking.m_TaskManager.waitUntil((float elapsed) => www.isDone);
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003E80 File Offset: 0x00002080
		public static void sendGAFProADPressed()
		{
			if (GAFInternetConnection.isConnected)
			{
				try
				{
					WWWForm wwwform = new WWWForm();
					wwwform.AddField("ID", SystemInfo.deviceUniqueIdentifier);
					wwwform.AddField("TargetPlatform", Application.platform.ToString());
					wwwform.AddField("UserTime", DateTime.Now.ToString());
					wwwform.AddField("GAFVersion", GAFSystem.VersionString);
					wwwform.AddField("UnityVersion", Application.unityVersion);
					wwwform.AddField("HasUnityPro", Application.HasProLicense().ToString());
					wwwform.AddField("EventType", GAFTracking.GAFTrackingEventType.GAFProADPressed.ToString());
					WWW www = new WWW(GAFTracking.gafUrl, wwwform);
					GAFTracking.m_TaskManager.waitUntil((float elapsed) => www.isDone);
				}
				catch
				{
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000021BC File Offset: 0x000003BC
		private static string gafUrl
		{
			get
			{
				return GAFServerConnection.urlBase + "converter/unity_pro_stats.php";
			}
		}

		// Token: 0x04000012 RID: 18
		private static GAFTaskManager m_TaskManager = new GAFTaskManager();

		// Token: 0x02000010 RID: 16
		private enum GAFTrackingEventType
		{
			// Token: 0x04000014 RID: 20
			AssetCreated,
			// Token: 0x04000015 RID: 21
			MovieClipCreated,
			// Token: 0x04000016 RID: 22
			PromoButtonPressed,
			// Token: 0x04000017 RID: 23
			OpenConverterWindowAfterCopy,
			// Token: 0x04000018 RID: 24
			DragAndDropFilesToConverter,
			// Token: 0x04000019 RID: 25
			CreatePrefabFromConverter,
			// Token: 0x0400001A RID: 26
			CreateGameObjectFromConverter,
			// Token: 0x0400001B RID: 27
			ReconvertButtonPressed,
			// Token: 0x0400001C RID: 28
			GAFProADPressed
		}
	}
}
