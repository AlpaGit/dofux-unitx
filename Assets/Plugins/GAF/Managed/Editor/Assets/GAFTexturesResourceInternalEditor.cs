using System;
using System.Collections.Generic;
using System.IO;
using GAF.Managed.Editor.Utils;
using GAF.Managed.GAFInternal.Assets;
using UnityEditor;
using UnityEngine;

namespace GAF.Managed.Editor.Assets
{
	// Token: 0x02000040 RID: 64
	[CustomEditor(typeof(GAFTexturesResourceInternal))]
	public class GAFTexturesResourceInternalEditor : UnityEditor.Editor
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00002B5F File Offset: 0x00000D5F
		private new GAFTexturesResourceInternal target
		{
			get
			{
				return (GAFTexturesResourceInternal)base.target;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00002B6C File Offset: 0x00000D6C
		public void OnEnable()
		{
			if (!target.isValid)
			{
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(target));
				return;
			}
			normalizeDrawPath();
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000A2CC File Offset: 0x000084CC
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			GUILayout.Space(3f);
			EditorGUILayout.ObjectField(target.asset, typeof(GAFAnimationAssetInternal), false, Array.Empty<GUILayoutOption>());
			GUILayout.Space(6f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Scale = " + target.scale, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Csf = " + target.csf, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.Space(6f);
			GUILayout.Label("* This folder will be used to find missing textures!", Array.Empty<GUILayoutOption>());
			GAFGuiUtil.drawDirectorySelector(ref m_DrawPath, target.currentDataPath, Application.dataPath + "/", "Data directory: Assets/", "Select destination folder", delegate(string newPath)
			{
				relocateResourceData(target, "Assets/" + newPath);
				normalizeDrawPath();
			}, normalizeDrawPath);
			GUI.enabled = !target.isReady;
			if (GUILayout.Button("Find textures!", Array.Empty<GUILayoutOption>()))
			{
				GAFResourceManagerInternal.instance.findResourceTextures(target, false);
			}
			GUI.enabled = true;
			if (target.validData.Count > 0)
			{
				GUILayout.Space(6f);
				GUILayout.Label("Found resource data:", Array.Empty<GUILayoutOption>());
				GUILayout.Space(3f);
				drawResourceDataList(target.validData);
			}
			if (target.invalidData.Count > 0)
			{
				GUILayout.Space(6f);
				GUILayout.Label("Not found resource data:", Array.Empty<GUILayoutOption>());
				GUILayout.Space(3f);
				drawResourceDataList(target.invalidData);
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000A4A8 File Offset: 0x000086A8
		private void drawResourceDataList(List<GAFResourceData> _Data)
		{
			foreach (GAFResourceData gafresourceData in _Data)
			{
				GUILayout.Label(gafresourceData.name, Array.Empty<GUILayoutOption>());
				Texture2D texture2D = EditorGUILayout.ObjectField(gafresourceData.sharedTexture, typeof(Texture2D), false, Array.Empty<GUILayoutOption>()) as Texture2D;
				if (texture2D != gafresourceData.sharedTexture)
				{
					if (texture2D != null)
					{
						Material sharedMaterial = GAFResourceManagerInternal.instance.getSharedMaterial(texture2D);
						sharedMaterial.mainTexture = texture2D;
						gafresourceData.set(texture2D, sharedMaterial);
						EditorUtility.SetDirty(sharedMaterial);
					}
					else
					{
						gafresourceData.set(null, null);
					}
					EditorUtility.SetDirty(target);
				}
				Material material = EditorGUILayout.ObjectField(gafresourceData.sharedMaterial, typeof(Material), false, Array.Empty<GUILayoutOption>()) as Material;
				if (material != gafresourceData.sharedMaterial)
				{
					gafresourceData.set(gafresourceData.sharedTexture, material);
				}
				GUILayout.Space(6f);
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000A5C0 File Offset: 0x000087C0
		private void normalizeDrawPath()
		{
			int length = "Assets/".Length;
			m_DrawPath = target.currentDataPath.Substring(length, target.currentDataPath.Length - length);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000A604 File Offset: 0x00008804
		private void relocateResourceData(GAFTexturesResourceInternal _Resource, string _NewPath)
		{
			foreach (GAFResourceData gafresourceData in _Resource.validData)
			{
				string assetPath = AssetDatabase.GetAssetPath(gafresourceData.sharedTexture);
				string assetPath2 = AssetDatabase.GetAssetPath(gafresourceData.sharedMaterial);
				string text = _NewPath + Path.GetFileName(assetPath);
				string text2 = _NewPath + Path.GetFileName(assetPath2);
				AssetDatabase.MoveAsset(assetPath, text);
				AssetDatabase.MoveAsset(assetPath2, text2);
			}
			_Resource.currentDataPath = _NewPath;
			EditorUtility.SetDirty(_Resource);
		}

		// Token: 0x040000D5 RID: 213
		private string m_DrawPath = string.Empty;
	}
}
