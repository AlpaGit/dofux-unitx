using System;
using System.Collections.Generic;
using System.IO;
using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x0200003D RID: 61
	public class TagDefineTimeline : TagBase
	{
		// Token: 0x06000122 RID: 290 RVA: 0x00007050 File Offset: 0x00005250
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _RootTimeline)
		{
			var num = _GAFFileReader.ReadUInt32();
			var framesCount = _GAFFileReader.ReadUInt32();
			var frameSize = GAFReader.ReadRect(_GAFFileReader);
			var pivot = GAFReader.ReadVector2(_GAFFileReader);
			var num2 = (int)_GAFFileReader.ReadByte();
			var linkageName = string.Empty;
			if (num2 == 1)
			{
				linkageName = GAFReader.ReadString(_GAFFileReader);
			}
			var gaftimelineData = new GAFTimelineData(num, linkageName, framesCount, frameSize, pivot);
			gaftimelineData.parent = _RootTimeline;
			if (_RootTimeline != null)
			{
				_RootTimeline.childs.Add(gaftimelineData);
			}
			if (linkageName.StartsWith("Anim"))
			{
				_SharedData.timelines.Add((int)num, gaftimelineData);
			}
			else
			{
				_SharedData.secondaryTimelines.Add((int)num, gaftimelineData);
			}
			var tagsDictionary = TagDefineTimeline.GetTagsDictionary();
			while (_GAFFileReader.BaseStream.Position < _Tag.expectedStreamPosition)
			{
				TagRecord tagRecord;
				try
				{
					tagRecord = GAFReader.OpenTag(_GAFFileReader);
				}
				catch (Exception ex)
				{
					throw new Exception(string.Concat(new object[]
					{
						"GAF! GAFReader::Read - Failed to open tag! Stream position - ",
						_GAFFileReader.BaseStream.Position.ToString(),
						"\nException - ",
						ex
					}));
				}
				if (tagRecord.type != TagBase.TagType.TagInvalid && tagsDictionary.ContainsKey(tagRecord.type))
				{
					try
					{
						tagsDictionary[tagRecord.type].Read(tagRecord, _GAFFileReader, ref _SharedData, ref gaftimelineData);
					}
					catch (Exception ex2)
					{
						throw new Exception(string.Concat(new object[]
						{
							"GAF! GAFReader::Read - Failed to read tag - ",
							tagRecord.type.ToString(),
							"\n Exception - ",
							ex2.ToString(),
							" in record ",
							tagRecord.type
						}));
					}
					GAFReader.CheckTag(tagRecord, _GAFFileReader);
				}
				else
				{
					GAFReader.CloseTag(tagRecord, _GAFFileReader);
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007200 File Offset: 0x00005400
		private static Dictionary<TagBase.TagType, TagBase> GetTagsDictionary()
		{
			return new Dictionary<TagBase.TagType, TagBase>
			{
				{
					TagBase.TagType.TagDefineNamedParts,
					new TagDefineNamedParts()
				},
				{
					TagBase.TagType.TagDefineSequences,
					new TagDefineSequences()
				},
				{
					TagBase.TagType.TagDefineAtlas2,
					new TagDefineAtlas2()
				},
				{
					TagBase.TagType.TagDefineAtlas3,
					new TagDefineAtlas3()
				},
				{
					TagBase.TagType.TagDefineStage,
					new TagDefineStage()
				},
				{
					TagBase.TagType.TagDefineAnimationObjects2,
					new TagDefineAnimationObjects2()
				},
				{
					TagBase.TagType.TagDefineAnimationMasks2,
					new TagDefineAnimationMasks2()
				},
				{
					TagBase.TagType.TagDefineAnimationFrames2,
					new TagDefineAnimationFrames2()
				},
				{
					TagBase.TagType.TagEnd,
					new TagDefineEnd()
				}
			};
		}
	}
}
