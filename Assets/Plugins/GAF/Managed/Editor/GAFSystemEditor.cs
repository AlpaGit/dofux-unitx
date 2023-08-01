using System.IO;
using UnityEngine;

namespace GAF.Managed.Editor
{
	// Token: 0x02000002 RID: 2
	public static class GAFSystemEditor
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002BD0 File Offset: 0x00000DD0
		public static string getPluginPath()
		{
			if (string.IsNullOrEmpty(GAFSystemEditor._path) || !Directory.Exists(GAFSystemEditor._path))
			{
				string[] directories = Directory.GetDirectories(Application.dataPath, "GAF", SearchOption.AllDirectories);
				string text = Application.dataPath;
				if (directories.Length == 0)
				{
					text += "/GAF/";
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
				}
				else
				{
					text = directories[directories.Length - 1] + "/";
				}
				int num = text.IndexOf("Assets");
				GAFSystemEditor._path = text.Substring(num, text.Length - num);
			}
			return GAFSystemEditor._path;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002C64 File Offset: 0x00000E64
		public static string getCachePath()
		{
			string text = GAFSystemEditor.getPluginPath();
			text += "Resources/";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			text += "Cache/";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			int num = text.IndexOf("Assets");
			return text.Substring(num, text.Length - num);
		}

		// Token: 0x04000001 RID: 1
		private static string _path;
	}
}
