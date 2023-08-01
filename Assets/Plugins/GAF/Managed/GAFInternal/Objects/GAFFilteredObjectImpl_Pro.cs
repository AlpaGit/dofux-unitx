using System;
using System.Linq;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x0200005B RID: 91
	internal class GAFFilteredObjectImpl_Pro : GAFObjectImpl
	{
		// Token: 0x0600025E RID: 606 RVA: 0x0000BE8F File Offset: 0x0000A08F
		public GAFFilteredObjectImpl_Pro(GAFObjectInternal _ThisObject, GAFSortingManager _SortingManager, GAFObjectData _Data) : base(_ThisObject, _SortingManager, _Data)
		{
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000BE9C File Offset: 0x0000A09C
		public override void initialize()
		{
			base.initialize();
			if (m_FilteredMaterial == null)
			{
				m_FilteredMaterial = new Material(Shader.Find("GAF/GAFFilteredObject"));
			}
			m_FilteredMaterial.renderQueue = 3000;
			var clip = serializedProperties.clip;
			if (clip.settings.hasIndividualMaterial)
			{
				m_FilteredMaterial.SetInt("_StencilID", GAFTransform.combineStencil(gafTransform.stencilValue, clip.settings.stencilValue));
				m_FilteredMaterial.SetColor("_CustomColorMultiplier", GAFTransform.combineColor(gafTransform.colorMultiplier, clip.settings.animationColorMultiplier));
				m_FilteredMaterial.SetVector("_CustomColorOffset", GAFTransform.combineColorOffset(gafTransform.colorOffset, clip.settings.animationColorOffset, clip.settings.animationColorMultiplier));
			}
			else
			{
				m_FilteredMaterial.SetInt("_StencilID", sharedMaterial.GetInt("_StencilID"));
				m_FilteredMaterial.SetColor("_CustomColorMultiplier", sharedMaterial.GetColor("_CustomColorMultiplier"));
				m_FilteredMaterial.SetVector("_CustomColorOffset", sharedMaterial.GetVector("_CustomColorOffset"));
			}
			serializedProperties.clip.registerExternalMaterial(m_FilteredMaterial);
			thisObject.onWillRenderObject += preRender;
			initShadowObject();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000C01F File Offset: 0x0000A21F
		public override void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			base.updateToState(_State, _Refresh);
			updateFilters(_Refresh);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000C030 File Offset: 0x0000A230
		public void updateFilters(bool _Refresh)
		{
			if (_Refresh || (currentState.filterData != previousState.filterData && currentState.alpha > 0f))
			{
				cleanFilterData();
				if (currentState.filterData == null)
				{
					currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : sharedMaterial);
					currentMesh.uv = (tempUV = initialUV);
					m_UpdateFilterData = false;
					return;
				}
				setup();
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
		public override void updateZPosition(float _New)
		{
			if (_New != gafTransform.transform.localPosition.z)
			{
				var localPosition = gafTransform.transform.localPosition;
				localPosition.z = _New;
				gafTransform.transform.localPosition = localPosition;
				if (currentState.useFilterData)
				{
					var clip = serializedProperties.clip;
					var num = clip.settings.pixelsPerUnit / clip.settings.scale;
					var num2 = 0.017453292f * currentState.filterData.angle;
					var vector = new Vector3(Mathf.Cos(num2) * currentState.filterData.distance, -Mathf.Sin(num2) * currentState.filterData.distance);
					m_Shadow.transform.localPosition = thisObject.cachedTransform.localPosition + vector + new Vector3(0f, 0f, 0.1f / num * clip.settings.zLayerScale);
				}
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000C1F8 File Offset: 0x0000A3F8
		public override void cleanUp()
		{
			base.cleanUp();
			thisObject.onWillRenderObject -= preRender;
			m_FilterMaterial = null;
			deleteUnityObject(m_FilteredMaterial);
			deleteUnityObject(m_FilterMaterial);
			m_DropShadowMesh = null;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000C248 File Offset: 0x0000A448
		private void preRender()
		{
			if (m_UpdateFilterData)
			{
				if (currentState.filterData != null)
				{
					switch (currentState.filterData.type)
					{
					case GAFFilterType.GFT_DropShadow:
						updateDropShadow();
						break;
					case GAFFilterType.GFT_Blur:
						updateBlur();
						break;
					case GAFFilterType.GFT_Glow:
						updateGlow();
						break;
					case GAFFilterType.GFT_ColorMatrix:
						updateColorMtx();
						break;
					}
				}
				m_UpdateFilterData = false;
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000C2C4 File Offset: 0x0000A4C4
		private void setup()
		{
			deleteUnityObject(m_FilterMaterial);
			switch (currentState.filterData.type)
			{
			case GAFFilterType.GFT_DropShadow:
			{
				m_FilterMaterial = new Material(Shader.Find("GAF/GAFGlow"));
				m_FilteredMaterial.DisableKeyword("GAF_COLOR_MTX_FILTER_ON");
				m_FilteredMaterial.EnableKeyword("GAF_COLOR_MTX_FILTER_OFF");
				tempUV = initialUV;
				currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : sharedMaterial);
				var clip = serializedProperties.clip;
				var num = clip.settings.pixelsPerUnit / clip.settings.scale;
				m_Shadow.SetActive(true);
				var num2 = 0.017453292f * currentState.filterData.angle;
				var vector = new Vector3(Mathf.Cos(num2) * currentState.filterData.distance, -Mathf.Sin(num2) * currentState.filterData.distance);
				m_Shadow.transform.localPosition = thisObject.cachedTransform.localPosition + vector + new Vector3(0f, 0f, 0.1f / num * clip.settings.zLayerScale);
				if (m_DropShadowMesh == null)
				{
					m_DropShadowMesh = new Mesh();
					m_DropShadowMesh.name = serializedProperties.name + "_shadow";
				}
				m_DropShadowMesh.vertices = tempVertices;
				m_DropShadowMesh.uv = filterUV;
				m_DropShadowMesh.uv2 = initialOffset;
				m_DropShadowMesh.uv3 = initialOffset;
				m_DropShadowMesh.colors32 = initialColors;
				m_DropShadowMesh.triangles = triangles;
				if (serializedProperties.clip.settings.useLights)
				{
					m_DropShadowMesh.normals = normals;
				}
				m_ShadowFilter.sharedMesh = m_DropShadowMesh;
				m_ShadowRenderer.sharedMaterial = m_FilteredMaterial;
				break;
			}
			case GAFFilterType.GFT_Blur:
				m_FilterMaterial = new Material(Shader.Find("GAF/GAFBlur"));
				m_FilteredMaterial.DisableKeyword("GAF_COLOR_MTX_FILTER_ON");
				m_FilteredMaterial.EnableKeyword("GAF_COLOR_MTX_FILTER_OFF");
				currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : m_FilteredMaterial);
				tempUV = filterUV;
				break;
			case GAFFilterType.GFT_Glow:
				m_FilterMaterial = new Material(Shader.Find("GAF/GAFGlow"));
				m_FilteredMaterial.DisableKeyword("GAF_COLOR_MTX_FILTER_ON");
				m_FilteredMaterial.EnableKeyword("GAF_COLOR_MTX_FILTER_OFF");
				currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : m_FilteredMaterial);
				tempUV = filterUV;
				break;
			case GAFFilterType.GFT_ColorMatrix:
				m_FilterMaterial = null;
				m_FilteredMaterial.mainTexture = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial.mainTexture : sharedTexture);
				m_FilteredMaterial.EnableKeyword("GAF_COLOR_MTX_FILTER_ON");
				m_FilteredMaterial.DisableKeyword("GAF_COLOR_MTX_FILTER_OFF");
				tempUV = initialUV;
				currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : m_FilteredMaterial);
				break;
			}
			if (m_FilterMaterial != null)
			{
				m_FilterMaterial.mainTexture = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial.mainTexture : sharedTexture);
				m_FilterMaterial.renderQueue = 3000;
			}
			m_UpdateFilterData = true;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000C708 File Offset: 0x0000A908
		private void cleanFilterData()
		{
			deleteUnityObject(m_FilterMaterial);
			deleteUnityObject(m_FilteredTexture);
			if (previousState.filterData != null && (currentState.filterData == null || (currentState.filterData != null && previousState.filterData.type != currentState.filterData.type)) && previousState.filterData.type == GAFFilterType.GFT_DropShadow)
			{
				m_Shadow.SetActive(false);
				m_ShadowFilter.sharedMesh = null;
				m_ShadowRenderer.sharedMaterial = null;
				deleteUnityObject(m_DropShadowMesh);
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000C7C0 File Offset: 0x0000A9C0
		private void updateBlur()
		{
			if (m_FilterMaterial != null && m_FilteredMaterial != null)
			{
				var clip = serializedProperties.clip;
				var vector5 = new Vector2(atlasElementData.width * clip.settings.csf, atlasElementData.height * clip.settings.csf);
				var vector2 = new Vector2(currentState.filterData.blurX / 4f, currentState.filterData.blurY / 4f);
				var vector3 = new Vector2(vector5.x + GaussianKernelSize * vector2.x, vector5.y + GaussianKernelSize * vector2.y);
				var temporary = RenderTexture.GetTemporary((int)vector3.x, (int)vector3.y);
				renderElementToRenderTexture(temporary, vector5, true);
				var temporary2 = RenderTexture.GetTemporary((int)vector3.x, (int)vector3.y);
				beginWithClear(temporary2);
				m_FilterMaterial.SetFloat(BlurX, vector2.x);
				m_FilterMaterial.SetFloat(BlurY, 0f);
				Graphics.Blit(temporary, temporary2, m_FilterMaterial);
				RenderTexture.ReleaseTemporary(temporary);
				deleteUnityObject(temporary);
				m_FilteredTexture = new RenderTexture((int)vector3.x, (int)vector3.y, 32);
				beginWithClear(m_FilteredTexture);
				m_FilterMaterial.SetFloat(BlurX, 0f);
				m_FilterMaterial.SetFloat(BlurY, vector2.y);
				Graphics.Blit(temporary2, m_FilteredTexture, m_FilterMaterial);
				RenderTexture.ReleaseTemporary(temporary2);
				deleteUnityObject(temporary2);
				var vector4 = new Vector3((from vector in tempVertices
										   select vector.x).Sum() / (float)tempVertices.Length, (from vector in tempVertices
				select vector.y).Sum() / (float)tempVertices.Length, (from vector in tempVertices
				select vector.z).Sum() / (float)tempVertices.Length);
				m_FilteredMaterial.mainTexture = m_FilteredTexture;
				m_FilteredMaterial.SetVector(Scale, new Vector4(vector3.x / vector5.x, vector3.y / vector5.y));
				m_FilteredMaterial.SetVector(Pivot, vector4);
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000CA94 File Offset: 0x0000AC94
		private void updateGlow()
		{
			if (m_FilterMaterial != null && m_FilteredMaterial != null)
			{
				var clip = serializedProperties.clip;
				var vector5 = new Vector2(atlasElementData.width * clip.settings.csf, atlasElementData.height * clip.settings.csf);
				var vector2 = new Vector2(currentState.filterData.blurX / 16f, currentState.filterData.blurY / 16f);
				var vector3 = new Vector2(vector5.x + GaussianKernelSize * vector2.x, vector5.y + GaussianKernelSize * vector2.y);
				var num = (currentState.filterData.strength > 1f) ? (currentState.filterData.strength / 2f) : currentState.filterData.strength;
				var temporary = RenderTexture.GetTemporary((int)vector3.x, (int)vector3.y);
				renderElementToRenderTexture(temporary, vector5, true);
				var array = BitConverter.GetBytes(currentState.filterData.color).Reverse<byte>().ToArray<byte>();
				var color = new Color((float)array[1] / 255f, (float)array[2] / 255f, (float)array[3] / 255f, (float)array[0] / 255f);
				var temporary2 = RenderTexture.GetTemporary((int)vector3.x, (int)vector3.y);
				beginWithClear(temporary2);
				m_FilterMaterial.SetFloat("_BlurX", vector2.x);
				m_FilterMaterial.SetFloat("_BlurY", 0f);
				m_FilterMaterial.SetColor("_GlowColor", color);
				m_FilterMaterial.SetFloat("_Strength", num);
				Graphics.Blit(temporary, temporary2, m_FilterMaterial);
				RenderTexture.ReleaseTemporary(temporary);
				deleteUnityObject(temporary);
				m_FilteredTexture = new RenderTexture((int)vector3.x, (int)vector3.y, 32);
				beginWithClear(m_FilteredTexture);
				m_FilterMaterial.SetFloat("_BlurX", 0f);
				m_FilterMaterial.SetFloat("_BlurY", vector2.y);
				m_FilterMaterial.SetColor("_GlowColor", color);
				m_FilterMaterial.SetFloat("_Strength", num);
				Graphics.Blit(temporary2, m_FilteredTexture, m_FilterMaterial);
				RenderTexture.ReleaseTemporary(temporary2);
				deleteUnityObject(temporary2);
				renderElementToRenderTexture(m_FilteredTexture, vector5, false);
				var vector4 = new Vector3((from vector in tempVertices
										   select vector.x).Sum() / (float)tempVertices.Length, (from vector in tempVertices
				select vector.y).Sum() / (float)tempVertices.Length, (from vector in tempVertices
				select vector.z).Sum() / (float)tempVertices.Length);
				m_FilteredMaterial.mainTexture = m_FilteredTexture;
				m_FilteredMaterial.SetVector("_Scale", new Vector4(vector3.x / vector5.x, vector3.y / vector5.y));
				m_FilteredMaterial.SetVector("_Pivot", vector4);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000CE54 File Offset: 0x0000B054
		private void updateDropShadow()
		{
			if (m_FilterMaterial != null && m_FilteredMaterial != null)
			{
				var clip = serializedProperties.clip;
				var vector5= new Vector2(atlasElementData.width * clip.settings.csf, atlasElementData.height * clip.settings.csf);
				var vector2 = new Vector2(currentState.filterData.blurX / 16f, currentState.filterData.blurY / 16f);
				var vector3 = new Vector2(vector5.x + GaussianKernelSize * vector2.x, vector5.y + GaussianKernelSize * vector2.y);
				var temporary = RenderTexture.GetTemporary((int)vector3.x, (int)vector3.y);
				renderElementToRenderTexture(temporary, vector5, true);
				var array = BitConverter.GetBytes(currentState.filterData.color).Reverse<byte>().ToArray<byte>();
				var color = new Color((float)array[1] / 255f, (float)array[2] / 255f, (float)array[3] / 255f, (float)array[0] / 255f * Mathf.Clamp(currentState.filterData.strength, 0f, 1f));
				var temporary2 = RenderTexture.GetTemporary((int)vector3.x, (int)vector3.y);
				beginWithClear(temporary2);
				m_FilterMaterial.SetFloat(BlurX, vector2.x);
				m_FilterMaterial.SetFloat(BlurY, 0f);
				m_FilterMaterial.SetColor(GlowColor, color);
				Graphics.Blit(temporary, temporary2, m_FilterMaterial);
				RenderTexture.ReleaseTemporary(temporary);
				deleteUnityObject(temporary);
				m_FilteredTexture = new RenderTexture((int)vector3.x, (int)vector3.y, 32);
				beginWithClear(m_FilteredTexture);
				m_FilterMaterial.SetFloat(BlurX, 0f);
				m_FilterMaterial.SetFloat(BlurY, vector2.y);
				m_FilterMaterial.SetColor(GlowColor, color);
				Graphics.Blit(temporary2, m_FilteredTexture, m_FilterMaterial);
				RenderTexture.ReleaseTemporary(temporary2);
				deleteUnityObject(temporary2);
				var vector4 = new Vector3((from vector in tempVertices
										   select vector.x).Sum() / (float)tempVertices.Length, (from vector in tempVertices
				select vector.y).Sum() / (float)tempVertices.Length, (from vector in tempVertices
				select vector.z).Sum() / (float)tempVertices.Length);
				m_FilteredMaterial.mainTexture = m_FilteredTexture;
				m_FilteredMaterial.SetVector(Scale, new Vector4(vector3.x / vector5.x, vector3.y / vector5.y));
				m_FilteredMaterial.SetVector(Pivot, vector4);
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000D1C0 File Offset: 0x0000B3C0
		private void updateColorMtx()
		{
			m_FilteredMaterial.SetMatrix(ColorMtx, currentState.filterData.colorMtx);
			m_FilteredMaterial.SetVector(Offset, currentState.filterData.colorOffset);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000A777 File Offset: 0x00008977
		private void beginWithClear(RenderTexture _RT)
		{
			_RT.DiscardContents();
			var active = RenderTexture.active;
			RenderTexture.active = _RT;
			GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
			RenderTexture.active = active;
			_RT.MarkRestoreExpected();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000D210 File Offset: 0x0000B410
		private void renderElementToRenderTexture(RenderTexture _RenderTexture, Vector2 _ElementOriginalSize, bool _Clear)
		{
			if (_Clear)
			{
				beginWithClear(_RenderTexture);
			}
			var active = RenderTexture.active;
			RenderTexture.active = _RenderTexture;
			var num = _ElementOriginalSize.x / (float)_RenderTexture.width;
			var num2 = _ElementOriginalSize.y / (float)_RenderTexture.height;
			currentMesh.vertices = new Vector3[]
			{
				new Vector3(-num, num2, 0f),
				new Vector3(-num, -num2, 0f),
				new Vector3(num, -num2, 0f),
				new Vector3(num, num2, 0f)
			};
			currentMesh.uv = initialUV;
			var material = new Material(Shader.Find("GAF/GAFObjectsGroup"));
			material.hideFlags = HideFlags.HideInInspector;
			material.mainTexture = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial.mainTexture : sharedTexture);
			material.DisableKeyword("GAF_VERTICES_TRANSFORM_ON");
			material.EnableKeyword("GAF_VERTICES_TRANSFORM_OFF");
			if (material.SetPass(0))
			{
				Graphics.DrawMeshNow(currentMesh, Matrix4x4.identity);
			}
			currentMesh.vertices = tempVertices;
			currentMesh.uv = tempUV;
			deleteUnityObject(material);
			RenderTexture.active = active;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000D36C File Offset: 0x0000B56C
		private void initShadowObject()
		{
			var transform = thisObject.cachedTransform.parent.Find(serializedProperties.name + "_shadow");
			if (transform != null)
			{
				m_Shadow           = transform.gameObject;
				m_Shadow.hideFlags = HideFlags.HideInHierarchy;
				m_ShadowFilter     = m_Shadow.GetComponent<MeshFilter>();
				m_ShadowRenderer   = m_Shadow.GetComponent<MeshRenderer>();
			}
			else
			{
				m_Shadow                         = new GameObject(serializedProperties.name + "_shadow");
				m_Shadow.transform.parent        = thisObject.cachedTransform.parent;
				m_Shadow.transform.localPosition = new Vector3(0f, 0f, 0f);
				m_Shadow.hideFlags               = HideFlags.HideInHierarchy;
				m_ShadowFilter                   = m_Shadow.AddComponent<MeshFilter>();
				m_ShadowRenderer                 = m_Shadow.AddComponent<MeshRenderer>();
			}
			m_ShadowRenderer.shadowCastingMode = 0;
			m_ShadowRenderer.receiveShadows = false;
			m_ShadowRenderer.useLightProbes = false;
			m_ShadowRenderer.sortingLayerName = serializedProperties.clip.settings.spriteLayerName;
			m_ShadowRenderer.sortingOrder = serializedProperties.clip.settings.spriteLayerValue;
			m_Shadow.SetActive(false);
		}

		// Token: 0x0400011B RID: 283
		private static readonly float GaussianKernelSize = 9f;

		// Token: 0x0400011C RID: 284
		private static readonly Vector2[] filterUV = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		// Token: 0x0400011D RID: 285
		private Material m_FilterMaterial;

		// Token: 0x0400011E RID: 286
		private Material m_FilteredMaterial;

		// Token: 0x0400011F RID: 287
		private RenderTexture m_FilteredTexture;

		// Token: 0x04000120 RID: 288
		private Mesh m_DropShadowMesh;

		// Token: 0x04000121 RID: 289
		private GameObject m_Shadow;

		// Token: 0x04000122 RID: 290
		private MeshFilter m_ShadowFilter;

		// Token: 0x04000123 RID: 291
		private MeshRenderer m_ShadowRenderer;

		// Token: 0x04000124 RID: 292
		private bool m_UpdateFilterData;
		private static readonly int BlurX = Shader.PropertyToID("_BlurX");
		private static readonly int BlurY = Shader.PropertyToID("_BlurY");
		private static readonly int Pivot = Shader.PropertyToID("_Pivot");
		private static readonly int Scale = Shader.PropertyToID("_Scale");
		private static readonly int GlowColor = Shader.PropertyToID("_GlowColor");
		private static readonly int ColorMtx = Shader.PropertyToID("_ColorMtx");
		private static readonly int Offset = Shader.PropertyToID("_Offset");
	}
}
