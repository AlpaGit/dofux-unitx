using System.Collections.Generic;
using System.IO;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x02000036 RID: 54
	public class TagDefineAtlas2 : TagBase
	{
		// Token: 0x06000114 RID: 276 RVA: 0x00006D40 File Offset: 0x00004F40
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline)
		{
			var scale = _GAFFileReader.ReadSingle();
			var dictionary = new Dictionary<uint, GAFTexturesData>();
			var dictionary2 = new Dictionary<uint, GAFAtlasElementData>();
			var b = _GAFFileReader.ReadByte();
			for (byte b2 = 0; b2 < b; b2 += 1)
			{
				var num = _GAFFileReader.ReadUInt32();
				var b3 = _GAFFileReader.ReadByte();
				var dictionary3 = new Dictionary<float, string>();
				for (byte b4 = 0; b4 < b3; b4 += 1)
				{
					var value = GAFReader.ReadString(_GAFFileReader);
					var key = _GAFFileReader.ReadSingle();
					dictionary3.Add(key, value);
				}
				dictionary.Add(num, new GAFTexturesData(num, dictionary3));
			}
			var num2 = _GAFFileReader.ReadUInt32();
			for (var num3 = 0U; num3 < num2; num3 += 1U)
			{
				var vector = GAFReader.ReadVector2(_GAFFileReader);
				var vector2 = GAFReader.ReadVector2(_GAFFileReader);
				var scale2 = _GAFFileReader.ReadSingle();
				var width = _GAFFileReader.ReadSingle();
				var height = _GAFFileReader.ReadSingle();
				var atlasID = _GAFFileReader.ReadUInt32();
				var num4 = _GAFFileReader.ReadUInt32();
				var flag = _GAFFileReader.ReadByte() == 1;
				var scale9GridRect = new Rect(0f, 0f, 0f, 0f);
				if (flag)
				{
					scale9GridRect = GAFReader.ReadRect(_GAFFileReader);
				}
				dictionary2.Add(num4, new GAFAtlasElementData(num4, vector.x, vector.y, vector2.x, vector2.y, width, height, atlasID, scale2, scale9GridRect));
			}
			_CurrentTimeline.atlases.Add(new GAFAtlasData(scale, dictionary, dictionary2));
		}
	}
}
