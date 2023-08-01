using System;
using System.IO;
using System.Linq;
using GAF.Managed.GAFInternal.Assets;
using UnityEditor;

namespace GAF.Managed.Editor.Utils
{
	// Token: 0x02000008 RID: 8
	internal class GAFResourcesRelocator : IGAFResourcesRelocator
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000031F4 File Offset: 0x000013F4
		public void relocate(SerializedObject _Container, string _Path)
		{
			if (_Container.FindProperty("m_GUID") != null)
			{
				Type type = _Container.targetObject.GetType();
				if (type != null && type.IsSubclassOf(typeof(GAFAnimationAssetInternal)))
				{
					foreach (GAFAnimationAssetInternal asset in _Container.targetObjects.Cast<GAFAnimationAssetInternal>().ToList<GAFAnimationAssetInternal>())
					{
						this.relocateAssetResources(asset, _Path);
					}
				}
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00003280 File Offset: 0x00001480
		private void relocateAssetResources(GAFAnimationAssetInternal _Asset, string _NewPath)
		{
			foreach (string str in _Asset.resourcesPaths)
			{
				string text = "Assets/" + str;
				if (AssetDatabase.LoadAssetAtPath(text, typeof(GAFTexturesResourceInternal)) as GAFTexturesResourceInternal != null)
				{
					string text2 = "Assets/" + _NewPath + Path.GetFileName(text);
					if (AssetDatabase.LoadAssetAtPath(text2, typeof(GAFTexturesResourceInternal)) as GAFTexturesResourceInternal != null)
					{
						AssetDatabase.DeleteAsset(text2);
					}
					AssetDatabase.MoveAsset(text, text2);
				}
			}
		}
	}
}
