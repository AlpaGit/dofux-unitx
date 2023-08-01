using System;
using System.IO;
using System.Linq;
using GAF.Managed.GAFInternal.Assets;
using UnityEditor;
using UnityEngine;

namespace GAF.Managed.Editor.Utils
{
	// Token: 0x02000006 RID: 6
	public class GAFAudioResourcesRelocator : IGAFResourcesRelocator
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00003048 File Offset: 0x00001248
		public void relocate(SerializedObject _Container, string _Path)
		{
			if (_Container.FindProperty("m_GUID") != null)
			{
				Type type = _Container.targetObject.GetType();
				if (type != null && type.IsSubclassOf(typeof(GAFAnimationAssetInternal)))
				{
					foreach (GAFAnimationAssetInternal asset in _Container.targetObjects.Cast<GAFAnimationAssetInternal>().ToList<GAFAnimationAssetInternal>())
					{
						this.relocateAudioResources(asset, _Path);
					}
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000030D4 File Offset: 0x000012D4
		private void relocateAudioResources(GAFAnimationAssetInternal _Asset, string _NewPath)
		{
			foreach (GAFAnimationAssetInternal.GAFSound gafsound in _Asset.audioResources)
			{
				string assetPath = AssetDatabase.GetAssetPath(gafsound.audio);
				string text = "Assets/" + _NewPath + Path.GetFileName(assetPath);
				if (AssetDatabase.LoadAssetAtPath(text, typeof(AudioClip)) as AudioClip != null)
				{
					AssetDatabase.DeleteAsset(text);
				}
				AssetDatabase.MoveAsset(assetPath, text);
			}
		}
	}
}
