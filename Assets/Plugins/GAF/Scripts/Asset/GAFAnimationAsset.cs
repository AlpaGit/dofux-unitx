
// File:			GAFAnimationAsset.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using GAF.Managed.GAFInternal.Assets;

namespace GAF.Scripts.Asset
{
	[System.Serializable]
	public class GAFAnimationAsset : GAFAnimationAssetInternal
	{
		#region /////// METHODS ///////
		//-----------------------Initialize---------------------//
		// <summary>
		// Initialize asset data with .gaf file binary data content and GUID.
		// <para />Gaf animation initial point.
		// </summary>
		// <param name="_GAFData">GAF asset binary data.</param>
		// <param name="_GUID">Unique identifier of asset file.</param>

		//public void initialize(byte[] _GAFData, string _GUID)

		//-----------------------------------------------------//

		//-----------------------Load---------------------//
		// <summary>
		// Load GAF asset binary data if it is not loaded. 
		// <para />Extract GAF-data into Unity form.
		// </summary>

		//public void load()

		//-----------------------------------------------------//

		//---------------------Get resource--------------------------//
		// <summary>
		// Find resource by scale and content scale factor values.
		// </summary>
		// <param name="_Scale">Scale of desired resource.</param>
		// <param name="_CSF">Content scale factor of desired resource.</param>
		// <returns></returns>

		//public GAFTexturesResourceInternal getResource(float _Scale, float _CSF)

		//-----------------------------------------------------------------//

		//---------------------Get animator controller-----------------//
		// <summary>
		// Get runtime animator controller associated with current animation.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public RuntimeAnimatorController getAnimatorController(int _TimelineID)

		//-----------------------------------------------------------------//

		//---------------------Get resource name----------------------//
		// <summary>
		// Generate resource name by scale and content scale factor values.
		// </summary>
		// <param name="_Scale">Scale of desired resource.</param>
		// <param name="_CSF">Content scale factor of desired resource.</param>
		// <returns></returns>

		//public string getResourceName(float _Scale, float _CSF)

		//------------------------------------------------------------//

		//-----------------------Get timelines---------------------//
		// <summary>
		// Get all timelines data from current animation.
		// </summary>
		// <returns></returns>

		//public List<GAFTimelineData> getTimelines()

		//-----------------------------------------------------//

		//-----------------------Get atlases---------------------//
		// <summary>
		// Get all atlas textures data used in timeline.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public List<GAFAtlasData> getAtlases(int _TimelineID)

		//-----------------------------------------------------//

		//------------------------Get objects-----------------------------//
		// <summary>
		// Get all animation objects data used in timeline.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public List<GAFObjectData> getObjects(int _TimelineID)

		//-----------------------------------------------------//

		//-------------------------Get masks----------------------------//

		// <summary>
		// Get all animation masks data used in timeline.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public  List<GAFObjectData> getMasks(int _TimelineID)

		//-----------------------------------------------------//

		//------------------------Get frames---------------------------//
		// <summary>
		// Get all animation frames data used in timeline.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public Dictionary<uint, GAFFrameData> getFrames(int _TimelineID)

		//------------------------------------------------------------//


		//----------------------Get sequences------------------------//
		// <summary>
		// Get all sequences data from timeline.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public List<GAFSequenceData> getSequences(int _TimelineID)

		//------------------------------------------------------------//


		//---------------------Get sequence IDs-----------------------//
		// <summary>
		// Get all sequences IDs for this timeline ID.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public List<string> getSequenceIDs(int _TimelineID)

		//------------------------------------------------------------//

		//---------------------Get named parts---------------------------//
		// <summary>
		// Get data, that contains correspondence between objects IDs and names.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public List<GAFNamedPartData> getNamedParts(int _TimelineID)

		//------------------------------------------------------------//

		//---------------------Get frames count---------------------------//
		// <summary>
		// Get count of frames of desired timeline.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public uint getFramesCount(int _TimelineID)

		//----------------------------------------------------------//

		//---------------------Get frame size---------------------------//
		// <summary>
		// Get general viewport size of desired timeline.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public Rect getFrameSize(int _TimelineID)

		//--------------------------------------------------------//

		//---------------------Get pivot-----------------------------//
		// <summary>
		// Get pivot of desired timeline.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public Vector2 getPivot(int _TimelineID)

		//------------------------------------------------------------//

		//---------------------Get external data----------------------//
		// <summary>
		// Get data, that contains external information about desired timeline.
		// </summary>
		// <param name="_TimelineID">ID of animation timeline.</param>
		// <returns></returns>

		//public GAFAssetExternalData getExternalData(int _TimelineID)

		//------------------------------------------------------------//

		#endregion ////// METHODS ///////

		#region ////// PROPERTIES //////

		//-----------------------Is loaded---------------------//
		// <summary>
		// Returns true if animation asset data was loaded.
		// </summary>

		//public bool isLoaded { get; }

		//----------------------------------------------------//

		//-----------------------Asset version-----------------------------//
		// <summary>
		// Returns Unity version of current animation asset.
		// </summary>

		//public int assetVersion { get; }

		//----------------------------------------------------//

		//-----------------------Major data version-----------------------------//
		// <summary>
		// Returns GAF major version of GAF format.
		// </summary>

		//public ushort majorDataVersion { get; }

		//----------------------------------------------------//

		//-----------------------Minor data version----------------------------//
		// <summary>
		// Returns GAF minor version of GAF format.
		// </summary>

		//public ushort minorDataVersion { get; }

		//----------------------------------------------------//

		//-----------------------Resources paths-----------------------------//
		// <summary>
		// Returns list with paths to animation resources assets.
		// </summary>

		//public List<string> resourcesPaths { get; }

		//----------------------------------------------------//

		//-------------------------Animators controllers---------------------------//
		// <summary>
		// Returns list with references to mecanim controllers.
		// </summary>

		//public List<RuntimeAnimatorController> animatorControllers { get; }

		//----------------------------------------------------//

		//-------------------------Scales---------------------------//
		// <summary>
		// Returns list of GAF convertion scales that can be used in animation clips.
		// </summary>

		//public List<float> scales { get; }

		//----------------------------------------------------//

		//--------------------------CSFs--------------------------//
		// <summary>
		// Returns list of GAF content scale factors that can be used in animation clips 
		// </summary>

		//public List<float> csfs { get; }

		//----------------------------------------------------//

		//-------------------------Resources directory---------------------------//
		// <summary>
		// Returns path to custom directory for animation resources storing.
		// </summary>

		//public string resourcesDirectory { get; }

		//----------------------------------------------------//

		//------------------------Mecanim resources directory----------------------------//
		// <summary>
		// Returns path to custom directory for mecanim resources storing.
		// </summary>

		//public string mecanimResourcesDirectory { get; }

		//----------------------------------------------------//

		#endregion ////// PROPERTIES /////
	}
}
