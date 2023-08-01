using System.IO;
using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x0200002D RID: 45
	public abstract class TagBase
	{
		// Token: 0x060000F9 RID: 249
		public abstract void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline);

		// Token: 0x0200002E RID: 46
		public enum TagType
		{
			// Token: 0x0400008D RID: 141
			TagInvalid = -1,
			// Token: 0x0400008E RID: 142
			TagEnd,
			// Token: 0x0400008F RID: 143
			TagDefineAtlas,
			// Token: 0x04000090 RID: 144
			TagDefineAnimationMasks,
			// Token: 0x04000091 RID: 145
			TagDefineAnimationObjects,
			// Token: 0x04000092 RID: 146
			TagDefineAnimationFrames,
			// Token: 0x04000093 RID: 147
			TagDefineNamedParts,
			// Token: 0x04000094 RID: 148
			TagDefineSequences,
			// Token: 0x04000095 RID: 149
			TagDefineTextFields,
			// Token: 0x04000096 RID: 150
			TagDefineAtlas2,
			// Token: 0x04000097 RID: 151
			TagDefineStage,
			// Token: 0x04000098 RID: 152
			TagDefineAnimationObjects2,
			// Token: 0x04000099 RID: 153
			TagDefineAnimationMasks2,
			// Token: 0x0400009A RID: 154
			TagDefineAnimationFrames2,
			// Token: 0x0400009B RID: 155
			TagDefineTimeline,
			// Token: 0x0400009C RID: 156
			TagDefineSounds,
			// Token: 0x0400009D RID: 157
			TagDefineAtlas3
		}
	}
}
