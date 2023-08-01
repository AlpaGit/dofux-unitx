using System;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000057 RID: 87
	internal class GAFBakedObjectImpl : IGAFObjectImpl, IComparable<GAFBakedObjectImpl>
	{
		// Token: 0x0600023E RID: 574 RVA: 0x0000B3D1 File Offset: 0x000095D1
		public GAFBakedObjectImpl(GAFObjectData _Data, GAFMeshManager _Manager) : base(_Data)
		{
			meshManager = _Manager;
			_Manager.registerSubObject(serializedProperties.objectID, this);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000B3F4 File Offset: 0x000095F4
		public override void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			previousState = currentState;
			currentState = _State;
			if (currentState.alpha > 0f && serializedProperties.visible)
			{
				updateColor();
				updateSorting();
				updateGeometry();
				return;
			}
			currentState = GAFObjectStateData.defaultState;
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
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000B4E0 File Offset: 0x000096E0
		public override void cleanUp()
		{
			deleteUnityObject(m_Mesh);
			m_Mesh = null;
			if (meshManager != null)
			{
				meshManager.unregisterSubObject(serializedProperties.objectID);
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000B519 File Offset: 0x00009719
		public virtual void combineMeshes(ref GAFMeshManager.MeshData _MeshData)
		{
			_MeshData.restObjects.Dequeue();
			_MeshData.pushData(this, currentMaterial);
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000B536 File Offset: 0x00009736
		public bool isVisible
		{
			get
			{
				return serializedProperties.visible && currentState.alpha > 0f;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000B559 File Offset: 0x00009759
		// (set) Token: 0x06000244 RID: 580 RVA: 0x0000B561 File Offset: 0x00009761
		public override Mesh currentMesh
		{
			get
			{
				return m_Mesh;
			}
			set
			{
				m_Mesh = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000B56A File Offset: 0x0000976A
		// (set) Token: 0x06000246 RID: 582 RVA: 0x0000B572 File Offset: 0x00009772
		public override Material currentMaterial
		{
			get
			{
				return m_Material;
			}
			set
			{
				m_Material = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000B57B File Offset: 0x0000977B
		// (set) Token: 0x06000248 RID: 584 RVA: 0x0000B583 File Offset: 0x00009783
		private protected GAFMeshManager meshManager { get; private set; }

		// Token: 0x06000249 RID: 585 RVA: 0x0000B58C File Offset: 0x0000978C
		public int CompareTo(GAFBakedObjectImpl _Other)
		{
			return _Other.currentState.zOrder.CompareTo(currentState.zOrder);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000B5B8 File Offset: 0x000097B8
		protected virtual void updateColor()
		{
			if (currentState.alpha != previousState.alpha)
			{
				for (var i = 0; i < tempColors.Length; i++)
				{
					tempColors[i].a = (byte)(currentState.alpha * 255f);
				}
				if (currentMaterial != null && currentMaterial.HasProperty("_TintColor"))
				{
					currentMaterial.SetColor("_TintColor", tempColors[0]);
				}
				currentMesh.colors32 = tempColors;
				if (currentState.alpha <= 0f || previousState.alpha <= 0f)
				{
					meshManager.pushSortRequest();
				}
				meshManager.pushSetupRequest();
			}
			if (currentState.colorTransformData != previousState.colorTransformData)
			{
				var colorOffset = getColorOffset();
				if (currentState.colorTransformData != null)
				{
					for (var j = 0; j < tempColors.Length; j++)
					{
						tempColors[j] = currentState.colorTransformData.multipliers;
					}
					for (var k = 0; k < colorOffset.Length; k++)
					{
						colorOffset[k] = currentState.colorTransformData.offsets;
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
					for (var m = 0; m < colorOffset.Length; m++)
					{
						colorOffset[m] = m_ZeroVector;
					}
				}
				if (currentMaterial != null && currentMaterial.HasProperty("_TintColor"))
				{
					currentMaterial.SetColor("_TintColor", tempColors[0]);
				}
				if (currentMaterial != null && currentMaterial.HasProperty("_TintColorOffset"))
				{
					currentMaterial.SetVector("_TintColorOffset", colorOffset[0]);
				}
				currentMesh.colors32 = tempColors;
				setColorOffset(colorOffset);
				meshManager.pushSetupRequest();
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000B83E File Offset: 0x00009A3E
		protected virtual void updateSorting()
		{
			if (currentState.zOrder != previousState.zOrder)
			{
				meshManager.pushSortRequest();
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000B864 File Offset: 0x00009A64
		protected virtual void updateGeometry()
		{
			if (currentState != previousState)
			{
				var clip = serializedProperties.clip;
				if (!clip.settings.cacheStates)
				{
					var pivotOffset = clip.settings.pivotOffset;
					var num = clip.settings.pixelsPerUnit / clip.settings.scale;
					var num2 = currentState.localPosition.x / num;
					var num3 = -currentState.localPosition.y / num;
					m_Matrix[0, 0] = currentState.a;
					if (clip.settings.flipX)
					{
						serializedProperties.flip = new Vector2(-num2 * 2f, 0f);
						m_Matrix[0, 1] = -currentState.c;
						m_Matrix[1, 0] = -currentState.b;
						m_Matrix[0, 3] = num2 - serializedProperties.offset.x - pivotOffset.x + serializedProperties.flip.x;
						m_Matrix[1, 3] = num3 - serializedProperties.offset.y - pivotOffset.y + serializedProperties.flip.y;
					}
					else
					{
						serializedProperties.flip = Vector2.zero;
						m_Matrix[0, 1] = currentState.c;
						m_Matrix[1, 0] = currentState.b;
						m_Matrix[0, 3] = num2 + serializedProperties.offset.x + pivotOffset.x + serializedProperties.flip.x;
						m_Matrix[1, 3] = num3 + serializedProperties.offset.y + pivotOffset.y + serializedProperties.flip.y;
					}
					m_Matrix[1, 1] = currentState.d;
					m_Matrix[2, 3] = 0f;
					for (var i = 0; i < initialVertices.Length; i++)
					{
						tempVertices[i] = m_Matrix.MultiplyPoint3x4(initialVertices[i]);
					}
				}
				else
				{
					tempVertices = currentState.vertices;
				}
				currentMesh.vertices = tempVertices;
				currentMesh.RecalculateBounds();
				meshManager.pushSetupRequest();
			}
		}

		// Token: 0x04000117 RID: 279
		private Mesh m_Mesh;

		// Token: 0x04000118 RID: 280
		private Material m_Material;
	}
}
