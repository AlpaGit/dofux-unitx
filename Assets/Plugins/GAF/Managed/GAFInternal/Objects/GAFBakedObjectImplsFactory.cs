using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000058 RID: 88
	internal static class GAFBakedObjectImplsFactory
	{
		// Token: 0x0600024D RID: 589 RVA: 0x0000BB24 File Offset: 0x00009D24
		public static GAFBakedObjectImpl getImpl(GAFObjectData _Data, GAFMeshManager _Manager)
		{
			GAFBakedObjectImpl result = null;
			var type = _Data.type;
			if (type != GAFObjectType.Texture)
			{
				if (type == GAFObjectType.Timeline)
				{
					result = new GAFBakedTimelineObjectImpl(_Data, _Manager);
				}
			}
			else
			{
				result = getImplForTextureType(_Data, _Manager);
			}
			return result;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000BB58 File Offset: 0x00009D58
		public static GAFBakedObjectImpl getImplForTextureType(GAFObjectData _Data, GAFMeshManager _Manager)
		{
			GAFBakedObjectImpl result = null;
			switch (_Data.behaviourType)
			{
			case ObjectBehaviourType.Simple:
			case ObjectBehaviourType.Colored:
				result = new GAFBakedObjectImpl(_Data, _Manager);
				break;
			case ObjectBehaviourType.Mask:
				result = new GAFBakedMaskObjectImpl(_Data, _Manager);
				break;
			case ObjectBehaviourType.Masked:
				result = new GAFBakedMaskedObjectImpl(_Data, _Manager);
				break;
			case ObjectBehaviourType.Filtered:
				result = new GAFBakedFilteredObjectImpl_Pro(_Data, _Manager);
				break;
			case ObjectBehaviourType.Complex:
				result = new GAFBakedComplexObjectImpl_Pro(_Data, _Manager);
				break;
			}
			return result;
		}
	}
}
