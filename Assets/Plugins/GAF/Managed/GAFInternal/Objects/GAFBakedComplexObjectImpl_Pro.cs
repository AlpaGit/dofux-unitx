using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000051 RID: 81
	internal class GAFBakedComplexObjectImpl_Pro : GAFBakedFilteredObjectImpl_Pro, IGAFMaskedInternal
	{
		// Token: 0x060001FC RID: 508 RVA: 0x000094B0 File Offset: 0x000076B0
		public GAFBakedComplexObjectImpl_Pro(GAFObjectData _Data, GAFMeshManager _Manager) : base(_Data, _Manager)
		{
		}

		// Token: 0x060001FD RID: 509 RVA: 0x000094BA File Offset: 0x000076BA
		public override void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			base.updateToState(_State, _Refresh);
			updateMasking(_Refresh);
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001FE RID: 510 RVA: 0x000094CB File Offset: 0x000076CB
		public uint objectID
		{
			get
			{
				return serializedProperties.objectID;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001FF RID: 511 RVA: 0x000094D8 File Offset: 0x000076D8
		public float zPosition
		{
			get
			{
				return (float)currentState.zOrder;
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000094E8 File Offset: 0x000076E8
		public void enableMasking()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : m_Mask.materials[sharedMaterial.name]);
			updateMasking();
			meshManager.pushSetupRequest();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009548 File Offset: 0x00007748
		public void updateMasking()
		{
			if (serializedProperties.customMaterial != null && serializedProperties.customMaterial == currentMaterial && currentMaterial.HasProperty("_StencilID"))
			{
				currentMaterial.SetInt("_StencilID", m_Mask.stencilID);
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000095AD File Offset: 0x000077AD
		public void disableMasking()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : sharedMaterial);
			updateMasking();
			meshManager.pushSetupRequest();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000095EC File Offset: 0x000077EC
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
		}

		// Token: 0x040000FC RID: 252
		private IGAFMaskInternal m_Mask;
	}
}
