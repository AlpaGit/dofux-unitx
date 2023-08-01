using System.Collections.Generic;
using System.IO;
using System.Linq;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x0200002C RID: 44
	internal class TagDefineAtlas3 : TagBase
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x00005C4C File Offset: 0x00003E4C
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline)
		{
			var num = _GAFFileReader.ReadSingle();
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
					var key = _GAFFileReader.ReadSingle();
					dictionary3.Add(key, value);
				}
				dictionary.Add(num2, new GAFTexturesData(num2, dictionary3));
			}
			var num3 = _GAFFileReader.ReadUInt32();
			for (var num4 = 0U; num4 < num3; num4 += 1U)
			{
				var vector = GAFReader.ReadVector2(_GAFFileReader);
				var vector2 = GAFReader.ReadVector2(_GAFFileReader);
				var width = _GAFFileReader.ReadSingle();
				var height = _GAFFileReader.ReadSingle();
				var atlasID = _GAFFileReader.ReadUInt32();
				var num5 = _GAFFileReader.ReadUInt32();
				var flag = _GAFFileReader.ReadByte() == 1;
				var scale9GridRect = new Rect(0f, 0f, 0f, 0f);
				if (flag)
				{
					scale9GridRect = GAFReader.ReadRect(_GAFFileReader);
				}
				var scaleX = _GAFFileReader.ReadSingle();
				var scaleY = _GAFFileReader.ReadSingle();
				var rotation = _GAFFileReader.ReadSByte();
				var linkageName = GAFReader.ReadString(_GAFFileReader);
				dictionary2.Add(num5, new GAFAtlasElementData(num5, vector.x, vector.y, vector2.x, vector2.y, width, height, atlasID, scaleX, scaleY, rotation, linkageName, scale9GridRect));
			}
			if (_CurrentTimeline != null)
			{
				_CurrentTimeline.atlases.Add(new GAFAtlasData(num, dictionary, dictionary2));
				return;
			}
			_SharedData.customRegions[num] = dictionary2.Values.ToList<GAFAtlasElementData>();
		}
	}
}
