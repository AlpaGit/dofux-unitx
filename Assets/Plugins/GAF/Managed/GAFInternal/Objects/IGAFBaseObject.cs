using GAF.Managed.GAFInternal.Data;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000043 RID: 67
	public interface IGAFBaseObject
	{
		/// <summary>
		/// Reset object state and create non-serialized data.
		/// </summary>
		// Token: 0x06000169 RID: 361
		void reload();

		/// <summary>
		/// Update object to desired state.
		/// </summary>
		/// <param name="_State">State data.</param>
		/// <param name="_Refresh">Force refresh state.</param>
		// Token: 0x0600016A RID: 362
		void updateToState(GAFObjectStateData _State, bool _Refresh);

		/// <summary>
		/// Clean non-serialized data.
		/// </summary>
		// Token: 0x0600016B RID: 363
		void cleanUp();

		/// <summary>
		/// Get the object identifier.
		/// </summary>
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600016C RID: 364
		uint objectID { get; }

		/// <summary>
		/// Get the name.
		/// </summary>
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600016D RID: 365
		string name { get; }

		/// <summary>
		/// Get the GAF type of object.
		/// </summary>
		/// <value>The type.</value>
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600016E RID: 366
		GAFObjectType type { get; }
	}
}
