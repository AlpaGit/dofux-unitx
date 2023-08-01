using System;
using System.Collections.Generic;
using GAF.Managed.GAFInternal.Core;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000047 RID: 71
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[AddComponentMenu("")]
	public class GAFMeshManager : GAFBehaviour
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600017F RID: 383 RVA: 0x000079AC File Offset: 0x00005BAC
		// (remove) Token: 0x06000180 RID: 384 RVA: 0x000079E4 File Offset: 0x00005BE4
		public event Action onWillRenderObject;

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00007A19 File Offset: 0x00005C19
		private List<GAFBakedObjectImpl> sortedObjects
		{
			get
			{
				if (m_SortedObjects == null)
				{
					m_SortedObjects = new List<GAFBakedObjectImpl>();
				}
				return m_SortedObjects;
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007A34 File Offset: 0x00005C34
		public void initialize()
		{
			m_Clip = GetComponent<GAFBaseClip>();
			m_ObjectsCount = m_Clip.getObjectsCount();
			m_ObjectsCount += m_Clip.asset.getMasks(m_Clip.timelineID).Count;
			m_MeshData.initialize(m_ObjectsCount);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007A9C File Offset: 0x00005C9C
		public void reload()
		{
			if (m_Clip == null)
			{
				m_Clip = GetComponent<GAFBaseClip>();
			}
			m_ObjectsCount = m_Clip.getObjectsCount();
			m_ObjectsCount += m_Clip.asset.getMasks(m_Clip.timelineID).Count;
			if (!m_MeshData.isInitialized)
			{
				m_MeshData.initialize(m_ObjectsCount);
			}
			cachedRenderer.shadowCastingMode = 0;
			cachedRenderer.receiveShadows = false;
			cachedRenderer.useLightProbes = false;
			cachedRenderer.sortingLayerName = m_Clip.settings.spriteLayerName;
			cachedRenderer.sortingOrder = m_Clip.settings.spriteLayerValue;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007B7C File Offset: 0x00005D7C
		public void updateToState()
		{
			if (isStateSet(MeshState.Sort))
			{
				sortedObjects.Clear();
				foreach (var keyValuePair in m_Objects)
				{
					if (keyValuePair.Value.isVisible)
					{
						sortedObjects.Add(keyValuePair.Value);
					}
				}
				sortedObjects.Sort();
			}
			if (isStateSet(MeshState.Setup))
			{
				if (cachedFilter.sharedMesh == null)
				{
					cachedFilter.sharedMesh = new Mesh();
					cachedFilter.sharedMesh.MarkDynamic();
				}
				else
				{
					cachedFilter.sharedMesh.Clear();
				}
				m_MeshData.clear();
				if (m_MeshData.restObjects.Count > 0)
				{
					m_MeshData.restObjects.Clear();
				}
				for (var i = 0; i < sortedObjects.Count; i++)
				{
					m_MeshData.restObjects.Enqueue(sortedObjects[i]);
				}
				while (m_MeshData.restObjects.Count > 0)
				{
					m_MeshData.restObjects.Peek().combineMeshes(ref m_MeshData);
				}
				if (MeshData.triangles.Count < m_MeshData.objectsCount)
				{
					MeshData.incrementTrianglesCount(m_MeshData.objectsCount - MeshData.triangles.Count);
				}
				if (m_Clip.settings.useLights)
				{
					var num = m_MeshData.objectsCount * 4;
					if (MeshData.normals.Count < num)
					{
						MeshData.incrementNormalsCount(num - MeshData.normals.Count);
					}
					var num2 = m_MeshData.vertices.Length;
					var array = new Vector3[num2];
					for (var j = 0; j < num2; j++)
					{
						array[j] = MeshData.normals[j];
					}
					cachedFilter.sharedMesh.normals = array;
				}
				cachedFilter.sharedMesh.subMeshCount = m_MeshData.actualObjectsCount;
				cachedFilter.sharedMesh.vertices = m_MeshData.vertices;
				cachedFilter.sharedMesh.uv = m_MeshData.uv;
				cachedFilter.sharedMesh.uv2 = m_MeshData.colorRG;
				cachedFilter.sharedMesh.uv3 = m_MeshData.colorBA;
				cachedFilter.sharedMesh.colors32 = m_MeshData.colors32;
				for (var k = 0; k < m_MeshData.actualObjectsCount; k++)
				{
					cachedFilter.sharedMesh.SetTriangles(MeshData.triangles[k], k);
				}
				cachedRenderer.sharedMaterials = m_MeshData.materials;
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007E98 File Offset: 0x00006098
		internal void registerSubObject(uint _ID, GAFBakedObjectImpl _Object)
		{
			m_Objects.Add(_ID, _Object);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00007EA7 File Offset: 0x000060A7
		internal void unregisterSubObject(uint _ID)
		{
			m_Objects.Remove(_ID);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007EB6 File Offset: 0x000060B6
		public void clear()
		{
			m_SortedObjects = null;
			m_Objects.Clear();
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007ECA File Offset: 0x000060CA
		public void pushSortRequest()
		{
			m_State |= MeshState.Sort;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00007EDA File Offset: 0x000060DA
		public void pushSetupRequest()
		{
			m_State |= MeshState.Setup;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00007EEA File Offset: 0x000060EA
		private void OnWillRenderObject()
		{
			if (onWillRenderObject != null)
			{
				onWillRenderObject();
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00007EFF File Offset: 0x000060FF
		private bool isStateSet(MeshState _State)
		{
			return (m_State & _State) == _State;
		}

		// Token: 0x040000BF RID: 191
		[SerializeField]
		[HideInInspector]
		private GAFBaseClip m_Clip;

		// Token: 0x040000C0 RID: 192
		private MeshState m_State;

		// Token: 0x040000C1 RID: 193
		private MeshData m_MeshData = new MeshData();

		// Token: 0x040000C2 RID: 194
		private Dictionary<uint, GAFBakedObjectImpl> m_Objects = new Dictionary<uint, GAFBakedObjectImpl>();

		// Token: 0x040000C3 RID: 195
		private List<GAFBakedObjectImpl> m_SortedObjects;

		// Token: 0x040000C4 RID: 196
		private int m_ObjectsCount;

		// Token: 0x02000048 RID: 72
		[Flags]
		private enum MeshState
		{
			// Token: 0x040000C6 RID: 198
			Null = 0,
			// Token: 0x040000C7 RID: 199
			Setup = 1,
			// Token: 0x040000C8 RID: 200
			Sort = 2
		}

		// Token: 0x02000049 RID: 73
		public class MeshData
		{
			// Token: 0x17000066 RID: 102
			// (get) Token: 0x0600018D RID: 397 RVA: 0x00007F2A File Offset: 0x0000612A
			// (set) Token: 0x0600018E RID: 398 RVA: 0x00007F31 File Offset: 0x00006131
			public static List<Vector3> normals
			{
				get
				{
					return m_Normals;
				}
				set
				{
					m_Normals = value;
				}
			}

			// Token: 0x17000067 RID: 103
			// (get) Token: 0x0600018F RID: 399 RVA: 0x00007F39 File Offset: 0x00006139
			public static List<int[]> triangles
			{
				get
				{
					return m_Triangles;
				}
			}

			// Token: 0x06000190 RID: 400 RVA: 0x00007F40 File Offset: 0x00006140
			public static void incrementNormalsCount(int _Count)
			{
				for (var i = 0; i < _Count; i++)
				{
					normals.Add(IGAFObjectImpl.normal);
				}
			}

			// Token: 0x06000191 RID: 401 RVA: 0x00007F68 File Offset: 0x00006168
			public static void incrementTrianglesCount(int _Count)
			{
				var num = triangles.Count + _Count;
				for (var i = triangles.Count; i < num; i++)
				{
					var num2 = i * 4;
					triangles.Add(new int[]
					{
						2 + num2,
						num2,
						1 + num2,
						3 + num2,
						num2,
						2 + num2
					});
				}
			}

			// Token: 0x06000192 RID: 402 RVA: 0x00007FCC File Offset: 0x000061CC
			public void initialize(int _ObjectsCount)
			{
				objectsCount = _ObjectsCount;
				var num = objectsCount * 4;
				m_Vertices = new Vector3[num];
				m_UV = new Vector2[num];
				m_Colors32 = new Color32[num];
				m_ColorRG = new Vector2[num];
				m_ColorBA = new Vector2[num];
			}

			// Token: 0x06000193 RID: 403 RVA: 0x00008028 File Offset: 0x00006228
			public void pushData(IGAFObjectImpl _Object, Material _Material)
			{
				var startIndex = m_ActualObjectsCount * 4;
				addVertices(_Object.tempVertices, startIndex);
				addUV(_Object.tempUV, startIndex);
				addColors(_Object.tempColors, startIndex);
				addColorOffsets(_Object.tempUV2, _Object.tempUV3, startIndex);
				addMaterial(_Material, m_ActualObjectsCount);
				m_ActualObjectsCount++;
			}

			// Token: 0x06000194 RID: 404 RVA: 0x00008094 File Offset: 0x00006294
			public void pushData(Mesh _Object, Material _Material)
			{
				if (_Object == null)
				{
					return;
				}
				var startIndex = m_ActualObjectsCount * 4;
				addVertices(_Object.vertices, startIndex);
				addUV(_Object.uv, startIndex);
				addColors(_Object.colors32, startIndex);
				addColorOffsets(_Object.uv2, _Object.uv3, startIndex);
				addMaterial(_Material, m_ActualObjectsCount);
				m_ActualObjectsCount++;
			}

			// Token: 0x06000195 RID: 405 RVA: 0x00008109 File Offset: 0x00006309
			public void clear()
			{
				m_Normals.Clear();
				m_Materials.Clear();
				m_ActualObjectsCount = 0;
			}

			// Token: 0x06000196 RID: 406 RVA: 0x00008128 File Offset: 0x00006328
			private void addVertices(Vector3[] _Vertices, int _StartIndex)
			{
				for (var i = 0; i < _Vertices.Length; i++)
				{
					m_Vertices[_StartIndex + i] = _Vertices[i];
				}
			}

			// Token: 0x06000197 RID: 407 RVA: 0x00008158 File Offset: 0x00006358
			private void addColors(Color32[] _Colors, int _StartIndex)
			{
				for (var i = 0; i < _Colors.Length; i++)
				{
					m_Colors32[_StartIndex + i] = _Colors[i];
				}
			}

			// Token: 0x06000198 RID: 408 RVA: 0x00008188 File Offset: 0x00006388
			private void addUV(Vector2[] _UV, int _StartIndex)
			{
				for (var i = 0; i < _UV.Length; i++)
				{
					m_UV[_StartIndex + i] = _UV[i];
				}
			}

			// Token: 0x06000199 RID: 409 RVA: 0x000081B8 File Offset: 0x000063B8
			private void addColorOffsets(Vector2[] _ColorRG, Vector2[] _ColorBA, int _StartIndex)
			{
				for (var i = 0; i < _ColorRG.Length; i++)
				{
					m_ColorRG[_StartIndex + i] = _ColorRG[i];
					m_ColorBA[_StartIndex + i] = _ColorBA[i];
				}
			}

			// Token: 0x0600019A RID: 410 RVA: 0x000081FD File Offset: 0x000063FD
			private void addMaterial(Material _Material, int _StartIndex)
			{
				m_Materials.Add(_Material);
			}

			// Token: 0x17000068 RID: 104
			// (get) Token: 0x0600019B RID: 411 RVA: 0x0000820B File Offset: 0x0000640B
			// (set) Token: 0x0600019C RID: 412 RVA: 0x00008213 File Offset: 0x00006413
			public int objectsCount
			{
				get
				{
					return m_ObjectsCount;
				}
				set
				{
					m_ObjectsCount = value;
				}
			}

			// Token: 0x17000069 RID: 105
			// (get) Token: 0x0600019D RID: 413 RVA: 0x0000821C File Offset: 0x0000641C
			// (set) Token: 0x0600019E RID: 414 RVA: 0x00008224 File Offset: 0x00006424
			internal Queue<GAFBakedObjectImpl> restObjects
			{
				get
				{
					return m_Rest;
				}
				set
				{
					m_Rest = value;
				}
			}

			// Token: 0x1700006A RID: 106
			// (get) Token: 0x0600019F RID: 415 RVA: 0x0000822D File Offset: 0x0000642D
			public bool isInitialized
			{
				get
				{
					return m_Vertices != null && m_Vertices.Length != 0;
				}
			}

			// Token: 0x1700006B RID: 107
			// (get) Token: 0x060001A0 RID: 416 RVA: 0x00008243 File Offset: 0x00006443
			public int actualObjectsCount
			{
				get
				{
					return m_ActualObjectsCount;
				}
			}

			// Token: 0x1700006C RID: 108
			// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000824B File Offset: 0x0000644B
			// (set) Token: 0x060001A2 RID: 418 RVA: 0x00008253 File Offset: 0x00006453
			public Vector3[] vertices
			{
				get
				{
					return m_Vertices;
				}
				internal set
				{
					m_Vertices = value;
				}
			}

			// Token: 0x1700006D RID: 109
			// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000825C File Offset: 0x0000645C
			// (set) Token: 0x060001A4 RID: 420 RVA: 0x00008264 File Offset: 0x00006464
			public Vector2[] uv
			{
				get
				{
					return m_UV;
				}
				internal set
				{
					m_UV = value;
				}
			}

			// Token: 0x1700006E RID: 110
			// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000826D File Offset: 0x0000646D
			// (set) Token: 0x060001A6 RID: 422 RVA: 0x00008275 File Offset: 0x00006475
			public Vector2[] colorRG
			{
				get
				{
					return m_ColorRG;
				}
				internal set
				{
					m_ColorRG = value;
				}
			}

			// Token: 0x1700006F RID: 111
			// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000827E File Offset: 0x0000647E
			// (set) Token: 0x060001A8 RID: 424 RVA: 0x00008286 File Offset: 0x00006486
			public Vector2[] colorBA
			{
				get
				{
					return m_ColorBA;
				}
				internal set
				{
					m_ColorBA = value;
				}
			}

			// Token: 0x17000070 RID: 112
			// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000828F File Offset: 0x0000648F
			// (set) Token: 0x060001AA RID: 426 RVA: 0x00008297 File Offset: 0x00006497
			public Color32[] colors32
			{
				get
				{
					return m_Colors32;
				}
				internal set
				{
					m_Colors32 = value;
				}
			}

			// Token: 0x17000071 RID: 113
			// (get) Token: 0x060001AB RID: 427 RVA: 0x000082A0 File Offset: 0x000064A0
			public Material[] materials
			{
				get
				{
					return m_Materials.ToArray();
				}
			}

			// Token: 0x040000C9 RID: 201
			private static List<Vector3> m_Normals = new List<Vector3>();

			// Token: 0x040000CA RID: 202
			private static List<int[]> m_Triangles = new List<int[]>();

			// Token: 0x040000CB RID: 203
			private int m_ActualObjectsCount;

			// Token: 0x040000CC RID: 204
			private Queue<GAFBakedObjectImpl> m_Rest = new Queue<GAFBakedObjectImpl>();

			// Token: 0x040000CD RID: 205
			private Vector3[] m_Vertices;

			// Token: 0x040000CE RID: 206
			private Vector2[] m_UV;

			// Token: 0x040000CF RID: 207
			private Color32[] m_Colors32;

			// Token: 0x040000D0 RID: 208
			private Vector2[] m_ColorRG;

			// Token: 0x040000D1 RID: 209
			private Vector2[] m_ColorBA;

			// Token: 0x040000D2 RID: 210
			private List<Material> m_Materials = new List<Material>();

			// Token: 0x040000D3 RID: 211
			private int m_ObjectsCount;
		}
	}
}
