
// File:			GAFConverterWindowListener.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using System.Collections.Generic;
using System.IO;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Utils;
using GAF.Scripts.Asset;
using UnityEditor;
using UnityEngine;

namespace GAF.Scripts.Core.Editor
{
	[InitializeOnLoad]
	public static class GAFConverterWindowListener
	{
		static GAFConverterWindowListener()
		{
		}

		private static void onCreateClip(string _AssetPath, bool _IsBaked, bool _IsAnimator)
		{
			var assetName = Path.GetFileNameWithoutExtension(_AssetPath).Replace(" ", "_");
			var assetDir = "Assets" + Path.GetDirectoryName(_AssetPath).Replace(Application.dataPath, "") + "/";

			var asset = AssetDatabase.LoadAssetAtPath(assetDir + assetName + ".asset", typeof(GAFAnimationAsset)) as GAFAnimationAsset;
			if (!System.Object.Equals(asset, null))
			{
				var clipObject = createClip(asset, _IsBaked, _IsAnimator);

				var selected = new List<Object>(Selection.gameObjects);
				selected.Add(clipObject);
				Selection.objects = selected.ToArray();
			}
			else
			{
				GAFUtils.Log("Cannot find asset with path - " + _AssetPath, "");
			}
		}

		private static void onCreateClipPrefab(string _AssetPath, bool _IsBaked, bool _IsAnimator)
		{
			var assetName = Path.GetFileNameWithoutExtension(_AssetPath).Replace(" ", "_");
			var assetDir = "Assets" + Path.GetDirectoryName(_AssetPath).Replace(Application.dataPath, "") + "/";

			var asset = AssetDatabase.LoadAssetAtPath(assetDir + assetName + ".asset", typeof(GAFAnimationAsset)) as GAFAnimationAsset;
			if (!System.Object.Equals(asset, null))
			{
				var selected = new List<Object>(Selection.gameObjects);

				var prefabPath = assetDir + assetName;
				if (_IsBaked)
				{
					prefabPath += "_baked";
				}
				if (_IsAnimator)
				{
					prefabPath += "_animator";
				}

				prefabPath += ".prefab";

				var existingPrefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
				if (existingPrefab == null)
				{
					var clipObject = createClip(asset, _IsBaked, _IsAnimator);
					var prefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
					prefab = PrefabUtility.ReplacePrefab(clipObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
					GameObject.DestroyImmediate(clipObject);
					selected.Add(prefab);
				}
				else
				{
					selected.Add(existingPrefab);
				}

				Selection.objects = selected.ToArray();
			}
			else
			{
				GAFUtils.Log("Cannot find asset with path - " + _AssetPath, "");
			}
		}

		private static void onCreateClipPrefabPlusInstance(string _AssetPath, bool _IsBaked, bool _IsAnimator)
		{
			var assetName = Path.GetFileNameWithoutExtension(_AssetPath).Replace(" ", "_");
			var assetDir = "Assets" + Path.GetDirectoryName(_AssetPath).Replace(Application.dataPath, "") + "/";

			var asset = AssetDatabase.LoadAssetAtPath(assetDir + assetName + ".asset", typeof(GAFAnimationAsset)) as GAFAnimationAsset;
			if (!System.Object.Equals(asset, null))
			{
				var selected = new List<Object>(Selection.gameObjects);

				var prefabPath = assetDir + assetName;
				if (_IsBaked)
				{
					prefabPath += "_baked";
				}
				if (_IsAnimator)
				{
					prefabPath += "_animator";
				}

				prefabPath += ".prefab";

				var existingPrefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
				if (existingPrefab == null)
				{
					var clipObject = createClip(asset, _IsBaked, _IsAnimator);
					var prefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
					prefab = PrefabUtility.ReplacePrefab(clipObject, prefab, ReplacePrefabOptions.ConnectToPrefab);

					selected.Add(clipObject);
					selected.Add(prefab);
				}
				else
				{
					var instance = PrefabUtility.InstantiatePrefab(existingPrefab) as GameObject;
					selected.Add(existingPrefab);
					selected.Add(instance);
				}

				Selection.objects = selected.ToArray();
			}
			else
			{
				GAFUtils.Log("Cannot find asset with path - " + _AssetPath, "");
			}
		}

		private static GameObject createClip(GAFAnimationAsset _Asset, bool _IsBaked, bool _IsAnimator)
		{
			var clipObject = new GameObject(_Asset.name);

			GAFBaseClip clip = null;
			if (_IsBaked)
			{
				if (!_IsAnimator)
					clip = clipObject.AddComponent<GAFBakedMovieClip>();
				else
					clip = clipObject.AddComponent<GAFBakedAnimator>();
			}
			else
			{
				if (!_IsAnimator)
					clip = clipObject.AddComponent<GAFMovieClip>();
				else
					clip = clipObject.AddComponent<GAFAnimator>();
			}

			clip.initialize(_Asset);
			clip.reload();

			return clipObject;
		}
	}
}
