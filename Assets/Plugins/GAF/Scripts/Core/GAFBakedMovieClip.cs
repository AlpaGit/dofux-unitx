
// File:			GAFBakedMovieClip.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
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
	[AddComponentMenu("GAF/GAFBakedMovieClip")]
	[RequireComponent(typeof(GAFBakedObjectsManager))]
	[ExecuteInEditMode]
	public class GAFBakedMovieClip : GAFMovieClipInternal<GAFBakedObjectsManager>
	{
		#region /////// GAFBaseClip ///////

		//----------------------Initialize---------------------------------//
		// <summary>
		// Initialize animation clip with it's root timeline. Crashes if root timeline is not present.
		// First operation that has to be performed to work with GAF playback.
		// Creating serializable data e.g. game objects of animation children.
		// </summary>
		// <param name="_Asset">Animation asset file.</param>

		//public virtual void initialize(GAFAnimationAssetInternal _Asset)

		//-------------------------------------------------------//

		//----------------------Initialize (overload)---------------------------------//
		// <summary>
		// Initialize animation clip.
		// First operation that has to be performed to work with GAF playback.
		// Creating serializable data e.g. game objects of animation children.
		// </summary>
		// <param name="_Asset">Animation asset file.</param>
		// <param name="_TimelineID">ID of timeline.</param>

		//public override void initialize(GAFAnimationAssetInternal _Asset, int _TimelineID)

		//-------------------------------------------------------//

		//---------------------Reload--------------------------//
		/// <summary>
		/// Reload animation view.
		/// Second opertation that has to be performed to work with GAF playback.
		/// Creating non-serializable data e.g. meshes, runtime materials.
		/// Updating animation view to a currently selected frame.
		/// Should be called every time when animation view has been changed e.g. setting animation color.
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

				var scale = settings.pixelsPerUnit / settings.scale;
				var matrix = Matrix4x4.identity;
				var obj = _Objects[_State.id].impl;

				matrix[0, 0] = _State.a;
				matrix[0, 1] = _State.c;
				matrix[1, 0] = _State.b;
				matrix[1, 1] = _State.d;
				matrix[0, 3] = _State.localPosition.x / scale + obj.serializedProperties.offset.x + settings.pivotOffset.x;
				matrix[1, 3] = -_State.localPosition.y / scale + obj.serializedProperties.offset.y + settings.pivotOffset.y;
				matrix[2, 3] = 0;

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
		// Moves animation to not initialized state.
		// </summary>
		// <param name="destroyChildren">Destroy game objects from scene.</param>

		//public void clear(bool destroyChildren = false)

		//-------------------------------------------------------//

		//---------------------Clean view--------------------------//
		// <summary>
		// Clean up animation non-serialized data.
		// Calling reload resets animation state.
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

		#region //////// GAFMovieClip /////////
		//---------------------Play--------------------------//
		// <summary>
		// Start playing animation.
		// </summary>

		//public void play()

		//-------------------------------------------------------//

		//---------------------Pause--------------------------//
		// <summary>
		// Pause animation.
		// </summary>

		//public void pause()

		//-------------------------------------------------------//

		//---------------------Stop--------------------------//
		// <summary>
		// Stop playing animation. Moving playback to a first frame in a current sequence.
		// </summary>

		//public void stop()

		//-------------------------------------------------------//

		//---------------------Go to and stop--------------------------//
		// <summary>
		// Go to the desired frame.
		// </summary>
		// <param name="_FrameNumber">Number of frame.</param>

		//public void gotoAndStop(uint _FrameNumber)

		//-------------------------------------------------------//

		//---------------------Go to and play--------------------------//
		// <summary>
		// Go to the desired frame and start playing.
		// </summary>
		// <param name="_FrameNumber">Number of frame.</param>

		//public void gotoAndPlay(uint _FrameNumber)

		//-------------------------------------------------------//

		//---------------------Sequence index to name--------------------------//
		// <summary>
		// Get sequence name by index.
		// </summary>
		// <param name="_Index">Index of sequence.</param>
		// <returns>System.String.</returns>

		//public string sequenceIndexToName(uint _Index)

		//-------------------------------------------------------//

		//---------------------Sequence name to index--------------------------//
		// <summary>
		// Get sequence index by name.
		// </summary>
		// <param name="_Name">Name of sequence.</param>
		// <returns>System.UInt32.</returns>

		//public uint sequenceNameToIndex(string _Name)

		//-------------------------------------------------------//

		//---------------------Set sequence--------------------------//
		// <summary>
		// Set sequence for playing.
		// </summary>
		// <param name="_SequenceName">Name of desired sequence.</param>
		// <param name="_PlayImmediately">Run this sequence immediately.</param>

		//public void setSequence(string _SequenceName, bool _PlayImmediately = false)

		//-------------------------------------------------------//

		//---------------------Set default sequence--------------------------//
		// <summary>
		// Set playback to a default sequence.
		// </summary>
		// <param name="_PlayImmediately">Run this sequence immediately.</param>

		//public void setDefaultSequence(bool _PlayImmediately = false)

		//-------------------------------------------------------//

		//-------------Get current sequence index-----------------//
		// <summary>
		// Get index of current sequence.
		// </summary>
		// <returns>System.UInt32.</returns>

		//public uint getCurrentSequenceIndex()

		//-------------------------------------------------------//

		//---------------------Get current frame number--------------------------//
		// <summary>
		// Get current frame number.
		// </summary>
		// <returns>System.UInt32.</returns>

		//public uint getCurrentFrameNumber()

		//-------------------------------------------------------//

		//---------------------Get frames count--------------------------//
		// <summary>
		// Get count of frames in current timeline.
		// </summary>
		// <returns>System.UInt32.</returns>

		//public uint getFramesCount()

		//-------------------------------------------------------//

		//---------------------Get animation wrap mode--------------------------//
		// <summary>
		// Get wrap mode.
		// </summary>
		// <returns>GAFWrapMode.</returns>

		//public GAFWrapMode getAnimationWrapMode()

		//-------------------------------------------------------//

		//---------------Set animation wrap mode-----------------//
		// <summary>
		// Set wrap mode.
		// </summary>
		// <param name="_Mode">Type of wrap mode.</param>

		//public void setAnimationWrapMode(GAFWrapMode _Mode)

		//-------------------------------------------------------//

		//---------------------Is playing--------------------------//
		// <summary>
		// Check if animation is playing.
		// </summary>
		// <returns><c>true</c> if this instance is playing; otherwise, <c>false</c>.</returns>

		//public bool isPlaying()

		//-------------------------------------------------------//

		//---------------------Duration--------------------------//
		// <summary>
		// Get duration of current sequence.
		// </summary>
		// <returns>System.Single.</returns>

		//public float duration()

		//-------------------------------------------------------//

		//---------------------Add trigger--------------------------//
		// <summary>
		// Add event trigger on a desired frame.
		// Used for defining callbacks e.g. sounds, custom effects.
		// Returns ID of current event.
		// </summary>
		// <param name="_Callback">Event will been call on selected frame.</param>
		// <param name="_FrameNumber">Frame number.</param>
		// <returns>System.String.</returns>

		//public string addTrigger(Action<IGAFMovieClip> _Callback, uint _FrameNumber)

		//-------------------------------------------------------//

		//---------------------Remove trigger--------------------------//
		// <summary>
		// Remove event trigger by its ID.
		// </summary>
		// <param name="_ID">ID of trigger.</param>

		//public void removeTrigger(string _ID)

		//-------------------------------------------------------//

		//----------------Remove all triggers--------------------//
		// <summary>
		// Remove all event triggers on selected frame.
		// </summary>
		// <param name="_FrameNumber">Number of frame.</param>

		//public void removeAllTriggers(uint _FrameNumber)

		//-------------------------------------------------------//

		//----------------Remove all triggers (overload)--------------------//
		// <summary>
		// Remove all event triggers in current animation.
		// </summary>

		//public void removeAllTriggers()

		//-------------------------------------------------------//

		#endregion //////// GAFMovieClip /////////

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
		//  instead of animation resources or during resources loading.
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

		#region /////// PROPERTIES ////////

		//----------------------Current sequence index----------------------------//
		// <summary>
		// Returns index of current sequence.
		// </summary>
		// <value>The index of the current sequence.</value>

		//public uint currentSequenceIndex { get; protected set; }

		//---------------------------------------------------------------//

		//----------------------Current sequence---------------------------------//
		// <summary>
		// Returns data of current sequence.
		// </summary>
		// <value>The current sequence.</value>

		//public GAFSequenceData currentSequence { get; }

		//---------------------------------------------------------------//

		#endregion /////// PROPERTIES ////////
	}
}
