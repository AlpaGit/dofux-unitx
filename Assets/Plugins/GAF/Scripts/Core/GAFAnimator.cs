
// File:			GAFAnimator.cs
// Version:			5.2
// Last changed:	2017/3/28 14:41
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using System.Collections.Generic;
using GAF.Managed.GAFInternal.Data;
using GAF.Managed.GAFInternal.Objects;
using GAF.Scripts.Core.Interfaces;
using GAF.Scripts.Objects;
using UnityEngine;

namespace GAF.Scripts.Core
{
	[AddComponentMenu("GAF/GAFAnimator")]
	[RequireComponent(typeof(GAFObjectsManager))]
	[ExecuteInEditMode]
	public class GAFAnimator : GAFAnimatorInternal<GAFObjectsManager>
	{
		#region /////// GAFBaseClip ///////

		//----------------------Initialize---------------------------------//
		// <summary>
		// Initialize animation clip with it's root timeline. Crashes if root timeline is not present.
		// <para/>First operation that has to be performed to work with GAF playback.
		// <para/>Creating serializable data e.g. game objects of animation children.
		// </summary>
		// <param name="_Asset">Animation asset file.</param>

		//public virtual void initialize(GAFAnimationAssetInternal _Asset)

		//-------------------------------------------------------//

		//----------------------Initialize (overload)---------------------------------//
		// <summary>
		// Initialize animation clip.
		// <para/>First operation that has to be performed to work with GAF playback.
		// <para/>Creating serializable data e.g. game objects of animation children.
		// </summary>
		// <param name="_Asset">Animation asset file.</param>
		// <param name="_TimelineID">ID of timeline.</param>

		//public override void initialize(GAFAnimationAssetInternal _Asset, int _TimelineID)

		//-------------------------------------------------------//

		//---------------------Reload--------------------------//
		/// <summary>
		/// Reload animation view.
		/// <para />Second operation that has to be performed to work with GAF playback.
		/// <para />Creating non-serializable data e.g. meshes, runtime materials.
		/// <para />Updating animation view to a currently selected frame.
		/// <para />Should be called every time when animation view has been changed e.g. setting animation color.
		/// </summary>
		public override void reload()
		{
			GAFCustomDelegateCreator.Init();

			base.reload();
		}

		protected override void setState(ref GAFObjectStateData _State, Dictionary<uint, IGAFObject> _Objects)
		{
			if (settings.cacheStates)
			{
				_State.vertices = new Vector3[4];

				var matrix = Matrix4x4.identity;
				var obj = _Objects[_State.id].impl;

				matrix[0, 0] = _State.a;
				matrix[0, 1] = _State.c;
				matrix[1, 0] = _State.b;
				matrix[1, 1] = _State.d;

				for (var j = 0; j < obj.initialVertices.Length; j++)
					_State.vertices[j] = matrix.MultiplyPoint3x4(obj.initialVertices[j]);
			}
			else
			{
				_State.vertices = null;
			}
		}

		//-------------------------------------------------------//

		//-------------------Get object------------------------------//
		// <summary>
		// Getting reference to an object by ID
		// </summary>
		// <param name="_ID">ID of animation suboject e.g "2_27" - "27" is object ID.</param>
		// <returns>IGAFObject.</returns>

		//public abstract IGAFObject getObject (uint _ID);

		//-------------------------------------------------------//

		//-------------------Get object (overload)------------------------------//
		// <summary>
		// Get object by name. If your object has custom name in inspector you can get it here.
		// </summary>
		// <param name="_PartName">Name of part.</param>
		// <returns></returns>

		//public virtual IGAFObject getObject(string _PartName)

		//-------------------------------------------------------//

		//---------------Object ID to part name------------------//
		// <summary>
		// Get object name by it's ID.
		// </summary>
		// <param name="_ID">ID of object.</param>
		// <returns></returns>

		//public string objectIDToPartName(uint _ID)

		//-------------------------------------------------------//

		//---------------Part name to object ID------------------//
		// <summary>
		// Get object id by name.
		// </summary>
		// <param name="_PartName">Named part name.</param>
		// <returns></returns>

		//public uint partNameToObjectID(string _PartName)

		//-------------------------------------------------------//

		//--------------------Get objects count------------------------------//
		// <summary>
		// Get number of animation objects
		// </summary>
		// <returns>System.Int32.</returns>

		//public override int getObjectsCount()

		//-------------------------------------------------------//

		//---------------------Clear--------------------------//
		// <summary>
		// Clear all information about objects in animation.
		// <para/>Moves animation to not initialized state.
		// </summary>
		// <param name="destroyChildren">Destroy game objects from scene.</param>

		//public void clear(bool destroyChildren = false)

		//-------------------------------------------------------//

