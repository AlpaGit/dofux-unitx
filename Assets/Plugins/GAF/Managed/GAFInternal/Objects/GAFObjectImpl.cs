using System;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000060 RID: 96
	internal class GAFObjectImpl : IGAFObjectImpl, IComparable<GAFObjectImpl>
	{
		// Token: 0x060002A0 RID: 672 RVA: 0x0000E3B4 File Offset: 0x0000C5B4
		public GAFObjectImpl(GAFObjectInternal _ThisObject, GAFSortingManager _SortingManager, GAFObjectData _Data) : base(_Data)
		{
			thisObject = _ThisObject;
			sortingManager = _SortingManager;
			gafTransform = _ThisObject.gafTransform;
			meshRenderer = _ThisObject.GetComponent<MeshRenderer>();
			meshFilter = _ThisObject.GetComponent<MeshFilter>();
			m_LocalColorData = new GAFPair<Color, Vector4>();
			resetRenderer();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000E418 File Offset: 0x0000C618
		public override void initialize()
		{
			base.initialize();
			thisObject.cachedTransform.localRotation = m_IdentityQuaternion;
			thisObject.cachedTransform.localScale = m_OneVector;
			sortingManager.registerSubObject(serializedProperties.objectID, this);
			thisObject.gafTransform.localVisible &= serializedProperties.visible;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000E490 File Offset: 0x0000C690
		public override void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			previousState = currentState;
			currentState = _State;
			if (currentState.alpha > 0f && serializedProperties.visible)
			{
				updateColor();
				updateSorting();
				updateGeometry();
				updateVisiblity();
				return;
			}
			currentState = GAFObjectStateData.defaultState;
			gafTransform.localVisible = false;
			gafTransform.realVisibility = false;
			for (var i = 0; i < tempColors.Length; i++)
			{
				tempColors[i].r = byte.MaxValue;
				tempColors[i].g = byte.MaxValue;
				tempColors[i].b = byte.MaxValue;
			}
			var array = new Vector4[4];
			for (var j = 0; j < array.Length; j++)
			{
				array[j] = m_ZeroVector;
			}
			currentMesh.colors32 = tempColors;
			setColorOffset(array);
			m_LocalColorData.Key = tempColors[0];
			m_LocalColorData.Value = array[0];
			gafTransform.localColorData = m_LocalColorData;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000E5DC File Offset: 0x0000C7DC
		public override void cleanUp()
		{
			if (sortingManager != null)
			{
				sortingManager.unregisterSubObject(serializedProperties.objectID);
			}
			if (currentMesh != null)
			{
				currentMesh.Clear();
				currentMesh.hideFlags = HideFlags.DontSave;
			}
			if (Application.isEditor)
			{
				Object.DestroyImmediate(meshFilter.sharedMesh, true);
			}
			else
			{
				Object.Destroy(meshFilter.mesh);
			}
			meshFilter = null;
			meshRenderer = null;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000E66C File Offset: 0x0000C86C
		public virtual void updateZPosition(float _New)
		{
			if (_New != gafTransform.transform.localPosition.z)
			{
				var localPosition = gafTransform.transform.localPosition;
				localPosition.z = _New;
				gafTransform.transform.localPosition = localPosition;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000E6BB File Offset: 0x0000C8BB
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0000E6C8 File Offset: 0x0000C8C8
		public override Mesh currentMesh
		{
			get
			{
				return meshFilter.sharedMesh;
			}
			set
			{
				meshFilter.sharedMesh = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000E6D6 File Offset: 0x0000C8D6
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x0000E6E3 File Offset: 0x0000C8E3
		public override Material currentMaterial
		{
			get
			{
				return meshRenderer.sharedMaterial;
			}
			set
			{
				meshRenderer.sharedMaterial = value;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000E6F1 File Offset: 0x0000C8F1
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000E6F9 File Offset: 0x0000C8F9
		public GAFTransform gafTransform { get; private set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000E702 File Offset: 0x0000C902
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000E70A File Offset: 0x0000C90A
		private protected GAFObjectInternal thisObject { get; private set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000E713 File Offset: 0x0000C913
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0000E71B File Offset: 0x0000C91B
		private protected GAFSortingManager sortingManager { get; private set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000E724 File Offset: 0x0000C924
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000E72C File Offset: 0x0000C92C
		private protected MeshRenderer meshRenderer { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000E735 File Offset: 0x0000C935
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000E73D File Offset: 0x0000C93D
		private protected MeshFilter meshFilter { get; private set; }

		// Token: 0x060002B3 RID: 691 RVA: 0x0000E748 File Offset: 0x0000C948
		public int CompareTo(GAFObjectImpl _Other)
		{
			return currentState.zOrder.CompareTo(_Other.currentState.zOrder);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000E774 File Offset: 0x0000C974
		private void updateVisiblity()
		{
			if (currentState.alpha != previousState.alpha && (currentState.alpha <= 0f || previousState.alpha <= 0f))
			{
				sortingManager.pushSortRequest();
				gafTransform.realVisibility = (currentState.alpha > 0f);
				gafTransform.localVisible = (currentState.alpha > 0f && serializedProperties.visible);
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000E810 File Offset: 0x0000CA10
		private void updateColor()
		{
			Vector4[] array = null;
			var animationColorMultiplier = serializedProperties.clip.settings.animationColorMultiplier;
			var animationColorOffset = serializedProperties.clip.settings.animationColorOffset;
			if (currentState.alpha != previousState.alpha || currentState.colorTransformData != previousState.colorTransformData)
			{
				array = getColorOffset();
			}
			if (currentState.alpha != previousState.alpha)
			{
				for (var i = 0; i < tempColors.Length; i++)
				{
					tempColors[i].a = (byte)(currentState.alpha * 255f);
				}
				if (currentMaterial.HasProperty("_TintColor"))
				{
					currentMaterial.SetColor("_TintColor", tempColors[0]);
				}
				GAFTransform.combineColor(gafTransform.colorMultiplier, animationColorMultiplier);
				GAFTransform.combineColorOffset(gafTransform.colorOffset, animationColorOffset, animationColorMultiplier);
				currentMesh.colors32 = tempColors;
				m_LocalColorData.Key = tempColors[0];
				m_LocalColorData.Value = array[0];
				gafTransform.localColorData = m_LocalColorData;
			}
			if (currentState.colorTransformData != previousState.colorTransformData)
			{
				if (currentState.colorTransformData != null)
				{
					for (var j = 0; j < tempColors.Length; j++)
					{
						tempColors[j] = currentState.colorTransformData.multipliers;
					}
					for (var k = 0; k < array.Length; k++)
					{
						array[k] = currentState.colorTransformData.offsets;
					}
				}
				else
				{
					for (var l = 0; l < tempColors.Length; l++)
					{
						tempColors[l].r = byte.MaxValue;
						tempColors[l].g = byte.MaxValue;
						tempColors[l].b = byte.MaxValue;
					}
					for (var m = 0; m < array.Length; m++)
					{
						array[m] = m_ZeroVector;
					}
				}
				if (currentMaterial.HasProperty("_TintColor"))
				{
					currentMaterial.SetColor("_TintColor", tempColors[0]);
				}
				if (currentMaterial.HasProperty("_TintColorOffset"))
				{
					currentMaterial.SetVector("_TintColorOffset", array[0]);
				}
				currentMesh.colors32 = tempColors;
				setColorOffset(array);
				m_LocalColorData.Key = tempColors[0];
				m_LocalColorData.Value = array[0];
				gafTransform.localColorData = m_LocalColorData;
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000EB32 File Offset: 0x0000CD32
		private void updateSorting()
		{
			if (currentState.zOrder != previousState.zOrder)
			{
				sortingManager.pushSortRequest();
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000EB58 File Offset: 0x0000CD58
		private void updateGeometry()
		{
			if (currentState != previousState)
			{
				var clip = serializedProperties.clip;
				if (currentState.useForceGeometry || !clip.settings.decomposeFlashTransform || (gafTransform.gafParent != null && gafTransform.gafParent.matrix != m_IdentityMatrix))
				{
					updateGeometryTransform(clip.asset.hasNesting, clip);
					return;
				}
				updateDecomposedTransform(clip);
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000EBE4 File Offset: 0x0000CDE4
		private void updateGeometryTransform(bool _UpdateMatrix, GAFBaseClip _Clip)
		{
			var pivotOffset = _Clip.settings.pivotOffset;
			var num = _Clip.settings.pixelsPerUnit / _Clip.settings.scale;
			var num2 = currentState.localPosition.x / num;
			var num3 = -currentState.localPosition.y / num;
			if (_Clip.settings.flipX)
			{
				serializedProperties.flip = new Vector2(-num2 * 2f, 0f);
				m_Matrix[0, 0] = currentState.a;
				m_Matrix[0, 1] = -currentState.c;
				m_Matrix[1, 0] = -currentState.b;
				m_Matrix[1, 1] = currentState.d;
				num2 = num2 - serializedProperties.offset.x - pivotOffset.x + serializedProperties.flip.x;
			}
			else
			{
				serializedProperties.flip = Vector2.zero;
				m_Matrix[0, 0] = currentState.a;
				m_Matrix[0, 1] = currentState.c;
				m_Matrix[1, 0] = currentState.b;
				m_Matrix[1, 1] = currentState.d;
				num2 = num2 + serializedProperties.offset.x + pivotOffset.x + serializedProperties.flip.x;
			}
			num3 = num3 + serializedProperties.offset.y + pivotOffset.y + serializedProperties.flip.y;
			if (_UpdateMatrix)
			{
				var matrix = gafTransform.gafParent.matrix;
				m_LocalPosition.x = num2 * matrix[0, 0] + num3 * matrix[0, 1];
				m_LocalPosition.y = num3 * matrix[1, 1] + num2 * matrix[1, 0];
				m_LocalPosition.z = thisObject.cachedTransform.localPosition.z;
				thisObject.cachedTransform.localPosition = m_LocalPosition;
				gafTransform.localMatrix = m_Matrix;
				m_Matrix = gafTransform.matrix;
			}
			else
			{
				m_LocalPosition.x = num2;
				m_LocalPosition.y = num3;
				m_LocalPosition.z = thisObject.cachedTransform.localPosition.z;
				thisObject.cachedTransform.localPosition = m_LocalPosition;
			}
			thisObject.cachedTransform.localRotation = m_IdentityQuaternion;
			thisObject.cachedTransform.localScale = m_OneVector;
			if (_Clip.settings.cacheStates)
			{
				tempVertices = currentState.vertices;
			}
			else
			{
				for (var i = 0; i < initialVertices.Length; i++)
				{
					tempVertices[i] = m_Matrix.MultiplyPoint3x4(initialVertices[i]);
				}
			}
			currentMesh.vertices = tempVertices;
			currentMesh.RecalculateBounds();
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000EF6C File Offset: 0x0000D16C
		private void updateDecomposedTransform(GAFBaseClip _Clip)
		{
			var pivotOffset = _Clip.settings.pivotOffset;
			var num = _Clip.settings.pixelsPerUnit / _Clip.settings.scale;
			var num2 = currentState.localPosition.x / num;
			var y = -currentState.localPosition.y / num;
			if (_Clip.settings.flipX)
			{
				serializedProperties.flip = new Vector2(-num2 * 2f, 0f);
				num2 = num2 - serializedProperties.offset.x - pivotOffset.x + serializedProperties.flip.x;
			}
			else
			{
				serializedProperties.flip = Vector2.zero;
				num2 = num2 + serializedProperties.offset.x + pivotOffset.x + serializedProperties.flip.x;
			}
			m_LocalPosition.x = num2;
			m_LocalPosition.y = y;
			m_LocalPosition.z = thisObject.cachedTransform.localPosition.z;
			thisObject.cachedTransform.localPosition = m_LocalPosition;
			if (gafTransform.localMatrix != m_IdentityMatrix || tempVertices != initialVertices)
			{
				gafTransform.localMatrix = m_IdentityMatrix;
				tempVertices[0] = initialVertices[0];
				tempVertices[1] = initialVertices[1];
				tempVertices[2] = initialVertices[2];
				tempVertices[3] = initialVertices[3];
				currentMesh.vertices = tempVertices;
				currentMesh.RecalculateBounds();
			}
			thisObject.cachedTransform.localRotation = (_Clip.settings.flipX ? Quaternion.Inverse(currentState.rotation) : currentState.rotation);
			thisObject.cachedTransform.localScale = currentState.scale;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
		protected virtual void resetRenderer()
		{
			var clip = serializedProperties.clip;
			meshRenderer.enabled = gafTransform.visible;
			meshRenderer.shadowCastingMode = 0;
			meshRenderer.useLightProbes = false;
			meshRenderer.receiveShadows = false;
			meshRenderer.sortingLayerName = clip.settings.spriteLayerName;
			meshRenderer.sortingOrder = clip.settings.spriteLayerValue;
		}

		// Token: 0x0400013C RID: 316
		protected GAFPair<Color, Vector4> m_LocalColorData;

		// Token: 0x0400013D RID: 317
		protected Vector3 m_LocalPosition = Vector3.zero;
	}
}
