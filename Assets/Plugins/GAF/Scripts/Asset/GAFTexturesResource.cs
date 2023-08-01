
// File:			GAFTexturesResource.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using GAF.Managed.GAFInternal.Assets;

namespace GAF.Scripts.Asset
{
	[System.Serializable]
	public class GAFTexturesResource : GAFTexturesResourceInternal
	{
		#region ////// METHODS ///////

		//---------------------Set data--------------------------//
		// <summary>
		// Set texture and material for desired data.
		// </summary>
		// <param name="_SearchName">Name of desired data.</param>
		// <param name="_SharedTexture">New texture for data.</param>
		// <param name="_SharedMaterial">New material for data.</param>

		//public void setData(string _SearchName, Texture2D _SharedTexture, Material _SharedMaterial)

		//--------------------------------------------------------//

		//---------------------Is data valid--------------------------//
		// <summary>
		// Check if data exists and have a texture and material.
		// </summary>
		// <param name="_SearchName">Name of desired data.</param>
		// <returns></returns>

		//public bool isDataValid(string _SearchName)

		//---------------------------------------------------------//

		//---------------------Get shared texture--------------------------//
		// <summary>
		// Get shared texture from data.
		// </summary>
		// <param name="_Name">Name of desired data.</param>
		// <returns></returns>

		//public Texture2D getSharedTexture(string _Name)

		//--------------------------------------------------------//

		//---------------------Get shared material--------------------------//
		// <summary>
		// Get shared material from data.
		// </summary>
		// <param name="_Name">Name of desired data.</param>
		// <returns></returns>

		//public Material getSharedMaterial(string _Name)

		//----------------------------------------------------------------//

		#endregion ////// METHODS //////

		#region ////// PROPERTIES //////

		//--------------------------Asset------------------------------//
		// <summary>
		// Return animation asset file associated with current resource.
		// </summary>

		//public GAFAnimationAssetInternal asset { get; }

		//--------------------------------------------------------//

		//--------------------------Is valid------------------------------//
		// <summary>
		// Returns true if asset associated with current resource is not null.
		// </summary>

		//public bool isValid { get; }

		//--------------------------------------------------------//

		//---------------------------Is ready-----------------------------//
		// <summary>
		// Returns true if resource is valid and all his data is valid too.
		// </summary>

		//public bool isReady { get; }

		//--------------------------------------------------------//

		//--------------------------Scale------------------------------//
		// <summary>
		// Returns resource's scale.
		// </summary>

		//public float scale { get; }

		//--------------------------------------------------------//

		//--------------------------CSF------------------------------//
		// <summary>
		// Returns resource's content scale factor.
		// </summary>

		//public float csf { get; }

		//--------------------------------------------------------//

		//------------------------Data--------------------------------//
		// <summary>
		// Returns all data of current resource.
		// </summary>

		//public List<GAFResourceData> data { get; }

		//--------------------------------------------------------//

		//--------------------------Valid data------------------------------//
		// <summary>
		// Returns only valid data of current resource.
		// </summary>

		//public List<GAFResourceData> validData { get; }

		//--------------------------------------------------------//

		//--------------------------Invalid data------------------------------//
		// <summary>
		// Returns only invalid data of current resource.
		// </summary>

		//public List<GAFResourceData> invalidData { get; }

		//--------------------------------------------------------//

		//------------------------Current data path--------------------------------//
		// <summary>
		// Returns path used to find shared texture and shared material.
		// </summary>

		//public string currentDataPath { get; set; }

		//--------------------------------------------------------//

		#endregion
	}
}