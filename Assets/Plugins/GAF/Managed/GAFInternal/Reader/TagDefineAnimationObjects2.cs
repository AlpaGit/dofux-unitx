using System.IO;
using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x02000034 RID: 52
	public class TagDefineAnimationObjects2 : TagBase
	{
		// Token: 0x06000110 RID: 272 RVA: 0x00006B70 File Offset: 0x00004D70
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline)
		{
			var num = _GAFFileReader.ReadUInt32();
			for (var num2 = 0U; num2 < num; num2 += 1U)
			{
				var id = _GAFFileReader.ReadUInt32();
				var atlasElementID = _GAFFileReader.ReadUInt32();
				var type = (GAFObjectType)_GAFFileReader.ReadUInt16();
				_CurrentTimeline.objects.Add(new GAFObjectData(id, atlasElementID, type));
			}
		}
	}
}
