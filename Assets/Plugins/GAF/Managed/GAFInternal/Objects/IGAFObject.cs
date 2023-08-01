using System;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000044 RID: 68
	public interface IGAFObject : IGAFBaseObject, IEquatable<IGAFObject>
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600016F RID: 367
		IGAFObjectImpl impl { get; }
	}
}
