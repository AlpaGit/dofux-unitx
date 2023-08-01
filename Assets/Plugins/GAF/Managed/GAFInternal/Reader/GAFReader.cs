using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GAF.Managed.GAFInternal.Data;
using GAF.Managed.GAFInternal.Utils;
using Pathfinding.Ionic.Zlib;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x0200002B RID: 43
	public class GAFReader
	{
		// Token: 0x060000ED RID: 237 RVA: 0x000056A4 File Offset: 0x000038A4
		public void Load(byte[] _AssetData, ref GAFAnimationData _SharedData, ref GAFHeader _test)
		{
			var gafheader = new GAFHeader();
			var memoryStream = new MemoryStream(_AssetData);
			using (var binaryReader = new BinaryReader(memoryStream))
			{
				if (binaryReader.BaseStream.Length > (long)GAFHeader.headerDataOffset)
				{
					gafheader.Read(binaryReader);
					_test = gafheader;
					if (gafheader.isValid)
					{
						_SharedData = new GAFAnimationData();
						_SharedData.majorVersion = gafheader.majorVersion;
						_SharedData.minorVersion = gafheader.minorVersion;
						var compression = gafheader.compression;
						if (compression != GAFHeader.CompressionType.CompressedZip)
						{
							if (compression == GAFHeader.CompressionType.CompressedNone)
							{
								this.Read(binaryReader, ref _SharedData);
							}
						}
						else
						{
							using (var zlibStream = new ZlibStream(memoryStream, CompressionMode.Decompress))
							{
								var array = new byte[gafheader.fileLength];
								zlibStream.Read(array, 0, array.Length);
								using (var binaryReader2 = new BinaryReader(new MemoryStream(array)))
								{
									this.Read(binaryReader2, ref _SharedData);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000057C0 File Offset: 0x000039C0
		public static Vector2 ReadVector2(BinaryReader _Reader)
		{
			var result = default(Vector2);
			result.x = _Reader.ReadSingle();
			result.y = _Reader.ReadSingle();
			return result;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000057F0 File Offset: 0x000039F0
		public static Rect ReadRect(BinaryReader _Reader)
		{
			var result = default(Rect);
			result.x = _Reader.ReadSingle();
			result.y = _Reader.ReadSingle();
			result.width = _Reader.ReadSingle();
			result.height = _Reader.ReadSingle();
			return result;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000583C File Offset: 0x00003A3C
		public static string ReadString(BinaryReader _Reader)
		{
			var count = _Reader.ReadUInt16();
			var array = _Reader.ReadBytes((int)count);
			return new UTF8Encoding().GetString(array, 0, array.Length);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005868 File Offset: 0x00003A68
		public static TagRecord OpenTag(BinaryReader _Reader)
		{
			var tagRecord = new TagRecord();
			var tagType = (TagBase.TagType)_Reader.ReadUInt16();
			if (!Enum.IsDefined(typeof(TagBase.TagType), tagType))
			{
				tagType = TagBase.TagType.TagInvalid;
			}
			tagRecord.type = tagType;
			tagRecord.tagSize = (long)((ulong)_Reader.ReadUInt32());
			tagRecord.expectedStreamPosition = _Reader.BaseStream.Position + tagRecord.tagSize;
			return tagRecord;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000058C8 File Offset: 0x00003AC8
		public static void CheckTag(TagRecord _Record, BinaryReader _Reader)
		{
			if (_Reader.BaseStream.Position != _Record.expectedStreamPosition)
			{
				GAFUtils.Error(string.Concat(new object[]
				{
					"GAFReader::CloseTag - Tag ",
					_Record.type.ToString(),
					" hasn't been correctly read, tag length is not respected. Expected ",
					_Record.expectedStreamPosition,
					" but actually ",
					_Reader.BaseStream.Position,
					" !"
				}));
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005950 File Offset: 0x00003B50
		public static void CloseTag(TagRecord _Record, BinaryReader _Reader)
		{
			_Reader.BaseStream.Position = _Record.expectedStreamPosition;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005964 File Offset: 0x00003B64
		private void Read(BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData)
		{
			var tagsDictionary = GAFReader.GetTagsDictionary((uint)_SharedData.majorVersion);
			if (_SharedData.majorVersion >= 4)
			{
				var num = _GAFFileReader.ReadUInt32();
				for (var num2 = 0U; num2 < num; num2 += 1U)
				{
					_SharedData.scales.Add(_GAFFileReader.ReadSingle());
				}
				var num3 = _GAFFileReader.ReadUInt32();
				for (var num4 = 0U; num4 < num3; num4 += 1U)
				{
					_SharedData.csfs.Add(_GAFFileReader.ReadSingle());
				}
			}
			else
			{
				var num5 = 0U;
				var linkageName = "rootTimeline";
				var framesCount = (uint)_GAFFileReader.ReadUInt16();
				var frameSize = GAFReader.ReadRect(_GAFFileReader);
				var pivot = GAFReader.ReadVector2(_GAFFileReader);
				_SharedData.timelines.Add((int)num5, new GAFTimelineData(num5, linkageName, framesCount, frameSize, pivot));
			}
			while (_GAFFileReader.BaseStream.Position < _GAFFileReader.BaseStream.Length)
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
						GAFTimelineData gaftimelineData = null;
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
			if (_SharedData != null)
			{
				foreach (var gaftimelineData2 in _SharedData.timelines.Values)
				{
					gaftimelineData2.sequences.Add(new GAFSequenceData("Default", 1U, gaftimelineData2.framesCount));
				}
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005BB0 File Offset: 0x00003DB0
		private static Dictionary<TagBase.TagType, TagBase> GetTagsDictionary(uint _MajorVersion)
		{
			return new Dictionary<TagBase.TagType, TagBase>
			{
				{
					TagBase.TagType.TagDefineAtlas,
					new TagDefineAtlas()
				},
				{
					TagBase.TagType.TagDefineAnimationMasks,
					new TagDefineAnimationMasks()
				},
				{
					TagBase.TagType.TagDefineAnimationObjects,
					new TagDefineAnimationObjects()
				},
				{
					TagBase.TagType.TagDefineAnimationFrames,
					new TagDefineAnimationFrames()
				},
				{
					TagBase.TagType.TagDefineSounds,
					new TagDefineSounds()
				},
				{
					TagBase.TagType.TagDefineAtlas3,
					new TagDefineAtlas3()
				},
				{
					TagBase.TagType.TagDefineNamedParts,
					new TagDefineNamedParts()
				},
				{
					TagBase.TagType.TagDefineSequences,
					new TagDefineSequences()
				},
				{
					TagBase.TagType.TagDefineStage,
					new TagDefineStage()
				},
				{
					TagBase.TagType.TagDefineTimeline,
					new TagDefineTimeline()
				},
				{
					TagBase.TagType.TagEnd,
					new TagDefineEnd()
				}
			};
		}

		// Token: 0x0400008B RID: 139
		private const uint TimelinesBaseVersion = 4U;
	}
}