		//---------------------Clean view--------------------------//
		// <summary>
		// Clean up animation non-serialized data.
		// <para/>Calling reload resets animation state.
		// </summary>

		//public void cleanView()

		//-------------------------------------------------------//

		//---------------------Get shared material--------------------------//
		// <summary>
		// Returns reference to a shared material
		// </summary>
		// <param name="_Name">Name of material (without .mat)</param>
		// <returns></returns>

		//public Material getSharedMaterial(string _Name)

		//-------------------------------------------------------//

		//------------------Register external material---------------------//
		// <summary>
		// Registering material as one that is used in this clip
		// </summary>
		// <param name="_Material">Material to register</param>

		//public void registerExternalMaterial(Material _Material)

		//-------------------------------------------------------//

		#endregion /////// GAFBaseClip ///////

		#region /////// GAFAnimator ////////

		//-----------------------Update to frame animator--------------------------------//
		// <summary>
		// Updates to frame animator.
		// </summary>
		// <param name="_FrameNumber">The frame number.</param>

		//public void updateToFrameAnimator(int _FrameNumber)

		//-------------------------------------------------------//

		//------------------Update to frame animator with refresh-----------------------//

		// <summary>
		// Updates to frame animator with refresh.
		// </summary>
		// <param name="_FrameNumber">The frame number.</param>

		//public void updateToFrameAnimatorWithRefresh(int _FrameNumber)

		//-------------------------------------------------------//

		#endregion /////// GAFAnimator ///////

		#region /////// BASE PROPERTIES ////////

		//----------------------Asset---------------------------------//
		// <summary>
		// Reference to asset file used in this clip.
		// </summary>

		//public GAFAnimationAssetInternal asset { get; protected set; }

		//-----------------------------------------------------------//

		//-----------------------Timeline ID-----------------------------//
		// <summary>
		// Timeline ID of this clip.
		// </summary>

		//public int timelineID { get; protected set; }

		//-----------------------------------------------------------//

		//-------------------------Is initialized-------------------------//
		// <summary>
		// Returns true if this clip already initialized.
		// </summary>

		//public bool isInitialized { get; protected set; }

		//-----------------------------------------------------------//

		//--------------------------Is loaded---------------------------------//
		// <summary>
		// Returns true if clip is ready to be played.
		// </summary>

		//public bool isLoaded { get; protected set; }

		//-----------------------------------------------------------//

		//-------------------------GAF transform--------------------------//
		// <summary>
		// GAF transformation component.
		// <para />Contains redundant data for nested clips transforms.
		// </summary>

		//public GAFTransform gafTransform { get; }

		//-----------------------------------------------------------//

		//-------------------------Settings----------------------------------//
		// <summary>
		// Returns settings of current clip.
		// </summary>

		//public GAFAnimationPlayerSettings settings { get; set; }

		//-----------------------------------------------------------//

		//-----------------------Current frame number-----------------------------//
		// <summary>
		// Returns current frame number.
		// </summary>

		//public uint currentFrameNumber { get; protected set; }

		//-----------------------------------------------------------//

		//-------------------------Resource----------------------------------//
		// <summary>
		// Returns reference to resource file of current animation.
		// </summary>

		//public GAFTexturesResourceInternal resource { get; set; }

		//-----------------------------------------------------------//

		//--------------------------Use custom delegate---------------------------------//
		// <summary>
		// Returns true if current clip uses custom method for getting resources.
		// </summary>

		//public bool useCustomDelegate { get; set; }

		//-----------------------------------------------------------//

		//-------------------------Custom get resource delegate--------------------//
		// <summary>
		// Returns delegate that contains custom method for getting resources.
		// </summary>

		//public System.Func<string, GAFTexturesResourceInternal> customGetResourceDelegate { get; set; }

		//-----------------------------------------------------------//

		//----------------------------Use placeholder-------------------------------//
		// <summary>
		// Returns true if animation will use placeholder texture
		// <para />instead of animation resources or during resources loading.
		// </summary>

		//public bool usePlaceholder { get; set; }

		//-----------------------------------------------------------//

		//-------------------------Placeholder size----------------------------------//
		// <summary>
		// Returns placeholder size.
		// </summary>

		//public Vector2 placeholderSize { get; set; }

		//-----------------------------------------------------------//

		//--------------------------Placeholder offset---------------------------------//
		// <summary>
		// Returns offset of placeholder position.
		// </summary>

		//public Vector2 placeholderOffset { get; set; }

		//-----------------------------------------------------------//

		#endregion /////// PROPERTIES ////////

		#region //////// PROPERTIES ///////

		//------------------------Is active-------------------------------//
		// <summary>
		// Gets a value indicating whether this instance is active.
		// </summary>
		// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>

		//public bool isActive { get; }

		//-------------------------------------------------------//

		#endregion //////// PROPERTIES ///////
	}
}
