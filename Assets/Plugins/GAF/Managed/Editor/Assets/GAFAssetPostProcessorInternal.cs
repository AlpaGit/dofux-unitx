using System;
using System.IO;
using System.Linq;
using GAF.Managed.Editor.Tracking;
using GAF.Managed.GAFInternal.Reader;
using UnityEditor;
using UnityEngine;

namespace GAF.Managed.Editor.Assets
{
	// Token: 0x02000038 RID: 56
	internal class GAFAssetPostProcessorInternal : AssetPostprocessor
	{
		// Token: 0x06000115 RID: 277 RVA: 0x000029F8 File Offset: 0x00000BF8
		public void OnPreprocessTexture()
		{
			GAFResourceManagerInternal.instance.preProcessTexture((TextureImporter)assetImporter);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00002A0F File Offset: 0x00000C0F
		public void OnPostprocessTexture(Texture2D texture)
		{
			GAFResourceManagerInternal.instance.postProcessTexture(assetPath, (TextureImporter)assetImporter);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00002A2C File Offset: 0x00000C2C
		public override uint GetVersion()
		{
			return 1U;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00009974 File Offset: 0x00007B74
		public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			foreach (string text in importedAssets)
			{
				if (text.EndsWith(".gaf"))
				{
					byte[] array = null;
					using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(text)))
					{
						array = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
					}
					if (array.Length > 4 && GAFHeader.isCorrectHeader((GAFHeader.CompressionType)BitConverter.ToInt32(array.Take(4).ToArray<byte>(), 0)))
					{
						GAFTracking.sendAssetCreatedRequest(text);
					}
				}
			}
		}
	}
}
