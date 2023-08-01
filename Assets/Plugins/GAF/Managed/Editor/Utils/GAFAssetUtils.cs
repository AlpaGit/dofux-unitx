using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GAF.Managed.Editor.Utils
{
	// Token: 0x02000003 RID: 3
	public static class GAFAssetUtils
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002CCC File Offset: 0x00000ECC
		public static T saveAsset<T>(T _Asset, string _Path) where T : Object
		{
			T result = default(T);
			if (object.Equals(_Asset, null))
			{
				result = default(T);
			}
			else
			{
				T t = AssetDatabase.LoadAssetAtPath(_Path, typeof(T)) as T;
				if (object.Equals(t, null))
				{
					AssetDatabase.CreateAsset(_Asset, _Path);
					result = (AssetDatabase.LoadAssetAtPath(_Path, typeof(T)) as T);
				}
				else
				{
					EditorUtility.CopySerialized(_Asset, t);
					EditorUtility.SetDirty(t);
					AssetDatabase.SaveAssets();
					result = t;
				}
			}
			return result;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002D70 File Offset: 0x00000F70
		public static List<T> findAssetsAtPath<T>(string _Path, string _Pattern, SearchOption _Option) where T : Object
		{
			string str = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length);
			List<T> list = new List<T>();
			foreach (string text in Directory.GetFiles(str + _Path, _Pattern, _Option))
			{
				T t = (T)((object)AssetDatabase.LoadAssetAtPath("Assets" + text.Remove(0, Application.dataPath.Length), typeof(T)));
				if (t != null)
				{
					list.Add(t);
				}
			}
			return list;
		}
	}
}
