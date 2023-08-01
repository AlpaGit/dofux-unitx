using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace GAF.Managed.Editor.Skin
{
	// Token: 0x0200001A RID: 26
	public class GAFStyles
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000050 RID: 80 RVA: 0x0000224E File Offset: 0x0000044E
		public static GAFStyles instance
		{
			get
			{
				if (GAFStyles.m_Instance == null)
				{
					GAFStyles.m_Instance = new GAFStyles();
					return GAFStyles.m_Instance;
				}
				return GAFStyles.m_Instance;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000051 RID: 81 RVA: 0x0000226C File Offset: 0x0000046C
		public Dictionary<GAFStyles.Style, GUIStyle> styles
		{
			get
			{
				if (this.m_Styles == null)
				{
					this.initStyles();
				}
				return this.m_Styles;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002282 File Offset: 0x00000482
		public Dictionary<string, Texture2D> textures
		{
			get
			{
				if (this.m_Textures == null)
				{
					this.initTextures();
				}
				return this.m_Textures;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002298 File Offset: 0x00000498
		public Vector2 windowSize
		{
			get
			{
				if (Application.platform != null)
				{
					return this.m_WindowSizeWin;
				}
				return this.m_WindowSizeOSX;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000022AE File Offset: 0x000004AE
		public Vector2 windowCheckForUpdSize
		{
			get
			{
				if (Application.platform != null)
				{
					return this.m_WindowCheckForUpdSizeWin;
				}
				return this.m_WindowCheckForUpdSizeOSX;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000022C4 File Offset: 0x000004C4
		public Vector2 selectViewSize
		{
			get
			{
				return this.m_SelectViewSize;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000022CC File Offset: 0x000004CC
		public Vector2 dragAreaSize
		{
			get
			{
				return this.m_DragAreaSize;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000022D4 File Offset: 0x000004D4
		public Vector2 notificationSize
		{
			get
			{
				return this.m_NotificationSize;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000022DC File Offset: 0x000004DC
		public Vector2 labelSize
		{
			get
			{
				return this.m_LabelSize;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000022E4 File Offset: 0x000004E4
		public Vector2 warningSize
		{
			get
			{
				return this.m_WarningSize;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000022EC File Offset: 0x000004EC
		public Vector2 bottomButtonSize
		{
			get
			{
				return this.m_BottomButtonSize;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000022F4 File Offset: 0x000004F4
		public float messageTextOffsetY
		{
			get
			{
				return this.m_MessageTextOffsetY;
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003F7C File Offset: 0x0000217C
		private void initStyles()
		{
			this.m_Styles = new Dictionary<GAFStyles.Style, GUIStyle>();
			GUIStyle guistyle = new GUIStyle(GUI.skin.label);
			guistyle.normal.background = null;
			guistyle.alignment         = TextAnchor.UpperCenter;
			guistyle.fontStyle         = FontStyle.Bold;
			this.m_Styles.Add(GAFStyles.Style.CenteredLabel, guistyle);
			GUIStyle guistyle2 = new GUIStyle(GUI.skin.label);
			guistyle2.normal.background = this.textures["dropAreaTexture"];
			guistyle2.normal.textColor  = new Color(0.5019608f, 0.49803922f, 0.49803922f);
			guistyle2.alignment         = TextAnchor.MiddleCenter;
			guistyle2.fontSize          = 20;
			guistyle2.fontStyle         = FontStyle.Bold;
			guistyle2.fixedWidth        = this.windowSize.x;
			guistyle2.fixedHeight       = this.dragAreaSize.y;
			this.m_Styles.Add(GAFStyles.Style.DropAreaLabel, guistyle2);
			GUIStyle guistyle3 = new GUIStyle(GUI.skin.label);
			guistyle3.fontStyle = 0;
			guistyle3.richText  = false;
			guistyle3.alignment = TextAnchor.UpperCenter;
			guistyle3.wordWrap  = false;
			this.m_Styles.Add(GAFStyles.Style.MessageLabel, guistyle3);
			GUIStyle guistyle4 = new GUIStyle(GUI.skin.button);
			guistyle4.fontSize = 12;
			this.m_Styles.Add(GAFStyles.Style.BottomButton, guistyle4);
			GUIStyle guistyle5 = new GUIStyle(GUI.skin.label);
			guistyle5.normal.background = null;
			guistyle5.alignment         = TextAnchor.MiddleCenter;
			guistyle5.fontStyle         = FontStyle.Bold;
			guistyle5.fontSize          = 18;
			this.m_Styles.Add(GAFStyles.Style.SettingsLabel, guistyle5);
			GUIStyle guistyle6 = new GUIStyle(GUI.skin.button);
			guistyle6.fixedWidth = 25f;
			guistyle6.fixedHeight = 24f;
			guistyle6.normal.background = this.textures["closeTextureNormal"];
			guistyle6.hover.background = this.textures["closeTextureHovered"];
			guistyle6.active.background = this.textures["closeTexturePressed"];
			this.m_Styles.Add(GAFStyles.Style.CloseButton, guistyle6);
			GUIStyle guistyle7 = new GUIStyle(GUI.skin.button);
			guistyle7.fixedWidth = 8f;
			guistyle7.fixedHeight = 7f;
			guistyle7.normal.background = this.textures["miniCloseTextureNormal"];
			guistyle7.active.background = this.textures["miniCloseTexturePressed"];
			this.m_Styles.Add(GAFStyles.Style.MiniCloseButton, guistyle7);
			GUIStyle guistyle8 = new GUIStyle(GUI.skin.button);
			guistyle8.fixedWidth = 16f;
			guistyle8.fixedHeight = 15f;
			guistyle8.normal.background = this.textures["addTextureNormal"];
			guistyle8.hover.background = this.textures["addTextureNormal"];
			guistyle8.active.background = this.textures["addTextureHovered"];
			this.m_Styles.Add(GAFStyles.Style.AddButton, guistyle8);
			GUIStyle guistyle9 = new GUIStyle(GUI.skin.button);
			guistyle9.fixedWidth = 16f;
			guistyle9.fixedHeight = 15f;
			guistyle9.normal.background = this.textures["removeTextureNormal"];
			guistyle9.hover.background = this.textures["removeTextureNormal"];
			guistyle9.active.background = this.textures["removeTextureHovered"];
			this.m_Styles.Add(GAFStyles.Style.RemoveButton, guistyle9);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004310 File Offset: 0x00002510
		private void initTextures()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			this.m_Textures = new Dictionary<string, Texture2D>();
			this.m_Textures["labelTexture"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("gafmedia"), "gafmedia");
			this.m_Textures["indicatorTexture"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("indicator"), "indicator");
			this.m_Textures["dropAreaTexture"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("droparea"), "droparea");
			this.m_Textures["closeTextureNormal"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("close_normal"), "close_normal");
			this.m_Textures["closeTextureHovered"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("close_hovered"), "close_hovered");
			this.m_Textures["closeTexturePressed"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("close_pressed"), "close_pressed");
			this.m_Textures["miniCloseTextureNormal"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("mini_close_normal"), "mini_close_normal");
			this.m_Textures["miniCloseTexturePressed"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("mini_close_pressed"), "mini_close_pressed");
			this.m_Textures["addTextureNormal"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("plus_normal"), "plus_normal");
			this.m_Textures["addTextureHovered"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("plus_hovered"), "plus_hovered");
			this.m_Textures["addTexturePressed"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("plus_pressed"), "plus_pressed");
			this.m_Textures["removeTextureNormal"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("minus_normal"), "minus_normal");
			this.m_Textures["removeTextureHovered"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("minus_hovered"), "minus_hovered");
			this.m_Textures["removeTexturePressed"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("minus_pressed"), "minus_pressed");
			this.m_Textures["warning"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("warning"), "warning");
			this.m_Textures["labelStudio"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("studio"), "studio");
			this.m_Textures["gafProFeature"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("gaf-pro-feature"), "gaf-pro-feature");
			this.m_Textures["demo"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("demo"), "demo");
			this.m_Textures["loginToUse"] = this.loadTextureFromStream(executingAssembly.GetManifestResourceStream("login-to-use"), "login-to-use");
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004600 File Offset: 0x00002800
		private Texture2D loadTextureFromStream(Stream _Stream, string _Name)
		{
			byte[] array = new byte[_Stream.Length];
			_Stream.Read(array, 0, (int)_Stream.Length);
			Texture2D texture2D = new Texture2D(4, 4, TextureFormat.ARGB32, false);
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.hideFlags  = HideFlags.DontSave;
			texture2D.name       = _Name;
			ImageConversion.LoadImage(texture2D, array);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x04000026 RID: 38
		private static GAFStyles m_Instance;

		// Token: 0x04000027 RID: 39
		private Dictionary<GAFStyles.Style, GUIStyle> m_Styles;

		// Token: 0x04000028 RID: 40
		private Dictionary<string, Texture2D> m_Textures;

		// Token: 0x04000029 RID: 41
		private readonly Vector2 m_WindowSizeWin = new Vector2(415f, 430f);

		// Token: 0x0400002A RID: 42
		private readonly Vector2 m_WindowSizeOSX = new Vector2(415f, 445f);

		// Token: 0x0400002B RID: 43
		private readonly Vector2 m_WindowCheckForUpdSizeWin = new Vector2(350f, 230f);

		// Token: 0x0400002C RID: 44
		private readonly Vector2 m_WindowCheckForUpdSizeOSX = new Vector2(350f, 245f);

		// Token: 0x0400002D RID: 45
		private readonly Vector2 m_SelectViewSize = new Vector2(365f, 107f);

		// Token: 0x0400002E RID: 46
		private readonly Vector2 m_DragAreaSize = new Vector2(365f, 125f);

		// Token: 0x0400002F RID: 47
		private readonly Vector2 m_NotificationSize = new Vector2(365f, 130f);

		// Token: 0x04000030 RID: 48
		private readonly Vector2 m_WarningSize = new Vector2(30f, 30f);

		// Token: 0x04000031 RID: 49
		private readonly Vector2 m_LabelSize = new Vector2(170f, 79f);

		// Token: 0x04000032 RID: 50
		private readonly Vector2 m_BottomButtonSize = new Vector2(170f, 27f);

		// Token: 0x04000033 RID: 51
		private readonly float m_MessageTextOffsetY = 15f;

		// Token: 0x0200001B RID: 27
		public enum Style
		{
			// Token: 0x04000035 RID: 53
			CenteredLabel,
			// Token: 0x04000036 RID: 54
			DropAreaLabel,
			// Token: 0x04000037 RID: 55
			MessageLabel,
			// Token: 0x04000038 RID: 56
			BottomButton,
			// Token: 0x04000039 RID: 57
			SettingsLabel,
			// Token: 0x0400003A RID: 58
			CloseButton,
			// Token: 0x0400003B RID: 59
			MiniCloseButton,
			// Token: 0x0400003C RID: 60
			AddButton,
			// Token: 0x0400003D RID: 61
			RemoveButton
		}
	}
}
