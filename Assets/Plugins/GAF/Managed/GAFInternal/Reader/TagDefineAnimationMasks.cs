using System.IO;
using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x02000031 RID: 49
	public class TagDefineAnimationMasks : TagBase
	{
		// Token: 0x0600010A RID: 266 RVA: 0x00006A94 File Offset: 0x00004C94
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline)
		{
			var num = _GAFFileReader.ReadUInt32();
			for (var num2 = 0U; num2 < num; num2 += 1U)
			{
				var id = _GAFFileReader.ReadUInt32();
				var atlasElementID = _GAFFileReader.ReadUInt32();
				_SharedData.rootTimeline.masks.Add(new GAFObjectData(id, atlasElementID, GAFObjectType.Texture));
			}
		}
	}
}
