using System.Collections.Generic;
using System.Linq;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x0200005E RID: 94
	internal class GAFMaskObjectImpl : GAFObjectImpl, IGAFMaskInternal, IGAFMaskedInternal
	{
		// Token: 0x06000285 RID: 645 RVA: 0x0000D7E7 File Offset: 0x0000B9E7
		public GAFMaskObjectImpl(GAFObjectInternal _ThisObject, GAFSortingManager _SortingManager, GAFObjectData _Data) : base(_ThisObject, _SortingManager, _Data)
		{
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000D808 File Offset: 0x0000BA08
		public override void initialize()
		{
			base.initialize();
			GAFStencilMaskManager.registerMask(serializedProperties.clip.GetInstanceID(), serializedProperties.objectID, this);
			var clip = serializedProperties.clip;
			stencilID = GAFTransform.combineStencil(gafTransform.stencilValue, clip.settings.stencilValue + 1);
			var num = GAFTransform.combineStencil(gafTransform.stencilValue, clip.settings.stencilValue);
			for (var i = 0; i < serializedProperties.clip.resource.data.Count; i++)
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
						material.SetInt("_StencilID", stencilID);
					}
					else
					{
						material.SetInt("_StencilID", stencilID);
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
			initMaskReseter();
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000DAE2 File Offset: 0x0000BCE2
		public override void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			base.updateToState(_State, _Refresh);
			updateMasking(_Refresh);
			updateMask(_Refresh);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000DAFC File Offset: 0x0000BCFC
		public override void cleanUp()
		{
			base.cleanUp();
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

		// Token: 0x06000289 RID: 649 RVA: 0x0000DBAC File Offset: 0x0000BDAC
		public override void updateZPosition(float _New)
		{
			var flag = gafTransform.transform.localPosition.z != _New;
			base.updateZPosition(_New);
			if (flag && m_Mask != null)
			{
				m_Mask.update();
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000DBE5 File Offset: 0x0000BDE5
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000DBED File Offset: 0x0000BDED
		public int stencilID { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000DBF6 File Offset: 0x0000BDF6
		public Dictionary<string, Material> materials
		{
			get
			{
				return m_Materials;
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000DBFE File Offset: 0x0000BDFE
		public void update()
		{
			updateMaskReseter();
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000DC06 File Offset: 0x0000BE06
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
			updateMaskReseter();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000DC41 File Offset: 0x0000BE41
		public void unregisterMaskedObject(IGAFMaskedInternal _Masked)
		{
			m_MaskedObjects.Remove(_Masked.objectID);
			_Masked.disableMasking();
			updateMaskReseter();
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000290 RID: 656 RVA: 0x000094CB File Offset: 0x000076CB
		public uint objectID
		{
			get
			{
				return serializedProperties.objectID;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000BC20 File Offset: 0x00009E20
		public float zPosition
		{
			get
			{
				return thisObject.cachedTransform.localPosition.z;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000BC37 File Offset: 0x00009E37
		public bool isVisible
		{
			get
			{
				return serializedProperties.visible && gafTransform.localVisible && currentState.alpha > 0f;
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000DC61 File Offset: 0x0000BE61
		public void enableMasking()
		{
			stencilID = m_Mask.stencilID + 1;
			updateMasking();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000DC7C File Offset: 0x0000BE7C
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
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000DD6C File Offset: 0x0000BF6C
		public void disableMasking()
		{
			stencilID = GAFTransform.combineStencil(gafTransform.stencilValue, serializedProperties.clip.settings.stencilValue + 1);
			updateMasking();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000DDA4 File Offset: 0x0000BFA4
		private void updateMask(bool _Refresh)
		{
			if (currentState.alpha != previousState.alpha)
			{
				if (currentState.alpha <= 0f || !isVisible)
				{
					disableMask();
				}
				else
				{
					enableMask();
				}
			}
			if (m_MaskReseter != null)
			{
				m_MaskReseter.transform.localPosition = new Vector3(thisObject.cachedTransform.localPosition.x, thisObject.cachedTransform.localPosition.y, m_MaskReseter.transform.localPosition.z);
				m_MaskReseter.transform.localRotation = thisObject.cachedTransform.localRotation;
				m_MaskReseter.transform.localScale = thisObject.cachedTransform.localScale;
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000DE98 File Offset: 0x0000C098
		private void enableMask()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : m_MaskMaterial);
			foreach (var keyValuePair in m_MaskedObjects)
			{
				keyValuePair.Value.enableMasking();
			}
			m_MaskReseter.SetActive(true);
			m_ReseterFilter.sharedMesh = currentMesh;
			m_ReseterRenderer.sharedMaterial = m_MaskResetMaterial;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000DF4C File Offset: 0x0000C14C
		private void disableMask()
		{
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : sharedMaterial);
			foreach (var keyValuePair in m_MaskedObjects)
			{
				keyValuePair.Value.disableMasking();
			}
			m_MaskReseter.SetActive(false);
			m_ReseterFilter.sharedMesh = null;
			m_ReseterRenderer.sharedMaterial = null;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000DFF4 File Offset: 0x0000C1F4
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
			if ((_Refresh || currentState.zOrder != previousState.zOrder) && m_Mask != null)
			{
				m_Mask.update();
			}
			if ((_Refresh || currentState.alpha != previousState.alpha) && m_Mask != null && (currentState.alpha <= 0f || previousState.alpha <= 0f))
			{
				m_Mask.update();
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000E100 File Offset: 0x0000C300
		private void initMaskReseter()
		{
			if (m_MaskReseter == null)
			{
				var transform = thisObject.cachedTransform.parent.Find(serializedProperties.name + "_reseter");
				if (transform != null)
				{
					m_MaskReseter           = transform.gameObject;
					m_MaskReseter.hideFlags = HideFlags.HideInHierarchy;
					m_ReseterFilter         = m_MaskReseter.GetComponent<MeshFilter>();
					m_ReseterRenderer       = m_MaskReseter.GetComponent<MeshRenderer>();
				}
				else
				{
					m_MaskReseter                         = new GameObject(serializedProperties.name + "_reseter");
					m_MaskReseter.transform.parent        = thisObject.cachedTransform.parent;
					m_MaskReseter.transform.localPosition = new Vector3(0f, 0f, 0f);
					m_MaskReseter.hideFlags               = HideFlags.HideInHierarchy;
					m_ReseterFilter                       = m_MaskReseter.AddComponent<MeshFilter>();
					m_ReseterRenderer                     = m_MaskReseter.AddComponent<MeshRenderer>();
				}
				m_MaskReseter.SetActive(false);
				m_ReseterRenderer.shadowCastingMode = 0;
				m_ReseterRenderer.useLightProbes = false;
				m_ReseterRenderer.receiveShadows = false;
				m_ReseterRenderer.sortingLayerName = serializedProperties.clip.settings.spriteLayerName;
				m_ReseterRenderer.sortingOrder = serializedProperties.clip.settings.spriteLayerValue;
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000E294 File Offset: 0x0000C494
		private void updateMaskReseter()
		{
			initMaskReseter();
			var num = zPosition;
			var source = from _masked in m_MaskedObjects
						 where _masked.Value.isVisible
						 select _masked;
			if (source.Any<KeyValuePair<uint, IGAFMaskedInternal>>())
			{
				num = source.Min((KeyValuePair<uint, IGAFMaskedInternal> _masked) => _masked.Value.zPosition);
			}
			var clip = serializedProperties.clip;
			var num2 = clip.settings.pixelsPerUnit / clip.settings.scale;
			var num3 = 0.1f / num2 * clip.settings.zLayerScale;
			m_MaskReseter.transform.localPosition = new Vector3(thisObject.cachedTransform.localPosition.x, thisObject.cachedTransform.localPosition.y, num - num3);
			m_MaskResetMaterial.SetInt("_StencilID", stencilID);
		}

		// Token: 0x04000130 RID: 304
		private IGAFMaskInternal m_Mask;

		// Token: 0x04000131 RID: 305
		private Material m_MaskMaterial;

		// Token: 0x04000132 RID: 306
		private Dictionary<uint, IGAFMaskedInternal> m_MaskedObjects = new Dictionary<uint, IGAFMaskedInternal>();

		// Token: 0x04000133 RID: 307
		private Dictionary<string, Material> m_Materials = new Dictionary<string, Material>();

		// Token: 0x04000134 RID: 308
		private GameObject m_MaskReseter;

		// Token: 0x04000135 RID: 309
		private MeshFilter m_ReseterFilter;

		// Token: 0x04000136 RID: 310
		private MeshRenderer m_ReseterRenderer;

		// Token: 0x04000137 RID: 311
		private Material m_MaskResetMaterial;
		private static readonly int StencilID = Shader.PropertyToID("_StencilID");
	}
}
