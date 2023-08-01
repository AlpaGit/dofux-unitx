using System.Collections.Generic;
using System.IO;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x02000035 RID: 53
	public class TagDefineAtlas : TagBase
	{
		// Token: 0x06000112 RID: 274 RVA: 0x00006BBC File Offset: 0x00004DBC
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline)
		{
			var num = _GAFFileReader.ReadSingle();
			if (!_SharedData.scales.Contains(num))
			{
				_SharedData.scales.Add(num);
			}
			var dictionary = new Dictionary<uint, GAFTexturesData>();
			var dictionary2 = new Dictionary<uint, GAFAtlasElementData>();
			var b = _GAFFileReader.ReadByte();
			for (byte b2 = 0; b2 < b; b2 += 1)
			{
				var num2 = _GAFFileReader.ReadUInt32();
				var b3 = _GAFFileReader.ReadByte();
				var dictionary3 = new Dictionary<float, string>();
				for (byte b4 = 0; b4 < b3; b4 += 1)
				{
					var value = GAFReader.ReadString(_GAFFileReader);
					var num3 = _GAFFileReader.ReadSingle();
					if (!_SharedData.csfs.Contains(num3))
					{
						_SharedData.csfs.Add(num3);
					}
					dictionary3.Add(num3, value);
				}
				dictionary.Add(num2, new GAFTexturesData(num2, dictionary3));
			}
			var num4 = _GAFFileReader.ReadUInt32();
			for (var num5 = 0U; num5 < num4; num5 += 1U)
			{
				var vector = GAFReader.ReadVector2(_GAFFileReader);
				var vector2 = GAFReader.ReadVector2(_GAFFileReader);
				var scale = _GAFFileReader.ReadSingle();
				var width = _GAFFileReader.ReadSingle();
				var height = _GAFFileReader.ReadSingle();
				var atlasID = _GAFFileReader.ReadUInt32();
				var num6 = _GAFFileReader.ReadUInt32();
				dictionary2.Add(num6, new GAFAtlasElementData(num6, vector.x, vector.y, vector2.x, vector2.y, width, height, atlasID, scale, new Rect(0f, 0f, 0f, 0f)));
			}
			_SharedData.rootTimeline.atlases.Add(new GAFAtlasData(num, dictionary, dictionary2));
		}
	}
}
