using System.IO;
using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x02000033 RID: 51
	public class TagDefineAnimationObjects : TagBase
	{
		// Token: 0x0600010E RID: 270 RVA: 0x00006B28 File Offset: 0x00004D28
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline)
		{
			var num = _GAFFileReader.ReadUInt32();
			for (var num2 = 0U; num2 < num; num2 += 1U)
			{
				var id = _GAFFileReader.ReadUInt32();
				var atlasElementID = _GAFFileReader.ReadUInt32();
				_SharedData.rootTimeline.objects.Add(new GAFObjectData(id, atlasElementID, GAFObjectType.Texture));
			}
		}
	}
}
