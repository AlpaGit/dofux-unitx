using System;
using System.Linq;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
    // Token: 0x02000052 RID: 82
    internal class GAFBakedFilteredObjectImpl_Pro : GAFBakedObjectImpl
    {
        // Token: 0x06000204 RID: 516 RVA: 0x00009675 File Offset: 0x00007875
        public GAFBakedFilteredObjectImpl_Pro(GAFObjectData _Data, GAFMeshManager _Manager) : base(_Data, _Manager)
        {
            meshManager.onWillRenderObject += preRender;
        }

        // Token: 0x06000205 RID: 517 RVA: 0x00009698 File Offset: 0x00007898
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
                var gafTransform = serializedProperties.clip.gafTransform;
                m_FilteredMaterial.SetInt("_StencilID",
                    GAFTransform.combineStencil(gafTransform.stencilValue, clip.settings.stencilValue));
                m_FilteredMaterial.SetColor("_CustomColorMultiplier",
                    GAFTransform.combineColor(gafTransform.colorMultiplier, clip.settings.animationColorMultiplier));
                m_FilteredMaterial.SetVector("_CustomColorOffset",
                    GAFTransform.combineColorOffset(gafTransform.colorOffset, clip.settings.animationColorOffset,
                        clip.settings.animationColorMultiplier));
            }
            else
            {
                m_FilteredMaterial.SetInt("_StencilID", sharedMaterial.GetInt("_StencilID"));
                m_FilteredMaterial.SetColor("_CustomColorMultiplier",
                    sharedMaterial.GetColor("_CustomColorMultiplier"));
                m_FilteredMaterial.SetVector("_CustomColorOffset",
                    sharedMaterial.GetVector("_CustomColorOffset"));
            }

            serializedProperties.clip.registerExternalMaterial(m_FilteredMaterial);
        }

        // Token: 0x06000206 RID: 518 RVA: 0x00009800 File Offset: 0x00007A00
        public override void updateToState(GAFObjectStateData _State, bool _Refresh)
        {
            base.updateToState(_State, _Refresh);
            updateFilters(_Refresh);
        }

        // Token: 0x06000207 RID: 519 RVA: 0x00009814 File Offset: 0x00007A14
        public override void cleanUp()
        {
            base.cleanUp();
            meshManager.onWillRenderObject -= preRender;
            m_FilteredTexture              =  null;
            deleteUnityObject(m_FilteredMaterial);
            deleteUnityObject(m_FilterMaterial);
            deleteUnityObject(m_DropShadowMesh);
        }

        // Token: 0x06000208 RID: 520 RVA: 0x0000986C File Offset: 0x00007A6C
        public override void combineMeshes(ref GAFMeshManager.MeshData _MeshData)
        {
            if (isVisible)
            {
                if (currentState.filterData != null &&
                    currentState.filterData.type == GAFFilterType.GFT_DropShadow)
                {
                    _MeshData.pushData(m_DropShadowMesh, m_FilteredMaterial);
                }

                base.combineMeshes(ref _MeshData);
                return;
            }

            _MeshData.restObjects.Dequeue();
        }

        // Token: 0x06000209 RID: 521 RVA: 0x000098C8 File Offset: 0x00007AC8
        private void updateFilters(bool _Refresh)
        {
            if (_Refresh || currentState.filterData != previousState.filterData)
            {
                cleanFilterData();
                if (currentState.filterData == null)
                {
                    currentMaterial = ((serializedProperties.customMaterial != null)
                        ? serializedProperties.customMaterial
                        : sharedMaterial);
                    tempUV = initialUV;
                }
                else
                {
                    setup();
                }

                meshManager.pushSortRequest();
                meshManager.pushSetupRequest();
            }
        }

        // Token: 0x0600020A RID: 522 RVA: 0x0000995C File Offset: 0x00007B5C
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
                    if (m_DropShadowMesh == null)
                    {
                        m_DropShadowMesh = new Mesh();
                    }

                    m_DropShadowMesh.name = serializedProperties.name + "_shadow";
                    var num = 0.017453292f * currentState.filterData.angle;
                    var vector = new Vector3(Mathf.Cos(num) * currentState.filterData.distance,
                        -Mathf.Sin(num) * currentState.filterData.distance);
                    var array = new Vector3[4];
                    for (var i = 0; i < array.Length; i++)
                    {
                        array[i] = tempVertices[i] + vector;
                    }

                    m_DropShadowMesh.vertices = array;
                    m_DropShadowMesh.uv       = filterUV;
                    m_DropShadowMesh.colors32 = initialColors;
                    m_DropShadowMesh.uv2      = initialOffset;
                    m_DropShadowMesh.uv3      = initialOffset;
                    currentMaterial = ((serializedProperties.customMaterial != null)
                        ? serializedProperties.customMaterial
                        : sharedMaterial);
                    tempUV = initialUV;
                    break;
                }
                case GAFFilterType.GFT_Blur:
                    m_FilterMaterial = new Material(Shader.Find("GAF/GAFBlur"));
                    m_FilteredMaterial.DisableKeyword("GAF_COLOR_MTX_FILTER_ON");
                    m_FilteredMaterial.EnableKeyword("GAF_COLOR_MTX_FILTER_OFF");
                    currentMaterial = ((serializedProperties.customMaterial != null)
                        ? serializedProperties.customMaterial
                        : m_FilteredMaterial);
                    tempUV = filterUV;
                    break;
                case GAFFilterType.GFT_Glow:
                    m_FilterMaterial = new Material(Shader.Find("GAF/GAFGlow"));
                    m_FilteredMaterial.DisableKeyword("GAF_COLOR_MTX_FILTER_ON");
                    m_FilteredMaterial.EnableKeyword("GAF_COLOR_MTX_FILTER_OFF");
                    currentMaterial = ((serializedProperties.customMaterial != null)
                        ? serializedProperties.customMaterial
                        : m_FilteredMaterial);
                    tempUV = filterUV;
                    break;
                case GAFFilterType.GFT_ColorMatrix:
                    m_FilterMaterial               = null;
                    m_FilteredMaterial.mainTexture = sharedTexture;
                    m_FilteredMaterial.EnableKeyword("GAF_COLOR_MTX_FILTER_ON");
                    m_FilteredMaterial.DisableKeyword("GAF_COLOR_MTX_FILTER_OFF");
                    currentMaterial = ((serializedProperties.customMaterial != null)
                        ? serializedProperties.customMaterial
                        : m_FilteredMaterial);
                    tempUV = initialUV;
                    break;
            }

            if (m_FilterMaterial != null)
            {
                m_FilterMaterial.mainTexture = sharedTexture;
                m_FilterMaterial.renderQueue = 3000;
            }

            m_UpdateFilterData = true;
        }

        // Token: 0x0600020B RID: 523 RVA: 0x00009CA9 File Offset: 0x00007EA9
        private void cleanFilterData()
        {
            deleteUnityObject(m_FilterMaterial);
            deleteUnityObject(m_FilteredTexture);
        }

        // Token: 0x0600020C RID: 524 RVA: 0x00009CC4 File Offset: 0x00007EC4
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

        // Token: 0x0600020D RID: 525 RVA: 0x00009D40 File Offset: 0x00007F40
        private void updateBlur()
        {
            if (m_FilterMaterial != null && m_FilteredMaterial != null)
            {
                var clip = serializedProperties.clip;
                var vector5 = new Vector2(atlasElementData.width * clip.settings.csf,
                    atlasElementData.height * clip.settings.csf);
                var vector2 = new Vector2(currentState.filterData.blurX / 4f,
                    currentState.filterData.blurY / 4f);
                var vector3 = new Vector2(vector5.x + GaussianKernelSize * vector2.x,
                    vector5.y + GaussianKernelSize * vector2.y);
                var temporary = RenderTexture.GetTemporary((int)vector3.x, (int)vector3.y);
                renderElementToRenderTexture(temporary, vector5, true);
                var temporary2 = RenderTexture.GetTemporary((int)vector3.x, (int)vector3.y);
                beginWithClear(temporary2);
                m_FilterMaterial.SetFloat("_BlurX", vector2.x);
                m_FilterMaterial.SetFloat("_BlurY", 0f);
                Graphics.Blit(temporary, temporary2, m_FilterMaterial);
                RenderTexture.ReleaseTemporary(temporary);
                deleteUnityObject(temporary);
                m_FilteredTexture = new RenderTexture((int)vector3.x, (int)vector3.y, 32);
                beginWithClear(m_FilteredTexture);
                m_FilterMaterial.SetFloat("_BlurX", 0f);
                m_FilterMaterial.SetFloat("_BlurY", vector2.y);
                Graphics.Blit(temporary2, m_FilteredTexture, m_FilterMaterial);
                RenderTexture.ReleaseTemporary(temporary2);
                deleteUnityObject(temporary2);
                var vector4 = new Vector3((tempVertices.Select(vector => vector.x)).Sum() / 4f, (tempVertices.Select(vector => vector.y)).Sum() / 4f, (from vector in tempVertices
                                                                           select vector.z).Sum() / 4f);
                m_FilteredMaterial.mainTexture = m_FilteredTexture;
                m_FilteredMaterial.SetVector("_Scale", new Vector4(vector3.x / vector5.x, vector3.y / vector5.y));
                m_FilteredMaterial.SetVector("_Pivot", vector4);
            }
        }

        // Token: 0x0600020E RID: 526 RVA: 0x0000A008 File Offset: 0x00008208
        private void updateGlow()
        {
            if (m_FilterMaterial != null && m_FilteredMaterial != null)
            {
                var clip = serializedProperties.clip;
                var vector5 = new Vector2(atlasElementData.width * clip.settings.csf,
                    atlasElementData.height * clip.settings.csf);
                var vector2 = new Vector2(currentState.filterData.blurX / 16f,
                    currentState.filterData.blurY / 16f);
                var vector3 = new Vector2(vector5.x + GaussianKernelSize * vector2.x,
                    vector5.y + GaussianKernelSize * vector2.y);
                var num = (currentState.filterData.strength > 1f)
                    ? (currentState.filterData.strength / 2f)
                    : currentState.filterData.strength;
                var temporary = RenderTexture.GetTemporary((int)vector3.x, (int)vector3.y);
                renderElementToRenderTexture(temporary, vector5, true);
                var array = BitConverter.GetBytes(currentState.filterData.color).Reverse<byte>()
                                        .ToArray<byte>();
                var color = new Color((float)array[1] / 255f, (float)array[2] / 255f, (float)array[3] / 255f,
                    (float)array[0] / 255f);
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
                                           select vector.x).Sum() / 4f, (from vector in tempVertices
                                                                         select vector.y).Sum() / 4f,
                    (from vector in tempVertices
                     select vector.z).Sum() / 4f);
                m_FilteredMaterial.mainTexture = m_FilteredTexture;
                m_FilteredMaterial.SetVector("_Scale", new Vector4(vector3.x / vector5.x, vector3.y / vector5.y));
                m_FilteredMaterial.SetVector("_Pivot", vector4);
            }
        }

        // Token: 0x0600020F RID: 527 RVA: 0x0000A3BC File Offset: 0x000085BC
        private void updateDropShadow()
        {
            if (m_FilterMaterial != null && m_FilteredMaterial != null)
            {
                var clip = serializedProperties.clip;
                var vector5 = new Vector2(atlasElementData.width * clip.settings.csf,
                    atlasElementData.height * clip.settings.csf);
                var vector2 = new Vector2(currentState.filterData.blurX / 16f,
                    currentState.filterData.blurY / 16f);
                var vector3 = new Vector2(vector5.x + GaussianKernelSize * vector2.x,
                    vector5.y + GaussianKernelSize * vector2.y);
                var temporary = RenderTexture.GetTemporary((int)vector3.x, (int)vector3.y);
                renderElementToRenderTexture(temporary, vector5, true);
                var array = BitConverter.GetBytes(currentState.filterData.color).Reverse<byte>()
                                        .ToArray<byte>();
                var color = new Color((float)array[1] / 255f, (float)array[2] / 255f, (float)array[3] / 255f,
                    (float)array[0] / 255f * Mathf.Clamp(currentState.filterData.strength, 0f, 1f));
                var temporary2 = RenderTexture.GetTemporary((int)vector3.x, (int)vector3.y);
                beginWithClear(temporary2);
                m_FilterMaterial.SetFloat("_BlurX", vector2.x);
                m_FilterMaterial.SetFloat("_BlurY", 0f);
                m_FilterMaterial.SetColor("_GlowColor", color);
                Graphics.Blit(temporary, temporary2, m_FilterMaterial);
                RenderTexture.ReleaseTemporary(temporary);
                deleteUnityObject(temporary);
                m_FilteredTexture = new RenderTexture((int)vector3.x, (int)vector3.y, 32);
                beginWithClear(m_FilteredTexture);
                m_FilterMaterial.SetFloat("_BlurX", 0f);
                m_FilterMaterial.SetFloat("_BlurY", vector2.y);
                m_FilterMaterial.SetColor("_GlowColor", color);
                Graphics.Blit(temporary2, m_FilteredTexture, m_FilterMaterial);
                RenderTexture.ReleaseTemporary(temporary2);
                deleteUnityObject(temporary2);
                var vector4 = new Vector3((from vector in tempVertices
                                           select vector.x).Sum() / 4f, (from vector in tempVertices
                                                                         select vector.y).Sum() / 4f,
                    (from vector in tempVertices select vector.z).Sum() / 4f);
                m_FilteredMaterial.mainTexture = m_FilteredTexture;
                m_FilteredMaterial.SetVector("_Scale", new Vector4(vector3.x / vector5.x, vector3.y / vector5.y));
                m_FilteredMaterial.SetVector("_Pivot", vector4);
            }
        }

        // Token: 0x06000210 RID: 528 RVA: 0x0000A71C File Offset: 0x0000891C
        private void updateColorMtx()
        {
            if (m_FilteredMaterial != null)
            {
                m_FilteredMaterial.SetMatrix("_ColorMtx", currentState.filterData.colorMtx);
                m_FilteredMaterial.SetVector("_Offset", currentState.filterData.colorOffset);
            }
        }

        // Token: 0x06000211 RID: 529 RVA: 0x0000A777 File Offset: 0x00008977
        private void beginWithClear(RenderTexture _RT)
        {
            _RT.DiscardContents();
            var active = RenderTexture.active;
            RenderTexture.active = _RT;
            GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
            RenderTexture.active = active;
            _RT.MarkRestoreExpected();
        }

        // Token: 0x06000212 RID: 530 RVA: 0x0000A7B8 File Offset: 0x000089B8
        private void renderElementToRenderTexture(RenderTexture _RenderTexture, Vector2 _ElementOriginalSize,
            bool _Clear)
        {
            if (_Clear)
            {
                beginWithClear(_RenderTexture);
            }

            var active = RenderTexture.active;
            RenderTexture.active = _RenderTexture;
            var num  = _ElementOriginalSize.x / (float)_RenderTexture.width;
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
            material.mainTexture = sharedTexture;
            material.DisableKeyword("GAF_VERTICES_TRANSFORM_ON");
            material.EnableKeyword("GAF_VERTICES_TRANSFORM_OFF");
            if (material.SetPass(0))
            {
                Graphics.DrawMeshNow(currentMesh, Matrix4x4.identity);
            }

            currentMesh.vertices = tempVertices;
            currentMesh.uv       = tempUV;
            deleteUnityObject(material);
            RenderTexture.active = active;
        }

        // Token: 0x040000FD RID: 253
        private static readonly float GaussianKernelSize = 9f;

        // Token: 0x040000FE RID: 254
        private static readonly Vector2[] filterUV = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f)
        };

        // Token: 0x040000FF RID: 255
        private Material m_FilterMaterial;

        // Token: 0x04000100 RID: 256
        private Material m_FilteredMaterial;

        // Token: 0x04000101 RID: 257
        private RenderTexture m_FilteredTexture;

        // Token: 0x04000102 RID: 258
        private Mesh m_DropShadowMesh;

        // Token: 0x04000103 RID: 259
        private bool m_UpdateFilterData;
    }
}