using System.IO;
using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x0200003A RID: 58
	internal class TagDefineSounds : TagBase
	{
		// Token: 0x0600011C RID: 284 RVA: 0x00006F64 File Offset: 0x00005164
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline)
		{
			var num = (uint)_GAFFileReader.ReadUInt16();
			for (var num2 = 0U; num2 < num; num2 += 1U)
			{
				var id = (uint)_GAFFileReader.ReadUInt16();
				var linkage = GAFReader.ReadString(_GAFFileReader);
				var fileName = GAFReader.ReadString(_GAFFileReader);
				_GAFFileReader.ReadSByte();
				_GAFFileReader.ReadSByte();
				_GAFFileReader.ReadSByte();
				_GAFFileReader.ReadBoolean();
				_GAFFileReader.ReadUInt32();
				_SharedData.audioClips.Add(new GAFSoundData(id, fileName, linkage));
			}
		}
	}
}
