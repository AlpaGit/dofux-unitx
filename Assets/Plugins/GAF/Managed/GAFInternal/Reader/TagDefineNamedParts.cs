using System.IO;
using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x02000038 RID: 56
	public class TagDefineNamedParts : TagBase
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00006EA0 File Offset: 0x000050A0
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline)
		{
			var num = _GAFFileReader.ReadUInt32();
			for (var num2 = 0U; num2 < num; num2 += 1U)
			{
				var objectID = _GAFFileReader.ReadUInt32();
				var name = GAFReader.ReadString(_GAFFileReader);
				var item = new GAFNamedPartData(objectID, name);
				if (_CurrentTimeline == null)
				{
					_SharedData.rootTimeline.namedParts.Add(item);
				}
				else
				{
					_CurrentTimeline.namedParts.Add(item);
				}
			}
		}
	}
}
