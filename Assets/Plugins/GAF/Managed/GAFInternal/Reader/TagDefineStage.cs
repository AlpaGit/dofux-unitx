using System.IO;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x0200003B RID: 59
	public class TagDefineStage : TagBase
	{
		// Token: 0x0600011E RID: 286 RVA: 0x00006FD4 File Offset: 0x000051D4
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline)
		{
			_SharedData.fps = (ushort)_GAFFileReader.ReadByte();
			var b = _GAFFileReader.ReadByte();
			var b2 = _GAFFileReader.ReadByte();
			var b3 = _GAFFileReader.ReadByte();
			var b4 = _GAFFileReader.ReadByte();
			_SharedData.color = new Color((float)b2 / 255f, (float)b3 / 255f, (float)b4 / 255f, (float)b / 255f);
			_SharedData.width = _GAFFileReader.ReadUInt16();
			_SharedData.height = _GAFFileReader.ReadUInt16();
		}
	}
}
