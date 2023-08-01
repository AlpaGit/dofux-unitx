using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x0200005A RID: 90
	internal class GAFComplexObjectImpl_Pro : GAFFilteredObjectImpl_Pro, IGAFMaskedInternal
	{
		// Token: 0x06000253 RID: 595 RVA: 0x0000BBCB File Offset: 0x00009DCB
		public GAFComplexObjectImpl_Pro(GAFObjectInternal _ThisObject, GAFSortingManager _SortingManager, GAFObjectData _Data) : base(_ThisObject, _SortingManager, _Data)
		{
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000BBD6 File Offset: 0x00009DD6
		public override void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			base.updateToState(_State, _Refresh);
			updateMasking(_Refresh);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000BBE7 File Offset: 0x00009DE7
		public override void updateZPosition(float _New)
		{
			var flag = gafTransform.transform.localPosition.z != _New;
			base.updateZPosition(_New);
			if (flag && m_Mask != null)
			{
				m_Mask.update();
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000256 RID: 598 RVA: 0x000094CB File Offset: 0x000076CB
		public uint objectID
		{
			get
			{
				return serializedProperties.objectID;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000BC20 File Offset: 0x00009E20
		public float zPosition
		{
			get
			{
				return thisObject.cachedTransform.localPosition.z;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000BC37 File Offset: 0x00009E37
		public bool isVisible
		{
			get
			{
				return serializedProperties.visible && gafTransform.localVisible && currentState.alpha > 0f;
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000BC68 File Offset: 0x00009E68
		public void enableMasking()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : m_Mask.materials[sharedMaterial.name]);
			updateMasking();
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000BCBC File Offset: 0x00009EBC
		public void updateMasking()
		{
			if (serializedProperties.customMaterial != null && serializedProperties.customMaterial == currentMaterial && currentMaterial.HasProperty("_StencilID"))
			{
				currentMaterial.SetInt("_StencilID", m_Mask.stencilID);
			}
			gafTransform.localStencilValue = m_Mask.stencilID;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000BD38 File Offset: 0x00009F38
		public void disableMasking()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : sharedMaterial);
			gafTransform.localStencilValue = GAFTransform.combineStencil(gafTransform.stencilValue, serializedProperties.clip.settings.stencilValue);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000BDA1 File Offset: 0x00009FA1
		public bool Equals(IGAFMaskedInternal _Other)
		{
			return objectID == _Other.objectID;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000BDB4 File Offset: 0x00009FB4
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

		// Token: 0x0400011A RID: 282
		private IGAFMaskInternal m_Mask;
	}
}
