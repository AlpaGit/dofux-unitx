using System;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000042 RID: 66
	[Flags]
	public enum ObjectBehaviourType
	{
		// Token: 0x040000B7 RID: 183
		Simple = 0,
		// Token: 0x040000B8 RID: 184
		Mask = 1,
		// Token: 0x040000B9 RID: 185
		Masked = 2,
		// Token: 0x040000BA RID: 186
		Filtered = 4,
		// Token: 0x040000BB RID: 187
		Colored = 8,
		// Token: 0x040000BC RID: 188
		Complex = 6
	}
}
