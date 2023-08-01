using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GAF.Managed.Editor.Utils;
using GAF.Managed.GAFInternal;
using GAF.Managed.GAFInternal.Assets;
using GAF.Managed.GAFInternal.Data;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace GAF.Managed.Editor.Assets
{
	// Token: 0x02000039 RID: 57
	public class GAFResourceManagerInternal
	{
		// Token: 0x0600011A RID: 282 RVA: 0x00002A37 File Offset: 0x00000C37
		protected GAFResourceManagerInternal()
		{
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00009A0C File Offset: 0x00007C0C
		public static GAFResourceManagerInternal instance
		{
			get
			{
				if (m_Instance == null)
				{
					object locker = m_Locker;
					lock (locker)
					{
						if (m_Instance == null)
						{
							m_Instance = new GAFResourceManagerInternal();
						}
					}
				}
				return m_Instance;
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00002A60 File Offset: 0x00000C60
		public static void initInstance<T>() where T : GAFResourceManagerInternal, new()
		{
			if (m_Instance == null || !(m_Instance is T))
			{
				m_Instance = Activator.CreateInstance<T>();
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00009A64 File Offset: 0x00007C64
		public void createResources<TypeOfResource>(GAFAnimationAssetInternal _Asset) where TypeOfResource : GAFTexturesResourceInternal
		{
			string assetPath = AssetDatabase.GetAssetPath(_Asset);
			if (!string.IsNullOrEmpty(assetPath))
			{
				GAFSystemEditor.getCachePath();
				Dictionary<KeyValuePair<float, float>, List<string>> dictionary = new Dictionary<KeyValuePair<float, float>, List<string>>();
				foreach (GAFTimelineData gaftimelineData in _Asset.getTimelines())
				{
					foreach (GAFAtlasData gafatlasData in gaftimelineData.atlases)
					{
						foreach (GAFTexturesData gaftexturesData in gafatlasData.texturesData.Values)
						{
							foreach (KeyValuePair<float, string> keyValuePair in gaftexturesData.files)
							{
								string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(keyValuePair.Value);
								KeyValuePair<float, float> key = new KeyValuePair<float, float>(gafatlasData.scale, keyValuePair.Key);
								if (!dictionary.ContainsKey(key))
								{
									dictionary[key] = new List<string>();
								}
								dictionary[key].Add(fileNameWithoutExtension);
							}
						}
					}
				}
				m_Resources.RemoveAll((GAFTexturesResourceInternal resource) => resource == null || !resource.isValid);
				SerializedObject serializedObject = new SerializedObject(_Asset);
				SerializedProperty serializedProperty = serializedObject.FindProperty("m_ResourcesDirectory");
				if (string.IsNullOrEmpty(serializedProperty.stringValue))
				{
					serializedProperty.stringValue = GAFSystem.CachePath;
					serializedObject.ApplyModifiedProperties();
					EditorUtility.SetDirty(_Asset);
				}
				foreach (KeyValuePair<KeyValuePair<float, float>, List<string>> keyValuePair2 in dictionary)
				{
					string str = _Asset.getResourceName(keyValuePair2.Key.Key, keyValuePair2.Key.Value) + ".asset";
					string path = "Assets/" + _Asset.resourcesDirectory + str;
					string dataPath = Path.GetDirectoryName(assetPath).Replace('\\', '/') + "/";
					TypeOfResource typeOfResource = ScriptableObject.CreateInstance<TypeOfResource>();
					typeOfResource = GAFAssetUtils.saveAsset<TypeOfResource>(typeOfResource, path);
					typeOfResource.initialize(_Asset, keyValuePair2.Value.Distinct<string>().ToList<string>(), keyValuePair2.Key.Key, keyValuePair2.Key.Value, dataPath);
					EditorUtility.SetDirty(typeOfResource);
					findResourceTextures(typeOfResource, true);
					if (!typeOfResource.isReady)
					{
						m_Resources.Add(typeOfResource);
					}
				}
				EditorUtility.SetDirty(_Asset);
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00009DB4 File Offset: 0x00007FB4
		public void findResourceTextures(GAFTexturesResourceInternal _Resource, bool _Reimport)
		{
			if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(_Resource)))
			{
				using (List<Texture2D>.Enumerator enumerator = GAFAssetUtils.findAssetsAtPath<Texture2D>(_Resource.currentDataPath, "*.png", SearchOption.TopDirectoryOnly).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Texture2D texture = enumerator.Current;
						GAFResourceData gafresourceData = _Resource.invalidData.Find((GAFResourceData _data) => _data.name == texture.name);
						if (gafresourceData != null)
						{
							if (_Reimport)
							{
								string assetPath = AssetDatabase.GetAssetPath(texture);
								TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
								if (hasCorrectImportSettings(textureImporter, _Resource))
								{
									gafresourceData.set(texture, getSharedMaterial(texture));
									m_ImportList.Remove(assetPath);
									EditorUtility.SetDirty(_Resource);
								}
								else
								{
									changeTextureImportSettings(textureImporter, _Resource);
									AssetDatabase.ImportAsset(textureImporter.assetPath, ImportAssetOptions.ForceUpdate);
								}
							}
							else
							{
								gafresourceData.set(texture, getSharedMaterial(texture));
								EditorUtility.SetDirty(_Resource);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00009ED4 File Offset: 0x000080D4
		public Material getSharedMaterial(Texture2D _Texture)
		{
			string assetPath = AssetDatabase.GetAssetPath(_Texture);
			string text = Path.GetDirectoryName(assetPath) + "/" + Path.GetFileNameWithoutExtension(assetPath) + ".mat";
			Material material = AssetDatabase.LoadAssetAtPath(text, typeof(Material)) as Material;
			if (material == null)
			{
				material = GAFAssetUtils.saveAsset<Material>(new Material(Shader.Find("GAF/GAFObjectsGroup"))
				{
					mainTexture = _Texture
				}, text);
			}
			else
			{
				material.mainTexture = _Texture;
			}
			return material;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00009F4C File Offset: 0x0000814C
		public void preProcessTexture(TextureImporter _Importer)
		{
			string textureName = Path.GetFileNameWithoutExtension(_Importer.assetPath);
			m_Resources.RemoveAll((GAFTexturesResourceInternal resource) => resource == null || !resource.isValid);
			foreach (GAFTexturesResourceInternal gaftexturesResourceInternal in m_Resources)
			{
				if (gaftexturesResourceInternal.currentDataPath == Path.GetDirectoryName(_Importer.assetPath) + "/")
				{
					List<GAFResourceData> invalidData = gaftexturesResourceInternal.invalidData;

					if (invalidData.Find((_data => _data.name == textureName)) != null && !hasCorrectImportSettings(_Importer, gaftexturesResourceInternal))
					{
						changeTextureImportSettings(_Importer, gaftexturesResourceInternal);
					}
				}
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000A03C File Offset: 0x0000823C
		public void postProcessTexture(string _TexturePath, TextureImporter _Importer)
		{
			m_TaskManager.waitFor(0.1f).then(delegate
			{
				postProcessTextureDelayed(_TexturePath, _Importer);
			});
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000A088 File Offset: 0x00008288
		public void defineAudioResources(GAFAnimationAssetInternal _Asset)
		{
			GAFAnimationData sharedData = _Asset.sharedData;
			string str = Path.GetDirectoryName(AssetDatabase.GetAssetPath(_Asset)) + "/";
			for (int i = 0; i < sharedData.audioClips.Count; i++)
			{
				GAFSoundData gafsoundData = sharedData.audioClips[i];
				AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(str + gafsoundData.fileName);
				if (audioClip != null)
				{
					_Asset.audioResources.Add(new GAFAnimationAssetInternal.GAFSound
					{
						ID = (int)gafsoundData.id,
						audio = audioClip
					});
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000208D File Offset: 0x0000028D
		public virtual void createMecanimResources(GAFAnimationAssetInternal _Asset)
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000208D File Offset: 0x0000028D
		protected virtual void createAnimations(GAFTimelineData _Timeline, AnimatorController _AnimatorController, string _AnimationsPath)
		{
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00002A8A File Offset: 0x00000C8A
		protected virtual AnimationClip createAnimationClip(GAFTimelineData _Timeline, GAFSequenceData _Sequence, string _Path)
		{
			return null;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000A120 File Offset: 0x00008320
		private void postProcessTextureDelayed(string _TexturePath, TextureImporter _Importer)
		{
			Texture2D texture = AssetDatabase.LoadAssetAtPath(_TexturePath, typeof(Texture2D)) as Texture2D;
			m_Resources.RemoveAll((GAFTexturesResourceInternal resource) => resource == null || !resource.isValid);
			foreach (GAFTexturesResourceInternal gaftexturesResourceInternal in m_Resources)
			{
				if (gaftexturesResourceInternal.currentDataPath == Path.GetDirectoryName(_TexturePath) + "/")
				{
					List<GAFResourceData> invalidData = gaftexturesResourceInternal.invalidData;
					GAFResourceData gafresourceData = invalidData.Find(((GAFResourceData _data) => _data.name == texture.name));
					if (gafresourceData != null)
					{
						if (hasCorrectImportSettings(_Importer, gaftexturesResourceInternal))
						{
							gafresourceData.set(texture, getSharedMaterial(texture));
							m_ImportList.Remove(_TexturePath);
							EditorUtility.SetDirty(gaftexturesResourceInternal);
						}
						else
						{
							changeTextureImportSettings(_Importer, gaftexturesResourceInternal);
							AssetDatabase.ImportAsset(_Importer.assetPath, ImportAssetOptions.ForceUpdate);
						}
					}
				}
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000A258 File Offset: 0x00008458
		private void changeTextureImportSettings(TextureImporter _Importer, GAFTexturesResourceInternal _Resource)
		{
			if (!m_ImportList.Contains(_Importer.assetPath))
			{
				_Importer.textureType    = TextureImporterType.Sprite;
				_Importer.maxTextureSize = 4096;
				_Importer.mipmapEnabled  = false;
				_Importer.textureFormat  = TextureImporterFormat.AutomaticTruecolor;
				TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
				_Importer.ReadTextureSettings(textureImporterSettings);
				textureImporterSettings.wrapMode = TextureWrapMode.Clamp;
				textureImporterSettings.readable = false;
				_Importer.SetTextureSettings(textureImporterSettings);
				m_ImportList.Add(_Importer.assetPath);
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00002A8D File Offset: 0x00000C8D
		private bool hasCorrectImportSettings(TextureImporter _Importer, GAFTexturesResourceInternal _Resource)
		{
			return _Importer.textureType == TextureImporterType.Sprite && _Importer.maxTextureSize == 4096 && !_Importer.mipmapEnabled && !_Importer.isReadable && _Importer.textureFormat == TextureImporterFormat.AutomaticTruecolor;
		}

		// Token: 0x040000C3 RID: 195
		private static readonly object m_Locker = new object();

		// Token: 0x040000C4 RID: 196
		private static volatile GAFResourceManagerInternal m_Instance = null;

		// Token: 0x040000C5 RID: 197
		private List<string> m_ImportList = new List<string>();

		// Token: 0x040000C6 RID: 198
		private List<GAFTexturesResourceInternal> m_Resources = new List<GAFTexturesResourceInternal>();

		// Token: 0x040000C7 RID: 199
		private GAFTaskManager m_TaskManager = new GAFTaskManager();
	}
}
