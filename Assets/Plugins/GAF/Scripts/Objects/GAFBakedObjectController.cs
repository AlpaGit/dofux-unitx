
// File:			GAFBakedObjectController.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using UnityEngine;

namespace GAF.Scripts.Objects
{
	[System.Serializable]
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	//[RequireComponent(typeof(MeshCollider))]
	public class GAFBakedObjectController : MonoBehaviour
	{
		#region Members

		//private MeshCollider m_Collider = null;

		#endregion // Members

		#region Interface

		/// <summary>
		/// Object associated with current controller.
		/// </summary>
		public GAFBakedObject bakedObject
		{
			get;
			private set;
		}

		/// <summary>
		/// Initialize controller data.
		/// </summary>
		/// <param name="_Object">Object associated with current controller.</param>
		public void init(GAFBakedObject _Object)
		{
			bakedObject = _Object;
		}

		/// <summary>
		/// Update current object
		/// </summary>
		/// <param name="_Mesh"></param>
		/// <param name="_Material"></param>
		public void updateToState(Mesh _Mesh, Material _Material)
		{
			//collider.sharedMesh = null;
			//collider.sharedMesh = _Mesh;
		}

		#endregion // Interface

		#region Implementation

//		new private MeshCollider collider
//		{
//			get 
//			{
//				if (m_Collider == null)
//					m_Collider = GetComponent<MeshCollider>();
//
//				return m_Collider;
//			}
//		}

		#endregion // Implementation
	}
}
