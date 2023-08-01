using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x0200005D RID: 93
	internal class GAFMaskedObjectImpl : GAFObjectImpl, IGAFMaskedInternal
	{
		// Token: 0x0600027A RID: 634 RVA: 0x0000BE8F File Offset: 0x0000A08F
		public GAFMaskedObjectImpl(GAFObjectInternal _ThisObject, GAFSortingManager _SortingManager, GAFObjectData _Data) : base(_ThisObject, _SortingManager, _Data)
		{
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000D576 File Offset: 0x0000B776
		public override void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			base.updateToState(_State, _Refresh);
			updateMasking(_Refresh);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000D587 File Offset: 0x0000B787
		public override void updateZPosition(float _New)
		{
			var flag = gafTransform.transform.localPosition.z != _New;
			base.updateZPosition(_New);
			if (flag && m_Mask != null)
			{
				m_Mask.update();
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600027D RID: 637 RVA: 0x000094CB File Offset: 0x000076CB
		public uint objectID
		{
			get
			{
				return serializedProperties.objectID;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000BC20 File Offset: 0x00009E20
		public float zPosition
		{
			get
			{
				return thisObject.cachedTransform.localPosition.z;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000BC37 File Offset: 0x00009E37
		public bool isVisible
		{
			get
			{
				return serializedProperties.visible && gafTransform.localVisible && currentState.alpha > 0f;
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000D5C0 File Offset: 0x0000B7C0
		public void enableMasking()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : m_Mask.materials[sharedMaterial.name]);
			updateMasking();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000D614 File Offset: 0x0000B814
		public void updateMasking()
		{
			if (serializedProperties.customMaterial != null && serializedProperties.customMaterial == currentMaterial && currentMaterial.HasProperty("_StencilID"))
			{
				currentMaterial.SetInt("_StencilID", m_Mask.stencilID);
			}
			gafTransform.localStencilValue = m_Mask.stencilID;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000D690 File Offset: 0x0000B890
		public void disableMasking()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : sharedMaterial);
			gafTransform.localStencilValue = GAFTransform.combineStencil(gafTransform.stencilValue, serializedProperties.clip.settings.stencilValue);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000D6F9 File Offset: 0x0000B8F9
		public bool Equals(IGAFMaskedInternal _Other)
		{
			return objectID == _Other.objectID;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000D70C File Offset: 0x0000B90C
		private void updateMasking(bool _Refresh)
		{
			if (_Refresh || currentState.maskID != previousState.maskID)
			{
				if (previousState.maskID >= 0)
				{
					m_Mask.unregisterMaskedObject(this);
					m_Mask = null;
				}
				if (currentState.maskID >= 0)
				{
					m_Mask = GAFStencilMaskManager.getMask(serializedProperties.clip.GetInstanceID(), (uint)currentState.maskID);
					m_Mask.registerMaskedObject(this);
				}
			}
			if ((_Refresh || currentState.alpha != previousState.alpha) && m_Mask != null && (currentState.alpha <= 0f || previousState.alpha <= 0f))
			{
				m_Mask.update();
			}
		}

		// Token: 0x0400012F RID: 303
		private IGAFMaskInternal m_Mask;
	}
}
