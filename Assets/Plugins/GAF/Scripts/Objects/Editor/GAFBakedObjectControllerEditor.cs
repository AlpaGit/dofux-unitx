
// File:			GAFBakedObjectControllerEditor.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		� 2017 GAFMedia
// Project:			GAF Unity plugin


using GAF.Managed.Editor.Objects;
using UnityEditor;
using UnityEngine;

namespace GAF.Scripts.Objects.Editor
{
	[CustomEditor(typeof(GAFBakedObjectController))]
	public class GAFBakedObjectControllerEditor : UnityEditor.Editor
	{
		#region Interface

		new public GAFBakedObjectController target
		{
			get
			{
				return base.target as GAFBakedObjectController;
			}
		}

		public void OnEnable()
		{
			EditorApplication.update += EditorUpdate;
		}

		public void OnDisable()
		{
			EditorApplication.update -= EditorUpdate;
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			GUI.enabled = false;
			EditorGUILayout.EnumPopup("Type", target.bakedObject.serializedProperties.behaviourType);
			GUI.enabled = true;

			GUILayout.Space(5f);
			var visible = EditorGUILayout.Toggle("Is Visible", target.bakedObject.serializedProperties.visible);
			if (visible != target.bakedObject.serializedProperties.visible)
			{
				target.bakedObject.serializedProperties.visible = visible;
				target.bakedObject.serializedProperties.clip.reload();
			}

			GUILayout.Space(5f);
			var offset = EditorGUILayout.Vector2Field("Offset", target.bakedObject.serializedProperties.offset);
			if (offset != target.bakedObject.serializedProperties.offset)
			{
				target.gameObject.transform.localPosition = new Vector3(offset.x, offset.y, 0f);
				target.bakedObject.serializedProperties.offset = (Vector3)offset;
				target.bakedObject.serializedProperties.clip.reload();
			}

			var meshMultiplier = EditorGUILayout.Vector2Field("Mesh Size Multiplier", target.bakedObject.serializedProperties.meshSizeMultiplier);
			if (meshMultiplier != target.bakedObject.serializedProperties.meshSizeMultiplier)
			{
				target.bakedObject.serializedProperties.meshSizeMultiplier = meshMultiplier;
				target.bakedObject.serializedProperties.clip.reload();
			}

			GUILayout.Space(5f);
			var material = EditorGUILayout.ObjectField("Material", target.bakedObject.serializedProperties.customMaterial, typeof(Material), false) as Material;
			if (material != target.bakedObject.serializedProperties.customMaterial)
			{
				target.bakedObject.serializedProperties.customMaterial = material;
				target.bakedObject.serializedProperties.clip.reload();
			}

			var useCustomAtlasRect = EditorGUILayout.Toggle("Use Custom Atlas Texture Rect", target.bakedObject.serializedProperties.useCustomAtlasTextureRect);
			if (useCustomAtlasRect != target.bakedObject.serializedProperties.useCustomAtlasTextureRect)
			{
				target.bakedObject.serializedProperties.useCustomAtlasTextureRect = useCustomAtlasRect;
				target.bakedObject.serializedProperties.clip.reload();
			}

			GUI.enabled = target.bakedObject.serializedProperties.useCustomAtlasTextureRect;
			var atlasTextureRect = EditorGUILayout.RectField("Atlas Texture Rect", target.bakedObject.serializedProperties.atlasTextureRect);
			if (atlasTextureRect != target.bakedObject.serializedProperties.atlasTextureRect)
			{
				target.bakedObject.serializedProperties.atlasTextureRect = atlasTextureRect;
				target.bakedObject.serializedProperties.clip.reload();
			}

			GUILayout.Space(5f);
			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Select custom region"))
				{
					var window = GAFCustomSelectionWindow.GetWindow(target.bakedObject.currentMaterial.mainTexture as Texture2D);
					window.onRectChange += (rect) =>
					{
						target.bakedObject.serializedProperties.atlasTextureRect = rect;
						target.bakedObject.serializedProperties.clip.reload();
					};
				}
				if (GUILayout.Button("Return defaults"))
				{
					target.bakedObject.serializedProperties.atlasTextureRect = getDefaultAtlasTextureData(target.bakedObject);
					target.bakedObject.serializedProperties.clip.reload();
				}
			}
			GUILayout.EndHorizontal();
			GUI.enabled = false;
		}

		#endregion // Interface

		#region Implementation

		private Rect getDefaultAtlasTextureData(GAFBakedObject _Object)
		{
			var clip = _Object.serializedProperties.clip;
			var atlasElementID = _Object.serializedProperties.atlasElementID;

			var atlasData = clip.asset.getAtlases(clip.timelineID).Find(atlas => atlas.scale == clip.settings.scale);
			var atlasElementData = atlasData.getElement((uint)atlasElementID);

			var x = atlasElementData.x;
			var y = atlasElementData.y;
			var width = atlasElementData.width;
			var height = atlasElementData.height;

			return new Rect(x, y, width, height);
		}

		private void EditorUpdate()
		{
			var actualPosition = new Vector3(target.bakedObject.serializedProperties.offset.x, target.bakedObject.serializedProperties.offset.y, 0f);
			if (target.gameObject.transform.localPosition != actualPosition)
			{
				target.bakedObject.serializedProperties.offset = new Vector3(target.gameObject.transform.localPosition.x, target.transform.localPosition.y, 0f);
				target.bakedObject.serializedProperties.clip.reload();
			}
		}

		#endregion // Implementation
	}
}