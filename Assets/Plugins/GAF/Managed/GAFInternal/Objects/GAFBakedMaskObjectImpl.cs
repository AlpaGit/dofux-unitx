using System.Collections.Generic;
using System.Linq;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000055 RID: 85
	internal class GAFBakedMaskObjectImpl : GAFBakedObjectImpl, IGAFMaskInternal, IGAFMaskedInternal
	{
		// Token: 0x06000227 RID: 551 RVA: 0x0000AB31 File Offset: 0x00008D31
		public GAFBakedMaskObjectImpl(GAFObjectData _Data, GAFMeshManager _Manager) : base(_Data, _Manager)
		{
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000AB54 File Offset: 0x00008D54
		public override void initialize()
		{
			base.initialize();
			GAFStencilMaskManager.registerMask(serializedProperties.clip.GetInstanceID(), serializedProperties.objectID, this);
			var clip = serializedProperties.clip;
			var gafTransform = clip.gafTransform;
			stencilID = GAFTransform.combineStencil(gafTransform.stencilValue, clip.settings.stencilValue + 1);
			var num = GAFTransform.combineStencil(gafTransform.stencilValue, clip.settings.stencilValue);
			GAFTransform.combineColor(gafTransform.colorMultiplier, clip.settings.animationColorMultiplier);
			GAFTransform.combineColorOffset(gafTransform.colorOffset, clip.settings.animationColorOffset, clip.settings.animationColorMultiplier);
			for (var i = 0; i < clip.resource.data.Count; i++)
			{
				var name = clip.resource.data[i].sharedMaterial.name;
				if (!materials.ContainsKey(name))
				{
					var material = new Material(Shader.Find("GAF/GAFObjectsGroup"));
					material.name = clip.resource.data[i].sharedMaterial.name;
					material.mainTexture = clip.resource.data[i].sharedMaterial.mainTexture;
					material.renderQueue = 3000;
					material.SetInt("_StencilID", stencilID);
					if (clip.settings.hasIndividualMaterial)
					{
						material.SetInt("_StencilID", num);
					}
					materials[material.name] = material;
					serializedProperties.clip.registerExternalMaterial(material);
				}
			}
			if (m_MaskMaterial == null)
			{
				m_MaskMaterial = new Material(Shader.Find("GAF/GAFMask"));
			}
			m_MaskMaterial.mainTexture = sharedTexture;
			m_MaskMaterial.renderQueue = 3000;
			if (m_MaskResetMaterial == null)
			{
				m_MaskResetMaterial = new Material(Shader.Find("GAF/GAFMaskReset"));
			}
			m_MaskResetMaterial.hideFlags   = HideFlags.HideInHierarchy;
			m_MaskResetMaterial.mainTexture = sharedTexture;
			m_MaskResetMaterial.renderQueue = 3000;
			if (clip.settings.hasIndividualMaterial)
			{
				m_MaskMaterial.SetInt(StencilID, num);
				m_MaskResetMaterial.SetInt(StencilID, num);
			}
			else
			{
				m_MaskMaterial.SetInt(StencilID, sharedMaterial.GetInt(StencilID));
				m_MaskResetMaterial.SetInt(StencilID, sharedMaterial.GetInt(StencilID));
			}
			serializedProperties.clip.registerExternalMaterial(m_MaskMaterial);
			serializedProperties.clip.registerExternalMaterial(m_MaskResetMaterial);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000AE3D File Offset: 0x0000903D
		public override void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			base.updateToState(_State, _Refresh);
			updateMasking(_Refresh);
			updateMask(_Refresh);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000AE58 File Offset: 0x00009058
		public override void cleanUp()
		{
			base.cleanUp();
			m_MaskMaterial = null;
			m_MaskResetMaterial = null;
			deleteUnityObject(m_MaskMaterial);
			deleteUnityObject(m_MaskResetMaterial);
			foreach (var keyValuePair in m_Materials)
			{
				deleteUnityObject(keyValuePair.Value);
			}
			m_Materials.Clear();
			m_MaskedObjects.Clear();
			GAFStencilMaskManager.unregisterMask(serializedProperties.clip.GetInstanceID(), serializedProperties.objectID);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000AF14 File Offset: 0x00009114
		public override void combineMeshes(ref GAFMeshManager.MeshData _MeshData)
		{
			_MeshData.restObjects.Dequeue();
			_MeshData.pushData(this, currentMaterial);
			var num = m_MaskedObjects.Count((KeyValuePair<uint, IGAFMaskedInternal> _masked) => _masked.Value.isVisible);
			for (var i = 0; i < num; i++)
			{
				_MeshData.restObjects.Peek().combineMeshes(ref _MeshData);
			}
			m_MaskResetMaterial.SetInt("_StencilID", stencilID);
			_MeshData.pushData(this, m_MaskResetMaterial);
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000AFA9 File Offset: 0x000091A9
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000AFB1 File Offset: 0x000091B1
		public int stencilID { get; private set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000AFBA File Offset: 0x000091BA
		public Dictionary<string, Material> materials
		{
			get
			{
				return m_Materials;
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00006E9C File Offset: 0x0000509C
		public void update()
		{
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000AFC2 File Offset: 0x000091C2
		public void registerMaskedObject(IGAFMaskedInternal _Masked)
		{
			if (!m_MaskedObjects.ContainsKey(_Masked.objectID))
			{
				m_MaskedObjects.Add(_Masked.objectID, _Masked);
			}
			if (isVisible)
			{
				_Masked.enableMasking();
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000AFF7 File Offset: 0x000091F7
		public void unregisterMaskedObject(IGAFMaskedInternal _Masked)
		{
			m_MaskedObjects.Remove(_Masked.objectID);
			_Masked.disableMasking();
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000232 RID: 562 RVA: 0x000094CB File Offset: 0x000076CB
		public uint objectID
		{
			get
			{
				return serializedProperties.objectID;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000233 RID: 563 RVA: 0x000094D8 File Offset: 0x000076D8
		public float zPosition
		{
			get
			{
				return (float)currentState.zOrder;
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000B011 File Offset: 0x00009211
		public void enableMasking()
		{
			stencilID = m_Mask.stencilID + 1;
			updateMasking();
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000B02C File Offset: 0x0000922C
		public void updateMasking()
		{
			if (currentMaterial.HasProperty("_StencilID"))
			{
				currentMaterial.SetInt("_StencilID", m_Mask.stencilID - 1);
			}
			m_MaskResetMaterial.SetInt("_StencilID", stencilID);
			foreach (var keyValuePair in materials)
			{
				keyValuePair.Value.SetInt("_StencilID", stencilID);
			}
			foreach (var keyValuePair2 in m_MaskedObjects)
			{
				keyValuePair2.Value.updateMasking();
			}
			meshManager.pushSetupRequest();
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000B128 File Offset: 0x00009328
		public void disableMasking()
		{
			var gafTransform = serializedProperties.clip.gafTransform;
			stencilID = GAFTransform.combineStencil(gafTransform.stencilValue, serializedProperties.clip.settings.stencilValue + 1);
			updateMasking();
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000B174 File Offset: 0x00009374
		private void updateMask(bool _Refresh)
		{
			if (currentState.alpha != previousState.alpha)
			{
				if (currentState.alpha <= 0f || !isVisible)
				{
					disableMask();
					return;
				}
				enableMask();
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000B1C0 File Offset: 0x000093C0
		private void enableMask()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : m_MaskMaterial);
			foreach (var keyValuePair in m_MaskedObjects)
			{
				keyValuePair.Value.enableMasking();
			}
			meshManager.pushSortRequest();
			meshManager.pushSetupRequest();
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000B25C File Offset: 0x0000945C
		private void disableMask()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : sharedMaterial);
			foreach (var keyValuePair in m_MaskedObjects)
			{
				keyValuePair.Value.disableMasking();
			}
			meshManager.pushSortRequest();
			meshManager.pushSetupRequest();
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000B2F8 File Offset: 0x000094F8
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
			if ((_Refresh || currentState.zOrder != previousState.zOrder) && m_Mask != null && isVisible)
			{
				m_Mask.update();
			}
		}

		// Token: 0x0400010F RID: 271
		private IGAFMaskInternal m_Mask;

		// Token: 0x04000110 RID: 272
		private Material m_MaskMaterial;

		// Token: 0x04000111 RID: 273
		private Material m_MaskResetMaterial;

		// Token: 0x04000112 RID: 274
		private Dictionary<uint, IGAFMaskedInternal> m_MaskedObjects = new Dictionary<uint, IGAFMaskedInternal>();

		// Token: 0x04000113 RID: 275
		private Dictionary<string, Material> m_Materials = new Dictionary<string, Material>();
		private static readonly int StencilID = Shader.PropertyToID("_StencilID");
	}
}
