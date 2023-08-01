using System;
using System.Linq;
using GAF.Managed.GAFInternal.Assets;
using UnityEditor;

namespace GAF.Managed.Editor.Utils
{
	// Token: 0x02000007 RID: 7
	public class GAFMecanimResourcesRelocatorIntenal : IGAFResourcesRelocator
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00003168 File Offset: 0x00001368
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

		// Token: 0x0600000C RID: 12 RVA: 0x0000208D File Offset: 0x0000028D
		protected virtual void relocateAssetResources(GAFAnimationAssetInternal _Asset, string _NewPath)
		{
		}
	}
}
