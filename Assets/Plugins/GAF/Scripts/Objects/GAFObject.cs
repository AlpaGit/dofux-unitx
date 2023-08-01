
// File:			GAFObject.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using GAF.Managed.GAFInternal.Objects;
using UnityEngine;

namespace GAF.Scripts.Objects
{
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class GAFObject : GAFObjectInternal
	{
		#region /////// GAFObject ////////

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

		#endregion /////// GAFObject ////////

		#region /////// IGAFObject /////////

		//---------------------Reload--------------------------//
		/// <summary>
		/// Reset object state and create non-serialized data.
		/// </summary>

		public override void reload()
		{
			cachedRenderer.useLightProbes = false;
			cachedRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
			cachedRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			cachedRenderer.receiveShadows = false;
			base.reload();
		}

		//-------------------------------------------------------//

		//---------------------Update to state--------------------------//
		// <summary>
		// Update object to desired state.
		// </summary>
		// <param name="_State">State data.</param>
		// <param name="_Refresh">Force refresh state.</param>

		//public void updateToState(GAFObjectStateData _State, bool _Refresh);)

		//-------------------------------------------------------//

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

		//-----------------------GAF transform----------------------------//
		// <summary>
		// GAF transformation component.
		// <para />Contains redundant data for nested clips transforms.
		// </summary>

		//public GAFTransform gafTransform { get; }

		//-------------------------------------------------------//

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