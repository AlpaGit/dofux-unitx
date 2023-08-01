using System;
using GAF.Managed.GAFInternal.Attributes;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000040 RID: 64
	[Serializable]
	public class GAFObjectData
	{
		// Token: 0x0600013B RID: 315 RVA: 0x00007404 File Offset: 0x00005604
		public GAFObjectData(string _Name, ObjectBehaviourType _Type, GAFBaseClip _Clip, GAFBaseObjectsManager _Manager, GAFInternal.Data.GAFObjectData _Data)
		{
			m_Name = _Name;
			m_BehaviourType = _Type;
			m_Clip = _Clip;
			m_Manager = _Manager;
			m_ID = (int)_Data.id;
			m_ObjectType = _Data.type;
			m_AtlasInitialElementID = (int)_Data.atlasElementID;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000074B8 File Offset: 0x000056B8
		public GAFObjectData(GAFObjectData _Other)
		{
			m_Name = _Other.name;
			m_BehaviourType = _Other.behaviourType;
			m_Clip = _Other.clip;
			m_Manager = _Other.manager;
			m_ID = (int)_Other.objectID;
			m_AtlasInitialElementID = (int)_Other.atlasElementID;
			m_IsVisible = _Other.visible;
			m_Material = _Other.customMaterial;
			m_Offset = _Other.offset;
			m_UseCustomAtlasTextureRect = _Other.useCustomAtlasTextureRect;
			m_AtlasTextureRect = _Other.atlasTextureRect;
			m_MeshSizeMultiplier = _Other.meshSizeMultiplier;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000075B8 File Offset: 0x000057B8
		public uint objectID
		{
			get
			{
				return (uint)m_ID;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000075C0 File Offset: 0x000057C0
		public string name
		{
			get
			{
				return m_Name;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000075C8 File Offset: 0x000057C8
		public ObjectBehaviourType behaviourType
		{
			get
			{
				return m_BehaviourType;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000075D0 File Offset: 0x000057D0
		public GAFObjectType type
		{
			get
			{
				return m_ObjectType;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000141 RID: 321 RVA: 0x000075D8 File Offset: 0x000057D8
		// (set) Token: 0x06000142 RID: 322 RVA: 0x000075E0 File Offset: 0x000057E0
		public uint atlasElementID
		{
			get
			{
				return (uint)m_AtlasInitialElementID;
			}
			set
			{
				m_AtlasInitialElementID = (int)value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000075E9 File Offset: 0x000057E9
		// (set) Token: 0x06000144 RID: 324 RVA: 0x000075F1 File Offset: 0x000057F1
		public int atlasCustomElementID
		{
			get
			{
				return m_AtlasCustomElementID;
			}
			set
			{
				m_AtlasCustomElementID = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000075FA File Offset: 0x000057FA
		public GAFBaseClip clip
		{
			get
			{
				return m_Clip;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00007602 File Offset: 0x00005802
		public GAFBaseObjectsManager manager
		{
			get
			{
				return m_Manager;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000760A File Offset: 0x0000580A
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00007612 File Offset: 0x00005812
		public bool visible
		{
			get
			{
				return m_IsVisible;
			}
			set
			{
				m_IsVisible = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000761B File Offset: 0x0000581B
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00007623 File Offset: 0x00005823
		public Vector2 offset
		{
			get
			{
				return m_Offset;
			}
			set
			{
				m_Offset = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000762C File Offset: 0x0000582C
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00007634 File Offset: 0x00005834
		public Vector2 flip
		{
			get
			{
				return m_Flip;
			}
			set
			{
				m_Flip = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000763D File Offset: 0x0000583D
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00007645 File Offset: 0x00005845
		public Material customMaterial
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600014F RID: 335 RVA: 0x0000764E File Offset: 0x0000584E
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00007656 File Offset: 0x00005856
		public bool useCustomAtlasTextureRect
		{
			get
			{
				return m_UseCustomAtlasTextureRect;
			}
			set
			{
				m_UseCustomAtlasTextureRect = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000765F File Offset: 0x0000585F
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00007667 File Offset: 0x00005867
		public Rect atlasTextureRect
		{
			get
			{
				return m_AtlasTextureRect;
			}
			set
			{
				m_AtlasTextureRect = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00007670 File Offset: 0x00005870
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00007678 File Offset: 0x00005878
		public Vector2 meshSizeMultiplier
		{
			get
			{
				return m_MeshSizeMultiplier;
			}
			set
			{
				m_MeshSizeMultiplier = value;
			}
		}

		// Token: 0x040000A3 RID: 163
		public static readonly Rect invalidRect = new Rect(0f, 0f, -1f, -1f);

		// Token: 0x040000A4 RID: 164
		[SerializeField]
		[HideInInspector]
		private string m_Name = string.Empty;

		// Token: 0x040000A5 RID: 165
		[HideInInspector]
		[SerializeField]
		private GAFBaseClip m_Clip;

		// Token: 0x040000A6 RID: 166
		[HideInInspector]
		[SerializeField]
		private GAFBaseObjectsManager m_Manager;

		// Token: 0x040000A7 RID: 167
		[HideInInspector]
		[SerializeField]
		private int m_ID = -1;

		// Token: 0x040000A8 RID: 168
		[SerializeField]
		[HideInInspector]
		private int m_AtlasCustomElementID = -1;

		// Token: 0x040000A9 RID: 169
		[HideInInspector]
		[SerializeField]
		private int m_AtlasInitialElementID = -1;

		// Token: 0x040000AA RID: 170
		[SerializeField]
		[GAFReadOnly(true)]
		private GAFObjectType m_ObjectType;

		// Token: 0x040000AB RID: 171
		[SerializeField]
		private ObjectBehaviourType m_BehaviourType;

		// Token: 0x040000AC RID: 172
		[SerializeField]
		[GAFReloadable("m_Data.m_Clip")]
		[Space(5f)]
		[GAFReadOnly(false)]
		private bool m_IsVisible = true;

		// Token: 0x040000AD RID: 173
		[Space(5f)]
		[GAFReloadable("m_Data.m_Clip")]
		[SerializeField]
		private Vector2 m_Offset = Vector3.zero;

		// Token: 0x040000AE RID: 174
		[SerializeField]
		[GAFReloadable("m_Data.m_Clip")]
		private Vector2 m_MeshSizeMultiplier = Vector2.one;

		// Token: 0x040000AF RID: 175
		[GAFReloadable("m_Data.m_Clip")]
		[SerializeField]
		[Space(5f)]
		private Material m_Material;

		// Token: 0x040000B0 RID: 176
		[GAFReloadable("m_Data.m_Clip")]
		[SerializeField]
		private bool m_UseCustomAtlasTextureRect;

		// Token: 0x040000B1 RID: 177
		[SerializeField]
		[GAFReloadable("m_Data.m_Clip")]
		private Rect m_AtlasTextureRect = invalidRect;

		// Token: 0x040000B2 RID: 178
		private Vector2 m_Flip = Vector3.zero;
	}
}
