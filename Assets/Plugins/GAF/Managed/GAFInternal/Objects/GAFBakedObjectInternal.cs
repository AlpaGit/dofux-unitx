using System;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	public class GAFBakedObjectInternal : IGAFObject, IGAFBaseObject, IEquatable<IGAFObject>
	{
		/// <summary>
		/// Create and initialize serialized object data.
		/// </summary>
		/// <param name="_Name">Object name.</param>
		/// <param name="_BehaviourType">Behaviour type of object.</param>
		/// <param name="_Clip">Clip associated with current object.</param>
		/// <param name="_Manager">Objects manager associated with current object.</param>
		/// <param name="_GAFData">Data loaded from .gaf file for current object.</param>
		// Token: 0x0600012C RID: 300 RVA: 0x000072B6 File Offset: 0x000054B6
		public virtual void initialize(string _Name, ObjectBehaviourType _BehaviourType, GAFBaseClip _Clip, GAFBaseObjectsManager _Manager, GAFInternal.Data.GAFObjectData _GAFData)
		{
			m_Data = new GAFObjectData(_Name, _BehaviourType, _Clip, _Manager, _GAFData);
		}

		/// <summary>
		/// Reset object state and create non-serialized data.
		/// </summary>
		// Token: 0x0600012D RID: 301 RVA: 0x000072CA File Offset: 0x000054CA
		public virtual void reload()
		{
			cleanUp();
			m_Impl = GAFBakedObjectImplsFactory.getImpl(m_Data, serializedProperties.clip.GetComponent<GAFMeshManager>());
			m_Impl.initialize();
		}

		/// <summary>
		/// Update object to desired state.
		/// </summary>
		/// <param name="_State">State data.</param>
		/// <param name="_Refresh">Force refresh state.</param>
		// Token: 0x0600012E RID: 302 RVA: 0x000072FE File Offset: 0x000054FE
		public virtual void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			if (m_Impl != null)
			{
				m_Impl.updateToState(_State, _Refresh);
			}
		}

		/// <summary>
		/// Clean non-serialized data.
		/// </summary>
		// Token: 0x0600012F RID: 303 RVA: 0x00007315 File Offset: 0x00005515
		public virtual void cleanUp()
		{
			if (m_Impl != null)
			{
				m_Impl.cleanUp();
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000130 RID: 304 RVA: 0x0000732A File Offset: 0x0000552A
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
		/// <value>The object identifier.</value>
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00007332 File Offset: 0x00005532
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
		/// <value>The name.</value>
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000733F File Offset: 0x0000553F
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
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000133 RID: 307 RVA: 0x0000734C File Offset: 0x0000554C
		public GAFObjectType type
		{
			get
			{
				return serializedProperties.type;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00007359 File Offset: 0x00005559
		public bool Equals(IGAFObject _Other)
		{
			return objectID == _Other.objectID;
		}

		/// <summary>
		/// Information about current state of this object.
		/// </summary>
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000736C File Offset: 0x0000556C
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
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00007390 File Offset: 0x00005590
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
		/// Gets the current material.
		/// </summary>
		/// <value>The current material.</value>
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000137 RID: 311 RVA: 0x000073B4 File Offset: 0x000055B4
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
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000138 RID: 312 RVA: 0x000073D8 File Offset: 0x000055D8
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
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000073FC File Offset: 0x000055FC
		public GAFObjectData serializedProperties
		{
			get
			{
				return m_Data;
			}
		}

		// Token: 0x040000A1 RID: 161
		[SerializeField]
		private GAFObjectData m_Data;

		// Token: 0x040000A2 RID: 162
		[HideInInspector]
		[NonSerialized]
		private GAFBakedObjectImpl m_Impl;
	}
}
