using System.IO;
using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x02000039 RID: 57
	public class TagDefineSequences : TagBase
	{
		// Token: 0x0600011A RID: 282 RVA: 0x00006EFC File Offset: 0x000050FC
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline)
		{
			var num = _GAFFileReader.ReadUInt32();
			for (var num2 = 0U; num2 < num; num2 += 1U)
			{
				var name = GAFReader.ReadString(_GAFFileReader);
				var startFrame = _GAFFileReader.ReadUInt16();
				var endFrame = _GAFFileReader.ReadUInt16();
				var item = new GAFSequenceData(name, (uint)startFrame, (uint)endFrame);
				if (_CurrentTimeline == null)
				{
					_SharedData.rootTimeline.sequences.Add(item);
				}
				else
				{
					_CurrentTimeline.sequences.Add(item);
				}
			}
		}
	}
}
