using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000054 RID: 84
	internal class GAFBakedMaskedObjectImpl : GAFBakedObjectImpl, IGAFMaskedInternal
	{
		// Token: 0x0600021F RID: 543 RVA: 0x0000A986 File Offset: 0x00008B86
		public GAFBakedMaskedObjectImpl(GAFObjectData _Data, GAFMeshManager _Manager) : base(_Data, _Manager)
		{
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000A990 File Offset: 0x00008B90
		public override void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			base.updateToState(_State, _Refresh);
			updateMasking(_Refresh);
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000221 RID: 545 RVA: 0x000094CB File Offset: 0x000076CB
		public uint objectID
		{
			get
			{
				return serializedProperties.objectID;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000222 RID: 546 RVA: 0x000094D8 File Offset: 0x000076D8
		public float zPosition
		{
			get
			{
				return (float)currentState.zOrder;
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000A9A4 File Offset: 0x00008BA4
		public void enableMasking()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : m_Mask.materials[sharedMaterial.name]);
			updateMasking();
			meshManager.pushSetupRequest();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000AA04 File Offset: 0x00008C04
		public void updateMasking()
		{
			if (serializedProperties.customMaterial != null && serializedProperties.customMaterial == currentMaterial && currentMaterial.HasProperty("_StencilID"))
			{
				currentMaterial.SetInt("_StencilID", m_Mask.stencilID);
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000AA69 File Offset: 0x00008C69
		public void disableMasking()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : sharedMaterial);
			updateMasking();
			meshManager.pushSetupRequest();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000AAA8 File Offset: 0x00008CA8
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

		// Token: 0x0400010E RID: 270
		private IGAFMaskInternal m_Mask;
	}
}
