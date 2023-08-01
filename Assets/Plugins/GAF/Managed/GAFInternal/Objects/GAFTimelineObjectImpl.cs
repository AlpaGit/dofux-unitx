using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000062 RID: 98
	internal class GAFTimelineObjectImpl : GAFObjectImpl, IGAFMaskedInternal
	{
		// Token: 0x060002BD RID: 701 RVA: 0x0000F2D4 File Offset: 0x0000D4D4
		public GAFTimelineObjectImpl(GAFObjectInternal _ThisObject, GAFSortingManager _SortingManager, GAFObjectData _Data) : base(_ThisObject, _SortingManager, _Data)
		{
			meshRenderer.enabled = false;
			m_LocalColorData = new GAFPair<Color, Vector4>();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000F2F8 File Offset: 0x0000D4F8
		public override void initialize()
		{
			var clip = serializedProperties.clip;
			var type = clip.GetType();
			var text = serializedProperties.name + "_clip";
			var transform = thisObject.cachedTransform.Find(text);
			if (transform == null)
			{
				var gameObject = new GameObject
				{
					name = text
				};
				gameObject.transform.parent = thisObject.cachedTransform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				m_NestedClip = (gameObject.AddComponent(type) as GAFBaseClip);
				m_NestedClip.initialize(clip.asset, (int)serializedProperties.atlasElementID);
				m_NestedClip.settings.hasIndividualMaterial = true;
				m_NestedClip.settings.ignoreTimeScale = clip.settings.ignoreTimeScale;
				m_NestedClip.settings.perfectTiming = clip.settings.perfectTiming;
				m_NestedClip.settings.pivotOffset = clip.settings.pivotOffset;
				m_NestedClip.settings.pixelsPerUnit = clip.settings.pixelsPerUnit;
				m_NestedClip.settings.playAutomatically = clip.settings.playAutomatically;
				m_NestedClip.settings.playInBackground = clip.settings.playInBackground;
				m_NestedClip.settings.scale = clip.settings.scale;
				m_NestedClip.settings.spriteLayerID = clip.settings.spriteLayerID;
				m_NestedClip.settings.spriteLayerName = clip.settings.spriteLayerName;
				m_NestedClip.settings.spriteLayerValue = clip.settings.spriteLayerValue;
				m_NestedClip.settings.stencilValue = clip.settings.stencilValue;
				m_NestedClip.settings.targetFPS = clip.settings.targetFPS;
				m_NestedClip.settings.wrapMode = clip.settings.wrapMode;
				m_NestedClip.settings.zLayerScale = clip.settings.zLayerScale;
				m_NestedClip.parent = gafTransform;
			}
			else
			{
				var gameObject2 = transform.gameObject;
				m_NestedClip = (gameObject2.GetComponent(type) as GAFBaseClip);
			}
			m_NestedClip.settings.hasIndividualMaterial = true;
			var component = m_NestedClip.GetComponent<GAFTransform>();
			component.gafParent = gafTransform;
			gafTransform.gafChilds[component.GetInstanceID()] = component;
			sortingManager.registerSubObject(serializedProperties.objectID, this);
			thisObject.gafTransform.localVisible &= serializedProperties.visible;
			m_NestedClip.reload();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000F604 File Offset: 0x0000D804
		public override void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			previousState = ((currentState == null) ? new GAFObjectStateData(serializedProperties.objectID) : currentState);
			currentState = _State;
			if (currentState.alpha > 0f)
			{
				updateColor(_Refresh);
				updateSorting(_Refresh);
				updateGeometry(_Refresh);
				updateMasking(_Refresh);
				updateVisiblity(_Refresh);
				return;
			}
			currentState = GAFObjectStateData.defaultState;
			gafTransform.localVisible = false;
			gafTransform.realVisibility = false;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000F697 File Offset: 0x0000D897
		public override void cleanUp()
		{
			sortingManager.unregisterSubObject(serializedProperties.objectID);
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x000094CB File Offset: 0x000076CB
		public uint objectID
		{
			get
			{
				return serializedProperties.objectID;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000BC20 File Offset: 0x00009E20
		public float zPosition
		{
			get
			{
				return thisObject.cachedTransform.localPosition.z;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000B536 File Offset: 0x00009736
		public bool isVisible
		{
			get
			{
				return serializedProperties.visible && currentState.alpha > 0f;
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000F6AF File Offset: 0x0000D8AF
		public void enableMasking()
		{
			updateMasking();
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000F6B7 File Offset: 0x0000D8B7
		public void updateMasking()
		{
			gafTransform.localStencilValue = m_Mask.stencilID;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000F6CF File Offset: 0x0000D8CF
		public void disableMasking()
		{
			gafTransform.localStencilValue = GAFTransform.combineStencil(gafTransform.stencilValue, serializedProperties.clip.settings.stencilValue);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000F704 File Offset: 0x0000D904
		private void updateVisiblity(bool _Refresh)
		{
			if ((_Refresh || currentState.alpha != previousState.alpha) && (currentState.alpha <= 0f || previousState.alpha <= 0f))
			{
				sortingManager.pushSortRequest();
				gafTransform.realVisibility = (currentState.alpha > 0f);
				gafTransform.localVisible = (currentState.alpha > 0f && serializedProperties.visible);
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000F7A4 File Offset: 0x0000D9A4
		private void updateColor(bool _Refresh)
		{
			if (_Refresh || currentState.alpha != previousState.alpha)
			{
				var localColorData = gafTransform.localColorData;
				var key = localColorData.Key;
				key.a = currentState.alpha;
				m_LocalColorData.Key = key;
				m_LocalColorData.Value = localColorData.Value;
				gafTransform.localColorData = m_LocalColorData;
			}
			if (_Refresh || currentState.colorTransformData != previousState.colorTransformData)
			{
				if (currentState.colorTransformData != null)
				{
					m_LocalColorData.Key = currentState.colorTransformData.multipliers;
					m_LocalColorData.Value = currentState.colorTransformData.offsets;
					gafTransform.localColorData = m_LocalColorData;
					return;
				}
				m_LocalColorData.Key = new Color(1f, 1f, 1f, gafTransform.localColorData.Key.a);
				m_LocalColorData.Value = Vector4.zero;
				gafTransform.localColorData = m_LocalColorData;
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000F8EC File Offset: 0x0000DAEC
		private void updateSorting(bool _Refresh)
		{
			if (_Refresh || currentState.zOrder != previousState.zOrder)
			{
				sortingManager.pushSortRequest();
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000F914 File Offset: 0x0000DB14
		private void updateGeometry(bool _Refresh)
		{
			if (_Refresh || !Equals(currentState, previousState))
			{
				var clip = serializedProperties.clip;
				var pivotOffset = clip.settings.pivotOffset;
				var num = clip.settings.pixelsPerUnit / clip.settings.scale;
				m_LocalPosition.x = currentState.localPosition.x / num + serializedProperties.offset.x + pivotOffset.x;
				m_LocalPosition.y = -currentState.localPosition.y / num + serializedProperties.offset.y + pivotOffset.y;
				m_LocalPosition.z = thisObject.cachedTransform.localPosition.z;
				thisObject.cachedTransform.localPosition = m_LocalPosition;
				if (!clip.settings.decomposeFlashTransform || currentState.useForceGeometry || (gafTransform.gafParent != null && gafTransform.gafParent.matrix != m_IdentityMatrix))
				{
					updateGeometryTransform();
					return;
				}
				updateDecomposedTransform();
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000FA64 File Offset: 0x0000DC64
		private void updateGeometryTransform()
		{
			thisObject.cachedTransform.localRotation = m_IdentityQuaternion;
			thisObject.cachedTransform.localScale = m_OneVector;
			m_Matrix[0, 0] = currentState.a;
			m_Matrix[0, 1] = currentState.c;
			m_Matrix[1, 0] = currentState.b;
			m_Matrix[1, 1] = currentState.d;
			gafTransform.localMatrix = m_Matrix;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000FB10 File Offset: 0x0000DD10
		private void updateDecomposedTransform()
		{
			if (gafTransform.localMatrix != m_IdentityMatrix)
			{
				gafTransform.localMatrix = m_IdentityMatrix;
			}
			thisObject.cachedTransform.localRotation = currentState.rotation;
			thisObject.cachedTransform.localScale = currentState.scale;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000FB7C File Offset: 0x0000DD7C
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

		// Token: 0x060002CE RID: 718 RVA: 0x0000FC08 File Offset: 0x0000DE08
		public override void updateZPosition(float _New)
		{
			var flag = gafTransform.transform.localPosition.z != _New && currentState.alpha != previousState.alpha;
			base.updateZPosition(_New);
			if (flag && m_Mask != null && (currentState.alpha <= 0f || previousState.alpha <= 0f))
			{
				m_Mask.update();
			}
		}

		// Token: 0x04000143 RID: 323
		private GAFBaseClip m_NestedClip;

		// Token: 0x04000144 RID: 324
		private IGAFMaskInternal m_Mask;
	}
}
