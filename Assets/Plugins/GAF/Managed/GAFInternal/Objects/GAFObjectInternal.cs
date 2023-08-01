using System;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000041 RID: 65
	[RequireComponent(typeof(GAFTransform))]
	[ExecuteInEditMode]
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	public class GAFObjectInternal : GAFBehaviour, IGAFObject, IGAFBaseObject, IEquatable<IGAFObject>
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000156 RID: 342 RVA: 0x000076A4 File Offset: 0x000058A4
		// (remove) Token: 0x06000157 RID: 343 RVA: 0x000076DC File Offset: 0x000058DC
		internal event Action onWillRenderObject;

		/// <summary>
		/// Create and initialize serialized object data.
		/// </summary>
		/// <param name="_Name">Object name.</param>
		/// <param name="_BehaviourType">Behaviour type of object.</param>
		/// <param name="_Clip">Clip associated with current object.</param>
		/// <param name="_Manager">Objects manager associated with current object.</param>
		/// <param name="_GAFData">Data loaded from .gaf file for current object.</param>
		// Token: 0x06000158 RID: 344 RVA: 0x00007711 File Offset: 0x00005911
		public void initialize(string _Name, ObjectBehaviourType _BehaviourType, GAFBaseClip _Clip, GAFBaseObjectsManager _Manager, GAFInternal.Data.GAFObjectData _GAFData)
		{
			m_Data = new GAFObjectData(_Name, _BehaviourType, _Clip, _Manager, _GAFData);
		}

		/// <summary>
		/// Reset object state and create non-serialized data.
		/// </summary>
		// Token: 0x06000159 RID: 345 RVA: 0x00007728 File Offset: 0x00005928
		public virtual void reload()
		{
			if (m_Impl != null)
			{
				m_Impl.cleanUp();
			}
			var gafsortingManager = serializedProperties.clip.GetComponent<GAFSortingManager>();
			if (gafsortingManager == null)
			{
				gafsortingManager = serializedProperties.clip.gameObject.AddComponent<GAFSortingManager>();
			}
			m_Impl = GAFObjectImplsFactory.getImpl(this, gafsortingManager, m_Data);
			m_Impl.initialize();
		}

		/// <summary>
		/// Update object to desired state.
		/// </summary>
		/// <param name="_State">State data.</param>
		/// <param name="_Refresh">Force refresh state.</param>
		// Token: 0x0600015A RID: 346 RVA: 0x00007796 File Offset: 0x00005996
		public void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			if (m_Impl != null)
			{
				m_Impl.updateToState(_State, _Refresh);
			}
		}

		/// <summary>
		/// Clean non-serialized data.
		/// </summary>
		// Token: 0x0600015B RID: 347 RVA: 0x000077AD File Offset: 0x000059AD
		public void cleanUp()
		{
			if (m_Impl != null)
			{
				m_Impl.cleanUp();
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000077C2 File Offset: 0x000059C2
		public IGAFObjectImpl impl
		{
			get
			{
				return m_Impl;
			}
		}

		/// <summary>
		/// Get the object identifier.
		/// </summary>
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000077CA File Offset: 0x000059CA
		public uint objectID
		{
			get
			{
				return serializedProperties.objectID;
			}
		}

		/// <summary>
		/// Get the name.
		/// </summary>
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000077D7 File Offset: 0x000059D7
		public string name
		{
			get
			{
				return serializedProperties.name;
			}
		}

		/// <summary>
		/// Get the GAF type of object.
		/// </summary>
		/// <value>The type.</value>
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600015F RID: 351 RVA: 0x000077E4 File Offset: 0x000059E4
		public GAFObjectType type
		{
			get
			{
				return serializedProperties.type;
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000077F1 File Offset: 0x000059F1
		public bool Equals(IGAFObject _Other)
		{
			return objectID == _Other.objectID;
		}

		/// <summary>
		/// Information about current state of this object.
		/// </summary>
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00007804 File Offset: 0x00005A04
		public GAFObjectStateData currentState
		{
			get
			{
				GAFObjectStateData result = null;
				if (m_Impl != null)
				{
					result = m_Impl.currentState;
				}
				return result;
			}
		}

		/// <summary>
		/// Information about previous state of this object.
		/// </summary>
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00007828 File Offset: 0x00005A28
		public GAFObjectStateData previousState
		{
			get
			{
				GAFObjectStateData result = null;
				if (m_Impl != null)
				{
					result = m_Impl.previousState;
				}
				return result;
			}
		}

		/// <summary>
		/// Material of this object.
		/// </summary>
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000784C File Offset: 0x00005A4C
		public Material currentMaterial
		{
			get
			{
				Material result = null;
				if (m_Impl != null)
				{
					result = m_Impl.currentMaterial;
				}
				return result;
			}
		}

		/// <summary>
		/// Mesh of this object.
		/// </summary>
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00007870 File Offset: 0x00005A70
		public Mesh currentMesh
		{
			get
			{
				Mesh result = null;
				if (m_Impl != null)
				{
					result = m_Impl.currentMesh;
				}
				return result;
			}
		}

		/// <summary>
		/// Serialized data of this object.
		/// </summary>
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00007894 File Offset: 0x00005A94
		public GAFObjectData serializedProperties
		{
			get
			{
				return m_Data;
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000789C File Offset: 0x00005A9C
		public void setCustomRegion(GAFAtlasElementData _CustomRegion)
		{
			if (_CustomRegion != null)
			{
				serializedProperties.atlasCustomElementID = (int)_CustomRegion.id;
				reload();
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000078B8 File Offset: 0x00005AB8
		private void OnWillRenderObject()
		{
			if (onWillRenderObject != null)
			{
				onWillRenderObject();
			}
		}

		// Token: 0x040000B4 RID: 180
		[SerializeField]
		private GAFObjectData m_Data;

		// Token: 0x040000B5 RID: 181
		[HideInInspector]
		[NonSerialized]
		private GAFObjectImpl m_Impl;
	}
}
