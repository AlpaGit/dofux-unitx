
// File:			GAFBakedObject.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using GAF.Managed.GAFInternal.Data;
using GAF.Managed.GAFInternal.Objects;
using UnityEngine;

namespace GAF.Scripts.Objects
{
	[System.Serializable]
	public class GAFBakedObject : GAFBakedObjectInternal
	{
		#region Members

		[HideInInspector][SerializeField] private GAFBakedObjectController m_Controller = null;

		#endregion

		#region /////// GAFBakedObject ////////

		//---------------------Initialize--------------------------//
		// <summary>
		// Create and initialize serialized object data.
		// </summary>
		// <param name="_Name">Object name.</param>
		// <param name="_BehaviourType">Object type.</param>
		// <param name="_Clip">Clip associated with current object.</param>
		// <param name="_Manager">Objects manager associated with current object.</param>
		// <param name="_GAFData">Data loaded from .gaf file for current object.</param>

		//public void initialize(string _Name , ObjectBehaviourType _BehaviourType, GAFBaseClip _Clip, GAFBaseObjectsManager _Manager , GAFInternal.Data.GAFObjectData _GAFData)

		//-------------------------------------------------------//

		//---------------------Add controller--------------------------//
		/// <summary>
		/// Add controller to current object.
		/// </summary>
		public void addController()
		{
			if (!hasController())
			{
				var controllerObject = new GameObject(serializedProperties.name);
				controllerObject.transform.parent = serializedProperties.clip.transform;
				controllerObject.transform.localPosition = new Vector3(serializedProperties.offset.x, serializedProperties.offset.y, 0);

				m_Controller = controllerObject.AddComponent<GAFBakedObjectController>();
			}
		}

		//------------------------------------------------------------//

		//------------------------Has controller-------------------------------//
		/// <summary>
		/// Returns true if current object has controller.
		/// </summary>
		/// <returns></returns>
		public bool hasController()
		{
			return m_Controller != null;
		}

		//------------------------------------------------------------//

		//-------------------------Remove controller-------------------------------//
		/// <summary>
		/// Remove controller from current object.
		/// </summary>
		public void removeController()
		{
			if (hasController())
			{
				if (Application.isEditor)
					Object.DestroyImmediate(m_Controller.gameObject);
				else
					Object.Destroy(m_Controller.gameObject);

				m_Controller = null;
			}
		}

		//------------------------------------------------------------//

		#endregion /////// GAFBakedObject ////////

		#region /////// IGAFObject /////////

		//---------------------Reload--------------------------//
		/// <summary>
		/// Reset object state and create non-serialized data.
		/// </summary>
		public override void reload ()
		{
			base.reload ();

			if (hasController())
				m_Controller.init(this);
		}

		//----------------------------------------------------//

		//---------------------Update to state--------------------------//
		/// <summary>
		/// Update object to desired state.
		/// </summary>
		/// <param name="_State">State data.</param>
		/// <param name="_Refresh">Force refresh state.</param>
		public override void updateToState(GAFObjectStateData _State, bool _Refresh)
		{
			base.updateToState(_State, _Refresh);

			if (hasController())
				m_Controller.updateToState(currentMesh, currentMaterial);
		}

		//----------------------------------------------------------//

		//---------------------Cleanup--------------------------//
		// <summary>
		// Clean non-serialized data.
		// </summary>

		//public void cleanUp()

		//-------------------------------------------------------//

		#endregion /////// IGAFObject /////////

		#region ////// BASE PROPERTIES ////////

		//-----------------------Object ID----------------------------//
		// <summary>
		// Get the object identifier.
		// </summary>

		//public uint objectID { get; }

		//-------------------------------------------------------//

		//-----------------------Name-------------------------------//
		// <summary>
		// Get the name.
		// </summary>

		//public string name { get; }

		//-------------------------------------------------------//

		//-----------------------Type--------------------------------//
		// <summary>
		// Get the GAF type of object.
		// </summary>
		// <value>The type.</value>

		//public GAFObjectType type { get; }

		//-------------------------------------------------------//

		#endregion ////// BASE PROPERTIES ////////

		#region /////// PROPERTIES ////////

		//-----------------------Current state----------------------------//
		// <summary>
		// Information about current state of this object.
		// </summary>

		//public GAFObjectStateData currentState { get; }

		//-------------------------------------------------------//

		//-----------------------Previous state----------------------------//
		// <summary>
		// Information about previous state of this object.
		// </summary>

		//public GAFObjectStateData previousState { get; }

		//-------------------------------------------------------//

		//-----------------------Current material----------------------------//
		// <summary>
		// Material of this object.
		// </summary>

		//public Material currentMaterial { get; }

		//-------------------------------------------------------//

		//-----------------------Current mesh----------------------------//
		// <summary>
		// Mesh of this object.
		// </summary>

		//public Mesh currentMesh { get; }

		//-------------------------------------------------------//

		//-----------------------Serialized properties----------------------------//
		// <summary>
		// Serialized data of this object.
		// </summary>

		//public GAFObjectData serializedProperties { get; }

		//-------------------------------------------------------//

		#endregion /////// PROPERTIES ////////
	}
}