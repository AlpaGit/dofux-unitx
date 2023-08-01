using System.IO;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x0200004F RID: 79
	public abstract class IGAFObjectImpl
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000085B5 File Offset: 0x000067B5
		// (set) Token: 0x060001CC RID: 460 RVA: 0x000085BD File Offset: 0x000067BD
		public Vector3[] tempVertices
		{
			get
			{
				return m_Vertices;
			}
			set
			{
				m_Vertices = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001CD RID: 461 RVA: 0x000085C6 File Offset: 0x000067C6
		// (set) Token: 0x060001CE RID: 462 RVA: 0x000085CE File Offset: 0x000067CE
		public Color32[] tempColors
		{
			get
			{
				return m_Colors;
			}
			set
			{
				m_Colors = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001CF RID: 463 RVA: 0x000085D7 File Offset: 0x000067D7
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x000085DF File Offset: 0x000067DF
		public Vector2[] tempUV
		{
			get
			{
				return m_UVs;
			}
			set
			{
				m_UVs = value;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x000085E8 File Offset: 0x000067E8
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x000085F0 File Offset: 0x000067F0
		public Vector2[] tempUV2
		{
			get
			{
				return m_TempUV2;
			}
			set
			{
				m_TempUV2 = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x000085F9 File Offset: 0x000067F9
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00008601 File Offset: 0x00006801
		public Vector2[] tempUV3
		{
			get
			{
				return m_TempUV3;
			}
			set
			{
				m_TempUV3 = value;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000860C File Offset: 0x0000680C
		public IGAFObjectImpl(GAFObjectData _Data)
		{
			serializedProperties = _Data;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000086A8 File Offset: 0x000068A8
		public virtual void initialize()
		{
			currentMesh = null;
			var clip = serializedProperties.clip;
			atlasData = clip.asset.getAtlases(clip.timelineID).Find((GAFAtlasData atlas) => atlas.scale == clip.settings.scale);
			if (serializedProperties.atlasCustomElementID != -1)
			{
				atlasElementData = clip.asset.sharedData.customRegions[clip.settings.scale].Find((GAFAtlasElementData x) => (ulong)x.id == (ulong)((long)serializedProperties.atlasCustomElementID));
			}
			else
			{
				atlasElementData = atlasData.getElement(serializedProperties.atlasElementID);
			}
			texturesData = atlasData.getAtlas(atlasElementData.atlasID);
			sharedTexture = clip.resource.getSharedTexture(Path.GetFileNameWithoutExtension(texturesData.getFileName(clip.settings.csf)));
			if (sharedMaterial == null)
			{
				sharedMaterial = clip.getSharedMaterial(Path.GetFileNameWithoutExtension(texturesData.getFileName(clip.settings.csf)));
			}
			currentState = new GAFObjectStateData(serializedProperties.objectID);
			calcInitialVertices();
			calcUV();
			currentMesh = new Mesh();
			currentMesh.MarkDynamic();
			currentMesh.name = serializedProperties.name;
			currentMesh.vertices = initialVertices;
			currentMesh.uv = initialUV;
			currentMesh.uv2 = initialOffset;
			currentMesh.uv3 = initialOffset;
			if (serializedProperties.clip.settings.useLights)
			{
				currentMesh.normals = normals;
			}
			currentMesh.triangles = triangles;
			currentMesh.colors32 = initialColors;
			m_TempUV2 = new Vector2[]
			{
				Vector2.zero,
				Vector2.zero,
				Vector2.zero,
				Vector2.zero
			};
			m_TempUV3 = new Vector2[]
			{
				Vector2.zero,
				Vector2.zero,
				Vector2.zero,
				Vector2.zero
			};
			m_Colors = new Color32[]
			{
				whiteColor,
				whiteColor,
				whiteColor,
				whiteColor
			};
			currentMaterial = ((serializedProperties.customMaterial != null) ? serializedProperties.customMaterial : sharedMaterial);
		}

		// Token: 0x060001D7 RID: 471
		public abstract void updateToState(GAFObjectStateData _State, bool _Refresh);

		// Token: 0x060001D8 RID: 472
		public abstract void cleanUp();

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x000089BA File Offset: 0x00006BBA
		// (set) Token: 0x060001DA RID: 474 RVA: 0x000089C2 File Offset: 0x00006BC2
		public GAFObjectStateData currentState { get; protected set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000089CB File Offset: 0x00006BCB
		// (set) Token: 0x060001DC RID: 476 RVA: 0x000089D3 File Offset: 0x00006BD3
		public GAFObjectStateData previousState { get; protected set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001DD RID: 477 RVA: 0x000089DC File Offset: 0x00006BDC
		// (set) Token: 0x060001DE RID: 478 RVA: 0x000089E4 File Offset: 0x00006BE4
		public virtual Mesh currentMesh { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001DF RID: 479 RVA: 0x000089ED File Offset: 0x00006BED
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x000089F5 File Offset: 0x00006BF5
		public virtual Material currentMaterial { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x000089FE File Offset: 0x00006BFE
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00008A06 File Offset: 0x00006C06
		public GAFObjectData serializedProperties { get; private set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00008A0F File Offset: 0x00006C0F
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00008A17 File Offset: 0x00006C17
		private protected Texture2D sharedTexture { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00008A20 File Offset: 0x00006C20
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00008A28 File Offset: 0x00006C28
		private protected Material sharedMaterial { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00008A31 File Offset: 0x00006C31
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00008A39 File Offset: 0x00006C39
		public Vector3[] initialVertices { get; private set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00008A42 File Offset: 0x00006C42
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00008A4A File Offset: 0x00006C4A
		private protected Vector2[] initialUV { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00008A53 File Offset: 0x00006C53
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00008A5B File Offset: 0x00006C5B
		private protected GAFAtlasData atlasData { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00008A64 File Offset: 0x00006C64
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00008A6C File Offset: 0x00006C6C
		private protected GAFAtlasElementData atlasElementData { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00008A75 File Offset: 0x00006C75
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00008A7D File Offset: 0x00006C7D
		private protected GAFTexturesData texturesData { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00008A86 File Offset: 0x00006C86
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00008A8E File Offset: 0x00006C8E
		protected bool isColored
		{
			get
			{
				return m_IsColored;
			}
			set
			{
				m_IsColored = value;
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008A98 File Offset: 0x00006C98
		private void calcInitialVertices()
		{
			var clip = serializedProperties.clip;
			var num = atlasElementData.scaleX * clip.settings.pixelsPerUnit;
			var num2 = atlasElementData.scaleY * clip.settings.pixelsPerUnit;
			var num3 = atlasElementData.pivotX / num;
			var num4 = atlasElementData.pivotY / num2;
			var num5 = atlasElementData.width / num * serializedProperties.meshSizeMultiplier.x;
			var num6 = atlasElementData.height / num2 * serializedProperties.meshSizeMultiplier.y;
			if (atlasElementData.rotation == 0)
			{
				num5 = atlasElementData.width / num * serializedProperties.meshSizeMultiplier.x;
				num6 = atlasElementData.height / num2 * serializedProperties.meshSizeMultiplier.y;
			}
			else
			{
				num6 = atlasElementData.width / num * serializedProperties.meshSizeMultiplier.x;
				num5 = atlasElementData.height / num2 * serializedProperties.meshSizeMultiplier.y;
			}
			initialVertices = new Vector3[4];
			if (!clip.settings.flipX)
			{
				initialVertices[0] = (m_Vertices[0] = new Vector3(-num3, num4 - num6, 0f));
				initialVertices[1] = (m_Vertices[1] = new Vector3(-num3, num4, 0f));
				initialVertices[2] = (m_Vertices[2] = new Vector3(-num3 + num5, num4, 0f));
				initialVertices[3] = (m_Vertices[3] = new Vector3(-num3 + num5, num4 - num6, 0f));
				return;
			}
			initialVertices[0] = (m_Vertices[0] = new Vector3(num3, num4 - num6, 0f));
			initialVertices[1] = (m_Vertices[1] = new Vector3(num3, num4, 0f));
			initialVertices[2] = (m_Vertices[2] = new Vector3(num3 - num5, num4, 0f));
			initialVertices[3] = (m_Vertices[3] = new Vector3(num3 - num5, num4 - num6, 0f));
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008D58 File Offset: 0x00006F58
		private void calcUV()
		{
			var clip = serializedProperties.clip;
			var texture2D = (serializedProperties.customMaterial != null && serializedProperties.customMaterial.mainTexture != null) ? (serializedProperties.customMaterial.mainTexture as Texture2D) : sharedTexture;
			if (serializedProperties.useCustomAtlasTextureRect)
			{
				if (serializedProperties.atlasTextureRect == GAFObjectData.invalidRect)
				{
					serializedProperties.atlasTextureRect = new Rect(atlasElementData.x, atlasElementData.y, atlasElementData.width, atlasElementData.height);
				}
			}
			else
			{
				serializedProperties.atlasTextureRect = new Rect(atlasElementData.x, atlasElementData.y, atlasElementData.width, atlasElementData.height);
			}
			var x = serializedProperties.atlasTextureRect.x;
			var y = serializedProperties.atlasTextureRect.y;
			var width = serializedProperties.atlasTextureRect.width;
			var height = serializedProperties.atlasTextureRect.height;
			var num = x * clip.settings.csf / (float)texture2D.width;
			var num2 = (x + width) * clip.settings.csf / (float)texture2D.width;
			var num3 = ((float)texture2D.height - y * clip.settings.csf - height * clip.settings.csf) / (float)texture2D.height;
			var num4 = ((float)texture2D.height - y * clip.settings.csf) / (float)texture2D.height;
			initialUV = new Vector2[4];
			if (clip.settings.flipX)
			{
				initialUV[0] = (m_UVs[0] = new Vector2(num, num3));
				initialUV[1] = (m_UVs[1] = new Vector2(num, num4));
				initialUV[2] = (m_UVs[2] = new Vector2(num2, num4));
				initialUV[3] = (m_UVs[3] = new Vector2(num2, num3));
				return;
			}
			if (atlasElementData.rotation == 0)
			{
				initialUV[0] = (m_UVs[0] = new Vector2(num, num3));
				initialUV[1] = (m_UVs[1] = new Vector2(num, num4));
				initialUV[2] = (m_UVs[2] = new Vector2(num2, num4));
				initialUV[3] = (m_UVs[3] = new Vector2(num2, num3));
				return;
			}
			if (atlasElementData.rotation == 1)
			{
				initialUV[0] = (m_UVs[0] = new Vector2(num, num4));
				initialUV[1] = (m_UVs[1] = new Vector2(num2, num4));
				initialUV[2] = (m_UVs[2] = new Vector2(num2, num3));
				initialUV[3] = (m_UVs[3] = new Vector2(num, num3));
				return;
			}
			initialUV[0] = (m_UVs[0] = new Vector2(num2, num3));
			initialUV[1] = (m_UVs[1] = new Vector2(num, num3));
			initialUV[2] = (m_UVs[2] = new Vector2(num, num4));
			initialUV[3] = (m_UVs[3] = new Vector2(num2, num4));
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000091FA File Offset: 0x000073FA
		protected void deleteUnityObject(Object _Obj)
		{
			if (_Obj != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(_Obj);
					return;
				}
				Object.DestroyImmediate(_Obj);
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000921C File Offset: 0x0000741C
		protected void setColorOffset(Vector4[] _ColorOffset)
		{
			for (var i = 0; i < _ColorOffset.Length; i++)
			{
				m_TempOffset.x = _ColorOffset[i].x;
				m_TempOffset.y = _ColorOffset[i].y;
				m_TempUV2[i] = m_TempOffset;
				m_TempOffset.x = _ColorOffset[i].z;
				m_TempOffset.y = _ColorOffset[i].w;
				m_TempUV3[i] = m_TempOffset;
			}
			currentMesh.uv2 = m_TempUV2;
			currentMesh.uv3 = m_TempUV3;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000092E0 File Offset: 0x000074E0
		protected Vector4[] getColorOffset()
		{
			for (var i = 0; i < 4; i++)
			{
				var array = m_TempUV2;
				var vector = m_TempColorOffset[i];
				vector.x = array[i].x;
				vector.y = array[i].y;
				array = m_TempUV3;
				vector.z = array[i].x;
				vector.w = array[i].y;
				m_TempColorOffset[i] = vector;
			}
			return m_TempColorOffset;
		}

		// Token: 0x040000DC RID: 220
		public static readonly Vector3 normal = new Vector3(0f, 0f, -1f);

		// Token: 0x040000DD RID: 221
		public static readonly Vector3[] normals = new Vector3[]
		{
			normal,
			normal,
			normal,
			normal
		};

		// Token: 0x040000DE RID: 222
		public static readonly Color32 whiteColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x040000DF RID: 223
		public static readonly Color32[] initialColors = new Color32[]
		{
			whiteColor,
			whiteColor,
			whiteColor,
			whiteColor
		};

		// Token: 0x040000E0 RID: 224
		public static readonly Vector2[] initialOffset = new Vector2[]
		{
			Vector2.zero,
			Vector2.zero,
			Vector2.zero,
			Vector2.zero
		};

		// Token: 0x040000E1 RID: 225
		public static readonly int[] triangles = new int[]
		{
			2,
			0,
			1,
			3,
			0,
			2
		};

		// Token: 0x040000E2 RID: 226
		private Vector4[] m_TempColorOffset = new Vector4[4];

		// Token: 0x040000E3 RID: 227
		private Vector2 m_TempOffset;

		// Token: 0x040000E4 RID: 228
		private bool m_IsColored;

		// Token: 0x040000E5 RID: 229
		protected Vector3[] m_Vertices = new Vector3[4];

		// Token: 0x040000E6 RID: 230
		protected Color32[] m_Colors = new Color32[4];

		// Token: 0x040000E7 RID: 231
		protected Vector2[] m_UVs = new Vector2[4];

		// Token: 0x040000E8 RID: 232
		protected Vector2[] m_TempUV2 = new Vector2[4];

		// Token: 0x040000E9 RID: 233
		protected Vector2[] m_TempUV3 = new Vector2[4];

		// Token: 0x040000EA RID: 234
		protected Matrix4x4 m_Matrix = Matrix4x4.identity;

		// Token: 0x040000EB RID: 235
		protected readonly Vector4 m_ZeroVector = Vector4.zero;

		// Token: 0x040000EC RID: 236
		protected readonly Vector3 m_OneVector = Vector3.one;

		// Token: 0x040000ED RID: 237
		protected readonly Quaternion m_IdentityQuaternion = Quaternion.identity;

		// Token: 0x040000EE RID: 238
		protected readonly Matrix4x4 m_IdentityMatrix = Matrix4x4.identity;
	}
}
