
// File:			GAFMecanimResourcesRelocator.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using GAF.Managed.Editor.Utils;
using GAF.Managed.GAFInternal.Assets;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace GAF.Scripts.Utils.Editor
{
	public class GAFMecanimResourcesRelocator : GAFMecanimResourcesRelocatorIntenal
	{
		protected override void relocateAssetResources(GAFAnimationAssetInternal _Asset, string _NewPath)
		{
			for (int i = 0; i < _Asset.animatorControllers.Count; ++i)
			{
				var controllerOldPath = AssetDatabase.GetAssetPath(_Asset.animatorControllers[i]);
				var animatorController = _Asset.animatorControllers[i] as AnimatorController;
				var controllerNewPath = "Assets/" + _NewPath + System.IO.Path.GetFileName(controllerOldPath);
				var existingController = AssetDatabase.LoadAssetAtPath(controllerNewPath, typeof(AnimatorController)) as AnimatorController;
				if (existingController != null)
					AssetDatabase.DeleteAsset(controllerNewPath);

				AssetDatabase.MoveAsset(controllerOldPath, controllerNewPath);

				for (int j = 0; j < animatorController.layers.Length; ++j)
				{
					var layerStateMachine = animatorController.layers[j].stateMachine;
					for (int k = 0; k < layerStateMachine.states.Length; ++k)
					{
						var state = layerStateMachine.states[k].state;
						var clip = state.motion as AnimationClip;
						var clipOldPath = AssetDatabase.GetAssetPath(clip);
						var clipNewPath = "Assets/" + _NewPath + System.IO.Path.GetFileName(clipOldPath);
						var existingClip = AssetDatabase.LoadAssetAtPath(controllerNewPath, typeof(AnimationClip)) as AnimationClip;
						if (existingClip != null)
							AssetDatabase.DeleteAsset(clipNewPath);

						AssetDatabase.MoveAsset(clipOldPath, clipNewPath);
					}
				}
			}
		}
	}
}
