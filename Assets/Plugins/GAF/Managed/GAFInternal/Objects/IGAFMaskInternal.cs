using System.Collections.Generic;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x0200004E RID: 78
	internal interface IGAFMaskInternal
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001C6 RID: 454
		int stencilID { get; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001C7 RID: 455
		Dictionary<string, Material> materials { get; }

		// Token: 0x060001C8 RID: 456
		void update();

		// Token: 0x060001C9 RID: 457
		void registerMaskedObject(IGAFMaskedInternal _Masked);

		// Token: 0x060001CA RID: 458
		void unregisterMaskedObject(IGAFMaskedInternal _Masked);
	}
}
