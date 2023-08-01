using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000061 RID: 97
	internal static class GAFObjectImplsFactory
	{
		// Token: 0x060002BB RID: 699 RVA: 0x0000F230 File Offset: 0x0000D430
		public static GAFObjectImpl getImpl(GAFObjectInternal _Object, GAFSortingManager _Manager, GAFObjectData _Data)
		{
			GAFObjectImpl result = null;
			var type = _Data.type;
			if (type != GAFObjectType.Texture)
			{
				if (type == GAFObjectType.Timeline)
				{
					result = new GAFTimelineObjectImpl(_Object, _Manager, _Data);
				}
			}
			else
			{
				result = getImplForTextureType(_Object, _Manager, _Data);
			}
			return result;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000F264 File Offset: 0x0000D464
		public static GAFObjectImpl getImplForTextureType(GAFObjectInternal _Object, GAFSortingManager _Manager, GAFObjectData _Data)
		{
			GAFObjectImpl result = null;
			switch (_Data.behaviourType)
			{
			case ObjectBehaviourType.Simple:
				result = new GAFObjectImpl(_Object, _Manager, _Data);
				break;
			case ObjectBehaviourType.Mask:
				result = new GAFMaskObjectImpl(_Object, _Manager, _Data);
				break;
			case ObjectBehaviourType.Masked:
				result = new GAFMaskedObjectImpl(_Object, _Manager, _Data);
				break;
			case ObjectBehaviourType.Filtered:
				result = new GAFFilteredObjectImpl_Pro(_Object, _Manager, _Data);
				break;
			case ObjectBehaviourType.Complex:
				result = new GAFComplexObjectImpl_Pro(_Object, _Manager, _Data);
				break;
			}
			return result;
		}
	}
}
